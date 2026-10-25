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

## 关于 KFCEngine

`KFCEngine` 是本项目的命名，不是原作者的命名——原程序集与发布帖里都没有引擎标识。
但这里 32 个源文件构成的确实是一套完整的视觉小说引擎：场景调度、Direct3D 渲染与字体排版、
自定义归档与加密、三语种剧本解析、MCI 音频与视频、手柄输入。

从程序集元数据与代码里能确认的技术特征：

从程序集元数据与代码里能确认的技术特征：

| 项目 | 事实 | 出处 |
| --- | --- | --- |
| 程序集标识 | `Yukihota 1.0.2199.33640`，Company `Mullen@KFC`，Description `MO2PURE`，Copyright `2005` | AssemblyInfo |
| 运行时 | .NET Framework 1.1（元数据 `v1.1.4322`），WinForms 承载窗口/对话框/上下文菜单 | CLI header、`System.Windows.Forms` 引用 |
| 图形 | Managed DirectX 9：Direct3D 9 渲染（Sprite 批处理 + 自定义字体）、DirectDraw、DirectInput（手柄 + 力反馈） | `Microsoft.DirectX.*` 引用 |
| 场景脚本 | 自定义 XML，简/繁/日三语并存，节点形如 `scene` / `dialogue` / `paragraph` / `click` / `back` | `Script.dat` → `Script/*.xml` |
| 资源打包 | 自定义 `.dat` 归档（文件名 + 偏移 + 长度索引），内容 3DES 加密，密钥派生自作者 ID `Mullen` | `ArchiveFile` / `ArchiveIndex` |
| 媒体播放 | winmm 的 MCI（`mciSendString`），开场动画 `Movie/KFC.wmv` | `MediaPackage` |
| 代码保护 | XenoCode PostBuild（标识符单字符化 + 方法壳搬运 + 元数据破坏） | `ProcessedByXenoCode` 属性 |

### 来历

按 2018 年触乐对冰天火焰的采访：2005 年春，一位《雪萤》的铁粉程序员加入汉化组，
提出「自己写一套引擎把 PS2 版移植到 PC」；素材靠截屏与抠图，先把通关过程录成视频，
再对着视频实现功能与重演演出；做了一年多，2006 年 3 月发布。

**这套引擎有两个产物。** 除了《雪萤》（2006-03），2007 年 9 月上市的《秋之回忆：从今以后～AGAIN》
PC 中文版也是用它移植的——北京娱乐通发行简体版、光谱资讯发行繁体版（2007-09-22），
一次移植两地发行：

> 北京娱乐通宣布，采用**国产游戏引擎**移植的《秋之回忆：从今以后～AGAIN》简体中文版将于 9 月下旬正式发行……
> 本次本地化采用了最新的国产游戏引擎，**从 PS2 版移植到 PC 平台**……
> **本次移植，是由 KIDS Fans Channel 成员，采用最新的微软 .net 技术完成。台湾的繁体中文版也采用了相同的移植版本。**
> —— 新浪游戏，2007-09-10

这也印证了 2018 年触乐那句「以《雪萤》研发出的引擎移植这部新作的计划」：计划确实落地了，
只是成品挂在北京娱乐通 / 光谱的商业发行名下，没有以 KFC 名义单独发布——所以在民间流传的
只有「娱乐通版 / 光谱版」，找不到「KFC 版」。

另外做了个格式对照：拿光谱中文化的《从今以后》**本篇**（C++ 原生，`MEMOSORE.exe` 无 CLI 头）
比对归档——KID 官方移植用 `"LNK"` magic + 8 字节文件名 + 偏移、图片是 `.cps`、脚本是 `ic01.scr`；
这套引擎用的是「条目数 + 扩展名 + 名字 + 偏移 + 长度」的自有布局、图片是 `png`、脚本是三份 XML，
还叠了 3DES。**从文件格式开始就是重写的**，不是给 KID 引擎套壳。详见
[provenance/2007-09-10-sina-again-port.md](provenance/2007-09-10-sina-again-port.md)。

