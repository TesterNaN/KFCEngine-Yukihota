using System;
using System.Drawing;
using System.Xml;
using Microsoft.DirectX.Direct3D;
using n0062;

namespace n0001
{
	public class StaffRoll : IDisposable
	{
		protected const int n0315 = 24;

		protected const int n0556 = 15;

		private MediaPackage edPackage;

		private ArchiveFile charaArchive;

		private ArchiveFile bgArchive;

		private XmlNode[] xmlDocuments;

		private XmlNode[] staffNodes = new XmlNode[3];

		private XmlNode[] musicNodes = new XmlNode[3];

		private Texture characterTexture;

		private ImageInformation imageInfo;

		private Texture backgroundTexture;

		private Device graphicsDevice;

		private Sprite spriteBatch;

		private FrameCallback deviceResetHandler;

		private Microsoft.DirectX.Direct3D.Font textFont;

		private ButtonLayout layout;

		protected Texture n0135;

		protected readonly string[,] titleLines;

		protected readonly string[] characterTextureNames;

		protected readonly string[,,] castNames;

		protected readonly string[] backgroundTextureNames;

		protected readonly Rectangle backgroundSourceRect;

		protected readonly Point backgroundPosition;

		protected readonly Point barPosition;

		protected readonly Size characterNameSize;

		private int staffLineCount;

		private bool started;

		private int characterIndex;

		private int backgroundIndex;

		public StaffRoll(string n0387, ArchiveFile charaNames, ArchiveFile n0385, XmlNode[] n0348)
		{
			titleLines = new string[3, 2]
			{
				{ "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪萤～", "ＣＡＳＴ" },
				{ "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪螢～", "ＣＡＳＴ" },
				{ "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪蛍～", "ＣＡＳＴ" }
			};
			characterTextureNames = new string[6] { "SH01CMA", "IK01CMA", "TT01CMA", "SS01AMA", "NS01CMA", "IS01AMA" };
			castNames = new string[3, 6, 2]
			{
				{
					{ "白河 萤", "水树 奈奈" },
					{ "伊波 健", "小尾 元政" },
					{ "飞世 巴", "仲西 环" },
					{ "白河 静流", "菊池 志穗" },
					{ "中森 翔太", "福山 润" },
					{ "稻穗 信", "间岛 淳司" }
				},
				{
					{ "白河 螢", "水樹 奈奈" },
					{ "伊波 健", "小尾 元政" },
					{ "飛世 巴", "仲西 環" },
					{ "白河 靜流", "菊池 志穗" },
					{ "中森 翔太", "福山 潤" },
					{ "稻穗 信", "間島 淳司" }
				},
				{
					{ "白河 ほたる", "水樹 奈々" },
					{ "伊波 健", "小尾 元政" },
					{ "飛世 巴", "仲西 環" },
					{ "白河 静流", "菊池 志穂" },
					{ "中森 翔太", "福山 潤" },
					{ "稲穂 信", "間島 淳司" }
				}
			};
			backgroundTextureNames = new string[7] { "EV_HP07B", "EV_HP01A", "EV_HP02A", "EV_HP03A", "EV_HP04A", "EV_HP06A", "EV_HP07A" };
			backgroundSourceRect = new Rectangle(0, 0, 300, 225);
			backgroundPosition = new Point(1200, 480);
			barPosition = new Point(0, 560);
			characterNameSize = new Size(30, 40);
			edPackage = new MediaPackage("ED");
			edPackage.Open(n0387);
			charaArchive = charaNames;
			bgArchive = n0385;
			xmlDocuments = n0348;
			for (int i = 0; i < 3; i++)
			{
				staffNodes[i] = xmlDocuments[i].SelectSingleNode("ed/staff");
				musicNodes[i] = xmlDocuments[i].SelectSingleNode("ed/music");
			}
			staffLineCount = staffNodes[0].ChildNodes.Count + 15;
			started = true;
			characterTexture = null;
			backgroundTexture = null;
			characterTexture = null;
		}

		public void Initialize(Device n0349, Sprite n0350, FrameCallback n0225, Texture n0351, System.Drawing.Font n0353, ButtonLayout n0046)
		{
			graphicsDevice = n0349;
			spriteBatch = n0350;
			deviceResetHandler = n0225;
			n0135 = n0351;
			textFont = new Microsoft.DirectX.Direct3D.Font(graphicsDevice, new System.Drawing.Font(n0353.FontFamily, 24f));
			layout = n0046;
		}

