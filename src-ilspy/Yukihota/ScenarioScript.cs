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

		public ScenarioScript(string scriptPath, int episode, Device device)
		{
			device = device;
			for (int i = 0; i < 3; i++)
			{
				documents[i] = new XmlDocument();
			}
			documents[0].Load(scriptArchive.ReadEntry("SC"));
			documents[1].Load(scriptArchive.ReadEntry("TC"));
			documents[2].Load(scriptArchive.ReadEntry("JP"));
			for (int j = 0; j < 3; j++)
			{
				episodeNodes[j] = documents[j].SelectNodes(scriptPath + "/episode")[episode - 1];
				if (episodeNodes[j] == null)
				{
					throw new Exception(errorMessage);
				}
				currentNodes[j] = episodeNodes[j];
			}
			episodeNumber = episode;
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

		public ScenarioResult Advance(out object resultData, out object resultOption, bool instant)
		{
			resultData = null;
			resultOption = null;
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
				bool found = true;
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
						found = false;
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
				if (found)
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
					resultOption = true;
				}
				else
				{
					resultOption = false;
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
						resultData = TextureLoader.FromStream(device, bgArchive.ReadEntry(currentNodes[0].Attributes["background"].Value), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
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
						resultData = Color.FromName(currentNodes[0].Attributes["backcolor"].Value);
					}
					catch (Exception ex2)
					{
						MessageBox.Show(ex2.Message);
						return ScenarioResult.Error;
					}
				}
				else
				{
					resultData = Color.Black;
				}
				if (currentNodes[0].Attributes["time"] == null || currentNodes[0].Attributes["time"].Value == "short")
				{
					resultOption = 1;
				}
				else if (currentNodes[0].Attributes["time"].Value == "long")
				{
					resultOption = 2;
				}
				else
				{
					if (!(currentNodes[0].Attributes["time"].Value == "no"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					resultOption = 0;
				}
				if (currentNodes[0].Attributes["album"] != null)
				{
					try
					{
						int albumIndex = int.Parse(currentNodes[0].Attributes["album"].Value);
						if (albumIndex > maxAlbumIndex)
						{
							maxAlbumIndex = albumIndex;
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
						for (int attempt = 0; attempt < 4; attempt++)
						{
							bool retry = false;
							try
							{
								if (File.Exists(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav"))
								{
									File.Delete(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
								}
							}
							catch (Exception ex4)
							{
								if (attempt >= 3)
								{
									MessageBox.Show(ex4.Message);
									return ScenarioResult.Error;
								}
								retry = true;
							}
							if (!retry)
							{
								break;
							}
							bgmSlotIndex = (bgmSlotIndex + 1) % 4;
						}
						File.Move(tempDirectory + "\\" + currentNodes[0].Attributes["name"].Value + ".wav", tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
						GC.Collect();
						resultData = tempDirectory + "\\bgm" + bgmSlotIndex + ".wav";
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
							resultOption = double.Parse(currentNodes[0].Attributes["position"].Value);
							return ScenarioResult.Bgm;
						}
						catch (Exception ex6)
						{
							MessageBox.Show(ex6.Message);
							return ScenarioResult.Error;
						}
					}
					resultOption = 0.0;
					return ScenarioResult.Bgm;
				}
				if (currentNodes[0].Attributes["mute"] != null && currentNodes[0].Attributes["mute"].Value == "true")
				{
					resultData = null;
					resultOption = 0.0;
					return ScenarioResult.Bgm;
				}
				if (currentNodes[0].Attributes["volume"] == null)
				{
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
				resultData = null;
				if (currentNodes[0].Attributes["volume"].Value == "down")
				{
					resultOption = false;
					return ScenarioResult.Bgm;
				}
				if (currentNodes[0].Attributes["volume"].Value == "up")
				{
					resultOption = true;
					return ScenarioResult.Bgm;
				}
				MessageBox.Show(errorMessage);
				return ScenarioResult.Error;
			}
			if (currentNodes[0].Name == "chara")
			{
				resultOption = new SpriteState();
				((SpriteState)resultOption).level = 0;
				try
				{
					((SpriteState)resultOption).index = int.Parse(currentNodes[0].Attributes["index"].Value);
				}
				catch (Exception ex7)
				{
					MessageBox.Show(ex7.Message);
					return ScenarioResult.Error;
				}
				if (currentNodes[0].Attributes["action"] != null && !(currentNodes[0].Attributes["action"].Value == "show"))
				{
					resultData = null;
					if (currentNodes[0].Attributes["action"].Value == "hide")
					{
						((SpriteState)resultOption).action = SpriteMode.Static;
					}
					else if (currentNodes[0].Attributes["action"].Value == "shock")
					{
						((SpriteState)resultOption).action = SpriteMode.Shaking;
						if (currentNodes[0].Attributes["level"] == null)
						{
							((SpriteState)resultOption).level = 0;
						}
						else
						{
							try
							{
								((SpriteState)resultOption).level = int.Parse(currentNodes[0].Attributes["level"].Value);
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
						resultData = TextureLoader.FromStream(device, charaArchive.ReadEntry(currentNodes[0].Attributes["code"].Value), imageInformation.Width, imageInformation.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						((SpriteState)resultOption).size = new Size(imageInformation.Width, imageInformation.Height);
					}
					catch (Exception ex9)
					{
						MessageBox.Show(ex9.Message);
						return ScenarioResult.Error;
					}
					((SpriteState)resultOption).action = SpriteMode.Static;
				}
				if (currentNodes[0].Attributes["location"] != null)
				{
					try
					{
						((SpriteState)resultOption).location = int.Parse(currentNodes[0].Attributes["location"].Value);
					}
					catch (Exception ex10)
					{
						MessageBox.Show(ex10.Message);
						return ScenarioResult.Error;
					}
				}
				else
				{
					((SpriteState)resultOption).location = int.MaxValue;
				}
				if (currentNodes[0].Attributes["transparent"] == null || currentNodes[0].Attributes["transparent"].Value == "false")
				{
					((SpriteState)resultOption).isTransparent = false;
				}
				else
				{
					if (!(currentNodes[0].Attributes["transparent"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((SpriteState)resultOption).isTransparent = true;
				}
				if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "true")
				{
					((SpriteState)resultOption).isAnimated = true;
				}
				else
				{
					if (!(currentNodes[0].Attributes["animation"].Value == "false"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((SpriteState)resultOption).isAnimated = false;
				}
				if (currentNodes[0].Attributes["synchro"] == null)
				{
					((SpriteState)resultOption).sync = !((SpriteState)resultOption).isAnimated;
				}
				else if (currentNodes[0].Attributes["synchro"].Value == "false")
				{
					((SpriteState)resultOption).sync = false;
				}
				else
				{
					if (!(currentNodes[0].Attributes["synchro"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((SpriteState)resultOption).sync = true;
				}
				return ScenarioResult.Character;
			}
			if (!(currentNodes[0].Name == "paragraph") && !(currentNodes[0].Name == "dialogue"))
			{
				if (currentNodes[0].Name == "sound")
				{
					resultOption = new InputState();
					if (currentNodes[0].Attributes["loop"] == null || currentNodes[0].Attributes["loop"].Value == "false")
					{
						((InputState)resultOption).loop = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["loop"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((InputState)resultOption).loop = true;
					}
					if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "true")
					{
						((InputState)resultOption).wait = true;
					}
					else
					{
						if (!(currentNodes[0].Attributes["animation"].Value == "false"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((InputState)resultOption).wait = false;
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
							for (int attempt = 0; attempt < 4; attempt++)
							{
								bool retry = false;
								try
								{
									if (File.Exists(tempDirectory + "\\se" + seSlotIndex + ".wav"))
									{
										File.Delete(tempDirectory + "\\se" + seSlotIndex + ".wav");
									}
								}
								catch (Exception ex11)
								{
									if (attempt >= 3)
									{
										MessageBox.Show(ex11.Message);
										return ScenarioResult.Error;
									}
									retry = true;
								}
								if (!retry)
								{
									break;
								}
								seSlotIndex = (seSlotIndex + 1) % 4;
							}
							File.Move(tempDirectory + "\\" + currentNodes[0].Attributes["name"].Value + ".wav", tempDirectory + "\\Se" + seSlotIndex + ".wav");
							GC.Collect();
							resultData = tempDirectory + "\\se" + seSlotIndex + ".wav";
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
					resultData = null;
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
						resultOption = 0;
					}
					else if (currentNodes[0].Attributes["time"].Value == "short")
					{
						resultOption = 1;
					}
					else if (currentNodes[0].Attributes["time"].Value == "middle")
					{
						resultOption = 2;
					}
					else
					{
						if (!(currentNodes[0].Attributes["time"].Value == "long"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						resultOption = 3;
					}
					return ScenarioResult.Jump;
				}
				if (currentNodes[0].Name == "clear")
				{
					if (currentNodes[0].Attributes["animation"] == null || currentNodes[0].Attributes["animation"].Value == "false")
					{
						resultOption = false;
						return ScenarioResult.Dismiss;
					}
					XmlNode[] array = currentNodes;
					if (!(array[0].Attributes["animation"].Value == "true"))
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					resultOption = true;
					return ScenarioResult.Dismiss;
				}
				if (currentNodes[0].Name == "title")
				{
					resultData = new string[3];
					for (int languageIndex = 0; languageIndex < 3; languageIndex++)
					{
						if (episodeNodes[languageIndex].Attributes["title"] == null)
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((string[])resultData)[languageIndex] = episodeNodes[languageIndex].Attributes["title"].Value;
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
						resultOption = new AnimItem();
						((AnimItem)resultOption).type = DrawItemType.Zoom;
						if (currentNodes[0].Attributes["left"] == null)
						{
							((AnimItem)resultOption).left = 0.0;
						}
						else
						{
							try
							{
								((AnimItem)resultOption).left = double.Parse(currentNodes[0].Attributes["left"].Value);
							}
							catch (Exception ex13)
							{
								MessageBox.Show(ex13.Message);
								return ScenarioResult.Error;
							}
						}
						if (currentNodes[0].Attributes["right"] == null)
						{
							((AnimItem)resultOption).right = 1.0;
						}
						else
						{
							try
							{
								((AnimItem)resultOption).right = double.Parse(currentNodes[0].Attributes["right"].Value);
							}
							catch (Exception ex14)
							{
								MessageBox.Show(ex14.Message);
								return ScenarioResult.Error;
							}
						}
						if (currentNodes[0].Attributes["top"] == null)
						{
							((AnimItem)resultOption).top = 0.0;
						}
						else
						{
							try
							{
								((AnimItem)resultOption).top = double.Parse(currentNodes[0].Attributes["top"].Value);
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
								((AnimItem)resultOption).bottom = double.Parse(currentNodes[0].Attributes["bottom"].Value);
							}
							catch (Exception ex16)
							{
								MessageBox.Show(ex16.Message);
								return ScenarioResult.Error;
							}
						}
						else
						{
							((AnimItem)resultOption).bottom = 1.0;
						}
						if (currentNodes[0].Attributes["time"] == null || currentNodes[0].Attributes["time"].Value == "short")
						{
							((AnimItem)resultOption).durationMode = 1;
						}
						else
						{
							if (!(currentNodes[0].Attributes["time"].Value == "long"))
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							((AnimItem)resultOption).durationMode = 2;
						}
					}
					else if (currentNodes[0].Attributes["type"].Value == "dark")
					{
						resultOption = new DrawItem();
						((DrawItem)resultOption).type = DrawItemType.Dim;
					}
					else if (currentNodes[0].Attributes["type"].Value == "think")
					{
						resultOption = new DrawItem();
						((DrawItem)resultOption).type = DrawItemType.Think;
					}
					else if (currentNodes[0].Attributes["type"].Value == "sun")
					{
						resultOption = new DrawItem();
						((DrawItem)resultOption).type = DrawItemType.Sun;
					}
					else if (currentNodes[0].Attributes["type"].Value == "sakura")
					{
						resultOption = new DrawItem();
						((DrawItem)resultOption).type = DrawItemType.Petals;
					}
					else if (currentNodes[0].Attributes["type"].Value == "snow")
					{
						resultOption = new DrawItem();
						((DrawItem)resultOption).type = DrawItemType.Snow;
					}
					else if (currentNodes[0].Attributes["type"].Value == "shock")
					{
						resultOption = new FlagItem();
						((FlagItem)resultOption).type = DrawItemType.Shake;
						if (currentNodes[0].Attributes["loop"] != null && !(currentNodes[0].Attributes["loop"].Value == "false"))
						{
							if (currentNodes[0].Attributes["loop"].Value == "true")
							{
								((FlagItem)resultOption).loop = true;
							}
						}
						else
						{
							((FlagItem)resultOption).loop = false;
						}
					}
					else
					{
						if (!(currentNodes[0].Attributes["type"].Value == "back"))
						{
							resultData = null;
							resultOption = null;
							return ScenarioResult.None;
						}
						resultOption = new TextureItem();
						((TextureItem)resultOption).type = DrawItemType.Background;
						((TextureItem)resultOption).texture = TextureLoader.FromStream(device, bgArchive.ReadEntry(currentNodes[0].Attributes["ground"].Value), 0, 0, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						if (currentNodes[0].Attributes["trans"] == null || currentNodes[0].Attributes["trans"].Value == "fade")
						{
							((TextureItem)resultOption).blend = TextureBlend.Normal;
						}
						else if (currentNodes[0].Attributes["trans"].Value == "left")
						{
							((TextureItem)resultOption).blend = TextureBlend.Left;
						}
						else
						{
							if (!(currentNodes[0].Attributes["trans"].Value == "right"))
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							((TextureItem)resultOption).blend = TextureBlend.Right;
						}
						if (currentNodes[0].Attributes["time"] == null || currentNodes[0].Attributes["time"].Value == "short")
						{
							((TextureItem)resultOption).durationMode = 1;
						}
						else if (currentNodes[0].Attributes["time"].Value == "long")
						{
							((TextureItem)resultOption).durationMode = 2;
						}
						else
						{
							if (!(currentNodes[0].Attributes["time"].Value == "no"))
							{
								MessageBox.Show(errorMessage);
								return ScenarioResult.Error;
							}
							((TextureItem)resultOption).durationMode = 0;
						}
						if (currentNodes[0].Attributes["album"] != null)
						{
							try
							{
								int albumIndex = int.Parse(currentNodes[0].Attributes["album"].Value);
								if (albumIndex > maxAlbumIndex)
								{
									maxAlbumIndex = albumIndex;
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
						object obj = resultOption;
						((DrawItem)obj).isAnimated = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["animation"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((DrawItem)resultOption).isAnimated = true;
					}
					if (currentNodes[0].Attributes["synchro"] == null)
					{
						((DrawItem)resultOption).sync = !((DrawItem)resultOption).isAnimated;
					}
					else if (currentNodes[0].Attributes["synchro"].Value == "false")
					{
						((DrawItem)resultOption).sync = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["synchro"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((DrawItem)resultOption).sync = true;
					}
					if (currentNodes[0].Attributes["finish"] == null || currentNodes[0].Attributes["finish"].Value == "false")
					{
						((DrawItem)resultOption).finish = false;
					}
					else
					{
						if (!(currentNodes[0].Attributes["finish"].Value == "true"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((DrawItem)resultOption).finish = true;
					}
					return ScenarioResult.Draw;
				}
				if (currentNodes[0].Name == "ed")
				{
					bgmArchive.Extract("BGM37", tempDirectory);
					for (int attempt = 0; attempt < 4; attempt++)
					{
						bool retry = false;
						try
						{
							if (File.Exists(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav"))
							{
								File.Delete(tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
							}
						}
						catch (Exception ex18)
						{
							if (attempt >= 3)
							{
								MessageBox.Show(ex18.Message);
								return ScenarioResult.Error;
							}
							retry = true;
						}
						if (!retry)
						{
							break;
						}
						bgmSlotIndex = (bgmSlotIndex + 1) % 4;
					}
					File.Move(tempDirectory + "\\BGM37.wav", tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
					GC.Collect();
					string instance = tempDirectory + "\\bgm" + bgmSlotIndex + ".wav";
					bgmSlotIndex = (bgmSlotIndex + 1) % 4;
					XmlDocument[] array2 = new XmlDocument[3];
					for (int languageIndex = 0; languageIndex < 3; languageIndex++)
					{
						array2[languageIndex] = new XmlDocument();
					}
					array2[0].Load(scriptArchive.ReadEntry("ED_SC"));
					array2[1].Load(scriptArchive.ReadEntry("ED_TC"));
					array2[2].Load(scriptArchive.ReadEntry("ED_JP"));
					StaffRoll inputInfo = new StaffRoll(instance, charaArchive, bgArchive, array2);
					resultData = inputInfo;
					return ScenarioResult.StaffRoll;
				}
				return ScenarioResult.None;
			}
			if (!currentNodes[0].HasChildNodes)
			{
				resultData = null;
			}
			else
			{
				resultData = new string[3];
				for (int languageIndex = 0; languageIndex < 3; languageIndex++)
				{
					string[] array3 = (string[])resultData;
					array3[languageIndex] = "";
					if (currentNodes[languageIndex].HasChildNodes)
					{
						for (int childIndex = 0; childIndex < currentNodes[languageIndex].ChildNodes.Count; childIndex++)
						{
							if (currentNodes[languageIndex].ChildNodes[childIndex].Name == "br")
							{
								if (currentNodes[languageIndex].ChildNodes[childIndex].Attributes["line"] == null)
								{
									string[] array4;
									IntPtr intPtr;
									(array4 = (string[])resultData)[(int)(intPtr = (IntPtr)languageIndex)] = array4[(int)intPtr] + "\n";
									continue;
								}
								int lineCount = 1;
								try
								{
									lineCount = int.Parse(currentNodes[languageIndex].ChildNodes[childIndex].Attributes["line"].Value);
								}
								catch (Exception ex19)
								{
									MessageBox.Show(ex19.Message);
									return ScenarioResult.Error;
								}
								for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
								{
									string[] array5;
									IntPtr intPtr2;
									(array5 = (string[])resultData)[(int)(intPtr2 = (IntPtr)languageIndex)] = array5[(int)intPtr2] + "\n";
								}
							}
							else
							{
								if (currentNodes[languageIndex].ChildNodes[childIndex].NodeType != XmlNodeType.Text)
								{
									MessageBox.Show(errorMessage);
									return ScenarioResult.Error;
								}
								string[] array6;
								IntPtr intPtr3;
								(array6 = (string[])resultData)[(int)(intPtr3 = (IntPtr)languageIndex)] = array6[(int)intPtr3] + currentNodes[languageIndex].ChildNodes[childIndex].Value;
							}
						}
						continue;
					}
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
			}
			string voicePath = null;
			bool hasVoice = false;
			if (currentNodes[0].Attributes["voice"] != null)
			{
				if (instant)
				{
					hasVoice = true;
				}
				else
				{
					try
					{
						voiceArchive.Extract(currentNodes[0].Attributes["voice"].Value, tempDirectory);
						for (int attempt = 0; attempt < 4; attempt++)
						{
							bool retry = false;
							try
							{
								if (File.Exists(tempDirectory + "\\voice" + voiceSlotIndex + ".wav"))
								{
									File.Delete(tempDirectory + "\\voice" + voiceSlotIndex + ".wav");
								}
							}
							catch (Exception ex20)
							{
								if (attempt >= 3)
								{
									MessageBox.Show(ex20.Message);
									return ScenarioResult.Error;
								}
								retry = true;
							}
							if (!retry)
							{
								break;
							}
							voiceSlotIndex = (voiceSlotIndex + 1) % 4;
						}
						File.Move(tempDirectory + "\\" + currentNodes[0].Attributes["voice"].Value + ".wav", tempDirectory + "\\voice" + voiceSlotIndex + ".wav");
						voicePath = tempDirectory + "\\voice" + voiceSlotIndex + ".wav";
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
				resultOption = new MessageCommand();
				if (currentNodes[0].Attributes["align"] != null && !(currentNodes[0].Attributes["align"].Value == "left"))
				{
					if (currentNodes[0].Attributes["align"].Value == "center")
					{
						((MessageCommand)resultOption).align = HorizontalAlign.Center;
					}
					else
					{
						if (!(currentNodes[0].Attributes["align"].Value == "right"))
						{
							MessageBox.Show(errorMessage);
							return ScenarioResult.Error;
						}
						((MessageCommand)resultOption).align = HorizontalAlign.Right;
					}
				}
				else
				{
					((MessageCommand)resultOption).align = HorizontalAlign.Left;
				}
				if (currentNodes[0].Attributes["show"] == null || currentNodes[0].Attributes["show"].Value == "print")
				{
					((MessageCommand)resultOption).showMode = TextRevealMode.Typewriter;
				}
				else if (currentNodes[0].Attributes["show"].Value == "fade")
				{
					((MessageCommand)resultOption).showMode = TextRevealMode.Fade;
				}
				if (currentNodes[0].Attributes["fore"] == null)
				{
					((MessageCommand)resultOption).foreColor = Color.White;
				}
				else
				{
					try
					{
						((MessageCommand)resultOption).foreColor = Color.FromName(currentNodes[0].Attributes["fore"].Value);
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
						((MessageCommand)resultOption).shadowColor = Color.FromName(currentNodes[0].Attributes["back"].Value);
					}
					catch (Exception ex23)
					{
						MessageBox.Show(ex23.Message);
						return ScenarioResult.Error;
					}
				}
				else
				{
					((MessageCommand)resultOption).shadowColor = Color.Black;
				}
				XmlNode nextSibling = currentNodes[0].NextSibling;
				while (nextSibling != null && nextSibling.NodeType == XmlNodeType.Comment)
				{
					nextSibling = nextSibling.NextSibling;
				}
				if (nextSibling != null && nextSibling.Name == "click")
				{
					((MessageCommand)resultOption).waitClick = true;
				}
				else
				{
					((MessageCommand)resultOption).waitClick = false;
				}
				((MessageCommand)resultOption).voice = voicePath;
				((MessageCommand)resultOption).instant = hasVoice;
				return ScenarioResult.Message;
			}
			resultOption = new CaptionCommand();
			if (currentNodes[0].Attributes["chara"] != null)
			{
				for (int languageIndex = 0; languageIndex < 3; languageIndex++)
				{
					if (currentNodes[languageIndex].Attributes["chara"] == null)
					{
						MessageBox.Show(errorMessage);
						return ScenarioResult.Error;
					}
					((CaptionCommand)resultOption).charaNames[languageIndex] = currentNodes[languageIndex].Attributes["chara"].Value;
				}
			}
			else
			{
				((CaptionCommand)resultOption).charaNames = null;
			}
			if (currentNodes[0].Attributes["disappear"] == null || currentNodes[0].Attributes["disappear"].Value == "false")
			{
				((CaptionCommand)resultOption).disappear = false;
			}
			else
			{
				if (!(currentNodes[0].Attributes["disappear"].Value == "true"))
				{
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
				((CaptionCommand)resultOption).disappear = true;
			}
			if (currentNodes[0].Attributes["continue"] == null || currentNodes[0].Attributes["continue"].Value == "false")
			{
				((CaptionCommand)resultOption).isContinuous = false;
			}
			else
			{
				if (!(currentNodes[0].Attributes["continue"].Value == "true"))
				{
					MessageBox.Show(errorMessage);
					return ScenarioResult.Error;
				}
				((CaptionCommand)resultOption).isContinuous = true;
			}
			((CaptionCommand)resultOption).hideCharaName = ((CaptionCommand)resultOption).disappear;
			if (((CaptionCommand)resultOption).charaNames != null)
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
						((CaptionCommand)resultOption).hideCharaName = true;
					}
				}
				else
				{
					((CaptionCommand)resultOption).hideCharaName = true;
				}
			}
			((CaptionCommand)resultOption).voice = voicePath;
			((CaptionCommand)resultOption).instant = hasVoice;
			return ScenarioResult.Caption;
		}

		public int GetEpisode()
		{
			return episodeNumber;
		}

		public int GetAlbumIndex()
		{
			return maxAlbumIndex;
		}
	}
}