顺带一提：2007 版运行需要 **.NET Framework 2.0** 与 DirectX for Managed Code（据骨灰站实测记录），
而《雪萤》只要 1.1——同一套引擎在一年里从 1.1 升到了 2.0，技术栈没变（依旧是 Managed DirectX 9）。

本仓库覆盖的是第一个产物。第二个产物的程序集目前没有留存，若能拿到，就能看到这套引擎在一年间的演进。

所以本仓库收录的这份，就是这套引擎目前已知的唯一实现。这也是它值得单独建档的原因：
不是「某部游戏的源码」，而是一套只留下一个产物的引擎。

> 以上特征全部来自程序集元数据与源码本身。`KFCEngine` 这个名字不含原始出处，
> 仅为本项目检索与叙述而设；若 KFC 作者希望使用其他名称，改掉即可。

## 出处与历史

原始发布帖（一手资料）：

- 站点：KeyFansClub 论坛 · 同人讨论区（综合同人区）
- 标题：**[原创][03.02][KFC特創組]Memories Off 2nd Pure～雪萤中文PC完全版**
- 链接：<https://keyfc.com/bbs/showtopic.aspx?forumid=10&forumpage=1&topicid=10942>
- 存档：[PROVENANCE.md](PROVENANCE.md)（主楼全文转录，防链接失效）
- 发帖：`jackpot`（凯），2006-03-02 21:49，当时 19700+ 查看 / 17 回复

帖中对程序性质的原话：

> MemoriesOff 2nd Pure～雪萤是 MemoriesOff Duet 中所附带的 MemoriesOff 2nd 的前传故事……
> 这就是 MemoriesOff 2nd Pure～雪萤 中文PC版！！！
>
> **本作品是 KFC 特别创作组首个完全自主从 PS2 主机移植到 PC 主机的游戏，它的重大意义在于实现了完全自主的技术汉化工作。本作品的 PC 版本程序完全是自主原创**，几乎具备目前 KID 游戏官方 PC 版的全部功能，同时增加了简体中文/繁体中文/日本语三种语种随时随地的可切换功能……
>
> 本版本为本作品的最终完全整合版。再下一进化版本制作完成前，本版本为最高完整版。

也就是说：**PC 端的程序实现是 KFC 自己写的，不是 KID 官方引擎**。
这解释了为什么它是一套 2005 年的 .NET 1.1 + WinForms + Managed DirectX 9 应用——
那个年代民间移植用 .NET 换掉 PS2 的 C/C++ 工具链，是相当务实的选择。

官方安装要求（原帖「安装步骤」）：

1. .NET Framework 1.1（`dotnetfx.exe` + 对应语言 `langpack.exe`）
2. DirectX 9.0c（`dxsetup.exe`）
3. `Setup.exe` 安装游戏

这正是本工程必须用 .NET 1.1 的 `csc.exe` 而不是现代 MSBuild 构建的原因：
目标运行时就是 1.1。

制作名单（原帖）：

| 分工 | 成员 |
| --- | --- |
| 制作企划 | Mullen |
| 剧本翻译 | take |
| 程序编写 | Mullen |
| 脚本编写 | Mullen |
| 图片处理 | 玛玛哈哈 |
| 破解协助 | 紫砂千雾 |
| 制作监督 | 冰天火焰 |

程序集元数据里的 `Company = Mullen@KFC`、`Description = MO2PURE`、
3DES 密钥 `"MullenMullenMullenMullen"`、归档校验串 `"Mullen"`，都与这份名单对得上。

原帖标注「作品大小：210M」，本仓库 `game-data/` 为 214 MB（差值是 `Manual/`、`Movie/`
与解包后的文件对齐开销），量级吻合。

### 第二个来源：2018 年触乐长文

发布十二年后，触乐采访了 KFC 特别创作组组长（后来的 HIKARI FIELD 创始人）冰天火焰，
写出了这套引擎**怎么来的**——这是目前唯一讲到过程的公开材料：

