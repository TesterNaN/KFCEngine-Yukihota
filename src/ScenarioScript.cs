using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.DirectX.Direct3D;

namespace Yukihota
{
	// Token: 0x02000021 RID: 33
	public class ScenarioScript : IDisposable
	{
		// Token: 0x06001F23 RID: 7971 RVA: 0x0001451C File Offset: 0x0001351C
		public ScenarioScript(string scriptPath, int episode, Device graphicsDevice)
		{
			this.device = graphicsDevice;
			for (int i = 0; i < 3; i++)
			{
				this.documents[i] = new XmlDocument();
			}
			this.documents[0].Load(ScenarioScript.scriptArchive.ReadEntry("SC"));
			this.documents[1].Load(ScenarioScript.scriptArchive.ReadEntry("TC"));
			this.documents[2].Load(ScenarioScript.scriptArchive.ReadEntry("JP"));
			for (int j = 0; j < 3; j++)
			{
				this.episodeNodes[j] = this.documents[j].SelectNodes(scriptPath + "/episode")[episode - 1];
				if (this.episodeNodes[j] == null)
				{
					throw new Exception(ScenarioScript.errorMessage);
				}
				this.currentNodes[j] = this.episodeNodes[j];
			}
			this.episodeNumber = episode;
			this.maxAlbumIndex = 0;
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0001464C File Offset: 0x0001364C
		~ScenarioScript()
		{
			this.Dispose();
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00014684 File Offset: 0x00013684
		public void Dispose()
		{
			for (int i = 0; i < 4; i++)
			{
				try
				{
					if (File.Exists(string.Concat(new object[]
					{
						ScenarioScript.tempDirectory,
						"\\bgm",
						i,
						".wav"
					})))
					{
						File.Delete(string.Concat(new object[]
						{
							ScenarioScript.tempDirectory,
							"\\bgm",
							i,
							".wav"
						}));
					}
				}
				catch
				{
				}
				try
				{
					if (File.Exists(string.Concat(new object[]
					{
						ScenarioScript.tempDirectory,
						"\\voice",
						i,
						".wav"
					})))
					{
						File.Delete(string.Concat(new object[]
						{
							ScenarioScript.tempDirectory,
							"\\voice",
							i,
							".wav"
						}));
					}
				}
				catch
				{
				}
				try
				{
					if (File.Exists(string.Concat(new object[]
					{
						ScenarioScript.tempDirectory,
						"\\se",
						i,
						".wav"
					})))
					{
						File.Delete(string.Concat(new object[]
						{
							ScenarioScript.tempDirectory,
							"\\se",
							i,
							".wav"
						}));
					}
				}
				catch
				{
				}
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x00014850 File Offset: 0x00013850
		public ScenarioResult Advance(out object resultData, out object resultOption, bool skipVoice)
		{
			resultData = null;
			resultOption = null;
			if (this.currentNodes[0] == null)
			{
				return ScenarioResult.Error;
			}
			if (this.currentNodes[0].Name != "paragraph" && this.currentNodes[0].Name != "dialogue" && this.currentNodes[0].HasChildNodes)
			{
				for (int i = 0; i < 3; i++)
				{
					if (!this.currentNodes[i].HasChildNodes)
					{
						MessageBox.Show(ScenarioScript.errorMessage);
						return ScenarioResult.Error;
					}
					this.currentNodes[i] = this.currentNodes[i].FirstChild;
				}
			}
			else if (this.currentNodes[0].NextSibling != null)
			{
				for (int j = 0; j < 3; j++)
				{
					if (this.currentNodes[j].NextSibling == null)
					{
						MessageBox.Show(ScenarioScript.errorMessage);
						return ScenarioResult.Error;
					}
					this.currentNodes[j] = this.currentNodes[j].NextSibling;
				}
			}
			else
			{
				bool flag = true;
				while (this.currentNodes[0].ParentNode != this.episodeNodes[0])
				{
					if (this.currentNodes[0].ParentNode.NextSibling != null)
					{
						for (int k = 0; k < 3; k++)
						{
							if (this.currentNodes[k].ParentNode == null || this.currentNodes[k].ParentNode.NextSibling == null)
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							this.currentNodes[k] = this.currentNodes[k].ParentNode.NextSibling;
						}
						flag = false;
						break;
					}
					for (int l = 0; l < 3; l++)
					{
						if (this.currentNodes[l].ParentNode == null)
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						this.currentNodes[l] = this.currentNodes[l].ParentNode;
					}
				}
				if (flag)
				{
					if (this.currentNodes[0].ParentNode.NextSibling == null)
					{
						return ScenarioResult.Error;
					}
					for (int m = 0; m < 3; m++)
					{
						if (this.currentNodes[m].ParentNode == null || this.currentNodes[m].ParentNode.NextSibling == null)
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						this.currentNodes[m] = this.currentNodes[m].ParentNode.NextSibling;
						this.episodeNodes[m] = this.currentNodes[m];
					}
					this.episodeNumber++;
				}
			}
			if (this.currentNodes[0].Name == "click")
			{
				if (this.currentNodes[0].PreviousSibling != null && (this.currentNodes[0].PreviousSibling.Name == "dialogue" || this.currentNodes[0].PreviousSibling.Name == "paragraph") && this.currentNodes[0].PreviousSibling.Attributes["voice"] != null)
				{
					resultOption = true;
				}
				else
				{
					resultOption = false;
				}
				return ScenarioResult.Click;
			}
			if (this.currentNodes[0].Name == "scene")
			{
				if (this.currentNodes[0].Attributes["background"] != null)
				{
					GC.Collect();
					try
					{
						resultData = TextureLoader.FromStream(this.device, ScenarioScript.bgArchive.ReadEntry(this.currentNodes[0].Attributes["background"].Value), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						goto IL_2066;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						return ScenarioResult.Error;
					}
				}
				goto IL_215A;
				IL_2066:
				if (this.currentNodes[0].Attributes["time"] == null || this.currentNodes[0].Attributes["time"].Value == "short")
				{
					resultOption = 1;
				}
				else if (this.currentNodes[0].Attributes["time"].Value == "long")
				{
					resultOption = 2;
				}
				else
				{
					if (!(this.currentNodes[0].Attributes["time"].Value == "no"))
					{
						MessageBox.Show(ScenarioScript.errorMessage);
						return ScenarioResult.Error;
					}
					resultOption = 0;
				}
				if (this.currentNodes[0].Attributes["album"] != null)
				{
					try
					{
						int num = int.Parse(this.currentNodes[0].Attributes["album"].Value);
						if (num > this.maxAlbumIndex)
						{
							this.maxAlbumIndex = num;
						}
					}
					catch (Exception ex2)
					{
						MessageBox.Show(ex2.Message);
						return ScenarioResult.Error;
					}
				}
				return ScenarioResult.Scene;
				IL_215A:
				if (this.currentNodes[0].Attributes["backcolor"] != null)
				{
					try
					{
						resultData = Color.FromName(this.currentNodes[0].Attributes["backcolor"].Value);
						goto IL_2066;
					}
					catch (Exception ex3)
					{
						MessageBox.Show(ex3.Message);
						return ScenarioResult.Error;
					}
				}
				resultData = Color.Black;
				goto IL_2066;
			}
			if (this.currentNodes[0].Name == "music")
			{
				if ((this.currentNodes[0].Attributes["mute"] != null && !(this.currentNodes[0].Attributes["mute"].Value == "false")) || this.currentNodes[0].Attributes["name"] == null)
				{
					goto IL_2B06;
				}
				try
				{
					ScenarioScript.bgmArchive.Extract(this.currentNodes[0].Attributes["name"].Value, ScenarioScript.tempDirectory);
					for (int n = 0; n < 4; n++)
					{
						bool flag2 = false;
						try
						{
							if (File.Exists(string.Concat(new object[]
							{
								ScenarioScript.tempDirectory,
								"\\bgm",
								ScenarioScript.bgmSlotIndex,
								".wav"
							})))
							{
								File.Delete(string.Concat(new object[]
								{
									ScenarioScript.tempDirectory,
									"\\bgm",
									ScenarioScript.bgmSlotIndex,
									".wav"
								}));
							}
						}
						catch (Exception ex4)
						{
							if (n >= 3)
							{
								MessageBox.Show(ex4.Message);
								return ScenarioResult.Error;
							}
							flag2 = true;
						}
						if (!flag2)
						{
							break;
						}
						ScenarioScript.bgmSlotIndex = (ScenarioScript.bgmSlotIndex + 1) % 4;
					}
					File.Move(ScenarioScript.tempDirectory + "\\" + this.currentNodes[0].Attributes["name"].Value + ".wav", string.Concat(new object[]
					{
						ScenarioScript.tempDirectory,
						"\\bgm",
						ScenarioScript.bgmSlotIndex,
						".wav"
					}));
					GC.Collect();
					resultData = string.Concat(new object[]
					{
						ScenarioScript.tempDirectory,
						"\\bgm",
						ScenarioScript.bgmSlotIndex,
						".wav"
					});
					ScenarioScript.bgmSlotIndex = (ScenarioScript.bgmSlotIndex + 1) % 4;
				}
				catch (Exception ex5)
				{
					MessageBox.Show(ex5.Message);
					return ScenarioResult.Error;
				}
				if (this.currentNodes[0].Attributes["position"] != null)
				{
					try
					{
						resultOption = double.Parse(this.currentNodes[0].Attributes["position"].Value);
						return ScenarioResult.Bgm;
					}
					catch (Exception ex6)
					{
						MessageBox.Show(ex6.Message);
						return ScenarioResult.Error;
					}
					goto IL_2B06;
				}
				resultOption = 0.0;
				return ScenarioResult.Bgm;
				IL_2B06:
				if (this.currentNodes[0].Attributes["mute"] != null && this.currentNodes[0].Attributes["mute"].Value == "true")
				{
					resultData = null;
					resultOption = 0.0;
					return ScenarioResult.Bgm;
				}
				if (this.currentNodes[0].Attributes["volume"] == null)
				{
					MessageBox.Show(ScenarioScript.errorMessage);
					return ScenarioResult.Error;
				}
				resultData = null;
				if (this.currentNodes[0].Attributes["volume"].Value == "down")
				{
					resultOption = false;
					return ScenarioResult.Bgm;
				}
				if (this.currentNodes[0].Attributes["volume"].Value == "up")
				{
					resultOption = true;
					return ScenarioResult.Bgm;
				}
				MessageBox.Show(ScenarioScript.errorMessage);
				return ScenarioResult.Error;
			}
			else
			{
				if (this.currentNodes[0].Name == "chara")
				{
					resultOption = new SpriteState();
					((SpriteState)resultOption).level = 0;
					try
					{
						((SpriteState)resultOption).index = int.Parse(this.currentNodes[0].Attributes["index"].Value);
					}
					catch (Exception ex7)
					{
						MessageBox.Show(ex7.Message);
						return ScenarioResult.Error;
					}
					if (this.currentNodes[0].Attributes["action"] != null)
					{
						if (!(this.currentNodes[0].Attributes["action"].Value == "show"))
						{
							resultData = null;
							if (this.currentNodes[0].Attributes["action"].Value == "hide")
							{
								((SpriteState)resultOption).action = SpriteMode.Static;
								goto IL_1F09;
							}
							if (!(this.currentNodes[0].Attributes["action"].Value == "shock"))
							{
								goto IL_1F09;
							}
							((SpriteState)resultOption).action = SpriteMode.Shaking;
							if (this.currentNodes[0].Attributes["level"] == null)
							{
								((SpriteState)resultOption).level = 0;
								goto IL_1F09;
							}
							try
							{
								((SpriteState)resultOption).level = int.Parse(this.currentNodes[0].Attributes["level"].Value);
							}
							catch (Exception ex8)
							{
								MessageBox.Show(ex8.Message);
								return ScenarioResult.Error;
							}
							goto IL_1F09;
						}
					}
					try
					{
						ImageInformation imageInformation = TextureLoader.ImageInformationFromStream(ScenarioScript.charaArchive.ReadEntry(this.currentNodes[0].Attributes["code"].Value));
						resultData = TextureLoader.FromStream(this.device, ScenarioScript.charaArchive.ReadEntry(this.currentNodes[0].Attributes["code"].Value), imageInformation.Width, imageInformation.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						((SpriteState)resultOption).size = new Size(imageInformation.Width, imageInformation.Height);
					}
					catch (Exception ex9)
					{
						MessageBox.Show(ex9.Message);
						return ScenarioResult.Error;
					}
					((SpriteState)resultOption).action = SpriteMode.Static;
					IL_1F09:
					if (this.currentNodes[0].Attributes["location"] != null)
					{
						try
						{
							((SpriteState)resultOption).location = int.Parse(this.currentNodes[0].Attributes["location"].Value);
							goto IL_19A6;
						}
						catch (Exception ex10)
						{
							MessageBox.Show(ex10.Message);
							return ScenarioResult.Error;
						}
						goto IL_2978;
					}
					((SpriteState)resultOption).location = int.MaxValue;
					IL_19A6:
					if (this.currentNodes[0].Attributes["transparent"] == null || this.currentNodes[0].Attributes["transparent"].Value == "false")
					{
						((SpriteState)resultOption).isTransparent = false;
					}
					else
					{
						if (!(this.currentNodes[0].Attributes["transparent"].Value == "true"))
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						((SpriteState)resultOption).isTransparent = true;
					}
					if (this.currentNodes[0].Attributes["animation"] == null || this.currentNodes[0].Attributes["animation"].Value == "true")
					{
						((SpriteState)resultOption).isAnimated = true;
					}
					else
					{
						if (!(this.currentNodes[0].Attributes["animation"].Value == "false"))
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						((SpriteState)resultOption).isAnimated = false;
					}
					if (this.currentNodes[0].Attributes["synchro"] == null)
					{
						((SpriteState)resultOption).sync = !((SpriteState)resultOption).isAnimated;
					}
					else if (this.currentNodes[0].Attributes["synchro"].Value == "false")
					{
						((SpriteState)resultOption).sync = false;
					}
					else
					{
						if (!(this.currentNodes[0].Attributes["synchro"].Value == "true"))
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						((SpriteState)resultOption).sync = true;
					}
					return ScenarioResult.Character;
				}
				IL_2978:
				if (!(this.currentNodes[0].Name == "paragraph") && !(this.currentNodes[0].Name == "dialogue"))
				{
					if (this.currentNodes[0].Name == "sound")
					{
						resultOption = new InputState();
						if (this.currentNodes[0].Attributes["loop"] == null || this.currentNodes[0].Attributes["loop"].Value == "false")
						{
							((InputState)resultOption).loop = false;
						}
						else
						{
							if (!(this.currentNodes[0].Attributes["loop"].Value == "true"))
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							((InputState)resultOption).loop = true;
						}
						if (this.currentNodes[0].Attributes["animation"] == null || this.currentNodes[0].Attributes["animation"].Value == "true")
						{
							((InputState)resultOption).wait = true;
						}
						else
						{
							if (!(this.currentNodes[0].Attributes["animation"].Value == "false"))
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							((InputState)resultOption).wait = false;
						}
						if (this.currentNodes[0].Attributes["mute"] == null || this.currentNodes[0].Attributes["mute"].Value == "false")
						{
							if (this.currentNodes[0].Attributes["name"] == null)
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							try
							{
								ScenarioScript.seArchive.Extract(this.currentNodes[0].Attributes["name"].Value, ScenarioScript.tempDirectory);
								for (int num2 = 0; num2 < 4; num2++)
								{
									bool flag3 = false;
									try
									{
										if (File.Exists(string.Concat(new object[]
										{
											ScenarioScript.tempDirectory,
											"\\se",
											ScenarioScript.seSlotIndex,
											".wav"
										})))
										{
											File.Delete(string.Concat(new object[]
											{
												ScenarioScript.tempDirectory,
												"\\se",
												ScenarioScript.seSlotIndex,
												".wav"
											}));
										}
									}
									catch (Exception ex11)
									{
										if (num2 >= 3)
										{
											MessageBox.Show(ex11.Message);
											return ScenarioResult.Error;
										}
										flag3 = true;
									}
									if (!flag3)
									{
										break;
									}
									ScenarioScript.seSlotIndex = (ScenarioScript.seSlotIndex + 1) % 4;
								}
								File.Move(ScenarioScript.tempDirectory + "\\" + this.currentNodes[0].Attributes["name"].Value + ".wav", string.Concat(new object[]
								{
									ScenarioScript.tempDirectory,
									"\\Se",
									ScenarioScript.seSlotIndex,
									".wav"
								}));
								GC.Collect();
								resultData = string.Concat(new object[]
								{
									ScenarioScript.tempDirectory,
									"\\se",
									ScenarioScript.seSlotIndex,
									".wav"
								});
								ScenarioScript.seSlotIndex = (ScenarioScript.seSlotIndex + 1) % 4;
								return ScenarioResult.Sound;
							}
							catch (Exception ex12)
							{
								MessageBox.Show(ex12.Message);
								return ScenarioResult.Error;
							}
							goto IL_0B06;
						}
						else
						{
							if (!(this.currentNodes[0].Attributes["mute"].Value == "true"))
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							resultData = null;
						}
						return ScenarioResult.Sound;
					}
					XmlNode[] u;
					if (this.currentNodes[0].Name == "wait")
					{
						if (this.currentNodes[0].Attributes["time"] == null)
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						if (this.currentNodes[0].Attributes["time"].Value == "no")
						{
							resultOption = 0;
						}
						else if (this.currentNodes[0].Attributes["time"].Value == "short")
						{
							resultOption = 1;
						}
						else if (this.currentNodes[0].Attributes["time"].Value == "middle")
						{
							resultOption = 2;
						}
						else
						{
							if (!(this.currentNodes[0].Attributes["time"].Value == "long"))
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							resultOption = 3;
						}
						return ScenarioResult.Jump;
					}
					else if (this.currentNodes[0].Name == "clear")
					{
						if (this.currentNodes[0].Attributes["animation"] == null || this.currentNodes[0].Attributes["animation"].Value == "false")
						{
							resultOption = false;
							return ScenarioResult.Dismiss;
						}
						u = this.currentNodes;
					}
					else
					{
						if (this.currentNodes[0].Name == "title")
						{
							resultData = new string[3];
							for (int num3 = 0; num3 < 3; num3++)
							{
								if (this.episodeNodes[num3].Attributes["title"] == null)
								{
									MessageBox.Show(ScenarioScript.errorMessage);
									return ScenarioResult.Error;
								}
								((string[])resultData)[num3] = this.episodeNodes[num3].Attributes["title"].Value;
							}
							return ScenarioResult.Notice;
						}
						if (this.currentNodes[0].Name == "filter")
						{
							if (this.currentNodes[0].Attributes["type"] == null)
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							if (this.currentNodes[0].Attributes["type"].Value == "zoom")
							{
								resultOption = new AnimItem();
								((AnimItem)resultOption).type = DrawItemType.Zoom;
								if (this.currentNodes[0].Attributes["left"] == null)
								{
									((AnimItem)resultOption).left = 0.0;
								}
								else
								{
									try
									{
										((AnimItem)resultOption).left = double.Parse(this.currentNodes[0].Attributes["left"].Value);
									}
									catch (Exception ex13)
									{
										MessageBox.Show(ex13.Message);
										return ScenarioResult.Error;
									}
								}
								if (this.currentNodes[0].Attributes["right"] == null)
								{
									((AnimItem)resultOption).right = 1.0;
								}
								else
								{
									try
									{
										((AnimItem)resultOption).right = double.Parse(this.currentNodes[0].Attributes["right"].Value);
									}
									catch (Exception ex14)
									{
										MessageBox.Show(ex14.Message);
										return ScenarioResult.Error;
									}
								}
								if (this.currentNodes[0].Attributes["top"] == null)
								{
									((AnimItem)resultOption).top = 0.0;
								}
								else
								{
									try
									{
										((AnimItem)resultOption).top = double.Parse(this.currentNodes[0].Attributes["top"].Value);
									}
									catch (Exception ex15)
									{
										MessageBox.Show(ex15.Message);
										return ScenarioResult.Error;
									}
								}
								if (this.currentNodes[0].Attributes["bottom"] != null)
								{
									try
									{
										((AnimItem)resultOption).bottom = double.Parse(this.currentNodes[0].Attributes["bottom"].Value);
										goto IL_28EA;
									}
									catch (Exception ex16)
									{
										MessageBox.Show(ex16.Message);
										return ScenarioResult.Error;
									}
									goto IL_287A;
								}
								((AnimItem)resultOption).bottom = 1.0;
								IL_28EA:
								if (this.currentNodes[0].Attributes["time"] == null || this.currentNodes[0].Attributes["time"].Value == "short")
								{
									((AnimItem)resultOption).durationMode = 1;
									goto IL_03B2;
								}
								if (this.currentNodes[0].Attributes["time"].Value == "long")
								{
									((AnimItem)resultOption).durationMode = 2;
									goto IL_03B2;
								}
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							else
							{
								if (this.currentNodes[0].Attributes["type"].Value == "dark")
								{
									resultOption = new DrawItem();
									((DrawItem)resultOption).type = DrawItemType.Dim;
									goto IL_03B2;
								}
								if (this.currentNodes[0].Attributes["type"].Value == "think")
								{
									resultOption = new DrawItem();
									((DrawItem)resultOption).type = DrawItemType.Think;
									goto IL_03B2;
								}
								if (this.currentNodes[0].Attributes["type"].Value == "sun")
								{
									resultOption = new DrawItem();
									((DrawItem)resultOption).type = DrawItemType.Sun;
									goto IL_03B2;
								}
								if (this.currentNodes[0].Attributes["type"].Value == "sakura")
								{
									resultOption = new DrawItem();
									((DrawItem)resultOption).type = DrawItemType.Petals;
									goto IL_03B2;
								}
								if (this.currentNodes[0].Attributes["type"].Value == "snow")
								{
									resultOption = new DrawItem();
									((DrawItem)resultOption).type = DrawItemType.Snow;
									goto IL_03B2;
								}
								if (this.currentNodes[0].Attributes["type"].Value == "shock")
								{
									resultOption = new FlagItem();
									((FlagItem)resultOption).type = DrawItemType.Shake;
									if (this.currentNodes[0].Attributes["loop"] != null)
									{
										if (!(this.currentNodes[0].Attributes["loop"].Value == "false"))
										{
											if (this.currentNodes[0].Attributes["loop"].Value == "true")
											{
												((FlagItem)resultOption).loop = true;
											}
											goto IL_03B2;
										}
									}
									((FlagItem)resultOption).loop = false;
									goto IL_03B2;
								}
								if (!(this.currentNodes[0].Attributes["type"].Value == "back"))
								{
									resultData = null;
									resultOption = null;
									return ScenarioResult.None;
								}
								resultOption = new TextureItem();
								((TextureItem)resultOption).type = DrawItemType.Background;
								((TextureItem)resultOption).texture = TextureLoader.FromStream(this.device, ScenarioScript.bgArchive.ReadEntry(this.currentNodes[0].Attributes["ground"].Value), 0, 0, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
								if (this.currentNodes[0].Attributes["trans"] == null || this.currentNodes[0].Attributes["trans"].Value == "fade")
								{
									((TextureItem)resultOption).blend = TextureBlend.Normal;
								}
								else if (this.currentNodes[0].Attributes["trans"].Value == "left")
								{
									((TextureItem)resultOption).blend = TextureBlend.Left;
								}
								else
								{
									if (!(this.currentNodes[0].Attributes["trans"].Value == "right"))
									{
										MessageBox.Show(ScenarioScript.errorMessage);
										return ScenarioResult.Error;
									}
									((TextureItem)resultOption).blend = TextureBlend.Right;
								}
								if (this.currentNodes[0].Attributes["time"] == null || this.currentNodes[0].Attributes["time"].Value == "short")
								{
									((TextureItem)resultOption).durationMode = 1;
								}
								else if (this.currentNodes[0].Attributes["time"].Value == "long")
								{
									((TextureItem)resultOption).durationMode = 2;
								}
								else
								{
									if (!(this.currentNodes[0].Attributes["time"].Value == "no"))
									{
										MessageBox.Show(ScenarioScript.errorMessage);
										return ScenarioResult.Error;
									}
									((TextureItem)resultOption).durationMode = 0;
								}
								if (this.currentNodes[0].Attributes["album"] != null)
								{
									try
									{
										int num4 = int.Parse(this.currentNodes[0].Attributes["album"].Value);
										if (num4 > this.maxAlbumIndex)
										{
											this.maxAlbumIndex = num4;
										}
										goto IL_03B2;
									}
									catch (Exception ex17)
									{
										MessageBox.Show(ex17.Message);
										return ScenarioResult.Error;
									}
									goto IL_04A7;
								}
								goto IL_03B2;
							}
							IL_0348:
							if (this.currentNodes[0].Attributes["synchro"] == null)
							{
								((DrawItem)resultOption).sync = !((DrawItem)resultOption).isAnimated;
							}
							else if (this.currentNodes[0].Attributes["synchro"].Value == "false")
							{
								((DrawItem)resultOption).sync = false;
							}
							else
							{
								if (!(this.currentNodes[0].Attributes["synchro"].Value == "true"))
								{
									MessageBox.Show(ScenarioScript.errorMessage);
									return ScenarioResult.Error;
								}
								((DrawItem)resultOption).sync = true;
							}
							if (this.currentNodes[0].Attributes["finish"] == null || this.currentNodes[0].Attributes["finish"].Value == "false")
							{
								((DrawItem)resultOption).finish = false;
							}
							else
							{
								if (!(this.currentNodes[0].Attributes["finish"].Value == "true"))
								{
									MessageBox.Show(ScenarioScript.errorMessage);
									return ScenarioResult.Error;
								}
								((DrawItem)resultOption).finish = true;
							}
							return ScenarioResult.Draw;
							IL_03B2:
							object obj;
							if (this.currentNodes[0].Attributes["animation"] == null || this.currentNodes[0].Attributes["animation"].Value == "false")
							{
								obj = resultOption;
							}
							else
							{
								if (this.currentNodes[0].Attributes["animation"].Value == "true")
								{
									((DrawItem)resultOption).isAnimated = true;
									goto IL_0348;
								}
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							IL_04A7:
							((DrawItem)obj).isAnimated = false;
							goto IL_0348;
						}
						else
						{
							if (this.currentNodes[0].Name == "ed")
							{
								ScenarioScript.bgmArchive.Extract("BGM37", ScenarioScript.tempDirectory);
								for (int num5 = 0; num5 < 4; num5++)
								{
									bool flag4 = false;
									try
									{
										if (File.Exists(string.Concat(new object[]
										{
											ScenarioScript.tempDirectory,
											"\\bgm",
											ScenarioScript.bgmSlotIndex,
											".wav"
										})))
										{
											File.Delete(string.Concat(new object[]
											{
												ScenarioScript.tempDirectory,
												"\\bgm",
												ScenarioScript.bgmSlotIndex,
												".wav"
											}));
										}
									}
									catch (Exception ex18)
									{
										if (num5 >= 3)
										{
											MessageBox.Show(ex18.Message);
											return ScenarioResult.Error;
										}
										flag4 = true;
									}
									if (!flag4)
									{
										break;
									}
									ScenarioScript.bgmSlotIndex = (ScenarioScript.bgmSlotIndex + 1) % 4;
								}
								File.Move(ScenarioScript.tempDirectory + "\\BGM37.wav", string.Concat(new object[]
								{
									ScenarioScript.tempDirectory,
									"\\bgm",
									ScenarioScript.bgmSlotIndex,
									".wav"
								}));
								GC.Collect();
								string text = string.Concat(new object[]
								{
									ScenarioScript.tempDirectory,
									"\\bgm",
									ScenarioScript.bgmSlotIndex,
									".wav"
								});
								ScenarioScript.bgmSlotIndex = (ScenarioScript.bgmSlotIndex + 1) % 4;
								XmlDocument[] array = new XmlDocument[3];
								for (int num6 = 0; num6 < 3; num6++)
								{
									array[num6] = new XmlDocument();
								}
								array[0].Load(ScenarioScript.scriptArchive.ReadEntry("ED_SC"));
								array[1].Load(ScenarioScript.scriptArchive.ReadEntry("ED_TC"));
								array[2].Load(ScenarioScript.scriptArchive.ReadEntry("ED_JP"));
								StaffRoll u065D = new StaffRoll(text, ScenarioScript.charaArchive, ScenarioScript.bgArchive, array);
								resultData = u065D;
								return ScenarioResult.StaffRoll;
							}
							return ScenarioResult.None;
						}
					}
					IL_0B06:
					if (!(u[0].Attributes["animation"].Value == "true"))
					{
						MessageBox.Show(ScenarioScript.errorMessage);
						return ScenarioResult.Error;
					}
					resultOption = true;
					return ScenarioResult.Dismiss;
				}
				if (!this.currentNodes[0].HasChildNodes)
				{
					resultData = null;
					goto IL_1E4B;
				}
				resultData = new string[3];
				int num7 = 0;
				IL_186C:
				string[] array2;
				if (num7 < 3)
				{
					array2 = (string[])resultData;
					goto IL_287A;
				}
				IL_1E4B:
				string text2 = null;
				bool flag5 = false;
				if (this.currentNodes[0].Attributes["voice"] != null)
				{
					if (skipVoice)
					{
						flag5 = true;
					}
					else
					{
						try
						{
							ScenarioScript.voiceArchive.Extract(this.currentNodes[0].Attributes["voice"].Value, ScenarioScript.tempDirectory);
							for (int num8 = 0; num8 < 4; num8++)
							{
								bool flag6 = false;
								try
								{
									if (File.Exists(string.Concat(new object[]
									{
										ScenarioScript.tempDirectory,
										"\\voice",
										ScenarioScript.voiceSlotIndex,
										".wav"
									})))
									{
										File.Delete(string.Concat(new object[]
										{
											ScenarioScript.tempDirectory,
											"\\voice",
											ScenarioScript.voiceSlotIndex,
											".wav"
										}));
									}
								}
								catch (Exception ex19)
								{
									if (num8 >= 3)
									{
										MessageBox.Show(ex19.Message);
										return ScenarioResult.Error;
									}
									flag6 = true;
								}
								if (!flag6)
								{
									break;
								}
								ScenarioScript.voiceSlotIndex = (ScenarioScript.voiceSlotIndex + 1) % 4;
							}
							File.Move(ScenarioScript.tempDirectory + "\\" + this.currentNodes[0].Attributes["voice"].Value + ".wav", string.Concat(new object[]
							{
								ScenarioScript.tempDirectory,
								"\\voice",
								ScenarioScript.voiceSlotIndex,
								".wav"
							}));
							text2 = string.Concat(new object[]
							{
								ScenarioScript.tempDirectory,
								"\\voice",
								ScenarioScript.voiceSlotIndex,
								".wav"
							});
							ScenarioScript.voiceSlotIndex = (ScenarioScript.voiceSlotIndex + 1) % 4;
						}
						catch (Exception ex20)
						{
							MessageBox.Show(ex20.Message);
							return ScenarioResult.Error;
						}
					}
				}
				if (this.currentNodes[0].Name == "paragraph")
				{
					resultOption = new MessageCommand();
					if (this.currentNodes[0].Attributes["align"] == null)
					{
						goto IL_16B4;
					}
					if (this.currentNodes[0].Attributes["align"].Value == "left")
					{
						goto IL_16B4;
					}
					if (this.currentNodes[0].Attributes["align"].Value == "center")
					{
						((MessageCommand)resultOption).align = HorizontalAlign.Center;
					}
					else
					{
						if (!(this.currentNodes[0].Attributes["align"].Value == "right"))
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						((MessageCommand)resultOption).align = HorizontalAlign.Right;
					}
					IL_140A:
					if (this.currentNodes[0].Attributes["show"] == null || this.currentNodes[0].Attributes["show"].Value == "print")
					{
						((MessageCommand)resultOption).showMode = TextRevealMode.Typewriter;
					}
					else if (this.currentNodes[0].Attributes["show"].Value == "fade")
					{
						((MessageCommand)resultOption).showMode = TextRevealMode.Fade;
					}
					if (this.currentNodes[0].Attributes["fore"] == null)
					{
						((MessageCommand)resultOption).foreColor = Color.White;
					}
					else
					{
						try
						{
							((MessageCommand)resultOption).foreColor = Color.FromName(this.currentNodes[0].Attributes["fore"].Value);
						}
						catch (Exception ex21)
						{
							MessageBox.Show(ex21.Message);
							return ScenarioResult.Error;
						}
					}
					if (this.currentNodes[0].Attributes["back"] != null)
					{
						try
						{
							((MessageCommand)resultOption).shadowColor = Color.FromName(this.currentNodes[0].Attributes["back"].Value);
							goto IL_1E57;
						}
						catch (Exception ex22)
						{
							MessageBox.Show(ex22.Message);
							return ScenarioResult.Error;
						}
					}
					// 原始 IL 在此处之后还有一对不可达的 ldloc/ret（dnSpy 只能输出成无名局部变量
					// obj2，无法生成合法 C#）。运行时永远执行不到，故仅保留位置注释。
					((MessageCommand)resultOption).shadowColor = Color.Black;
					IL_1E57:
					XmlNode xmlNode = this.currentNodes[0].NextSibling;
					while (xmlNode != null && xmlNode.NodeType == XmlNodeType.Comment)
					{
						xmlNode = xmlNode.NextSibling;
					}
					if (xmlNode != null && xmlNode.Name == "click")
					{
						((MessageCommand)resultOption).waitClick = true;
					}
					else
					{
						((MessageCommand)resultOption).waitClick = false;
					}
					((MessageCommand)resultOption).voice = text2;
					((MessageCommand)resultOption).instant = flag5;
					return ScenarioResult.Message;
					IL_16B4:
					((MessageCommand)resultOption).align = HorizontalAlign.Left;
					goto IL_140A;
				}
				resultOption = new CaptionCommand();
				if (this.currentNodes[0].Attributes["chara"] != null)
				{
					for (int num9 = 0; num9 < 3; num9++)
					{
						if (this.currentNodes[num9].Attributes["chara"] == null)
						{
							MessageBox.Show(ScenarioScript.errorMessage);
							return ScenarioResult.Error;
						}
						((CaptionCommand)resultOption).charaNames[num9] = this.currentNodes[num9].Attributes["chara"].Value;
					}
				}
				else
				{
					((CaptionCommand)resultOption).charaNames = null;
				}
				if (this.currentNodes[0].Attributes["disappear"] == null || this.currentNodes[0].Attributes["disappear"].Value == "false")
				{
					((CaptionCommand)resultOption).disappear = false;
				}
				else
				{
					if (!(this.currentNodes[0].Attributes["disappear"].Value == "true"))
					{
						MessageBox.Show(ScenarioScript.errorMessage);
						return ScenarioResult.Error;
					}
					((CaptionCommand)resultOption).disappear = true;
				}
				if (this.currentNodes[0].Attributes["continue"] == null || this.currentNodes[0].Attributes["continue"].Value == "false")
				{
					((CaptionCommand)resultOption).isContinuous = false;
				}
				else
				{
					if (!(this.currentNodes[0].Attributes["continue"].Value == "true"))
					{
						MessageBox.Show(ScenarioScript.errorMessage);
						return ScenarioResult.Error;
					}
					((CaptionCommand)resultOption).isContinuous = true;
				}
				((CaptionCommand)resultOption).hideCharaName = ((CaptionCommand)resultOption).disappear;
				if (((CaptionCommand)resultOption).charaNames != null)
				{
					XmlNode xmlNode2 = this.currentNodes[0].NextSibling;
					while (xmlNode2 != null && xmlNode2.Name != "dialogue")
					{
						xmlNode2 = xmlNode2.NextSibling;
					}
					if (xmlNode2 != null)
					{
						if (xmlNode2.Attributes["chara"] == null)
						{
							((CaptionCommand)resultOption).hideCharaName = true;
						}
					}
					else
					{
						((CaptionCommand)resultOption).hideCharaName = true;
					}
				}
				((CaptionCommand)resultOption).voice = text2;
				((CaptionCommand)resultOption).instant = flag5;
				return ScenarioResult.Caption;
				IL_287A:
				array2[num7] = "";
				if (this.currentNodes[num7].HasChildNodes)
				{
					for (int num10 = 0; num10 < this.currentNodes[num7].ChildNodes.Count; num10++)
					{
						if (this.currentNodes[num7].ChildNodes[num10].Name == "br")
						{
							if (this.currentNodes[num7].ChildNodes[num10].Attributes["line"] == null)
							{
								string[] array3;
								IntPtr intPtr;
								(array3 = (string[])resultData)[(int)(intPtr = (IntPtr)num7)] = array3[(int)intPtr] + "\n";
							}
							else
							{
								int num11 = 1;
								try
								{
									num11 = int.Parse(this.currentNodes[num7].ChildNodes[num10].Attributes["line"].Value);
								}
								catch (Exception ex23)
								{
									MessageBox.Show(ex23.Message);
									return ScenarioResult.Error;
								}
								for (int num12 = 0; num12 < num11; num12++)
								{
									string[] array3;
									IntPtr intPtr;
									(array3 = (string[])resultData)[(int)(intPtr = (IntPtr)num7)] = array3[(int)intPtr] + "\n";
								}
							}
						}
						else
						{
							if (this.currentNodes[num7].ChildNodes[num10].NodeType != XmlNodeType.Text)
							{
								MessageBox.Show(ScenarioScript.errorMessage);
								return ScenarioResult.Error;
							}
							string[] array3;
							IntPtr intPtr;
							(array3 = (string[])resultData)[(int)(intPtr = (IntPtr)num7)] = array3[(int)intPtr] + this.currentNodes[num7].ChildNodes[num10].Value;
						}
					}
					num7++;
					goto IL_186C;
				}
				MessageBox.Show(ScenarioScript.errorMessage);
				return ScenarioResult.Error;
			}
			// 原 IL 在此处是一条不可达的 ldloc/ret（dnSpy 只能生成未初始化的局部变量，
			// 编译器因「不可达」而不报 CS0165）。所有分支都已 return/goto，这里给等价兜底。
			return ScenarioResult.Error;
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000177C0 File Offset: 0x000167C0
		public int GetEpisode()
		{
			return this.episodeNumber;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000177C8 File Offset: 0x000167C8
		public int GetAlbumIndex()
		{
			return this.maxAlbumIndex;
		}

		// Token: 0x040000D5 RID: 213
		protected Device device;

		// Token: 0x040000D6 RID: 214
		protected XmlDocument[] documents = new XmlDocument[3];

		// Token: 0x040000D7 RID: 215
		protected XmlNode[] episodeNodes = new XmlNode[3];

		// Token: 0x040000D8 RID: 216
		protected XmlNode[] currentNodes = new XmlNode[3];

		// Token: 0x040000D9 RID: 217
		protected static readonly string errorMessage = "Script Error";

		// Token: 0x040000DA RID: 218
		protected int episodeNumber;

		// Token: 0x040000DB RID: 219
		protected int maxAlbumIndex;

		// Token: 0x040000DC RID: 220
		protected static ArchiveFile bgArchive = new ArchiveFile("Bg", ArchiveMode.Read);

		// Token: 0x040000DD RID: 221
		protected static ArchiveFile bgmArchive = new ArchiveFile("Bgm", ArchiveMode.Read);

		// Token: 0x040000DE RID: 222
		protected static ArchiveFile charaArchive = new ArchiveFile("Chara", ArchiveMode.Read);

		// Token: 0x040000DF RID: 223
		protected static ArchiveFile scriptArchive = new ArchiveFile("Script", ArchiveMode.Read);

		// Token: 0x040000E0 RID: 224
		protected static ArchiveFile seArchive = new ArchiveFile("Se", ArchiveMode.Read);

		// Token: 0x040000E1 RID: 225
		protected static ArchiveFile voiceArchive = new ArchiveFile("Voice", ArchiveMode.Read);

		// Token: 0x040000E2 RID: 226
		// 原始 IL 里同一字段被赋值两次：
		//   IL_002C  call Path.GetTempPath()   → stsfld
		//   IL_0036  ldsfld 同字段 → Substring(0, Length-1) → stsfld 同字段
		// C# 字段初始化器无法表达"先赋初值再去掉尾部分隔符"，dnSpy 因此把它压成了自引用
		// （静态构造时读到 null 会抛 NullReferenceException）。这里用静态构造函数还原真实语义。
		private static string tempDirectory = Path.GetTempPath();

		static ScenarioScript()
		{
			tempDirectory = tempDirectory.Substring(0, tempDirectory.Length - 1);
		}

		// Token: 0x040000E3 RID: 227
		private static int bgmSlotIndex = 0;

		// Token: 0x040000E4 RID: 228
		private static int voiceSlotIndex = 0;

		// Token: 0x040000E5 RID: 229
		private static int seSlotIndex = 0;
	}
}
