using System;
using System.Drawing;
using System.Xml;
using Microsoft.DirectX.Direct3D;
using Yukihota.Audio;

namespace Yukihota
{
	public class StaffRoll : IDisposable
	{
		protected const int fontSize = 24;

		protected const int charWidth = 15;

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

		protected Texture overlayTexture;

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

		public StaffRoll(string musicPath, ArchiveFile characterArchive, ArchiveFile backgroundArchive, XmlNode[] edNodes)
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
			edPackage.Open(musicPath);
			charaArchive = characterArchive;
			bgArchive = backgroundArchive;
			xmlDocuments = edNodes;
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

		public void Initialize(Device device, Sprite sprite, FrameCallback frameCallback, Texture whiteTexture, System.Drawing.Font font, ButtonLayout buttonLayout)
		{
			graphicsDevice = device;
			spriteBatch = sprite;
			deviceResetHandler = frameCallback;
			overlayTexture = whiteTexture;
			textFont = new Microsoft.DirectX.Direct3D.Font(graphicsDevice, new System.Drawing.Font(font.FontFamily, 24f));
			layout = buttonLayout;
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
			double elapsed = (double)edPackage.GetPosition() / 1000.0;
			if (elapsed < 8.0)
			{
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				int titleRow = (int)elapsed / 4;
				double titlePhase = elapsed - (double)(titleRow * 4);
				int titleAlpha;
				if (titlePhase < 1.0)
				{
					titleAlpha = (int)(titlePhase * 255.0);
				}
				else
				{
					titleAlpha = ((!(titlePhase < 3.0)) ? ((int)((4.0 - titlePhase) * 255.0)) : 255);
				}
				if (titleAlpha == 255)
				{
					textFont.DrawText(spriteBatch, titleLines[(int)layout, titleRow], 400 - titleLines[(int)layout, titleRow].Length * 15, 280, Color.White);
				}
				else
				{
					textFont.DrawText(spriteBatch, titleLines[(int)layout, titleRow], 400 - titleLines[(int)layout, titleRow].Length * 15, 280, Color.FromArgb(titleAlpha, Color.White));
				}
			}
			else if (elapsed < 38.0)
			{
				if (backgroundTexture != null)
				{
					backgroundTexture.Dispose();
				}
				int castIndex = (int)(elapsed - 8.0) / 5;
				double castPhase = elapsed - 8.0 - (double)(castIndex * 5);
				if (characterIndex != castIndex)
				{
					if (characterTexture != null)
					{
						characterTexture.Dispose();
					}
					imageInfo = TextureLoader.ImageInformationFromStream(charaArchive.ReadEntry(characterTextureNames[castIndex]));
					characterTexture = TextureLoader.FromStream(graphicsDevice, charaArchive.ReadEntry(characterTextureNames[castIndex]), imageInfo.Width, imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					characterIndex = castIndex;
				}
				if (castPhase < 0.5)
				{
				}
				int castAlpha;
				if (castPhase < 1.5)
				{
					castAlpha = (int)((castPhase - 0.5) * 255.0);
				}
				else
				{
					castAlpha = ((castPhase < 3.5) ? 255 : ((castPhase < 4.5) ? ((int)((4.5 - castPhase) * 255.0)) : 0));
				}
				if (castAlpha > 0)
				{
					if (castAlpha == 255)
					{
						spriteBatch.Draw2D(characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - imageInfo.Width / 2, 600 - imageInfo.Height), Color.White);
						textFont.DrawText(spriteBatch, castNames[(int)layout, castIndex, 0], 505 - (int)((double)castNames[(int)layout, castIndex, 0].Length - 0.5) * 15, 260, Color.White);
						textFont.DrawText(spriteBatch, castNames[(int)layout, castIndex, 1], 505 - (int)((double)castNames[(int)layout, castIndex, 1].Length - 0.5) * 15, 300, Color.White);
					}
					else
					{
						spriteBatch.Draw2D(characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - imageInfo.Width / 2, 600 - imageInfo.Height), Color.FromArgb(castAlpha, Color.White));
						textFont.DrawText(spriteBatch, castNames[(int)layout, castIndex, 0], 505 - (int)((double)castNames[(int)layout, castIndex, 0].Length - 0.5) * 15, 260, Color.FromArgb(castAlpha, Color.White));
						textFont.DrawText(spriteBatch, castNames[(int)layout, castIndex, 1], 505 - (int)((double)castNames[(int)layout, castIndex, 1].Length - 0.5) * 15, 300, Color.FromArgb(castAlpha, Color.White));
					}
				}
			}
			else if (elapsed < 171.0)
			{
				int bgIndex = (int)(elapsed - 38.0) / 19;
				double segmentTime = elapsed - 38.0 - (double)(bgIndex * 19);
				if (backgroundIndex != bgIndex)
				{
					if (backgroundTexture != null)
					{
						backgroundTexture.Dispose();
					}
					imageInfo = TextureLoader.ImageInformationFromStream(bgArchive.ReadEntry(backgroundTextureNames[bgIndex]));
					backgroundTexture = TextureLoader.FromStream(graphicsDevice, bgArchive.ReadEntry(backgroundTextureNames[bgIndex]), imageInfo.Width, imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					backgroundIndex = bgIndex;
				}
				if (characterTexture != null)
				{
					characterTexture.Dispose();
				}
				if (segmentTime < 0.5)
				{
				}
				int bgAlpha;
				if (segmentTime < 2.5)
				{
					bgAlpha = (int)((segmentTime - 0.5) / 2.0 * 255.0);
				}
				else
				{
					bgAlpha = ((segmentTime < 16.5) ? 255 : ((segmentTime < 18.5) ? ((int)((18.5 - segmentTime) / 2.0 * 255.0)) : 0));
				}
				if (bgAlpha > 0)
				{
					if (bgAlpha == 255)
					{
						spriteBatch.Draw2D(backgroundTexture, Rectangle.Empty, backgroundSourceRect, Point.Empty, 0f, backgroundPosition, Color.White);
					}
					else
					{
						spriteBatch.Draw2D(backgroundTexture, Rectangle.Empty, backgroundSourceRect, Point.Empty, 0f, backgroundPosition, Color.FromArgb(bgAlpha, Color.White));
					}
					spriteBatch.Draw2D(overlayTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, Point.Empty, Color.Transparent);
				}
				if (elapsed < 163.0)
				{
					segmentTime = elapsed - 38.0;
					double staffPosition = segmentTime / 125.0 * (double)staffLineCount;
					double staffStartLine = staffPosition - 15.0;
					for (int i = (int)staffStartLine; i <= (int)staffPosition; i++)
					{
						if (i >= 0 && i < staffLineCount - 15 && staffNodes[0].ChildNodes[i].HasChildNodes)
						{
							textFont.DrawText(spriteBatch, staffNodes[(int)layout].ChildNodes[i].FirstChild.Value, 92, (int)(((double)i - staffStartLine) * 40.0) + 2, Color.Black);
							textFont.DrawText(spriteBatch, staffNodes[(int)layout].ChildNodes[i].FirstChild.Value, 90, (int)(((double)i - staffStartLine) * 40.0), Color.White);
						}
					}
				}
			}
			else if (elapsed * 1000.0 < (double)edPackage.GetLength())
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
				edPackage.Close();
				result = false;
			}
			spriteBatch.Draw2D(overlayTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, barPosition, Color.Black);
			for (int j = 0; j < musicNodes[(int)layout].ChildNodes.Count - 1; j++)
			{
				double musicStart = double.Parse(musicNodes[(int)layout].ChildNodes[j].Attributes["start"].Value);
				double musicEnd = double.Parse(musicNodes[(int)layout].ChildNodes[j].Attributes["end"].Value);
				double nextMusicStart = double.Parse(musicNodes[(int)layout].ChildNodes[j + 1].Attributes["start"].Value);
				if (!(elapsed > musicStart) || !(elapsed < nextMusicStart))
				{
					continue;
				}
				string value = musicNodes[(int)layout].ChildNodes[j].FirstChild.Value;
				for (int k = 0; k < value.Length; k++)
				{
					textFont.DrawText(spriteBatch, value[k].ToString(), 400 - value.Length * 15 + k * 30 + 2, 562, Color.White);
				}
				if (elapsed < musicEnd)
				{
					double lyricProgress = (elapsed - musicStart) / (musicEnd - musicStart) * (double)value.Length;
					for (int l = 0; l < (int)lyricProgress; l++)
					{
						textFont.DrawText(spriteBatch, value[l].ToString(), 400 - value.Length * 15 + l * 30, 560, Color.Yellow);
					}
					textFont.DrawText(spriteBatch, value[(int)lyricProgress].ToString(), 400 - value.Length * 15 + (int)lyricProgress * 30, 560, Color.FromArgb((int)((lyricProgress - (double)(int)lyricProgress) * 255.0), Color.Yellow));
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

		public void Stop()
		{
			edPackage.Close();
			if (characterTexture != null)
			{
				characterTexture.Dispose();
			}
			if (backgroundTexture != null)
			{
				backgroundTexture.Dispose();
			}
		}

		public void SetPaused(bool paused)
		{
			if (paused)
			{
				edPackage.Pause();
			}
			else
			{
				edPackage.Resume();
			}
		}

		public void SetButtonLayout(ButtonLayout buttonLayout)
		{
			layout = buttonLayout;
		}

		public void Dispose()
		{
			edPackage.Close();
			GC.SuppressFinalize(this);
		}
	}
}