> 到了 2005 年春天，汉化组的队伍加入了一名能力超强的程序员。他是一名《雪萤》的铁粉，受到此前《雪萤》翻译连载以及中文影像版的感召，
> 他当时提出了一个所有人做梦都没想到过的方案：**他想要自己编写一套游戏引擎来把 PS2 平台上的《雪萤》移植到 PC 上。**
> ……首先，冰天火焰通过截屏的方式把 PS2 版游戏的背景素材、人物素材和 CG 图像截取出来，接下来由美工人员想尽办法从游戏截图中把人物立绘抠出来。
> 再然后，把整个游戏录制成一个完整的视频，接下来由程序人员对照着视频进行引擎的编写，将游戏的基本功能一一重现。
> 最后，还要由脚本编写人员再对照着视频把游戏里的演出效果重写一遍。

全文存档：[provenance/2018-04-20-chuapp-interview.md](provenance/2018-04-20-chuapp-interview.md)；
另有 2007 年移植报道：[provenance/2007-09-10-sina-again-port.md](provenance/2007-09-10-sina-again-port.md)。
出处索引见 [PROVENANCE.md](PROVENANCE.md)。

> 原帖**没有**给这套程序实现起名字。本仓库使用的 `KFCEngine` 是自取的工作名，
> 理由见上一节。

## 还原过程

```
原版 Yukihota.exe
  │ ① dnSpy.Console 全量导出（VS2005 风格工程，TargetFrameworkVersion v1.1）
  ▼
work/dnspy_dump   64 类型 / 2 resx / ico
  │ ② 全量标识符 ASCII 化（933 个符号，含 \uXXXX 转义）
  │ ③ C# 1.1 语法降级 + 反编译瑕疵修补
  ▼
src/*.cs
  │ ④ 类型/命名空间语义命名，按类型名重排文件
  │ ⑤ 回填 27 个 FieldRVA 数据块（<PrivateImplementationDetails>）
  │ ⑥ 删除混淆器注入的空方法壳与标记属性
  │ ⑦ 资源改回原始 v1 格式、置 32BITREQUIRED、工程化
  ▼
本仓库
```

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

## 判定为「混淆器注入」而删除的内容

判定标准：原始源码里本不存在、由保护器额外生成。三条独立证据——零引用、匿名类 `<EMPTY_NAME>`、XenoCode 自己的命名空间。

| 内容 | 规模 | 处理 |
| --- | --- | --- |
| 空方法壳（`internal class <EMPTY_NAME>`） | 7863 个方法 | 删除 |
| `XenoCode.User.Attributes.AssemblyAttributes.ProcessedByXenoCode` | 1 类 | 删除 |
| `XenoCode.User.Attributes.AssemblyAttributes.SuppressDisassembly` | 1 类 | 删除 |
| `[assembly: ProcessedByXenoCode]` / `[assembly: SuppressDisassembly]` | 2 条 | 删除 |

清理后：MethodDef 8192 → **330**，CustomAttribute 14 → **13**。

**保留的**：`StaticData`（原 `ࠐ`，27 个数据块）经逐一比对确认是 C# 编译器的 `<PrivateImplementationDetails>`——数据块内容与源码里 `new int[]{...}` 初始化器完全对应（`{5,15,20}`、23 元素调色板、32 元素正弦表…），大小都是 `12/16/24/28/64/92/128` 的 `__StaticArrayInitTypeSize` 模式。它是编译器产物、不是混淆器产物，因此保留并回填了全部 1408 字节（运行时校验逐字节一致）。

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

## 征集

如果你手上还有这套引擎的其他痕迹，欢迎开 issue：

- **《秋之回忆：从今以后～AGAIN》PC 中文版（娱乐通 / 光谱，2007）的可执行文件**——同一引擎的第二个产物，比对它能看出引擎的演进
- 原始安装包（`Setup.exe`）、当时的 BT 种子或发布帖存档
- 作者 `Mullen` 的技术说明、开发记录
- 2006 年那个「进化版本」的任何线索（原帖称本版为「最终完全整合版」，并提到还有后续计划）
- HIKARI FIELD 早期的技术资料、KFC 特别创作组的开发记录

这类资料比代码更难保存，也更难找回。
