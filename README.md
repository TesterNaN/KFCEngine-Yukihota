# KFCEngine — Memories Off 2nd ～雪萤～ 逆向还原

> 一套 2005 年的 .NET 视觉小说引擎，网上已近乎绝迹；本仓库是它目前已知的唯一完整源码档案。

《秋之回忆 2nd ～雪萤～》的 PC 版由 **KID Fans Club（KFC）** 制作（程序集署名 `Mullen@KFC`，
Description `MO2PURE`，Copyright 2005），是一个 **.NET Framework 1.1 + WinForms + Managed DirectX 9**
应用，被 **XenoCode PostBuild** 保护过。

这套程序是 KFC 从 PS2 自主移植到 PC 时的原创实现（见[出处与历史](#出处与历史)），
而它在网上几乎没有存世资料：没有公开源码、没有文档、没有作者留下的技术记录，连名字都没有。
本仓库把它从保护壳里还原出来，是它目前已知的唯一完整源码档案。

> **关于名字**：原始程序集与 2006 年的发布帖都没有给这套实现命名（命名空间全被混淆成单字符）。
> `KFCEngine` 是本项目为它约定的名字，用于检索与叙述，**不是官方名称**。

> **同一套引擎的第二代**：2007 年的《秋之回忆：从今以后 AGAIN》用的也是这套引擎的重写版（V2A），
> 其源码已经单独还原 → **[KFCEngine-Again-TTIME](https://github.com/TesterNaN/KFCEngine-Again-TTIME)**
> 之所以还是要命名，是因为「某部游戏的源码」不足以指代一套完整引擎——而它已经快没有别的痕迹留下了。

本仓库把它还原成**可读、可编译、可运行**的 C# 源码工程，并附上完整还原过程与踩坑记录。

- 反编译产物：`src/` 32 个文件、10983 行，标识符全部语义化，无 `nXXXX` 占位符
- 编译产物：用原始 .NET 1.1 工具链（`csc.exe` 7.10）可零错误重建
- 运行验证：能过免责声明 → 设置窗 → 标题 → 开场动画 → 正片（立绘/对话框/字幕/右键菜单）

---

## 仓库结构

```
.
├─ README.md
├─ NOTICE.md                     版权与使用声明
├─ Yukihota.sln / Yukihota.csproj
├─ src/                          32 个源文件，文件名 = 类型名
│   ├─ GalGameForm.cs            主窗体 + 游戏循环（原 Ӓ.Ӕ，~2400 行）
│   ├─ OptionsForm.cs            设置窗体（原 Ӓ.ӓ）
│   ├─ ScreenRenderer.cs         Direct3D 渲染器：Sprite 队列 / 文字 / 特效（原 Ӓ.ۂ）
│   ├─ ScenarioScript.cs         剧本解析（Script/*.xml，三语种）
│   ├─ SceneDirector.cs          场景指令分发
│   ├─ ArchiveFile.cs            .dat 归档读写（含 3DES）
│   ├─ ArchiveIndex.cs           归档索引
│   ├─ AudioManager.cs           BGM/VOICE/SE
│   ├─ MediaPackage.cs           MCI（winmm）媒体包
│   ├─ StaffRoll.cs              ED 演职表
│   ├─ StaticData.cs             编译器 <PrivateImplementationDetails>（27 个数据块）
│   └─ …                         命令/枚举/数据结构
├─ Properties/AssemblyInfo.cs
├─ resources/
│   ├─ res1.bin                 原始 .NET 1.1 格式资源（OptionsForm）
│   └─ res2.bin                 同上（GalGameForm）
├─ OptionsForm.resx             供 VS 设计器查看（不参与编译）
├─ GalGameForm.resx
├─ Yukihota.ico
├─ lib/                         Microsoft.DirectX.* 1.0.2902 托管包装
├─ game-data/                   游戏资源（只放数据：归档 / 动画 / 说明书 / 运行库）
│   ├─ Bg.dat Bgm.dat Chara.dat Script.dat Se.dat System.dat Voice.dat
│   ├─ save_sys.dat             初始配置（游戏运行时会回写）
│   ├─ Movie/KFC.wmv            开场动画（InitOpeningMovie 里以 "Movie\\KFC.wmv" 打开）
│   ├─ Manual/                  说明书 HTML + 图片（手册按钮指向 Manual\\Index_{sc,tc}.html）
│   ├─ Readme_sc.txt            原始发布说明（简）
│   ├─ Readme_tc.txt            原始发布说明（繁）
│   ├─ Microsoft.DirectX*.dll   Managed DirectX 9 托管包装
│   └─ d3dx9_26.dll             Direct3D 9 扩展运行库
```

构建产物不落在 `game-data/`：`msbuild` 会输出到 `bin\$(Configuration)\`，
并自动把 `game-data/` 下的全部文件（含 `Movie\`、`Manual\` 子目录）复制到 exe 同目录，
所以 `bin\Release\` 是一个可以直接双击运行的完整目录。
`bin/` 与 `obj/` 都在 `.gitignore` 里。

## 快速开始

构建（命令行，无需任何脚本）：

```powershell
msbuild Yukihota.csproj                 # 输出 bin\Release\Yukihota.exe（并复制游戏数据）
msbuild Yukihota.csproj /t:Rebuild      # 清理后重建
msbuild Yukihota.csproj /p:Configuration=Debug
```

构建逻辑全部在 `Yukihota.csproj` 里：自定义 target 直接调用
`%WINDIR%\Microsoft.NET\Framework\v1.1.4322\csc.exe`，用 `WriteLinesToFile`
生成响应文件传参数，再用一个内联任务（`CodeTaskFactory`）补上 csc 1.1 没有的
`/platform:x86`——也就是把 CLI header 的 32BITREQUIRED 置位。

> 为什么不用 `Microsoft.CSharp.targets`：VS2005 之后的 MSBuild 只认 v2.0+ 目标框架，
> 也不会去调用 1.1 的 csc。要产出 `v1.1.4322` 元数据的程序集，只能用 1.1 的编译器。
> 如果本机没有 v1.1，可以指定：`msbuild Yukihota.csproj /p:DotNet11Dir=D:\path\to\v1.1.4322`

运行：双击 `bin\Release\Yukihota.exe`。

前提：系统装有 .NET Framework 1.1 与 DirectX 9 运行时（`d3dx9_26.dll` 已随仓库附带）。

启动参数（原程序自带）：

- `/nocopyright` 跳过免责声明
- `/noop` no-op 模式

---

## 这个程序是什么

| 项目 | 结果 |
| --- | --- |
| 容器 | PE32, x86, IL-Only, CLI flags = ILONLY \| 32BITREQUIRED |
| 元数据版本 | `v1.1.4322`（.NET Framework 1.1） |
| 引擎标识 | 原始程序集里**没有**引擎名（命名空间被混淆成单字符）；本项目暂称 KFCEngine |
| 程序集 | `Yukihota 1.0.2199.33640`，Company `Mullen@KFC`，Description `MO2PURE` |
| 保护 | XenoCode PostBuild（`ProcessedByXenoCode` / `SuppressDisassembly`；标识符压成单字符） |
| 代码规模 | TypeDef 64，MethodDef 8192（真实 329，其余 7863 是空壳），Field 639，FieldRVA 27 |
| 资源 | `Ӓ.ӓ.resources`（设置窗）、`Ӓ.Ӕ.resources`（主窗） |

### 三个关键结论

1. **IL 没有被加密**。业务代码完整躺在 64 个类型里，XenoCode 只做了「标识符改名 + 方法壳搬运 + 元数据破坏」。
2. 8192 个 MethodDef 里有 7863 个是保护器留下的空方法壳（堆在一个匿名类里，RVA=0、零引用），已按判定标准删除。
3. 元数据里有一条故意写坏的 `CustomAttribute`（constructor coded index = 0），会让 ILSpy / 新版 dnSpy 直接抛 `BadImageFormatException`；dnSpy.Console（dnlib）能容错，所以用命令行导出。

## 修过的问题

反编译输出按现代 C# 生成，直接喂给 1.1 编译器有 388 条错误；另有 2 个只有运行时才暴露的坑。

| # | 问题 | 位置 | 修法 |
| --- | --- | --- | --- |
| 1 | 非 ASCII 标识符含 1.1 不接受的码位（`\u082C`、`\u085C`） | 全工程 | 933 个符号 ASCII 化 |
| 2 | `global::System.Drawing.Font`（C# 2.0） | 5 个文件 | 去掉 `global::` |
| 3 | `default(int)` 字面量（C# 2.0） | 2 个文件 | 换成 `0`/`false`/`null` |
| 4 | `string.get_Chars(i)` 显式调访问器 | 12 处 | 改 `s[i]` |
| 5 | IL 把 int 直传枚举参数 | 4 处 | 加 `(FileMode)`/`(FileAccess)`/`(CryptoStreamMode)` |
| 6 | 方法组转委托（C# 2.0） | 8 处 | 补 `new KeyEventHandler(...)` |
| 7 | `using (IEnumerator e = ...)` | 1 处 | 改直接声明 |
| 8 | `base..ctor();` 伪代码、不可达 `obj2` | 2 处 | 删除 + 位置注释 |
| 9 | 嵌套 struct private、字段 internal（CS0052） | `StaticData` | struct 提升 internal |
| 10 | 入口点被混淆，编译器找不到 Main | `GalGameForm` | 恢复方法名 `Main` |
| 11 | XenoCode 标记属性类未继承 `Attribute` | 2 类 | 已整体删除 |
| 12 | `AssemblyAlgorithmId(0)` 二义性 | AssemblyInfo | 加 `(uint)` |
| 13 | **资源格式**：ResGen 4.x 产 v2 `.resources`，.NET 1.1 读不了 | 运行时 | 直接嵌入原 exe 的 v1 资源 |
| 14 | **32BITREQUIRED 缺失**：64 位 CLR 加载 32 位 DirectX 失败 | 运行时 | post-build 置 CLI flags |
| 15 | **dnSpy 把「同一静态字段赋值两次」压成自引用** → `TypeInitializationException` | `ScenarioScript.tempDirectory` | 拆成字段初始化器 + 静态构造 |
| 16 | 不可达路径里的 `return 未赋值局部变量` | `ScenarioScript` | 改为 `return ScenarioResult.Error;` |

第 13、14 条不动，程序能编译、能启动，显示完免责声明后立刻以 `0xE0434352` 退出。
第 15 条更隐蔽：编译器不报错，只有点 New Game 真正构造 `ScenarioScript` 时才炸。原始 IL：

```
IL_002C: call      Path::GetTempPath()
IL_0031: stsfld    string 'Ӓ.؎'::'\u0618'     // 先存
IL_0036: ldsfld    ...                        // 读回来
IL_0048: callvirt  String::Substring(int32,int32)
IL_004D: stsfld    string 'Ӓ.؎'::'\u0618'     // 去尾分隔符后存回同一字段
```

同一个字段赋值两次，C# 字段初始化器只能写一次，dnSpy 就压成了 `x = x.Substring(...)`。
还原：`tempDirectory = Path.GetTempPath();` + 静态构造函数里 `tempDirectory = tempDirectory.Substring(0, tempDirectory.Length - 1);`

## 命名还原

反编译出来的代码原本全是 `n0037` / `n0124`。这一步把它们换成语义名：

| 项目 | 数量 |
| --- | --- |
| 成员/参数映射 | 1026 |
| 枚举成员 | 44（8 个枚举） |
| 枚举类型改名 | 2（`FontSizeMode`→`HorizontalAlign`、`AlignMode`→`TextRevealMode`，原名与实际语义不符） |
| 跨类型引用修正 | 529（靠编译器错误逐轮收敛：489→49→23→19→1→0） |
| 最终残留占位符 | **0** |

做法：按文件分工读代码产映射 → 按**声明作用域**替换（只改本文件声明的符号和 `this.X`，`别的对象.X` 留给下一步）→ 枚举用**限定替换**（`FontSizeMode.n0437` 与 `TextureBlend.n0437` 同名不同义）→ 跨类型引用用编译器闭环修正。

改动效果：

```csharp
// 之前
case n0383.n0404:
    if (!this.n0365) this.n0356.n0405(((n0406)this.n0378).n0407);
    this.n0358.n0408((string[])this.n0377, ((n0409)this.n0378).n0410, ...);

// 之后
case ScenarioResult.Message:
    if (!this.skipMode) this.audio.PlayVoice(((ScriptCommand)this.resultOption).voice);
    this.renderer.SetMessage((string[])this.resultData, ((MessageCommand)this.resultOption).align,
                             ((MessageCommand)this.resultOption).showMode, ...);
```

## 语言限制导致的非 1:1 表达（均在代码内注释）

| 原始形态 | 本工程写法 | 原因 |
| --- | --- | --- |
| 嵌套 struct private + 字段 internal | struct 提升 internal | CS0052 可见性规则 |
| 静态字段的 RVA 初值 | 静态构造 + `__makeref` 指针写入 | C# 不支持字段 RVA 初始化器 |
| 一处不可达 `ldloc/ret` | 删代码 + 位置注释 | 无名局部变量无法生成合法 C# |
| 入口方法名被混淆 | 恢复为 `Main` | C# 强制入口方法名 |
| 窗体用 resx 编译资源 | 直接嵌入原 v1 `.resources` | ResGen 4.x 产物 1.1 读不了 |
| C# 1.1 无 `partial` | 设计器与业务代码同文件 | 语言版本限制 |
| `<PrivateImplementationDetails>` | `StaticData` + 静态构造 | 类型名含 `<>`，C# 无法表达 |

## 验证与已知差异

已实测：免责声明 → 设置窗（LOGO/手柄/显示模式/三语言）→ 标题 → New Game → KID LOGO 动画 → 正片（立绘 + 背景 + 对话框 + 角色名牌 + 逐字文本）→ 右键菜单（取消/选项 F8/显示模式 Alt+Enter/快进 F3/自动 F4/隐藏对话 F2/语言/字体/返回主菜单/结束游戏），配置回写正常。

静态体检：`csc /warn:4` 全开只有 10 条 CS0162（9 条是 `try/catch` 展开后的 `goto` 残留，无害；1 条已修）。无恒真比较、无自身赋值、无 `goto` 缺失、无 TODO、无空实现。

已知差异：

- 标题菜单第三项显示 `???`：该条资源字符串在 `黑体 24pt` 下渲染不出（或原版亦如此），属数据问题。
- 少量成员名在不同类型间共用同一混淆字符，ASCII 化后按首个语义场景取名（不影响编译运行）。
- 存档/读档、相册、ED 这些路径未做自动化验证（需要真实交互），代码本身无异常。

## 版权与免责

见 [NOTICE.md](NOTICE.md)。本仓库仅用于**失传媒体存档与技术研究**，不提供任何商业用途。

## 同一条引擎线上的其他作品

这套引擎后来又被用了两次，都已经完成还原：

| | 作品 | 仓库 | 状态 |
|---|---|---|---|
| v1 | 《雪萤》 | 本仓库 | ✓ 可运行到正片 |
| v2 | 《AGAIN》 | [KFCEngine-Again-TTIME](https://github.com/TesterNaN/KFCEngine-Again-TTIME) | ✓ 可运行到序幕 |
| v3 | 《思念的碎片 -Close to-》 | [KFCEngine-CloseTo](https://github.com/TesterNaN/KFCEngine-CloseTo) | ✓ 可运行到开场 |

拿 AGAIN 那一代和本仓库对比，能清楚看到引擎一年的演进：命名空间从一个 `Ӓ` 拆成
八个分层模块，运行时从 .NET 1.1 升到 2.0，视频从 winmm MCI 换成 DirectShow，
保护也从 XenoCode 换成了 C++ loader + StarForce + 3DES 加密程序集。

