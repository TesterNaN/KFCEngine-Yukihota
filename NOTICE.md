# 版权与使用声明

本仓库同时包含**多份不同来源的材料**，版权归属各自不同，请分别看待。

## 1. 原始游戏与数据文件

| 内容 | 版权归属 |
| --- | --- |
| 《Memories Off 2nd ～雪萤～》原作、剧本、美术、音乐、语音 | © KID (Kindle Imagine Develop) |
| 程序实现（引擎、场景脚本格式、归档格式）、简体/繁体中文文本 | © KID Fans Club（程序集元数据署名 `Mullen@KFC`，2005） |
| `game-data/*.dat`、`game-data/save_sys.dat` | 上述两方的产物，本仓库只是原封转存 |

这些文件**不是**本项目的成果。它们在此仅作为**失传媒体（lost media）存档**保留：
原发布渠道早已失效，本仓库的目标是让这份中文化作品不至于彻底消失。

## 2. 逆向还原的源代码

`src/`、`Properties/`、`resources/`、`Yukihota.csproj`、`README.md` 等，
是从上述程序集反编译、反混淆、重构而来。这部分工作（反混淆、命名、C# 1.1 适配、
缺陷修复、工程化）由本仓库贡献者完成，按 **MIT** 许可提供：

```
MIT License

Copyright (c) 2026 Yukihota decompilation contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

注意：MIT 只覆盖"还原工作本身"（重命名、重构、构建脚本、文档）。
它**不能**改变第 1 条中原始游戏内容与汉化文本的版权状态——
这些内容的权利仍在其各自权利人手中。

## 3. 第三方运行库

| 文件 | 版权 |
| --- | --- |
| `game-data/Microsoft.DirectX*.dll` | © Microsoft（Managed DirectX 9 托管包装，随 DirectX SDK 分发） |
| `game-data/d3dx9_26.dll` | © Microsoft（Direct3D 9 扩展运行库） |

这些是程序运行所必需的依赖，按原样转存。

## 4. 使用范围

本项目**仅用于**：

- 失传媒体存档与保存
- .NET 程序保护/反保护技术的教学与研究
- 对一份 2005 年中文化作品的技术考古

请不要用于任何商业用途，也不要把 `game-data/` 里的数据当作可自由再分发的素材。
如果你希望合法地体验这部作品，请支持官方的后续发行版本。

## 5. 免责

本仓库按「现状」提供，不附带任何明示或暗示的担保。使用本仓库内容所产生的一切后果由使用者自行承担。
如权利人认为本仓库的存档行为侵犯其权益，请通过 Issue 联系。
