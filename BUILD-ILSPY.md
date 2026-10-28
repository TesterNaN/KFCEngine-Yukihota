# ILSpy 还原路线

这个分支放的是同一份程序集用 **ILSpy 11 + `-lv CSharp2`** 反编译的结果，
用来和 `main` 分支的 dnSpy 路线对照。

```
dnspy 分支       dnSpy（dnlib）反编译 → 手工语义化 1026 个成员 → 修 16 类问题
ilspy 分支       ILSpy -lv CSharp2 反编译 → 修 38 处方法组 → 能编译能运行
```

## 目录

```
src-ilspy/          ILSpy 原始产物，未语义化（n0001 / n0063 这种名字）
src-ilspy-build/    补好资源与构建脚本的工程，可直接 msbuild
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

能读的是从壳里剥出来的纯托管模块（`work/v11_raw/Yukihota.exe`，139,264 字节）。
用它反编译，35 个文件、10,184 行。

## 命令

```bash
ilspycmd -lv CSharp2 -p -o src-ilspy <剥离后的模块>
```

`-lv CSharp2` 一步关掉了所有现代语法。产物里 `=>`、`?.`、`var`、
`using var`、元组、插值字符串的出现次数都是 0。

## 还差什么才能编译

裸产物直接喂 csc 1.1 有 **38 条错误**，全是同一类：

```
CS0654: Method 'n0001.n0002.n0027(object, System.EventArgs)' referenced without parentheses
```

ILSpy 生成了方法组转委托（C# 2.0 语法），csc 1.1 不认。按方法签名的参数类型
补上 `new EventHandler(...)`，37 处自动修，剩 1 处是自定义委托 `n0248`。

补完之后 csc 1.1 编译 **0 错误**。

## 还有两处资源要改名

`ResourceManager(typeof(n0063))` 会去找 `n0001.n0063.resources`，
而仓库里的资源叫 `Yukihota.GalGameForm.resources`。因为类名被 ILSpy 编成了 `n0063`，
资源逻辑名要跟着改：

```xml
<EmbeddedResource Include="resources\res1.bin">
  <LogicalName>n0001.n0002.resources</LogicalName>
</EmbeddedResource>
<EmbeddedResource Include="resources\res2.bin">
  <LogicalName>n0001.n0063.resources</LogicalName>
</EmbeddedResource>
```

## 跑起来

`src-ilspy-build/bin/Release/Yukihota.exe`（180,224 字节），配上 `game-data/` 里的数据，
能到 KID Fans Club 启动画面。

启动时会先弹选项窗体——`showOptionsOnStartup` 默认 `true`，原版就是这样，
不是缺陷。

## 和 dnSpy 路线对照

| | dnSpy | ILSpy `-lv CSharp2` |
|---|---|---|
| 读原始程序集 | 能（容错坏 CustomAttribute） | 不能（SRM 崩），要先剥壳 |
| 裸产物喂 csc 1.1 | 388 条错误 | 38 条（同一类，可脚本修） |
| 运行时硬伤 | 2 个，含 `TypeInitializationException` | 0 |
| goto | 保留 75 处 | 重构为 12 处 |
| `string.Concat` | 27 处保留 | 0（转成 `+`） |
| 静态字段双赋值 | 压成自引用 → 运行时崩 | 降级为局部变量 → 能跑但失真 |
| 资源逻辑名 | 类名 `GalGameForm` 天然对上 | 需手工改 |
| 语义化 | 已完成 1026 个成员 | 未做 |

## 结论

**形状忠实**：dnSpy。goto、`string.Concat`、字段结构都保留着 IL 的原样。

**行为可靠**：ILSpy。它有偏离，但偏离是能看出来的；dnSpy 那个自引用不会自己暴露，
只有玩家点到 New Game 才炸。

**拿哪个当基础**：ILSpy。388 → 38 的人工量差 10 倍，且没有隐藏的运行时坑。
dnSpy 那 388 条编译错误都在明面上，唯独最要命的第 15 条不报错。

拿 dnSpy 版当交叉校验，专盯静态字段、闭包、goto 密集的方法——这些是 ILSpy
容易改写过头的地方。