		public bool Update()
		{
			if (started)
			{
				edPackage.Play(false);
				started = false;
				characterIndex = -1;
				backgroundIndex = -1;
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
				characterTexture = null;
				backgroundTexture = null;
			}
			bool result = true;
			graphicsDevice.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			graphicsDevice.BeginScene();
			spriteBatch.Begin(SpriteFlags.AlphaBlend);
			double num = (double)edPackage.n0269() / 1000.0;
			if (num < 8.0)
			{
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				int num2 = (int)num / 4;
				double num3 = num - (double)(num2 * 4);
				int num4;
				if (num3 < 1.0)
				{
					num4 = (int)(num3 * 255.0);
				}
				else
				{
					num4 = ((!(num3 < 3.0)) ? ((int)((4.0 - num3) * 255.0)) : 255);
				}
				if (num4 == 255)
				{
					textFont.DrawText(spriteBatch, titleLines[(int)layout, num2], 400 - titleLines[(int)layout, num2].Length * 15, 280, Color.White);
				}
				else
				{
					textFont.DrawText(spriteBatch, titleLines[(int)layout, num2], 400 - titleLines[(int)layout, num2].Length * 15, 280, Color.FromArgb(num4, Color.White));
				}
			}
			else if (num < 38.0)
			{
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
				int num5 = (int)(num - 8.0) / 5;
				double num6 = num - 8.0 - (double)(num5 * 5);
				if (characterIndex != num5)
				{
					if (characterTexture != null)
					{
						characterTexture.Dispose();
					}
					imageInfo = TextureLoader.ImageInformationFromStream(charaArchive.ReadEntry(characterTextureNames[num5]));
					characterTexture = TextureLoader.FromStream(graphicsDevice, charaArchive.ReadEntry(characterTextureNames[num5]), imageInfo.Width, imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					characterIndex = num5;
				}
				if (num6 < 0.5)
				{
				}
				int num7;
				if (num6 < 1.5)
				{
					num7 = (int)((num6 - 0.5) * 255.0);
				}
				else
				{
					num7 = ((num6 < 3.5) ? 255 : ((num6 < 4.5) ? ((int)((4.5 - num6) * 255.0)) : 0));
				}
				if (num7 > 0)
				{
					if (num7 == 255)
					{
						spriteBatch.Draw2D(characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - imageInfo.Width / 2, 600 - imageInfo.Height), Color.White);
						textFont.DrawText(spriteBatch, castNames[(int)layout, num5, 0], 505 - (int)((double)castNames[(int)layout, num5, 0].Length - 0.5) * 15, 260, Color.White);
						textFont.DrawText(spriteBatch, castNames[(int)layout, num5, 1], 505 - (int)((double)castNames[(int)layout, num5, 1].Length - 0.5) * 15, 300, Color.White);
					}
					else
					{
						spriteBatch.Draw2D(characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - imageInfo.Width / 2, 600 - imageInfo.Height), Color.FromArgb(num7, Color.White));
						textFont.DrawText(spriteBatch, castNames[(int)layout, num5, 0], 505 - (int)((double)castNames[(int)layout, num5, 0].Length - 0.5) * 15, 260, Color.FromArgb(num7, Color.White));
						textFont.DrawText(spriteBatch, castNames[(int)layout, num5, 1], 505 - (int)((double)castNames[(int)layout, num5, 1].Length - 0.5) * 15, 300, Color.FromArgb(num7, Color.White));
					}
				}
			}
			else if (num < 171.0)
			{
				int num8 = (int)(num - 38.0) / 19;
				double num9 = num - 38.0 - (double)(num8 * 19);
				if (backgroundIndex != num8)
				{
					if (backgroundTexture != null)
					{
						backgroundTexture.Dispose();
					}
					imageInfo = TextureLoader.ImageInformationFromStream(bgArchive.ReadEntry(backgroundTextureNames[num8]));
					backgroundTexture = TextureLoader.FromStream(graphicsDevice, bgArchive.ReadEntry(backgroundTextureNames[num8]), imageInfo.Width, imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					backgroundIndex = num8;
				}
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				if (num9 < 0.5)
				{
				}
				int num10;
				if (num9 < 2.5)
				{
					num10 = (int)((num9 - 0.5) / 2.0 * 255.0);
				}
				else
				{
					num10 = ((num9 < 16.5) ? 255 : ((num9 < 18.5) ? ((int)((18.5 - num9) / 2.0 * 255.0)) : 0));
				}
				if (num10 > 0)
				{
					if (num10 == 255)
					{
						spriteBatch.Draw2D(backgroundTexture, Rectangle.Empty, backgroundSourceRect, Point.Empty, 0f, backgroundPosition, Color.White);
					}
					else
					{
						spriteBatch.Draw2D(backgroundTexture, Rectangle.Empty, backgroundSourceRect, Point.Empty, 0f, backgroundPosition, Color.FromArgb(num10, Color.White));
					}
					spriteBatch.Draw2D(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, Point.Empty, Color.Transparent);
				}
				if (num < 163.0)
				{
					num9 = num - 38.0;
					double num11 = num9 / 125.0 * (double)staffLineCount;
					double num12 = num11 - 15.0;
					for (int i = (int)num12; i <= (int)num11; i++)
					{
						if (i >= 0 && i < staffLineCount - 15 && staffNodes[0].ChildNodes[i].HasChildNodes)
						{
							textFont.DrawText(spriteBatch, staffNodes[(int)layout].ChildNodes[i].FirstChild.Value, 92, (int)(((double)i - num12) * 40.0) + 2, Color.Black);
							textFont.DrawText(spriteBatch, staffNodes[(int)layout].ChildNodes[i].FirstChild.Value, 90, (int)(((double)i - num12) * 40.0), Color.White);
						}
					}
				}
			}
			else if (num * 1000.0 < (double)edPackage.n0270())
			{
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
			}
			else
			{
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
				edPackage.n0162();
				result = false;
			}
			spriteBatch.Draw2D(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, barPosition, Color.Black);
			for (int j = 0; j < musicNodes[(int)layout].ChildNodes.Count - 1; j++)
			{
				double num13 = double.Parse(musicNodes[(int)layout].ChildNodes[j].Attributes["start"].Value);
				double num14 = double.Parse(musicNodes[(int)layout].ChildNodes[j].Attributes["end"].Value);
				double num15 = double.Parse(musicNodes[(int)layout].ChildNodes[j + 1].Attributes["start"].Value);
				if (!(num > num13) || !(num < num15))
				{
					continue;
				}
				string value = musicNodes[(int)layout].ChildNodes[j].FirstChild.Value;
				for (int k = 0; k < value.Length; k++)
				{
					textFont.DrawText(spriteBatch, value[k].ToString(), 400 - value.Length * 15 + k * 30 + 2, 562, Color.White);
				}
				if (num < num14)
				{
					double num16 = (num - num13) / (num14 - num13) * (double)value.Length;
					for (int l = 0; l < (int)num16; l++)
					{
						textFont.DrawText(spriteBatch, value[l].ToString(), 400 - value.Length * 15 + l * 30, 560, Color.Yellow);
					}
					textFont.DrawText(spriteBatch, value[(int)num16].ToString(), 400 - value.Length * 15 + (int)num16 * 30, 560, Color.FromArgb((int)((num16 - (double)(int)num16) * 255.0), Color.Yellow));
				}
				else
				{
					for (int m = 0; m < value.Length; m++)
					{
						textFont.DrawText(spriteBatch, value[m].ToString(), 400 - value.Length * 15 + m * 30, 560, Color.Yellow);
					}
				}
				break;
			}
			spriteBatch.End();
			graphicsDevice.EndScene();
			try
			{
				graphicsDevice.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					graphicsDevice.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					deviceResetHandler();
				}
				catch
				{
				}
			}
			return result;
		}

		public void n0230()
		{
			edPackage.n0162();
			if (characterTexture != null)
			{
				characterTexture.Dispose();
			}
			if (backgroundTexture != null)
			{
				backgroundTexture.Dispose();
			}
		}

		public void SetPaused(bool n0241)
		{
			if (n0241)
			{
				edPackage.SetPaused();
			}
			else
			{
				edPackage.n0165();
			}
		}

		public void SetButtonLayout(ButtonLayout n0046)
		{
			layout = n0046;
		}

		public void Dispose()
		{
			edPackage.n0162();
			GC.SuppressFinalize(this);
		}
	}
}
