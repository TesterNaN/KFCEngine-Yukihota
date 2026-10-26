using System;
using System.Drawing;
using System.Xml;
using Microsoft.DirectX.Direct3D;
using Yukihota.Audio;

namespace Yukihota
{
	// Token: 0x02000023 RID: 35
	public class StaffRoll : IDisposable
	{
		// Token: 0x06001F39 RID: 7993 RVA: 0x00017A90 File Offset: 0x00016A90
		public StaffRoll(string musicPath, ArchiveFile characterArchive, ArchiveFile backgroundArchive, XmlNode[] edNodes)
		{
			string[,] titleLines = new string[3, 2];
			titleLines[0, 0] = "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪萤～";
			titleLines[0, 1] = "ＣＡＳＴ";
			titleLines[1, 0] = "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪螢～";
			titleLines[1, 1] = "ＣＡＳＴ";
			titleLines[2, 0] = "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪蛍～";
			titleLines[2, 1] = "ＣＡＳＴ";
			this.titleLines = titleLines;
			this.characterTextureNames = new string[] { "SH01CMA", "IK01CMA", "TT01CMA", "SS01AMA", "NS01CMA", "IS01AMA" };
			string[,,] castNames = new string[3, 6, 2];
			castNames[0, 0, 0] = "白河 萤";
			castNames[0, 0, 1] = "水树 奈奈";
			castNames[0, 1, 0] = "伊波 健";
			castNames[0, 1, 1] = "小尾 元政";
			castNames[0, 2, 0] = "飞世 巴";
			castNames[0, 2, 1] = "仲西 环";
			castNames[0, 3, 0] = "白河 静流";
			castNames[0, 3, 1] = "菊池 志穗";
			castNames[0, 4, 0] = "中森 翔太";
			castNames[0, 4, 1] = "福山 润";
			castNames[0, 5, 0] = "稻穗 信";
			castNames[0, 5, 1] = "间岛 淳司";
			castNames[1, 0, 0] = "白河 螢";
			castNames[1, 0, 1] = "水樹 奈奈";
			castNames[1, 1, 0] = "伊波 健";
			castNames[1, 1, 1] = "小尾 元政";
			castNames[1, 2, 0] = "飛世 巴";
			castNames[1, 2, 1] = "仲西 環";
			castNames[1, 3, 0] = "白河 靜流";
			castNames[1, 3, 1] = "菊池 志穗";
			castNames[1, 4, 0] = "中森 翔太";
			castNames[1, 4, 1] = "福山 潤";
			castNames[1, 5, 0] = "稻穗 信";
			castNames[1, 5, 1] = "間島 淳司";
			castNames[2, 0, 0] = "白河 ほたる";
			castNames[2, 0, 1] = "水樹 奈々";
			castNames[2, 1, 0] = "伊波 健";
			castNames[2, 1, 1] = "小尾 元政";
			castNames[2, 2, 0] = "飛世 巴";
			castNames[2, 2, 1] = "仲西 環";
			castNames[2, 3, 0] = "白河 静流";
			castNames[2, 3, 1] = "菊池 志穂";
			castNames[2, 4, 0] = "中森 翔太";
			castNames[2, 4, 1] = "福山 潤";
			castNames[2, 5, 0] = "稲穂 信";
			castNames[2, 5, 1] = "間島 淳司";
			this.castNames = castNames;
			this.backgroundTextureNames = new string[] { "EV_HP07B", "EV_HP01A", "EV_HP02A", "EV_HP03A", "EV_HP04A", "EV_HP06A", "EV_HP07A" };
			this.backgroundSourceRect = new Rectangle(0, 0, 300, 225);
			this.backgroundPosition = new Point(1200, 480);
			this.barPosition = new Point(0, 560);
			this.characterNameSize = new Size(30, 40);
			this.edPackage = new MediaPackage("ED");
			this.edPackage.Open(musicPath);
			this.charaArchive = characterArchive;
			this.bgArchive = backgroundArchive;
			this.xmlDocuments = edNodes;
			for (int i = 0; i < 3; i++)
			{
				this.staffNodes[i] = this.xmlDocuments[i].SelectSingleNode("ed/staff");
				this.musicNodes[i] = this.xmlDocuments[i].SelectSingleNode("ed/music");
			}
			this.staffLineCount = this.staffNodes[0].ChildNodes.Count + 15;
			this.started = true;
			this.characterTexture = null;
			this.backgroundTexture = null;
			this.characterTexture = null;
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x00017F24 File Offset: 0x00016F24
		public void Initialize(Device device, Sprite sprite, FrameCallback frameCallback, Texture whiteTexture, System.Drawing.Font font, ButtonLayout buttonLayout)
		{
			this.graphicsDevice = device;
			this.spriteBatch = sprite;
			this.deviceResetHandler = frameCallback;
			this.overlayTexture = whiteTexture;
			this.textFont = new Microsoft.DirectX.Direct3D.Font(this.graphicsDevice, new System.Drawing.Font(font.FontFamily, 24f));
			this.layout = buttonLayout;
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x00017F78 File Offset: 0x00016F78
		public bool Update()
		{
			if (this.started)
			{
				this.edPackage.Play(false);
				this.started = false;
				this.characterIndex = -1;
				this.backgroundIndex = -1;
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
				this.characterTexture = null;
				this.backgroundTexture = null;
			}
			bool running = true;
			this.graphicsDevice.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			this.graphicsDevice.BeginScene();
			this.spriteBatch.Begin(SpriteFlags.AlphaBlend);
			double seconds = (double)this.edPackage.GetPosition() / 1000.0;
			if (seconds < 8.0)
			{
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				int titleLine = (int)seconds / 4;
				double titlePhase = seconds - (double)(titleLine * 4);
				int titleAlpha;
				if (titlePhase < 1.0)
				{
					titleAlpha = (int)(titlePhase * 255.0);
				}
				else if (titlePhase < 3.0)
				{
					titleAlpha = 255;
				}
				else
				{
					titleAlpha = (int)((4.0 - titlePhase) * 255.0);
				}
				if (titleAlpha == 255)
				{
					this.textFont.DrawText(this.spriteBatch, this.titleLines[(int)this.layout, titleLine], 400 - this.titleLines[(int)this.layout, titleLine].Length * 15, 280, Color.White);
				}
				else
				{
					this.textFont.DrawText(this.spriteBatch, this.titleLines[(int)this.layout, titleLine], 400 - this.titleLines[(int)this.layout, titleLine].Length * 15, 280, Color.FromArgb(titleAlpha, Color.White));
				}
			}
			else if (seconds < 38.0)
			{
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
				int charaIndex = (int)(seconds - 8.0) / 5;
				double charaPhase = seconds - 8.0 - (double)(charaIndex * 5);
				if (this.characterIndex != charaIndex)
				{
					if (this.characterTexture != null)
					{
						this.characterTexture.Dispose();
					}
					this.imageInfo = TextureLoader.ImageInformationFromStream(this.charaArchive.ReadEntry(this.characterTextureNames[charaIndex]));
					this.characterTexture = TextureLoader.FromStream(this.graphicsDevice, this.charaArchive.ReadEntry(this.characterTextureNames[charaIndex]), this.imageInfo.Width, this.imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					this.characterIndex = charaIndex;
				}
				if (charaPhase < 0.5)
				{
				}
				int charaAlpha;
				if (charaPhase < 1.5)
				{
					charaAlpha = (int)((charaPhase - 0.5) * 255.0);
				}
				else if (charaPhase < 3.5)
				{
					charaAlpha = 255;
				}
				else if (charaPhase < 4.5)
				{
					charaAlpha = (int)((4.5 - charaPhase) * 255.0);
				}
				else
				{
					charaAlpha = 0;
				}
				if (charaAlpha > 0)
				{
					if (charaAlpha == 255)
					{
						this.spriteBatch.Draw2D(this.characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - this.imageInfo.Width / 2, 600 - this.imageInfo.Height), Color.White);
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, charaIndex, 0], 505 - (int)((double)this.castNames[(int)this.layout, charaIndex, 0].Length - 0.5) * 15, 260, Color.White);
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, charaIndex, 1], 505 - (int)((double)this.castNames[(int)this.layout, charaIndex, 1].Length - 0.5) * 15, 300, Color.White);
					}
					else
					{
						this.spriteBatch.Draw2D(this.characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - this.imageInfo.Width / 2, 600 - this.imageInfo.Height), Color.FromArgb(charaAlpha, Color.White));
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, charaIndex, 0], 505 - (int)((double)this.castNames[(int)this.layout, charaIndex, 0].Length - 0.5) * 15, 260, Color.FromArgb(charaAlpha, Color.White));
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, charaIndex, 1], 505 - (int)((double)this.castNames[(int)this.layout, charaIndex, 1].Length - 0.5) * 15, 300, Color.FromArgb(charaAlpha, Color.White));
					}
				}
			}
			else if (seconds < 171.0)
			{
				int bgIndex = (int)(seconds - 38.0) / 19;
				double bgPhase = seconds - 38.0 - (double)(bgIndex * 19);
				if (this.backgroundIndex != bgIndex)
				{
					if (this.backgroundTexture != null)
					{
						this.backgroundTexture.Dispose();
					}
					this.imageInfo = TextureLoader.ImageInformationFromStream(this.bgArchive.ReadEntry(this.backgroundTextureNames[bgIndex]));
					this.backgroundTexture = TextureLoader.FromStream(this.graphicsDevice, this.bgArchive.ReadEntry(this.backgroundTextureNames[bgIndex]), this.imageInfo.Width, this.imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					this.backgroundIndex = bgIndex;
				}
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				if (bgPhase < 0.5)
				{
				}
				int bgAlpha;
				if (bgPhase < 2.5)
				{
					bgAlpha = (int)((bgPhase - 0.5) / 2.0 * 255.0);
				}
				else if (bgPhase < 16.5)
				{
					bgAlpha = 255;
				}
				else if (bgPhase < 18.5)
				{
					bgAlpha = (int)((18.5 - bgPhase) / 2.0 * 255.0);
				}
				else
				{
					bgAlpha = 0;
				}
				if (bgAlpha > 0)
				{
					if (bgAlpha == 255)
					{
						this.spriteBatch.Draw2D(this.backgroundTexture, Rectangle.Empty, this.backgroundSourceRect, Point.Empty, 0f, this.backgroundPosition, Color.White);
					}
					else
					{
						this.spriteBatch.Draw2D(this.backgroundTexture, Rectangle.Empty, this.backgroundSourceRect, Point.Empty, 0f, this.backgroundPosition, Color.FromArgb(bgAlpha, Color.White));
					}
					this.spriteBatch.Draw2D(this.overlayTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, Point.Empty, Color.Transparent);
				}
				if (seconds < 163.0)
				{
					bgPhase = seconds - 38.0;
					double scrollPosition = bgPhase / 125.0 * (double)this.staffLineCount;
					double firstVisible = scrollPosition - 15.0;
					for (int i = (int)firstVisible; i <= (int)scrollPosition; i++)
					{
						if (i >= 0 && i < this.staffLineCount - 15 && this.staffNodes[0].ChildNodes[i].HasChildNodes)
						{
							this.textFont.DrawText(this.spriteBatch, this.staffNodes[(int)this.layout].ChildNodes[i].FirstChild.Value, 92, (int)(((double)i - firstVisible) * 40.0) + 2, Color.Black);
							this.textFont.DrawText(this.spriteBatch, this.staffNodes[(int)this.layout].ChildNodes[i].FirstChild.Value, 90, (int)(((double)i - firstVisible) * 40.0), Color.White);
						}
					}
				}
			}
			else if (seconds * 1000.0 < (double)this.edPackage.GetLength())
			{
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
			}
			else
			{
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
				this.edPackage.Close();
				running = false;
			}
			this.spriteBatch.Draw2D(this.overlayTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, this.barPosition, Color.Black);
			int j = 0;
			while (j < this.musicNodes[(int)this.layout].ChildNodes.Count - 1)
			{
				double lineIndex = double.Parse(this.musicNodes[(int)this.layout].ChildNodes[j].Attributes["start"].Value);
				double scrollEnd = double.Parse(this.musicNodes[(int)this.layout].ChildNodes[j].Attributes["end"].Value);
				double nextStart = double.Parse(this.musicNodes[(int)this.layout].ChildNodes[j + 1].Attributes["start"].Value);
				if (seconds > lineIndex && seconds < nextStart)
				{
					string value = this.musicNodes[(int)this.layout].ChildNodes[j].FirstChild.Value;
					for (int k = 0; k < value.Length; k++)
					{
						this.textFont.DrawText(this.spriteBatch, value[k].ToString(), 400 - value.Length * 15 + k * 30 + 2, 562, Color.White);
					}
					if (seconds < scrollEnd)
					{
						double charProgress = (seconds - lineIndex) / (scrollEnd - lineIndex) * (double)value.Length;
						for (int l = 0; l < (int)charProgress; l++)
						{
							this.textFont.DrawText(this.spriteBatch, value[l].ToString(), 400 - value.Length * 15 + l * 30, 560, Color.Yellow);
						}
						this.textFont.DrawText(this.spriteBatch, value[(int)charProgress].ToString(), 400 - value.Length * 15 + (int)charProgress * 30, 560, Color.FromArgb((int)((charProgress - (double)((int)charProgress)) * 255.0), Color.Yellow));
						break;
					}
					int m = 0;
					while (m < value.Length)
					{
						this.textFont.DrawText(this.spriteBatch, value[m].ToString(), 400 - value.Length * 15 + m * 30, 560, Color.Yellow);
						int unused = m++;
					}
					break;
				}
				else
				{
					j++;
				}
			}
			this.spriteBatch.End();
			this.graphicsDevice.EndScene();
			try
			{
				this.graphicsDevice.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					this.graphicsDevice.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					this.deviceResetHandler();
				}
				catch
				{
				}
			}
			return running;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x00018D9C File Offset: 0x00017D9C
		public void Stop()
		{
			this.edPackage.Close();
			if (this.characterTexture != null)
			{
				this.characterTexture.Dispose();
			}
			if (this.backgroundTexture != null)
			{
				this.backgroundTexture.Dispose();
			}
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x00018DDC File Offset: 0x00017DDC
		public void SetPaused(bool paused)
		{
			if (paused)
			{
				this.edPackage.Pause();
				return;
			}
			this.edPackage.Resume();
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x00018DF8 File Offset: 0x00017DF8
		public void SetButtonLayout(ButtonLayout buttonLayout)
		{
			this.layout = buttonLayout;
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x00018E04 File Offset: 0x00017E04
		public void Dispose()
		{
			this.edPackage.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x040000EB RID: 235
		protected const int fontSize = 24;

		// Token: 0x040000EC RID: 236
		protected const int charWidth = 15;

		// Token: 0x040000ED RID: 237
		private MediaPackage edPackage;

		// Token: 0x040000EE RID: 238
		private ArchiveFile charaArchive;

		// Token: 0x040000EF RID: 239
		private ArchiveFile bgArchive;

		// Token: 0x040000F0 RID: 240
		private XmlNode[] xmlDocuments;

		// Token: 0x040000F1 RID: 241
		private XmlNode[] staffNodes = new XmlNode[3];

		// Token: 0x040000F2 RID: 242
		private XmlNode[] musicNodes = new XmlNode[3];

		// Token: 0x040000F3 RID: 243
		private Texture characterTexture;

		// Token: 0x040000F4 RID: 244
		private ImageInformation imageInfo;

		// Token: 0x040000F5 RID: 245
		private Texture backgroundTexture;

		// Token: 0x040000F6 RID: 246
		private Device graphicsDevice;

		// Token: 0x040000F7 RID: 247
		private Sprite spriteBatch;

		// Token: 0x040000F8 RID: 248
		private FrameCallback deviceResetHandler;

		// Token: 0x040000F9 RID: 249
		private Microsoft.DirectX.Direct3D.Font textFont;

		// Token: 0x040000FA RID: 250
		private ButtonLayout layout;

		// Token: 0x040000FB RID: 251
		protected Texture overlayTexture;

		// Token: 0x040000FC RID: 252
		protected readonly string[,] titleLines;

		// Token: 0x040000FD RID: 253
		protected readonly string[] characterTextureNames;

		// Token: 0x040000FE RID: 254
		protected readonly string[,,] castNames;

		// Token: 0x040000FF RID: 255
		protected readonly string[] backgroundTextureNames;

		// Token: 0x04000100 RID: 256
		protected readonly Rectangle backgroundSourceRect;

		// Token: 0x04000101 RID: 257
		protected readonly Point backgroundPosition;

		// Token: 0x04000102 RID: 258
		protected readonly Point barPosition;

		// Token: 0x04000103 RID: 259
		protected readonly Size characterNameSize;

		// Token: 0x04000104 RID: 260
		private int staffLineCount;

		// Token: 0x04000105 RID: 261
		private bool started;

		// Token: 0x04000106 RID: 262
		private int characterIndex;

		// Token: 0x04000107 RID: 263
		private int backgroundIndex;
	}
}
