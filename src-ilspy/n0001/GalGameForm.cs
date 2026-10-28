using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;
using System.Xml;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectDraw;
using Microsoft.DirectX.DirectInput;
using Yukihota.Audio;

namespace Yukihota
{
	public class GalGameForm : Form
	{
		private const int n0315 = 24;

		protected const int n0316 = 274;

		protected const int n0317 = 61760;

		protected const int n0318 = 61808;

		protected const int n0319 = 10;

		protected const int n0320 = 180;

		protected const int n0321 = 32;

		protected const int n0322 = 310;

		protected const int n0323 = 368;

		protected const int n0324 = 5;

		protected const int n0325 = 10;

		protected const int n0326 = 23;

		protected const int n0327 = 225;

		protected const int n0328 = 25;

		protected const int n0329 = 273;

		protected const int n0330 = 25;

		protected const int n0331 = 16;

		protected const int n0332 = 25;

		protected const int n0333 = 3;

		protected const int n0334 = 6;

		protected const int n0335 = 10;

		protected const int n0336 = 280;

		protected const int n0337 = 28;

		protected const int n0338 = 80;

		protected const int n0339 = 40;

		protected const int n0340 = 16;

		protected const int n0341 = 8;

		protected const int n0342 = 10;

		protected const int n0343 = 120;

		protected const int n0344 = 90;

		protected const int n0345 = 7;

		protected const int n0346 = 2;

		protected const int n0347 = 24;

		private Microsoft.DirectX.DirectInput.Device joystick;

		private bool[] joystickButtonStates = new bool[10];

		private EffectObject forceFeedbackEffect;

		private Point windowedLocation;

		private int gameMode;

		private SceneDirector director;

		private Point lastCursorPosition;

		private MediaPackage menuBgmPlayer;

		private Microsoft.DirectX.Direct3D.Device device;

		private Microsoft.DirectX.DirectDraw.Device displayDevice;

		private bool isBorderless;

		private bool suppressLostFocus;

		private Sprite sprite;

		protected Texture n0135;

		private static bool n0125;

		private bool languageResetPending = false;

		private ArchiveFile systemArchive;

		private ArchiveFile bgmArchive;

		private ArchiveFile bgArchive;

		private string fontName = "黑体";

		private bool fullscreen = true;

		private ButtonLayout language = ButtonLayout.SimplifiedChinese;

		private bool showOptionsOnStartup = true;

		private bool fadeEnabled = true;

		private bool envEnabled = true;

		private bool sunEnabled = true;

		private bool shockEnabled = true;

		private Color textColor = Color.FromArgb(128, 128, 255);

		private int textSpeed = 4;

		private bool voiceSynchro = true;

		private int unlockedEpisodeCount = 0;

		private int unlockedAlbumCount = 0;

		private IContainer components;

		private Timer scenarioTimer;

		private Timer menuTimer;

		private Timer optionsTimer;

		private ContextMenu contextMenu;

		private MenuItem menuCancelItem;

		private MenuItem menuSeparator1;

		private MenuItem menuOptionsItem;

		private MenuItem menuSeparator2;

		private MenuItem menuDisplayModeItem;

		private MenuItem menuSeparator3;

		private MenuItem menuSkipItem;

		private MenuItem menuAutoItem;

		private MenuItem menuHideWindowItem;

		private MenuItem menuSeparator4;

		private MenuItem menuLanguageItem;

		private MenuItem menuSimplifiedItem;

		private MenuItem menuTraditionalItem;

		private MenuItem menuJapaneseItem;

		private MenuItem menuSeparator5;

		private MenuItem menuFontItem;

		private MenuItem menuSeparator6;

		private MenuItem menuBackToTitleItem;

		private MenuItem menuSeparator7;

		private MenuItem menuExitItem;

		private Timer logoFadeTimer;

		private Timer episodeTimer;

		private Timer albumTimer;

		private PictureBox movieBox;

		private FontDialog fontDialog;

		private bool enterKeyDown = false;

		private bool spaceKeyDown = false;

		private bool f2KeyDown = false;

		private bool f3KeyDown = false;

		private bool f4KeyDown = false;

		private bool f8KeyDown = false;

		private bool f12KeyDown = false;

		protected ArrayList n0216 = new ArrayList();

		protected ArrayList n0217 = new ArrayList();

		protected ArrayList n0218 = new ArrayList();

		protected ArrayList n0219 = new ArrayList();

		protected ArrayList n0220 = new ArrayList();

		protected string overlayText;

		protected readonly int[] logoFadeThresholds = new int[3] { 5, 15, 20 };

		private int logoFrame;

		private Texture logoTexture;

		private MediaPackage openingMovie;

		private int menuFadeCounter;

		private bool menuFadeIn;

		private Texture[] titleTextures = new Texture[3];

		private Texture menuButtonTexture;

		private int menuSelection;

		protected readonly Size menuButtonSize = new Size(360, 192);

		private bool[] menuItemEnabled = new bool[5];

		private int optionsFadeCounter;

		private bool optionsFadeIn;

		private Texture[] optionsBackTextures = new Texture[3];

		private Texture[] optionsLabelTextures = new Texture[3];

		protected readonly int[] optionLabelX = new int[23]
		{
			500, 650, 500, 650, 500, 650, 500, 650, 275, 350,
			425, 500, 575, 650, 725, 325, 400, 475, 550, 625,
			700, 275, 525
		};

		protected readonly int[] optionLabelY = new int[23]
		{
			35, 35, 65, 65, 95, 95, 125, 125, 170, 170,
			170, 170, 170, 170, 170, 215, 215, 215, 215, 215,
			215, 260, 260
		};

		protected readonly int[] optionSourceX = new int[23]
		{
			0, 25, 0, 25, 0, 25, 0, 25, 50, 75,
			100, 125, 150, 175, 200, 225, 245, 265, 285, 305,
			325, 0, 125
		};

		protected readonly int[] optionSourceY = new int[23]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 25, 25
		};

		protected readonly int[] optionSourceWidth = new int[23]
		{
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 20, 20, 20, 20, 20,
			20, 125, 100
		};

		protected readonly int[] optionSourceHeight = new int[23]
		{
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25
		};

		private int optionCursorFrame;

		protected readonly int[] optionCursorFrames = new int[6] { 0, 1, 2, 2, 1, 0 };

		protected readonly Size optionsLabelSize = new Size(345, 50);

		private bool[] optionItemVisible = new bool[23];

		private int optionSelection;

		private int returnMode;

		private int episodeFadeCounter;

		private bool episodeFadeIn;

		private Texture episodeBackTexture;

		private Texture episodeMenuTexture;

		private int episodeSelection;

		protected readonly int[] episodeColumnX = new int[2] { 100, 420 };

		protected readonly Size episodeMenuSize = new Size(560, 476);

		private int albumFadeCounter;

		private bool albumFadeIn;

		private Texture albumBackTexture;

		private Texture albumPictureTexture;

		private int albumSelection;

		protected readonly int[] albumThumbX = new int[7] { 360, 500, 640, 500, 640, 500, 640 };

		protected readonly int[] albumThumbY = new int[7] { 120, 120, 120, 240, 240, 360, 360 };

		protected readonly int albumThumbDimAlpha = 128;

		protected readonly string[] albumTitles = new string[7] { "１：Ｅｉｎｓａｔｚ", "２：Ａｕｆｔａｋｔ", "３：Ａｇｏｇｉｋ", "４：ｆｏｃｏ", "５：ｃｏｎｔｅｎｅｒｅｚｚａ", "６：Ｇ．Ｐ．", "７：Ａａｂｓｔｕｆｅｎ" };

		protected readonly Point textOrigin = new Point(40, 20);

		protected readonly Size charCellSize = new Size(30, 38);

		private Microsoft.DirectX.Direct3D.Font d3dFont;

		private int albumGroup;

		private int albumImageIndex;

		private int albumImageTransition;

		private Texture albumCurrentTexture;

		private Texture albumPrevTexture;

		protected readonly int[] albumImageCounts = new int[7] { 3, 2, 1, 1, 1, 1, 2 };

		private readonly Size albumPictureSize = new Size(120, 720);

		public GalGameForm()
		{
			InitializeComponent();
			base.GotFocus += new EventHandler(this.n0064);
			base.LostFocus += new EventHandler(this.n0065);
			gameMode = -2;
			menuBgmPlayer = new MediaPackage("MENU");
		}

		protected override void Dispose(bool n0010)
		{
			if (n0010 && components != null)
			{
				components.Dispose();
			}
			base.Dispose(n0010);
		}

