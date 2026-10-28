using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.DirectX.Direct3D;

namespace Yukihota
{
	public class ScenarioScript : IDisposable
	{
		protected Device device;

		protected XmlDocument[] documents = new XmlDocument[3];

		protected XmlNode[] episodeNodes = new XmlNode[3];

		protected XmlNode[] currentNodes = new XmlNode[3];

		protected static readonly string errorMessage = "Script Error";

		protected int episodeNumber;

		protected int maxAlbumIndex;

		protected static ArchiveFile bgArchive = new ArchiveFile("Bg", ArchiveMode.Read);

		protected static ArchiveFile bgmArchive = new ArchiveFile("Bgm", ArchiveMode.Read);

		protected static ArchiveFile charaArchive = new ArchiveFile("Chara", ArchiveMode.Read);

		protected static ArchiveFile scriptArchive = new ArchiveFile("Script", ArchiveMode.Read);

		protected static ArchiveFile seArchive = new ArchiveFile("Se", ArchiveMode.Read);

		protected static ArchiveFile voiceArchive = new ArchiveFile("Voice", ArchiveMode.Read);

		private static string tempDirectory = tempDirectory.Substring(0, tempDirectory.Length - 1);

		private static int bgmSlotIndex = 0;

		private static int voiceSlotIndex = 0;

		private static int seSlotIndex = 0;

		public ScenarioScript(string n0348, int n0246, Device n0349)
		{
			device = n0349;
			for (int i = 0; i < 3; i++)
			{
				documents[i] = new XmlDocument();
			}
			documents[0].Load(scriptArchive.ReadEntry("SC"));
			documents[1].Load(scriptArchive.ReadEntry("TC"));
			documents[2].Load(scriptArchive.ReadEntry("JP"));
			for (int j = 0; j < 3; j++)
			{
				episodeNodes[j] = documents[j].SelectNodes(n0348 + "/episode")[n0246 - 1];
				if (episodeNodes[j] == null)
				{
					throw new Exception(errorMessage);
				}
				currentNodes[j] = episodeNodes[j];
			}
			episodeNumber = n0246;
			maxAlbumIndex = 0;
		}

		~ScenarioScript()
		{
			Dispose();
		}

		public void Dispose()
		{
			for (int i = 0; i < 4; i++)
			{
				try
				{
					if (File.Exists(tempDirectory + "\\bgm" + i + ".wav"))
					{
						File.Delete(tempDirectory + "\\bgm" + i + ".wav");
					}
				}
				catch
				{
				}
				try
				{
					if (File.Exists(tempDirectory + "\\voice" + i + ".wav"))
					{
						File.Delete(tempDirectory + "\\voice" + i + ".wav");
					}
				}
				catch
				{
				}
				try
				{
					if (File.Exists(tempDirectory + "\\se" + i + ".wav"))
					{
						File.Delete(tempDirectory + "\\se" + i + ".wav");
					}
				}
				catch
				{
				}
			}
			GC.SuppressFinalize(this);
		}

