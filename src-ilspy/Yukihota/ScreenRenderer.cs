using System;
using System.Collections;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Yukihota
{
	public class ScreenRenderer : IDisposable
	{
		protected const int shakeFrameCount = 16;

		protected const int shakeStep = 4;

		protected const int indicatorCellWidth = 32;

		protected const int indicatorCellHeight = 32;

		private const int fontCount = 3;

		protected const int shadowOffsetPixels = 2;

		protected const int characterFadeFrames = 8;

		protected const int shockFrames = 16;

		protected const int backgroundFadeFrames = 16;

		protected const int halfAlpha = 128;

		protected const int sunSpriteCount = 5;

		protected const int sunSpawnDelayMin = 20;

		protected const int sunSpriteSize = 80;

		protected const double sunScaleMin = 0.5;

		protected const double sunSpawnChance = 0.25;

		protected const int sunScaleMax = 450;

		protected const int sunOffsetRange = 75;

		protected const int sunCenterOffset = 18;

		protected const int petalTextureSize = 80;

		protected const int petalMaxCount = 30;

		protected const int petalMinCount = 20;

		protected const float petalSpeedRange = 16f;

		protected const float petalDriftRange = 8f;

		protected const float petalDriftStep = 0.1f;

		protected const int petalLifeMin = 40;

		protected const int petalLifeMax = 200;

		protected const double petalSpawnChance = 0.2;

		protected const int petalFrameSizePixels = 24;

		protected const int petalFrameCount = 4;

		protected const int petalTypeCount = 4;

		protected const int petalStepCount = 8;

		protected const int snowflakeMaxCount = 25;

		protected const int snowflakeMinCount = 15;

		protected const float snowSpeedRange = 6f;

		protected const float snowSpeedBase = 2f;

		protected const float snowSwayRange = 24f;

		protected const float snowSwayBase = 8f;

		protected const float snowSwayFrequencyRange = 0.02f;

		protected const float snowSwayFrequencyBase = 0.01f;

		protected const double twoPi = Math.PI * 2.0;

		protected const double snowSpawnChance = 0.2;

		protected const int snowFrameSizePixels = 16;

		protected const int snowFrameCount = 4;

		protected const int dimFadeFrames = 10;

		protected const int dimAlpha = 128;

		protected const int thinkFadeFrames = 10;

		protected const int characterAppearFrames = 8;

		protected const int characterAppearDivisor = 6;

		protected const int dialogueExpandFrames = 3;

		protected const int dialogueTextWidth = 15;

		protected const int dialogueShrinkFrames = 3;

		protected const int messageFadeFrames = 10;

		protected const int messageHoldFrames = 50;

		protected const int noticeLineWidth = 15;

		protected const int textShadowDistance = 2;

		protected const int fontSizePixels = 24;

		protected const int messageSpeedDivisor = 6;

		protected const int textFadeLength = 4;

		protected const int captionFadeFrames = 10;

		protected const int captionLineWidth = 10;

		protected const int captionShadowDistance = 2;

		protected const int captionFontHeight = 24;

		protected const int messageMaxLineWidth = 25;

		protected const int messageOptionIndent = 13;

		protected const int messageWrapFrames = 3;

		protected const int messageRevealStep = 6;

		protected const int messageRevealChunk = 4;

		protected const int messageFadeStep = 2;

		protected const int dialogueFramePadding = 24;

		protected const int dialogueLineWidth = 25;

		protected const int dialogueFadeStep = 3;

		protected const int optionLineCount = 5;

		protected const int modeIconFrameCount = 9;

		protected const int modeIconColumns = 3;

		protected const int modeIconCellSize = 32;

		protected ArchiveFile systemArchive;

		private ButtonLayout layoutIndex;

		private System.Drawing.Font baseFont;

		private bool dialogueHidden;

		private AudioManager audioManager;

		private Texture modeIconTexture;

		private int modeIconIndex;

		private Device device;

		private Sprite sprite;

		private FrameCallback resetCallback;

		protected readonly Rectangle screenRect = new Rectangle(0, 0, 800, 600);

		protected Texture whiteTexture;

		protected ArrayList textureQueue = new ArrayList();

		protected ArrayList destRectQueue = new ArrayList();

		protected ArrayList srcRectQueue = new ArrayList();

		protected ArrayList drawPositionQueue = new ArrayList();

		protected ArrayList colorQueue = new ArrayList();

		protected ArrayList textQueue = new ArrayList();

		protected ArrayList startIndexQueue = new ArrayList();

		protected ArrayList endIndexQueue = new ArrayList();

		protected ArrayList textPositionQueue = new ArrayList();

		protected ArrayList charWidthQueue = new ArrayList();

		protected ArrayList lineHeightQueue = new ArrayList();

		protected ArrayList shadowOffsetQueue = new ArrayList();

		protected ArrayList fontSizeModeQueue = new ArrayList();

		protected ArrayList lineWidthQueue = new ArrayList();

		protected ArrayList fontIndexQueue = new ArrayList();

		protected ArrayList fadeOutQueue = new ArrayList();

		protected ArrayList fadeLengthQueue = new ArrayList();

		protected ArrayList textColorQueue = new ArrayList();

		protected ArrayList shadowColorQueue = new ArrayList();

		private bool fadeEnabled;

		private bool sunEffectEnabled;

		private bool effectEnabled;

		private bool shockEffectEnabled;

		private Color dialogueColor;

		private int messageSpeed = 1;

		private bool syncVoice;

		private int indicatorPhase;

		protected readonly int[] indicatorColumns = new int[16]
		{
			3, 3, 2, 2, 1, 1, 0, 0, 0, 0,
			1, 1, 2, 2, 3, 3
		};

		protected readonly int[] indicatorOffsetX = new int[32]
		{
			-2, -1, -1, 0, 0, 1, 1, 2, 2, 1,
			1, 0, 0, -1, -1, -2, -2, -1, -1, 0,
			0, 1, 1, 2, 2, 1, 1, 0, 0, -1,
			-1, -2
		};

		protected readonly int[] indicatorOffsetY = new int[32]
		{
			-1, -1, 0, 0, 0, 0, 1, 1, 1, 1,
			2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
			0, 0, -1, -1, -1, -1, -2, -2, -2, -2,
			-1, -1
		};

		private Texture backgroundTexture;

		private Texture desaturatedBackgroundTexture;

		private Color backgroundColor;

		private Texture previousBackgroundTexture;

		private Color previousBackgroundColor;

		private bool backgroundWipeActive;

		private bool backgroundIsColor;

		private bool previousBackgroundIsColor;

		private bool backgroundDrawnAsColor;

		private int backgroundFadeCounter;

		private int backgroundFadeSpeed;

		protected readonly int[] backgroundFadeDurations = new int[3] { 1, 10, 40 };

		private bool backgroundRedrawPending;

		private TextureBlend backgroundWipeBlend;

		private bool zoomActive;

		private Rectangle zoomTargetRect;

		private Rectangle zoomSourceRect;

		private int zoomCounter;

		private int zoomSpeedIndex;

		protected readonly int[] zoomDurations = new int[3] { 1, 10, 40 };

		private bool shakeActive;

		private bool shakeLoop;

		private int shakeFrame;

		protected readonly int[] shakeOffsetTableX = new int[16]
		{
			-16, -8, 0, 8, 16, 8, 0, -8, -8, 0,
			8, 0, -4, 0, 4, 0
		};

		protected readonly int[] shakeOffsetTableY = new int[16]
		{
			-16, 16, 0, 0, -16, 16, 0, 0, -8, 8,
			0, 0, -4, 4, 0, 0
		};

		protected readonly int[] quakeOffsetTableX = new int[16]
		{
			-4, -3, -2, -1, 0, 1, 2, 3, 4, 3,
			2, 1, 0, -1, -2, -3
		};

		protected readonly int[] quakeOffsetTableY = new int[16]
		{
			-4, -2, 0, 2, 4, 2, 0, -2, -4, -2,
			0, 2, 4, 2, 0, -2
		};

		private bool sunActive;

		private Texture sunTexture;

		private int[] sunLifeCounters = new int[5];

		private int[] sunLifeTotals = new int[5];

		private double[] sunScales = new double[5];

		private int[] sunOffsets = new int[5];

		private Random sunRandom;

		private bool petalActive;

		private Texture petalTexture;

		private int[] petalFrames = new int[30];

		private PointF[] petalPositions = new PointF[30];

		private float[] petalVelocitiesY = new float[30];

		private float[] petalVelocitiesX = new float[30];

		private float[] petalTargetVelocitiesY = new float[30];

		private float[] petalTargetVelocitiesX = new float[30];

		private int[] petalTurnTimers = new int[30];

		private int[] petalLifeCounters = new int[30];

		private Random petalRandom;

		private int petalActiveCount;

		protected readonly Size petalFrameSize = new Size(96, 192);

		private bool snowActive;

		private Texture snowTexture;

		private PointF[] snowPositions = new PointF[25];

		private float[] snowSpeeds = new float[25];

		private float[] snowSwayAmplitudes = new float[25];

		private float[] snowSwayFrequencies = new float[25];

		private float[] snowSwayPhases = new float[25];

		private int[] snowLifeCounters = new int[25];

		private Random snowRandom;

		private int snowActiveCount;

		protected readonly Size snowFrameSize = new Size(64, 16);

		private bool dimActive;

		private bool dimReverse;

		private int dimCounter;

		private bool thinkActive;

		private bool thinkReverse;

		private int thinkCounter;

		private Texture thinkTexture;

		private Texture[] characterTextures = new Texture[3];

		private Texture[] previousCharacterTextures = new Texture[3];

		private Texture[] characterShadowTextures = new Texture[3];

		private Size[] characterSizes = new Size[3];

		private Size[] previousCharacterSizes = new Size[3];

		private int[] characterAppearCounters = new int[3];

		private int[] characterOffsets = new int[3];

		private int[] previousCharacterOffsets = new int[3];

		private bool[] characterSemiTransparent = new bool[3];

		private int[] characterShakeCounters = new int[3];

		private int[] characterShakePatterns = new int[3];

		protected readonly int[] characterShakePatternLengths = new int[3] { 4, 6, 16 };

		protected readonly Color semiTransparentWhite = Color.FromArgb(128, Color.White);

		protected int[][] characterShakeTableX;

		protected int[][] characterShakeTableY;

		private string[] noticeLines;

		private int noticeTimer;

		protected readonly Point noticePosition = new Point(300, 20);

		protected readonly Size noticeCharSize = new Size(30, 40);

		private Microsoft.DirectX.Direct3D.Font noticeFont;

		private string[] messageLines;

		private int[] messageStartIndexes = new int[3];

		private int messageVisibleChars;

		private double messageProgress;

		private int lineRevealCounter;

		private int messageFadeCounter;

		private HorizontalAlign messageFontSizeMode;

		private TextRevealMode messageAlignMode;

		private bool showNextIndicator;

		private Color messageTextColor;

		private Color messageShadowColor;

		private double messageVoiceLength;

		private Point lastTextPosition;

		protected readonly Point messagePosition = new Point(20, 60);

		protected readonly Size messageCharSize = new Size(30, 40);

		private Microsoft.DirectX.Direct3D.Font messageFont;

		private bool dialogueActive;

		private bool characterPanelVisible;

		protected Texture dialogueTexture;

		protected Texture characterPanelTexture;

		private bool dialogueClosing;

		private int dialogueCounter;

		protected readonly Rectangle dialogueRect = new Rectangle(10, 440, 780, 150);

		protected readonly Point dialogueTextAnchor = new Point(400, 515);

		protected readonly Point characterPanelPosition = new Point(10, 380);

		protected readonly Point captionOptionAnchor = new Point(110, 410);

		protected readonly Size characterPanelSize = new Size(200, 60);

		private string[] captionLines;

		private string[] captionOptionLines;

		private int[] captionStartIndexes = new int[3];

		private int captionVisibleChars;

		private double captionProgress;

		private double captionVoiceLength;

		private bool captionLocked;

		private Point captionLastPosition;

		private bool skipCaptionAnimation;

		protected readonly Point captionPosition = new Point(25, 455);

		protected readonly Size captionCharSize = new Size(30, 40);

		protected readonly Point captionOptionPosition = new Point(110, 390);

		private Microsoft.DirectX.Direct3D.Font captionFont;

		private int modeIconFrame;

		protected readonly Point modeIconPosition = new Point(40, 20);

		protected readonly Size modeIconTextureSize = new Size(128, 96);

		public ScreenRenderer(Device d3dDevice, Sprite d3dSprite, FrameCallback frameCallback, Texture defaultTexture, System.Drawing.Font font, ButtonLayout buttonLayout, Color color, AudioManager audio)
		{
			device = d3dDevice;
			sprite = d3dSprite;
			resetCallback = frameCallback;
			whiteTexture = defaultTexture;
			dialogueColor = color;
			baseFont = font;
			layoutIndex = buttonLayout;
			audioManager = audio;
			dialogueHidden = false;
			modeIconIndex = 0;
			systemArchive = new ArchiveFile("System", ArchiveMode.Read);
			InitNextIndicator();
			InitBackground();
			InitEffects();
			InitCharacters();
			InitNotice();
			InitMessage();
			InitDialogueFrame();
			InitCaption();
			InitModeIcon();
		}

		~ScreenRenderer()
		{
			textureQueue.Clear();
			destRectQueue.Clear();
			srcRectQueue.Clear();
			drawPositionQueue.Clear();
			colorQueue.Clear();
			textQueue.Clear();
			startIndexQueue.Clear();
			endIndexQueue.Clear();
			textPositionQueue.Clear();
			charWidthQueue.Clear();
			lineHeightQueue.Clear();
			shadowOffsetQueue.Clear();
			fontSizeModeQueue.Clear();
			lineWidthQueue.Clear();
			fontIndexQueue.Clear();
			fadeOutQueue.Clear();
			fadeLengthQueue.Clear();
			textColorQueue.Clear();
			shadowColorQueue.Clear();
		}

		public void Render()
		{
			device.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			textureQueue.Clear();
			destRectQueue.Clear();
			srcRectQueue.Clear();
			drawPositionQueue.Clear();
			colorQueue.Clear();
			textQueue.Clear();
			startIndexQueue.Clear();
			endIndexQueue.Clear();
			textPositionQueue.Clear();
			charWidthQueue.Clear();
			lineHeightQueue.Clear();
			shadowOffsetQueue.Clear();
			fontSizeModeQueue.Clear();
			lineWidthQueue.Clear();
			fontIndexQueue.Clear();
			fadeOutQueue.Clear();
			fadeLengthQueue.Clear();
			textColorQueue.Clear();
			shadowColorQueue.Clear();
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			DrawBackground();
			DrawBackgroundEffects();
			DrawCharacters();
			DrawForegroundEffects();
			DrawMessage();
			if (!dialogueHidden)
			{
				DrawCaption();
			}
			FadeOutBackground();
			DrawNotice();
			DrawModeIcon();
			sprite.End();
			device.EndScene();
			try
			{
				device.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					device.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					resetCallback();
				}
				catch
				{
				}
			}
		}

		public void Repaint()
		{
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			int layerIndex = 0;
			for (int i = 0; i < textureQueue.Count; i++)
			{
				if (textureQueue[i] != null)
				{
					sprite.Draw2D((Texture)textureQueue[i], (Rectangle)destRectQueue[i], (Rectangle)srcRectQueue[i], Point.Empty, 0f, (Point)drawPositionQueue[i], (Color)colorQueue[i]);
					continue;
				}
				Microsoft.DirectX.Direct3D.Font textFont;
				switch ((int)fontIndexQueue[layerIndex])
				{
				case 1:
					textFont = noticeFont;
					break;
				case 2:
					textFont = messageFont;
					break;
				case 3:
					textFont = captionFont;
					break;
				default:
					textFont = noticeFont;
					break;
				}
				DrawTextCore((string)textQueue[layerIndex], (int)startIndexQueue[layerIndex], (double)endIndexQueue[layerIndex], (Point)textPositionQueue[layerIndex], (int)charWidthQueue[layerIndex], (int)lineHeightQueue[layerIndex], (int)shadowOffsetQueue[layerIndex], (HorizontalAlign)fontSizeModeQueue[layerIndex], (int)lineWidthQueue[layerIndex], textFont, (bool)fadeOutQueue[layerIndex], (int)fadeLengthQueue[layerIndex], (Color)textColorQueue[layerIndex], (Color)shadowColorQueue[layerIndex], false);
				layerIndex++;
			}
			sprite.End();
			device.EndScene();
			try
			{
				device.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					device.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					resetCallback();
				}
				catch
				{
				}
			}
		}

		public bool IsBusy()
		{
			if (backgroundFadeCounter > 0 || zoomCounter > 0 || dimCounter > 0 || messageFadeCounter > 0 || dialogueCounter > 0 || (shakeFrame > 0 && !shakeLoop))
			{
				return true;
			}
			for (int i = 0; i < 3; i++)
			{
				if (characterAppearCounters[i] > 0 || characterShakeCounters[i] > 0)
				{
					return true;
				}
			}
			if (messageLines != null)
			{
				switch (messageAlignMode)
				{
				case TextRevealMode.Typewriter:
					if (messageProgress < (double)(messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4))
					{
						return true;
					}
					break;
				case TextRevealMode.Fade:
					if (lineRevealCounter > 0)
					{
						return true;
					}
					break;
				}
			}
			return captionLines != null && captionProgress < (double)(captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4);
		}

		public void HideDialogue(bool content)
		{
			dialogueHidden = content;
			if (content && captionLines != null)
			{
				captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
				captionVisibleChars = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex];
			}
		}

		public void HideDialogue()
		{
			dialogueHidden = !dialogueHidden;
			if (dialogueHidden && captionLines != null && captionVoiceLength == 0.0)
			{
				captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
				captionVisibleChars = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex];
			}
		}

		public void instant()
		{
			if (backgroundFadeCounter > 0)
			{
				backgroundFadeCounter = 0;
				if (backgroundWipeActive)
				{
					backgroundWipeActive = false;
				}
				else
				{
					backgroundRedrawPending = true;
				}
			}
			if (zoomCounter > 0)
			{
				zoomCounter = 0;
			}
			if (dimCounter > 0)
			{
				dimCounter = 0;
				if (dimReverse)
				{
					dimActive = false;
				}
			}
			if (thinkCounter > 0)
			{
				thinkCounter = 0;
				if (thinkReverse)
				{
					thinkActive = false;
				}
			}
			if (dialogueCounter > 0)
			{
				dialogueCounter = 0;
				if (dialogueClosing)
				{
					dialogueActive = false;
				}
			}
			if (shakeFrame > 0 && !shakeLoop)
			{
				shakeFrame = 1;
			}
			for (int i = 0; i < 3; i++)
			{
				if (characterAppearCounters[i] > 0)
				{
					characterAppearCounters[i] = 0;
				}
				if (characterShakeCounters[i] > 0)
				{
					characterShakeCounters[i] = 1;
				}
			}
			if (messageLines != null)
			{
				if (messageFadeCounter > 0)
				{
					messageFadeCounter = 0;
					messageProgress = 0.0;
					lineRevealCounter = 0;
					messageLines = null;
					for (int j = 0; j < 3; j++)
					{
						messageStartIndexes[j] = 0;
					}
					messageVisibleChars = 0;
				}
				else
				{
					messageVisibleChars = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex];
					switch (messageAlignMode)
					{
					case TextRevealMode.Typewriter:
						messageProgress = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4;
						break;
					case TextRevealMode.Fade:
						lineRevealCounter = 0;
						break;
					}
				}
			}
			if (captionLines != null)
			{
				captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
				captionVisibleChars = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex];
			}
		}

		public void SetButtonLayout(ButtonLayout buttonLayout)
		{
			if (layoutIndex != buttonLayout)
			{
				layoutIndex = buttonLayout;
				RebuildNoticeFont();
				RebuildMessageFont();
				RebuildCaptionFont();
			}
		}

		public void SetFont(System.Drawing.Font font)
		{
			if (baseFont != font)
			{
				baseFont = font;
				RebuildNoticeFont();
				RebuildMessageFont();
				RebuildCaptionFont();
			}
		}

		private void QueueDraw(Texture texture, Rectangle destRect, Rectangle srcRect, Point position, Color color)
		{
			sprite.Draw2D(texture, destRect, srcRect, Point.Empty, 0f, position, color);
			textureQueue.Add(texture);
			destRectQueue.Add(destRect);
			srcRectQueue.Add(srcRect);
			drawPositionQueue.Add(position);
			colorQueue.Add(color);
		}

		public void Dispose()
		{
			DisposeBackground();
			DisposeEffects();
			DisposeCharacters();
			DisposeNoticeFont();
			DisposeMessageFont();
			DisposeDialogueFrame();
			DisposeCaptionFont();
			DisposeModeIcon();
			GC.SuppressFinalize(this);
		}

		public void SetFadeEnabled(bool extensionName)
		{
			if (fadeEnabled != extensionName)
			{
				RebuildNoticeFont();
				RebuildMessageFont();
				RebuildCaptionFont();
			}
			fadeEnabled = extensionName;
		}

		public void SetSunEffectEnabled(bool extensionName)
		{
			sunEffectEnabled = extensionName;
		}

		public void SetEffectEnabled(bool extensionName)
		{
			effectEnabled = extensionName;
		}

		public void SetShockEffectEnabled(bool extensionName)
		{
			shockEffectEnabled = extensionName;
		}

		public void SetDialogueColor(Color extensionName)
		{
			dialogueColor = extensionName;
		}

		public void SetTextSpeed(int extensionName)
		{
			if (extensionName > 0)
			{
				messageSpeed = extensionName;
				return;
			}
			throw new Exception("Invalid Speed");
		}

		public void SetSyncVoice(bool extensionName)
		{
			syncVoice = extensionName;
		}

		private void InitNextIndicator()
		{
			indicatorPhase = 0;
		}

		private void DrawNextIndicator(int x, int y)
		{
			Rectangle indicatorRect = new Rectangle(indicatorColumns[indicatorPhase % 16] * 32, 0, 32, 32);
			Point indicatorPos = new Point(x + indicatorOffsetX[indicatorPhase], y + indicatorOffsetY[indicatorPhase]);
			QueueDraw(modeIconTexture, indicatorRect, Rectangle.Empty, indicatorPos, Color.White);
			indicatorPhase++;
			indicatorPhase %= 32;
		}

		private Point DrawTextCore(string content, Point origin, int charWidth, int lineHeight, int offset, Microsoft.DirectX.Direct3D.Font font, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(content, 0, content.Length, origin, charWidth, lineHeight, offset, HorizontalAlign.Left, 0, font, foreColor, shadowColor);
		}

		private Point DrawTextCore(string content, int startIndex, int endIndex, Point origin, int charWidth, int lineHeight, int offset, HorizontalAlign align, int lineWidth, Microsoft.DirectX.Direct3D.Font font, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(content, startIndex, endIndex, origin, charWidth, lineHeight, offset, align, lineWidth, font, false, 0, foreColor, shadowColor);
		}

		private Point DrawTextCore(string content, double endIndex, Point origin, int charWidth, int lineHeight, int offset, Microsoft.DirectX.Direct3D.Font font, int LanguageCount, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(content, 0, endIndex, origin, charWidth, lineHeight, offset, HorizontalAlign.Left, 0, font, true, LanguageCount, foreColor, shadowColor);
		}

		private Point DrawTextCore(string content, double endIndex, Point origin, int charWidth, int lineHeight, int offset, HorizontalAlign align, int lineWidth, Microsoft.DirectX.Direct3D.Font font, int LanguageCount, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(content, 0, endIndex, origin, charWidth, lineHeight, offset, align, lineWidth, font, true, LanguageCount, foreColor, shadowColor);
		}

		private Point DrawTextCore(string content, int startIndex, double endIndex, Point origin, int charWidth, int lineHeight, int offset, HorizontalAlign align, int lineWidth, Microsoft.DirectX.Direct3D.Font font, bool Typewriter, int LanguageCount, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(content, startIndex, endIndex, origin, charWidth, lineHeight, offset, align, lineWidth, font, Typewriter, LanguageCount, foreColor, shadowColor, true);
		}

		private Point DrawTextCore(string content, int startIndex, double endIndex, Point origin, int charWidth, int lineHeight, int offset, HorizontalAlign align, int lineWidth, Microsoft.DirectX.Direct3D.Font font, bool Typewriter, int LanguageCount, Color foreColor, Color shadowColor, bool scroll)
		{
			if (scroll)
			{
				textureQueue.Add(null);
				destRectQueue.Add(null);
				srcRectQueue.Add(null);
				drawPositionQueue.Add(null);
				colorQueue.Add(null);
				textQueue.Add(content);
				startIndexQueue.Add(startIndex);
				endIndexQueue.Add(endIndex);
				textPositionQueue.Add(origin);
				charWidthQueue.Add(charWidth);
				lineHeightQueue.Add(lineHeight);
				shadowOffsetQueue.Add(offset);
				fontSizeModeQueue.Add(align);
				lineWidthQueue.Add(lineWidth);
				fadeOutQueue.Add(Typewriter);
				fadeLengthQueue.Add(LanguageCount);
				textColorQueue.Add(foreColor);
				shadowColorQueue.Add(shadowColor);
			}
			int cursor;
			switch (align)
			{
			case HorizontalAlign.Left:
				cursor = origin.X;
				break;
			case HorizontalAlign.Center:
			{
				int firstBreak = content.IndexOf('\n');
				if (firstBreak == -1)
				{
					firstBreak = content.Length;
				}
				cursor = origin.X + charWidth * (lineWidth - firstBreak) / 2;
				break;
			}
			case HorizontalAlign.Right:
			{
				int lineBreak = content.IndexOf('\n');
				if (lineBreak == -1)
				{
					lineBreak = content.Length;
				}
				cursor = origin.X + charWidth * (lineWidth - lineBreak);
				break;
			}
			default:
				cursor = origin.X;
				break;
			}
			int lineY = origin.Y;
			for (int i = 0; i < content.Length; i++)
			{
				if (content[i] == '\n')
				{
					switch (align)
					{
					case HorizontalAlign.Left:
						cursor = origin.X;
						break;
					case HorizontalAlign.Center:
					{
						int nextBreak2 = content.IndexOf('\n', i + 1);
						if (nextBreak2 == -1)
						{
							nextBreak2 = content.Length;
						}
						int segmentLength2 = nextBreak2 - i - 1;
						cursor = origin.X + charWidth * (lineWidth - segmentLength2) / 2;
						break;
					}
					case HorizontalAlign.Right:
					{
						int nextBreak = content.IndexOf('\n', i + 1);
						if (nextBreak == -1)
						{
							nextBreak = content.Length;
						}
						int segmentLength = nextBreak - i - 1;
						cursor = origin.X + charWidth * (lineWidth - segmentLength);
						break;
					}
					}
					lineY += lineHeight;
					continue;
				}
				if (i >= startIndex && (double)i < endIndex)
				{
					if (fadeEnabled)
					{
						Sprite spriteLocal = this.sprite;
						font.DrawText(spriteLocal, content[i].ToString(), cursor + offset, lineY + offset, shadowColor);
					}
					Sprite sprite2 = sprite;
					font.DrawText(sprite2, content[i].ToString(), cursor, lineY, foreColor);
				}
				else if (Typewriter && i >= startIndex && (double)i >= endIndex && (double)i < endIndex + (double)LanguageCount && fadeEnabled)
				{
					int alpha = (int)(255.0 * ((double)(LanguageCount - i) + endIndex) / (double)LanguageCount);
					Sprite sprite3 = sprite;
					font.DrawText(sprite3, content[i].ToString(), cursor + offset, lineY + offset, (shadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha, shadowColor));
					Sprite sprite4 = sprite;
					font.DrawText(sprite4, content[i].ToString(), cursor, lineY, Color.FromArgb(alpha, foreColor));
				}
				cursor += charWidth;
			}
			return new Point(cursor, lineY);
		}

		private void InitBackground()
		{
			backgroundFadeCounter = 0;
			backgroundColor = (previousBackgroundColor = Color.White);
			backgroundTexture = (previousBackgroundTexture = (desaturatedBackgroundTexture = null));
			backgroundIsColor = (previousBackgroundIsColor = true);
			backgroundWipeActive = false;
			backgroundDrawnAsColor = false;
			backgroundRedrawPending = false;
		}

		private void DisposeBackground()
		{
			if (backgroundTexture != null)
			{
				backgroundTexture.Dispose();
			}
			if (previousBackgroundTexture != null)
			{
				previousBackgroundTexture.Dispose();
			}
			if (desaturatedBackgroundTexture != null)
			{
				desaturatedBackgroundTexture.Dispose();
			}
		}

		public void SetBackground(Texture content, int durationMode)
		{
			if (previousBackgroundTexture != null)
			{
				previousBackgroundTexture.Dispose();
			}
			previousBackgroundTexture = backgroundTexture;
			previousBackgroundColor = backgroundColor;
			backgroundTexture = content;
			backgroundColor = Color.White;
			previousBackgroundIsColor = backgroundIsColor;
			backgroundIsColor = false;
			backgroundFadeSpeed = durationMode;
			backgroundFadeCounter = backgroundFadeDurations[durationMode];
			backgroundWipeActive = false;
			if (thinkActive)
			{
				RebuildDesaturatedBackground();
			}
			else if (desaturatedBackgroundTexture != null)
			{
				desaturatedBackgroundTexture.Dispose();
				desaturatedBackgroundTexture = null;
			}
		}

		public void SetBackground(Color content, int durationMode)
		{
			if (previousBackgroundTexture != null)
			{
				previousBackgroundTexture.Dispose();
			}
			previousBackgroundTexture = backgroundTexture;
			previousBackgroundColor = backgroundColor;
			backgroundColor = content;
			backgroundTexture = null;
			previousBackgroundIsColor = backgroundIsColor;
			backgroundIsColor = true;
			backgroundFadeSpeed = durationMode;
			backgroundFadeCounter = backgroundFadeDurations[durationMode];
			backgroundWipeActive = false;
			if (!thinkActive && desaturatedBackgroundTexture != null)
			{
				desaturatedBackgroundTexture.Dispose();
				desaturatedBackgroundTexture = null;
			}
		}

		private void DrawBackground()
		{
			if (backgroundFadeCounter > 0 || backgroundRedrawPending)
			{
				if (backgroundWipeActive)
				{
					if (previousBackgroundTexture != null)
					{
						if (zoomActive)
						{
							QueueDraw(previousBackgroundTexture, zoomSourceRect, screenRect, Point.Empty, Color.White);
						}
						else
						{
							QueueDraw(previousBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						}
					}
					else
					{
						QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, previousBackgroundColor);
					}
					if (fadeEnabled)
					{
						Point wipePosition = new Point(0, 0);
						switch (backgroundWipeBlend)
						{
						case TextureBlend.Normal:
							if (zoomActive)
							{
								QueueDraw(backgroundTexture, zoomTargetRect, screenRect, Point.Empty, Color.FromArgb((int)(255.0 * (double)(backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / (double)backgroundFadeDurations[backgroundFadeSpeed]), Color.White));
							}
							else
							{
								QueueDraw(backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / (double)backgroundFadeDurations[backgroundFadeSpeed]), Color.White));
							}
							break;
						case TextureBlend.Left:
						{
							if (zoomActive)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle sourceRect;
							for (int j = 0; j < 16; j++)
							{
								wipePosition.X = 832 * (backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / backgroundFadeDurations[backgroundFadeSpeed] + (-j - 1) * 2;
								sourceRect = new Rectangle(wipePosition.X, 0, 2, 600);
								int alpha2 = (int)((double)(j + 1) / 17.0 * 255.0);
								QueueDraw(backgroundTexture, sourceRect, Rectangle.Empty, wipePosition, Color.FromArgb(alpha2, Color.White));
							}
							wipePosition.X = 0;
							sourceRect = new Rectangle(wipePosition.X, 0, 832 * (backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / backgroundFadeDurations[backgroundFadeSpeed] - 32, 600);
							QueueDraw(backgroundTexture, sourceRect, Rectangle.Empty, wipePosition, Color.White);
							break;
						}
						case TextureBlend.Right:
						{
							if (zoomActive)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle indicatorPos;
							for (int i = 0; i < 16; i++)
							{
								wipePosition.X = 832 * backgroundFadeCounter / backgroundFadeDurations[backgroundFadeSpeed] + (i - 16) * 2;
								indicatorPos = new Rectangle(wipePosition.X, 0, 2, 600);
								int alpha = (int)((double)(i + 1) / 17.0 * 255.0);
								QueueDraw(backgroundTexture, indicatorPos, Rectangle.Empty, wipePosition, Color.FromArgb(alpha, Color.White));
							}
							wipePosition.X = 832 * backgroundFadeCounter / backgroundFadeDurations[backgroundFadeSpeed];
							indicatorPos = new Rectangle(wipePosition.X, 0, 800 - wipePosition.X, 600);
							QueueDraw(backgroundTexture, indicatorPos, Rectangle.Empty, wipePosition, Color.White);
							break;
						}
						}
						backgroundFadeCounter--;
					}
					else
					{
						backgroundFadeCounter = 0;
					}
					if (backgroundFadeCounter == 0)
					{
						backgroundWipeActive = false;
					}
				}
				else if (!previousBackgroundIsColor)
				{
					if (!zoomActive && (!shakeActive || !shakeLoop))
					{
						if (previousBackgroundTexture != null)
						{
							if (thinkActive)
							{
								QueueDraw(desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
							else
							{
								QueueDraw(previousBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, previousBackgroundColor);
						}
					}
					backgroundDrawnAsColor = false;
				}
				else
				{
					if (!zoomActive && (!shakeActive || !shakeLoop))
					{
						if (backgroundTexture != null)
						{
							if (thinkActive)
							{
								QueueDraw(desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
							else
							{
								QueueDraw(backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, backgroundColor);
						}
					}
					backgroundDrawnAsColor = true;
				}
				if (backgroundRedrawPending)
				{
					backgroundRedrawPending = false;
				}
				return;
			}
			if (!zoomActive && (!shakeActive || !shakeLoop))
			{
				if (backgroundTexture != null)
				{
					if (thinkActive)
					{
						if (thinkCounter > 0)
						{
							float fadeRatio = (thinkReverse ? ((float)thinkCounter / 10f) : ((float)(10 - thinkCounter) / 10f));
							QueueDraw(backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							QueueDraw(desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * fadeRatio), Color.White));
						}
						else
						{
							QueueDraw(desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						}
					}
					else
					{
						QueueDraw(backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
					}
				}
				else
				{
					QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, backgroundColor);
				}
			}
			backgroundDrawnAsColor = true;
		}

		private void FadeOutBackground()
		{
			if (backgroundFadeCounter <= 0 || backgroundWipeActive)
			{
				return;
			}
			if (fadeEnabled)
			{
				if (backgroundDrawnAsColor && previousBackgroundIsColor)
				{
					QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)backgroundFadeCounter / (double)backgroundFadeDurations[backgroundFadeSpeed]), previousBackgroundColor));
				}
				else
				{
					if (backgroundDrawnAsColor || !backgroundIsColor)
					{
						throw new Exception("Not Implement Yet");
					}
					QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / (double)backgroundFadeDurations[backgroundFadeSpeed]), backgroundColor));
				}
				backgroundFadeCounter--;
			}
			else
			{
				backgroundFadeCounter = 0;
			}
		}

		private void RebuildDesaturatedBackground()
		{
			if (desaturatedBackgroundTexture != null)
			{
				desaturatedBackgroundTexture.Dispose();
			}
			if (!(backgroundTexture != null))
			{
				return;
			}
			desaturatedBackgroundTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("WHITE"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			uint[,] array = (uint[,])backgroundTexture.LockRectangle(typeof(uint), 0, LockFlags.None, 800, 600);
			uint[,] array2 = (uint[,])desaturatedBackgroundTexture.LockRectangle(typeof(uint), 0, LockFlags.None, 800, 600);
			for (int i = 0; i < 800; i++)
			{
				for (int j = 0; j < 600; j++)
				{
					uint pixel = array[i, j];
					uint gray = ((pixel & 0xFF) + ((pixel >> 8) & 0xFF) + ((pixel >> 16) & 0xFF)) / 3;
					array2[i, j] = (uint)(-16777216 + (int)(gray << 16) + (int)(gray << 8)) + gray;
				}
			}
			backgroundTexture.UnlockRectangle(0);
			desaturatedBackgroundTexture.UnlockRectangle(0);
		}

		private void InitEffects()
		{
			InitBackgroundWipe();
			InitZoom();
			InitShake();
			InitSunEffect();
			InitPetals();
			InitSnow();
			InitDimOverlay();
			InitThinkOverlay();
		}

		private void DisposeEffects()
		{
			DisposeSunEffect();
			DisposePetals();
			DisposeSnow();
			DisposeThinkOverlay();
		}

		private void InitBackgroundWipe()
		{
			backgroundWipeBlend = TextureBlend.Normal;
		}

		public void SetBackgroundWithWipe(Texture texture, TextureBlend blend, bool animate, int durationMode)
		{
			if (previousBackgroundTexture != null)
			{
				previousBackgroundTexture.Dispose();
			}
			previousBackgroundTexture = backgroundTexture;
			previousBackgroundColor = backgroundColor;
			backgroundTexture = texture;
			backgroundColor = Color.White;
			previousBackgroundIsColor = backgroundIsColor;
			backgroundIsColor = false;
			backgroundFadeSpeed = durationMode;
			if (animate)
			{
				backgroundFadeCounter = backgroundFadeDurations[durationMode];
			}
			else
			{
				backgroundFadeCounter = 0;
			}
			backgroundWipeActive = true;
			backgroundWipeBlend = blend;
		}

		private void InitZoom()
		{
			zoomActive = false;
			zoomTargetRect = (zoomSourceRect = screenRect);
			zoomCounter = 0;
			zoomSpeedIndex = 0;
		}

		public void ZoomBackground(double left, double right, double top, double bottom, int zoomSpeed, bool animate, bool finish)
		{
			if (finish)
			{
				zoomActive = false;
				zoomSourceRect = (zoomTargetRect = screenRect);
				return;
			}
			zoomActive = true;
			if (backgroundTexture != null)
			{
				backgroundIsColor = false;
			}
			else if (previousBackgroundTexture != null)
			{
				previousBackgroundIsColor = false;
			}
			zoomSourceRect = zoomTargetRect;
			zoomTargetRect = new Rectangle((int)(left * 800.0), (int)(top * 600.0), (int)((right - left) * 800.0), (int)((bottom - top) * 600.0));
			zoomSpeedIndex = zoomSpeed;
			if (animate)
			{
				zoomCounter = zoomDurations[zoomSpeedIndex];
			}
			else
			{
				zoomCounter = 0;
			}
		}

		public void DrawZoomTransition()
		{
			if (!zoomActive || backgroundWipeActive)
			{
				return;
			}
			if (zoomCounter > 0)
			{
				if (fadeEnabled)
				{
					if (!shakeActive || !shakeLoop)
					{
						double progress = (double)zoomCounter / (double)zoomDurations[zoomSpeedIndex];
						double inverse = 1.0 - progress;
						Rectangle zoomRect = new Rectangle((int)(progress * (double)zoomSourceRect.Left + inverse * (double)zoomTargetRect.Left), (int)(progress * (double)zoomSourceRect.Top + inverse * (double)zoomTargetRect.Top), (int)(progress * (double)zoomSourceRect.Width + inverse * (double)zoomTargetRect.Width), (int)(progress * (double)zoomSourceRect.Height + inverse * (double)zoomTargetRect.Height));
						if (backgroundTexture != null)
						{
							QueueDraw(backgroundTexture, zoomRect, screenRect, Point.Empty, Color.White);
						}
						else if (previousBackgroundTexture != null)
						{
							QueueDraw(previousBackgroundTexture, zoomRect, screenRect, Point.Empty, Color.White);
						}
					}
					zoomCounter--;
				}
				else
				{
					if (!shakeActive || !shakeLoop)
					{
						if (backgroundTexture != null)
						{
							QueueDraw(backgroundTexture, zoomTargetRect, screenRect, Point.Empty, Color.White);
						}
						else if (previousBackgroundTexture != null)
						{
							QueueDraw(previousBackgroundTexture, zoomTargetRect, screenRect, Point.Empty, Color.White);
						}
					}
					zoomCounter = 0;
				}
			}
			else if (!shakeActive || !shakeLoop)
			{
				if (backgroundTexture != null)
				{
					QueueDraw(backgroundTexture, zoomTargetRect, screenRect, Point.Empty, Color.White);
				}
				else if (previousBackgroundTexture != null)
				{
					QueueDraw(previousBackgroundTexture, zoomTargetRect, screenRect, Point.Empty, Color.White);
				}
			}
			if (!shakeActive || !shakeLoop)
			{
				QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		private void InitShake()
		{
			shakeActive = false;
			shakeLoop = false;
			shakeFrame = 0;
		}

		public void SetShake(bool loop, bool finish)
		{
			if (!finish)
			{
				shakeActive = true;
				shakeLoop = loop;
				if (loop)
				{
					shakeFrame = 0;
				}
				else
				{
					shakeFrame = 16;
				}
				if (backgroundTexture != null)
				{
					backgroundIsColor = false;
				}
				else if (previousBackgroundTexture != null)
				{
					previousBackgroundIsColor = false;
				}
			}
			else
			{
				shakeActive = false;
			}
		}

		private void DrawShake()
		{
			if (!shakeActive)
			{
				return;
			}
			if (!shakeLoop)
			{
				if (shockEffectEnabled)
				{
					if (backgroundTexture != null)
					{
						QueueDraw(backgroundTexture, Rectangle.Empty, Rectangle.Empty, new Point(shakeOffsetTableX[16 - shakeFrame], shakeOffsetTableY[16 - shakeFrame]), Color.FromArgb(128, Color.White));
					}
					else if (previousBackgroundTexture != null)
					{
						QueueDraw(previousBackgroundTexture, Rectangle.Empty, Rectangle.Empty, new Point(shakeOffsetTableX[16 - shakeFrame], shakeOffsetTableY[16 - shakeFrame]), Color.FromArgb(128, Color.White));
					}
				}
				shakeFrame--;
				if (shakeFrame == 0)
				{
					shakeActive = false;
				}
				return;
			}
			Rectangle rectangle;
			if (zoomActive && !backgroundWipeActive)
			{
				if (zoomCounter > 0 && fadeEnabled)
				{
					double progress = (double)zoomCounter / (double)zoomDurations[zoomSpeedIndex];
					double inverse = 1.0 - progress;
					rectangle = new Rectangle((int)(progress * (double)zoomSourceRect.Left + inverse * (double)zoomTargetRect.Left), (int)(progress * (double)zoomSourceRect.Top + inverse * (double)zoomTargetRect.Top), (int)(progress * (double)zoomSourceRect.Width + inverse * (double)zoomTargetRect.Width), (int)(progress * (double)zoomSourceRect.Height + inverse * (double)zoomTargetRect.Height));
				}
				else
				{
					rectangle = zoomTargetRect;
				}
			}
			else
			{
				rectangle = Rectangle.Empty;
			}
			if (backgroundTexture != null)
			{
				QueueDraw(backgroundTexture, rectangle, screenRect, new Point(quakeOffsetTableX[shakeFrame], quakeOffsetTableY[shakeFrame]), Color.White);
			}
			else if (previousBackgroundTexture != null)
			{
				QueueDraw(previousBackgroundTexture, rectangle, screenRect, new Point(quakeOffsetTableX[shakeFrame], quakeOffsetTableY[shakeFrame]), Color.White);
			}
			shakeFrame++;
			if (shakeFrame == 16)
			{
				shakeFrame = 0;
			}
			if (rectangle != Rectangle.Empty)
			{
				QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		private void InitSunEffect()
		{
			sunTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("SUN"), 80, 80, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			sunActive = false;
			for (int i = 0; i < 5; i++)
			{
				sunLifeCounters[i] = 0;
				sunLifeTotals[i] = 0;
				sunScales[i] = 0.0;
				sunOffsets[i] = 0;
			}
			sunRandom = new Random();
		}

		private void DisposeSunEffect()
		{
			sunTexture.Dispose();
		}

		public void SetSunActive(bool finish)
		{
			if (!finish)
			{
				sunActive = true;
				backgroundIsColor = false;
			}
			else
			{
				sunActive = false;
			}
		}

		private void DrawSunEffect()
		{
			if (!sunActive || !sunEffectEnabled)
			{
				return;
			}
			int freeSlot = -1;
			for (int i = 0; i < 5; i++)
			{
				if (sunLifeTotals[i] == 0)
				{
					freeSlot = i;
					break;
				}
			}
			if (freeSlot >= 0 && sunRandom.NextDouble() < 0.25)
			{
				sunLifeCounters[freeSlot] = 0;
				sunLifeTotals[freeSlot] = (int)(20.0 + 60.0 * sunRandom.NextDouble());
				sunScales[freeSlot] = 0.5 + 0.5 * sunRandom.NextDouble();
				sunOffsets[freeSlot] = (int)(-75.0 + 150.0 * sunRandom.NextDouble());
			}
			for (int j = 0; j < 5; j++)
			{
				if (sunLifeTotals[j] > 0)
				{
					if ((double)sunLifeCounters[j] < (double)sunLifeTotals[j] / 2.0)
					{
						DrawSunSprite(sunOffsets[j], sunScales[j], (double)sunLifeCounters[j] / ((double)sunLifeTotals[j] / 2.0));
						int[] array;
						IntPtr intPtr;
						(array = sunLifeCounters)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
						continue;
					}
					if (sunLifeCounters[j] < sunLifeTotals[j])
					{
						DrawSunSprite(sunOffsets[j], sunScales[j], (double)(sunLifeTotals[j] - sunLifeCounters[j]) / ((double)sunLifeTotals[j] / 2.0));
						int[] array2;
						IntPtr intPtr2;
						(array2 = sunLifeCounters)[(int)(intPtr2 = (IntPtr)j)] = array2[(int)intPtr2] + 1;
						continue;
					}
					sunLifeCounters[j] = 0;
					sunLifeTotals[j] = 0;
					sunScales[j] = 0.0;
					sunOffsets[j] = 0;
				}
			}
			QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
		}

		private void DrawSunSprite(int origin, double scale, double durationMode)
		{
			int x = (origin - 18) * 80 / (int)(450.0 * scale);
			int y = (-origin - 18) * 80 / (int)(450.0 * scale);
			Rectangle sunRect = new Rectangle(0, 0, 80, 80);
			Rectangle indicatorPos = new Rectangle(0, 0, (int)(450.0 * scale), (int)(450.0 * scale));
			QueueDraw(sunTexture, sunRect, indicatorPos, new Point(x, y), Color.FromArgb((int)(scale * durationMode * 255.0), Color.White));
		}

		private void InitPetals()
		{
			petalTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("SAKURA"), petalFrameSize.Width, petalFrameSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			petalActive = false;
			for (int i = 0; i < 30; i++)
			{
				petalFrames[i] = 0;
				petalPositions[i] = Point.Empty;
				petalVelocitiesY[i] = 0f;
				petalVelocitiesX[i] = 0f;
				petalTargetVelocitiesY[i] = 0f;
				petalTargetVelocitiesX[i] = 0f;
				petalTurnTimers[i] = 0;
				petalLifeCounters[i] = 0;
			}
			petalActiveCount = 0;
			petalRandom = new Random();
		}

		private void DisposePetals()
		{
			petalTexture.Dispose();
		}

		public void SetPetalsActive(bool finish)
		{
			if (!finish)
			{
				petalActive = true;
				backgroundIsColor = false;
				ResetPetals();
			}
			else
			{
				petalActive = false;
			}
		}

		private void ResetPetals()
		{
			petalActiveCount = petalRandom.Next(20, 30);
			for (int i = 0; i < petalActiveCount; i++)
			{
				petalFrames[i] = petalRandom.Next(4);
				petalPositions[i] = new PointF(petalRandom.Next(-24, 800), petalRandom.Next(-24, 600));
				petalVelocitiesY[i] = (float)(petalRandom.NextDouble() * 16.0);
				petalVelocitiesX[i] = (float)(petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
				petalTargetVelocitiesY[i] = petalVelocitiesY[i];
				petalTargetVelocitiesX[i] = petalVelocitiesX[i];
				petalTurnTimers[i] = petalRandom.Next(200);
				petalLifeCounters[i] = petalRandom.Next(1, 9);
			}
			for (int j = petalActiveCount; j < 30; j++)
			{
				petalFrames[j] = 0;
				petalPositions[j] = PointF.Empty;
				petalVelocitiesY[j] = 0f;
				petalVelocitiesX[j] = 0f;
				petalTargetVelocitiesY[j] = 0f;
				petalTargetVelocitiesX[j] = 0f;
				petalTurnTimers[j] = 0;
				petalLifeCounters[j] = 0;
			}
		}

		private void DrawPetals(bool upperHalf)
		{
			if (!petalActive || !effectEnabled)
			{
				return;
			}
			int freeSlot = -1;
			for (int i = ((!upperHalf) ? 1 : 0); i < 30; i += 2)
			{
				if (petalLifeCounters[i] == 0)
				{
					freeSlot = i;
					break;
				}
			}
			if (freeSlot >= 0 && (petalActiveCount < 20 || !(petalRandom.NextDouble() >= 0.2)))
			{
				petalFrames[freeSlot] = petalRandom.Next(4);
				int randomX = petalRandom.Next(-3297, 624);
				petalVelocitiesY[freeSlot] = (float)(petalRandom.NextDouble() * 16.0);
				petalVelocitiesX[freeSlot] = (float)(petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
				petalTargetVelocitiesY[freeSlot] = petalVelocitiesY[freeSlot];
				petalTargetVelocitiesX[freeSlot] = petalVelocitiesX[freeSlot];
				if (randomX < 0)
				{
					petalPositions[freeSlot] = new PointF((0f - (float)randomX - 1f) % 824f - 24f, -24f);
				}
				else if (petalVelocitiesX[freeSlot] > 0f)
				{
					petalPositions[freeSlot] = new PointF(-24f, randomX - 24);
				}
				else
				{
					petalPositions[freeSlot] = new PointF(800f, randomX - 24);
				}
				petalTurnTimers[freeSlot] = petalRandom.Next(40, 200);
				petalLifeCounters[freeSlot] = petalRandom.Next(1, 9);
				petalActiveCount++;
			}
			for (int j = ((!upperHalf) ? 1 : 0); j < 30; j += 2)
			{
				if (petalLifeCounters[j] <= 0)
				{
					continue;
				}
				DrawPetal(petalFrames[j], petalLifeCounters[j], petalPositions[j]);
				int[] array;
				IntPtr intPtr;
				(array = petalFrames)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
				if (petalFrames[j] == 4)
				{
					petalFrames[j] = 0;
				}
				(array = petalTurnTimers)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] - 1;
				if (petalTurnTimers[j] == 0)
				{
					petalTargetVelocitiesY[j] = (float)(petalRandom.NextDouble() * 16.0);
					petalTargetVelocitiesX[j] = (float)(petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
					petalTurnTimers[j] = petalRandom.Next(40, 200);
				}
				if (petalVelocitiesX[j] != petalTargetVelocitiesX[j] || petalVelocitiesY[j] != petalTargetVelocitiesY[j])
				{
					float deltaY = petalTargetVelocitiesY[j] - petalVelocitiesY[j];
					float deltaX = petalTargetVelocitiesX[j] - petalVelocitiesX[j];
					float velocityRatio = ((deltaY == 0f) ? float.NaN : Math.Abs(deltaX / deltaY));
					if (Math.Abs(deltaX) > Math.Abs(deltaY))
					{
						if (Math.Abs(deltaX) > 0.1f)
						{
							float[] array2;
							(array2 = petalVelocitiesX)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((deltaX > 0f) ? 0.1f : (-0.1f));
							if (!float.IsNaN(velocityRatio))
							{
								(array2 = petalVelocitiesY)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((deltaY > 0f) ? 0.1f : (-0.1f)) / velocityRatio;
							}
						}
						else
						{
							petalVelocitiesX[j] = petalTargetVelocitiesX[j];
							petalVelocitiesY[j] = petalTargetVelocitiesY[j];
						}
					}
					else if (Math.Abs(deltaY) > 0.1f)
					{
						float[] array3;
						(array3 = petalVelocitiesY)[(int)(intPtr = (IntPtr)j)] = array3[(int)intPtr] + ((deltaY > 0f) ? 0.1f : (-0.1f));
						(array3 = petalVelocitiesX)[(int)(intPtr = (IntPtr)j)] = array3[(int)intPtr] + ((deltaX > 0f) ? 0.1f : (-0.1f)) * velocityRatio;
					}
					else
					{
						petalVelocitiesY[j] = petalTargetVelocitiesY[j];
						petalVelocitiesX[j] = petalTargetVelocitiesX[j];
					}
				}
				petalPositions[j] = new PointF(petalPositions[j].X + petalVelocitiesX[j], petalPositions[j].Y + petalVelocitiesY[j]);
				if (petalPositions[j].X < -24f || petalPositions[j].X > 800f || petalPositions[j].Y > 600f)
				{
					petalFrames[j] = 0;
					petalPositions[j] = PointF.Empty;
					petalVelocitiesY[j] = 0f;
					petalVelocitiesX[j] = 0f;
					petalTargetVelocitiesY[j] = 0f;
					petalTargetVelocitiesX[j] = 0f;
					petalTurnTimers[j] = 0;
					petalLifeCounters[j] = 0;
					petalActiveCount--;
				}
			}
		}

		private void DrawPetal(int frame, int index, PointF origin)
		{
			Rectangle petalRect = new Rectangle(frame * 24, (index - 1) * 24, 24, 24);
			QueueDraw(petalTexture, petalRect, Rectangle.Empty, new Point((int)origin.X, (int)origin.Y), Color.White);
		}

		private void InitSnow()
		{
			snowTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("SNOW"), snowFrameSize.Width, snowFrameSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			snowActive = false;
			for (int i = 0; i < 25; i++)
			{
				snowPositions[i] = Point.Empty;
				snowSpeeds[i] = 0f;
				snowSwayAmplitudes[i] = 0f;
				snowSwayFrequencies[i] = 0f;
				snowSwayPhases[i] = 0f;
				snowLifeCounters[i] = 0;
			}
			snowActiveCount = 0;
			snowRandom = new Random();
		}

		private void DisposeSnow()
		{
			snowTexture.Dispose();
		}

		public void SetSnowActive(bool finish)
		{
			if (!finish)
			{
				snowActive = true;
				backgroundIsColor = false;
				ResetSnow();
			}
			else
			{
				snowActive = false;
			}
		}

		private void ResetSnow()
		{
			snowActiveCount = snowRandom.Next(15, 25);
			for (int i = 0; i < snowActiveCount; i++)
			{
				snowPositions[i] = new PointF(snowRandom.Next(-16, 800), snowRandom.Next(-16, 600));
				snowSpeeds[i] = (float)(snowRandom.NextDouble() * 4.0 + 2.0);
				snowSwayAmplitudes[i] = (float)(snowRandom.NextDouble() * 16.0 + 8.0);
				snowSwayFrequencies[i] = (float)(snowRandom.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
				snowSwayPhases[i] = (float)(snowRandom.NextDouble() * (Math.PI * 2.0));
				snowLifeCounters[i] = snowRandom.Next(1, 5);
			}
			for (int j = snowActiveCount; j < 25; j++)
			{
				snowPositions[j] = PointF.Empty;
				snowSpeeds[j] = 0f;
				snowSwayAmplitudes[j] = 0f;
				snowSwayFrequencies[j] = 0f;
				snowSwayPhases[j] = 0f;
				snowLifeCounters[j] = 0;
			}
		}

		private void DrawSnow()
		{
			if (!snowActive || !effectEnabled)
			{
				return;
			}
			int freeSlot = -1;
			for (int i = 0; i < 25; i++)
			{
				if (snowLifeCounters[i] == 0)
				{
					freeSlot = i;
					break;
				}
			}
			if (freeSlot >= 0 && (snowActiveCount < 15 || snowRandom.NextDouble() < 0.2))
			{
				snowPositions[freeSlot] = new PointF(snowRandom.Next(-16, 800), -16f);
				snowSpeeds[freeSlot] = (float)(snowRandom.NextDouble() * 4.0 + 2.0);
				snowSwayAmplitudes[freeSlot] = (float)(snowRandom.NextDouble() * 16.0 + 8.0);
				snowSwayFrequencies[freeSlot] = (float)(snowRandom.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
				snowSwayPhases[freeSlot] = (float)(snowRandom.NextDouble() * (Math.PI * 2.0));
				snowLifeCounters[freeSlot] = snowRandom.Next(1, 5);
				snowActiveCount++;
			}
			for (int j = 0; j < 25; j++)
			{
				if (snowLifeCounters[j] > 0)
				{
					DrawSnowflake(snowLifeCounters[j], new PointF((float)((double)snowPositions[j].X + (double)snowSwayAmplitudes[j] * Math.Sin(snowSwayFrequencies[j] * snowPositions[j].Y + snowSwayPhases[j])), snowPositions[j].Y));
					snowPositions[j] = new PointF(snowPositions[j].X, snowPositions[j].Y + snowSpeeds[j]);
					if (snowPositions[j].Y > 600f)
					{
						snowPositions[j] = PointF.Empty;
						snowSpeeds[j] = 0f;
						snowSwayAmplitudes[j] = 0f;
						snowSwayFrequencies[j] = 0f;
						snowSwayPhases[j] = 0f;
						snowLifeCounters[j] = 0;
						snowActiveCount--;
					}
				}
			}
		}

		private void DrawSnowflake(int index, PointF origin)
		{
			Rectangle snowRect = new Rectangle((index - 1) * 16, 0, 16, 16);
			QueueDraw(snowTexture, snowRect, Rectangle.Empty, new Point((int)origin.X, (int)origin.Y), Color.White);
		}

		private void InitDimOverlay()
		{
			dimActive = false;
			dimReverse = false;
			dimCounter = 0;
		}

		public void SetDimActive(bool animate, bool finish)
		{
			if (!finish)
			{
				dimActive = true;
				dimReverse = false;
				backgroundIsColor = false;
				if (animate)
				{
					dimCounter = 10;
				}
				else
				{
					dimCounter = 0;
				}
			}
			else
			{
				dimReverse = true;
				if (animate)
				{
					dimCounter = 10;
					return;
				}
				dimCounter = 0;
				dimActive = false;
			}
		}

		private void DrawDimOverlay()
		{
			if (!dimActive)
			{
				return;
			}
			if (dimCounter > 0)
			{
				if (fadeEnabled)
				{
					float fadeRatio = (dimReverse ? ((float)dimCounter / 10f) : ((float)(10 - dimCounter) / 10f));
					QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(128f * fadeRatio), Color.Black));
					dimCounter--;
				}
				else
				{
					QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
					dimCounter = 0;
				}
				if (dimReverse && dimCounter == 0)
				{
					dimActive = false;
				}
			}
			else
			{
				QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
			}
		}

		private void InitThinkOverlay()
		{
			thinkTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("THINK"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			thinkActive = false;
			thinkReverse = false;
			thinkCounter = 0;
		}

		private void DisposeThinkOverlay()
		{
			thinkTexture.Dispose();
		}

		public void SetThinkActive(bool animate, bool finish)
		{
			if (!finish)
			{
				thinkActive = true;
				thinkReverse = false;
				backgroundIsColor = false;
				if (animate)
				{
					thinkCounter = 10;
				}
				else
				{
					thinkCounter = 0;
				}
				RebuildDesaturatedBackground();
				RebuildCharacterShadow();
			}
			else
			{
				thinkReverse = true;
				if (animate)
				{
					thinkCounter = 10;
					return;
				}
				thinkCounter = 0;
				thinkActive = false;
			}
		}

		private void DrawThinkOverlay()
		{
			if (!thinkActive)
			{
				return;
			}
			if (thinkCounter > 0)
			{
				if (fadeEnabled)
				{
					float fadeRatio = (thinkReverse ? ((float)thinkCounter / 10f) : ((float)(10 - thinkCounter) / 10f));
					QueueDraw(thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * fadeRatio), Color.White));
					thinkCounter--;
				}
				else
				{
					QueueDraw(thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
					thinkCounter = 0;
				}
				if (thinkReverse && thinkCounter == 0)
				{
					thinkActive = false;
				}
			}
			else
			{
				QueueDraw(thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
		}

		private void DrawBackgroundEffects()
		{
			DrawZoomTransition();
			DrawShake();
			DrawSunEffect();
			DrawPetals(false);
		}

		private void DrawForegroundEffects()
		{
			DrawPetals(true);
			DrawSnow();
			DrawDimOverlay();
			DrawThinkOverlay();
		}

		private void InitCharacters()
		{
			characterShakeTableX = new int[3][];
			characterShakeTableX[0] = new int[4] { 4, -4, 4, -4 };
			characterShakeTableX[1] = new int[6] { 8, 0, -8, 8, 0, -8 };
			characterShakeTableX[2] = new int[16]
			{
				16, 0, -16, 0, 16, 0, -16, 0, 8, -8,
				8, -8, 4, -4, 4, -4
			};
			characterShakeTableY = new int[3][];
			characterShakeTableY[0] = new int[4] { 0, 0, 4, 4 };
			characterShakeTableY[1] = new int[6] { 0, 0, 4, 4, 8, 8 };
			characterShakeTableY[2] = new int[16]
			{
				0, 0, 8, 8, 16, 16, 8, 8, 0, 0,
				8, 8, 0, 0, 4, 4
			};
			for (int i = 0; i < 3; i++)
			{
				characterTextures[i] = null;
				previousCharacterTextures[i] = null;
				characterShadowTextures[i] = null;
				characterAppearCounters[i] = 0;
				characterShakeCounters[i] = 0;
				characterOffsets[i] = 0;
				previousCharacterOffsets[i] = 0;
				characterSizes[i] = Size.Empty;
				previousCharacterSizes[i] = Size.Empty;
			}
		}

		private void DisposeCharacters()
		{
			for (int i = 0; i < 3; i++)
			{
				if (characterTextures[i] != null)
				{
					characterTextures[i].Dispose();
				}
				if (previousCharacterTextures[i] != null)
				{
					previousCharacterTextures[i].Dispose();
				}
				if (characterShadowTextures[i] != null)
				{
					characterShadowTextures[i].Dispose();
				}
			}
		}

		public void SetCharacter(Texture content, Size size, int index, int location, bool isTransparent, bool animate)
		{
			if (previousCharacterTextures[index] != null)
			{
				previousCharacterTextures[index].Dispose();
			}
			previousCharacterTextures[index] = characterTextures[index];
			previousCharacterSizes[index] = characterSizes[index];
			characterTextures[index] = content;
			characterSizes[index] = size;
			previousCharacterOffsets[index] = characterOffsets[index];
			if (location != int.MaxValue)
			{
				characterOffsets[index] = location;
			}
			if (content != null)
			{
				characterSemiTransparent[index] = isTransparent;
			}
			if (animate)
			{
				characterAppearCounters[index] = 8;
			}
			if (thinkActive)
			{
				RebuildCharacterShadow();
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				if (characterShadowTextures[i] != null)
				{
					characterShadowTextures[i].Dispose();
					characterShadowTextures[i] = null;
				}
			}
		}

		public void SetCharacterShake(int index, int level)
		{
			characterShakePatterns[index] = level;
			characterShakeCounters[index] = characterShakePatternLengths[level];
		}

		private void DrawCharacters()
		{
			for (int i = 0; i < 3; i++)
			{
				if (characterAppearCounters[i] > 0)
				{
					if (fadeEnabled)
					{
						if (previousCharacterTextures[i] != null && characterTextures[i] != null)
						{
							if (characterAppearCounters[i] > 2)
							{
								int alpha = (int)(Math.Pow((double)(8 - characterAppearCounters[i]) / 6.0, 2.0) * 255.0 / (double)((!characterSemiTransparent[i]) ? 1 : 2));
								QueueDraw(previousCharacterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + previousCharacterOffsets[i] * 15 - previousCharacterSizes[i].Width / 2, 600 - previousCharacterSizes[i].Height), characterSemiTransparent[i] ? Color.FromArgb(128, Color.White) : Color.White);
								QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.FromArgb(alpha, Color.White));
							}
							else
							{
								int alpha2 = (int)(Math.Pow((double)characterAppearCounters[i] / 2.0, 2.0) * 255.0 / (double)((!characterSemiTransparent[i]) ? 1 : 2));
								QueueDraw(previousCharacterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - previousCharacterSizes[i].Width / 2, 600 - previousCharacterSizes[i].Height), Color.FromArgb(alpha2, Color.White));
								QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), characterSemiTransparent[i] ? Color.FromArgb(128, Color.White) : Color.White);
							}
						}
						else if (previousCharacterTextures[i] != null)
						{
							QueueDraw(previousCharacterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + previousCharacterOffsets[i] * 15 - previousCharacterSizes[i].Width / 2, 600 - previousCharacterSizes[i].Height), Color.FromArgb((int)((float)characterAppearCounters[i] / 8f * 255f / (float)((!characterSemiTransparent[i]) ? 1 : 2)), Color.White));
						}
						else
						{
							if (!(characterTextures[i] != null))
							{
								throw new Exception("No Chara");
							}
							QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.FromArgb((int)((float)(8 - characterAppearCounters[i]) / 8f * 255f / (float)((!characterSemiTransparent[i]) ? 1 : 2)), Color.White));
						}
						int[] array;
						IntPtr intPtr;
						(array = characterAppearCounters)[(int)(intPtr = (IntPtr)i)] = array[(int)intPtr] - 1;
					}
					else
					{
						if (characterTextures[i] != null)
						{
							QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.White);
						}
						characterAppearCounters[i] = 0;
					}
				}
				else
				{
					if (!(characterTextures[i] != null))
					{
						continue;
					}
					if (characterShakeCounters[i] > 0)
					{
						if (fadeEnabled && characterSemiTransparent[i])
						{
							QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2 + characterShakeTableX[characterShakePatterns[i]][characterShakePatternLengths[characterShakePatterns[i]] - characterShakeCounters[i]], 600 - characterSizes[i].Height + characterShakeTableY[characterShakePatterns[i]][characterShakePatternLengths[characterShakePatterns[i]] - characterShakeCounters[i]]), semiTransparentWhite);
						}
						else
						{
							QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2 + characterShakeTableX[characterShakePatterns[i]][characterShakePatternLengths[characterShakePatterns[i]] - characterShakeCounters[i]], 600 - characterSizes[i].Height + characterShakeTableY[characterShakePatterns[i]][characterShakePatternLengths[characterShakePatterns[i]] - characterShakeCounters[i]]), Color.White);
						}
						int[] array2;
						IntPtr intPtr2;
						(array2 = characterShakeCounters)[(int)(intPtr2 = (IntPtr)i)] = array2[(int)intPtr2] - 1;
					}
					else if (fadeEnabled && characterSemiTransparent[i])
					{
						QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), semiTransparentWhite);
					}
					else if (thinkActive)
					{
						if (thinkCounter > 0)
						{
							float fadeRatio = (thinkReverse ? ((float)thinkCounter / 10f) : ((float)(10 - thinkCounter) / 10f));
							QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.White);
							QueueDraw(characterShadowTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.FromArgb((int)(255f * fadeRatio), Color.White));
						}
						else
						{
							QueueDraw(characterShadowTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.White);
						}
					}
					else
					{
						QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.White);
					}
				}
			}
		}

		private void RebuildCharacterShadow()
		{
			for (int i = 0; i < 3; i++)
			{
				if (characterShadowTextures[i] != null)
				{
					characterShadowTextures[i].Dispose();
				}
				if (!(characterTextures[i] != null))
				{
					continue;
				}
				characterShadowTextures[i] = TextureLoader.FromStream(device, systemArchive.ReadEntry("WHITE"), characterSizes[i].Width, characterSizes[i].Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
				uint[,] array = (uint[,])characterTextures[i].LockRectangle(typeof(uint), 0, LockFlags.None, characterSizes[i].Width, characterSizes[i].Height);
				uint[,] array2 = (uint[,])characterShadowTextures[i].LockRectangle(typeof(uint), 0, LockFlags.None, characterSizes[i].Width, characterSizes[i].Height);
				for (int j = 0; j < characterSizes[i].Width; j++)
				{
					for (int k = 0; k < characterSizes[i].Height; k++)
					{
						uint pixel = array[j, k];
						uint gray = ((pixel & 0xFF) + ((pixel >> 8) & 0xFF) + ((pixel >> 16) & 0xFF)) / 3;
						uint sourceAlpha = (pixel >> 24) & 0xFF;
						array2[j, k] = (sourceAlpha << 24) + (gray << 16) + (gray << 8) + gray;
					}
				}
				characterTextures[i].UnlockRectangle(0);
				characterShadowTextures[i].UnlockRectangle(0);
			}
		}

		private void InitNotice()
		{
			noticeLines = null;
			noticeTimer = 0;
			RebuildNoticeFont();
		}

		private void DisposeNoticeFont()
		{
			if (noticeFont != null)
			{
				noticeFont.Dispose();
			}
		}

		private void RebuildNoticeFont()
		{
			if (noticeFont != null)
			{
				noticeFont.Dispose();
			}
			noticeFont = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font(baseFont.FontFamily, 24f));
		}

		public void ShowNotice(string[] content)
		{
			noticeLines = content;
			noticeTimer = 70;
		}

		private void DrawNotice()
		{
			if (noticeLines != null)
			{
				if (noticeTimer == 0)
				{
					noticeLines = null;
					return;
				}
				int alpha;
				if (fadeEnabled)
				{
					if (noticeTimer <= 10)
					{
						alpha = (int)((float)noticeTimer / 10f * 255f);
					}
					else
					{
						alpha = ((noticeTimer > 60) ? ((int)((float)(70 - noticeTimer) / 10f * 255f)) : 255);
					}
				}
				else
				{
					alpha = 255;
				}
				DrawTextCore(noticeLines[(int)layoutIndex], 0, noticeLines[(int)layoutIndex].Length, noticePosition, noticeCharSize.Width, noticeCharSize.Height, 2, HorizontalAlign.Right, 15, noticeFont, Color.FromArgb(alpha, Color.White), Color.FromArgb(alpha, Color.Black));
				fontIndexQueue.Add(1);
				noticeTimer--;
			}
		}

		private void InitMessage()
		{
			messageLines = null;
			messageFontSizeMode = HorizontalAlign.Left;
			messageAlignMode = TextRevealMode.Fade;
			messageProgress = 0.0;
			lineRevealCounter = 0;
			messageFadeCounter = 0;
			messageVisibleChars = 0;
			showNextIndicator = false;
			messageTextColor = Color.White;
			messageShadowColor = Color.Black;
			messageVoiceLength = 0.0;
			lastTextPosition = messagePosition;
			for (int i = 0; i < 3; i++)
			{
				messageStartIndexes[i] = 0;
			}
			RebuildMessageFont();
		}

		private void DisposeMessageFont()
		{
			if (messageFont != null)
			{
				messageFont.Dispose();
			}
		}

		private void RebuildMessageFont()
		{
			if (messageFont != null)
			{
				messageFont.Dispose();
			}
			messageFont = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font(baseFont.FontFamily, 24f));
			if (messageLines == null || messageStartIndexes[(int)layoutIndex] + messageVisibleChars <= 0 || messageVisibleChars <= 0)
			{
				return;
			}
			bool created = false;
			for (int i = 0; i < 3; i++)
			{
				if (messageStartIndexes[i] + messageVisibleChars >= messageLines[i].Length)
				{
					created = true;
					break;
				}
			}
			if (created)
			{
				messageVisibleChars = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex];
				if (messageAlignMode == TextRevealMode.Typewriter)
				{
					messageProgress = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4;
				}
			}
		}

		public void SetMessage(string[] content, HorizontalAlign align, TextRevealMode showMode, bool waitForInput, Color foreColor, Color shadowColor, double length)
		{
			if (content == null)
			{
				return;
			}
			messageFontSizeMode = align;
			messageAlignMode = showMode;
			showNextIndicator = waitForInput;
			messageTextColor = foreColor;
			messageShadowColor = shadowColor;
			messageVoiceLength = length;
			if (messageLines == null)
			{
				messageLines = new string[3];
				for (int i = 0; i < 3; i++)
				{
					messageLines[i] = "";
				}
			}
			for (int j = 0; j < 3; j++)
			{
				messageStartIndexes[j] = messageLines[j].Length;
				string[] array;
				IntPtr intPtr;
				(array = messageLines)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + content[j];
			}
			messageVisibleChars = 0;
			switch (messageAlignMode)
			{
			case TextRevealMode.Typewriter:
				messageProgress = 0.0;
				break;
			case TextRevealMode.Fade:
				lineRevealCounter = 10;
				break;
			}
		}

		public void DismissMessage(bool animate)
		{
			if (animate)
			{
				messageFadeCounter = 10;
				return;
			}
			messageFadeCounter = 0;
			messageProgress = 0.0;
			lineRevealCounter = 0;
			messageLines = null;
			for (int i = 0; i < 3; i++)
			{
				messageStartIndexes[i] = 0;
			}
			messageVisibleChars = 0;
		}

		private void DrawMessage()
		{
			if (messageLines == null)
			{
				return;
			}
			if (messageFadeCounter > 0)
			{
				if (fadeEnabled)
				{
					int alpha = (int)((float)messageFadeCounter / 10f * 255f);
					DrawTextCore(messageLines[(int)layoutIndex], 0, messageLines[(int)layoutIndex].Length, messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, Color.FromArgb(alpha, messageTextColor), (messageShadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha, messageShadowColor));
					fontIndexQueue.Add(2);
					messageFadeCounter--;
				}
				else
				{
					messageFadeCounter = 0;
				}
				if (messageFadeCounter == 0)
				{
					messageProgress = 0.0;
					lineRevealCounter = 0;
					messageLines = null;
					for (int i = 0; i < 3; i++)
					{
						messageStartIndexes[i] = 0;
					}
					messageVisibleChars = 0;
				}
				return;
			}
			switch (messageAlignMode)
			{
			case TextRevealMode.Typewriter:
				if (messageProgress < (double)(messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4))
				{
					if (messageStartIndexes[(int)layoutIndex] > 0)
					{
						lastTextPosition = DrawTextCore(messageLines[(int)layoutIndex], 0, messageStartIndexes[(int)layoutIndex], messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, messageTextColor, messageShadowColor);
						fontIndexQueue.Add(2);
					}
					if (messageVoiceLength > 0.0 && syncVoice)
					{
						try
						{
							double voiceProgress = audioManager.GetVoicePosition() / messageVoiceLength;
							messageProgress = (double)(messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4) * voiceProgress;
						}
						catch (NullReferenceException)
						{
							messageProgress = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4;
						}
					}
					else
					{
						messageProgress += (double)messageSpeed / 6.0;
					}
					lastTextPosition = DrawTextCore(messageLines[(int)layoutIndex], messageStartIndexes[(int)layoutIndex], (double)messageStartIndexes[(int)layoutIndex] + messageProgress - 4.0, messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, true, 4, messageTextColor, messageShadowColor);
					fontIndexQueue.Add(2);
					messageVisibleChars = (int)(messageProgress - 4.0);
					if (messageVisibleChars < 0)
					{
						messageVisibleChars = 0;
					}
				}
				else
				{
					lastTextPosition = DrawTextCore(messageLines[(int)layoutIndex], messageLines[(int)layoutIndex].Length, messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, 4, messageTextColor, messageShadowColor);
					fontIndexQueue.Add(2);
				}
				break;
			case TextRevealMode.Fade:
				if (messageStartIndexes[(int)layoutIndex] + messageVisibleChars > 0)
				{
					lastTextPosition = DrawTextCore(messageLines[(int)layoutIndex], 0, messageStartIndexes[(int)layoutIndex] + messageVisibleChars, messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, messageTextColor, messageShadowColor);
					fontIndexQueue.Add(2);
				}
				if (lineRevealCounter > 0)
				{
					if (fadeEnabled)
					{
						int alpha2 = (int)((float)(10 - lineRevealCounter) / 10f * 255f);
						lastTextPosition = DrawTextCore(messageLines[(int)layoutIndex], messageStartIndexes[(int)layoutIndex], messageLines[(int)layoutIndex].Length, messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, Color.FromArgb(alpha2, messageTextColor), (messageShadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha2, messageShadowColor));
						fontIndexQueue.Add(2);
						lineRevealCounter--;
					}
					else
					{
						lastTextPosition = DrawTextCore(messageLines[(int)layoutIndex], messageStartIndexes[(int)layoutIndex], messageLines[(int)layoutIndex].Length, messagePosition, messageCharSize.Width, messageCharSize.Height, 2, messageFontSizeMode, 25, messageFont, messageTextColor, messageShadowColor);
						fontIndexQueue.Add(2);
						lineRevealCounter = 0;
					}
					if (lineRevealCounter == 0)
					{
						messageVisibleChars = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex];
					}
				}
				break;
			}
			if (!showNextIndicator || ((messageAlignMode != TextRevealMode.Typewriter || !syncVoice) && audioManager.IsVoiceBlocking()))
			{
				return;
			}
			switch (messageAlignMode)
			{
			case TextRevealMode.Typewriter:
				if (messageProgress >= (double)messageLines[(int)layoutIndex].Length - (double)messageStartIndexes[(int)layoutIndex] + 4.0)
				{
					DrawNextIndicator(lastTextPosition.X, lastTextPosition.Y);
				}
				break;
			case TextRevealMode.Fade:
				if (lineRevealCounter == 0)
				{
					DrawNextIndicator(lastTextPosition.X, lastTextPosition.Y);
				}
				break;
			}
		}

		private void InitDialogueFrame()
		{
			dialogueTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("DIALOGUE"), dialogueRect.Width, dialogueRect.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			characterPanelTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("CHARA"), characterPanelSize.Width, characterPanelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			dialogueCounter = 0;
			dialogueClosing = false;
			dialogueActive = false;
			characterPanelVisible = false;
		}

		private void DisposeDialogueFrame()
		{
			dialogueTexture.Dispose();
			characterPanelTexture.Dispose();
		}

		protected bool DrawDialogueFrame(bool charaNames)
		{
			if (charaNames)
			{
				characterPanelVisible = true;
				QueueDraw(characterPanelTexture, Rectangle.Empty, Rectangle.Empty, characterPanelPosition, dialogueColor);
				return true;
			}
			if (!dialogueActive)
			{
				dialogueActive = true;
				dialogueCounter = ((!skipCaptionAnimation) ? 3 : 0);
				dialogueClosing = false;
			}
			if (skipCaptionAnimation)
			{
				skipCaptionAnimation = false;
			}
			if (dialogueCounter > 0)
			{
				if (fadeEnabled)
				{
					if (!dialogueClosing && dialogueCounter < 3)
					{
						float closeRatio = (float)(3 - dialogueCounter) / 3f;
						float closeX = ((float)dialogueRect.X + (1f - closeRatio) / 2f * (float)dialogueRect.Width) / closeRatio;
						float closeY = ((float)dialogueRect.Y + (1f - closeRatio) / 2f * (float)dialogueRect.Height) / closeRatio;
						float closeWidth = closeRatio * (float)dialogueRect.Width;
						float closeHeight = closeRatio * (float)dialogueRect.Height;
						QueueDraw(dialogueTexture, new Rectangle(0, 0, dialogueRect.Width, dialogueRect.Height), new Rectangle(0, 0, (int)closeWidth, (int)closeHeight), new Point((int)closeX, (int)closeY), dialogueColor);
						QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					else if (dialogueClosing && dialogueCounter > 0)
					{
						float openRatio = (float)dialogueCounter / 3f;
						float openX = ((float)dialogueRect.X + (1f - openRatio) / 2f * (float)dialogueRect.Width) / openRatio;
						float openY = ((float)dialogueRect.Y + (1f - openRatio) / 2f * (float)dialogueRect.Height) / openRatio;
						float openWidth = openRatio * (float)dialogueRect.Width;
						float openHeight = openRatio * (float)dialogueRect.Height;
						QueueDraw(dialogueTexture, new Rectangle(0, 0, dialogueRect.Width, dialogueRect.Height), new Rectangle(0, 0, (int)openWidth, (int)openHeight), new Point((int)openX, (int)openY), dialogueColor);
						QueueDraw(whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					dialogueCounter--;
				}
				else
				{
					QueueDraw(dialogueTexture, Rectangle.Empty, Rectangle.Empty, dialogueRect.Location, dialogueColor);
					dialogueCounter = 0;
				}
				if (dialogueClosing && dialogueCounter == 0)
				{
					dialogueActive = false;
				}
				return false;
			}
			QueueDraw(dialogueTexture, Rectangle.Empty, Rectangle.Empty, dialogueRect.Location, dialogueColor);
			return true;
		}

		private void InitCaption()
		{
			captionLines = null;
			captionOptionLines = null;
			captionProgress = 0.0;
			captionVisibleChars = 0;
			captionVoiceLength = 0.0;
			captionLocked = false;
			captionLastPosition = captionPosition;
			for (int i = 0; i < 3; i++)
			{
				captionStartIndexes[i] = 0;
			}
			RebuildCaptionFont();
		}

		private void DisposeCaptionFont()
		{
			if (captionFont != null)
			{
				captionFont.Dispose();
			}
		}

		private void RebuildCaptionFont()
		{
			if (captionFont != null)
			{
				captionFont.Dispose();
			}
			captionFont = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font(baseFont.FontFamily, 24f));
			if (captionLines == null || captionStartIndexes[(int)layoutIndex] + captionVisibleChars <= 0 || captionVisibleChars <= 0)
			{
				return;
			}
			bool created = false;
			for (int i = 0; i < 3; i++)
			{
				if (captionStartIndexes[i] + captionVisibleChars >= captionLines[i].Length)
				{
					created = true;
					break;
				}
			}
			if (created)
			{
				captionVisibleChars = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex];
				captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
			}
		}

		public void SetCaption(string[] content, string[] charaNames, double length, bool isContinuous, bool instant)
		{
			if (content == null)
			{
				return;
			}
			captionVoiceLength = length;
			captionOptionLines = charaNames;
			captionLocked = isContinuous;
			skipCaptionAnimation = instant;
			if (captionLines == null)
			{
				captionLines = new string[3];
				for (int i = 0; i < 3; i++)
				{
					captionLines[i] = "";
				}
			}
			for (int j = 0; j < 3; j++)
			{
				captionStartIndexes[j] = captionLines[j].Length;
				string[] array;
				IntPtr intPtr;
				(array = captionLines)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + content[j];
			}
			captionVisibleChars = 0;
			captionProgress = 0.0;
		}

		public void CloseDialogue(bool shadowColor, bool hideCharacterPanel, bool instant)
		{
			if (captionLocked)
			{
				return;
			}
			captionProgress = 0.0;
			captionLines = null;
			captionOptionLines = null;
			skipCaptionAnimation = instant && shadowColor;
			for (int i = 0; i < 3; i++)
			{
				captionStartIndexes[i] = 0;
			}
			captionVisibleChars = 0;
			if (shadowColor)
			{
				dialogueCounter = ((!skipCaptionAnimation) ? 3 : 0);
				if (skipCaptionAnimation)
				{
					dialogueActive = false;
				}
				dialogueClosing = true;
			}
			if (hideCharacterPanel)
			{
				characterPanelVisible = false;
			}
		}

		private void DrawCaption()
		{
			if (captionLines == null)
			{
				if (dialogueActive)
				{
					DrawDialogueFrame(false);
				}
				if (characterPanelVisible)
				{
					DrawDialogueFrame(true);
				}
			}
			else
			{
				if (!DrawDialogueFrame(false))
				{
					return;
				}
				if (captionProgress < (double)(captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4))
				{
					if (captionVoiceLength > 0.0 && syncVoice)
					{
						try
						{
							double voiceProgress = audioManager.GetVoicePosition() / captionVoiceLength;
							captionProgress = (double)(captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4) * voiceProgress;
						}
						catch (NullReferenceException)
						{
							captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
						}
					}
					else
					{
						captionProgress += (double)messageSpeed / 6.0;
					}
					captionLastPosition = DrawTextCore(captionLines[(int)layoutIndex], (double)captionStartIndexes[(int)layoutIndex] + captionProgress - 4.0, captionPosition, captionCharSize.Width, captionCharSize.Height, 2, captionFont, 4, Color.White, Color.Black);
					fontIndexQueue.Add(3);
					captionVisibleChars = (int)(captionProgress - 4.0);
					if (captionVisibleChars < 0)
					{
						captionVisibleChars = 0;
					}
				}
				else
				{
					captionLastPosition = DrawTextCore(captionLines[(int)layoutIndex], captionLines[(int)layoutIndex].Length, captionPosition, captionCharSize.Width, captionCharSize.Height, 2, captionFont, 4, Color.White, Color.Black);
					fontIndexQueue.Add(3);
				}
				if ((syncVoice || !audioManager.IsVoiceBlocking()) && captionProgress >= (double)captionLines[(int)layoutIndex].Length - (double)captionStartIndexes[(int)layoutIndex] + 4.0)
				{
					DrawNextIndicator(captionLastPosition.X, captionLastPosition.Y);
				}
				if (captionOptionLines != null && DrawDialogueFrame(true))
				{
					DrawTextCore(captionOptionLines[(int)layoutIndex], new Point(captionOptionPosition.X - captionCharSize.Width * captionOptionLines[(int)layoutIndex].Length / 2, captionOptionPosition.Y), captionCharSize.Width, captionCharSize.Height, 2, captionFont, Color.White, Color.Black);
					fontIndexQueue.Add(3);
				}
			}
		}

		public void SetMessageSpeed(int content)
		{
			modeIconIndex = content;
		}

		private void InitModeIcon()
		{
			modeIconTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("ICON"), modeIconTextureSize.Width, modeIconTextureSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			modeIconFrame = 0;
		}

		private void DisposeModeIcon()
		{
			modeIconTexture.Dispose();
		}

		private void DrawModeIcon()
		{
			if (modeIconIndex > 0)
			{
				Rectangle modeIconRect = new Rectangle(modeIconFrame / 3 * 32, modeIconIndex * 32, 32, 32);
				QueueDraw(modeIconTexture, modeIconRect, Rectangle.Empty, modeIconPosition, Color.White);
				modeIconFrame++;
				if (modeIconFrame == 9)
				{
					modeIconFrame = 0;
				}
			}
		}
	}
}
