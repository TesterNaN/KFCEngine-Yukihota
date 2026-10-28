using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Microsoft.DirectX.Direct3D;

namespace n0001
{
	public class ScenarioScript : IDisposable
	{
		protected Device n0133;

		protected XmlDocument[] n0472 = new XmlDocument[3];

		protected XmlNode[] n0158 = new XmlNode[3];

		protected XmlNode[] n0475 = new XmlNode[3];

		protected static readonly string n0474 = "Script Error";

		protected int n0476;

		protected int n0477;

		protected static ArchiveFile n0153 = new ArchiveFile("Bg", ArchiveMode.n0151);

		protected static ArchiveFile n0152 = new ArchiveFile("Bgm", ArchiveMode.n0151);

		protected static ArchiveFile n0482 = new ArchiveFile("Chara", ArchiveMode.n0151);

		protected static ArchiveFile n0473 = new ArchiveFile("Script", ArchiveMode.n0151);

		protected static ArchiveFile n0483 = new ArchiveFile("Se", ArchiveMode.n0151);

		protected static ArchiveFile n0488 = new ArchiveFile("Voice", ArchiveMode.n0151);

		private static string tempDirectory = tempDirectory.Substring(0, tempDirectory.Length - 1);

		private static int bgmSlotIndex = 0;

		private static int voiceSlotIndex = 0;

		private static int seSlotIndex = 0;

