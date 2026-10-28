# ILSpy 还原路线

这个分支放的是同一份程序集用 **ILSpy 11 + `-lv CSharp2`** 反编译、并完整语义化的结果。

```
dnspy 分支       dnSpy（dnlib）反编译 → 手工语义化 1026 个成员 → 修 16 类问题
ilspy 分支       ILSpy -lv CSharp2 反编译 → 修 38 处方法组 → 语义化全部标识符 → 0 编译错误
```

## 目录

```
src-ilspy/          ILSpy 产物，已完整语义化
src-ilspy-build/    构建工程，msbuild 直接出 exe
```

## 为什么 ILSpy 需要先剥一层

直接喂 `Yukihota_Slayed.exe` 会抛：

```
System.BadImageFormatException: Invalid coded index.
   at CustomAttributeTableReader.GetConstructor
   at SRMExtensions.GetNullableContext
```

XenoCode 在元数据里写了一条 constructor coded index = 0 的 `CustomAttribute`。
ILSpy 走 `System.Reflection.Metadata`，读到那里就崩。dnSpy 走 dnlib，容错。

能读的是从壳里剥出来的纯托管模块（`work/v11_raw/Yukihota.exe`，139,264 字节），
用它反编译出 35 个文件、10,184 行。

## 命令

```bash
ilspycmd -lv CSharp2 -p -o src-ilspy <剥离后的模块>
```

`-lv CSharp2` 一步关掉了所有现代语法。产物里 `=>`、`?.`、`var`、
`using var`、元组、插值字符串的出现次数都是 0。

## 编译前的两处必要修补

**一，38 处方法组转委托**

```
CS0654: Method 'n0001.n0002.n0027(object, System.EventArgs)' referenced without parentheses
```

csc 1.1 不认方法组，按参数类型补 `new EventHandler(...)`，37 处可以脚本批处理，
剩 1 处是自定义委托 `FrameCallback`。

**二，资源逻辑名跟随类名**

`ResourceManager(typeof(GalGameForm))` 按类名找 `.resources`。类名换成语义名之后，
`LogicalName` 必须同步：

```xml
<LogicalName>Yukihota.OptionsForm.resources</LogicalName>
<LogicalName>Yukihota.GalGameForm.resources</LogicalName>
```

## 语义化的顺序与方法

原始产物里全部是 `n0001` / `n0063` / `n0861` 这种编号，共 7,766 处引用。
按下面的顺序逐层清除，每一层都用 dnSpy 版作对照，并在每步之后编译验证。

| 层 | 数量 | 配对依据 |
| --- | --- | --- |
| 命名空间 | 2 | `n0001` → `Yukihota`，`n0062` → `Yukihota.Audio` |
| 类 | 22 | 字段类型集合 + 方法签名组成的指纹 |
| 枚举与委托 | 9 | 成员数量 + 引用它的字段位置 |
| 方法 | 54 + 68 + 65 + 73 | 返回类型 + 参数类型；签名相同的按声明顺序 |
| 常量 | 92 | 声明顺序 + 数值 |
| 字段 | 403 | 声明顺序 + 类型 |
| 参数与局部 | 其余 | 逐个方法对照 |

几条经验：

**签名相同的按声明顺序配。** `ScreenRenderer` 有 99 个方法，大半是 `void` 无参，
签名完全无法区分，只能靠元数据里的声明顺序，用返回类型逐个校验。

**跨类的必须全局替换，类内的必须限定文件。** 公开方法被别的类调用，
只改定义处会断 100 多处引用；而 `n` 编号不是全局 RID，
同一个 `n0067` 在 `GalGameForm` 是字段、在 `StaffRoll` 是局部变量。

**编号会撞。** `n0399` 同时表示 `SpriteState.isAnimated`、`DrawItem.isAnimated`、
`InputState.wait`、`AnimItem.isAnimated`、`TextureItem.isAnimated`，
以及 `ScreenRenderer` 四个方法里的一个 bool 参数。按强制转换前缀和所在方法分别处理。

**枚举成员会被字段替换误伤。** `ScenarioResult` 里一度出现 `waitClick`、`charaNames`、
`finish` 这些名字——它们是同号的字段名。发现后按 dnSpy 版整体重写了七个枚举。

**全局替换会压掉同名局部变量。** `n880` 在 `ScreenRenderer` 里是八个不同的局部量
（一个 `Font`、六个 `Rectangle`、一个 `Point`），一次全局替换把它们全变成 `optionsForm`。
按方法改回 `textFont`、`indicatorRect`、`wipePosition`、`zoomRect`、`sunRect`、
`petalRect`、`snowRect`、`modeIconRect`。

## 和 dnSpy 路线对照

| | dnSpy | ILSpy `-lv CSharp2` |
|---|---|---|
| 读原始程序集 | 能（容错坏 CustomAttribute） | 不能，要先剥壳 |
| 裸产物喂 csc 1.1 | 388 条错误 | 38 条（同一类，可脚本修） |
| 运行时硬伤 | 2 个，含 `TypeInitializationException` | 0 |
| goto | 保留 75 处 | 重构为 12 处 |
| `string.Concat` | 27 处保留 | 0（转成 `+`） |
| 静态字段双赋值 | 压成自引用 → 运行时崩 | 降级为局部变量 → 能跑但失真 |
| 语义化 | 1026 个成员映射 | 7,766 处引用全部清除 |

## 现状

```
10,184 行   n 名引用 0   csc 1.1 编译 0 错误   能启动到标题画面
```

启动时会先弹选项窗体——`showOptionsOnStartup` 默认 `true`，原版如此，不是缺陷。
