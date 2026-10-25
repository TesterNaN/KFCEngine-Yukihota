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
			string[,] array = new string[3, 2];
			array[0, 0] = "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪萤～";
			array[0, 1] = "ＣＡＳＴ";
			array[1, 0] = "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪螢～";
			array[1, 1] = "ＣＡＳＴ";
			array[2, 0] = "Ｍｅｍｏｒｉｅｓ\u3000Ｏｆｆ\u3000２ｎｄ～雪蛍～";
			array[2, 1] = "ＣＡＳＴ";
			this.titleLines = array;
			this.characterTextureNames = new string[] { "SH01CMA", "IK01CMA", "TT01CMA", "SS01AMA", "NS01CMA", "IS01AMA" };
			string[,,] array2 = new string[3, 6, 2];
			array2[0, 0, 0] = "白河 萤";
			array2[0, 0, 1] = "水树 奈奈";
			array2[0, 1, 0] = "伊波 健";
			array2[0, 1, 1] = "小尾 元政";
			array2[0, 2, 0] = "飞世 巴";
			array2[0, 2, 1] = "仲西 环";
			array2[0, 3, 0] = "白河 静流";
			array2[0, 3, 1] = "菊池 志穗";
			array2[0, 4, 0] = "中森 翔太";
			array2[0, 4, 1] = "福山 润";
			array2[0, 5, 0] = "稻穗 信";
			array2[0, 5, 1] = "间岛 淳司";
			array2[1, 0, 0] = "白河 螢";
			array2[1, 0, 1] = "水樹 奈奈";
			array2[1, 1, 0] = "伊波 健";
			array2[1, 1, 1] = "小尾 元政";
			array2[1, 2, 0] = "飛世 巴";
			array2[1, 2, 1] = "仲西 環";
			array2[1, 3, 0] = "白河 靜流";
			array2[1, 3, 1] = "菊池 志穗";
			array2[1, 4, 0] = "中森 翔太";
			array2[1, 4, 1] = "福山 潤";
			array2[1, 5, 0] = "稻穗 信";
			array2[1, 5, 1] = "間島 淳司";
			array2[2, 0, 0] = "白河 ほたる";
			array2[2, 0, 1] = "水樹 奈々";
			array2[2, 1, 0] = "伊波 健";
			array2[2, 1, 1] = "小尾 元政";
			array2[2, 2, 0] = "飛世 巴";
			array2[2, 2, 1] = "仲西 環";
			array2[2, 3, 0] = "白河 静流";
			array2[2, 3, 1] = "菊池 志穂";
			array2[2, 4, 0] = "中森 翔太";
			array2[2, 4, 1] = "福山 潤";
			array2[2, 5, 0] = "稲穂 信";
			array2[2, 5, 1] = "間島 淳司";
			this.castNames = array2;
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
			bool flag = true;
			this.graphicsDevice.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			this.graphicsDevice.BeginScene();
			this.spriteBatch.Begin(SpriteFlags.AlphaBlend);
			double num = (double)this.edPackage.GetPosition() / 1000.0;
			if (num < 8.0)
			{
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				int num2 = (int)num / 4;
				double num3 = num - (double)(num2 * 4);
				int num4;
				if (num3 < 1.0)
				{
					num4 = (int)(num3 * 255.0);
				}
				else if (num3 < 3.0)
				{
					num4 = 255;
				}
				else
				{
					num4 = (int)((4.0 - num3) * 255.0);
				}
				if (num4 == 255)
				{
					this.textFont.DrawText(this.spriteBatch, this.titleLines[(int)this.layout, num2], 400 - this.titleLines[(int)this.layout, num2].Length * 15, 280, Color.White);
				}
				else
				{
					this.textFont.DrawText(this.spriteBatch, this.titleLines[(int)this.layout, num2], 400 - this.titleLines[(int)this.layout, num2].Length * 15, 280, Color.FromArgb(num4, Color.White));
				}
			}
			else if (num < 38.0)
			{
				if (this.backgroundTexture != null)
				{
					this.backgroundTexture.Dispose();
				}
				int num2 = (int)(num - 8.0) / 5;
				double num3 = num - 8.0 - (double)(num2 * 5);
				if (this.characterIndex != num2)
				{
					if (this.characterTexture != null)
					{
						this.characterTexture.Dispose();
					}
					this.imageInfo = TextureLoader.ImageInformationFromStream(this.charaArchive.ReadEntry(this.characterTextureNames[num2]));
					this.characterTexture = TextureLoader.FromStream(this.graphicsDevice, this.charaArchive.ReadEntry(this.characterTextureNames[num2]), this.imageInfo.Width, this.imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					this.characterIndex = num2;
				}
				if (num3 < 0.5)
				{
				}
				int num5;
				if (num3 < 1.5)
				{
					num5 = (int)((num3 - 0.5) * 255.0);
				}
				else if (num3 < 3.5)
				{
					num5 = 255;
				}
				else if (num3 < 4.5)
				{
					num5 = (int)((4.5 - num3) * 255.0);
				}
				else
				{
					num5 = 0;
				}
				if (num5 > 0)
				{
					if (num5 == 255)
					{
						this.spriteBatch.Draw2D(this.characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - this.imageInfo.Width / 2, 600 - this.imageInfo.Height), Color.White);
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, num2, 0], 505 - (int)((double)this.castNames[(int)this.layout, num2, 0].Length - 0.5) * 15, 260, Color.White);
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, num2, 1], 505 - (int)((double)this.castNames[(int)this.layout, num2, 1].Length - 0.5) * 15, 300, Color.White);
					}
					else
					{
						this.spriteBatch.Draw2D(this.characterTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, new Point(250 - this.imageInfo.Width / 2, 600 - this.imageInfo.Height), Color.FromArgb(num5, Color.White));
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, num2, 0], 505 - (int)((double)this.castNames[(int)this.layout, num2, 0].Length - 0.5) * 15, 260, Color.FromArgb(num5, Color.White));
						this.textFont.DrawText(this.spriteBatch, this.castNames[(int)this.layout, num2, 1], 505 - (int)((double)this.castNames[(int)this.layout, num2, 1].Length - 0.5) * 15, 300, Color.FromArgb(num5, Color.White));
					}
				}
			}
			else if (num < 171.0)
			{
				int num2 = (int)(num - 38.0) / 19;
				double num3 = num - 38.0 - (double)(num2 * 19);
				if (this.backgroundIndex != num2)
				{
					if (this.backgroundTexture != null)
					{
						this.backgroundTexture.Dispose();
					}
					this.imageInfo = TextureLoader.ImageInformationFromStream(this.bgArchive.ReadEntry(this.backgroundTextureNames[num2]));
					this.backgroundTexture = TextureLoader.FromStream(this.graphicsDevice, this.bgArchive.ReadEntry(this.backgroundTextureNames[num2]), this.imageInfo.Width, this.imageInfo.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					this.backgroundIndex = num2;
				}
				if (this.characterTexture != null)
				{
					this.characterTexture.Dispose();
				}
				if (num3 < 0.5)
				{
				}
				int num6;
				if (num3 < 2.5)
				{
					num6 = (int)((num3 - 0.5) / 2.0 * 255.0);
				}
				else if (num3 < 16.5)
				{
					num6 = 255;
				}
				else if (num3 < 18.5)
				{
					num6 = (int)((18.5 - num3) / 2.0 * 255.0);
				}
				else
				{
					num6 = 0;
				}
				if (num6 > 0)
				{
					if (num6 == 255)
					{
						this.spriteBatch.Draw2D(this.backgroundTexture, Rectangle.Empty, this.backgroundSourceRect, Point.Empty, 0f, this.backgroundPosition, Color.White);
					}
					else
					{
						this.spriteBatch.Draw2D(this.backgroundTexture, Rectangle.Empty, this.backgroundSourceRect, Point.Empty, 0f, this.backgroundPosition, Color.FromArgb(num6, Color.White));
					}
					this.spriteBatch.Draw2D(this.overlayTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, Point.Empty, Color.Transparent);
				}
				if (num < 163.0)
				{
					num3 = num - 38.0;
					double num7 = num3 / 125.0 * (double)this.staffLineCount;
					double num8 = num7 - 15.0;
					for (int i = (int)num8; i <= (int)num7; i++)
					{
						if (i >= 0 && i < this.staffLineCount - 15 && this.staffNodes[0].ChildNodes[i].HasChildNodes)
						{
							this.textFont.DrawText(this.spriteBatch, this.staffNodes[(int)this.layout].ChildNodes[i].FirstChild.Value, 92, (int)(((double)i - num8) * 40.0) + 2, Color.Black);
							this.textFont.DrawText(this.spriteBatch, this.staffNodes[(int)this.layout].ChildNodes[i].FirstChild.Value, 90, (int)(((double)i - num8) * 40.0), Color.White);
						}
					}
				}
			}
			else if (num * 1000.0 < (double)this.edPackage.GetLength())
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
				flag = false;
			}
			this.spriteBatch.Draw2D(this.overlayTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, 0f, this.barPosition, Color.Black);
			int j = 0;
			while (j < this.musicNodes[(int)this.layout].ChildNodes.Count - 1)
			{
				double num9 = double.Parse(this.musicNodes[(int)this.layout].ChildNodes[j].Attributes["start"].Value);
				double num10 = double.Parse(this.musicNodes[(int)this.layout].ChildNodes[j].Attributes["end"].Value);
				double num11 = double.Parse(this.musicNodes[(int)this.layout].ChildNodes[j + 1].Attributes["start"].Value);
				if (num > num9 && num < num11)
				{
					string value = this.musicNodes[(int)this.layout].ChildNodes[j].FirstChild.Value;
					for (int k = 0; k < value.Length; k++)
					{
						this.textFont.DrawText(this.spriteBatch, value[k].ToString(), 400 - value.Length * 15 + k * 30 + 2, 562, Color.White);
					}
					if (num < num10)
					{
						double num12 = (num - num9) / (num10 - num9) * (double)value.Length;
						for (int l = 0; l < (int)num12; l++)
						{
							this.textFont.DrawText(this.spriteBatch, value[l].ToString(), 400 - value.Length * 15 + l * 30, 560, Color.Yellow);
						}
						this.textFont.DrawText(this.spriteBatch, value[(int)num12].ToString(), 400 - value.Length * 15 + (int)num12 * 30, 560, Color.FromArgb((int)((num12 - (double)((int)num12)) * 255.0), Color.Yellow));
						break;
					}
					int m = 0;
					while (m < value.Length)
					{
						this.textFont.DrawText(this.spriteBatch, value[m].ToString(), 400 - value.Length * 15 + m * 30, 560, Color.Yellow);
						int num13 = m++;
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
			return flag;
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