		public ScenarioScript(string n0348, int n0246, Device n0349)
		{
			n0133 = n0349;
			for (int i = 0; i < 3; i++)
			{
				n0472[i] = new XmlDocument();
			}
			n0472[0].Load(n0473.ReadEntry("SC"));
			n0472[1].Load(n0473.ReadEntry("TC"));
			n0472[2].Load(n0473.ReadEntry("JP"));
			for (int j = 0; j < 3; j++)
			{
				n0158[j] = n0472[j].SelectNodes(n0348 + "/episode")[n0246 - 1];
				if (n0158[j] == null)
				{
					throw new Exception(n0474);
				}
				n0475[j] = n0158[j];
			}
			n0476 = n0246;
			n0477 = 0;
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

		public ScenarioResult Advance(out object n0479, out object n0480, bool n0168)
		{
			n0479 = null;
			n0480 = null;
			if (n0475[0] == null)
			{
				return ScenarioResult.n0442;
			}
			if (n0475[0].Name != "paragraph" && n0475[0].Name != "dialogue" && n0475[0].HasChildNodes)
			{
				for (int i = 0; i < 3; i++)
				{
					if (!n0475[i].HasChildNodes)
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					n0475[i] = n0475[i].FirstChild;
				}
			}
			else if (n0475[0].NextSibling != null)
			{
				for (int j = 0; j < 3; j++)
				{
					if (n0475[j].NextSibling == null)
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					n0475[j] = n0475[j].NextSibling;
				}
			}
			else
			{
				bool flag = true;
				while (n0475[0].ParentNode != n0158[0])
				{
					if (n0475[0].ParentNode.NextSibling != null)
					{
						for (int k = 0; k < 3; k++)
						{
							if (n0475[k].ParentNode == null || n0475[k].ParentNode.NextSibling == null)
							{
								MessageBox.Show(n0474);
								return ScenarioResult.n0442;
							}
							n0475[k] = n0475[k].ParentNode.NextSibling;
						}
						flag = false;
						break;
					}
					for (int l = 0; l < 3; l++)
					{
						if (n0475[l].ParentNode == null)
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						n0475[l] = n0475[l].ParentNode;
					}
				}
				if (flag)
				{
					if (n0475[0].ParentNode.NextSibling == null)
					{
						return ScenarioResult.n0442;
					}
					for (int m = 0; m < 3; m++)
					{
						if (n0475[m].ParentNode == null || n0475[m].ParentNode.NextSibling == null)
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						n0475[m] = n0475[m].ParentNode.NextSibling;
						n0158[m] = n0475[m];
					}
					n0476++;
				}
			}
			if (n0475[0].Name == "click")
			{
				if (n0475[0].PreviousSibling != null && (n0475[0].PreviousSibling.Name == "dialogue" || n0475[0].PreviousSibling.Name == "paragraph") && n0475[0].PreviousSibling.Attributes["voice"] != null)
				{
					n0480 = true;
				}
				else
				{
					n0480 = false;
				}
				return ScenarioResult.n0174;
			}
			if (n0475[0].Name == "scene")
			{
				if (n0475[0].Attributes["background"] != null)
				{
					GC.Collect();
					try
					{
						n0479 = TextureLoader.FromStream(n0133, n0153.ReadEntry(n0475[0].Attributes["background"].Value), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
						return ScenarioResult.n0442;
					}
				}
				else if (n0475[0].Attributes["backcolor"] != null)
				{
					try
					{
						n0479 = Color.FromName(n0475[0].Attributes["backcolor"].Value);
					}
					catch (Exception ex2)
					{
						MessageBox.Show(ex2.Message);
						return ScenarioResult.n0442;
					}
				}
				else
				{
					n0479 = Color.Black;
				}
				if (n0475[0].Attributes["time"] == null || n0475[0].Attributes["time"].Value == "short")
				{
					n0480 = 1;
				}
				else if (n0475[0].Attributes["time"].Value == "long")
				{
					n0480 = 2;
				}
				else
				{
					if (!(n0475[0].Attributes["time"].Value == "no"))
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					n0480 = 0;
				}
				if (n0475[0].Attributes["album"] != null)
				{
					try
					{
						int num = int.Parse(n0475[0].Attributes["album"].Value);
						if (num > n0477)
						{
							n0477 = num;
						}
					}
					catch (Exception ex3)
					{
						MessageBox.Show(ex3.Message);
						return ScenarioResult.n0442;
					}
				}
				return ScenarioResult.n0385;
			}
			if (n0475[0].Name == "music")
			{
				if ((n0475[0].Attributes["mute"] == null || n0475[0].Attributes["mute"].Value == "false") && n0475[0].Attributes["name"] != null)
				{
					try
					{
						n0152.Extract(n0475[0].Attributes["name"].Value, tempDirectory);
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
									return ScenarioResult.n0442;
								}
								flag2 = true;
							}
							if (!flag2)
							{
								break;
							}
							bgmSlotIndex = (bgmSlotIndex + 1) % 4;
						}
						File.Move(tempDirectory + "\\" + n0475[0].Attributes["name"].Value + ".wav", tempDirectory + "\\bgm" + bgmSlotIndex + ".wav");
						GC.Collect();
						n0479 = tempDirectory + "\\bgm" + bgmSlotIndex + ".wav";
						bgmSlotIndex = (bgmSlotIndex + 1) % 4;
					}
					catch (Exception ex5)
					{
						MessageBox.Show(ex5.Message);
						return ScenarioResult.n0442;
					}
					if (n0475[0].Attributes["position"] != null)
					{
						try
						{
							n0480 = double.Parse(n0475[0].Attributes["position"].Value);
							return ScenarioResult.n0387;
						}
						catch (Exception ex6)
						{
							MessageBox.Show(ex6.Message);
							return ScenarioResult.n0442;
						}
					}
					n0480 = 0.0;
					return ScenarioResult.n0387;
				}
				if (n0475[0].Attributes["mute"] != null && n0475[0].Attributes["mute"].Value == "true")
				{
					n0479 = null;
					n0480 = 0.0;
					return ScenarioResult.n0387;
				}
				if (n0475[0].Attributes["volume"] == null)
				{
					MessageBox.Show(n0474);
					return ScenarioResult.n0442;
				}
				n0479 = null;
				if (n0475[0].Attributes["volume"].Value == "down")
				{
					n0480 = false;
					return ScenarioResult.n0387;
				}
				if (n0475[0].Attributes["volume"].Value == "up")
				{
					n0480 = true;
					return ScenarioResult.n0387;
				}
				MessageBox.Show(n0474);
				return ScenarioResult.n0442;
			}
			if (n0475[0].Name == "chara")
			{
				n0480 = new SpriteState();
				((SpriteState)n0480).n0402 = 0;
				try
				{
					((SpriteState)n0480).n0309 = int.Parse(n0475[0].Attributes["index"].Value);
				}
				catch (Exception ex7)
				{
					MessageBox.Show(ex7.Message);
					return ScenarioResult.n0442;
				}
				if (n0475[0].Attributes["action"] != null && !(n0475[0].Attributes["action"].Value == "show"))
				{
					n0479 = null;
					if (n0475[0].Attributes["action"].Value == "hide")
					{
						((SpriteState)n0480).n0392 = SpriteMode.n0394;
					}
					else if (n0475[0].Attributes["action"].Value == "shock")
					{
						((SpriteState)n0480).n0392 = SpriteMode.n0400;
						if (n0475[0].Attributes["level"] == null)
						{
							((SpriteState)n0480).n0402 = 0;
						}
						else
						{
							try
							{
								((SpriteState)n0480).n0402 = int.Parse(n0475[0].Attributes["level"].Value);
							}
							catch (Exception ex8)
							{
								MessageBox.Show(ex8.Message);
								return ScenarioResult.n0442;
							}
						}
					}
				}
				else
				{
					try
					{
						ImageInformation imageInformation = TextureLoader.ImageInformationFromStream(n0482.ReadEntry(n0475[0].Attributes["code"].Value));
						n0479 = TextureLoader.FromStream(n0133, n0482.ReadEntry(n0475[0].Attributes["code"].Value), imageInformation.Width, imageInformation.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						((SpriteState)n0480).n0396 = new Size(imageInformation.Width, imageInformation.Height);
					}
					catch (Exception ex9)
					{
						MessageBox.Show(ex9.Message);
						return ScenarioResult.n0442;
					}
					((SpriteState)n0480).n0392 = SpriteMode.n0394;
				}
				if (n0475[0].Attributes["location"] != null)
				{
					try
					{
						((SpriteState)n0480).n0397 = int.Parse(n0475[0].Attributes["location"].Value);
					}
					catch (Exception ex10)
					{
						MessageBox.Show(ex10.Message);
						return ScenarioResult.n0442;
					}
				}
				else
				{
					((SpriteState)n0480).n0397 = int.MaxValue;
				}
				if (n0475[0].Attributes["transparent"] == null || n0475[0].Attributes["transparent"].Value == "false")
				{
					((SpriteState)n0480).n0398 = false;
				}
				else
				{
					if (!(n0475[0].Attributes["transparent"].Value == "true"))
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					((SpriteState)n0480).n0398 = true;
				}
				if (n0475[0].Attributes["animation"] == null || n0475[0].Attributes["animation"].Value == "true")
				{
					((SpriteState)n0480).n0399 = true;
				}
				else
				{
					if (!(n0475[0].Attributes["animation"].Value == "false"))
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					((SpriteState)n0480).n0399 = false;
				}
				if (n0475[0].Attributes["synchro"] == null)
				{
					((SpriteState)n0480).n0462 = !((SpriteState)n0480).n0399;
				}
				else if (n0475[0].Attributes["synchro"].Value == "false")
				{
					((SpriteState)n0480).n0462 = false;
				}
				else
				{
					if (!(n0475[0].Attributes["synchro"].Value == "true"))
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					((SpriteState)n0480).n0462 = true;
				}
				return ScenarioResult.n0390;
			}
			if (!(n0475[0].Name == "paragraph") && !(n0475[0].Name == "dialogue"))
			{
				if (n0475[0].Name == "sound")
				{
					n0480 = new InputState();
					if (n0475[0].Attributes["loop"] == null || n0475[0].Attributes["loop"].Value == "false")
					{
						((InputState)n0480).n0424 = false;
					}
					else
					{
						if (!(n0475[0].Attributes["loop"].Value == "true"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((InputState)n0480).n0424 = true;
					}
					if (n0475[0].Attributes["animation"] == null || n0475[0].Attributes["animation"].Value == "true")
					{
						((InputState)n0480).n0399 = true;
					}
					else
					{
						if (!(n0475[0].Attributes["animation"].Value == "false"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((InputState)n0480).n0399 = false;
					}
					if (n0475[0].Attributes["mute"] == null || n0475[0].Attributes["mute"].Value == "false")
					{
						if (n0475[0].Attributes["name"] == null)
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						try
						{
							n0483.Extract(n0475[0].Attributes["name"].Value, tempDirectory);
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
										return ScenarioResult.n0442;
									}
									flag3 = true;
								}
								if (!flag3)
								{
									break;
								}
								seSlotIndex = (seSlotIndex + 1) % 4;
							}
							File.Move(tempDirectory + "\\" + n0475[0].Attributes["name"].Value + ".wav", tempDirectory + "\\Se" + seSlotIndex + ".wav");
							GC.Collect();
							n0479 = tempDirectory + "\\se" + seSlotIndex + ".wav";
							seSlotIndex = (seSlotIndex + 1) % 4;
							return ScenarioResult.n0421;
						}
						catch (Exception ex12)
						{
							MessageBox.Show(ex12.Message);
							return ScenarioResult.n0442;
						}
					}
					if (!(n0475[0].Attributes["mute"].Value == "true"))
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					n0479 = null;
					return ScenarioResult.n0421;
				}
				if (n0475[0].Name == "wait")
				{
					if (n0475[0].Attributes["time"] == null)
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					if (n0475[0].Attributes["time"].Value == "no")
					{
						n0480 = 0;
					}
					else if (n0475[0].Attributes["time"].Value == "short")
					{
						n0480 = 1;
					}
					else if (n0475[0].Attributes["time"].Value == "middle")
					{
						n0480 = 2;
					}
					else
					{
						if (!(n0475[0].Attributes["time"].Value == "long"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						n0480 = 3;
					}
					return ScenarioResult.n0425;
				}
				if (n0475[0].Name == "clear")
				{
					if (n0475[0].Attributes["animation"] == null || n0475[0].Attributes["animation"].Value == "false")
					{
						n0480 = false;
						return ScenarioResult.n0426;
					}
					XmlNode[] array = n0475;
					if (!(array[0].Attributes["animation"].Value == "true"))
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					n0480 = true;
					return ScenarioResult.n0426;
				}
				if (n0475[0].Name == "title")
				{
					n0479 = new string[3];
					for (int num4 = 0; num4 < 3; num4++)
					{
						if (n0158[num4].Attributes["title"] == null)
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((string[])n0479)[num4] = n0158[num4].Attributes["title"].Value;
					}
					return ScenarioResult.n0428;
				}
				if (n0475[0].Name == "filter")
				{
					if (n0475[0].Attributes["type"] == null)
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					if (n0475[0].Attributes["type"].Value == "zoom")
					{
						n0480 = new AnimItem();
						((AnimItem)n0480).n0432 = DrawItemType.n0434;
						if (n0475[0].Attributes["left"] == null)
						{
							((AnimItem)n0480).n0437 = 0.0;
						}
						else
						{
							try
							{
								((AnimItem)n0480).n0437 = double.Parse(n0475[0].Attributes["left"].Value);
							}
							catch (Exception ex13)
							{
								MessageBox.Show(ex13.Message);
								return ScenarioResult.n0442;
							}
						}
						if (n0475[0].Attributes["right"] == null)
						{
							((AnimItem)n0480).n0438 = 1.0;
						}
						else
						{
							try
							{
								((AnimItem)n0480).n0438 = double.Parse(n0475[0].Attributes["right"].Value);
							}
							catch (Exception ex14)
							{
								MessageBox.Show(ex14.Message);
								return ScenarioResult.n0442;
							}
						}
						if (n0475[0].Attributes["top"] == null)
						{
							((AnimItem)n0480).n0439 = 0.0;
						}
						else
						{
							try
							{
								((AnimItem)n0480).n0439 = double.Parse(n0475[0].Attributes["top"].Value);
							}
							catch (Exception ex15)
							{
								MessageBox.Show(ex15.Message);
								return ScenarioResult.n0442;
							}
						}
						if (n0475[0].Attributes["bottom"] != null)
						{
							try
							{
								((AnimItem)n0480).n0440 = double.Parse(n0475[0].Attributes["bottom"].Value);
							}
							catch (Exception ex16)
							{
								MessageBox.Show(ex16.Message);
								return ScenarioResult.n0442;
							}
						}
						else
						{
							((AnimItem)n0480).n0440 = 1.0;
						}
						if (n0475[0].Attributes["time"] == null || n0475[0].Attributes["time"].Value == "short")
						{
							((AnimItem)n0480).n0441 = 1;
						}
						else
						{
							if (!(n0475[0].Attributes["time"].Value == "long"))
							{
								MessageBox.Show(n0474);
								return ScenarioResult.n0442;
							}
							((AnimItem)n0480).n0441 = 2;
						}
					}
					else if (n0475[0].Attributes["type"].Value == "dark")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).n0432 = DrawItemType.n0443;
					}
					else if (n0475[0].Attributes["type"].Value == "think")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).n0432 = DrawItemType.n0451;
					}
					else if (n0475[0].Attributes["type"].Value == "sun")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).n0432 = DrawItemType.n0445;
					}
					else if (n0475[0].Attributes["type"].Value == "sakura")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).n0432 = DrawItemType.n0447;
					}
					else if (n0475[0].Attributes["type"].Value == "snow")
					{
						n0480 = new DrawItem();
						((DrawItem)n0480).n0432 = DrawItemType.n0449;
					}
					else if (n0475[0].Attributes["type"].Value == "shock")
					{
						n0480 = new FlagItem();
						((FlagItem)n0480).n0432 = DrawItemType.n0400;
						if (n0475[0].Attributes["loop"] != null && !(n0475[0].Attributes["loop"].Value == "false"))
						{
							if (n0475[0].Attributes["loop"].Value == "true")
							{
								((FlagItem)n0480).n0424 = true;
							}
						}
						else
						{
							((FlagItem)n0480).n0424 = false;
						}
					}
					else
					{
						if (!(n0475[0].Attributes["type"].Value == "back"))
						{
							n0479 = null;
							n0480 = null;
							return ScenarioResult.n0485;
						}
						n0480 = new TextureItem();
						((TextureItem)n0480).n0432 = DrawItemType.n0413;
						((TextureItem)n0480).n0455 = TextureLoader.FromStream(n0133, n0153.ReadEntry(n0475[0].Attributes["ground"].Value), 0, 0, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
						if (n0475[0].Attributes["trans"] == null || n0475[0].Attributes["trans"].Value == "fade")
						{
							((TextureItem)n0480).n0456 = TextureBlend.n0487;
						}
						else if (n0475[0].Attributes["trans"].Value == "left")
						{
							((TextureItem)n0480).n0456 = TextureBlend.n0437;
						}
						else
						{
							if (!(n0475[0].Attributes["trans"].Value == "right"))
							{
								MessageBox.Show(n0474);
								return ScenarioResult.n0442;
							}
							((TextureItem)n0480).n0456 = TextureBlend.n0438;
						}
						if (n0475[0].Attributes["time"] == null || n0475[0].Attributes["time"].Value == "short")
						{
							((TextureItem)n0480).n0441 = 1;
						}
						else if (n0475[0].Attributes["time"].Value == "long")
						{
							((TextureItem)n0480).n0441 = 2;
						}
						else
						{
							if (!(n0475[0].Attributes["time"].Value == "no"))
							{
								MessageBox.Show(n0474);
								return ScenarioResult.n0442;
							}
							((TextureItem)n0480).n0441 = 0;
						}
						if (n0475[0].Attributes["album"] != null)
						{
							try
							{
								int num5 = int.Parse(n0475[0].Attributes["album"].Value);
								if (num5 > n0477)
								{
									n0477 = num5;
								}
							}
							catch (Exception ex17)
							{
								MessageBox.Show(ex17.Message);
								return ScenarioResult.n0442;
							}
						}
					}
					if (n0475[0].Attributes["animation"] == null || n0475[0].Attributes["animation"].Value == "false")
					{
						object obj = n0480;
						((DrawItem)obj).n0399 = false;
					}
					else
					{
						if (!(n0475[0].Attributes["animation"].Value == "true"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((DrawItem)n0480).n0399 = true;
					}
					if (n0475[0].Attributes["synchro"] == null)
					{
						((DrawItem)n0480).n0462 = !((DrawItem)n0480).n0399;
					}
					else if (n0475[0].Attributes["synchro"].Value == "false")
					{
						((DrawItem)n0480).n0462 = false;
					}
					else
					{
						if (!(n0475[0].Attributes["synchro"].Value == "true"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((DrawItem)n0480).n0462 = true;
					}
					if (n0475[0].Attributes["finish"] == null || n0475[0].Attributes["finish"].Value == "false")
					{
						((DrawItem)n0480).n0442 = false;
					}
					else
					{
						if (!(n0475[0].Attributes["finish"].Value == "true"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((DrawItem)n0480).n0442 = true;
					}
					return ScenarioResult.n0430;
				}
				if (n0475[0].Name == "ed")
				{
					n0152.Extract("BGM37", tempDirectory);
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
								return ScenarioResult.n0442;
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
					array2[0].Load(n0473.ReadEntry("ED_SC"));
					array2[1].Load(n0473.ReadEntry("ED_TC"));
					array2[2].Load(n0473.ReadEntry("ED_JP"));
					StaffRoll n537 = new StaffRoll(n536, n0482, n0153, array2);
					n0479 = n537;
					return ScenarioResult.n0459;
				}
				return ScenarioResult.n0485;
			}
			if (!n0475[0].HasChildNodes)
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
					if (n0475[num8].HasChildNodes)
					{
						for (int num9 = 0; num9 < n0475[num8].ChildNodes.Count; num9++)
						{
							if (n0475[num8].ChildNodes[num9].Name == "br")
							{
								if (n0475[num8].ChildNodes[num9].Attributes["line"] == null)
								{
									string[] array4;
									IntPtr intPtr;
									(array4 = (string[])n0479)[(int)(intPtr = (IntPtr)num8)] = array4[(int)intPtr] + "\n";
									continue;
								}
								int num10 = 1;
								try
								{
									num10 = int.Parse(n0475[num8].ChildNodes[num9].Attributes["line"].Value);
								}
								catch (Exception ex19)
								{
									MessageBox.Show(ex19.Message);
									return ScenarioResult.n0442;
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
								if (n0475[num8].ChildNodes[num9].NodeType != XmlNodeType.Text)
								{
									MessageBox.Show(n0474);
									return ScenarioResult.n0442;
								}
								string[] array6;
								IntPtr intPtr3;
								(array6 = (string[])n0479)[(int)(intPtr3 = (IntPtr)num8)] = array6[(int)intPtr3] + n0475[num8].ChildNodes[num9].Value;
							}
						}
						continue;
					}
					MessageBox.Show(n0474);
					return ScenarioResult.n0442;
				}
			}
			string n538 = null;
			bool n539 = false;
			if (n0475[0].Attributes["voice"] != null)
			{
				if (n0168)
				{
					n539 = true;
				}
				else
				{
					try
					{
						n0488.Extract(n0475[0].Attributes["voice"].Value, tempDirectory);
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
									return ScenarioResult.n0442;
								}
								flag5 = true;
							}
							if (!flag5)
							{
								break;
							}
							voiceSlotIndex = (voiceSlotIndex + 1) % 4;
						}
						File.Move(tempDirectory + "\\" + n0475[0].Attributes["voice"].Value + ".wav", tempDirectory + "\\voice" + voiceSlotIndex + ".wav");
						n538 = tempDirectory + "\\voice" + voiceSlotIndex + ".wav";
						voiceSlotIndex = (voiceSlotIndex + 1) % 4;
					}
					catch (Exception ex21)
					{
						MessageBox.Show(ex21.Message);
						return ScenarioResult.n0442;
					}
				}
			}
			if (n0475[0].Name == "paragraph")
			{
				n0480 = new MessageCommand();
				if (n0475[0].Attributes["align"] != null && !(n0475[0].Attributes["align"].Value == "left"))
				{
					if (n0475[0].Attributes["align"].Value == "center")
					{
						((MessageCommand)n0480).n0410 = HorizontalAlign.n0491;
					}
					else
					{
						if (!(n0475[0].Attributes["align"].Value == "right"))
						{
							MessageBox.Show(n0474);
							return ScenarioResult.n0442;
						}
						((MessageCommand)n0480).n0410 = HorizontalAlign.n0438;
					}
				}
				else
				{
					((MessageCommand)n0480).n0410 = HorizontalAlign.n0437;
				}
				if (n0475[0].Attributes["show"] == null || n0475[0].Attributes["show"].Value == "print")
				{
					((MessageCommand)n0480).n0411 = TextRevealMode.n0493;
				}
				else if (n0475[0].Attributes["show"].Value == "fade")
				{
					((MessageCommand)n0480).n0411 = TextRevealMode.n0487;
				}
				if (n0475[0].Attributes["fore"] == null)
				{
					((MessageCommand)n0480).n0412 = Color.White;
				}
				else
				{
					try
					{
						((MessageCommand)n0480).n0412 = Color.FromName(n0475[0].Attributes["fore"].Value);
					}
					catch (Exception ex22)
					{
						MessageBox.Show(ex22.Message);
						return ScenarioResult.n0442;
					}
				}
				if (n0475[0].Attributes["back"] != null)
				{
					try
					{
						((MessageCommand)n0480).n0413 = Color.FromName(n0475[0].Attributes["back"].Value);
					}
					catch (Exception ex23)
					{
						MessageBox.Show(ex23.Message);
						return ScenarioResult.n0442;
					}
				}
				else
				{
					((MessageCommand)n0480).n0413 = Color.Black;
				}
				XmlNode nextSibling = n0475[0].NextSibling;
				while (nextSibling != null && nextSibling.NodeType == XmlNodeType.Comment)
				{
					nextSibling = nextSibling.NextSibling;
				}
				if (nextSibling != null && nextSibling.Name == "click")
				{
					((MessageCommand)n0480).n0174 = true;
				}
				else
				{
					((MessageCommand)n0480).n0174 = false;
				}
				((MessageCommand)n0480).n0407 = n538;
				((MessageCommand)n0480).n0168 = n539;
				return ScenarioResult.n0404;
			}
			n0480 = new CaptionCommand();
			if (n0475[0].Attributes["chara"] != null)
			{
				for (int num13 = 0; num13 < 3; num13++)
				{
					if (n0475[num13].Attributes["chara"] == null)
					{
						MessageBox.Show(n0474);
						return ScenarioResult.n0442;
					}
					((CaptionCommand)n0480).n0390[num13] = n0475[num13].Attributes["chara"].Value;
				}
			}
			else
			{
				((CaptionCommand)n0480).n0390 = null;
			}
			if (n0475[0].Attributes["disappear"] == null || n0475[0].Attributes["disappear"].Value == "false")
			{
				((CaptionCommand)n0480).n0417 = false;
			}
			else
			{
				if (!(n0475[0].Attributes["disappear"].Value == "true"))
				{
					MessageBox.Show(n0474);
					return ScenarioResult.n0442;
				}
				((CaptionCommand)n0480).n0417 = true;
			}
			if (n0475[0].Attributes["continue"] == null || n0475[0].Attributes["continue"].Value == "false")
			{
				((CaptionCommand)n0480).n0420 = false;
			}
			else
			{
				if (!(n0475[0].Attributes["continue"].Value == "true"))
				{
					MessageBox.Show(n0474);
					return ScenarioResult.n0442;
				}
				((CaptionCommand)n0480).n0420 = true;
			}
			((CaptionCommand)n0480).n0418 = ((CaptionCommand)n0480).n0417;
			if (((CaptionCommand)n0480).n0390 != null)
			{
				XmlNode nextSibling2 = n0475[0].NextSibling;
				while (nextSibling2 != null && nextSibling2.Name != "dialogue")
				{
					nextSibling2 = nextSibling2.NextSibling;
				}
				if (nextSibling2 != null)
				{
					if (nextSibling2.Attributes["chara"] == null)
					{
						((CaptionCommand)n0480).n0418 = true;
					}
				}
				else
				{
					((CaptionCommand)n0480).n0418 = true;
				}
			}
			((CaptionCommand)n0480).n0407 = n538;
			((CaptionCommand)n0480).n0168 = n539;
			return ScenarioResult.n0415;
		}

		public int n0157()
		{
			return n0476;
		}

		public int n0159()
		{
			return n0477;
		}
	}
}
