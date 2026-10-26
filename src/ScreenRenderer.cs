using System;
using System.Collections;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Yukihota
{
	// Token: 0x0200003F RID: 63
	public class ScreenRenderer : IDisposable
	{
		// Token: 0x06001F8A RID: 8074 RVA: 0x0001AE1C File Offset: 0x00019E1C
		public ScreenRenderer(Device d3dDevice, Sprite d3dSprite, FrameCallback frameCallback, Texture defaultTexture, System.Drawing.Font font, ButtonLayout buttonLayout, Color color, AudioManager audio)
		{
			this.device = d3dDevice;
			this.sprite = d3dSprite;
			this.resetCallback = frameCallback;
			this.whiteTexture = defaultTexture;
			this.dialogueColor = color;
			this.baseFont = font;
			this.layoutIndex = buttonLayout;
			this.audioManager = audio;
			this.dialogueHidden = false;
			this.modeIconIndex = 0;
			this.systemArchive = new ArchiveFile("System", ArchiveMode.Read);
			this.InitNextIndicator();
			this.InitBackground();
			this.InitEffects();
			this.InitCharacters();
			this.InitNotice();
			this.InitMessage();
			this.InitDialogueFrame();
			this.InitCaption();
			this.InitModeIcon();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x0001B3E4 File Offset: 0x0001A3E4
		~ScreenRenderer()
		{
			this.textureQueue.Clear();
			this.destRectQueue.Clear();
			this.srcRectQueue.Clear();
			this.drawPositionQueue.Clear();
			this.colorQueue.Clear();
			this.textQueue.Clear();
			this.startIndexQueue.Clear();
			this.endIndexQueue.Clear();
			this.textPositionQueue.Clear();
			this.charWidthQueue.Clear();
			this.lineHeightQueue.Clear();
			this.shadowOffsetQueue.Clear();
			this.fontSizeModeQueue.Clear();
			this.lineWidthQueue.Clear();
			this.fontIndexQueue.Clear();
			this.fadeOutQueue.Clear();
			this.fadeLengthQueue.Clear();
			this.textColorQueue.Clear();
			this.shadowColorQueue.Clear();
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0001B4F0 File Offset: 0x0001A4F0
		public void Render()
		{
			this.device.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			this.textureQueue.Clear();
			this.destRectQueue.Clear();
			this.srcRectQueue.Clear();
			this.drawPositionQueue.Clear();
			this.colorQueue.Clear();
			this.textQueue.Clear();
			this.startIndexQueue.Clear();
			this.endIndexQueue.Clear();
			this.textPositionQueue.Clear();
			this.charWidthQueue.Clear();
			this.lineHeightQueue.Clear();
			this.shadowOffsetQueue.Clear();
			this.fontSizeModeQueue.Clear();
			this.lineWidthQueue.Clear();
			this.fontIndexQueue.Clear();
			this.fadeOutQueue.Clear();
			this.fadeLengthQueue.Clear();
			this.textColorQueue.Clear();
			this.shadowColorQueue.Clear();
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			this.DrawBackground();
			this.DrawBackgroundEffects();
			this.DrawCharacters();
			this.DrawForegroundEffects();
			this.DrawMessage();
			if (!this.dialogueHidden)
			{
				this.DrawCaption();
			}
			this.FadeOutBackground();
			this.DrawNotice();
			this.DrawModeIcon();
			this.sprite.End();
			this.device.EndScene();
			try
			{
				this.device.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					this.device.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					this.resetCallback();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0001B6EC File Offset: 0x0001A6EC
		public void Repaint()
		{
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			int queueIndex = 0;
			for (int i = 0; i < this.textureQueue.Count; i++)
			{
				if (this.textureQueue[i] != null)
				{
					this.sprite.Draw2D((Texture)this.textureQueue[i], (Rectangle)this.destRectQueue[i], (Rectangle)this.srcRectQueue[i], Point.Empty, 0f, (Point)this.drawPositionQueue[i], (Color)this.colorQueue[i]);
				}
				else
				{
					Microsoft.DirectX.Direct3D.Font font;
					switch ((int)this.fontIndexQueue[queueIndex])
					{
					case 1:
						font = this.noticeFont;
						break;
					case 2:
						font = this.messageFont;
						break;
					case 3:
						font = this.captionFont;
						break;
					default:
						font = this.noticeFont;
						break;
					}
					this.DrawTextCore((string)this.textQueue[queueIndex], (int)this.startIndexQueue[queueIndex], (double)this.endIndexQueue[queueIndex], (Point)this.textPositionQueue[queueIndex], (int)this.charWidthQueue[queueIndex], (int)this.lineHeightQueue[queueIndex], (int)this.shadowOffsetQueue[queueIndex], (HorizontalAlign)this.fontSizeModeQueue[queueIndex], (int)this.lineWidthQueue[queueIndex], font, (bool)this.fadeOutQueue[queueIndex], (int)this.fadeLengthQueue[queueIndex], (Color)this.textColorQueue[queueIndex], (Color)this.shadowColorQueue[queueIndex], false);
					queueIndex++;
				}
			}
			this.sprite.End();
			this.device.EndScene();
			try
			{
				this.device.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					this.device.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					this.resetCallback();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0001BA00 File Offset: 0x0001AA00
		public bool IsBusy()
		{
			if (this.backgroundFadeCounter > 0 || this.zoomCounter > 0 || this.dimCounter > 0 || this.messageFadeCounter > 0 || this.dialogueCounter > 0 || (this.shakeFrame > 0 && !this.shakeLoop))
			{
				return true;
			}
			int i = 0;
			while (i < 3)
			{
				if (this.characterAppearCounters[i] > 0 || this.characterShakeCounters[i] > 0)
				{
					return true;
				}
				int index = i++;
			}
			if (this.messageLines != null)
			{
				switch (this.messageAlignMode)
				{
				case TextRevealMode.Typewriter:
					if (this.messageProgress < (double)(this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex] + 4))
					{
						return true;
					}
					break;
				case TextRevealMode.Fade:
					if (this.lineRevealCounter > 0)
					{
						return true;
					}
					break;
				}
			}
			return this.captionLines != null && this.captionProgress < (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4);
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0001BB24 File Offset: 0x0001AB24
		public void HideDialogue(bool content)
		{
			this.dialogueHidden = content;
			if (content && this.captionLines != null)
			{
				this.captionProgress = (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4);
				this.captionVisibleChars = this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex];
			}
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x0001BB98 File Offset: 0x0001AB98
		public void HideDialogue()
		{
			this.dialogueHidden = !this.dialogueHidden;
			if (this.dialogueHidden && this.captionLines != null && this.captionVoiceLength == 0.0)
			{
				this.captionProgress = (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4);
				this.captionVisibleChars = this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex];
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x0001BC2C File Offset: 0x0001AC2C
		public void FastForward()
		{
			if (this.backgroundFadeCounter > 0)
			{
				this.backgroundFadeCounter = 0;
				if (this.backgroundWipeActive)
				{
					this.backgroundWipeActive = false;
				}
				else
				{
					this.backgroundRedrawPending = true;
				}
			}
			if (this.zoomCounter > 0)
			{
				this.zoomCounter = 0;
			}
			if (this.dimCounter > 0)
			{
				this.dimCounter = 0;
				if (this.dimReverse)
				{
					this.dimActive = false;
				}
			}
			if (this.thinkCounter > 0)
			{
				this.thinkCounter = 0;
				if (this.thinkReverse)
				{
					this.thinkActive = false;
				}
			}
			if (this.dialogueCounter > 0)
			{
				this.dialogueCounter = 0;
				if (this.dialogueClosing)
				{
					this.dialogueActive = false;
				}
			}
			if (this.shakeFrame > 0 && !this.shakeLoop)
			{
				this.shakeFrame = 1;
			}
			for (int i = 0; i < 3; i++)
			{
				if (this.characterAppearCounters[i] > 0)
				{
					this.characterAppearCounters[i] = 0;
				}
				if (this.characterShakeCounters[i] > 0)
				{
					this.characterShakeCounters[i] = 1;
				}
			}
			if (this.messageLines != null)
			{
				if (this.messageFadeCounter > 0)
				{
					this.messageFadeCounter = 0;
					this.messageProgress = 0.0;
					this.lineRevealCounter = 0;
					this.messageLines = null;
					for (int j = 0; j < 3; j++)
					{
						this.messageStartIndexes[j] = 0;
					}
					this.messageVisibleChars = 0;
				}
				else
				{
					this.messageVisibleChars = this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex];
					switch (this.messageAlignMode)
					{
					case TextRevealMode.Typewriter:
						this.messageProgress = (double)(this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex] + 4);
						break;
					case TextRevealMode.Fade:
						this.lineRevealCounter = 0;
						break;
					}
				}
			}
			if (this.captionLines != null)
			{
				this.captionProgress = (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4);
				this.captionVisibleChars = this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex];
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x0001BE8C File Offset: 0x0001AE8C
		public void SetButtonLayout(ButtonLayout buttonLayout)
		{
			if (this.layoutIndex != buttonLayout)
			{
				this.layoutIndex = buttonLayout;
				this.RebuildNoticeFont();
				this.RebuildMessageFont();
				this.RebuildCaptionFont();
			}
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x0001BEB0 File Offset: 0x0001AEB0
		public void SetFont(System.Drawing.Font font)
		{
			if (this.baseFont != font)
			{
				this.baseFont = font;
				this.RebuildNoticeFont();
				this.RebuildMessageFont();
				this.RebuildCaptionFont();
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x0001BED4 File Offset: 0x0001AED4
		private void QueueDraw(Texture texture, Rectangle destRect, Rectangle srcRect, Point position, Color color)
		{
			this.sprite.Draw2D(texture, destRect, srcRect, Point.Empty, 0f, position, color);
			this.textureQueue.Add(texture);
			this.destRectQueue.Add(destRect);
			this.srcRectQueue.Add(srcRect);
			this.drawPositionQueue.Add(position);
			this.colorQueue.Add(color);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x0001BF54 File Offset: 0x0001AF54
		public void Dispose()
		{
			this.DisposeBackground();
			this.DisposeEffects();
			this.DisposeCharacters();
			this.DisposeNoticeFont();
			this.DisposeMessageFont();
			this.DisposeDialogueFrame();
			this.DisposeCaptionFont();
			this.DisposeModeIcon();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x0001BF8C File Offset: 0x0001AF8C
		public void SetFadeEnabled(bool value)
		{
			if (this.fadeEnabled != value)
			{
				this.RebuildNoticeFont();
				this.RebuildMessageFont();
				this.RebuildCaptionFont();
			}
			this.fadeEnabled = value;
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x0001BFB0 File Offset: 0x0001AFB0
		public void SetSunEffectEnabled(bool value)
		{
			this.sunEffectEnabled = value;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x0001BFBC File Offset: 0x0001AFBC
		public void SetEffectEnabled(bool value)
		{
			this.effectEnabled = value;
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x0001BFC8 File Offset: 0x0001AFC8
		public void SetShockEffectEnabled(bool value)
		{
			this.shockEffectEnabled = value;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x0001BFD4 File Offset: 0x0001AFD4
		public void SetDialogueColor(Color value)
		{
			this.dialogueColor = value;
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0001BFE0 File Offset: 0x0001AFE0
		public void SetMessageSpeed(int value)
		{
			if (value > 0)
			{
				this.messageSpeed = value;
				return;
			}
			throw new Exception("Invalid Speed");
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0001BFF8 File Offset: 0x0001AFF8
		public void SetSyncVoice(bool value)
		{
			this.syncVoice = value;
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x0001C004 File Offset: 0x0001B004
		private void InitNextIndicator()
		{
			this.indicatorPhase = 0;
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x0001C010 File Offset: 0x0001B010
		private void DrawNextIndicator(int x, int y)
		{
			Rectangle rectangle = new Rectangle(this.indicatorColumns[this.indicatorPhase % 16] * 32, 0, 32, 32);
			Point point = new Point(x + this.indicatorOffsetX[this.indicatorPhase], y + this.indicatorOffsetY[this.indicatorPhase]);
			this.QueueDraw(this.modeIconTexture, rectangle, Rectangle.Empty, point, Color.White);
			this.indicatorPhase++;
			this.indicatorPhase %= 32;
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x0001C09C File Offset: 0x0001B09C
		private Point DrawTextCore(string content, Point origin, int charWidth, int lineHeight, int shadowOffset, Microsoft.DirectX.Direct3D.Font font, Color textColor, Color shadowColor)
		{
			return this.DrawTextCore(content, 0, content.Length, origin, charWidth, lineHeight, shadowOffset, HorizontalAlign.Left, 0, font, textColor, shadowColor);
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x0001C0C8 File Offset: 0x0001B0C8
		private Point DrawTextCore(string content, int startIndex, int endIndex, Point origin, int charWidth, int lineHeight, int shadowOffset, HorizontalAlign fontSizeMode, int lineWidth, Microsoft.DirectX.Direct3D.Font font, Color textColor, Color shadowColor)
		{
			return this.DrawTextCore(content, startIndex, (double)endIndex, origin, charWidth, lineHeight, shadowOffset, fontSizeMode, lineWidth, font, false, 0, textColor, shadowColor);
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x0001C0F4 File Offset: 0x0001B0F4
		private Point DrawTextCore(string content, double endIndex, Point origin, int charWidth, int lineHeight, int shadowOffset, Microsoft.DirectX.Direct3D.Font font, int fadeLength, Color textColor, Color shadowColor)
		{
			return this.DrawTextCore(content, 0, endIndex, origin, charWidth, lineHeight, shadowOffset, HorizontalAlign.Left, 0, font, true, fadeLength, textColor, shadowColor);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x0001C11C File Offset: 0x0001B11C
		private Point DrawTextCore(string content, double endIndex, Point origin, int charWidth, int lineHeight, int shadowOffset, HorizontalAlign fontSizeMode, int lineWidth, Microsoft.DirectX.Direct3D.Font font, int fadeLength, Color textColor, Color shadowColor)
		{
			return this.DrawTextCore(content, 0, endIndex, origin, charWidth, lineHeight, shadowOffset, fontSizeMode, lineWidth, font, true, fadeLength, textColor, shadowColor);
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x0001C148 File Offset: 0x0001B148
		private Point DrawTextCore(string content, int startIndex, double endIndex, Point origin, int charWidth, int lineHeight, int shadowOffset, HorizontalAlign fontSizeMode, int lineWidth, Microsoft.DirectX.Direct3D.Font font, bool scroll, int fadeLength, Color textColor, Color shadowColor)
		{
			return this.DrawTextCore(content, startIndex, endIndex, origin, charWidth, lineHeight, shadowOffset, fontSizeMode, lineWidth, font, scroll, fadeLength, textColor, shadowColor, true);
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0001C178 File Offset: 0x0001B178
		private Point DrawTextCore(string content, int startIndex, double endIndex, Point origin, int charWidth, int lineHeight, int shadowOffset, HorizontalAlign fontSizeMode, int lineWidth, Microsoft.DirectX.Direct3D.Font font, bool scroll, int fadeLength, Color textColor, Color shadowColor, bool enqueue)
		{
			if (enqueue)
			{
				this.textureQueue.Add(null);
				this.destRectQueue.Add(null);
				this.srcRectQueue.Add(null);
				this.drawPositionQueue.Add(null);
				this.colorQueue.Add(null);
				this.textQueue.Add(content);
				this.startIndexQueue.Add(startIndex);
				this.endIndexQueue.Add(endIndex);
				this.textPositionQueue.Add(origin);
				this.charWidthQueue.Add(charWidth);
				this.lineHeightQueue.Add(lineHeight);
				this.shadowOffsetQueue.Add(shadowOffset);
				this.fontSizeModeQueue.Add(fontSizeMode);
				this.lineWidthQueue.Add(lineWidth);
				this.fadeOutQueue.Add(scroll);
				this.fadeLengthQueue.Add(fadeLength);
				this.textColorQueue.Add(textColor);
				this.shadowColorQueue.Add(shadowColor);
			}
			int penX;
			switch (fontSizeMode)
			{
			case HorizontalAlign.Left:
				penX = origin.X;
				break;
			case HorizontalAlign.Center:
			{
				int lineEnd = content.IndexOf('\n');
				if (lineEnd == -1)
				{
					lineEnd = content.Length;
				}
				penX = origin.X + charWidth * (lineWidth - lineEnd) / 2;
				break;
			}
			case HorizontalAlign.Right:
			{
				int lineEnd = content.IndexOf('\n');
				if (lineEnd == -1)
				{
					lineEnd = content.Length;
				}
				penX = origin.X + charWidth * (lineWidth - lineEnd);
				break;
			}
			default:
				penX = origin.X;
				break;
			}
			int penY = origin.Y;
			int i = 0;
			while (i < content.Length)
			{
				if (content[i] == '\n')
				{
					switch (fontSizeMode)
					{
					case HorizontalAlign.Left:
						penX = origin.X;
						break;
					case HorizontalAlign.Center:
					{
						int lineEnd = content.IndexOf('\n', i + 1);
						if (lineEnd == -1)
						{
							lineEnd = content.Length;
						}
						int lineCharCount = lineEnd - i - 1;
						penX = origin.X + charWidth * (lineWidth - lineCharCount) / 2;
						break;
					}
					case HorizontalAlign.Right:
					{
						int lineEnd = content.IndexOf('\n', i + 1);
						if (lineEnd == -1)
						{
							lineEnd = content.Length;
						}
						int lineCharCount = lineEnd - i - 1;
						penX = origin.X + charWidth * (lineWidth - lineCharCount);
						break;
					}
					}
					penY += lineHeight;
				}
				else
				{
					if (i >= startIndex && (double)i < endIndex)
					{
						char c;
						if (this.fadeEnabled)
						{
							Sprite sprite = this.sprite;
							c = content[i];
							font.DrawText(sprite, c.ToString(), penX + shadowOffset, penY + shadowOffset, shadowColor);
						}
						Sprite sprite2 = this.sprite;
						c = content[i];
						font.DrawText(sprite2, c.ToString(), penX, penY, textColor);
					}
					else if (scroll && i >= startIndex && (double)i >= endIndex && (double)i < endIndex + (double)fadeLength && this.fadeEnabled)
					{
						int fadeAlpha = (int)(255.0 * ((double)(fadeLength - i) + endIndex) / (double)fadeLength);
						Sprite sprite3 = this.sprite;
						char c = content[i];
						font.DrawText(sprite3, c.ToString(), penX + shadowOffset, penY + shadowOffset, (shadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(fadeAlpha, shadowColor));
						Sprite sprite4 = this.sprite;
						c = content[i];
						font.DrawText(sprite4, c.ToString(), penX, penY, Color.FromArgb(fadeAlpha, textColor));
					}
					penX += charWidth;
				}
				int unusedIndex = i++;
			}
			return new Point(penX, penY);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0001C5B0 File Offset: 0x0001B5B0
		private void InitBackground()
		{
			this.backgroundFadeCounter = 0;
			this.backgroundColor = (this.previousBackgroundColor = Color.White);
			this.backgroundTexture = (this.previousBackgroundTexture = (this.desaturatedBackgroundTexture = null));
			this.backgroundIsColor = (this.previousBackgroundIsColor = true);
			this.backgroundWipeActive = false;
			this.backgroundDrawnAsColor = false;
			this.backgroundRedrawPending = false;
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0001C61C File Offset: 0x0001B61C
		private void DisposeBackground()
		{
			if (this.backgroundTexture != null)
			{
				this.backgroundTexture.Dispose();
			}
			if (this.previousBackgroundTexture != null)
			{
				this.previousBackgroundTexture.Dispose();
			}
			if (this.desaturatedBackgroundTexture != null)
			{
				this.desaturatedBackgroundTexture.Dispose();
			}
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0001C674 File Offset: 0x0001B674
		public void SetBackground(Texture content, int fadeSpeed)
		{
			if (this.previousBackgroundTexture != null)
			{
				this.previousBackgroundTexture.Dispose();
			}
			this.previousBackgroundTexture = this.backgroundTexture;
			this.previousBackgroundColor = this.backgroundColor;
			this.backgroundTexture = content;
			this.backgroundColor = Color.White;
			this.previousBackgroundIsColor = this.backgroundIsColor;
			this.backgroundIsColor = false;
			this.backgroundFadeSpeed = fadeSpeed;
			this.backgroundFadeCounter = this.backgroundFadeDurations[fadeSpeed];
			this.backgroundWipeActive = false;
			if (this.thinkActive)
			{
				this.RebuildDesaturatedBackground();
				return;
			}
			if (this.desaturatedBackgroundTexture != null)
			{
				this.desaturatedBackgroundTexture.Dispose();
				this.desaturatedBackgroundTexture = null;
			}
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0001C72C File Offset: 0x0001B72C
		public void SetBackground(Color content, int fadeSpeed)
		{
			if (this.previousBackgroundTexture != null)
			{
				this.previousBackgroundTexture.Dispose();
			}
			this.previousBackgroundTexture = this.backgroundTexture;
			this.previousBackgroundColor = this.backgroundColor;
			this.backgroundColor = content;
			this.backgroundTexture = null;
			this.previousBackgroundIsColor = this.backgroundIsColor;
			this.backgroundIsColor = true;
			this.backgroundFadeSpeed = fadeSpeed;
			this.backgroundFadeCounter = this.backgroundFadeDurations[fadeSpeed];
			this.backgroundWipeActive = false;
			if (!this.thinkActive && this.desaturatedBackgroundTexture != null)
			{
				this.desaturatedBackgroundTexture.Dispose();
				this.desaturatedBackgroundTexture = null;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0001C7D8 File Offset: 0x0001B7D8
		private void DrawBackground()
		{
			if (this.backgroundFadeCounter > 0 || this.backgroundRedrawPending)
			{
				if (this.backgroundWipeActive)
				{
					if (this.previousBackgroundTexture != null)
					{
						if (this.zoomActive)
						{
							this.QueueDraw(this.previousBackgroundTexture, this.zoomSourceRect, this.screenRect, Point.Empty, Color.White);
						}
						else
						{
							this.QueueDraw(this.previousBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						}
					}
					else
					{
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, this.previousBackgroundColor);
					}
					if (this.fadeEnabled)
					{
						Point point = new Point(0, 0);
						switch (this.backgroundWipeBlend)
						{
						case TextureBlend.Normal:
							if (this.zoomActive)
							{
								this.QueueDraw(this.backgroundTexture, this.zoomTargetRect, this.screenRect, Point.Empty, Color.FromArgb((int)(255.0 * (double)(this.backgroundFadeDurations[this.backgroundFadeSpeed] - this.backgroundFadeCounter) / (double)this.backgroundFadeDurations[this.backgroundFadeSpeed]), Color.White));
							}
							else
							{
								this.QueueDraw(this.backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(this.backgroundFadeDurations[this.backgroundFadeSpeed] - this.backgroundFadeCounter) / (double)this.backgroundFadeDurations[this.backgroundFadeSpeed]), Color.White));
							}
							break;
						case TextureBlend.Left:
						{
							if (this.zoomActive)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle rectangle;
							for (int i = 0; i < 16; i++)
							{
								point.X = 832 * (this.backgroundFadeDurations[this.backgroundFadeSpeed] - this.backgroundFadeCounter) / this.backgroundFadeDurations[this.backgroundFadeSpeed] + (-i - 1) * 2;
								rectangle = new Rectangle(point.X, 0, 2, 600);
								int rowAlpha = (int)((double)(i + 1) / 17.0 * 255.0);
								this.QueueDraw(this.backgroundTexture, rectangle, Rectangle.Empty, point, Color.FromArgb(rowAlpha, Color.White));
							}
							point.X = 0;
							rectangle = new Rectangle(point.X, 0, 832 * (this.backgroundFadeDurations[this.backgroundFadeSpeed] - this.backgroundFadeCounter) / this.backgroundFadeDurations[this.backgroundFadeSpeed] - 32, 600);
							this.QueueDraw(this.backgroundTexture, rectangle, Rectangle.Empty, point, Color.White);
							break;
						}
						case TextureBlend.Right:
						{
							if (this.zoomActive)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle rectangle;
							for (int j = 0; j < 16; j++)
							{
								point.X = 832 * this.backgroundFadeCounter / this.backgroundFadeDurations[this.backgroundFadeSpeed] + (j - 16) * 2;
								rectangle = new Rectangle(point.X, 0, 2, 600);
								int rowAlpha = (int)((double)(j + 1) / 17.0 * 255.0);
								this.QueueDraw(this.backgroundTexture, rectangle, Rectangle.Empty, point, Color.FromArgb(rowAlpha, Color.White));
							}
							point.X = 832 * this.backgroundFadeCounter / this.backgroundFadeDurations[this.backgroundFadeSpeed];
							rectangle = new Rectangle(point.X, 0, 800 - point.X, 600);
							this.QueueDraw(this.backgroundTexture, rectangle, Rectangle.Empty, point, Color.White);
							break;
						}
						}
						this.backgroundFadeCounter--;
					}
					else
					{
						this.backgroundFadeCounter = 0;
					}
					if (this.backgroundFadeCounter == 0)
					{
						this.backgroundWipeActive = false;
					}
				}
				else if (!this.previousBackgroundIsColor)
				{
					if (!this.zoomActive && (!this.shakeActive || !this.shakeLoop))
					{
						if (this.previousBackgroundTexture != null)
						{
							if (this.thinkActive)
							{
								this.QueueDraw(this.desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
							else
							{
								this.QueueDraw(this.previousBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, this.previousBackgroundColor);
						}
					}
					this.backgroundDrawnAsColor = false;
				}
				else
				{
					if (!this.zoomActive && (!this.shakeActive || !this.shakeLoop))
					{
						if (this.backgroundTexture != null)
						{
							if (this.thinkActive)
							{
								this.QueueDraw(this.desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
							else
							{
								this.QueueDraw(this.backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, this.backgroundColor);
						}
					}
					this.backgroundDrawnAsColor = true;
				}
				if (this.backgroundRedrawPending)
				{
					this.backgroundRedrawPending = false;
					return;
				}
			}
			else
			{
				if (!this.zoomActive && (!this.shakeActive || !this.shakeLoop))
				{
					if (this.backgroundTexture != null)
					{
						if (this.thinkActive)
						{
							if (this.thinkCounter > 0)
							{
								float thinkProgress;
								if (!this.thinkReverse)
								{
									thinkProgress = (float)(10 - this.thinkCounter) / 10f;
								}
								else
								{
									thinkProgress = (float)this.thinkCounter / 10f;
								}
								this.QueueDraw(this.backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
								this.QueueDraw(this.desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * thinkProgress), Color.White));
							}
							else
							{
								this.QueueDraw(this.desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							this.QueueDraw(this.backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						}
					}
					else
					{
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, this.backgroundColor);
					}
				}
				this.backgroundDrawnAsColor = true;
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0001CEF4 File Offset: 0x0001BEF4
		private void FadeOutBackground()
		{
			if (this.backgroundFadeCounter > 0 && !this.backgroundWipeActive)
			{
				if (this.fadeEnabled)
				{
					if (this.backgroundDrawnAsColor && this.previousBackgroundIsColor)
					{
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)this.backgroundFadeCounter / (double)this.backgroundFadeDurations[this.backgroundFadeSpeed]), this.previousBackgroundColor));
					}
					else
					{
						if (this.backgroundDrawnAsColor || !this.backgroundIsColor)
						{
							throw new Exception("Not Implement Yet");
						}
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(this.backgroundFadeDurations[this.backgroundFadeSpeed] - this.backgroundFadeCounter) / (double)this.backgroundFadeDurations[this.backgroundFadeSpeed]), this.backgroundColor));
					}
					this.backgroundFadeCounter--;
					return;
				}
				this.backgroundFadeCounter = 0;
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0001D020 File Offset: 0x0001C020
		private void RebuildDesaturatedBackground()
		{
			if (this.desaturatedBackgroundTexture != null)
			{
				this.desaturatedBackgroundTexture.Dispose();
			}
			if (this.backgroundTexture != null)
			{
				this.desaturatedBackgroundTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("WHITE"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
				uint[,] source = (uint[,])this.backgroundTexture.LockRectangle(typeof(uint), 0, LockFlags.None, new int[] { 800, 600 });
				uint[,] desaturated = (uint[,])this.desaturatedBackgroundTexture.LockRectangle(typeof(uint), 0, LockFlags.None, new int[] { 800, 600 });
				for (int i = 0; i < 800; i++)
				{
					for (int j = 0; j < 600; j++)
					{
						uint pixel = source[i, j];
						uint gray = ((pixel & 255U) + ((pixel >> 8) & 255U) + ((pixel >> 16) & 255U)) / 3U;
						desaturated[i, j] = 4278190080U + (gray << 16) + (gray << 8) + gray;
					}
				}
				this.backgroundTexture.UnlockRectangle(0);
				this.desaturatedBackgroundTexture.UnlockRectangle(0);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0001D19C File Offset: 0x0001C19C
		private void InitEffects()
		{
			this.InitBackgroundWipe();
			this.InitZoom();
			this.InitShake();
			this.InitSunEffect();
			this.InitPetals();
			this.InitSnow();
			this.InitDimOverlay();
			this.InitThinkOverlay();
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0001D1D0 File Offset: 0x0001C1D0
		private void DisposeEffects()
		{
			this.DisposeSunEffect();
			this.DisposePetals();
			this.DisposeSnow();
			this.DisposeThinkOverlay();
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0001D1EC File Offset: 0x0001C1EC
		private void InitBackgroundWipe()
		{
			this.backgroundWipeBlend = TextureBlend.Normal;
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0001D1F8 File Offset: 0x0001C1F8
		public void SetBackgroundWithWipe(Texture newBackground, TextureBlend blend, bool animate, int fadeSpeed)
		{
			if (this.previousBackgroundTexture != null)
			{
				this.previousBackgroundTexture.Dispose();
			}
			this.previousBackgroundTexture = this.backgroundTexture;
			this.previousBackgroundColor = this.backgroundColor;
			this.backgroundTexture = newBackground;
			this.backgroundColor = Color.White;
			this.previousBackgroundIsColor = this.backgroundIsColor;
			this.backgroundIsColor = false;
			this.backgroundFadeSpeed = fadeSpeed;
			if (animate)
			{
				this.backgroundFadeCounter = this.backgroundFadeDurations[fadeSpeed];
			}
			else
			{
				this.backgroundFadeCounter = 0;
			}
			this.backgroundWipeActive = true;
			this.backgroundWipeBlend = blend;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0001D294 File Offset: 0x0001C294
		private void InitZoom()
		{
			this.zoomActive = false;
			this.zoomTargetRect = (this.zoomSourceRect = this.screenRect);
			this.zoomCounter = 0;
			this.zoomSpeedIndex = 0;
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x0001D2CC File Offset: 0x0001C2CC
		public void ZoomBackground(double left, double right, double top, double bottom, int zoomSpeed, bool animate, bool instant)
		{
			if (instant)
			{
				this.zoomActive = false;
				this.zoomSourceRect = (this.zoomTargetRect = this.screenRect);
				return;
			}
			this.zoomActive = true;
			if (this.backgroundTexture != null)
			{
				this.backgroundIsColor = false;
			}
			else if (this.previousBackgroundTexture != null)
			{
				this.previousBackgroundIsColor = false;
			}
			this.zoomSourceRect = this.zoomTargetRect;
			this.zoomTargetRect = new Rectangle((int)(left * 800.0), (int)(top * 600.0), (int)((right - left) * 800.0), (int)((bottom - top) * 600.0));
			this.zoomSpeedIndex = zoomSpeed;
			if (animate)
			{
				this.zoomCounter = this.zoomDurations[this.zoomSpeedIndex];
				return;
			}
			this.zoomCounter = 0;
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x0001D3B0 File Offset: 0x0001C3B0
		public void DrawZoomTransition()
		{
			if (this.zoomActive && !this.backgroundWipeActive)
			{
				if (this.zoomCounter > 0)
				{
					if (this.fadeEnabled)
					{
						if (!this.shakeActive || !this.shakeLoop)
						{
							double progress = (double)this.zoomCounter / (double)this.zoomDurations[this.zoomSpeedIndex];
							double inverseProgress = 1.0 - progress;
							Rectangle rectangle = new Rectangle((int)(progress * (double)this.zoomSourceRect.Left + inverseProgress * (double)this.zoomTargetRect.Left), (int)(progress * (double)this.zoomSourceRect.Top + inverseProgress * (double)this.zoomTargetRect.Top), (int)(progress * (double)this.zoomSourceRect.Width + inverseProgress * (double)this.zoomTargetRect.Width), (int)(progress * (double)this.zoomSourceRect.Height + inverseProgress * (double)this.zoomTargetRect.Height));
							if (this.backgroundTexture != null)
							{
								this.QueueDraw(this.backgroundTexture, rectangle, this.screenRect, Point.Empty, Color.White);
							}
							else if (this.previousBackgroundTexture != null)
							{
								this.QueueDraw(this.previousBackgroundTexture, rectangle, this.screenRect, Point.Empty, Color.White);
							}
						}
						this.zoomCounter--;
					}
					else
					{
						if (!this.shakeActive || !this.shakeLoop)
						{
							if (this.backgroundTexture != null)
							{
								this.QueueDraw(this.backgroundTexture, this.zoomTargetRect, this.screenRect, Point.Empty, Color.White);
							}
							else if (this.previousBackgroundTexture != null)
							{
								this.QueueDraw(this.previousBackgroundTexture, this.zoomTargetRect, this.screenRect, Point.Empty, Color.White);
							}
						}
						this.zoomCounter = 0;
					}
				}
				else if (!this.shakeActive || !this.shakeLoop)
				{
					if (this.backgroundTexture != null)
					{
						this.QueueDraw(this.backgroundTexture, this.zoomTargetRect, this.screenRect, Point.Empty, Color.White);
					}
					else if (this.previousBackgroundTexture != null)
					{
						this.QueueDraw(this.previousBackgroundTexture, this.zoomTargetRect, this.screenRect, Point.Empty, Color.White);
					}
				}
				if (!this.shakeActive || !this.shakeLoop)
				{
					this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
				}
			}
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x0001D678 File Offset: 0x0001C678
		private void InitShake()
		{
			this.shakeActive = false;
			this.shakeLoop = false;
			this.shakeFrame = 0;
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0001D690 File Offset: 0x0001C690
		public void SetShake(bool loop, bool instant)
		{
			if (!instant)
			{
				this.shakeActive = true;
				this.shakeLoop = loop;
				if (loop)
				{
					this.shakeFrame = 0;
				}
				else
				{
					this.shakeFrame = 16;
				}
				if (this.backgroundTexture != null)
				{
					this.backgroundIsColor = false;
					return;
				}
				if (this.previousBackgroundTexture != null)
				{
					this.previousBackgroundIsColor = false;
					return;
				}
			}
			else
			{
				this.shakeActive = false;
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0001D6FC File Offset: 0x0001C6FC
		private void DrawShake()
		{
			if (this.shakeActive)
			{
				if (!this.shakeLoop)
				{
					if (this.shockEffectEnabled)
					{
						if (this.backgroundTexture != null)
						{
							this.QueueDraw(this.backgroundTexture, Rectangle.Empty, Rectangle.Empty, new Point(this.shakeOffsetTableX[16 - this.shakeFrame], this.shakeOffsetTableY[16 - this.shakeFrame]), Color.FromArgb(128, Color.White));
						}
						else if (this.previousBackgroundTexture != null)
						{
							this.QueueDraw(this.previousBackgroundTexture, Rectangle.Empty, Rectangle.Empty, new Point(this.shakeOffsetTableX[16 - this.shakeFrame], this.shakeOffsetTableY[16 - this.shakeFrame]), Color.FromArgb(128, Color.White));
						}
					}
					this.shakeFrame--;
					if (this.shakeFrame == 0)
					{
						this.shakeActive = false;
						return;
					}
				}
				else
				{
					Rectangle rectangle;
					if (this.zoomActive && !this.backgroundWipeActive)
					{
						if (this.zoomCounter > 0 && this.fadeEnabled)
						{
							double progress = (double)this.zoomCounter / (double)this.zoomDurations[this.zoomSpeedIndex];
							double inverseProgress = 1.0 - progress;
							rectangle = new Rectangle((int)(progress * (double)this.zoomSourceRect.Left + inverseProgress * (double)this.zoomTargetRect.Left), (int)(progress * (double)this.zoomSourceRect.Top + inverseProgress * (double)this.zoomTargetRect.Top), (int)(progress * (double)this.zoomSourceRect.Width + inverseProgress * (double)this.zoomTargetRect.Width), (int)(progress * (double)this.zoomSourceRect.Height + inverseProgress * (double)this.zoomTargetRect.Height));
						}
						else
						{
							rectangle = this.zoomTargetRect;
						}
					}
					else
					{
						rectangle = Rectangle.Empty;
					}
					if (this.backgroundTexture != null)
					{
						this.QueueDraw(this.backgroundTexture, rectangle, this.screenRect, new Point(this.quakeOffsetTableX[this.shakeFrame], this.quakeOffsetTableY[this.shakeFrame]), Color.White);
					}
					else if (this.previousBackgroundTexture != null)
					{
						this.QueueDraw(this.previousBackgroundTexture, rectangle, this.screenRect, new Point(this.quakeOffsetTableX[this.shakeFrame], this.quakeOffsetTableY[this.shakeFrame]), Color.White);
					}
					this.shakeFrame++;
					if (this.shakeFrame == 16)
					{
						this.shakeFrame = 0;
					}
					if (rectangle != Rectangle.Empty)
					{
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
				}
			}
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x0001DA0C File Offset: 0x0001CA0C
		private void InitSunEffect()
		{
			this.sunTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("SUN"), 80, 80, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.sunActive = false;
			for (int i = 0; i < 5; i++)
			{
				this.sunLifeCounters[i] = 0;
				this.sunLifeTotals[i] = 0;
				this.sunScales[i] = 0.0;
				this.sunOffsets[i] = 0;
			}
			this.sunRandom = new Random();
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0001DA98 File Offset: 0x0001CA98
		private void DisposeSunEffect()
		{
			this.sunTexture.Dispose();
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0001DAA8 File Offset: 0x0001CAA8
		public void SetSunActive(bool instant)
		{
			if (!instant)
			{
				this.sunActive = true;
				this.backgroundIsColor = false;
				return;
			}
			this.sunActive = false;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x0001DAC4 File Offset: 0x0001CAC4
		private void DrawSunEffect()
		{
			if (this.sunActive && this.sunEffectEnabled)
			{
				int num = -1;
				for (int i = 0; i < 5; i++)
				{
					if (this.sunLifeTotals[i] == 0)
					{
						num = i;
						break;
					}
				}
				if (num >= 0 && this.sunRandom.NextDouble() < 0.25)
				{
					this.sunLifeCounters[num] = 0;
					this.sunLifeTotals[num] = (int)(20.0 + 60.0 * this.sunRandom.NextDouble());
					this.sunScales[num] = 0.5 + 0.5 * this.sunRandom.NextDouble();
					this.sunOffsets[num] = (int)(-75.0 + 150.0 * this.sunRandom.NextDouble());
				}
				for (int j = 0; j < 5; j++)
				{
					if (this.sunLifeTotals[j] > 0)
					{
						if ((double)this.sunLifeCounters[j] < (double)this.sunLifeTotals[j] / 2.0)
						{
							this.DrawSunSprite(this.sunOffsets[j], this.sunScales[j], (double)this.sunLifeCounters[j] / ((double)this.sunLifeTotals[j] / 2.0));
							int[] array;
							IntPtr intPtr;
							(array = this.sunLifeCounters)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
						}
						else if (this.sunLifeCounters[j] < this.sunLifeTotals[j])
						{
							this.DrawSunSprite(this.sunOffsets[j], this.sunScales[j], (double)(this.sunLifeTotals[j] - this.sunLifeCounters[j]) / ((double)this.sunLifeTotals[j] / 2.0));
							int[] array;
							IntPtr intPtr;
							(array = this.sunLifeCounters)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
						}
						else
						{
							this.sunLifeCounters[j] = 0;
							this.sunLifeTotals[j] = 0;
							this.sunScales[j] = 0.0;
							this.sunOffsets[j] = 0;
						}
					}
				}
				this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0001DD14 File Offset: 0x0001CD14
		private void DrawSunSprite(int origin, double scale, double fadeSpeed)
		{
			int num = (origin - 18) * 80 / (int)(450.0 * scale);
			int num2 = (-origin - 18) * 80 / (int)(450.0 * scale);
			Rectangle rectangle = new Rectangle(0, 0, 80, 80);
			Rectangle rectangle2 = new Rectangle(0, 0, (int)(450.0 * scale), (int)(450.0 * scale));
			this.QueueDraw(this.sunTexture, rectangle, rectangle2, new Point(num, num2), Color.FromArgb((int)(scale * fadeSpeed * 255.0), Color.White));
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0001DDB0 File Offset: 0x0001CDB0
		private void InitPetals()
		{
			this.petalTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("SAKURA"), this.petalFrameSize.Width, this.petalFrameSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.petalActive = false;
			for (int i = 0; i < 30; i++)
			{
				this.petalFrames[i] = 0;
				this.petalPositions[i] = Point.Empty;
				this.petalVelocitiesY[i] = 0f;
				this.petalVelocitiesX[i] = 0f;
				this.petalTargetVelocitiesY[i] = 0f;
				this.petalTargetVelocitiesX[i] = 0f;
				this.petalTurnTimers[i] = 0;
				this.petalLifeCounters[i] = 0;
			}
			this.petalActiveCount = 0;
			this.petalRandom = new Random();
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0001DEAC File Offset: 0x0001CEAC
		private void DisposePetals()
		{
			this.petalTexture.Dispose();
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0001DEBC File Offset: 0x0001CEBC
		public void SetPetalsActive(bool instant)
		{
			if (!instant)
			{
				this.petalActive = true;
				this.backgroundIsColor = false;
				this.ResetPetals();
				return;
			}
			this.petalActive = false;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0001DEE0 File Offset: 0x0001CEE0
		private void ResetPetals()
		{
			this.petalActiveCount = this.petalRandom.Next(20, 30);
			for (int i = 0; i < this.petalActiveCount; i++)
			{
				this.petalFrames[i] = this.petalRandom.Next(4);
				this.petalPositions[i] = new PointF((float)this.petalRandom.Next(-24, 800), (float)this.petalRandom.Next(-24, 600));
				this.petalVelocitiesY[i] = (float)(this.petalRandom.NextDouble() * 16.0);
				this.petalVelocitiesX[i] = (float)(this.petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
				this.petalTargetVelocitiesY[i] = this.petalVelocitiesY[i];
				this.petalTargetVelocitiesX[i] = this.petalVelocitiesX[i];
				this.petalTurnTimers[i] = this.petalRandom.Next(200);
				this.petalLifeCounters[i] = this.petalRandom.Next(1, 9);
			}
			for (int j = this.petalActiveCount; j < 30; j++)
			{
				this.petalFrames[j] = 0;
				this.petalPositions[j] = PointF.Empty;
				this.petalVelocitiesY[j] = 0f;
				this.petalVelocitiesX[j] = 0f;
				this.petalTargetVelocitiesY[j] = 0f;
				this.petalTargetVelocitiesX[j] = 0f;
				this.petalTurnTimers[j] = 0;
				this.petalLifeCounters[j] = 0;
			}
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0001E0A8 File Offset: 0x0001D0A8
		private void DrawPetals(bool upperHalf)
		{
			if (this.petalActive && this.effectEnabled)
			{
				int num = -1;
				for (int i = (upperHalf ? 0 : 1); i < 30; i += 2)
				{
					if (this.petalLifeCounters[i] == 0)
					{
						num = i;
						break;
					}
				}
				if (num >= 0)
				{
					if (this.petalActiveCount >= 20)
					{
						if (this.petalRandom.NextDouble() >= 0.2)
						{
							goto IL_03B4;
						}
					}
					this.petalFrames[num] = this.petalRandom.Next(4);
					int num2 = this.petalRandom.Next(-3297, 624);
					this.petalVelocitiesY[num] = (float)(this.petalRandom.NextDouble() * 16.0);
					this.petalVelocitiesX[num] = (float)(this.petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
					this.petalTargetVelocitiesY[num] = this.petalVelocitiesY[num];
					this.petalTargetVelocitiesX[num] = this.petalVelocitiesX[num];
					if (num2 < 0)
					{
						this.petalPositions[num] = new PointF((float)((-(float)num2 - 1) % 824 - 24), -24f);
					}
					else if (this.petalVelocitiesX[num] > 0f)
					{
						this.petalPositions[num] = new PointF(-24f, (float)(num2 - 24));
					}
					else
					{
						this.petalPositions[num] = new PointF(800f, (float)(num2 - 24));
					}
					this.petalTurnTimers[num] = this.petalRandom.Next(40, 200);
					this.petalLifeCounters[num] = this.petalRandom.Next(1, 9);
					this.petalActiveCount++;
				}
				IL_03B4:
				for (int j = (upperHalf ? 0 : 1); j < 30; j += 2)
				{
					if (this.petalLifeCounters[j] > 0)
					{
						this.DrawPetal(this.petalFrames[j], this.petalLifeCounters[j], this.petalPositions[j]);
						int[] array;
						IntPtr intPtr;
						(array = this.petalFrames)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
						if (this.petalFrames[j] == 4)
						{
							this.petalFrames[j] = 0;
						}
						(array = this.petalTurnTimers)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] - 1;
						if (this.petalTurnTimers[j] == 0)
						{
							this.petalTargetVelocitiesY[j] = (float)(this.petalRandom.NextDouble() * 16.0);
							this.petalTargetVelocitiesX[j] = (float)(this.petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
							this.petalTurnTimers[j] = this.petalRandom.Next(40, 200);
						}
						if (this.petalVelocitiesX[j] != this.petalTargetVelocitiesX[j] || this.petalVelocitiesY[j] != this.petalTargetVelocitiesY[j])
						{
							float num3 = this.petalTargetVelocitiesY[j] - this.petalVelocitiesY[j];
							float num4 = this.petalTargetVelocitiesX[j] - this.petalVelocitiesX[j];
							float num5;
							if (num3 != 0f)
							{
								num5 = Math.Abs(num4 / num3);
							}
							else
							{
								num5 = float.NaN;
							}
							if (Math.Abs(num4) > Math.Abs(num3))
							{
								if (Math.Abs(num4) > 0.1f)
								{
									float[] array2;
									(array2 = this.petalVelocitiesX)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num4 > 0f) ? 0.1f : (-0.1f));
									if (!float.IsNaN(num5))
									{
										(array2 = this.petalVelocitiesY)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num3 > 0f) ? 0.1f : (-0.1f)) / num5;
									}
								}
								else
								{
									this.petalVelocitiesX[j] = this.petalTargetVelocitiesX[j];
									this.petalVelocitiesY[j] = this.petalTargetVelocitiesY[j];
								}
							}
							else if (Math.Abs(num3) > 0.1f)
							{
								float[] array2;
								(array2 = this.petalVelocitiesY)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num3 > 0f) ? 0.1f : (-0.1f));
								(array2 = this.petalVelocitiesX)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num4 > 0f) ? 0.1f : (-0.1f)) * num5;
							}
							else
							{
								this.petalVelocitiesY[j] = this.petalTargetVelocitiesY[j];
								this.petalVelocitiesX[j] = this.petalTargetVelocitiesX[j];
							}
						}
						this.petalPositions[j] = new PointF(this.petalPositions[j].X + this.petalVelocitiesX[j], this.petalPositions[j].Y + this.petalVelocitiesY[j]);
						if (this.petalPositions[j].X < -24f || this.petalPositions[j].X > 800f || this.petalPositions[j].Y > 600f)
						{
							this.petalFrames[j] = 0;
							this.petalPositions[j] = PointF.Empty;
							this.petalVelocitiesY[j] = 0f;
							this.petalVelocitiesX[j] = 0f;
							this.petalTargetVelocitiesY[j] = 0f;
							this.petalTargetVelocitiesX[j] = 0f;
							this.petalTurnTimers[j] = 0;
							this.petalLifeCounters[j] = 0;
							this.petalActiveCount--;
						}
					}
				}
			}
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0001E6C0 File Offset: 0x0001D6C0
		private void DrawPetal(int frame, int lifeIndex, PointF origin)
		{
			Rectangle rectangle = new Rectangle(frame * 24, (lifeIndex - 1) * 24, 24, 24);
			this.QueueDraw(this.petalTexture, rectangle, Rectangle.Empty, new Point((int)origin.X, (int)origin.Y), Color.White);
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0001E710 File Offset: 0x0001D710
		private void InitSnow()
		{
			this.snowTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("SNOW"), this.snowFrameSize.Width, this.snowFrameSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.snowActive = false;
			for (int i = 0; i < 25; i++)
			{
				this.snowPositions[i] = Point.Empty;
				this.snowSpeeds[i] = 0f;
				this.snowSwayAmplitudes[i] = 0f;
				this.snowSwayFrequencies[i] = 0f;
				this.snowSwayPhases[i] = 0f;
				this.snowLifeCounters[i] = 0;
			}
			this.snowActiveCount = 0;
			this.snowRandom = new Random();
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0001E7EC File Offset: 0x0001D7EC
		private void DisposeSnow()
		{
			this.snowTexture.Dispose();
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0001E7FC File Offset: 0x0001D7FC
		public void SetSnowActive(bool instant)
		{
			if (!instant)
			{
				this.snowActive = true;
				this.backgroundIsColor = false;
				this.ResetSnow();
				return;
			}
			this.snowActive = false;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0001E820 File Offset: 0x0001D820
		private void ResetSnow()
		{
			this.snowActiveCount = this.snowRandom.Next(15, 25);
			for (int i = 0; i < this.snowActiveCount; i++)
			{
				this.snowPositions[i] = new PointF((float)this.snowRandom.Next(-16, 800), (float)this.snowRandom.Next(-16, 600));
				this.snowSpeeds[i] = (float)(this.snowRandom.NextDouble() * 4.0 + 2.0);
				this.snowSwayAmplitudes[i] = (float)(this.snowRandom.NextDouble() * 16.0 + 8.0);
				this.snowSwayFrequencies[i] = (float)(this.snowRandom.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
				this.snowSwayPhases[i] = (float)(this.snowRandom.NextDouble() * 6.283185307179586);
				this.snowLifeCounters[i] = this.snowRandom.Next(1, 5);
			}
			for (int j = this.snowActiveCount; j < 25; j++)
			{
				this.snowPositions[j] = PointF.Empty;
				this.snowSpeeds[j] = 0f;
				this.snowSwayAmplitudes[j] = 0f;
				this.snowSwayFrequencies[j] = 0f;
				this.snowSwayPhases[j] = 0f;
				this.snowLifeCounters[j] = 0;
			}
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0001E9C8 File Offset: 0x0001D9C8
		private void DrawSnow()
		{
			if (this.snowActive && this.effectEnabled)
			{
				int num = -1;
				for (int i = 0; i < 25; i++)
				{
					if (this.snowLifeCounters[i] == 0)
					{
						num = i;
						break;
					}
				}
				if (num >= 0 && (this.snowActiveCount < 15 || this.snowRandom.NextDouble() < 0.2))
				{
					this.snowPositions[num] = new PointF((float)this.snowRandom.Next(-16, 800), -16f);
					this.snowSpeeds[num] = (float)(this.snowRandom.NextDouble() * 4.0 + 2.0);
					this.snowSwayAmplitudes[num] = (float)(this.snowRandom.NextDouble() * 16.0 + 8.0);
					this.snowSwayFrequencies[num] = (float)(this.snowRandom.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
					this.snowSwayPhases[num] = (float)(this.snowRandom.NextDouble() * 6.283185307179586);
					this.snowLifeCounters[num] = this.snowRandom.Next(1, 5);
					this.snowActiveCount++;
				}
				for (int j = 0; j < 25; j++)
				{
					if (this.snowLifeCounters[j] > 0)
					{
						this.DrawSnowflake(this.snowLifeCounters[j], new PointF((float)((double)this.snowPositions[j].X + (double)this.snowSwayAmplitudes[j] * Math.Sin((double)(this.snowSwayFrequencies[j] * this.snowPositions[j].Y + this.snowSwayPhases[j]))), this.snowPositions[j].Y));
						this.snowPositions[j] = new PointF(this.snowPositions[j].X, this.snowPositions[j].Y + this.snowSpeeds[j]);
						if (this.snowPositions[j].Y > 600f)
						{
							this.snowPositions[j] = PointF.Empty;
							this.snowSpeeds[j] = 0f;
							this.snowSwayAmplitudes[j] = 0f;
							this.snowSwayFrequencies[j] = 0f;
							this.snowSwayPhases[j] = 0f;
							this.snowLifeCounters[j] = 0;
							this.snowActiveCount--;
						}
					}
				}
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0001EC90 File Offset: 0x0001DC90
		private void DrawSnowflake(int lifeIndex, PointF origin)
		{
			Rectangle rectangle = new Rectangle((lifeIndex - 1) * 16, 0, 16, 16);
			this.QueueDraw(this.snowTexture, rectangle, Rectangle.Empty, new Point((int)origin.X, (int)origin.Y), Color.White);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x0001ECDC File Offset: 0x0001DCDC
		private void InitDimOverlay()
		{
			this.dimActive = false;
			this.dimReverse = false;
			this.dimCounter = 0;
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x0001ECF4 File Offset: 0x0001DCF4
		public void SetDimActive(bool animate, bool instant)
		{
			if (!instant)
			{
				this.dimActive = true;
				this.dimReverse = false;
				this.backgroundIsColor = false;
				if (animate)
				{
					this.dimCounter = 10;
					return;
				}
				this.dimCounter = 0;
				return;
			}
			else
			{
				this.dimReverse = true;
				if (animate)
				{
					this.dimCounter = 10;
					return;
				}
				this.dimCounter = 0;
				this.dimActive = false;
				return;
			}
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x0001ED54 File Offset: 0x0001DD54
		private void DrawDimOverlay()
		{
			if (this.dimActive)
			{
				if (this.dimCounter > 0)
				{
					if (this.fadeEnabled)
					{
						float num;
						if (!this.dimReverse)
						{
							num = (float)(10 - this.dimCounter) / 10f;
						}
						else
						{
							num = (float)this.dimCounter / 10f;
						}
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(128f * num), Color.Black));
						this.dimCounter--;
					}
					else
					{
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
						this.dimCounter = 0;
					}
					if (this.dimReverse && this.dimCounter == 0)
					{
						this.dimActive = false;
						return;
					}
				}
				else
				{
					this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
				}
			}
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x0001EE6C File Offset: 0x0001DE6C
		private void InitThinkOverlay()
		{
			this.thinkTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("THINK"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.thinkActive = false;
			this.thinkReverse = false;
			this.thinkCounter = 0;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0001EEC4 File Offset: 0x0001DEC4
		private void DisposeThinkOverlay()
		{
			this.thinkTexture.Dispose();
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x0001EED4 File Offset: 0x0001DED4
		public void SetThinkActive(bool animate, bool instant)
		{
			if (!instant)
			{
				this.thinkActive = true;
				this.thinkReverse = false;
				this.backgroundIsColor = false;
				if (animate)
				{
					this.thinkCounter = 10;
				}
				else
				{
					this.thinkCounter = 0;
				}
				this.RebuildDesaturatedBackground();
				this.RebuildCharacterShadow();
				return;
			}
			this.thinkReverse = true;
			if (animate)
			{
				this.thinkCounter = 10;
				return;
			}
			this.thinkCounter = 0;
			this.thinkActive = false;
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0001EF44 File Offset: 0x0001DF44
		private void DrawThinkOverlay()
		{
			if (this.thinkActive)
			{
				if (this.thinkCounter > 0)
				{
					if (this.fadeEnabled)
					{
						float num;
						if (!this.thinkReverse)
						{
							num = (float)(10 - this.thinkCounter) / 10f;
						}
						else
						{
							num = (float)this.thinkCounter / 10f;
						}
						this.QueueDraw(this.thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * num), Color.White));
						this.thinkCounter--;
					}
					else
					{
						this.QueueDraw(this.thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						this.thinkCounter = 0;
					}
					if (this.thinkReverse && this.thinkCounter == 0)
					{
						this.thinkActive = false;
						return;
					}
				}
				else
				{
					this.QueueDraw(this.thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
				}
			}
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0001F050 File Offset: 0x0001E050
		private void DrawBackgroundEffects()
		{
			this.DrawZoomTransition();
			this.DrawShake();
			this.DrawSunEffect();
			this.DrawPetals(false);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x0001F06C File Offset: 0x0001E06C
		private void DrawForegroundEffects()
		{
			this.DrawPetals(true);
			this.DrawSnow();
			this.DrawDimOverlay();
			this.DrawThinkOverlay();
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0001F088 File Offset: 0x0001E088
		private void InitCharacters()
		{
			this.characterShakeTableX = new int[3][];
			this.characterShakeTableX[0] = new int[] { 4, -4, 4, -4 };
			this.characterShakeTableX[1] = new int[] { 8, 0, -8, 8, 0, -8 };
			this.characterShakeTableX[2] = new int[]
			{
				16, 0, -16, 0, 16, 0, -16, 0, 8, -8,
				8, -8, 4, -4, 4, -4
			};
			this.characterShakeTableY = new int[3][];
			this.characterShakeTableY[0] = new int[]
			{
				0,
				0,
				4,
				4
			};
			this.characterShakeTableY[1] = new int[] { 0, 0, 4, 4, 8, 8 };
			this.characterShakeTableY[2] = new int[]
			{
				0, 0, 8, 8, 16, 16, 8, 8, 0, 0,
				8, 8, 0, 0, 4, 4
			};
			int i = 0;
			while (i < 3)
			{
				this.characterTextures[i] = null;
				this.previousCharacterTextures[i] = null;
				this.characterShadowTextures[i] = null;
				this.characterAppearCounters[i] = 0;
				this.characterShakeCounters[i] = 0;
				this.characterOffsets[i] = 0;
				this.previousCharacterOffsets[i] = 0;
				this.characterSizes[i] = Size.Empty;
				this.previousCharacterSizes[i] = Size.Empty;
				int num = i++;
			}
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0001F1DC File Offset: 0x0001E1DC
		private void DisposeCharacters()
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.characterTextures[i] != null)
				{
					this.characterTextures[i].Dispose();
				}
				if (this.previousCharacterTextures[i] != null)
				{
					this.previousCharacterTextures[i].Dispose();
				}
				if (this.characterShadowTextures[i] != null)
				{
					this.characterShadowTextures[i].Dispose();
				}
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x0001F254 File Offset: 0x0001E254
		public void SetCharacter(Texture content, Size size, int characterIndex, int offsetIndex, bool semiTransparent, bool animate)
		{
			if (this.previousCharacterTextures[characterIndex] != null)
			{
				this.previousCharacterTextures[characterIndex].Dispose();
			}
			this.previousCharacterTextures[characterIndex] = this.characterTextures[characterIndex];
			this.previousCharacterSizes[characterIndex] = this.characterSizes[characterIndex];
			this.characterTextures[characterIndex] = content;
			this.characterSizes[characterIndex] = size;
			this.previousCharacterOffsets[characterIndex] = this.characterOffsets[characterIndex];
			if (offsetIndex != 2147483647)
			{
				this.characterOffsets[characterIndex] = offsetIndex;
			}
			if (content != null)
			{
				this.characterSemiTransparent[characterIndex] = semiTransparent;
			}
			if (animate)
			{
				this.characterAppearCounters[characterIndex] = 8;
			}
			if (this.thinkActive)
			{
				this.RebuildCharacterShadow();
				return;
			}
			int i = 0;
			while (i < 3)
			{
				if (this.characterShadowTextures[i] != null)
				{
					this.characterShadowTextures[i].Dispose();
					this.characterShadowTextures[i] = null;
				}
				int num = i++;
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0001F370 File Offset: 0x0001E370
		public void SetCharacterShake(int characterIndex, int shakePattern)
		{
			this.characterShakePatterns[characterIndex] = shakePattern;
			this.characterShakeCounters[characterIndex] = this.characterShakePatternLengths[shakePattern];
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0001F38C File Offset: 0x0001E38C
		private void DrawCharacters()
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.characterAppearCounters[i] > 0)
				{
					if (this.fadeEnabled)
					{
						if (this.previousCharacterTextures[i] != null && this.characterTextures[i] != null)
						{
							if (this.characterAppearCounters[i] > 2)
							{
								int num = (int)(Math.Pow((double)(8 - this.characterAppearCounters[i]) / 6.0, 2.0) * 255.0 / (double)(this.characterSemiTransparent[i] ? 2 : 1));
								this.QueueDraw(this.previousCharacterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.previousCharacterOffsets[i] * 15 - this.previousCharacterSizes[i].Width / 2, 600 - this.previousCharacterSizes[i].Height), this.characterSemiTransparent[i] ? Color.FromArgb(128, Color.White) : Color.White);
								this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.FromArgb(num, Color.White));
							}
							else
							{
								int num2 = (int)(Math.Pow((double)this.characterAppearCounters[i] / 2.0, 2.0) * 255.0 / (double)(this.characterSemiTransparent[i] ? 2 : 1));
								this.QueueDraw(this.previousCharacterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.previousCharacterSizes[i].Width / 2, 600 - this.previousCharacterSizes[i].Height), Color.FromArgb(num2, Color.White));
								this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), this.characterSemiTransparent[i] ? Color.FromArgb(128, Color.White) : Color.White);
							}
						}
						else if (this.previousCharacterTextures[i] != null)
						{
							this.QueueDraw(this.previousCharacterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.previousCharacterOffsets[i] * 15 - this.previousCharacterSizes[i].Width / 2, 600 - this.previousCharacterSizes[i].Height), Color.FromArgb((int)((float)this.characterAppearCounters[i] / 8f * 255f / (float)(this.characterSemiTransparent[i] ? 2 : 1)), Color.White));
						}
						else
						{
							if (!(this.characterTextures[i] != null))
							{
								throw new Exception("No Chara");
							}
							this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.FromArgb((int)((float)(8 - this.characterAppearCounters[i]) / 8f * 255f / (float)(this.characterSemiTransparent[i] ? 2 : 1)), Color.White));
						}
						int[] array;
						IntPtr intPtr;
						(array = this.characterAppearCounters)[(int)(intPtr = (IntPtr)i)] = array[(int)intPtr] - 1;
					}
					else
					{
						if (this.characterTextures[i] != null)
						{
							this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.White);
						}
						this.characterAppearCounters[i] = 0;
					}
				}
				else if (this.characterTextures[i] != null)
				{
					if (this.characterShakeCounters[i] > 0)
					{
						if (this.fadeEnabled && this.characterSemiTransparent[i])
						{
							this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2 + this.characterShakeTableX[this.characterShakePatterns[i]][this.characterShakePatternLengths[this.characterShakePatterns[i]] - this.characterShakeCounters[i]], 600 - this.characterSizes[i].Height + this.characterShakeTableY[this.characterShakePatterns[i]][this.characterShakePatternLengths[this.characterShakePatterns[i]] - this.characterShakeCounters[i]]), this.semiTransparentWhite);
						}
						else
						{
							this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2 + this.characterShakeTableX[this.characterShakePatterns[i]][this.characterShakePatternLengths[this.characterShakePatterns[i]] - this.characterShakeCounters[i]], 600 - this.characterSizes[i].Height + this.characterShakeTableY[this.characterShakePatterns[i]][this.characterShakePatternLengths[this.characterShakePatterns[i]] - this.characterShakeCounters[i]]), Color.White);
						}
						int[] array;
						IntPtr intPtr;
						(array = this.characterShakeCounters)[(int)(intPtr = (IntPtr)i)] = array[(int)intPtr] - 1;
					}
					else if (this.fadeEnabled && this.characterSemiTransparent[i])
					{
						this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), this.semiTransparentWhite);
					}
					else if (this.thinkActive)
					{
						if (this.thinkCounter > 0)
						{
							float num3;
							if (!this.thinkReverse)
							{
								num3 = (float)(10 - this.thinkCounter) / 10f;
							}
							else
							{
								num3 = (float)this.thinkCounter / 10f;
							}
							this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.White);
							this.QueueDraw(this.characterShadowTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.FromArgb((int)(255f * num3), Color.White));
						}
						else
						{
							this.QueueDraw(this.characterShadowTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.White);
						}
					}
					else
					{
						this.QueueDraw(this.characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + this.characterOffsets[i] * 15 - this.characterSizes[i].Width / 2, 600 - this.characterSizes[i].Height), Color.White);
					}
				}
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x0001FD48 File Offset: 0x0001ED48
		private void RebuildCharacterShadow()
		{
			for (int i = 0; i < 3; i++)
			{
				if (this.characterShadowTextures[i] != null)
				{
					this.characterShadowTextures[i].Dispose();
				}
				if (this.characterTextures[i] != null)
				{
					this.characterShadowTextures[i] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("WHITE"), this.characterSizes[i].Width, this.characterSizes[i].Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
					uint[,] array = (uint[,])this.characterTextures[i].LockRectangle(typeof(uint), 0, LockFlags.None, new int[]
					{
						this.characterSizes[i].Width,
						this.characterSizes[i].Height
					});
					uint[,] array2 = (uint[,])this.characterShadowTextures[i].LockRectangle(typeof(uint), 0, LockFlags.None, new int[]
					{
						this.characterSizes[i].Width,
						this.characterSizes[i].Height
					});
					for (int j = 0; j < this.characterSizes[i].Width; j++)
					{
						for (int k = 0; k < this.characterSizes[i].Height; k++)
						{
							uint num = array[j, k];
							uint num2 = ((num & 255U) + ((num >> 8) & 255U) + ((num >> 16) & 255U)) / 3U;
							uint num3 = (num >> 24) & 255U;
							array2[j, k] = (num3 << 24) + (num2 << 16) + (num2 << 8) + num2;
						}
					}
					this.characterTextures[i].UnlockRectangle(0);
					this.characterShadowTextures[i].UnlockRectangle(0);
				}
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x0001FF84 File Offset: 0x0001EF84
		private void InitNotice()
		{
			this.noticeLines = null;
			this.noticeCounter = 0;
			this.RebuildNoticeFont();
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x0001FF9C File Offset: 0x0001EF9C
		private void DisposeNoticeFont()
		{
			if (this.noticeFont != null)
			{
				this.noticeFont.Dispose();
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0001FFB8 File Offset: 0x0001EFB8
		private void RebuildNoticeFont()
		{
			if (this.noticeFont != null)
			{
				this.noticeFont.Dispose();
			}
			this.noticeFont = new Microsoft.DirectX.Direct3D.Font(this.device, new System.Drawing.Font(this.baseFont.FontFamily, 24f));
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x00020004 File Offset: 0x0001F004
		public void ShowNotice(string[] content)
		{
			this.noticeLines = content;
			this.noticeCounter = 70;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00020018 File Offset: 0x0001F018
		private void DrawNotice()
		{
			if (this.noticeLines == null)
			{
				return;
			}
			if (this.noticeCounter == 0)
			{
				this.noticeLines = null;
				return;
			}
			int num;
			if (this.fadeEnabled)
			{
				if (this.noticeCounter <= 10)
				{
					num = (int)((float)this.noticeCounter / 10f * 255f);
				}
				else if (this.noticeCounter <= 60)
				{
					num = 255;
				}
				else
				{
					num = (int)((float)(70 - this.noticeCounter) / 10f * 255f);
				}
			}
			else
			{
				num = 255;
			}
			this.DrawTextCore(this.noticeLines[(int)this.layoutIndex], 0, this.noticeLines[(int)this.layoutIndex].Length, this.noticePosition, this.noticeCharSize.Width, this.noticeCharSize.Height, 2, HorizontalAlign.Right, 15, this.noticeFont, Color.FromArgb(num, Color.White), Color.FromArgb(num, Color.Black));
			this.fontIndexQueue.Add(1);
			this.noticeCounter--;
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x00020140 File Offset: 0x0001F140
		private void InitMessage()
		{
			this.messageLines = null;
			this.messageFontSizeMode = HorizontalAlign.Left;
			this.messageAlignMode = TextRevealMode.Fade;
			this.messageProgress = 0.0;
			this.lineRevealCounter = 0;
			this.messageFadeCounter = 0;
			this.messageVisibleChars = 0;
			this.showNextIndicator = false;
			this.messageTextColor = Color.White;
			this.messageShadowColor = Color.Black;
			this.messageVoiceLength = 0.0;
			this.lastTextPosition = this.messagePosition;
			int i = 0;
			while (i < 3)
			{
				this.messageStartIndexes[i] = 0;
				int num = i++;
			}
			this.RebuildMessageFont();
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000201E0 File Offset: 0x0001F1E0
		private void DisposeMessageFont()
		{
			if (this.messageFont != null)
			{
				this.messageFont.Dispose();
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000201FC File Offset: 0x0001F1FC
		private void RebuildMessageFont()
		{
			if (this.messageFont != null)
			{
				this.messageFont.Dispose();
			}
			this.messageFont = new Microsoft.DirectX.Direct3D.Font(this.device, new System.Drawing.Font(this.baseFont.FontFamily, 24f));
			if (this.messageLines != null && this.messageStartIndexes[(int)this.layoutIndex] + this.messageVisibleChars > 0 && this.messageVisibleChars > 0)
			{
				bool flag = false;
				for (int i = 0; i < 3; i++)
				{
					if (this.messageStartIndexes[i] + this.messageVisibleChars >= this.messageLines[i].Length)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.messageVisibleChars = this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex];
					if (this.messageAlignMode == TextRevealMode.Typewriter)
					{
						this.messageProgress = (double)(this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex] + 4);
					}
				}
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x0002031C File Offset: 0x0001F31C
		public void SetMessage(string[] content, HorizontalAlign fontSizeMode, TextRevealMode alignMode, bool waitForInput, Color textColor, Color shadowColor, double voiceLength)
		{
			if (content == null)
			{
				return;
			}
			this.messageFontSizeMode = fontSizeMode;
			this.messageAlignMode = alignMode;
			this.showNextIndicator = waitForInput;
			this.messageTextColor = textColor;
			this.messageShadowColor = shadowColor;
			this.messageVoiceLength = voiceLength;
			if (this.messageLines == null)
			{
				this.messageLines = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.messageLines[i] = "";
				}
			}
			for (int j = 0; j < 3; j++)
			{
				this.messageStartIndexes[j] = this.messageLines[j].Length;
				string[] array;
				IntPtr intPtr;
				(array = this.messageLines)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + content[j];
			}
			this.messageVisibleChars = 0;
			switch (this.messageAlignMode)
			{
			case TextRevealMode.Typewriter:
				this.messageProgress = 0.0;
				return;
			case TextRevealMode.Fade:
				this.lineRevealCounter = 10;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0002040C File Offset: 0x0001F40C
		public void DismissMessage(bool animate)
		{
			if (animate)
			{
				this.messageFadeCounter = 10;
				return;
			}
			this.messageFadeCounter = 0;
			this.messageProgress = 0.0;
			this.lineRevealCounter = 0;
			this.messageLines = null;
			for (int i = 0; i < 3; i++)
			{
				this.messageStartIndexes[i] = 0;
			}
			this.messageVisibleChars = 0;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0002046C File Offset: 0x0001F46C
		private void DrawMessage()
		{
			if (this.messageLines == null)
			{
				return;
			}
			if (this.messageFadeCounter > 0)
			{
				if (this.fadeEnabled)
				{
					int num = (int)((float)this.messageFadeCounter / 10f * 255f);
					this.DrawTextCore(this.messageLines[(int)this.layoutIndex], 0, this.messageLines[(int)this.layoutIndex].Length, this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, Color.FromArgb(num, this.messageTextColor), (this.messageShadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(num, this.messageShadowColor));
					this.fontIndexQueue.Add(2);
					this.messageFadeCounter--;
				}
				else
				{
					this.messageFadeCounter = 0;
				}
				if (this.messageFadeCounter == 0)
				{
					this.messageProgress = 0.0;
					this.lineRevealCounter = 0;
					this.messageLines = null;
					int i = 0;
					while (i < 3)
					{
						this.messageStartIndexes[i] = 0;
						int num2 = i++;
					}
					this.messageVisibleChars = 0;
					return;
				}
			}
			else
			{
				switch (this.messageAlignMode)
				{
				case TextRevealMode.Typewriter:
					if (this.messageProgress < (double)(this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex] + 4))
					{
						if (this.messageStartIndexes[(int)this.layoutIndex] > 0)
						{
							this.lastTextPosition = this.DrawTextCore(this.messageLines[(int)this.layoutIndex], 0, this.messageStartIndexes[(int)this.layoutIndex], this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, this.messageTextColor, this.messageShadowColor);
							this.fontIndexQueue.Add(2);
						}
						if (this.messageVoiceLength > 0.0 && this.syncVoice)
						{
							try
							{
								double num3 = this.audioManager.GetVoicePosition() / this.messageVoiceLength;
								this.messageProgress = (double)(this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex] + 4) * num3;
								goto IL_03E9;
							}
							catch (NullReferenceException)
							{
								this.messageProgress = (double)(this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex] + 4);
								goto IL_03E9;
							}
							goto IL_056E;
						}
						goto IL_056E;
						IL_03E9:
						this.lastTextPosition = this.DrawTextCore(this.messageLines[(int)this.layoutIndex], this.messageStartIndexes[(int)this.layoutIndex], (double)this.messageStartIndexes[(int)this.layoutIndex] + this.messageProgress - 4.0, this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, true, 4, this.messageTextColor, this.messageShadowColor);
						this.fontIndexQueue.Add(2);
						this.messageVisibleChars = (int)(this.messageProgress - 4.0);
						if (this.messageVisibleChars < 0)
						{
							this.messageVisibleChars = 0;
							break;
						}
						break;
						IL_056E:
						this.messageProgress += (double)this.messageSpeed / 6.0;
						goto IL_03E9;
					}
					this.lastTextPosition = this.DrawTextCore(this.messageLines[(int)this.layoutIndex], (double)this.messageLines[(int)this.layoutIndex].Length, this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, 4, this.messageTextColor, this.messageShadowColor);
					this.fontIndexQueue.Add(2);
					break;
				case TextRevealMode.Fade:
					if (this.messageStartIndexes[(int)this.layoutIndex] + this.messageVisibleChars > 0)
					{
						this.lastTextPosition = this.DrawTextCore(this.messageLines[(int)this.layoutIndex], 0, this.messageStartIndexes[(int)this.layoutIndex] + this.messageVisibleChars, this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, this.messageTextColor, this.messageShadowColor);
						this.fontIndexQueue.Add(2);
					}
					if (this.lineRevealCounter > 0)
					{
						if (this.fadeEnabled)
						{
							int num4 = (int)((float)(10 - this.lineRevealCounter) / 10f * 255f);
							this.lastTextPosition = this.DrawTextCore(this.messageLines[(int)this.layoutIndex], this.messageStartIndexes[(int)this.layoutIndex], this.messageLines[(int)this.layoutIndex].Length, this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, Color.FromArgb(num4, this.messageTextColor), (this.messageShadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(num4, this.messageShadowColor));
							this.fontIndexQueue.Add(2);
							this.lineRevealCounter--;
						}
						else
						{
							this.lastTextPosition = this.DrawTextCore(this.messageLines[(int)this.layoutIndex], this.messageStartIndexes[(int)this.layoutIndex], this.messageLines[(int)this.layoutIndex].Length, this.messagePosition, this.messageCharSize.Width, this.messageCharSize.Height, 2, this.messageFontSizeMode, 25, this.messageFont, this.messageTextColor, this.messageShadowColor);
							this.fontIndexQueue.Add(2);
							this.lineRevealCounter = 0;
						}
						if (this.lineRevealCounter == 0)
						{
							this.messageVisibleChars = this.messageLines[(int)this.layoutIndex].Length - this.messageStartIndexes[(int)this.layoutIndex];
						}
					}
					break;
				}
				if (this.showNextIndicator && ((this.messageAlignMode == TextRevealMode.Typewriter && this.syncVoice) || !this.audioManager.IsVoiceBlocking()))
				{
					switch (this.messageAlignMode)
					{
					case TextRevealMode.Typewriter:
						if (this.messageProgress >= (double)this.messageLines[(int)this.layoutIndex].Length - (double)this.messageStartIndexes[(int)this.layoutIndex] + 4.0)
						{
							this.DrawNextIndicator(this.lastTextPosition.X, this.lastTextPosition.Y);
							return;
						}
						break;
					case TextRevealMode.Fade:
						if (this.lineRevealCounter == 0)
						{
							this.DrawNextIndicator(this.lastTextPosition.X, this.lastTextPosition.Y);
						}
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00020C38 File Offset: 0x0001FC38
		private void InitDialogueFrame()
		{
			this.dialogueTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("DIALOGUE"), this.dialogueRect.Width, this.dialogueRect.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.characterPanelTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("CHARA"), this.characterPanelSize.Width, this.characterPanelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.dialogueCounter = 0;
			this.dialogueClosing = false;
			this.dialogueActive = false;
			this.characterPanelVisible = false;
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00020CF4 File Offset: 0x0001FCF4
		private void DisposeDialogueFrame()
		{
			this.dialogueTexture.Dispose();
			this.characterPanelTexture.Dispose();
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x00020D0C File Offset: 0x0001FD0C
		protected bool DrawDialogueFrame(bool optionLines)
		{
			if (optionLines)
			{
				this.characterPanelVisible = true;
				this.QueueDraw(this.characterPanelTexture, Rectangle.Empty, Rectangle.Empty, this.characterPanelPosition, this.dialogueColor);
				return true;
			}
			if (!this.dialogueActive)
			{
				this.dialogueActive = true;
				this.dialogueCounter = (this.skipCaptionAnimation ? 0 : 3);
				this.dialogueClosing = false;
			}
			if (this.skipCaptionAnimation)
			{
				this.skipCaptionAnimation = false;
			}
			if (this.dialogueCounter > 0)
			{
				if (this.fadeEnabled)
				{
					if (!this.dialogueClosing && this.dialogueCounter < 3)
					{
						float num = (float)(3 - this.dialogueCounter) / 3f;
						float num2 = ((float)this.dialogueRect.X + (1f - num) / 2f * (float)this.dialogueRect.Width) / num;
						float num3 = ((float)this.dialogueRect.Y + (1f - num) / 2f * (float)this.dialogueRect.Height) / num;
						float num4 = num * (float)this.dialogueRect.Width;
						float num5 = num * (float)this.dialogueRect.Height;
						this.QueueDraw(this.dialogueTexture, new Rectangle(0, 0, this.dialogueRect.Width, this.dialogueRect.Height), new Rectangle(0, 0, (int)num4, (int)num5), new Point((int)num2, (int)num3), this.dialogueColor);
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					else if (this.dialogueClosing && this.dialogueCounter > 0)
					{
						float num6 = (float)this.dialogueCounter / 3f;
						float num7 = ((float)this.dialogueRect.X + (1f - num6) / 2f * (float)this.dialogueRect.Width) / num6;
						float num8 = ((float)this.dialogueRect.Y + (1f - num6) / 2f * (float)this.dialogueRect.Height) / num6;
						float num9 = num6 * (float)this.dialogueRect.Width;
						float num10 = num6 * (float)this.dialogueRect.Height;
						this.QueueDraw(this.dialogueTexture, new Rectangle(0, 0, this.dialogueRect.Width, this.dialogueRect.Height), new Rectangle(0, 0, (int)num9, (int)num10), new Point((int)num7, (int)num8), this.dialogueColor);
						this.QueueDraw(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					this.dialogueCounter--;
				}
				else
				{
					this.QueueDraw(this.dialogueTexture, Rectangle.Empty, Rectangle.Empty, this.dialogueRect.Location, this.dialogueColor);
					this.dialogueCounter = 0;
				}
				if (this.dialogueClosing && this.dialogueCounter == 0)
				{
					this.dialogueActive = false;
				}
				return false;
			}
			this.QueueDraw(this.dialogueTexture, Rectangle.Empty, Rectangle.Empty, this.dialogueRect.Location, this.dialogueColor);
			return true;
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000210C8 File Offset: 0x000200C8
		private void InitCaption()
		{
			this.captionLines = null;
			this.captionOptionLines = null;
			this.captionProgress = 0.0;
			this.captionVisibleChars = 0;
			this.captionVoiceLength = 0.0;
			this.captionLocked = false;
			this.captionLastPosition = this.captionPosition;
			for (int i = 0; i < 3; i++)
			{
				this.captionStartIndexes[i] = 0;
			}
			this.RebuildCaptionFont();
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0002113C File Offset: 0x0002013C
		private void DisposeCaptionFont()
		{
			if (this.captionFont != null)
			{
				this.captionFont.Dispose();
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00021158 File Offset: 0x00020158
		private void RebuildCaptionFont()
		{
			if (this.captionFont != null)
			{
				this.captionFont.Dispose();
			}
			this.captionFont = new Microsoft.DirectX.Direct3D.Font(this.device, new System.Drawing.Font(this.baseFont.FontFamily, 24f));
			if (this.captionLines != null && this.captionStartIndexes[(int)this.layoutIndex] + this.captionVisibleChars > 0 && this.captionVisibleChars > 0)
			{
				bool flag = false;
				for (int i = 0; i < 3; i++)
				{
					if (this.captionStartIndexes[i] + this.captionVisibleChars >= this.captionLines[i].Length)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.captionVisibleChars = this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex];
					this.captionProgress = (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4);
				}
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x00021274 File Offset: 0x00020274
		public void SetCaption(string[] content, string[] optionLines, double voiceLength, bool captionLockedFlag, bool FastForward)
		{
			if (content == null)
			{
				return;
			}
			this.captionVoiceLength = voiceLength;
			this.captionOptionLines = optionLines;
			this.captionLocked = captionLockedFlag;
			this.skipCaptionAnimation = FastForward;
			if (this.captionLines == null)
			{
				this.captionLines = new string[3];
				for (int i = 0; i < 3; i++)
				{
					this.captionLines[i] = "";
				}
			}
			for (int j = 0; j < 3; j++)
			{
				this.captionStartIndexes[j] = this.captionLines[j].Length;
				string[] array;
				IntPtr intPtr;
				(array = this.captionLines)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + content[j];
			}
			this.captionVisibleChars = 0;
			this.captionProgress = 0.0;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0002132C File Offset: 0x0002032C
		public void CloseDialogue(bool shadowColor, bool hideCharacterPanel, bool FastForward)
		{
			if (this.captionLocked)
			{
				return;
			}
			this.captionProgress = 0.0;
			this.captionLines = null;
			this.captionOptionLines = null;
			this.skipCaptionAnimation = FastForward && shadowColor;
			for (int i = 0; i < 3; i++)
			{
				this.captionStartIndexes[i] = 0;
			}
			this.captionVisibleChars = 0;
			if (shadowColor)
			{
				this.dialogueCounter = (this.skipCaptionAnimation ? 0 : 3);
				if (this.skipCaptionAnimation)
				{
					this.dialogueActive = false;
				}
				this.dialogueClosing = true;
			}
			if (hideCharacterPanel)
			{
				this.characterPanelVisible = false;
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000213D0 File Offset: 0x000203D0
		private void DrawCaption()
		{
			if (this.captionLines == null)
			{
				if (this.dialogueActive)
				{
					this.DrawDialogueFrame(false);
				}
				if (this.characterPanelVisible)
				{
					this.DrawDialogueFrame(true);
				}
				return;
			}
			if (this.DrawDialogueFrame(false))
			{
				if (this.captionProgress < (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4))
				{
					if (this.captionVoiceLength > 0.0 && this.syncVoice)
					{
						try
						{
							double num = this.audioManager.GetVoicePosition() / this.captionVoiceLength;
							this.captionProgress = (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4) * num;
							goto IL_019E;
						}
						catch (NullReferenceException)
						{
							this.captionProgress = (double)(this.captionLines[(int)this.layoutIndex].Length - this.captionStartIndexes[(int)this.layoutIndex] + 4);
							goto IL_019E;
						}
						goto IL_034F;
					}
					goto IL_034F;
					IL_019E:
					this.captionLastPosition = this.DrawTextCore(this.captionLines[(int)this.layoutIndex], (double)this.captionStartIndexes[(int)this.layoutIndex] + this.captionProgress - 4.0, this.captionPosition, this.captionCharSize.Width, this.captionCharSize.Height, 2, this.captionFont, 4, Color.White, Color.Black);
					this.fontIndexQueue.Add(3);
					this.captionVisibleChars = (int)(this.captionProgress - 4.0);
					if (this.captionVisibleChars < 0)
					{
						this.captionVisibleChars = 0;
						goto IL_00D6;
					}
					goto IL_00D6;
					IL_034F:
					this.captionProgress += (double)this.messageSpeed / 6.0;
					goto IL_019E;
				}
				this.captionLastPosition = this.DrawTextCore(this.captionLines[(int)this.layoutIndex], (double)this.captionLines[(int)this.layoutIndex].Length, this.captionPosition, this.captionCharSize.Width, this.captionCharSize.Height, 2, this.captionFont, 4, Color.White, Color.Black);
				this.fontIndexQueue.Add(3);
				IL_00D6:
				if ((this.syncVoice || !this.audioManager.IsVoiceBlocking()) && this.captionProgress >= (double)this.captionLines[(int)this.layoutIndex].Length - (double)this.captionStartIndexes[(int)this.layoutIndex] + 4.0)
				{
					this.DrawNextIndicator(this.captionLastPosition.X, this.captionLastPosition.Y);
				}
				if (this.captionOptionLines != null && this.DrawDialogueFrame(true))
				{
					this.DrawTextCore(this.captionOptionLines[(int)this.layoutIndex], new Point(this.captionOptionPosition.X - this.captionCharSize.Width * this.captionOptionLines[(int)this.layoutIndex].Length / 2, this.captionOptionPosition.Y), this.captionCharSize.Width, this.captionCharSize.Height, 2, this.captionFont, Color.White, Color.Black);
					this.fontIndexQueue.Add(3);
				}
			}
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x00021790 File Offset: 0x00020790
		public void SetModeIcon(int content)
		{
			this.modeIconIndex = content;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0002179C File Offset: 0x0002079C
		private void InitModeIcon()
		{
			this.modeIconTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("ICON"), this.modeIconTextureSize.Width, this.modeIconTextureSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.modeIconFrame = 0;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000217F8 File Offset: 0x000207F8
		private void DisposeModeIcon()
		{
			this.modeIconTexture.Dispose();
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x00021808 File Offset: 0x00020808
		private void DrawModeIcon()
		{
			if (this.modeIconIndex > 0)
			{
				Rectangle rectangle = new Rectangle(this.modeIconFrame / 3 * 32, this.modeIconIndex * 32, 32, 32);
				this.QueueDraw(this.modeIconTexture, rectangle, Rectangle.Empty, this.modeIconPosition, Color.White);
				this.modeIconFrame++;
				if (this.modeIconFrame == 9)
				{
					this.modeIconFrame = 0;
				}
			}
		}

		// Token: 0x04000183 RID: 387
		protected const int shakeFrameCount = 16;

		// Token: 0x04000184 RID: 388
		protected const int shakeStep = 4;

		// Token: 0x04000185 RID: 389
		protected const int indicatorCellWidth = 32;

		// Token: 0x04000186 RID: 390
		protected const int indicatorCellHeight = 32;

		// Token: 0x04000187 RID: 391
		private const int fontCount = 3;

		// Token: 0x04000188 RID: 392
		protected const int shadowOffsetPixels = 2;

		// Token: 0x04000189 RID: 393
		protected const int characterFadeFrames = 8;

		// Token: 0x0400018A RID: 394
		protected const int indicatorPhaseCount = 16;

		// Token: 0x0400018B RID: 395
		protected const int backgroundFadeFrames = 16;

		// Token: 0x0400018C RID: 396
		protected const int halfAlpha = 128;

		// Token: 0x0400018D RID: 397
		protected const int sunSpriteCount = 5;

		// Token: 0x0400018E RID: 398
		protected const int sunSpawnDelayMin = 20;

		// Token: 0x0400018F RID: 399
		protected const int sunSpriteSize = 80;

		// Token: 0x04000190 RID: 400
		protected const double sunScaleMin = 0.5;

		// Token: 0x04000191 RID: 401
		protected const double sunSpawnChance = 0.25;

		// Token: 0x04000192 RID: 402
		protected const int sunScaleMax = 450;

		// Token: 0x04000193 RID: 403
		protected const int sunOffsetRange = 75;

		// Token: 0x04000194 RID: 404
		protected const int sunCenterOffset = 18;

		// Token: 0x04000195 RID: 405
		protected const int petalTextureSize = 80;

		// Token: 0x04000196 RID: 406
		protected const int petalMaxCount = 30;

		// Token: 0x04000197 RID: 407
		protected const int petalMinCount = 20;

		// Token: 0x04000198 RID: 408
		protected const float petalSpeedRange = 16f;

		// Token: 0x04000199 RID: 409
		protected const float petalDriftRange = 8f;

		// Token: 0x0400019A RID: 410
		protected const float petalDriftStep = 0.1f;

		// Token: 0x0400019B RID: 411
		protected const int petalLifeMin = 40;

		// Token: 0x0400019C RID: 412
		protected const int petalLifeMax = 200;

		// Token: 0x0400019D RID: 413
		protected const double petalSpawnChance = 0.2;

		// Token: 0x0400019E RID: 414
		protected const int petalFrameSizePixels = 24;

		// Token: 0x0400019F RID: 415
		protected const int petalFrameCount = 4;

		// Token: 0x040001A0 RID: 416
		protected const int petalTypeCount = 4;

		// Token: 0x040001A1 RID: 417
		protected const int petalStepCount = 8;

		// Token: 0x040001A2 RID: 418
		protected const int snowflakeMaxCount = 25;

		// Token: 0x040001A3 RID: 419
		protected const int snowflakeMinCount = 15;

		// Token: 0x040001A4 RID: 420
		protected const float snowSpeedRange = 6f;

		// Token: 0x040001A5 RID: 421
		protected const float snowSpeedBase = 2f;

		// Token: 0x040001A6 RID: 422
		protected const float snowSwayRange = 24f;

		// Token: 0x040001A7 RID: 423
		protected const float snowSwayBase = 8f;

		// Token: 0x040001A8 RID: 424
		protected const float snowSwayFrequencyRange = 0.02f;

		// Token: 0x040001A9 RID: 425
		protected const float snowSwayFrequencyBase = 0.01f;

		// Token: 0x040001AA RID: 426
		protected const double twoPi = 6.283185307179586;

		// Token: 0x040001AB RID: 427
		protected const double snowSpawnChance = 0.2;

		// Token: 0x040001AC RID: 428
		protected const int snowFrameSizePixels = 16;

		// Token: 0x040001AD RID: 429
		protected const int snowFrameCount = 4;

		// Token: 0x040001AE RID: 430
		protected const int dimFadeFrames = 10;

		// Token: 0x040001AF RID: 431
		protected const int dimAlpha = 128;

		// Token: 0x040001B0 RID: 432
		protected const int thinkFadeFrames = 10;

		// Token: 0x040001B1 RID: 433
		protected const int characterAppearFrames = 8;

		// Token: 0x040001B2 RID: 434
		protected const int characterAppearDivisor = 6;

		// Token: 0x040001B3 RID: 435
		protected const int dialogueExpandFrames = 3;

		// Token: 0x040001B4 RID: 436
		protected const int dialogueTextWidth = 15;

		// Token: 0x040001B5 RID: 437
		protected const int dialogueShrinkFrames = 3;

		// Token: 0x040001B6 RID: 438
		protected const int messageFadeFrames = 10;

		// Token: 0x040001B7 RID: 439
		protected const int messageHoldFrames = 50;

		// Token: 0x040001B8 RID: 440
		protected const int noticeLineWidth = 15;

		// Token: 0x040001B9 RID: 441
		protected const int textShadowDistance = 2;

		// Token: 0x040001BA RID: 442
		protected const int fontSizePixels = 24;

		// Token: 0x040001BB RID: 443
		protected const int messageSpeedDivisor = 6;

		// Token: 0x040001BC RID: 444
		protected const int textFadeLength = 4;

		// Token: 0x040001BD RID: 445
		protected const int captionFadeFrames = 10;

		// Token: 0x040001BE RID: 446
		protected const int captionLineWidth = 10;

		// Token: 0x040001BF RID: 447
		protected const int captionShadowDistance = 2;

		// Token: 0x040001C0 RID: 448
		protected const int captionFontHeight = 24;

		// Token: 0x040001C1 RID: 449
		protected const int messageMaxLineWidth = 25;

		// Token: 0x040001C2 RID: 450
		protected const int messageOptionIndent = 13;

		// Token: 0x040001C3 RID: 451
		protected const int messageWrapFrames = 3;

		// Token: 0x040001C4 RID: 452
		protected const int messageRevealStep = 6;

		// Token: 0x040001C5 RID: 453
		protected const int messageRevealChunk = 4;

		// Token: 0x040001C6 RID: 454
		protected const int messageFadeStep = 2;

		// Token: 0x040001C7 RID: 455
		protected const int dialogueFramePadding = 24;

		// Token: 0x040001C8 RID: 456
		protected const int dialogueLineWidth = 25;

		// Token: 0x040001C9 RID: 457
		protected const int dialogueFadeStep = 3;

		// Token: 0x040001CA RID: 458
		protected const int optionLineCount = 5;

		// Token: 0x040001CB RID: 459
		protected const int modeIconFrameCount = 9;

		// Token: 0x040001CC RID: 460
		protected const int modeIconColumns = 3;

		// Token: 0x040001CD RID: 461
		protected const int modeIconCellSize = 32;

		// Token: 0x040001CE RID: 462
		protected ArchiveFile systemArchive;

		// Token: 0x040001CF RID: 463
		private ButtonLayout layoutIndex;

		// Token: 0x040001D0 RID: 464
		private System.Drawing.Font baseFont;

		// Token: 0x040001D1 RID: 465
		private bool dialogueHidden;

		// Token: 0x040001D2 RID: 466
		private AudioManager audioManager;

		// Token: 0x040001D3 RID: 467
		private Texture modeIconTexture;

		// Token: 0x040001D4 RID: 468
		private int modeIconIndex;

		// Token: 0x040001D5 RID: 469
		private Device device;

		// Token: 0x040001D6 RID: 470
		private Sprite sprite;

		// Token: 0x040001D7 RID: 471
		private FrameCallback resetCallback;

		// Token: 0x040001D8 RID: 472
		protected readonly Rectangle screenRect = new Rectangle(0, 0, 800, 600);

		// Token: 0x040001D9 RID: 473
		protected Texture whiteTexture;

		// Token: 0x040001DA RID: 474
		protected ArrayList textureQueue = new ArrayList();

		// Token: 0x040001DB RID: 475
		protected ArrayList destRectQueue = new ArrayList();

		// Token: 0x040001DC RID: 476
		protected ArrayList srcRectQueue = new ArrayList();

		// Token: 0x040001DD RID: 477
		protected ArrayList drawPositionQueue = new ArrayList();

		// Token: 0x040001DE RID: 478
		protected ArrayList colorQueue = new ArrayList();

		// Token: 0x040001DF RID: 479
		protected ArrayList textQueue = new ArrayList();

		// Token: 0x040001E0 RID: 480
		protected ArrayList startIndexQueue = new ArrayList();

		// Token: 0x040001E1 RID: 481
		protected ArrayList endIndexQueue = new ArrayList();

		// Token: 0x040001E2 RID: 482
		protected ArrayList textPositionQueue = new ArrayList();

		// Token: 0x040001E3 RID: 483
		protected ArrayList charWidthQueue = new ArrayList();

		// Token: 0x040001E4 RID: 484
		protected ArrayList lineHeightQueue = new ArrayList();

		// Token: 0x040001E5 RID: 485
		protected ArrayList shadowOffsetQueue = new ArrayList();

		// Token: 0x040001E6 RID: 486
		protected ArrayList fontSizeModeQueue = new ArrayList();

		// Token: 0x040001E7 RID: 487
		protected ArrayList lineWidthQueue = new ArrayList();

		// Token: 0x040001E8 RID: 488
		protected ArrayList fontIndexQueue = new ArrayList();

		// Token: 0x040001E9 RID: 489
		protected ArrayList fadeOutQueue = new ArrayList();

		// Token: 0x040001EA RID: 490
		protected ArrayList fadeLengthQueue = new ArrayList();

		// Token: 0x040001EB RID: 491
		protected ArrayList textColorQueue = new ArrayList();

		// Token: 0x040001EC RID: 492
		protected ArrayList shadowColorQueue = new ArrayList();

		// Token: 0x040001ED RID: 493
		private bool fadeEnabled;

		// Token: 0x040001EE RID: 494
		private bool sunEffectEnabled;

		// Token: 0x040001EF RID: 495
		private bool effectEnabled;

		// Token: 0x040001F0 RID: 496
		private bool shockEffectEnabled;

		// Token: 0x040001F1 RID: 497
		private Color dialogueColor;

		// Token: 0x040001F2 RID: 498
		private int messageSpeed = 1;

		// Token: 0x040001F3 RID: 499
		private bool syncVoice;

		// Token: 0x040001F4 RID: 500
		private int indicatorPhase;

		// Token: 0x040001F5 RID: 501
		protected readonly int[] indicatorColumns = new int[]
		{
			3, 3, 2, 2, 1, 1, 0, 0, 0, 0,
			1, 1, 2, 2, 3, 3
		};

		// Token: 0x040001F6 RID: 502
		protected readonly int[] indicatorOffsetX = new int[]
		{
			-2, -1, -1, 0, 0, 1, 1, 2, 2, 1,
			1, 0, 0, -1, -1, -2, -2, -1, -1, 0,
			0, 1, 1, 2, 2, 1, 1, 0, 0, -1,
			-1, -2
		};

		// Token: 0x040001F7 RID: 503
		protected readonly int[] indicatorOffsetY = new int[]
		{
			-1, -1, 0, 0, 0, 0, 1, 1, 1, 1,
			2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
			0, 0, -1, -1, -1, -1, -2, -2, -2, -2,
			-1, -1
		};

		// Token: 0x040001F8 RID: 504
		private Texture backgroundTexture;

		// Token: 0x040001F9 RID: 505
		private Texture desaturatedBackgroundTexture;

		// Token: 0x040001FA RID: 506
		private Color backgroundColor;

		// Token: 0x040001FB RID: 507
		private Texture previousBackgroundTexture;

		// Token: 0x040001FC RID: 508
		private Color previousBackgroundColor;

		// Token: 0x040001FD RID: 509
		private bool backgroundWipeActive;

		// Token: 0x040001FE RID: 510
		private bool backgroundIsColor;

		// Token: 0x040001FF RID: 511
		private bool previousBackgroundIsColor;

		// Token: 0x04000200 RID: 512
		private bool backgroundDrawnAsColor;

		// Token: 0x04000201 RID: 513
		private int backgroundFadeCounter;

		// Token: 0x04000202 RID: 514
		private int backgroundFadeSpeed;

		// Token: 0x04000203 RID: 515
		protected readonly int[] backgroundFadeDurations = new int[] { 1, 10, 40 };

		// Token: 0x04000204 RID: 516
		private bool backgroundRedrawPending;

		// Token: 0x04000205 RID: 517
		private TextureBlend backgroundWipeBlend;

		// Token: 0x04000206 RID: 518
		private bool zoomActive;

		// Token: 0x04000207 RID: 519
		private Rectangle zoomTargetRect;

		// Token: 0x04000208 RID: 520
		private Rectangle zoomSourceRect;

		// Token: 0x04000209 RID: 521
		private int zoomCounter;

		// Token: 0x0400020A RID: 522
		private int zoomSpeedIndex;

		// Token: 0x0400020B RID: 523
		protected readonly int[] zoomDurations = new int[] { 1, 10, 40 };

		// Token: 0x0400020C RID: 524
		private bool shakeActive;

		// Token: 0x0400020D RID: 525
		private bool shakeLoop;

		// Token: 0x0400020E RID: 526
		private int shakeFrame;

		// Token: 0x0400020F RID: 527
		protected readonly int[] shakeOffsetTableX = new int[]
		{
			-16, -8, 0, 8, 16, 8, 0, -8, -8, 0,
			8, 0, -4, 0, 4, 0
		};

		// Token: 0x04000210 RID: 528
		protected readonly int[] shakeOffsetTableY = new int[]
		{
			-16, 16, 0, 0, -16, 16, 0, 0, -8, 8,
			0, 0, -4, 4, 0, 0
		};

		// Token: 0x04000211 RID: 529
		protected readonly int[] quakeOffsetTableX = new int[]
		{
			-4, -3, -2, -1, 0, 1, 2, 3, 4, 3,
			2, 1, 0, -1, -2, -3
		};

		// Token: 0x04000212 RID: 530
		protected readonly int[] quakeOffsetTableY = new int[]
		{
			-4, -2, 0, 2, 4, 2, 0, -2, -4, -2,
			0, 2, 4, 2, 0, -2
		};

		// Token: 0x04000213 RID: 531
		private bool sunActive;

		// Token: 0x04000214 RID: 532
		private Texture sunTexture;

		// Token: 0x04000215 RID: 533
		private int[] sunLifeCounters = new int[5];

		// Token: 0x04000216 RID: 534
		private int[] sunLifeTotals = new int[5];

		// Token: 0x04000217 RID: 535
		private double[] sunScales = new double[5];

		// Token: 0x04000218 RID: 536
		private int[] sunOffsets = new int[5];

		// Token: 0x04000219 RID: 537
		private Random sunRandom;

		// Token: 0x0400021A RID: 538
		private bool petalActive;

		// Token: 0x0400021B RID: 539
		private Texture petalTexture;

		// Token: 0x0400021C RID: 540
		private int[] petalFrames = new int[30];

		// Token: 0x0400021D RID: 541
		private PointF[] petalPositions = new PointF[30];

		// Token: 0x0400021E RID: 542
		private float[] petalVelocitiesY = new float[30];

		// Token: 0x0400021F RID: 543
		private float[] petalVelocitiesX = new float[30];

		// Token: 0x04000220 RID: 544
		private float[] petalTargetVelocitiesY = new float[30];

		// Token: 0x04000221 RID: 545
		private float[] petalTargetVelocitiesX = new float[30];

		// Token: 0x04000222 RID: 546
		private int[] petalTurnTimers = new int[30];

		// Token: 0x04000223 RID: 547
		private int[] petalLifeCounters = new int[30];

		// Token: 0x04000224 RID: 548
		private Random petalRandom;

		// Token: 0x04000225 RID: 549
		private int petalActiveCount;

		// Token: 0x04000226 RID: 550
		protected readonly Size petalFrameSize = new Size(96, 192);

		// Token: 0x04000227 RID: 551
		private bool snowActive;

		// Token: 0x04000228 RID: 552
		private Texture snowTexture;

		// Token: 0x04000229 RID: 553
		private PointF[] snowPositions = new PointF[25];

		// Token: 0x0400022A RID: 554
		private float[] snowSpeeds = new float[25];

		// Token: 0x0400022B RID: 555
		private float[] snowSwayAmplitudes = new float[25];

		// Token: 0x0400022C RID: 556
		private float[] snowSwayFrequencies = new float[25];

		// Token: 0x0400022D RID: 557
		private float[] snowSwayPhases = new float[25];

		// Token: 0x0400022E RID: 558
		private int[] snowLifeCounters = new int[25];

		// Token: 0x0400022F RID: 559
		private Random snowRandom;

		// Token: 0x04000230 RID: 560
		private int snowActiveCount;

		// Token: 0x04000231 RID: 561
		protected readonly Size snowFrameSize = new Size(64, 16);

		// Token: 0x04000232 RID: 562
		private bool dimActive;

		// Token: 0x04000233 RID: 563
		private bool dimReverse;

		// Token: 0x04000234 RID: 564
		private int dimCounter;

		// Token: 0x04000235 RID: 565
		private bool thinkActive;

		// Token: 0x04000236 RID: 566
		private bool thinkReverse;

		// Token: 0x04000237 RID: 567
		private int thinkCounter;

		// Token: 0x04000238 RID: 568
		private Texture thinkTexture;

		// Token: 0x04000239 RID: 569
		private Texture[] characterTextures = new Texture[3];

		// Token: 0x0400023A RID: 570
		private Texture[] previousCharacterTextures = new Texture[3];

		// Token: 0x0400023B RID: 571
		private Texture[] characterShadowTextures = new Texture[3];

		// Token: 0x0400023C RID: 572
		private Size[] characterSizes = new Size[3];

		// Token: 0x0400023D RID: 573
		private Size[] previousCharacterSizes = new Size[3];

		// Token: 0x0400023E RID: 574
		private int[] characterAppearCounters = new int[3];

		// Token: 0x0400023F RID: 575
		private int[] characterOffsets = new int[3];

		// Token: 0x04000240 RID: 576
		private int[] previousCharacterOffsets = new int[3];

		// Token: 0x04000241 RID: 577
		private bool[] characterSemiTransparent = new bool[3];

		// Token: 0x04000242 RID: 578
		private int[] characterShakeCounters = new int[3];

		// Token: 0x04000243 RID: 579
		private int[] characterShakePatterns = new int[3];

		// Token: 0x04000244 RID: 580
		protected readonly int[] characterShakePatternLengths = new int[] { 4, 6, 16 };

		// Token: 0x04000245 RID: 581
		protected readonly Color semiTransparentWhite = Color.FromArgb(128, Color.White);

		// Token: 0x04000246 RID: 582
		protected int[][] characterShakeTableX;

		// Token: 0x04000247 RID: 583
		protected int[][] characterShakeTableY;

		// Token: 0x04000248 RID: 584
		private string[] noticeLines;

		// Token: 0x04000249 RID: 585
		private int noticeCounter;

		// Token: 0x0400024A RID: 586
		protected readonly Point noticePosition = new Point(300, 20);

		// Token: 0x0400024B RID: 587
		protected readonly Size noticeCharSize = new Size(30, 40);

		// Token: 0x0400024C RID: 588
		private Microsoft.DirectX.Direct3D.Font noticeFont;

		// Token: 0x0400024D RID: 589
		private string[] messageLines;

		// Token: 0x0400024E RID: 590
		private int[] messageStartIndexes = new int[3];

		// Token: 0x0400024F RID: 591
		private int messageVisibleChars;

		// Token: 0x04000250 RID: 592
		private double messageProgress;

		// Token: 0x04000251 RID: 593
		private int lineRevealCounter;

		// Token: 0x04000252 RID: 594
		private int messageFadeCounter;

		// Token: 0x04000253 RID: 595
		private HorizontalAlign messageFontSizeMode;

		// Token: 0x04000254 RID: 596
		private TextRevealMode messageAlignMode;

		// Token: 0x04000255 RID: 597
		private bool showNextIndicator;

		// Token: 0x04000256 RID: 598
		private Color messageTextColor;

		// Token: 0x04000257 RID: 599
		private Color messageShadowColor;

		// Token: 0x04000258 RID: 600
		private double messageVoiceLength;

		// Token: 0x04000259 RID: 601
		private Point lastTextPosition;

		// Token: 0x0400025A RID: 602
		protected readonly Point messagePosition = new Point(20, 60);

		// Token: 0x0400025B RID: 603
		protected readonly Size messageCharSize = new Size(30, 40);

		// Token: 0x0400025C RID: 604
		private Microsoft.DirectX.Direct3D.Font messageFont;

		// Token: 0x0400025D RID: 605
		private bool dialogueActive;

		// Token: 0x0400025E RID: 606
		private bool characterPanelVisible;

		// Token: 0x0400025F RID: 607
		protected Texture dialogueTexture;

		// Token: 0x04000260 RID: 608
		protected Texture characterPanelTexture;

		// Token: 0x04000261 RID: 609
		private bool dialogueClosing;

		// Token: 0x04000262 RID: 610
		private int dialogueCounter;

		// Token: 0x04000263 RID: 611
		protected readonly Rectangle dialogueRect = new Rectangle(10, 440, 780, 150);

		// Token: 0x04000264 RID: 612
		protected readonly Point dialogueTextAnchor = new Point(400, 515);

		// Token: 0x04000265 RID: 613
		protected readonly Point characterPanelPosition = new Point(10, 380);

		// Token: 0x04000266 RID: 614
		protected readonly Point captionOptionAnchor = new Point(110, 410);

		// Token: 0x04000267 RID: 615
		protected readonly Size characterPanelSize = new Size(200, 60);

		// Token: 0x04000268 RID: 616
		private string[] captionLines;

		// Token: 0x04000269 RID: 617
		private string[] captionOptionLines;

		// Token: 0x0400026A RID: 618
		private int[] captionStartIndexes = new int[3];

		// Token: 0x0400026B RID: 619
		private int captionVisibleChars;

		// Token: 0x0400026C RID: 620
		private double captionProgress;

		// Token: 0x0400026D RID: 621
		private double captionVoiceLength;

		// Token: 0x0400026E RID: 622
		private bool captionLocked;

		// Token: 0x0400026F RID: 623
		private Point captionLastPosition;

		// Token: 0x04000270 RID: 624
		private bool skipCaptionAnimation;

		// Token: 0x04000271 RID: 625
		protected readonly Point captionPosition = new Point(25, 455);

		// Token: 0x04000272 RID: 626
		protected readonly Size captionCharSize = new Size(30, 40);

		// Token: 0x04000273 RID: 627
		protected readonly Point captionOptionPosition = new Point(110, 390);

		// Token: 0x04000274 RID: 628
		private Microsoft.DirectX.Direct3D.Font captionFont;

		// Token: 0x04000275 RID: 629
		private int modeIconFrame;

		// Token: 0x04000276 RID: 630
		protected readonly Point modeIconPosition = new Point(40, 20);

		// Token: 0x04000277 RID: 631
		protected readonly Size modeIconTextureSize = new Size(128, 96);
	}
}
