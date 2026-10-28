using System;
using System.Collections;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Yukihota
{
	public class ScreenRenderer : IDisposable
	{
		protected const int n0779 = 16;

		protected const int n0780 = 4;

		protected const int n0781 = 32;

		protected const int n0782 = 32;

		private const int n0783 = 3;

		protected const int n0784 = 2;

		protected const int n0785 = 8;

		protected const int shockFrames = 16;

		protected const int n0786 = 16;

		protected const int n0787 = 128;

		protected const int n0788 = 5;

		protected const int n0789 = 20;

		protected const int n0790 = 80;

		protected const double n0791 = 0.5;

		protected const double n0792 = 0.25;

		protected const int n0793 = 450;

		protected const int n0794 = 75;

		protected const int n0795 = 18;

		protected const int n0796 = 80;

		protected const int n0797 = 30;

		protected const int n0798 = 20;

		protected const float n0799 = 16f;

		protected const float n0800 = 8f;

		protected const float n0801 = 0.1f;

		protected const int n0802 = 40;

		protected const int n0803 = 200;

		protected const double n0804 = 0.2;

		protected const int n0805 = 24;

		protected const int n0806 = 4;

		protected const int n0807 = 4;

		protected const int n0808 = 8;

		protected const int n0809 = 25;

		protected const int n0810 = 15;

		protected const float n0811 = 6f;

		protected const float n0812 = 2f;

		protected const float n0813 = 24f;

		protected const float n0814 = 8f;

		protected const float n0815 = 0.02f;

		protected const float n0816 = 0.01f;

		protected const double n0817 = Math.PI * 2.0;

		protected const double n0818 = 0.2;

		protected const int n0819 = 16;

		protected const int n0820 = 4;

		protected const int n0821 = 10;

		protected const int n0822 = 128;

		protected const int n0823 = 10;

		protected const int n0824 = 8;

		protected const int n0825 = 6;

		protected const int n0826 = 3;

		protected const int n0827 = 15;

		protected const int n0828 = 3;

		protected const int n0829 = 10;

		protected const int n0830 = 50;

		protected const int n0831 = 15;

		protected const int n0832 = 2;

		protected const int n0833 = 24;

		protected const int n0834 = 6;

		protected const int n0835 = 4;

		protected const int n0836 = 10;

		protected const int n0837 = 10;

		protected const int n0838 = 2;

		protected const int n0839 = 24;

		protected const int n0840 = 25;

		protected const int n0841 = 13;

		protected const int n0842 = 3;

		protected const int n0843 = 6;

		protected const int n0844 = 4;

		protected const int n0845 = 2;

		protected const int n0846 = 24;

		protected const int n0847 = 25;

		protected const int n0848 = 3;

		protected const int n0849 = 5;

		protected const int n0850 = 9;

		protected const int n0851 = 3;

		protected const int n0852 = 32;

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

		protected Texture n0135;

		protected ArrayList n0216 = new ArrayList();

		protected ArrayList n0217 = new ArrayList();

		protected ArrayList n0218 = new ArrayList();

		protected ArrayList n0219 = new ArrayList();

		protected ArrayList n0220 = new ArrayList();

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

		private string[] n0747;

		private int n0748;

		protected readonly Point noticePosition = new Point(300, 20);

		protected readonly Size noticeCharSize = new Size(30, 40);

		private Microsoft.DirectX.Direct3D.Font n0593;

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

		public ScreenRenderer(Device n0349, Sprite n0350, FrameCallback n0225, Texture n0351, System.Drawing.Font n0353, ButtonLayout n0046, Color n0266, AudioManager n0557)
		{
			device = n0349;
			sprite = n0350;
			resetCallback = n0225;
			n0135 = n0351;
			dialogueColor = n0266;
			baseFont = n0353;
			layoutIndex = n0046;
			audioManager = n0557;
			dialogueHidden = false;
			modeIconIndex = 0;
			systemArchive = new ArchiveFile("System", ArchiveMode.Read);
			n0561();
			n0562();
			n0563();
			n0564();
			n0565();
			n0566();
			n0567();
			n0568();
			n0569();
		}

		~ScreenRenderer()
		{
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
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

		public void n0463()
		{
			device.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
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
			n0584();
			n0585();
			n0586();
			n0587();
			n0588();
			if (!dialogueHidden)
			{
				n0589();
			}
			n0590();
			n0591();
			n0592();
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

		public void n0215()
		{
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			int num = 0;
			for (int i = 0; i < n0216.Count; i++)
			{
				if (n0216[i] != null)
				{
					sprite.Draw2D((Texture)n0216[i], (Rectangle)n0217[i], (Rectangle)n0218[i], Point.Empty, 0f, (Point)n0219[i], (Color)n0220[i]);
					continue;
				}
				Microsoft.DirectX.Direct3D.Font n880;
				switch ((int)fontIndexQueue[num])
				{
				case 1:
					n880 = n0593;
					break;
				case 2:
					n880 = messageFont;
					break;
				case 3:
					n880 = captionFont;
					break;
				default:
					n880 = n0593;
					break;
				}
				DrawTextCore((string)textQueue[num], (int)startIndexQueue[num], (double)endIndexQueue[num], (Point)textPositionQueue[num], (int)charWidthQueue[num], (int)lineHeightQueue[num], (int)shadowOffsetQueue[num], (HorizontalAlign)fontSizeModeQueue[num], (int)lineWidthQueue[num], n880, (bool)fadeOutQueue[num], (int)fadeLengthQueue[num], (Color)textColorQueue[num], (Color)shadowColorQueue[num], false);
				num++;
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

		public void n0197(bool n0241)
		{
			dialogueHidden = n0241;
			if (n0241 && captionLines != null)
			{
				captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
				captionVisibleChars = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex];
			}
		}

		public void n0197()
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

		public void SetButtonLayout(ButtonLayout n0046)
		{
			if (layoutIndex != n0046)
			{
				layoutIndex = n0046;
				n0625();
				n0626();
				n0627();
			}
		}

		public void SetFont(System.Drawing.Font n0353)
		{
			if (baseFont != n0353)
			{
				baseFont = n0353;
				n0625();
				n0626();
				n0627();
			}
		}

		private void QueueDraw(Texture n0262, Rectangle n0263, Rectangle n0264, Point n0265, Color n0266)
		{
			sprite.Draw2D(n0262, n0263, n0264, Point.Empty, 0f, n0265, n0266);
			n0216.Add(n0262);
			n0217.Add(n0263);
			n0218.Add(n0264);
			n0219.Add(n0265);
			n0220.Add(n0266);
		}

		public void Dispose()
		{
			n0628();
			n0629();
			n0630();
			n0631();
			n0632();
			n0633();
			n0634();
			n0635();
			GC.SuppressFinalize(this);
		}

		public void n0249(bool extensionName)
		{
			if (fadeEnabled != extensionName)
			{
				n0625();
				n0626();
				n0627();
			}
			fadeEnabled = extensionName;
		}

		public void n0250(bool extensionName)
		{
			sunEffectEnabled = extensionName;
		}

		public void n0251(bool extensionName)
		{
			effectEnabled = extensionName;
		}

		public void n0252(bool extensionName)
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

		public void n0254(bool extensionName)
		{
			syncVoice = extensionName;
		}

		private void n0561()
		{
			indicatorPhase = 0;
		}

		private void n0637(int n0638, int n0639)
		{
			Rectangle n880 = new Rectangle(indicatorColumns[indicatorPhase % 16] * 32, 0, 32, 32);
			Point n881 = new Point(n0638 + indicatorOffsetX[indicatorPhase], n0639 + indicatorOffsetY[indicatorPhase]);
			QueueDraw(modeIconTexture, n880, Rectangle.Empty, n881, Color.White);
			indicatorPhase++;
			indicatorPhase %= 32;
		}

		private Point DrawTextCore(string n0241, Point n0644, int n0645, int n0646, int offset, Microsoft.DirectX.Direct3D.Font n0353, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(n0241, 0, n0241.Length, n0644, n0645, n0646, offset, HorizontalAlign.Left, 0, n0353, foreColor, shadowColor);
		}

		private Point DrawTextCore(string n0241, int n0647, int n0648, Point n0644, int n0645, int n0646, int offset, HorizontalAlign align, int n0649, Microsoft.DirectX.Direct3D.Font n0353, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(n0241, n0647, n0648, n0644, n0645, n0646, offset, align, n0649, n0353, false, 0, foreColor, shadowColor);
		}

		private Point DrawTextCore(string n0241, double n0648, Point n0644, int n0645, int n0646, int offset, Microsoft.DirectX.Direct3D.Font n0353, int LanguageCount, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(n0241, 0, n0648, n0644, n0645, n0646, offset, HorizontalAlign.Left, 0, n0353, true, LanguageCount, foreColor, shadowColor);
		}

		private Point DrawTextCore(string n0241, double n0648, Point n0644, int n0645, int n0646, int offset, HorizontalAlign align, int n0649, Microsoft.DirectX.Direct3D.Font n0353, int LanguageCount, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(n0241, 0, n0648, n0644, n0645, n0646, offset, align, n0649, n0353, true, LanguageCount, foreColor, shadowColor);
		}

		private Point DrawTextCore(string n0241, int n0647, double n0648, Point n0644, int n0645, int n0646, int offset, HorizontalAlign align, int n0649, Microsoft.DirectX.Direct3D.Font n0353, bool Typewriter, int LanguageCount, Color foreColor, Color shadowColor)
		{
			return DrawTextCore(n0241, n0647, n0648, n0644, n0645, n0646, offset, align, n0649, n0353, Typewriter, LanguageCount, foreColor, shadowColor, true);
		}

		private Point DrawTextCore(string n0241, int n0647, double n0648, Point n0644, int n0645, int n0646, int offset, HorizontalAlign align, int n0649, Microsoft.DirectX.Direct3D.Font n0353, bool Typewriter, int LanguageCount, Color foreColor, Color shadowColor, bool n0650)
		{
			if (n0650)
			{
				n0216.Add(null);
				n0217.Add(null);
				n0218.Add(null);
				n0219.Add(null);
				n0220.Add(null);
				textQueue.Add(n0241);
				startIndexQueue.Add(n0647);
				endIndexQueue.Add(n0648);
				textPositionQueue.Add(n0644);
				charWidthQueue.Add(n0645);
				lineHeightQueue.Add(n0646);
				shadowOffsetQueue.Add(offset);
				fontSizeModeQueue.Add(align);
				lineWidthQueue.Add(n0649);
				fadeOutQueue.Add(Typewriter);
				fadeLengthQueue.Add(LanguageCount);
				textColorQueue.Add(foreColor);
				shadowColorQueue.Add(shadowColor);
			}
			int num;
			switch (align)
			{
			case HorizontalAlign.Left:
				num = n0644.X;
				break;
			case HorizontalAlign.Center:
			{
				int num3 = n0241.IndexOf('\n');
				if (num3 == -1)
				{
					num3 = n0241.Length;
				}
				num = n0644.X + n0645 * (n0649 - num3) / 2;
				break;
			}
			case HorizontalAlign.Right:
			{
				int num2 = n0241.IndexOf('\n');
				if (num2 == -1)
				{
					num2 = n0241.Length;
				}
				num = n0644.X + n0645 * (n0649 - num2);
				break;
			}
			default:
				num = n0644.X;
				break;
			}
			int num4 = n0644.Y;
			for (int i = 0; i < n0241.Length; i++)
			{
				if (n0241[i] == '\n')
				{
					switch (align)
					{
					case HorizontalAlign.Left:
						num = n0644.X;
						break;
					case HorizontalAlign.Center:
					{
						int num7 = n0241.IndexOf('\n', i + 1);
						if (num7 == -1)
						{
							num7 = n0241.Length;
						}
						int num8 = num7 - i - 1;
						num = n0644.X + n0645 * (n0649 - num8) / 2;
						break;
					}
					case HorizontalAlign.Right:
					{
						int num5 = n0241.IndexOf('\n', i + 1);
						if (num5 == -1)
						{
							num5 = n0241.Length;
						}
						int num6 = num5 - i - 1;
						num = n0644.X + n0645 * (n0649 - num6);
						break;
					}
					}
					num4 += n0646;
					continue;
				}
				if (i >= n0647 && (double)i < n0648)
				{
					if (fadeEnabled)
					{
						Sprite spriteLocal = this.sprite;
						n0353.DrawText(spriteLocal, n0241[i].ToString(), num + offset, num4 + offset, shadowColor);
					}
					Sprite sprite2 = sprite;
					n0353.DrawText(sprite2, n0241[i].ToString(), num, num4, foreColor);
				}
				else if (Typewriter && i >= n0647 && (double)i >= n0648 && (double)i < n0648 + (double)LanguageCount && fadeEnabled)
				{
					int alpha = (int)(255.0 * ((double)(LanguageCount - i) + n0648) / (double)LanguageCount);
					Sprite sprite3 = sprite;
					n0353.DrawText(sprite3, n0241[i].ToString(), num + offset, num4 + offset, (shadowColor == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha, shadowColor));
					Sprite sprite4 = sprite;
					n0353.DrawText(sprite4, n0241[i].ToString(), num, num4, Color.FromArgb(alpha, foreColor));
				}
				num += n0645;
			}
			return new Point(num, num4);
		}

		private void n0562()
		{
			backgroundFadeCounter = 0;
			backgroundColor = (previousBackgroundColor = Color.White);
			backgroundTexture = (previousBackgroundTexture = (desaturatedBackgroundTexture = null));
			backgroundIsColor = (previousBackgroundIsColor = true);
			backgroundWipeActive = false;
			backgroundDrawnAsColor = false;
			backgroundRedrawPending = false;
		}

		private void n0628()
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

		public void SetBackground(Texture n0241, int durationMode)
		{
			if (previousBackgroundTexture != null)
			{
				previousBackgroundTexture.Dispose();
			}
			previousBackgroundTexture = backgroundTexture;
			previousBackgroundColor = backgroundColor;
			backgroundTexture = n0241;
			backgroundColor = Color.White;
			previousBackgroundIsColor = backgroundIsColor;
			backgroundIsColor = false;
			backgroundFadeSpeed = durationMode;
			backgroundFadeCounter = backgroundFadeDurations[durationMode];
			backgroundWipeActive = false;
			if (thinkActive)
			{
				n0660();
			}
			else if (desaturatedBackgroundTexture != null)
			{
				desaturatedBackgroundTexture.Dispose();
				desaturatedBackgroundTexture = null;
			}
		}

		public void SetBackground(Color n0241, int durationMode)
		{
			if (previousBackgroundTexture != null)
			{
				previousBackgroundTexture.Dispose();
			}
			previousBackgroundTexture = backgroundTexture;
			previousBackgroundColor = backgroundColor;
			backgroundColor = n0241;
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

		private void n0584()
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
						QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, previousBackgroundColor);
					}
					if (fadeEnabled)
					{
						Point n880 = new Point(0, 0);
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
							Rectangle n882;
							for (int j = 0; j < 16; j++)
							{
								n880.X = 832 * (backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / backgroundFadeDurations[backgroundFadeSpeed] + (-j - 1) * 2;
								n882 = new Rectangle(n880.X, 0, 2, 600);
								int alpha2 = (int)((double)(j + 1) / 17.0 * 255.0);
								QueueDraw(backgroundTexture, n882, Rectangle.Empty, n880, Color.FromArgb(alpha2, Color.White));
							}
							n880.X = 0;
							n882 = new Rectangle(n880.X, 0, 832 * (backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / backgroundFadeDurations[backgroundFadeSpeed] - 32, 600);
							QueueDraw(backgroundTexture, n882, Rectangle.Empty, n880, Color.White);
							break;
						}
						case TextureBlend.Right:
						{
							if (zoomActive)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle n881;
							for (int i = 0; i < 16; i++)
							{
								n880.X = 832 * backgroundFadeCounter / backgroundFadeDurations[backgroundFadeSpeed] + (i - 16) * 2;
								n881 = new Rectangle(n880.X, 0, 2, 600);
								int alpha = (int)((double)(i + 1) / 17.0 * 255.0);
								QueueDraw(backgroundTexture, n881, Rectangle.Empty, n880, Color.FromArgb(alpha, Color.White));
							}
							n880.X = 832 * backgroundFadeCounter / backgroundFadeDurations[backgroundFadeSpeed];
							n881 = new Rectangle(n880.X, 0, 800 - n880.X, 600);
							QueueDraw(backgroundTexture, n881, Rectangle.Empty, n880, Color.White);
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
							QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, previousBackgroundColor);
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
							QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, backgroundColor);
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
							float num = (thinkReverse ? ((float)thinkCounter / 10f) : ((float)(10 - thinkCounter) / 10f));
							QueueDraw(backgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							QueueDraw(desaturatedBackgroundTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * num), Color.White));
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
					QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, backgroundColor);
				}
			}
			backgroundDrawnAsColor = true;
		}

		private void n0590()
		{
			if (backgroundFadeCounter <= 0 || backgroundWipeActive)
			{
				return;
			}
			if (fadeEnabled)
			{
				if (backgroundDrawnAsColor && previousBackgroundIsColor)
				{
					QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)backgroundFadeCounter / (double)backgroundFadeDurations[backgroundFadeSpeed]), previousBackgroundColor));
				}
				else
				{
					if (backgroundDrawnAsColor || !backgroundIsColor)
					{
						throw new Exception("Not Implement Yet");
					}
					QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(backgroundFadeDurations[backgroundFadeSpeed] - backgroundFadeCounter) / (double)backgroundFadeDurations[backgroundFadeSpeed]), backgroundColor));
				}
				backgroundFadeCounter--;
			}
			else
			{
				backgroundFadeCounter = 0;
			}
		}

		private void n0660()
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
					uint num = array[i, j];
					uint num2 = ((num & 0xFF) + ((num >> 8) & 0xFF) + ((num >> 16) & 0xFF)) / 3;
					array2[i, j] = (uint)(-16777216 + (int)(num2 << 16) + (int)(num2 << 8)) + num2;
				}
			}
			backgroundTexture.UnlockRectangle(0);
			desaturatedBackgroundTexture.UnlockRectangle(0);
		}

		private void n0563()
		{
			n0667();
			n0668();
			n0669();
			n0670();
			n0671();
			n0672();
			n0673();
			n0674();
		}

		private void n0629()
		{
			n0675();
			n0676();
			n0677();
			n0678();
		}

		private void n0667()
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

		private void n0668()
		{
			zoomActive = false;
			zoomTargetRect = (zoomSourceRect = screenRect);
			zoomCounter = 0;
			zoomSpeedIndex = 0;
		}

		public void ZoomBackground(double left, double right, double top, double bottom, int n0680, bool animate, bool finish)
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
			zoomSpeedIndex = n0680;
			if (animate)
			{
				zoomCounter = zoomDurations[zoomSpeedIndex];
			}
			else
			{
				zoomCounter = 0;
			}
		}

		public void n0682()
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
						double num = (double)zoomCounter / (double)zoomDurations[zoomSpeedIndex];
						double num2 = 1.0 - num;
						Rectangle n880 = new Rectangle((int)(num * (double)zoomSourceRect.Left + num2 * (double)zoomTargetRect.Left), (int)(num * (double)zoomSourceRect.Top + num2 * (double)zoomTargetRect.Top), (int)(num * (double)zoomSourceRect.Width + num2 * (double)zoomTargetRect.Width), (int)(num * (double)zoomSourceRect.Height + num2 * (double)zoomTargetRect.Height));
						if (backgroundTexture != null)
						{
							QueueDraw(backgroundTexture, n880, screenRect, Point.Empty, Color.White);
						}
						else if (previousBackgroundTexture != null)
						{
							QueueDraw(previousBackgroundTexture, n880, screenRect, Point.Empty, Color.White);
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
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		private void n0669()
		{
			shakeActive = false;
			shakeLoop = false;
			shakeFrame = 0;
		}

		public void n0458(bool loop, bool finish)
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

		private void n0683()
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
					double num = (double)zoomCounter / (double)zoomDurations[zoomSpeedIndex];
					double num2 = 1.0 - num;
					rectangle = new Rectangle((int)(num * (double)zoomSourceRect.Left + num2 * (double)zoomTargetRect.Left), (int)(num * (double)zoomSourceRect.Top + num2 * (double)zoomTargetRect.Top), (int)(num * (double)zoomSourceRect.Width + num2 * (double)zoomTargetRect.Width), (int)(num * (double)zoomSourceRect.Height + num2 * (double)zoomTargetRect.Height));
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
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		private void n0670()
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

		private void n0675()
		{
			sunTexture.Dispose();
		}

		public void n0446(bool finish)
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

		private void n0695()
		{
			if (!sunActive || !sunEffectEnabled)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < 5; i++)
			{
				if (sunLifeTotals[i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && sunRandom.NextDouble() < 0.25)
			{
				sunLifeCounters[num] = 0;
				sunLifeTotals[num] = (int)(20.0 + 60.0 * sunRandom.NextDouble());
				sunScales[num] = 0.5 + 0.5 * sunRandom.NextDouble();
				sunOffsets[num] = (int)(-75.0 + 150.0 * sunRandom.NextDouble());
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
			QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
		}

		private void DrawSunSprite(int n0644, double n0697, double durationMode)
		{
			int x = (n0644 - 18) * 80 / (int)(450.0 * n0697);
			int y = (-n0644 - 18) * 80 / (int)(450.0 * n0697);
			Rectangle n880 = new Rectangle(0, 0, 80, 80);
			Rectangle n881 = new Rectangle(0, 0, (int)(450.0 * n0697), (int)(450.0 * n0697));
			QueueDraw(sunTexture, n880, n881, new Point(x, y), Color.FromArgb((int)(n0697 * durationMode * 255.0), Color.White));
		}

		private void n0671()
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

		private void n0676()
		{
			petalTexture.Dispose();
		}

		public void n0448(bool finish)
		{
			if (!finish)
			{
				petalActive = true;
				backgroundIsColor = false;
				n0711();
			}
			else
			{
				petalActive = false;
			}
		}

		private void n0711()
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

		private void n0712(bool n0713)
		{
			if (!petalActive || !effectEnabled)
			{
				return;
			}
			int num = -1;
			for (int i = ((!n0713) ? 1 : 0); i < 30; i += 2)
			{
				if (petalLifeCounters[i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && (petalActiveCount < 20 || !(petalRandom.NextDouble() >= 0.2)))
			{
				petalFrames[num] = petalRandom.Next(4);
				int num2 = petalRandom.Next(-3297, 624);
				petalVelocitiesY[num] = (float)(petalRandom.NextDouble() * 16.0);
				petalVelocitiesX[num] = (float)(petalRandom.NextDouble() * 8.0 * 2.0 - 8.0);
				petalTargetVelocitiesY[num] = petalVelocitiesY[num];
				petalTargetVelocitiesX[num] = petalVelocitiesX[num];
				if (num2 < 0)
				{
					petalPositions[num] = new PointF((0f - (float)num2 - 1f) % 824f - 24f, -24f);
				}
				else if (petalVelocitiesX[num] > 0f)
				{
					petalPositions[num] = new PointF(-24f, num2 - 24);
				}
				else
				{
					petalPositions[num] = new PointF(800f, num2 - 24);
				}
				petalTurnTimers[num] = petalRandom.Next(40, 200);
				petalLifeCounters[num] = petalRandom.Next(1, 9);
				petalActiveCount++;
			}
			for (int j = ((!n0713) ? 1 : 0); j < 30; j += 2)
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
					float num3 = petalTargetVelocitiesY[j] - petalVelocitiesY[j];
					float num4 = petalTargetVelocitiesX[j] - petalVelocitiesX[j];
					float num5 = ((num3 == 0f) ? float.NaN : Math.Abs(num4 / num3));
					if (Math.Abs(num4) > Math.Abs(num3))
					{
						if (Math.Abs(num4) > 0.1f)
						{
							float[] array2;
							(array2 = petalVelocitiesX)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num4 > 0f) ? 0.1f : (-0.1f));
							if (!float.IsNaN(num5))
							{
								(array2 = petalVelocitiesY)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num3 > 0f) ? 0.1f : (-0.1f)) / num5;
							}
						}
						else
						{
							petalVelocitiesX[j] = petalTargetVelocitiesX[j];
							petalVelocitiesY[j] = petalTargetVelocitiesY[j];
						}
					}
					else if (Math.Abs(num3) > 0.1f)
					{
						float[] array3;
						(array3 = petalVelocitiesY)[(int)(intPtr = (IntPtr)j)] = array3[(int)intPtr] + ((num3 > 0f) ? 0.1f : (-0.1f));
						(array3 = petalVelocitiesX)[(int)(intPtr = (IntPtr)j)] = array3[(int)intPtr] + ((num4 > 0f) ? 0.1f : (-0.1f)) * num5;
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

		private void DrawPetal(int n0715, int n0716, PointF n0644)
		{
			Rectangle n880 = new Rectangle(n0715 * 24, (n0716 - 1) * 24, 24, 24);
			QueueDraw(petalTexture, n880, Rectangle.Empty, new Point((int)n0644.X, (int)n0644.Y), Color.White);
		}

		private void n0672()
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

		private void n0677()
		{
			snowTexture.Dispose();
		}

		public void n0450(bool finish)
		{
			if (!finish)
			{
				snowActive = true;
				backgroundIsColor = false;
				n0728();
			}
			else
			{
				snowActive = false;
			}
		}

		private void n0728()
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

		private void n0729()
		{
			if (!snowActive || !effectEnabled)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < 25; i++)
			{
				if (snowLifeCounters[i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && (snowActiveCount < 15 || snowRandom.NextDouble() < 0.2))
			{
				snowPositions[num] = new PointF(snowRandom.Next(-16, 800), -16f);
				snowSpeeds[num] = (float)(snowRandom.NextDouble() * 4.0 + 2.0);
				snowSwayAmplitudes[num] = (float)(snowRandom.NextDouble() * 16.0 + 8.0);
				snowSwayFrequencies[num] = (float)(snowRandom.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
				snowSwayPhases[num] = (float)(snowRandom.NextDouble() * (Math.PI * 2.0));
				snowLifeCounters[num] = snowRandom.Next(1, 5);
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

		private void DrawSnowflake(int n0716, PointF n0644)
		{
			Rectangle n880 = new Rectangle((n0716 - 1) * 16, 0, 16, 16);
			QueueDraw(snowTexture, n880, Rectangle.Empty, new Point((int)n0644.X, (int)n0644.Y), Color.White);
		}

		private void n0673()
		{
			dimActive = false;
			dimReverse = false;
			dimCounter = 0;
		}

		public void n0444(bool animate, bool finish)
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

		private void n0731()
		{
			if (!dimActive)
			{
				return;
			}
			if (dimCounter > 0)
			{
				if (fadeEnabled)
				{
					float num = (dimReverse ? ((float)dimCounter / 10f) : ((float)(10 - dimCounter) / 10f));
					QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(128f * num), Color.Black));
					dimCounter--;
				}
				else
				{
					QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
					dimCounter = 0;
				}
				if (dimReverse && dimCounter == 0)
				{
					dimActive = false;
				}
			}
			else
			{
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
			}
		}

		private void n0674()
		{
			thinkTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("THINK"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			thinkActive = false;
			thinkReverse = false;
			thinkCounter = 0;
		}

		private void n0678()
		{
			thinkTexture.Dispose();
		}

		public void n0452(bool animate, bool finish)
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
				n0660();
				n0733();
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

		private void n0734()
		{
			if (!thinkActive)
			{
				return;
			}
			if (thinkCounter > 0)
			{
				if (fadeEnabled)
				{
					float num = (thinkReverse ? ((float)thinkCounter / 10f) : ((float)(10 - thinkCounter) / 10f));
					QueueDraw(thinkTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * num), Color.White));
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

		private void n0585()
		{
			n0682();
			n0683();
			n0695();
			n0712(false);
		}

		private void n0587()
		{
			n0712(true);
			n0729();
			n0731();
			n0734();
		}

		private void n0564()
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

		private void n0630()
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

		public void SetCharacter(Texture n0241, Size size, int index, int location, bool isTransparent, bool animate)
		{
			if (previousCharacterTextures[index] != null)
			{
				previousCharacterTextures[index].Dispose();
			}
			previousCharacterTextures[index] = characterTextures[index];
			previousCharacterSizes[index] = characterSizes[index];
			characterTextures[index] = n0241;
			characterSizes[index] = size;
			previousCharacterOffsets[index] = characterOffsets[index];
			if (location != int.MaxValue)
			{
				characterOffsets[index] = location;
			}
			if (n0241 != null)
			{
				characterSemiTransparent[index] = isTransparent;
			}
			if (animate)
			{
				characterAppearCounters[index] = 8;
			}
			if (thinkActive)
			{
				n0733();
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

		public void n0403(int index, int level)
		{
			characterShakePatterns[index] = level;
			characterShakeCounters[index] = characterShakePatternLengths[level];
		}

		private void n0586()
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
							float num = (thinkReverse ? ((float)thinkCounter / 10f) : ((float)(10 - thinkCounter) / 10f));
							QueueDraw(characterTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.White);
							QueueDraw(characterShadowTextures[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + characterOffsets[i] * 15 - characterSizes[i].Width / 2, 600 - characterSizes[i].Height), Color.FromArgb((int)(255f * num), Color.White));
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

		private void n0733()
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
						uint num = array[j, k];
						uint num2 = ((num & 0xFF) + ((num >> 8) & 0xFF) + ((num >> 16) & 0xFF)) / 3;
						uint num3 = (num >> 24) & 0xFF;
						array2[j, k] = (num3 << 24) + (num2 << 16) + (num2 << 8) + num2;
					}
				}
				characterTextures[i].UnlockRectangle(0);
				characterShadowTextures[i].UnlockRectangle(0);
			}
		}

		private void n0565()
		{
			n0747 = null;
			n0748 = 0;
			n0625();
		}

		private void n0631()
		{
			if (n0593 != null)
			{
				n0593.Dispose();
			}
		}

		private void n0625()
		{
			if (n0593 != null)
			{
				n0593.Dispose();
			}
			n0593 = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font(baseFont.FontFamily, 24f));
		}

		public void ShowNotice(string[] n0241)
		{
			n0747 = n0241;
			n0748 = 70;
		}

		private void n0591()
		{
			if (n0747 != null)
			{
				if (n0748 == 0)
				{
					n0747 = null;
					return;
				}
				int alpha;
				if (fadeEnabled)
				{
					if (n0748 <= 10)
					{
						alpha = (int)((float)n0748 / 10f * 255f);
					}
					else
					{
						alpha = ((n0748 > 60) ? ((int)((float)(70 - n0748) / 10f * 255f)) : 255);
					}
				}
				else
				{
					alpha = 255;
				}
				DrawTextCore(n0747[(int)layoutIndex], 0, n0747[(int)layoutIndex].Length, noticePosition, noticeCharSize.Width, noticeCharSize.Height, 2, HorizontalAlign.Right, 15, n0593, Color.FromArgb(alpha, Color.White), Color.FromArgb(alpha, Color.Black));
				fontIndexQueue.Add(1);
				n0748--;
			}
		}

		private void n0566()
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
			n0626();
		}

		private void n0632()
		{
			if (messageFont != null)
			{
				messageFont.Dispose();
			}
		}

		private void n0626()
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
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (messageStartIndexes[i] + messageVisibleChars >= messageLines[i].Length)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				messageVisibleChars = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex];
				if (messageAlignMode == TextRevealMode.Typewriter)
				{
					messageProgress = messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4;
				}
			}
		}

		public void SetMessage(string[] n0241, HorizontalAlign align, TextRevealMode showMode, bool n0758, Color foreColor, Color shadowColor, double length)
		{
			if (n0241 == null)
			{
				return;
			}
			messageFontSizeMode = align;
			messageAlignMode = showMode;
			showNextIndicator = n0758;
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
				(array = messageLines)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + n0241[j];
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

		public void n0427(bool animate)
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

		private void n0588()
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
							double num = audioManager.GetVoicePosition() / messageVoiceLength;
							messageProgress = (double)(messageLines[(int)layoutIndex].Length - messageStartIndexes[(int)layoutIndex] + 4) * num;
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
					n0637(lastTextPosition.X, lastTextPosition.Y);
				}
				break;
			case TextRevealMode.Fade:
				if (lineRevealCounter == 0)
				{
					n0637(lastTextPosition.X, lastTextPosition.Y);
				}
				break;
			}
		}

		private void n0567()
		{
			dialogueTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("DIALOGUE"), dialogueRect.Width, dialogueRect.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			characterPanelTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("CHARA"), characterPanelSize.Width, characterPanelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			dialogueCounter = 0;
			dialogueClosing = false;
			dialogueActive = false;
			characterPanelVisible = false;
		}

		private void n0633()
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
						float num = (float)(3 - dialogueCounter) / 3f;
						float num2 = ((float)dialogueRect.X + (1f - num) / 2f * (float)dialogueRect.Width) / num;
						float num3 = ((float)dialogueRect.Y + (1f - num) / 2f * (float)dialogueRect.Height) / num;
						float num4 = num * (float)dialogueRect.Width;
						float num5 = num * (float)dialogueRect.Height;
						QueueDraw(dialogueTexture, new Rectangle(0, 0, dialogueRect.Width, dialogueRect.Height), new Rectangle(0, 0, (int)num4, (int)num5), new Point((int)num2, (int)num3), dialogueColor);
						QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					else if (dialogueClosing && dialogueCounter > 0)
					{
						float num6 = (float)dialogueCounter / 3f;
						float num7 = ((float)dialogueRect.X + (1f - num6) / 2f * (float)dialogueRect.Width) / num6;
						float num8 = ((float)dialogueRect.Y + (1f - num6) / 2f * (float)dialogueRect.Height) / num6;
						float num9 = num6 * (float)dialogueRect.Width;
						float num10 = num6 * (float)dialogueRect.Height;
						QueueDraw(dialogueTexture, new Rectangle(0, 0, dialogueRect.Width, dialogueRect.Height), new Rectangle(0, 0, (int)num9, (int)num10), new Point((int)num7, (int)num8), dialogueColor);
						QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
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

		private void n0568()
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
			n0627();
		}

		private void n0634()
		{
			if (captionFont != null)
			{
				captionFont.Dispose();
			}
		}

		private void n0627()
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
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (captionStartIndexes[i] + captionVisibleChars >= captionLines[i].Length)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				captionVisibleChars = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex];
				captionProgress = captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4;
			}
		}

		public void SetCaption(string[] n0241, string[] charaNames, double length, bool isContinuous, bool instant)
		{
			if (n0241 == null)
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
				(array = captionLines)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + n0241[j];
			}
			captionVisibleChars = 0;
			captionProgress = 0.0;
		}

		public void CloseDialogue(bool shadowColor, bool n0773, bool instant)
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
			if (n0773)
			{
				characterPanelVisible = false;
			}
		}

		private void n0589()
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
							double num = audioManager.GetVoicePosition() / captionVoiceLength;
							captionProgress = (double)(captionLines[(int)layoutIndex].Length - captionStartIndexes[(int)layoutIndex] + 4) * num;
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
					n0637(captionLastPosition.X, captionLastPosition.Y);
				}
				if (captionOptionLines != null && DrawDialogueFrame(true))
				{
					DrawTextCore(captionOptionLines[(int)layoutIndex], new Point(captionOptionPosition.X - captionCharSize.Width * captionOptionLines[(int)layoutIndex].Length / 2, captionOptionPosition.Y), captionCharSize.Width, captionCharSize.Height, 2, captionFont, Color.White, Color.Black);
					fontIndexQueue.Add(3);
				}
			}
		}

		public void n0467(int n0241)
		{
			modeIconIndex = n0241;
		}

		private void n0569()
		{
			modeIconTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("ICON"), modeIconTextureSize.Width, modeIconTextureSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			modeIconFrame = 0;
		}

		private void n0635()
		{
			modeIconTexture.Dispose();
		}

		private void n0592()
		{
			if (modeIconIndex > 0)
			{
				Rectangle n880 = new Rectangle(modeIconFrame / 3 * 32, modeIconIndex * 32, 32, 32);
				QueueDraw(modeIconTexture, n880, Rectangle.Empty, modeIconPosition, Color.White);
				modeIconFrame++;
				if (modeIconFrame == 9)
				{
					modeIconFrame = 0;
				}
			}
		}
	}
}