		public ScenarioResult Advance(out object n0479, out object n0480, bool instant)
		{
			n0479 = null;
			n0480 = null;
			if (currentNodes[0] == null)
			{
				return ScenarioResult.Error;
			}
			if (currentNodes[0].Name != "paragraph" && currentNodes[0].Name != "dialogue" && currentNodes[0].HasChildNodes)
			{
				for (int i = 0; i < 3; i++)
				{
					if (!currentNodes[i].HasChildNodes)
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					currentNodes[i] = currentNodes[i].FirstChild;
				}
			}
			else if (currentNodes[0].NextSibling != null)
			{
				for (int j = 0; j < 3; j++)
				{
					if (currentNodes[j].NextSibling == null)
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					currentNodes[j] = currentNodes[j].NextSibling;
				}
			}
			else
			{
				bool flag = true;
				while (currentNodes[0].ParentNode != episodeNodes[0])
				{
					if (currentNodes[0].ParentNode.NextSibling != null)
					{
						for (int k = 0; k < 3; k++)
						{
							if (currentNodes[k].ParentNode == null || currentNodes[k].ParentNode.NextSibling == null)
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							currentNodes[k] = currentNodes[k].ParentNode.NextSibling;
						}
						flag = false;
						break;
					}
					for (int l = 0; l < 3; l++)
					{
						if (currentNodes[l].ParentNode == null)
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						currentNodes[l] = currentNodes[l].ParentNode;
					}
				}
				if (flag)
				{
					if (currentNodes[0].ParentNode.NextSibling == null)
					{
						return ScenarioResult.Error;
					}
					for (int m = 0; m < 3; m++)
					{
						if (currentNodes[m].ParentNode == null || currentNodes[m].ParentNode.NextSibling == null)
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						currentNodes[m] = currentNodes[m].ParentNode.NextSibling;
						episodeNodes[m] = currentNodes[m];
					}
					episodeNumber++;
				}
			}
			if (currentNodes[0].Name == "click")
			{
				if (currentNodes[0].PreviousSibling != null && (currentNodes[0].PreviousSibling.Name == "dialogue" || currentNodes[0].PreviousSibling.Name == "paragraph") && currentNodes[0].PreviousSibling.Attributes["voice"] != null)
				{
					n0480 = true;
				}
				else
				{
					n0480 = false;
				}
				return ScenarioResult.Click;
			}
			if (currentNodes[0].Name == "scene")
			{
				if (currentNodes[0].Attributes["background"] != null)
				{
					GC.Collect();
					try
					{
						n0479 = TextureLoader.FromStream(device, bgArchive.ReadEntry(currentNodes[0].Attributes["background"].Value), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						return ScenarioResult.Error;
					}
				}
				else if (currentNodes[0].Attributes["backcolor"] != null)
				{
					try
					{
						n0479 = Color.FromName(currentNodes[0].Attributes["backcolor"].Value);
					}
					catch (Exception ex2)
					{
						MessageBox.Show(ex2.Message);
						return ScenarioResult.Error;
					}
				}
				else
				{
					n0479 = Color.Black;
				}
				if (currentNodes[0].Attributes["time"] == null || currentNodes[0].Attributes["time"].Value == "short")
				{
					n0480 = 1;
				}
				else if (currentNodes[0].Attributes["time"].Value == "long")
				{
					n0480 = 2;
				}
				else
				{
					if (!(currentNodes[0].Attributes["time"].Value == "no"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					n0480 = 0;
				}
				if (currentNodes[0].Attributes["album"] != null)
				{
					try
					{
						int num = int.Parse(currentNodes[0].Attributes["album"].Value);
						if (num > maxAlbumIndex)
						{
							maxAlbumIndex = num;
						}
					}
					catch (Exception ex3)
					{
						MessageBox.Show(ex3.Message);
						return ScenarioResult.Error;
					}
				}
				return ScenarioResult.Scene;
			}
			if (currentNodes[0].Name == "music")
			{
				if ((currentNodes[0].Attributes["mute"] == null || currentNodes[0].Attributes["mute"].Value == "false") && currentNodes[0].Attributes["name"] != null)
				{
					try
					{
						bgmArchive.Extract(currentNodes[0].Attributes["name"].Value, tempDirectory);
						for (int num2 = 0; num2 < 4; num2++)
						{
							bool flag2 = false;
							try
							{
								if (File.Exists(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav"))
								{
									File.Delete(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
								}
							}
							catch (Exception ex4)
							{
								if (num2 >= 3)
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
							bgmSlotIndex = (bgmSlotIndex + 1) % 4;
						}
						File.Move(tempDirectory + "\\" + currentNodes[0].Attributes["name"].Value + ".wav", tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
						GC.Collect();
						n0479 = tempDirectory + "\\bgm" + bgmSlotIndex + ".wav";
						bgmSlotIndex = (bgmSlotIndex + 1) % 4;
					}
					catch (Exception ex5)
					{
						MessageBox.Show(ex5.Message);
						return ScenarioResult.Error;
					}
					if (currentNodes[0].Attributes["position"] != null)
					{
						try
						{
							n0480 = double.Parse(currentNodes[0].Attributes["position"].Value);
							return ScenarioResult.Bgm;
						}
						catch (Exception ex6)
						{
							MessageBox.Show(ex6.Message);
							return ScenarioResult.Error;
						}
					}
					n0480 = 0.0;
					return ScenarioResult.Bgm;
				}
				if (currentNodes[0].Attributes["mute"] != null && currentNodes[0].Attributes["mute"].Value == "true")
				{
					n0479 = null;
					n0480 = 0.0;
					return ScenarioResult.Bgm;
				}
				if (currentNodes[0].Attributes["volume"] == null)
				{
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
				n0479 = null;
				if (currentNodes[0].Attributes["volume"].Value == "down")
				{
					n0480 = false;
					return ScenarioResult.Bgm;
				}
				if (currentNodes[0].Attributes["volume"].Value == "up")
				{
					n0480 = true;
					return ScenarioResult.Bgm;
				}
				MessageBox.Show(errorMessage);
				return ScenarioResult.Error;
			}
			if (currentNodes[0].Name == "chara")
			{
				n0480 = new SpriteState();
				((SpriteState)n0480).level = 0;
				try
				{
					((SpriteState)n0480).index = int.Parse(currentNodes[0].Attributes["index"].Value);
				}
				catch (Exception ex7)
				{
					MessageBox.Show(ex7.Message);
					return ScenarioResult.Error;
				}
				if (currentNodes[0].Attributes["action"] != null && !(currentNodes[0].Attributes["action"].Value == "show"))
				{
					n0479 = null;
					if (currentNodes[0].Attributes["action"].Value == "hide")
					{
						((SpriteState)n0480).action = SpriteMode.Static;
					}
					else if (currentNodes[0].Attributes["action"].Value == "shock")
					{
						((SpriteState)n0480).action = SpriteMode.Shaking;
						if (currentNodes[0].Attributes["level"] == null)
						{
							((SpriteState)n0480).level = 0;
						}
						else
						{
							try
							{
								((SpriteState)n0480).level = int.Parse(currentNodes[0].Attributes["level"].Value);
							}
							catch (Exception ex8)
							{
								MessageBox.Show(ex8.Message);
								return ScenarioResult.Error;
							}
						}
					}
				}
				else
				{
					try
					{
						ImageInformation imageInformation = TextureLoader.ImageInformationFromStream(charaArchive.ReadEntry(currentNodes[0].Attributes["code"].Value));
						n0479 = TextureLoader.FromStream(device, charaArchive.ReadEntry(currentNodes[0].Attributes["code"].Value), imageInformation.Width, imageInformation.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						((SpriteState)n0480).size = new Size(imageInformation.Width, imageInformation.Height);
					}
					catch (Exception ex9)
					{
						MessageBox.Show(ex9.Message);
						return ScenarioResult.Error;
					}
					((SpriteState)n0480).action = SpriteMode.Static;
				}
				if (currentNodes[0].Attributes["location"] != null)
				{
					try
					{
						((SpriteState)n0480).location = int.Parse(currentNodes[0].Attributes["location"].Value);
					}
					catch (Exception ex10)
					{
						MessageBox.Show(ex10.Message);
						return ScenarioResult.Error;
					}
				}
				else
				{
					((SpriteState)n0480).location = int.MaxValue;
				}
				if (currentNodes[0].Attributes["transparent"] == null || currentNodes[0].Attributes["transparent"].Value == "false")
				{
					((SpriteState)n0480).isTransparent = false;
				}
				else
				{
					if (!(currentNodes[0].Attributes["transparent"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((SpriteState)n0480).isTransparent = true;
				}
				if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "true")
				{
					((SpriteState)n0480).isAnimated = true;
				}
				else
				{
					if (!(currentNodes[0].Attributes["animation"].Value == "false"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((SpriteState)n0480).isAnimated = false;
				}
				if (currentNodes[0].Attributes["synchro"] == null)
				{
					((SpriteState)n0480).sync = !((SpriteState)n0480).isAnimated;
				}
				else if (currentNodes[0].Attributes["synchro"].Value == "false")
				{
					((SpriteState)n0480).sync = false;
				}
				else
				{
					if (!(currentNodes[0].Attributes["synchro"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((SpriteState)n0480).sync = true;
				}
				return ScenarioResult.Character;
			}
			if (!(currentNodes[0].Name == "paragraph") && !(currentNodes[0].Name == "dialogue"))
			{
				if (currentNodes[0].Name == "sound")
				{
					n0480 = new InputState();
					if (currentNodes[0].Attributes["loop"] == null || currentNodes[0].Attributes["loop"].Value == "false")
					{
						((InputState)n0480).loop = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["loop"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((InputState)n0480).loop = true;
					}
					if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "true")
					{
						((InputState)n0480).wait = true;
					}
					else
					{
						if (!(currentNodes[0].Attributes["animation"].Value == "false"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((InputState)n0480).wait = false;
					}
					if (currentNodes[0].Attributes["mute"] == null || currentNodes[0].Attributes["mute"].Value == "false")
					{
						if (currentNodes[0].Attributes["name"] == null)
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						try
						{
							seArchive.Extract(currentNodes[0].Attributes["name"].Value, tempDirectory);
							for (int num3 = 0; num3 < 4; num3++)
							{
								bool flag3 = false;
								try
								{
									if (File.Exists(tempDirectory + "\\se" + seSlotIndex + ".wav"))
									{
										File.Delete(tempDirectory + "\\se" + seSlotIndex + ".wav");
									}
								}
								catch (Exception ex11)
								{
									if (num3 >= 3)
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
								seSlotIndex = (seSlotIndex + 1) % 4;
							}
							File.Move(tempDirectory + "\\" + currentNodes[0].Attributes["name"].Value + ".wav", tempDirectory + "\\Se" + seSlotIndex + ".wav");
							GC.Collect();
							n0479 = tempDirectory + "\\se" + seSlotIndex + ".wav";
							seSlotIndex = (seSlotIndex + 1) % 4;
							return ScenarioResult.Sound;
						}
						catch (Exception ex12)
						{
							MessageBox.Show(ex12.Message);
							return ScenarioResult.Error;
						}
					}
					if (!(currentNodes[0].Attributes["mute"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					n0479 = null;
					return ScenarioResult.Sound;
				}
				if (currentNodes[0].Name == "wait")
				{
					if (currentNodes[0].Attributes["time"] == null)
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					if (currentNodes[0].Attributes["time"].Value == "no")
					{
						n0480 = 0;
					}
					else if (currentNodes[0].Attributes["time"].Value == "short")
					{
						n0480 = 1;
					}
					else if (currentNodes[0].Attributes["time"].Value == "middle")
					{
						n0480 = 2;
					}
					else
					{
						if (!(currentNodes[0].Attributes["time"].Value == "long"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						n0480 = 3;
					}
					return ScenarioResult.Jump;
				}
				if (currentNodes[0].Name == "clear")
				{
					if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "false")
					{
						n0480 = false;
						return ScenarioResult.Dismiss;
					}
					XmlNode[] array = currentNodes;
					if (!(array[0].Attributes["animation"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					n0480 = true;
					return ScenarioResult.Dismiss;
				}
				if (currentNodes[0].Name == "title")
				{
					n0479 = new string[3];
					for (int num4 = 0; num4 < 3; num4++)
					{
						if (episodeNodes[num4].Attributes["title"] == null)
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((string[])n0479)[num4] = episodeNodes[num4].Attributes["title"].Value;
					}
					return ScenarioResult.Notice;
				}
				if (currentNodes[0].Name == "filter")
				{
					if (currentNodes[0].Attributes["type"] == null)
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					if (currentNodes[0].Attributes["type"].Value == "zoom")
					{
						n0480 = new AnimItem();
						((AnimItem)n0480).type = DrawItemType.Zoom;
						if (currentNodes[0].Attributes["left"] == null)
						{
							((AnimItem)n0480).left = 0.0;
						}
						else
						{
							try
							{
								((AnimItem)n0480).left = double.Parse(currentNodes[0].Attributes["left"].Value);
							}
							catch (Exception ex13)
							{
								MessageBox.Show(ex13.Message);
								return ScenarioResult.Error;
							}
						}
						if (currentNodes[0].Attributes["right"] == null)
						{
							((AnimItem)n0480).right = 1.0;
						}
						else
						{
							try
							{
								((AnimItem)n0480).right = double.Parse(currentNodes[0].Attributes["right"].Value);
							}
							catch (Exception ex14)
							{
								MessageBox.Show(ex14.Message);
								return ScenarioResult.Error;
							}
						}
						if (currentNodes[0].Attributes["top"] == null)
						{
							((AnimItem)n0480).top = 0.0;
						}
						else
						{
							try
							{
								((AnimItem)n0480).top = double.Parse(currentNodes[0].Attributes["top"].Value);
							}
							catch (Exception ex15)
							{
								MessageBox.Show(ex15.Message);
								return ScenarioResult.Error;
							}
						}
						if (currentNodes[0].Attributes["bottom"] != null)
						{
							try
							{
								((AnimItem)n0480).bottom = double.Parse(currentNodes[0].Attributes["bottom"].Value);
							}
							catch (Exception ex16)
							{
								MessageBox.Show(ex16.Message);
								return ScenarioResult.Error;
							}
						}
						else
						{
							((AnimItem)n0480).bottom = 1.0;
						}
						if (currentNodes[0].Attributes["time"] == null || currentNodes[0].Attributes["time"].Value == "short")
						{
							((AnimItem)n0480).durationMode = 1;
						}
						else
						{
							if (!(currentNodes[0].Attributes["time"].Value == "long"))
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							((AnimItem)n0480).durationMode = 2;
						}
					}
					else if (currentNodes[0].Attributes["type"].Value == "dark")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).type = DrawItemType.Dim;
					}
					else if (currentNodes[0].Attributes["type"].Value == "think")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).type = DrawItemType.Think;
					}
					else if (currentNodes[0].Attributes["type"].Value == "sun")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).type = DrawItemType.Sun;
					}
					else if (currentNodes[0].Attributes["type"].Value == "sakura")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).type = DrawItemType.Petals;
					}
					else if (currentNodes[0].Attributes["type"].Value == "snow")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).type = DrawItemType.Snow;
					}
					else if (currentNodes[0].Attributes["type"].Value == "shock")
					{
						n0480 = new FlagItem();
						((FlagItem)n0480).type = DrawItemType.Shake;
						if (currentNodes[0].Attributes["loop"] != null && !(currentNodes[0].Attributes["loop"].Value == "false"))
						{
							if (currentNodes[0].Attributes["loop"].Value == "true")
							{
								((FlagItem)n0480).loop = true;
							}
						}
						else
						{
							((FlagItem)n0480).loop = false;
						}
					}
					else
					{
						if (!(currentNodes[0].Attributes["type"].Value == "back"))
						{
							n0479 = null;
							n0480 = null;
							return ScenarioResult.None;
						}
						n0480 = new TextureItem();
						((TextureItem)n0480).type = DrawItemType.Background;
						((TextureItem)n0480).texture = TextureLoader.FromStream(device, bgArchive.ReadEntry(currentNodes[0].Attributes["ground"].Value), 0, 0, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						if (currentNodes[0].Attributes["trans"] == null || currentNodes[0].Attributes["trans"].Value == "fade")
						{
							((TextureItem)n0480).blend = TextureBlend.Normal;
						}
						else if (currentNodes[0].Attributes["trans"].Value == "left")
						{
							((TextureItem)n0480).blend = TextureBlend.Left;
						}
						else
						{
							if (!(currentNodes[0].Attributes["trans"].Value == "right"))
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							((TextureItem)n0480).blend = TextureBlend.Right;
						}
						if (currentNodes[0].Attributes["time"] == null || currentNodes[0].Attributes["time"].Value == "short")
						{
							((TextureItem)n0480).durationMode = 1;
						}
						else if (currentNodes[0].Attributes["time"].Value == "long")
						{
							((TextureItem)n0480).durationMode = 2;
						}
						else
						{
							if (!(currentNodes[0].Attributes["time"].Value == "no"))
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							((TextureItem)n0480).durationMode = 0;
						}
						if (currentNodes[0].Attributes["album"] != null)
						{
							try
							{
								int num5 = int.Parse(currentNodes[0].Attributes["album"].Value);
								if (num5 > maxAlbumIndex)
								{
									maxAlbumIndex = num5;
								}
							}
							catch (Exception ex17)
							{
								MessageBox.Show(ex17.Message);
								return ScenarioResult.Error;
							}
						}
					}
					if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "false")
					{
						object obj = n0480;
						((DrawItem)obj).isAnimated = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["animation"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((DrawItem)n0480).isAnimated = true;
					}
					if (currentNodes[0].Attributes["synchro"] == null)
					{
						((DrawItem)n0480).sync = !((DrawItem)n0480).isAnimated;
					}
					else if (currentNodes[0].Attributes["synchro"].Value == "false")
					{
						((DrawItem)n0480).sync = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["synchro"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((DrawItem)n0480).sync = true;
					}
					if (currentNodes[0].Attributes["finish"] == null || currentNodes[0].Attributes["finish"].Value == "false")
					{
						((DrawItem)n0480).finish = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["finish"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((DrawItem)n0480).finish = true;
					}
					return ScenarioResult.Draw;
				}
				if (currentNodes[0].Name == "ed")
				{
					bgmArchive.Extract("BGM37", tempDirectory);
					for (int num6 = 0; num6 < 4; num6++)
					{
						bool flag4 = false;
						try
						{
							if (File.Exists(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav"))
							{
								File.Delete(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
							}
						}
						catch (Exception ex18)
						{
							if (num6 >= 3)
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
						bgmSlotIndex = (bgmSlotIndex + 1) % 4;
					}
					File.Move(tempDirectory + "\\BGM37.wav", tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
					GC.Collect();
					string n536 = tempDirectory + "\\bgm" + bgmSlotIndex + ".wav";
					bgmSlotIndex = (bgmSlotIndex + 1) % 4;
					XmlDocument[] array2 = new XmlDocument[3];
					for (int num7 = 0; num7 < 3; num7++)
					{
						array2[num7] = new XmlDocument();
					}
					array2[0].Load(scriptArchive.ReadEntry("ED_SC"));
					array2[1].Load(scriptArchive.ReadEntry("ED_TC"));
					array2[2].Load(scriptArchive.ReadEntry("ED_JP"));
					StaffRoll n537 = new StaffRoll(n536, charaArchive, bgArchive, array2);
					n0479 = n537;
					return ScenarioResult.StaffRoll;
				}
				return ScenarioResult.None;
			}
			if (!currentNodes[0].HasChildNodes)
			{
				n0479 = null;
			}
			else
			{
				n0479 = new string[3];
				for (int num8 = 0; num8 < 3; num8++)
				{
					string[] array3 = (string[])n0479;
					array3[num8] = "";
					if (currentNodes[num8].HasChildNodes)
					{
						for (int num9 = 0; num9 < currentNodes[num8].ChildNodes.Count; num9++)
						{
							if (currentNodes[num8].ChildNodes[num9].Name == "br")
							{
								if (currentNodes[num8].ChildNodes[num9].Attributes["line"] == null)
								{
									string[] array4;
									IntPtr intPtr;
									(array4 = (string[])n0479)[(int)(intPtr = (IntPtr)num8)] = array4[(int)intPtr] + "\n";
									continue;
								}
								int num10 = 1;
								try
								{
									num10 = int.Parse(currentNodes[num8].ChildNodes[num9].Attributes["line"].Value);
								}
								catch (Exception ex19)
								{
									MessageBox.Show(ex19.Message);
									return ScenarioResult.Error;
								}
								for (int num11 = 0; num11 < num10; num11++)
								{
									string[] array5;
									IntPtr intPtr2;
									(array5 = (string[])n0479)[(int)(intPtr2 = (IntPtr)num8)] = array5[(int)intPtr2] + "\n";
								}
							}
							else
							{
								if (currentNodes[num8].ChildNodes[num9].NodeType != XmlNodeType.Text)
								{
									MessageBox.Show(errorMessage);
									return ScenarioResult.Error;
								}
								string[] array6;
								IntPtr intPtr3;
								(array6 = (string[])n0479)[(int)(intPtr3 = (IntPtr)num8)] = array6[(int)intPtr3] + currentNodes[num8].ChildNodes[num9].Value;
							}
						}
						continue;
					}
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
			}
			string n538 = null;
			bool n539 = false;
			if (currentNodes[0].Attributes["voice"] != null)
			{
				if (instant)
				{
					n539 = true;
				}
				else
				{
					try
					{
						voiceArchive.Extract(currentNodes[0].Attributes["voice"].Value, tempDirectory);
						for (int num12 = 0; num12 < 4; num12++)
						{
							bool flag5 = false;
							try
							{
								if (File.Exists(tempDirectory + "\\voice" + voiceSlotIndex + ".wav"))
								{
									File.Delete(tempDirectory + "\\voice" + voiceSlotIndex + ".wav");
								}
							}
							catch (Exception ex20)
							{
								if (num12 >= 3)
								{
									MessageBox.Show(ex20.Message);
									return ScenarioResult.Error;
								}
								flag5 = true;
							}
							if (!flag5)
							{
								break;
							}
							voiceSlotIndex = (voiceSlotIndex + 1) % 4;
						}
						File.Move(tempDirectory + "\\" + currentNodes[0].Attributes["voice"].Value + ".wav", tempDirectory + "\\voice" + voiceSlotIndex + ".wav");
						n538 = tempDirectory + "\\voice" + voiceSlotIndex + ".wav";
						voiceSlotIndex = (voiceSlotIndex + 1) % 4;
					}
					catch (Exception ex21)
					{
						MessageBox.Show(ex21.Message);
						return ScenarioResult.Error;
					}
				}
			}
			if (currentNodes[0].Name == "paragraph")
			{
				n0480 = new MessageCommand();
				if (currentNodes[0].Attributes["align"] != null && !(currentNodes[0].Attributes["align"].Value == "left"))
				{
					if (currentNodes[0].Attributes["align"].Value == "center")
					{
						((MessageCommand)n0480).align = HorizontalAlign.Center;
					}
					else
					{
						if (!(currentNodes[0].Attributes["align"].Value == "right"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((MessageCommand)n0480).align = HorizontalAlign.Right;
					}
				}
				else
				{
					((MessageCommand)n0480).align = HorizontalAlign.Left;
				}
				if (currentNodes[0].Attributes["show"] == null || currentNodes[0].Attributes["show"].Value == "print")
				{
					((MessageCommand)n0480).showMode = TextRevealMode.Typewriter;
				}
				else if (currentNodes[0].Attributes["show"].Value == "fade")
				{
					((MessageCommand)n0480).showMode = TextRevealMode.Fade;
				}
				if (currentNodes[0].Attributes["fore"] == null)
				{
					((MessageCommand)n0480).foreColor = Color.White;
				}
				else
				{
					try
					{
						((MessageCommand)n0480).foreColor = Color.FromName(currentNodes[0].Attributes["fore"].Value);
					}
					catch (Exception ex22)
					{
						MessageBox.Show(ex22.Message);
						return ScenarioResult.Error;
					}
				}
				if (currentNodes[0].Attributes["back"] != null)
				{
					try
					{
						((MessageCommand)n0480).shadowColor = Color.FromName(currentNodes[0].Attributes["back"].Value);
					}
					catch (Exception ex23)
					{
						MessageBox.Show(ex23.Message);
						return ScenarioResult.Error;
					}
				}
				else
				{
					((MessageCommand)n0480).shadowColor = Color.Black;
				}
				XmlNode nextSibling = currentNodes[0].NextSibling;
				while (nextSibling != null && nextSibling.NodeType == XmlNodeType.Comment)
				{
					nextSibling = nextSibling.NextSibling;
				}
				if (nextSibling != null && nextSibling.Name == "click")
				{
					((MessageCommand)n0480).waitClick = true;
				}
				else
				{
					((MessageCommand)n0480).waitClick = false;
				}
				((MessageCommand)n0480).voice = n538;
				((MessageCommand)n0480).instant = n539;
				return ScenarioResult.Message;
			}
			n0480 = new CaptionCommand();
			if (currentNodes[0].Attributes["chara"] != null)
			{
				for (int num13 = 0; num13 < 3; num13++)
				{
					if (currentNodes[num13].Attributes["chara"] == null)
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((CaptionCommand)n0480).charaNames[num13] = currentNodes[num13].Attributes["chara"].Value;
				}
			}
			else
			{
				((CaptionCommand)n0480).charaNames = null;
			}
			if (currentNodes[0].Attributes["disappear"] == null || currentNodes[0].Attributes["disappear"].Value == "false")
			{
				((CaptionCommand)n0480).disappear = false;
			}
			else
			{
				if (!(currentNodes[0].Attributes["disappear"].Value == "true"))
				{
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
				((CaptionCommand)n0480).disappear = true;
			}
			if (currentNodes[0].Attributes["continue"] == null || currentNodes[0].Attributes["continue"].Value == "false")
			{
				((CaptionCommand)n0480).isContinuous = false;
			}
			else
			{
				if (!(currentNodes[0].Attributes["continue"].Value == "true"))
				{
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
				((CaptionCommand)n0480).isContinuous = true;
			}
			((CaptionCommand)n0480).hideCharaName = ((CaptionCommand)n0480).disappear;
			if (((CaptionCommand)n0480).charaNames != null)
			{
				XmlNode nextSibling2 = currentNodes[0].NextSibling;
				while (nextSibling2 != null && nextSibling2.Name != "dialogue")
				{
					nextSibling2 = nextSibling2.NextSibling;
				}
				if (nextSibling2 != null)
				{
					if (nextSibling2.Attributes["chara"] == null)
					{
						((CaptionCommand)n0480).hideCharaName = true;
					}
				}
				else
				{
					((CaptionCommand)n0480).hideCharaName = true;
				}
			}
			((CaptionCommand)n0480).voice = n538;
			((CaptionCommand)n0480).instant = n539;
			return ScenarioResult.Caption;
		}

		public int n0157()
		{
			return episodeNumber;
		}

		public int n0159()
		{
			return maxAlbumIndex;
		}
	}
}