		private void InitializeComponent()
		{
			components = new Container();
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(GalGameForm));
			scenarioTimer = new Timer(components);
			menuTimer = new Timer(components);
			optionsTimer = new Timer(components);
			contextMenu = new ContextMenu();
			menuCancelItem = new MenuItem();
			menuSeparator1 = new MenuItem();
			menuOptionsItem = new MenuItem();
			menuSeparator2 = new MenuItem();
			menuDisplayModeItem = new MenuItem();
			menuSeparator3 = new MenuItem();
			menuSkipItem = new MenuItem();
			menuAutoItem = new MenuItem();
			menuHideWindowItem = new MenuItem();
			menuSeparator4 = new MenuItem();
			menuLanguageItem = new MenuItem();
			menuSimplifiedItem = new MenuItem();
			menuTraditionalItem = new MenuItem();
			menuJapaneseItem = new MenuItem();
			menuSeparator5 = new MenuItem();
			menuFontItem = new MenuItem();
			menuSeparator6 = new MenuItem();
			menuBackToTitleItem = new MenuItem();
			menuSeparator7 = new MenuItem();
			menuExitItem = new MenuItem();
			fontDialog = new FontDialog();
			logoFadeTimer = new Timer(components);
			episodeTimer = new Timer(components);
			albumTimer = new Timer(components);
			movieBox = new PictureBox();
			base.SuspendLayout();
			scenarioTimer.Interval = 50;
			scenarioTimer.Tick += new EventHandler(this.n0097);
			menuTimer.Interval = 50;
			menuTimer.Tick += new EventHandler(this.n0098);
			optionsTimer.Interval = 50;
			optionsTimer.Tick += new EventHandler(this.n0099);
			contextMenu.MenuItems.AddRange(new MenuItem[17]
			{
				menuCancelItem, menuSeparator1, menuOptionsItem, menuSeparator2, menuDisplayModeItem, menuSeparator3, menuSkipItem, menuAutoItem, menuHideWindowItem, menuSeparator4,
				menuLanguageItem, menuSeparator5, menuFontItem, menuSeparator6, menuBackToTitleItem, menuSeparator7, menuExitItem
			});
			menuCancelItem.Index = 0;
			menuCancelItem.Text = "取消(&C)";
			menuCancelItem.Click += new EventHandler(this.n0100);
			menuSeparator1.Index = 1;
			menuSeparator1.Text = "-";
			menuOptionsItem.Index = 2;
			menuOptionsItem.Text = "选项(&O)\tF8";
			menuOptionsItem.Click += new EventHandler(this.n0101);
			menuSeparator2.Index = 3;
			menuSeparator2.Text = "-";
			menuDisplayModeItem.Index = 4;
			menuDisplayModeItem.Text = "显示模式(&M)\tAlt+Enter";
			menuDisplayModeItem.Click += new EventHandler(this.n0102);
			menuSeparator3.Index = 5;
			menuSeparator3.Text = "-";
			menuSkipItem.Index = 6;
			menuSkipItem.Text = "快进(&Q)\tF3";
			menuSkipItem.Click += new EventHandler(this.n0103);
			menuAutoItem.Index = 7;
			menuAutoItem.Text = "自动播放(&A)\tF4";
			menuAutoItem.Click += new EventHandler(this.n0104);
			menuHideWindowItem.Index = 8;
			menuHideWindowItem.Text = "隐藏对话(&H)\tF2";
			menuHideWindowItem.Click += new EventHandler(this.n0105);
			menuSeparator4.Index = 9;
			menuSeparator4.Text = "-";
			menuLanguageItem.Index = 10;
			menuLanguageItem.MenuItems.AddRange(new MenuItem[3] { menuSimplifiedItem, menuTraditionalItem, menuJapaneseItem });
			menuLanguageItem.Text = "语言(&L)\tF12";
			menuSimplifiedItem.Index = 0;
			menuSimplifiedItem.RadioCheck = true;
			menuSimplifiedItem.Text = "简体中文(&S)";
			menuSimplifiedItem.Click += new EventHandler(this.n0106);
			menuTraditionalItem.Index = 1;
			menuTraditionalItem.RadioCheck = true;
			menuTraditionalItem.Text = "繁體中文(&T)";
			menuTraditionalItem.Click += new EventHandler(this.n0107);
			menuJapaneseItem.Enabled = false;
			menuJapaneseItem.Index = 2;
			menuJapaneseItem.RadioCheck = true;
			menuJapaneseItem.Text = "日本語(&J)";
			menuJapaneseItem.Click += new EventHandler(this.n0108);
			menuSeparator5.Index = 11;
			menuSeparator5.Text = "-";
			menuFontItem.Index = 12;
			menuFontItem.Text = "字体(&F)";
			menuFontItem.Click += new EventHandler(this.n0109);
			menuSeparator6.Index = 13;
			menuSeparator6.Text = "-";
			menuBackToTitleItem.Index = 14;
			menuBackToTitleItem.Text = "返回主菜单(&B)";
			menuBackToTitleItem.Click += new EventHandler(this.n0110);
			menuSeparator7.Index = 15;
			menuSeparator7.Text = "-";
			menuExitItem.Index = 16;
			menuExitItem.Text = "结束游戏(&E)\tAlt+F4";
			menuExitItem.Click += new EventHandler(this.n0111);
			fontDialog.AllowVerticalFonts = false;
			fontDialog.Font = new System.Drawing.Font("黑体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			fontDialog.FontMustExist = true;
			fontDialog.MaxSize = 24;
			fontDialog.MinSize = 24;
			fontDialog.ShowEffects = false;
			logoFadeTimer.Interval = 50;
			logoFadeTimer.Tick += new EventHandler(this.n0112);
			episodeTimer.Interval = 50;
			episodeTimer.Tick += new EventHandler(this.n0113);
			albumTimer.Interval = 50;
			albumTimer.Tick += new EventHandler(this.n0114);
			movieBox.BackColor = Color.White;
			movieBox.Location = new Point(0, 0);
			movieBox.Name = "ptbMovie";
			movieBox.Size = new Size(800, 600);
			movieBox.TabIndex = 0;
			movieBox.TabStop = false;
			movieBox.Visible = false;
			AutoScaleBaseSize = new Size(6, 14);
			BackColor = Color.White;
			ClientSize = new Size(800, 600);
			base.Controls.Add(movieBox);
			Font = new System.Drawing.Font("黑体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resourceManager.GetObject("$this.Icon");
			KeyPreview = true;
			MaximizeBox = false;
			base.Name = "GalGame";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "秋之回忆 ～雪萤～";
			base.KeyDown += new KeyEventHandler(this.n0115);
			base.MouseDown += new MouseEventHandler(this.n0116);
			Closing += new CancelEventHandler(this.OnClosing);
			Load += new EventHandler(this.n0118);
			base.KeyUp += new KeyEventHandler(this.n0119);
			Activated += new EventHandler(this.n0120);
			base.Paint += new PaintEventHandler(this.OnPaint);
			base.MouseMove += new MouseEventHandler(this.n0122);
			base.ResumeLayout(false);
		}

		[STAThread]
		private static void Main(string[] n0124)
		{
			try
			{
				if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
				{
					Application.Exit();
					return;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			bool flag = true;
			n0125 = false;
			for (int i = 0; i < n0124.Length; i++)
			{
				if (n0124[i] == "/nocopyright")
				{
					flag = false;
				}
				if (n0124[i] == "/noop")
				{
					n0125 = true;
				}
			}
			if (flag)
			{
				MessageBox.Show("本游戏为KID Fans Club特别创作组免费制作并发布。\n仅供广大KID Fans试玩，请于下载后24小时自觉删除，\n任何个人或组织不得将其用于任何形式的商业用途，\n违者如造成商业侵权，KID Fans Club将不负连带责任。\n相关说明见Readme.txt\n\n本遊戲為KID Fans Club特別創作組免費製作並發佈。\n僅供廣大KID Fans試玩，請於下載後24小時自覺刪除，\n任何個人或組織不得將其用於任何形式的商業用途，\n違者如造成商業侵權，KID Fans Club將不負連帶責任。\n相關說明見Readme.txt");
			}
			Application.Run(new GalGameForm());
		}

		protected override void WndProc(ref Message n0126)
		{
			if (n0126.Msg == 274 && ((int)n0126.WParam == 61760 || (int)n0126.WParam == 61808))
			{
				n0126.Result = (IntPtr)1;
			}
			else
			{
				base.WndProc(ref n0126);
			}
		}

		private void n0118(object n0035, EventArgs n0036)
		{
			n0127();
			OptionsForm n880 = new OptionsForm();
			n880.SetFontName(new System.Drawing.Font(fontName, 24f));
			n880.n0055(fullscreen);
			n880.n0057(language);
			n880.n0059(showOptionsOnStartup);
			if (showOptionsOnStartup && n880.ShowDialog() == DialogResult.Cancel)
			{
				Application.Exit();
				return;
			}
			fontName = n880.GetFontName().Name;
			fullscreen = n880.n0054();
			language = n880.GetButtonLayout();
			showOptionsOnStartup = n880.n0058();
			ArchiveFile.VerifyKey("Mullen");
			for (int i = 0; i < 8; i++)
			{
				joystickButtonStates[i] = false;
			}
			joystick = n880.GetDevice();
			if (joystick != null)
			{
				try
				{
					joystick.SetDataFormat(DeviceDataFormat.Joystick);
					joystick.SetCooperativeLevel(this, Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Exclusive);
					foreach (object @object in joystick.Objects)
					{
						DeviceObjectInstance deviceObjectInstance = (DeviceObjectInstance)@object;
						if ((deviceObjectInstance.ObjectId & 3) != 0)
						{
							joystick.Properties.SetRange(ParameterHow.ById, deviceObjectInstance.ObjectId, new InputRange(-1000, 1000));
						}
					}
					if (n880.n0052())
					{
						int[] array = null;
						foreach (object object2 in joystick.GetObjects(DeviceObjectTypeFlags.Axis))
						{
							DeviceObjectInstance deviceObjectInstance2 = (DeviceObjectInstance)object2;
							if ((deviceObjectInstance2.Flags & 1) != 0)
							{
								if (array != null)
								{
									int[] array2 = new int[array.Length + 1];
									array.CopyTo(array2, 0);
									array = array2;
								}
								else
								{
									array = new int[1];
								}
								array[array.Length - 1] = deviceObjectInstance2.Offset;
								if (2 == array.Length)
								{
									break;
								}
							}
						}
						IEnumerator enumerator2 = joystick.GetEffects(EffectType.All).GetEnumerator();
						while (enumerator2.MoveNext())
						{
							object current3 = enumerator2.Current;
							EffectInformation effectInformation = (EffectInformation)current3;
							if (DInputHelper.GetTypeCode(effectInformation.EffectType) == 1)
							{
								Microsoft.DirectX.DirectInput.Effect die = new Microsoft.DirectX.DirectInput.Effect();
								die.SetDirection(new int[array.Length]);
								die.SetAxes(new int[array.Length]);
								die.EffectType = EffectType.ConstantForce;
								die.ConditionStruct = new Condition[array.Length];
								die.Duration = -1;
								die.Gain = 10000;
								die.SamplePeriod = 0;
								die.TriggerButton = -1;
								die.TriggerRepeatInterval = -1;
								die.Flags = EffectFlags.Cartesian | EffectFlags.ObjectOffsets;
								die.Constant.Magnitude = 5000;
								die.SetAxes(array);
								forceFeedbackEffect = new EffectObject(effectInformation.EffectGuid, die, joystick);
							}
						}
					}
					else
					{
						forceFeedbackEffect = null;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					Application.Exit();
					return;
				}
			}
			PresentParameters presentParameters = new PresentParameters();
			presentParameters.SwapEffect = SwapEffect.Flip;
			presentParameters.Windowed = true;
			try
			{
				device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, this, Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing, presentParameters);
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
				Application.Exit();
				return;
			}
			sprite = new Sprite(device);
			windowedLocation = base.Location;
			displayDevice = new Microsoft.DirectX.DirectDraw.Device();
			n0147(n880.n0054());
			isBorderless = false;
			suppressLostFocus = false;
			try
			{
				systemArchive = new ArchiveFile("System", ArchiveMode.Read);
				bgmArchive = new ArchiveFile("Bgm", ArchiveMode.Read);
				bgArchive = new ArchiveFile("Bg", ArchiveMode.Read);
			}
			catch (Exception ex3)
			{
				MessageBox.Show(ex3.Message);
				Application.Exit();
				return;
			}
			try
			{
				string tempPath = Path.GetTempPath();
				tempPath = tempPath.Substring(0, tempPath.Length - 1);
				bgmArchive.Extract("BGM12", tempPath);
				menuBgmPlayer.Open(tempPath + "\\BGM12.wav");
			}
			catch (Exception ex4)
			{
				MessageBox.Show(ex4.Message);
				Application.Exit();
				return;
			}
			ApplyLanguage(language);
			switch (language)
			{
			case ButtonLayout.SimplifiedChinese:
				menuSimplifiedItem.Checked = true;
				break;
			case ButtonLayout.TraditionalChinese:
				menuTraditionalItem.Checked = true;
				break;
			case ButtonLayout.Japanese:
				menuJapaneseItem.Checked = true;
				break;
			}
			lastCursorPosition = Cursor.Position;
			n0135 = TextureLoader.FromStream(device, systemArchive.ReadEntry("WHITE"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			try
			{
				n0138();
				n0139();
				n0140();
				n0141();
				n0142();
				n0143();
			}
			catch (Exception ex5)
			{
				MessageBox.Show(ex5.Message);
				Application.Exit();
			}
		}

		private void OnClosing(object n0035, CancelEventArgs n0036)
		{
			if (gameMode == 1)
			{
				if (director.n0157() > unlockedEpisodeCount)
				{
					unlockedEpisodeCount = director.n0157();
				}
				if (director.n0159() > unlockedAlbumCount)
				{
					unlockedAlbumCount = director.n0159();
				}
			}
			n0161();
			if (joystick != null)
			{
				try
				{
					if (null != forceFeedbackEffect)
					{
						forceFeedbackEffect.Stop();
					}
					joystick.Unacquire();
					joystick.Properties.AutoCenter = true;
				}
				catch (InputException)
				{
				}
			}
			string tempPath = Path.GetTempPath();
			menuBgmPlayer.Close();
			try
			{
				if (File.Exists(tempPath + "BGM12.wav"))
				{
					File.Delete(tempPath + "BGM12.wav");
				}
			}
			catch
			{
			}
		}

		private void n0120(object n0035, EventArgs n0036)
		{
			if (joystick != null)
			{
				try
				{
					joystick.Acquire();
				}
				catch (InputException)
				{
				}
			}
		}

		private void n0064(object n0035, EventArgs n0036)
		{
			if (isBorderless)
			{
				n0147(true);
				WindowState = FormWindowState.Normal;
			}
			switch (gameMode)
			{
			case -2:
				if (openingMovie != null)
				{
					openingMovie.Play();
				}
				break;
			case -1:
				logoFadeTimer.Start();
				break;
			case 0:
				menuTimer.Start();
				menuBgmPlayer.Resume();
				break;
			case 1:
				scenarioTimer.Start();
				director.SetPaused(false, true);
				break;
			case 2:
				episodeTimer.Start();
				menuBgmPlayer.Resume();
				break;
			case 3:
				optionsTimer.Start();
				if (returnMode == 0)
				{
					menuBgmPlayer.Resume();
				}
				else if (returnMode == 1)
				{
					director.SetPaused(false, true);
				}
				break;
			case 4:
				albumTimer.Start();
				menuBgmPlayer.Resume();
				break;
			}
		}

		protected override void OnLostFocus(EventArgs n0036)
		{
			if (!suppressLostFocus)
			{
				base.OnLostFocus(n0036);
			}
		}

		private void n0065(object n0035, EventArgs n0036)
		{
			isBorderless = FormBorderStyle == FormBorderStyle.None;
			if (isBorderless)
			{
				n0147(false);
				WindowState = FormWindowState.Minimized;
			}
			switch (gameMode)
			{
			case -2:
				if (openingMovie != null)
				{
					openingMovie.Pause();
				}
				break;
			case -1:
				logoFadeTimer.Stop();
				break;
			case 0:
				menuTimer.Stop();
				menuBgmPlayer.Pause();
				break;
			case 1:
				scenarioTimer.Stop();
				director.SetPaused(true, true);
				break;
			case 2:
				episodeTimer.Stop();
				menuBgmPlayer.Pause();
				break;
			case 3:
				optionsTimer.Stop();
				if (returnMode == 0)
				{
					menuBgmPlayer.Pause();
				}
				else if (returnMode == 1)
				{
					director.SetPaused(true, true);
				}
				break;
			case 4:
				albumTimer.Stop();
				menuBgmPlayer.Pause();
				break;
			}
		}

		private void n0115(object n0035, KeyEventArgs n0036)
		{
			switch (n0036.KeyCode)
			{
			case Keys.ControlKey:
				if (gameMode == 1)
				{
					director.instant(true, true);
				}
				break;
			case Keys.Return:
				if (enterKeyDown)
				{
					break;
				}
				enterKeyDown = true;
				if (n0036.Alt)
				{
					n0147();
					break;
				}
				switch (gameMode)
				{
				case 0:
					if (menuFadeCounter == 0 && menuSelection > 0 && menuItemEnabled[menuSelection - 1])
					{
						n0173();
					}
					break;
				case 1:
					director.waitClick();
					break;
				case 2:
					if (episodeFadeCounter == 0 && episodeSelection > 0 && unlockedEpisodeCount >= episodeSelection)
					{
						n0177();
					}
					break;
				case 3:
					if (optionsFadeCounter == 0 && optionSelection > 0)
					{
						n0180(optionSelection - 1);
					}
					break;
				case 4:
					if (albumFadeCounter == 0 && albumImageTransition == 0)
					{
						if (albumGroup != 0)
						{
							ShowAlbumImage(albumGroup, albumImageIndex + 1);
						}
						else if (albumSelection > 0 && unlockedAlbumCount >= albumSelection)
						{
							ShowAlbumImage(albumSelection, 0);
						}
					}
					break;
				}
				break;
			case Keys.Space:
				if (spaceKeyDown)
				{
					break;
				}
				spaceKeyDown = true;
				switch (gameMode)
				{
				case 1:
					if (!director.n0188())
					{
						contextMenu.Show(this, new Point(400, 300));
					}
					break;
				case 2:
					if (episodeFadeCounter == 0)
					{
						episodeSelection = 0;
						n0177();
					}
					break;
				case 3:
					if (optionsFadeCounter == 0)
					{
						n0189();
					}
					break;
				case 4:
					if (albumFadeCounter == 0 && albumImageTransition == 0)
					{
						if (albumGroup == 0)
						{
							n0190();
						}
						else
						{
							n0191();
						}
					}
					break;
				}
				break;
			case Keys.Left:
				switch (gameMode)
				{
				case 3:
					n0192(0);
					break;
				case 4:
					if (albumGroup == 0)
					{
						n0193(0);
					}
					break;
				}
				break;
			case Keys.Up:
				switch (gameMode)
				{
				case 0:
					n0194(0);
					break;
				case 1:
					break;
				case 2:
					n0195(0);
					break;
				case 3:
					n0192(2);
					break;
				}
				break;
			case Keys.Right:
				switch (gameMode)
				{
				case 3:
					n0192(1);
					break;
				case 4:
					if (albumGroup == 0)
					{
						n0193(1);
					}
					break;
				}
				break;
			case Keys.Down:
				switch (gameMode)
				{
				case 0:
					n0194(1);
					break;
				case 1:
					break;
				case 2:
					n0195(1);
					break;
				case 3:
					n0192(3);
					break;
				}
				break;
			case Keys.F2:
				if (!f2KeyDown)
				{
					f2KeyDown = true;
					if (gameMode == 1)
					{
						director.n0197();
					}
				}
				break;
			case Keys.F3:
				if (!f3KeyDown)
				{
					f3KeyDown = true;
					if (gameMode == 1)
					{
						director.instant();
					}
				}
				break;
			case Keys.F4:
				if (!f4KeyDown)
				{
					f4KeyDown = true;
					if (gameMode == 1)
					{
						director.n0200();
					}
				}
				break;
			case Keys.F8:
				if (!f8KeyDown)
				{
					f8KeyDown = true;
					if (gameMode == 1 && !director.n0188())
					{
						director.n0200(false);
						director.SetPaused(true, false);
						scenarioTimer.Stop();
						n0140();
						n0202();
					}
				}
				break;
			case Keys.F12:
				if (f12KeyDown)
				{
					break;
				}
				f12KeyDown = true;
				if (gameMode == 1)
				{
					language++;
					if (language == ButtonLayout.LanguageCount)
					{
						language = ButtonLayout.SimplifiedChinese;
					}
					if (!director.n0188() && language == ButtonLayout.Japanese)
					{
						language = ButtonLayout.SimplifiedChinese;
					}
					if (director.n0188())
					{
						languageResetPending = true;
					}
					menuSimplifiedItem.Checked = false;
					menuTraditionalItem.Checked = false;
					menuJapaneseItem.Checked = false;
					switch (language)
					{
					case ButtonLayout.SimplifiedChinese:
						menuSimplifiedItem.Checked = true;
						break;
					case ButtonLayout.TraditionalChinese:
						menuTraditionalItem.Checked = true;
						break;
					case ButtonLayout.Japanese:
						menuTraditionalItem.Checked = true;
						break;
					}
					ApplyLanguage(language);
					director.SetButtonLayout(language);
				}
				break;
			}
		}

		private void n0119(object n0035, KeyEventArgs n0036)
		{
			switch (n0036.KeyCode)
			{
			case Keys.Return:
				enterKeyDown = false;
				break;
			case Keys.ControlKey:
				if (gameMode == 1)
				{
					director.instant(false, true);
				}
				break;
			case Keys.Space:
				spaceKeyDown = false;
				break;
			case Keys.F2:
				f2KeyDown = false;
				break;
			case Keys.F3:
				f3KeyDown = false;
				break;
			case Keys.F4:
				f4KeyDown = false;
				break;
			case Keys.F8:
				f8KeyDown = false;
				break;
			case Keys.F12:
				f12KeyDown = false;
				break;
			}
		}

		private void n0116(object n0035, MouseEventArgs n0036)
		{
			switch (gameMode)
			{
			case 0:
				n0207(n0035, n0036);
				break;
			case 1:
				if (n0036.Button == MouseButtons.Left)
				{
					director.waitClick();
				}
				else if (n0036.Button == MouseButtons.Right && !director.n0188())
				{
					director.SetPaused(true, false);
					scenarioTimer.Stop();
					contextMenu.Show((Control)n0035, new Point(n0036.X, n0036.Y));
					director.SetPaused(false, false);
					scenarioTimer.Start();
				}
				break;
			case 2:
				n0208(n0035, n0036);
				break;
			case 3:
				n0209(n0035, n0036);
				break;
			case 4:
				n0210(n0035, n0036);
				break;
			}
		}

		private void n0122(object n0035, MouseEventArgs n0036)
		{
			switch (gameMode)
			{
			case 0:
				n0211(n0035, n0036);
				break;
			case 1:
				break;
			case 2:
				n0212(n0035, n0036);
				break;
			case 3:
				n0213(n0035, n0036);
				break;
			case 4:
				n0214(n0035, n0036);
				break;
			}
		}

		private void OnPaint(object n0035, PaintEventArgs n0036)
		{
			if (gameMode == -2)
			{
				return;
			}
			if (gameMode == 1)
			{
				director.n0215();
				return;
			}
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			for (int i = 0; i < n0216.Count; i++)
			{
				if (n0216[i] != null)
				{
					sprite.Draw2D((Texture)n0216[i], (Rectangle)n0217[i], (Rectangle)n0218[i], Point.Empty, 0f, (Point)n0219[i], (Color)n0220[i]);
					continue;
				}
				for (int j = 0; j < overlayText.Length; j++)
				{
					Microsoft.DirectX.Direct3D.Font font = d3dFont;
					Sprite spriteLocal = this.sprite;
					string text = overlayText[j].ToString();
					int num = textOrigin.X;
					font.DrawText(spriteLocal, text, num + charCellSize.Width * j + 2, textOrigin.Y + 2, Color.Black);
					Microsoft.DirectX.Direct3D.Font font2 = d3dFont;
					Sprite sprite2 = spriteLocal;
					string text2 = overlayText[j].ToString();
					int num2 = textOrigin.X;
					font2.DrawText(sprite2, text2, num2 + charCellSize.Width * j, textOrigin.Y, Color.White);
				}
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
					n0225();
				}
				catch
				{
				}
			}
		}

		private void n0097(object n0035, EventArgs n0036)
		{
			bool flag = false;
			switch (PollJoystick())
			{
			case 0:
				director.waitClick();
				break;
			case 1:
				director.n0197();
				break;
			case 2:
				director.instant();
				break;
			case 3:
				director.n0200();
				break;
			case 8:
				if (!director.n0188())
				{
					director.n0200(false);
					director.SetPaused(true, false);
					scenarioTimer.Stop();
					n0140();
					n0202();
					flag = true;
				}
				break;
			case 9:
				if (!director.n0188())
				{
					n0227();
					flag = true;
				}
				break;
			}
			if (languageResetPending && language == ButtonLayout.Japanese && !director.n0188())
			{
				language = ButtonLayout.SimplifiedChinese;
				menuSimplifiedItem.Checked = true;
				menuTraditionalItem.Checked = false;
				menuJapaneseItem.Checked = false;
				ApplyLanguage(language);
				director.SetButtonLayout(language);
				languageResetPending = false;
			}
			if (!flag && !director.Update())
			{
				n0227();
			}
		}

		private void n0100(object n0035, EventArgs n0036)
		{
		}

		private void n0101(object n0035, EventArgs n0036)
		{
			director.n0200(false);
			director.SetPaused(true, false);
			scenarioTimer.Stop();
			n0140();
			n0202();
		}

		private void n0102(object n0035, EventArgs n0036)
		{
			n0147();
		}

		private void n0103(object n0035, EventArgs n0036)
		{
			director.instant();
		}

		private void n0104(object n0035, EventArgs n0036)
		{
			director.n0200();
		}

		private void n0105(object n0035, EventArgs n0036)
		{
			director.n0197();
		}

		private void n0106(object n0035, EventArgs n0036)
		{
			menuSimplifiedItem.Checked = true;
			menuTraditionalItem.Checked = false;
			menuJapaneseItem.Checked = false;
			language = ButtonLayout.SimplifiedChinese;
			ApplyLanguage(ButtonLayout.SimplifiedChinese);
			director.SetButtonLayout(ButtonLayout.SimplifiedChinese);
		}

		private void n0107(object n0035, EventArgs n0036)
		{
			menuSimplifiedItem.Checked = false;
			menuTraditionalItem.Checked = true;
			menuJapaneseItem.Checked = false;
			language = ButtonLayout.TraditionalChinese;
			ApplyLanguage(ButtonLayout.TraditionalChinese);
			director.SetButtonLayout(ButtonLayout.TraditionalChinese);
		}

		private void n0108(object n0035, EventArgs n0036)
		{
			menuSimplifiedItem.Checked = false;
			menuTraditionalItem.Checked = false;
			menuJapaneseItem.Checked = true;
			language = ButtonLayout.Japanese;
			ApplyLanguage(ButtonLayout.Japanese);
			director.SetButtonLayout(ButtonLayout.Japanese);
		}

		private void n0109(object n0035, EventArgs n0036)
		{
			if (fontDialog.ShowDialog() == DialogResult.OK)
			{
				director.SetFont(fontDialog.Font);
				fontName = fontDialog.Font.Name;
			}
		}

		private void n0110(object n0035, EventArgs n0036)
		{
			n0227();
		}

		private void n0111(object n0035, EventArgs n0036)
		{
			director.Stop();
			director.Dispose();
			Close();
			Application.Exit();
		}

		private void n0127()
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load("save_sys.dat");
			}
			catch
			{
				return;
			}
			ReadSetting(xmlDocument, "sys/win/font", ref fontName);
			string n880 = null;
			ReadSetting(xmlDocument, "sys/win/mode", ref n880);
			switch (n880)
			{
			case "Fullscreen":
				fullscreen = true;
				break;
			case "Windowed":
				fullscreen = false;
				break;
			}
			n880 = null;
			ReadSetting(xmlDocument, "sys/win/language", ref n880);
			switch (n880)
			{
			case "SC":
				language = ButtonLayout.SimplifiedChinese;
				break;
			case "TC":
				language = ButtonLayout.TraditionalChinese;
				break;
			case "JP":
				language = ButtonLayout.Japanese;
				break;
			}
			ReadSetting(xmlDocument, "sys/win/show", ref showOptionsOnStartup);
			ReadSetting(xmlDocument, "sys/options/fade", ref fadeEnabled);
			ReadSetting(xmlDocument, "sys/options/env", ref envEnabled);
			ReadSetting(xmlDocument, "sys/options/sun", ref sunEnabled);
			ReadSetting(xmlDocument, "sys/options/shock", ref shockEnabled);
			n880 = null;
			ReadSetting(xmlDocument, "sys/options/color", ref n880);
			switch (n880)
			{
			case "Green":
				textColor = Color.FromArgb(128, 255, 128);
				break;
			case "Blue":
				textColor = Color.FromArgb(128, 128, 255);
				break;
			case "Red":
				textColor = Color.FromArgb(255, 128, 128);
				break;
			case "Yellow":
				textColor = Color.FromArgb(255, 255, 128);
				break;
			case "Purple":
				textColor = Color.FromArgb(255, 128, 255);
				break;
			case "Cyan":
				textColor = Color.FromArgb(128, 255, 255);
				break;
			case "Black":
				textColor = Color.FromArgb(128, 128, 128);
				break;
			}
			ReadSetting(xmlDocument, "sys/options/speed", 1, 6, ref textSpeed);
			ReadSetting(xmlDocument, "sys/options/synchro", ref voiceSynchro);
			ReadSetting(xmlDocument, "sys/save/episode", 0, 16, ref unlockedEpisodeCount);
			ReadSetting(xmlDocument, "sys/save/album", 0, 7, ref unlockedAlbumCount);
		}

		private void n0161()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("sys"));
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("win"));
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("font")).InnerText = fontName;
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("mode")).InnerText = (fullscreen ? "Fullscreen" : "Windowed");
			switch (language)
			{
			case ButtonLayout.SimplifiedChinese:
				xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("language")).InnerText = "SC";
				break;
			case ButtonLayout.TraditionalChinese:
				xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("language")).InnerText = "TC";
				break;
			default:
				xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("language")).InnerText = "SC";
				break;
			}
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("show")).InnerText = showOptionsOnStartup.ToString();
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("options"));
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("fade")).InnerText = fadeEnabled.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("env")).InnerText = envEnabled.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("sun")).InnerText = sunEnabled.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("shock")).InnerText = shockEnabled.ToString();
			string innerText;
			if (textColor == Color.FromArgb(128, 255, 128))
			{
				innerText = "Green";
			}
			else if (textColor == Color.FromArgb(128, 128, 255))
			{
				innerText = "Blue";
			}
			else if (textColor == Color.FromArgb(255, 128, 128))
			{
				innerText = "Red";
			}
			else if (textColor == Color.FromArgb(255, 255, 128))
			{
				innerText = "Yellow";
			}
			else if (textColor == Color.FromArgb(255, 128, 255))
			{
				innerText = "Purple";
			}
			else if (textColor == Color.FromArgb(128, 255, 255))
			{
				innerText = "Cyan";
			}
			else
			{
				innerText = ((!(textColor == Color.FromArgb(128, 128, 128))) ? "Blue" : "Black");
			}
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("color")).InnerText = innerText;
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("speed")).InnerText = textSpeed.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("synchro")).InnerText = voiceSynchro.ToString();
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("save"));
			xmlDocument.SelectSingleNode("/sys/save").AppendChild(xmlDocument.CreateElement("episode")).InnerText = unlockedEpisodeCount.ToString();
			xmlDocument.SelectSingleNode("/sys/save").AppendChild(xmlDocument.CreateElement("album")).InnerText = unlockedAlbumCount.ToString();
			try
			{
				xmlDocument.Save("save_sys.dat");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n游戏保存可能失败");
			}
		}

		private void ReadSetting(XmlDocument n0239, string n0240, ref string n0241)
		{
			XmlNode xmlNode = n0239.SelectSingleNode(n0240);
			if (xmlNode != null)
			{
				n0241 = xmlNode.InnerText;
			}
		}

		private void ReadSetting(XmlDocument n0239, string n0240, ref bool n0241)
		{
			XmlNode xmlNode = n0239.SelectSingleNode(n0240);
			if (xmlNode != null)
			{
				if (xmlNode.InnerText == "True")
				{
					n0241 = true;
				}
				else if (xmlNode.InnerText == "False")
				{
					n0241 = false;
				}
			}
		}

		private void ReadSetting(XmlDocument n0239, string n0240, int n0242, int n0243, ref int n0241)
		{
			XmlNode xmlNode = n0239.SelectSingleNode(n0240);
			if (xmlNode == null)
			{
				return;
			}
			try
			{
				int num = int.Parse(xmlNode.InnerText);
				if (num >= n0242 && num <= n0243)
				{
					n0241 = num;
				}
			}
			catch
			{
			}
		}

		private void n0225(bool n0244)
		{
			PresentParameters presentParameters = new PresentParameters();
			presentParameters.SwapEffect = SwapEffect.Flip;
			presentParameters.Windowed = true;
			device.Reset(presentParameters);
		}

		private void n0225()
		{
			n0225(FormBorderStyle == FormBorderStyle.None);
		}

		private void n0147(bool n0244)
		{
			if (n0244)
			{
				TopMost = true;
				FormBorderStyle = FormBorderStyle.None;
				windowedLocation = base.Location;
				base.Location = new Point(0, 0);
				displayDevice.SetDisplayMode(800, 600, 32, 0, true);
				base.Width = 800;
				base.Height = 600;
			}
			else
			{
				displayDevice.RestoreDisplayMode();
				FormBorderStyle = FormBorderStyle.FixedSingle;
				base.Location = windowedLocation;
				TopMost = false;
				ClientSize = new Size(800, 600);
			}
		}

		private void n0147()
		{
			if (FormBorderStyle == FormBorderStyle.None)
			{
				n0147(false);
			}
			else
			{
				n0147(true);
			}
		}

		private void ApplyLanguage(ButtonLayout n0046)
		{
			switch (n0046)
			{
			case ButtonLayout.SimplifiedChinese:
				menuCancelItem.Text = "取消(&C)";
				menuOptionsItem.Text = "选项(&O)\tF8";
				menuDisplayModeItem.Text = "显示模式(&M)\tAlt+Enter";
				menuSkipItem.Text = "快进(&Q)\tF3";
				menuAutoItem.Text = "自动播放(&A)\tF4";
				menuHideWindowItem.Text = "隐藏对话(&H)\tF2";
				menuLanguageItem.Text = "语言(&L)";
				menuFontItem.Text = "字体(&F)";
				menuBackToTitleItem.Text = "返回主菜单(&B)";
				menuExitItem.Text = "结束游戏(&E)\tAlt+F4";
				Text = "秋之回忆 ～雪萤～";
				break;
			case ButtonLayout.TraditionalChinese:
				menuCancelItem.Text = "取消(&C)";
				menuOptionsItem.Text = "選項(&O)\tF8";
				menuDisplayModeItem.Text = "顯示模式(&M)\tAlt+Enter";
				menuSkipItem.Text = "快進(&Q)\tF3";
				menuAutoItem.Text = "自動播放(&A)\tF4";
				menuHideWindowItem.Text = "隱藏對話(&H)\tF2";
				menuLanguageItem.Text = "語言(&L)";
				menuFontItem.Text = "字體(&F)";
				menuBackToTitleItem.Text = "返回主菜單(&B)";
				menuExitItem.Text = "結束遊戲(&E)\tAlt+F4";
				Text = "秋之回憶 ～雪螢～";
				break;
			case ButtonLayout.Japanese:
				menuCancelItem.Text = "戻る(&C)";
				menuOptionsItem.Text = "オプション設定(&O)\tF8";
				menuDisplayModeItem.Text = "画面モード(&M)\tAlt+Enter";
				menuSkipItem.Text = "スキップ(&Q)\tF3";
				menuAutoItem.Text = "オートモード(&A)\tF4";
				menuHideWindowItem.Text = "ウインドウ消去(&H)\tF2";
				menuLanguageItem.Text = "言語(&L)";
				menuFontItem.Text = "フォント(&F)";
				menuBackToTitleItem.Text = "タイトルに戻る(&B)";
				menuExitItem.Text = "ゲーム終了(&E)\tAlt+F4";
				Text = "メモリーズオフデュエット ～雪蛍～";
				break;
			}
		}

		private int PollJoystick()
		{
			if (joystick != null)
			{
				try
				{
					joystick.Poll();
				}
				catch (InputException ex)
				{
					if (ex is NotAcquiredException || ex is InputLostException)
					{
						try
						{
							joystick.Acquire();
						}
						catch (InputException)
						{
							return -1;
						}
					}
				}
				JoystickState currentJoystickState;
				try
				{
					currentJoystickState = joystick.CurrentJoystickState;
				}
				catch (InputException)
				{
					return -1;
				}
				byte[] buttons = currentJoystickState.GetButtons();
				int num = currentJoystickState.X;
				int num2 = currentJoystickState.Y;
				if (num < -500 && num < -Math.Abs(num2))
				{
					buttons[4] = 128;
				}
				else if (num > 500 && num > Math.Abs(num2))
				{
					buttons[5] = 128;
				}
				else if (num2 < -500 && num2 <= -Math.Abs(num))
				{
					buttons[6] = 128;
				}
				else if (num2 > 500 && num2 >= Math.Abs(num))
				{
					buttons[7] = 128;
				}
				for (int i = 0; i < 10; i++)
				{
					if ((buttons[i] & 0x80) != 0)
					{
						if (!joystickButtonStates[i])
						{
							joystickButtonStates[i] = true;
							return i;
						}
					}
					else
					{
						joystickButtonStates[i] = false;
					}
				}
				return -1;
			}
			return -1;
		}

		private void n0245(int n0246)
		{
			try
			{
				director = new SceneDirector("Yukihota", n0246, device, sprite, new FrameCallback(n0225), n0135, forceFeedbackEffect, new System.Drawing.Font(fontName, 24f), language, textColor);
				director.n0249(fadeEnabled);
				director.n0250(sunEnabled);
				director.n0251(envEnabled);
				director.n0252(shockEnabled);
				director.SetTextSpeed(textSpeed);
				director.n0254(voiceSynchro);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Application.Exit();
			}
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			overlayText = null;
			gameMode = 1;
			menuBgmPlayer.Stop();
			n0255();
			n0256();
			n0257();
			n0258();
			n0259();
			scenarioTimer.Start();
		}

		private void n0227()
		{
			if (director.n0157() > unlockedEpisodeCount)
			{
				unlockedEpisodeCount = director.n0157();
			}
			if (director.n0159() > unlockedAlbumCount)
			{
				unlockedAlbumCount = director.n0159();
			}
			scenarioTimer.Stop();
			director.Stop();
			director.Dispose();
			director = null;
			GC.Collect();
			n0139();
			n0140();
			n0141();
			n0142();
			n0143();
			n0260();
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

		private void n0141()
		{
			logoTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("KID_LOGO"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			if (n0125)
			{
				logoFrame = 0;
				gameMode = -1;
			}
		}

		private void n0257()
		{
			logoTexture.Dispose();
			logoTexture = null;
		}

		private void n0138()
		{
			gameMode = -2;
			if (!n0125)
			{
				movieBox.Visible = true;
				openingMovie = new MediaPackage("Op");
				openingMovie.Open("Movie\\KFC.wmv", movieBox);
				openingMovie.Play();
			}
			logoFadeTimer.Start();
		}

		private void n0112(object n0035, EventArgs n0036)
		{
			if (gameMode == -2 && !n0125)
			{
				if (openingMovie.GetPosition() < openingMovie.GetLength() && openingMovie.GetLength() != -1)
				{
					return;
				}
				logoFrame = 0;
				gameMode = -1;
				movieBox.Visible = false;
				suppressLostFocus = true;
				openingMovie.Close();
				suppressLostFocus = false;
				openingMovie = null;
			}
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			overlayText = null;
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			int alpha;
			if (logoFrame < logoFadeThresholds[0])
			{
				alpha = (int)((double)logoFrame / (double)logoFadeThresholds[0] * 255.0);
			}
			else if (logoFrame < logoFadeThresholds[1])
			{
				alpha = 255;
			}
			else if (logoFrame < logoFadeThresholds[2])
			{
				alpha = (int)((double)(logoFadeThresholds[2] - logoFrame) / (double)(logoFadeThresholds[2] - logoFadeThresholds[1]) * 255.0);
			}
			else
			{
				logoFadeTimer.Stop();
				n0260();
				alpha = 0;
			}
			QueueDraw(logoTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
			logoFrame++;
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
					n0225();
				}
				catch
				{
				}
			}
		}

		private void n0139()
		{
			titleTextures[0] = TextureLoader.FromStream(device, systemArchive.ReadEntry("TITLESC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			titleTextures[1] = TextureLoader.FromStream(device, systemArchive.ReadEntry("TITLETC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			titleTextures[2] = TextureLoader.FromStream(device, systemArchive.ReadEntry("TITLEJP"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			menuButtonTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("MENU"), menuButtonSize.Width, menuButtonSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0255()
		{
			for (int i = 0; i < 3; i++)
			{
				titleTextures[i].Dispose();
				titleTextures[i] = null;
			}
			menuButtonTexture.Dispose();
			menuButtonTexture = null;
		}

		private void n0260()
		{
			gameMode = 0;
			menuFadeCounter = 10;
			menuFadeIn = true;
			bool[] array = menuItemEnabled;
			bool flag;
			menuItemEnabled[2] = (flag = (menuItemEnabled[4] = true));
			array[0] = flag;
			menuItemEnabled[1] = unlockedEpisodeCount > 0;
			menuItemEnabled[3] = unlockedAlbumCount > 0;
			if (!menuBgmPlayer.IsPlaying())
			{
				menuBgmPlayer.Play(true);
			}
			menuTimer.Start();
		}

		private void n0173()
		{
			menuFadeCounter = 10;
			menuFadeIn = false;
		}

		private void n0194(int n0277)
		{
			if (menuFadeCounter > 0)
			{
				return;
			}
			switch (n0277)
			{
			case 0:
				if (menuSelection == 0)
				{
					menuSelection = 5;
					break;
				}
				menuSelection--;
				if (menuSelection == 0)
				{
					menuSelection = 5;
				}
				break;
			case 1:
				if (menuSelection == 0)
				{
					menuSelection = 1;
					break;
				}
				menuSelection++;
				if (menuSelection == 6)
				{
					menuSelection = 1;
				}
				break;
			}
		}

		private void n0098(object n0035, EventArgs n0036)
		{
			GC.Collect();
			int num = PollJoystick();
			if (num == 0)
			{
				if (menuSelection > 0 && menuItemEnabled[menuSelection - 1])
				{
					n0173();
				}
			}
			else
			{
				int num2 = num;
				if (num2 == 6)
				{
					n0194(0);
				}
				else if (num == 7)
				{
					n0194(1);
				}
			}
			bool flag = false;
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			overlayText = null;
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			QueueDraw(titleTextures[(int)language], Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < 5; i++)
			{
				Rectangle n880 = new Rectangle((menuSelection - 1 != i) ? 180 : 0, 32 * (menuItemEnabled[i] ? i : 5), 180, 32);
				QueueDraw(menuButtonTexture, n880, Rectangle.Empty, new Point(310, 368 + 32 * i), Color.White);
			}
			if (menuFadeCounter > 0)
			{
				int alpha = (int)((double)(menuFadeIn ? menuFadeCounter : (10 - menuFadeCounter)) / 10.0 * 255.0);
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				menuFadeCounter--;
				if (!menuFadeIn && menuFadeCounter == 0)
				{
					menuTimer.Stop();
					switch (menuSelection)
					{
					case 1:
						flag = true;
						break;
					case 2:
						n0278();
						break;
					case 3:
						n0202();
						break;
					case 4:
						n0279();
						break;
					case 5:
						Close();
						Application.Exit();
						break;
					}
					menuSelection = 0;
				}
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
					n0225();
				}
				catch
				{
				}
			}
			if (flag)
			{
				n0245(1);
			}
		}

		private void n0207(object n0035, MouseEventArgs n0036)
		{
			if (menuFadeCounter == 0 && n0036.Button == MouseButtons.Left)
			{
				lastCursorPosition = Point.Empty;
				n0211(n0035, n0036);
				if (menuSelection > 0 && menuItemEnabled[menuSelection - 1])
				{
					n0173();
				}
			}
		}

		private void n0211(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(lastCursorPosition))
			{
				return;
			}
			lastCursorPosition = Cursor.Position;
			if (menuFadeCounter == 0)
			{
				if (n0036.X >= 310 && n0036.X < 490 && n0036.Y >= 368 && n0036.Y < 528)
				{
					menuSelection = (n0036.Y - 368) / 32 + 1;
				}
				else
				{
					menuSelection = 0;
				}
			}
		}

		private void n0140()
		{
			optionCursorFrame = 0;
			optionsBackTextures[0] = TextureLoader.FromStream(device, systemArchive.ReadEntry("OPT_BGSC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			optionsBackTextures[1] = TextureLoader.FromStream(device, systemArchive.ReadEntry("OPT_BGTC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			optionsBackTextures[2] = TextureLoader.FromStream(device, systemArchive.ReadEntry("OPT_BGJP"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] array = optionsLabelTextures;
			int num = 0;
			Microsoft.DirectX.Direct3D.Device deviceLocal = this.device;
			Stream stream = systemArchive.ReadEntry("OPTIONSC");
			int num2 = optionsLabelSize.Width;
			array[num] = TextureLoader.FromStream(deviceLocal, stream, num2, optionsLabelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] array2 = optionsLabelTextures;
			int num3 = 1;
			Microsoft.DirectX.Direct3D.Device device2 = device;
			Stream stream2 = systemArchive.ReadEntry("OPTIONTC");
			int num4 = optionsLabelSize.Width;
			array2[num3] = TextureLoader.FromStream(device2, stream2, num4, optionsLabelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] array3 = optionsLabelTextures;
			int num5 = 2;
			Microsoft.DirectX.Direct3D.Device device3 = device;
			Stream stream3 = systemArchive.ReadEntry("OPTIONJP");
			int num6 = optionsLabelSize.Width;
			array3[num5] = TextureLoader.FromStream(device3, stream3, num6, optionsLabelSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0256()
		{
			for (int i = 0; i < 3; i++)
			{
				optionsBackTextures[i].Dispose();
				optionsBackTextures[i] = null;
				optionsLabelTextures[i].Dispose();
				optionsLabelTextures[i] = null;
			}
		}

		private void n0202()
		{
			GC.Collect();
			returnMode = gameMode;
			gameMode = 3;
			optionsFadeCounter = 10;
			optionsFadeIn = true;
			optionsTimer.Start();
		}

		private void n0189()
		{
			optionsFadeCounter = 10;
			optionsFadeIn = false;
		}

		private void n0192(int n0277)
		{
			if (optionsFadeCounter > 0)
			{
				return;
			}
			if (optionSelection != 0)
			{
				if (optionSelection >= 1 && optionSelection <= 8)
				{
					switch (n0277)
					{
					case 0:
					case 1:
						optionSelection += ((optionSelection % 2 != 0) ? 1 : (-1));
						break;
					case 2:
						optionSelection -= 2;
						if (optionSelection <= 0)
						{
							optionSelection += 23;
						}
						break;
					case 3:
						optionSelection += 2;
						break;
					}
				}
				else if (optionSelection >= 9 && optionSelection <= 15)
				{
					switch (n0277)
					{
					case 0:
						optionSelection--;
						if (optionSelection == 8)
						{
							optionSelection = 15;
						}
						break;
					case 1:
						optionSelection++;
						if (optionSelection == 16)
						{
							optionSelection = 9;
						}
						break;
					case 2:
						if (optionSelection == 9)
						{
							optionSelection = 7;
						}
						else
						{
							optionSelection = 8;
						}
						break;
					case 3:
						if (optionSelection <= 14)
						{
							optionSelection += 7;
						}
						else
						{
							optionSelection = 21;
						}
						break;
					}
				}
				else if (optionSelection >= 16 && optionSelection <= 21)
				{
					switch (n0277)
					{
					case 0:
						optionSelection--;
						if (optionSelection == 15)
						{
							optionSelection = 21;
						}
						break;
					case 1:
						optionSelection++;
						if (optionSelection == 22)
						{
							optionSelection = 16;
						}
						break;
					case 2:
						optionSelection -= 7;
						break;
					case 3:
						if (optionSelection == 16)
						{
							optionSelection = 22;
						}
						else
						{
							optionSelection = 23;
						}
						break;
					}
				}
				else if (optionSelection >= 22 && optionSelection <= 23)
				{
					switch (n0277)
					{
					case 0:
					case 1:
						optionSelection += ((optionSelection % 2 != 1) ? 1 : (-1));
						break;
					case 2:
						optionSelection -= 6;
						break;
					case 3:
						optionSelection -= 21;
						break;
					}
				}
			}
			else
			{
				switch (n0277)
				{
				case 0:
				case 2:
					optionSelection = 23;
					break;
				case 1:
				case 3:
					optionSelection = 1;
					break;
				}
			}
		}

		private void n0180(int n0287)
		{
			switch (n0287)
			{
			case 0:
				fadeEnabled = true;
				break;
			case 1:
				fadeEnabled = false;
				break;
			case 2:
				envEnabled = true;
				break;
			case 3:
				envEnabled = false;
				break;
			case 4:
				sunEnabled = true;
				break;
			case 5:
				sunEnabled = false;
				break;
			case 6:
				shockEnabled = true;
				break;
			case 7:
				shockEnabled = false;
				break;
			case 8:
				textColor = Color.FromArgb(128, 255, 128);
				break;
			case 9:
				textColor = Color.FromArgb(128, 128, 255);
				break;
			case 10:
				textColor = Color.FromArgb(255, 128, 128);
				break;
			case 11:
				textColor = Color.FromArgb(255, 255, 128);
				break;
			case 12:
				textColor = Color.FromArgb(255, 128, 255);
				break;
			case 13:
				textColor = Color.FromArgb(128, 255, 255);
				break;
			case 14:
				textColor = Color.FromArgb(128, 128, 128);
				break;
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
				textSpeed = n0287 - 14;
				break;
			case 21:
				voiceSynchro = false;
				break;
			case 22:
				voiceSynchro = true;
				break;
			}
		}

		private void n0099(object n0035, EventArgs n0036)
		{
			int num = PollJoystick();
			switch (num)
			{
			case 0:
				if (optionSelection > 0)
				{
					n0180(optionSelection - 1);
				}
				break;
			case 1:
				n0189();
				break;
			}
			if (num >= 4 && num <= 7)
			{
				n0192(num - 4);
			}
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			overlayText = null;
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			QueueDraw(optionsBackTextures[(int)language], Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < 23; i++)
			{
				optionItemVisible[i] = false;
			}
			if (fadeEnabled)
			{
				optionItemVisible[0] = true;
			}
			else
			{
				optionItemVisible[1] = true;
			}
			if (envEnabled)
			{
				optionItemVisible[2] = true;
			}
			else
			{
				optionItemVisible[3] = true;
			}
			if (sunEnabled)
			{
				optionItemVisible[4] = true;
			}
			else
			{
				optionItemVisible[5] = true;
			}
			if (shockEnabled)
			{
				optionItemVisible[6] = true;
			}
			else
			{
				optionItemVisible[7] = true;
			}
			if (textColor == Color.FromArgb(128, 255, 128))
			{
				optionItemVisible[8] = true;
			}
			else if (textColor == Color.FromArgb(128, 128, 255))
			{
				optionItemVisible[9] = true;
			}
			else if (textColor == Color.FromArgb(255, 128, 128))
			{
				optionItemVisible[10] = true;
			}
			else if (textColor == Color.FromArgb(255, 255, 128))
			{
				optionItemVisible[11] = true;
			}
			else if (textColor == Color.FromArgb(255, 128, 255))
			{
				optionItemVisible[12] = true;
			}
			else if (textColor == Color.FromArgb(128, 255, 255))
			{
				optionItemVisible[13] = true;
			}
			else if (textColor == Color.FromArgb(128, 128, 128))
			{
				optionItemVisible[14] = true;
			}
			optionItemVisible[textSpeed + 14] = true;
			if (!voiceSynchro)
			{
				optionItemVisible[21] = true;
			}
			else
			{
				optionItemVisible[22] = true;
			}
			for (int j = 0; j < 23; j++)
			{
				if (optionItemVisible[j])
				{
					Rectangle n880 = new Rectangle(optionSourceX[j], optionSourceY[j], optionSourceWidth[j], optionSourceHeight[j]);
					QueueDraw(optionsLabelTextures[(int)language], n880, Rectangle.Empty, new Point(optionLabelX[j], optionLabelY[j]), Color.White);
				}
			}
			if (optionSelection > 0)
			{
				Rectangle n881 = new Rectangle(225 + 16 * optionCursorFrames[optionCursorFrame], 25, 16, 25);
				QueueDraw(optionsLabelTextures[(int)language], n881, Rectangle.Empty, new Point(optionLabelX[optionSelection - 1] - 16, optionLabelY[optionSelection - 1]), Color.White);
				n881 = new Rectangle(273 + 16 * optionCursorFrames[optionCursorFrame], 25, 16, 25);
				QueueDraw(optionsLabelTextures[(int)language], n881, Rectangle.Empty, new Point(optionLabelX[optionSelection - 1] + optionSourceWidth[optionSelection - 1], optionLabelY[optionSelection - 1]), Color.White);
				optionCursorFrame++;
				if (optionCursorFrame == 6)
				{
					optionCursorFrame = 0;
				}
			}
			bool flag = false;
			if (optionsFadeCounter > 0)
			{
				int alpha = (int)((double)(optionsFadeIn ? optionsFadeCounter : (10 - optionsFadeCounter)) / 10.0 * 255.0);
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				optionsFadeCounter--;
				if (!optionsFadeIn && optionsFadeCounter == 0)
				{
					optionsTimer.Stop();
					if (returnMode == 0)
					{
						n0260();
					}
					else if (returnMode == 1)
					{
						n0216.Clear();
						n0217.Clear();
						n0218.Clear();
						n0219.Clear();
						n0220.Clear();
						overlayText = null;
						gameMode = 1;
						director.n0249(fadeEnabled);
						director.n0250(sunEnabled);
						director.n0251(envEnabled);
						director.n0252(shockEnabled);
						director.SetDialogueColor(textColor);
						director.SetTextSpeed(textSpeed);
						director.n0254(voiceSynchro);
						director.SetPaused(false, false);
						scenarioTimer.Start();
						flag = true;
					}
					optionSelection = 0;
				}
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
					n0225();
				}
				catch
				{
				}
			}
			if (flag)
			{
				n0256();
			}
		}

		private void n0209(object n0035, MouseEventArgs n0036)
		{
			if (optionsFadeCounter != 0)
			{
				return;
			}
			if (n0036.Button == MouseButtons.Left)
			{
				lastCursorPosition = Point.Empty;
				n0213(n0035, n0036);
				if (optionSelection > 0)
				{
					n0180(optionSelection - 1);
				}
			}
			else if (n0036.Button == MouseButtons.Right)
			{
				n0189();
			}
		}

		private void n0213(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(lastCursorPosition))
			{
				return;
			}
			lastCursorPosition = Cursor.Position;
			if (optionsFadeCounter == 0)
			{
				for (int i = 0; i < 23; i++)
				{
					if (n0036.X >= optionLabelX[i] && n0036.X < optionLabelX[i] + optionSourceWidth[i] && n0036.Y >= optionLabelY[i] && n0036.Y < optionLabelY[i] + optionSourceHeight[i])
					{
						optionSelection = i + 1;
						return;
					}
				}
			}
			optionSelection = 0;
		}

		private void n0142()
		{
			episodeBackTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("SCUT_BG"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			episodeMenuTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("SCUTMENU"), episodeMenuSize.Width, episodeMenuSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0258()
		{
			episodeBackTexture.Dispose();
			episodeBackTexture = null;
			episodeMenuTexture.Dispose();
			episodeMenuTexture = null;
		}

		private void n0278()
		{
			gameMode = 2;
			episodeFadeCounter = 10;
			episodeFadeIn = true;
			episodeTimer.Start();
		}

		private void n0177()
		{
			episodeFadeCounter = 10;
			episodeFadeIn = false;
		}

		private void n0195(int n0277)
		{
			if (episodeFadeCounter > 0)
			{
				return;
			}
			switch (n0277)
			{
			case 0:
				if (episodeSelection == 0)
				{
					episodeSelection = 16;
					break;
				}
				episodeSelection--;
				if (episodeSelection == 0)
				{
					episodeSelection = 16;
				}
				break;
			case 1:
				if (episodeSelection == 0)
				{
					episodeSelection = 1;
					break;
				}
				episodeSelection++;
				if (episodeSelection == 17)
				{
					episodeSelection = 1;
				}
				break;
			}
		}

		private void n0113(object n0035, EventArgs n0036)
		{
			switch (PollJoystick())
			{
			case 0:
				if (episodeSelection > 0 && unlockedEpisodeCount >= episodeSelection)
				{
					n0177();
				}
				break;
			case 1:
				episodeSelection = 0;
				n0177();
				break;
			case 6:
				n0195(0);
				break;
			case 7:
				n0195(1);
				break;
			}
			int num = 0;
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			ArrayList arrayList = n0219;
			arrayList.Clear();
			n0220.Clear();
			overlayText = null;
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			QueueDraw(episodeBackTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < unlockedEpisodeCount; i++)
			{
				Rectangle n880 = new Rectangle((episodeSelection - 1 != i) ? 280 : 0, 28 * i, 280, 28);
				QueueDraw(episodeMenuTexture, n880, Rectangle.Empty, new Point(episodeColumnX[(i >= 8) ? 1 : 0], 80 + 40 * (i % 8)), Color.White);
			}
			for (int j = unlockedEpisodeCount; j < 16; j++)
			{
				Rectangle n881 = new Rectangle((episodeSelection - 1 != j) ? 280 : 0, 448, 280, 28);
				QueueDraw(episodeMenuTexture, n881, Rectangle.Empty, new Point(episodeColumnX[(j >= 8) ? 1 : 0], 80 + 40 * (j % 8)), Color.White);
			}
			if (episodeFadeCounter > 0)
			{
				int alpha = (int)((double)(episodeFadeIn ? episodeFadeCounter : (10 - episodeFadeCounter)) / 10.0 * 255.0);
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				episodeFadeCounter--;
				if (!episodeFadeIn && episodeFadeCounter == 0)
				{
					episodeTimer.Stop();
					if (episodeSelection == 0)
					{
						n0260();
					}
					else
					{
						num = episodeSelection;
					}
					episodeSelection = 0;
				}
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
					n0225();
				}
				catch
				{
				}
			}
			if (num > 0)
			{
				n0245(num);
			}
		}

		private void n0208(object n0035, MouseEventArgs n0036)
		{
			if (episodeFadeCounter != 0)
			{
				return;
			}
			if (n0036.Button == MouseButtons.Left)
			{
				lastCursorPosition = Point.Empty;
				n0212(n0035, n0036);
				if (episodeSelection > 0 && unlockedEpisodeCount >= episodeSelection)
				{
					n0177();
				}
			}
			else if (n0036.Button == MouseButtons.Right)
			{
				episodeSelection = 0;
				n0177();
			}
		}

		private void n0212(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(lastCursorPosition))
			{
				return;
			}
			lastCursorPosition = Cursor.Position;
			if (episodeFadeCounter != 0)
			{
				return;
			}
			episodeSelection = 0;
			for (int i = 0; i < 16; i++)
			{
				if (n0036.X > episodeColumnX[(i >= 8) ? 1 : 0] && n0036.X < episodeColumnX[(i >= 8) ? 1 : 0] + 280 && n0036.Y >= 80 + 40 * (i % 8) && n0036.Y < 80 + 40 * (i % 8) + 28)
				{
					episodeSelection = i + 1;
					break;
				}
			}
		}

		private void n0143()
		{
			albumBackTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("ALBUMBG"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			albumPictureTexture = TextureLoader.FromStream(device, systemArchive.ReadEntry("AL_PIC"), albumPictureSize.Width, albumPictureSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0259()
		{
			albumBackTexture.Dispose();
			albumBackTexture = null;
			albumPictureTexture.Dispose();
			albumPictureTexture = null;
		}

		private void n0279()
		{
			gameMode = 4;
			albumFadeCounter = 10;
			albumFadeIn = true;
			albumGroup = 0;
			albumImageIndex = 0;
			albumImageTransition = 0;
			albumPrevTexture = (albumCurrentTexture = null);
			d3dFont = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font(fontName, 24f));
			albumTimer.Start();
		}

		private void n0190()
		{
			albumFadeCounter = 10;
			albumFadeIn = false;
		}

		private void ShowAlbumImage(int n0308, int index)
		{
			if (index >= albumImageCounts[n0308 - 1])
			{
				n0191();
				return;
			}
			albumGroup = n0308;
			albumImageIndex = index;
			albumImageTransition = 10;
			if (albumPrevTexture != null)
			{
				albumPrevTexture.Dispose();
			}
			albumPrevTexture = albumCurrentTexture;
			albumCurrentTexture = TextureLoader.FromStream(device, bgArchive.ReadEntry("EV_HP0" + n0308 + (char)(index + 65)), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0191()
		{
			albumGroup = 0;
			albumImageIndex = 0;
			albumImageTransition = 10;
			if (albumPrevTexture != null)
			{
				albumPrevTexture.Dispose();
			}
			albumPrevTexture = albumCurrentTexture;
			albumCurrentTexture = null;
		}

		private void n0193(int n0277)
		{
			if (albumFadeCounter > 0 || albumImageTransition > 0)
			{
				return;
			}
			switch (n0277)
			{
			case 0:
				if (albumSelection == 0)
				{
					albumSelection = 7;
					break;
				}
				albumSelection--;
				if (albumSelection == 0)
				{
					albumSelection = 7;
				}
				break;
			case 1:
				if (albumSelection == 0)
				{
					albumSelection = 1;
					break;
				}
				albumSelection++;
				if (albumSelection == 8)
				{
					albumSelection = 1;
				}
				break;
			}
		}

		private void n0114(object n0035, EventArgs n0036)
		{
			switch (PollJoystick())
			{
			case 0:
				if (albumGroup == 0)
				{
					if (albumSelection > 0 && unlockedAlbumCount >= albumSelection)
					{
						ShowAlbumImage(albumSelection, 0);
					}
				}
				else
				{
					ShowAlbumImage(albumGroup, albumImageIndex + 1);
				}
				break;
			case 1:
				if (albumGroup == 0)
				{
					n0190();
				}
				else
				{
					n0191();
				}
				break;
			case 4:
				if (albumGroup == 0)
				{
					n0193(0);
				}
				break;
			case 5:
				if (albumGroup == 0)
				{
					n0193(1);
				}
				break;
			}
			device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			overlayText = null;
			device.BeginScene();
			sprite.Begin(SpriteFlags.AlphaBlend);
			if (albumGroup == 0 || (albumImageIndex == 0 && albumImageTransition > 0))
			{
				QueueDraw(albumBackTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
				for (int i = 0; i < unlockedAlbumCount; i++)
				{
					QueueDraw(albumPictureTexture, new Rectangle(0, 90 * i, 120, 90), Rectangle.Empty, new Point(albumThumbX[i], albumThumbY[i]), (albumSelection - 1 == i) ? Color.White : Color.FromArgb(albumThumbDimAlpha, Color.White));
				}
				for (int j = unlockedAlbumCount; j < 7; j++)
				{
					QueueDraw(albumPictureTexture, new Rectangle(0, 630, 120, 90), Rectangle.Empty, new Point(albumThumbX[j], albumThumbY[j]), (albumSelection - 1 == j) ? Color.White : Color.FromArgb(albumThumbDimAlpha, Color.White));
				}
				if (albumSelection > 0 && unlockedAlbumCount >= albumSelection)
				{
					int num = 0;
					while (num < albumTitles[albumSelection - 1].Length)
					{
						d3dFont.DrawText(sprite, albumTitles[albumSelection - 1][num].ToString(), textOrigin.X + charCellSize.Width * num + 2, textOrigin.Y + 2, Color.Black);
						d3dFont.DrawText(sprite, albumTitles[albumSelection - 1][num].ToString(), textOrigin.X + charCellSize.Width * num, textOrigin.Y, Color.White);
						int num2 = num;
						int num3 = 1;
						num = num2 + num3;
					}
					n0216.Add(null);
					n0217.Add(null);
					n0218.Add(null);
					n0219.Add(null);
					n0220.Add(null);
					overlayText = albumTitles[albumSelection - 1];
				}
			}
			else if (albumImageTransition == 0)
			{
				QueueDraw(albumCurrentTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
			else
			{
				QueueDraw(albumPrevTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
			if (albumFadeCounter > 0)
			{
				int alpha = (int)((double)(albumFadeIn ? albumFadeCounter : (10 - albumFadeCounter)) / 10.0 * 255.0);
				QueueDraw(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				albumFadeCounter--;
				if (!albumFadeIn && albumFadeCounter == 0)
				{
					albumTimer.Stop();
					n0260();
					albumSelection = 0;
				}
			}
			else if (albumImageTransition > 0)
			{
				if (albumGroup > 0)
				{
					QueueDraw(albumCurrentTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)((double)(10 - albumImageTransition) / 10.0 * 255.0), Color.White));
				}
				else
				{
					QueueDraw(albumPrevTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)((double)albumImageTransition / 10.0 * 255.0), Color.White));
				}
				albumImageTransition--;
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
					n0225();
				}
				catch
				{
				}
			}
			if (albumGroup == 0 && albumImageTransition == 0 && albumPrevTexture != null)
			{
				albumPrevTexture.Dispose();
				albumPrevTexture = null;
			}
		}

		private void n0210(object n0035, MouseEventArgs n0036)
		{
			if (albumFadeCounter != 0)
			{
				return;
			}
			if (n0036.Button == MouseButtons.Left)
			{
				if (albumGroup != 0)
				{
					ShowAlbumImage(albumGroup, albumImageIndex + 1);
					return;
				}
				lastCursorPosition = Point.Empty;
				n0214(n0035, n0036);
				if (albumSelection > 0 && unlockedAlbumCount >= albumSelection)
				{
					ShowAlbumImage(albumSelection, 0);
				}
			}
			else if (n0036.Button == MouseButtons.Right)
			{
				if (albumGroup == 0)
				{
					n0190();
				}
				else
				{
					n0191();
				}
			}
		}

		private void n0214(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(lastCursorPosition))
			{
				return;
			}
			lastCursorPosition = Cursor.Position;
			if (albumFadeCounter != 0)
			{
				return;
			}
			albumSelection = 0;
			for (int i = 0; i < 7; i++)
			{
				if (n0036.X > albumThumbX[i] && n0036.X < albumThumbX[i] + 120 && n0036.Y >= albumThumbY[i] && n0036.Y < albumThumbY[i] + 90)
				{
					albumSelection = i + 1;
					break;
				}
			}
		}
	}
}
