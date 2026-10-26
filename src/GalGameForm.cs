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

// 说明：.NET 1.1 的 C# 不支持 partial class，窗体初始化（InitializeComponent）与
// 游戏逻辑只能同处一个文件，因此本文件同时包含设计器代码与业务代码。
namespace Yukihota
{
	// Token: 0x02000017 RID: 23
	public class GalGameForm : Form
	{
		// Token: 0x06001EB8 RID: 7864 RVA: 0x0000D14C File Offset: 0x0000C14C
		public GalGameForm()
		{
			this.InitializeComponent();
			base.GotFocus += new EventHandler(this.OnGotFocus);
			base.LostFocus += new EventHandler(this.OnLostFocus);
			this.gameMode = -2;
			this.menuBgmPlayer = new MediaPackage("MENU");
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x0000D4FC File Offset: 0x0000C4FC
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x0000D51C File Offset: 0x0000C51C
		private void InitializeComponent()
		{
			this.components = new Container();
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(GalGameForm));
			this.scenarioTimer = new Timer(this.components);
			this.menuTimer = new Timer(this.components);
			this.optionsTimer = new Timer(this.components);
			this.contextMenu = new ContextMenu();
			this.menuCancelItem = new MenuItem();
			this.menuSeparator1 = new MenuItem();
			this.menuOptionsItem = new MenuItem();
			this.menuSeparator2 = new MenuItem();
			this.menuDisplayModeItem = new MenuItem();
			this.menuSeparator3 = new MenuItem();
			this.menuSkipItem = new MenuItem();
			this.menuAutoItem = new MenuItem();
			this.menuHideWindowItem = new MenuItem();
			this.menuSeparator4 = new MenuItem();
			this.menuLanguageItem = new MenuItem();
			this.menuSimplifiedItem = new MenuItem();
			this.menuTraditionalItem = new MenuItem();
			this.menuJapaneseItem = new MenuItem();
			this.menuSeparator5 = new MenuItem();
			this.menuFontItem = new MenuItem();
			this.menuSeparator6 = new MenuItem();
			this.menuBackToTitleItem = new MenuItem();
			this.menuSeparator7 = new MenuItem();
			this.menuExitItem = new MenuItem();
			this.fontDialog = new FontDialog();
			this.logoFadeTimer = new Timer(this.components);
			this.episodeTimer = new Timer(this.components);
			this.albumTimer = new Timer(this.components);
			this.movieBox = new PictureBox();
			base.SuspendLayout();
			this.scenarioTimer.Interval = 50;
			this.scenarioTimer.Tick += new EventHandler(this.OnScenarioTimerTick);
			this.menuTimer.Interval = 50;
			this.menuTimer.Tick += new EventHandler(this.OnMenuTimerTick);
			this.optionsTimer.Interval = 50;
			this.optionsTimer.Tick += new EventHandler(this.OnOptionsTimerTick);
			this.contextMenu.MenuItems.AddRange(new MenuItem[]
			{
				this.menuCancelItem, this.menuSeparator1, this.menuOptionsItem, this.menuSeparator2, this.menuDisplayModeItem, this.menuSeparator3, this.menuSkipItem, this.menuAutoItem, this.menuHideWindowItem, this.menuSeparator4,
				this.menuLanguageItem, this.menuSeparator5, this.menuFontItem, this.menuSeparator6, this.menuBackToTitleItem, this.menuSeparator7, this.menuExitItem
			});
			this.menuCancelItem.Index = 0;
			this.menuCancelItem.Text = "取消(&C)";
			this.menuCancelItem.Click += new EventHandler(this.OnMenuCancelClick);
			this.menuSeparator1.Index = 1;
			this.menuSeparator1.Text = "-";
			this.menuOptionsItem.Index = 2;
			this.menuOptionsItem.Text = "选项(&O)\tF8";
			this.menuOptionsItem.Click += new EventHandler(this.OnMenuOptionsClick);
			this.menuSeparator2.Index = 3;
			this.menuSeparator2.Text = "-";
			this.menuDisplayModeItem.Index = 4;
			this.menuDisplayModeItem.Text = "显示模式(&M)\tAlt+Enter";
			this.menuDisplayModeItem.Click += new EventHandler(this.OnMenuDisplayModeClick);
			this.menuSeparator3.Index = 5;
			this.menuSeparator3.Text = "-";
			this.menuSkipItem.Index = 6;
			this.menuSkipItem.Text = "快进(&Q)\tF3";
			this.menuSkipItem.Click += new EventHandler(this.OnMenuSkipClick);
			this.menuAutoItem.Index = 7;
			this.menuAutoItem.Text = "自动播放(&A)\tF4";
			this.menuAutoItem.Click += new EventHandler(this.OnMenuAutoClick);
			this.menuHideWindowItem.Index = 8;
			this.menuHideWindowItem.Text = "隐藏对话(&H)\tF2";
			this.menuHideWindowItem.Click += new EventHandler(this.OnMenuHideWindowClick);
			this.menuSeparator4.Index = 9;
			this.menuSeparator4.Text = "-";
			this.menuLanguageItem.Index = 10;
			this.menuLanguageItem.MenuItems.AddRange(new MenuItem[] { this.menuSimplifiedItem, this.menuTraditionalItem, this.menuJapaneseItem });
			this.menuLanguageItem.Text = "语言(&L)\tF12";
			this.menuSimplifiedItem.Index = 0;
			this.menuSimplifiedItem.RadioCheck = true;
			this.menuSimplifiedItem.Text = "简体中文(&S)";
			this.menuSimplifiedItem.Click += new EventHandler(this.OnMenuSimplifiedClick);
			this.menuTraditionalItem.Index = 1;
			this.menuTraditionalItem.RadioCheck = true;
			this.menuTraditionalItem.Text = "繁體中文(&T)";
			this.menuTraditionalItem.Click += new EventHandler(this.OnMenuTraditionalClick);
			this.menuJapaneseItem.Enabled = false;
			this.menuJapaneseItem.Index = 2;
			this.menuJapaneseItem.RadioCheck = true;
			this.menuJapaneseItem.Text = "日本語(&J)";
			this.menuJapaneseItem.Click += new EventHandler(this.OnMenuJapaneseClick);
			this.menuSeparator5.Index = 11;
			this.menuSeparator5.Text = "-";
			this.menuFontItem.Index = 12;
			this.menuFontItem.Text = "字体(&F)";
			this.menuFontItem.Click += new EventHandler(this.OnMenuFontClick);
			this.menuSeparator6.Index = 13;
			this.menuSeparator6.Text = "-";
			this.menuBackToTitleItem.Index = 14;
			this.menuBackToTitleItem.Text = "返回主菜单(&B)";
			this.menuBackToTitleItem.Click += new EventHandler(this.OnMenuBackToTitleClick);
			this.menuSeparator7.Index = 15;
			this.menuSeparator7.Text = "-";
			this.menuExitItem.Index = 16;
			this.menuExitItem.Text = "结束游戏(&E)\tAlt+F4";
			this.menuExitItem.Click += new EventHandler(this.OnMenuExitClick);
			this.fontDialog.AllowVerticalFonts = false;
			this.fontDialog.Font = new System.Drawing.Font("黑体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.fontDialog.FontMustExist = true;
			this.fontDialog.MaxSize = 24;
			this.fontDialog.MinSize = 24;
			this.fontDialog.ShowEffects = false;
			this.logoFadeTimer.Interval = 50;
			this.logoFadeTimer.Tick += new EventHandler(this.OnLogoFadeTimerTick);
			this.episodeTimer.Interval = 50;
			this.episodeTimer.Tick += new EventHandler(this.OnEpisodeTimerTick);
			this.albumTimer.Interval = 50;
			this.albumTimer.Tick += new EventHandler(this.OnAlbumTimerTick);
			this.movieBox.BackColor = Color.White;
			this.movieBox.Location = new Point(0, 0);
			this.movieBox.Name = "ptbMovie";
			this.movieBox.Size = new Size(800, 600);
			this.movieBox.TabIndex = 0;
			this.movieBox.TabStop = false;
			this.movieBox.Visible = false;
			this.AutoScaleBaseSize = new Size(6, 14);
			this.BackColor = Color.White;
			base.ClientSize = new Size(800, 600);
			base.Controls.Add(this.movieBox);
			this.Font = new System.Drawing.Font("黑体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Icon = (Icon)resourceManager.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.MaximizeBox = false;
			base.Name = "GalGame";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "秋之回忆 ～雪萤～";
			base.KeyDown += new KeyEventHandler(this.OnKeyDown);
			base.MouseDown += new MouseEventHandler(this.OnMouseDown);
			base.Closing += new CancelEventHandler(this.OnClosing);
			base.Load += new EventHandler(this.OnLoad);
			base.KeyUp += new KeyEventHandler(this.OnKeyUp);
			base.Activated += new EventHandler(this.OnActivated);
			base.Paint += new PaintEventHandler(this.OnPaint);
			base.MouseMove += new MouseEventHandler(this.OnMouseMove);
			base.ResumeLayout(false);
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x0000DEC8 File Offset: 0x0000CEC8
		// 原始方法名被混淆为单字符（元数据 entrypoint token 指向它）。
		// C# 要求入口方法必须叫 Main，故按语言要求恢复该方法名，签名与属性不变。
		[STAThread]
		private static void Main(string[] args)
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
			bool running = true;
			GalGameForm.noOperationMode = false;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i] == "/nocopyright")
				{
					running = false;
				}
				if (args[i] == "/noop")
				{
					GalGameForm.noOperationMode = true;
				}
			}
			if (running)
			{
				MessageBox.Show("本游戏为KID Fans Club特别创作组免费制作并发布。\n仅供广大KID Fans试玩，请于下载后24小时自觉删除，\n任何个人或组织不得将其用于任何形式的商业用途，\n违者如造成商业侵权，KID Fans Club将不负连带责任。\n相关说明见Readme.txt\n\n本遊戲為KID Fans Club特別創作組免費製作並發佈。\n僅供廣大KID Fans試玩，請於下載後24小時自覺刪除，\n任何個人或組織不得將其用於任何形式的商業用途，\n違者如造成商業侵權，KID Fans Club將不負連帶責任。\n相關說明見Readme.txt");
			}
			Application.Run(new GalGameForm());
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x0000DF7C File Offset: 0x0000CF7C
		protected override void WndProc(ref Message message)
		{
			if (message.Msg == 274 && ((int)message.WParam == 61760 || (int)message.WParam == 61808))
			{
				message.Result = (IntPtr)1;
				return;
			}
			base.WndProc(ref message);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0000DFD0 File Offset: 0x0000CFD0
		private void OnLoad(object sender, EventArgs e)
		{
			this.LoadSettings();
			OptionsForm OptionsForm = new OptionsForm();
			OptionsForm.SetFontName(new System.Drawing.Font(this.fontName, 24f));
			OptionsForm.SetFullscreen(this.fullscreen);
			OptionsForm.SetButtonLayout(this.language);
			OptionsForm.SetShowOptions(this.showOptionsOnStartup);
			if (this.showOptionsOnStartup && OptionsForm.ShowDialog() == DialogResult.Cancel)
			{
				Application.Exit();
				return;
			}
			this.fontName = OptionsForm.GetFontName().Name;
			this.fullscreen = OptionsForm.GetFullscreen();
			this.language = OptionsForm.GetButtonLayout();
			this.showOptionsOnStartup = OptionsForm.GetShowOptions();
			ArchiveFile.VerifyKey("Mullen");
			for (int i = 0; i < 8; i++)
			{
				this.joystickButtonStates[i] = false;
			}
			this.joystick = OptionsForm.GetDevice();
			if (this.joystick != null)
			{
				try
				{
					this.joystick.SetDataFormat(DeviceDataFormat.Joystick);
					this.joystick.SetCooperativeLevel(this, Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Exclusive);
					foreach (object obj in this.joystick.Objects)
					{
						DeviceObjectInstance deviceObjectInstance = (DeviceObjectInstance)obj;
						if ((deviceObjectInstance.ObjectId & 3) != 0)
						{
							this.joystick.Properties.SetRange(ParameterHow.ById, deviceObjectInstance.ObjectId, new InputRange(-1000, 1000));
						}
					}
					if (OptionsForm.GetForceFeedback())
					{
						int[] axes = null;
						foreach (object obj2 in this.joystick.GetObjects(DeviceObjectTypeFlags.Axis))
						{
							DeviceObjectInstance deviceObjectInstance2 = (DeviceObjectInstance)obj2;
							if ((deviceObjectInstance2.Flags & 1) != 0)
							{
								if (axes != null)
								{
									int[] grownAxes = new int[axes.Length + 1];
									axes.CopyTo(grownAxes, 0);
									axes = grownAxes;
								}
								else
								{
									axes = new int[1];
								}
								axes[axes.Length - 1] = deviceObjectInstance2.Offset;
								if (2 == axes.Length)
								{
									break;
								}
							}
						}
						IEnumerator enumerator = this.joystick.GetEffects(EffectType.All).GetEnumerator();
						{
							while (enumerator.MoveNext())
							{
								object obj3 = enumerator.Current;
								EffectInformation effectInformation = (EffectInformation)obj3;
								if (DInputHelper.GetTypeCode(effectInformation.EffectType) == 1)
								{
									Microsoft.DirectX.DirectInput.Effect effect = new Microsoft.DirectX.DirectInput.Effect();
									effect.SetDirection(new int[axes.Length]);
									effect.SetAxes(new int[axes.Length]);
									effect.EffectType = EffectType.ConstantForce;
									effect.ConditionStruct = new Condition[axes.Length];
									effect.Duration = -1;
									effect.Gain = 10000;
									effect.SamplePeriod = 0;
									effect.TriggerButton = -1;
									effect.TriggerRepeatInterval = -1;
									effect.Flags = EffectFlags.Cartesian | EffectFlags.ObjectOffsets;
									effect.Constant.Magnitude = 5000;
									effect.SetAxes(axes);
									this.forceFeedbackEffect = new EffectObject(effectInformation.EffectGuid, effect, this.joystick);
								}
							}
							goto IL_0503;
						}
					}
					this.forceFeedbackEffect = null;
					IL_0503:;
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
				this.device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, this, Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing, new PresentParameters[] { presentParameters });
				goto IL_00D2;
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
				Application.Exit();
				return;
			}
			IL_005F:
			ButtonLayout ButtonLayout;
			switch (ButtonLayout)
			{
			case ButtonLayout.SimplifiedChinese:
				this.menuSimplifiedItem.Checked = true;
				break;
			case ButtonLayout.TraditionalChinese:
				this.menuTraditionalItem.Checked = true;
				break;
			case ButtonLayout.Japanese:
				this.menuJapaneseItem.Checked = true;
				break;
			}
			this.lastCursorPosition = Cursor.Position;
			this.whiteTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("WHITE"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			try
			{
				this.InitOpeningMovie();
				this.LoadTitleResources();
				this.LoadOptionsResources();
				this.LoadLogoTexture();
				this.LoadEpisodeResources();
				this.LoadAlbumResources();
			}
			catch (Exception ex3)
			{
				MessageBox.Show(ex3.Message);
				Application.Exit();
			}
			return;
			IL_00D2:
			this.sprite = new Sprite(this.device);
			this.windowedLocation = base.Location;
			this.displayDevice = new Microsoft.DirectX.DirectDraw.Device();
			this.ApplyFullscreen(OptionsForm.GetFullscreen());
			this.isBorderless = false;
			this.suppressLostFocus = false;
			try
			{
				this.systemArchive = new ArchiveFile("System", ArchiveMode.Read);
				this.bgmArchive = new ArchiveFile("Bgm", ArchiveMode.Read);
				this.bgArchive = new ArchiveFile("Bg", ArchiveMode.Read);
			}
			catch (Exception ex4)
			{
				MessageBox.Show(ex4.Message);
				Application.Exit();
				return;
			}
			try
			{
				string text = Path.GetTempPath();
				text = text.Substring(0, text.Length - 1);
				this.bgmArchive.Extract("BGM12", text);
				this.menuBgmPlayer.Open(text + "\\BGM12.wav");
			}
			catch (Exception ex5)
			{
				MessageBox.Show(ex5.Message);
				Application.Exit();
				return;
			}
			this.ApplyLanguage(this.language);
			ButtonLayout = this.language;
			goto IL_005F;
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x0000E614 File Offset: 0x0000D614
		private void OnClosing(object sender, CancelEventArgs e)
		{
			if (this.gameMode == 1)
			{
				if (this.director.GetEpisode() > this.unlockedEpisodeCount)
				{
					this.unlockedEpisodeCount = this.director.GetEpisode();
				}
				if (this.director.GetAlbumIndex() > this.unlockedAlbumCount)
				{
					this.unlockedAlbumCount = this.director.GetAlbumIndex();
				}
			}
			this.SaveSettings();
			if (this.joystick != null)
			{
				try
				{
					if (null != this.forceFeedbackEffect)
					{
						this.forceFeedbackEffect.Stop();
					}
					this.joystick.Unacquire();
					this.joystick.Properties.AutoCenter = true;
				}
				catch (InputException)
				{
				}
			}
			string tempPath = Path.GetTempPath();
			this.menuBgmPlayer.Close();
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

		// Token: 0x06001EBF RID: 7871 RVA: 0x0000E738 File Offset: 0x0000D738
		private void OnActivated(object sender, EventArgs e)
		{
			if (this.joystick != null)
			{
				try
				{
					this.joystick.Acquire();
				}
				catch (InputException)
				{
				}
			}
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x0000E780 File Offset: 0x0000D780
		private void OnGotFocus(object sender, EventArgs e)
		{
			if (this.isBorderless)
			{
				this.ApplyFullscreen(true);
				base.WindowState = FormWindowState.Normal;
			}
			switch (this.gameMode)
			{
			case -2:
				if (this.openingMovie != null)
				{
					this.openingMovie.Play();
					return;
				}
				break;
			case -1:
				this.logoFadeTimer.Start();
				return;
			case 0:
				this.menuTimer.Start();
				this.menuBgmPlayer.Resume();
				return;
			case 1:
				this.scenarioTimer.Start();
				this.director.SetPaused(false, true);
				return;
			case 2:
				this.episodeTimer.Start();
				this.menuBgmPlayer.Resume();
				return;
			case 3:
				this.optionsTimer.Start();
				if (this.returnMode == 0)
				{
					this.menuBgmPlayer.Resume();
					return;
				}
				if (this.returnMode == 1)
				{
					this.director.SetPaused(false, true);
					return;
				}
				break;
			case 4:
				this.albumTimer.Start();
				this.menuBgmPlayer.Resume();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x0000E8A4 File Offset: 0x0000D8A4
		protected override void OnLostFocus(EventArgs e)
		{
			if (this.suppressLostFocus)
			{
				return;
			}
			base.OnLostFocus(e);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x0000E8B8 File Offset: 0x0000D8B8
		private void OnLostFocus(object sender, EventArgs e)
		{
			this.isBorderless = base.FormBorderStyle == FormBorderStyle.None;
			if (this.isBorderless)
			{
				this.ApplyFullscreen(false);
				base.WindowState = FormWindowState.Minimized;
			}
			switch (this.gameMode)
			{
			case -2:
				if (this.openingMovie != null)
				{
					this.openingMovie.Pause();
					return;
				}
				break;
			case -1:
				this.logoFadeTimer.Stop();
				return;
			case 0:
				this.menuTimer.Stop();
				this.menuBgmPlayer.Pause();
				return;
			case 1:
				this.scenarioTimer.Stop();
				this.director.SetPaused(true, true);
				return;
			case 2:
				this.episodeTimer.Stop();
				this.menuBgmPlayer.Pause();
				return;
			case 3:
				this.optionsTimer.Stop();
				if (this.returnMode == 0)
				{
					this.menuBgmPlayer.Pause();
					return;
				}
				if (this.returnMode == 1)
				{
					this.director.SetPaused(true, true);
					return;
				}
				break;
			case 4:
				this.albumTimer.Stop();
				this.menuBgmPlayer.Pause();
				break;
			default:
				return;
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0000E9DC File Offset: 0x0000D9DC
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode <= Keys.ControlKey)
			{
				if (keyCode != Keys.Return)
				{
					if (keyCode != Keys.ControlKey)
					{
						return;
					}
					if (this.gameMode == 1)
					{
						this.director.SetSkipMode(true, true);
						return;
					}
				}
				else
				{
					if (this.enterKeyDown)
					{
						return;
					}
					this.enterKeyDown = true;
					if (e.Alt)
					{
						this.ApplyFullscreen();
						return;
					}
					switch (this.gameMode)
					{
					case 0:
						if (this.menuFadeCounter == 0 && this.menuSelection > 0 && this.menuItemEnabled[this.menuSelection - 1])
						{
							this.CloseMenu();
							return;
						}
						break;
					case 1:
						this.director.OnClick();
						return;
					case 2:
						if (this.episodeFadeCounter == 0 && this.episodeSelection > 0 && this.unlockedEpisodeCount >= this.episodeSelection)
						{
							this.CloseEpisodeSelect();
							return;
						}
						break;
					case 3:
						if (this.optionsFadeCounter == 0 && this.optionSelection > 0)
						{
							this.ApplyOptionValue(this.optionSelection - 1);
							return;
						}
						break;
					case 4:
						if (this.albumFadeCounter == 0 && this.albumImageTransition == 0)
						{
							if (this.albumGroup != 0)
							{
								this.ShowAlbumImage(this.albumGroup, this.albumImageIndex + 1);
								return;
							}
							if (this.albumSelection > 0 && this.unlockedAlbumCount >= this.albumSelection)
							{
								this.ShowAlbumImage(this.albumSelection, 0);
								return;
							}
						}
						break;
					default:
						return;
					}
				}
			}
			else
			{
				switch (keyCode)
				{
				case Keys.Space:
					if (this.spaceKeyDown)
					{
						return;
					}
					this.spaceKeyDown = true;
					switch (this.gameMode)
					{
					case 1:
						if (!this.director.IsStaffRollPlaying())
						{
							this.contextMenu.Show(this, new Point(400, 300));
							return;
						}
						break;
					case 2:
						if (this.episodeFadeCounter == 0)
						{
							this.episodeSelection = 0;
							this.CloseEpisodeSelect();
							return;
						}
						break;
					case 3:
						if (this.optionsFadeCounter == 0)
						{
							this.CloseOptions();
							return;
						}
						break;
					case 4:
						if (this.albumFadeCounter == 0 && this.albumImageTransition == 0)
						{
							if (this.albumGroup == 0)
							{
								this.CloseAlbum();
								return;
							}
							this.BackToAlbumGrid();
							return;
						}
						break;
					default:
						return;
					}
					break;
				case Keys.Prior:
				case Keys.Next:
				case Keys.End:
				case Keys.Home:
					break;
				case Keys.Left:
					switch (this.gameMode)
					{
					case 3:
						this.MoveOptionSelection(0);
						return;
					case 4:
						if (this.albumGroup == 0)
						{
							this.MoveAlbumSelection(0);
							return;
						}
						break;
					default:
						return;
					}
					break;
				case Keys.Up:
					switch (this.gameMode)
					{
					case 0:
						this.MoveMenuSelection(0);
						return;
					case 1:
						break;
					case 2:
						this.MoveEpisodeSelection(0);
						return;
					case 3:
						this.MoveOptionSelection(2);
						return;
					default:
						return;
					}
					break;
				case Keys.Right:
					switch (this.gameMode)
					{
					case 3:
						this.MoveOptionSelection(1);
						return;
					case 4:
						if (this.albumGroup == 0)
						{
							this.MoveAlbumSelection(1);
							return;
						}
						break;
					default:
						return;
					}
					break;
				case Keys.Down:
					switch (this.gameMode)
					{
					case 0:
						this.MoveMenuSelection(1);
						return;
					case 1:
						break;
					case 2:
						this.MoveEpisodeSelection(1);
						return;
					case 3:
						this.MoveOptionSelection(3);
						return;
					default:
						return;
					}
					break;
				default:
					switch (keyCode)
					{
					case Keys.F2:
						if (this.f2KeyDown)
						{
							return;
						}
						this.f2KeyDown = true;
						if (this.gameMode == 1)
						{
							this.director.ToggleMessageWindow();
							return;
						}
						break;
					case Keys.F3:
						if (this.f3KeyDown)
						{
							return;
						}
						this.f3KeyDown = true;
						if (this.gameMode == 1)
						{
							this.director.SetSkipMode();
							return;
						}
						break;
					case Keys.F4:
						if (this.f4KeyDown)
						{
							return;
						}
						this.f4KeyDown = true;
						if (this.gameMode == 1)
						{
							this.director.SetAutoMode();
							return;
						}
						break;
					case Keys.F5:
					case Keys.F6:
					case Keys.F7:
						break;
					case Keys.F8:
						if (this.f8KeyDown)
						{
							return;
						}
						this.f8KeyDown = true;
						if (this.gameMode == 1 && !this.director.IsStaffRollPlaying())
						{
							this.director.SetAutoMode(false);
							this.director.SetPaused(true, false);
							this.scenarioTimer.Stop();
							this.LoadOptionsResources();
							this.OpenOptions();
							return;
						}
						break;
					default:
						if (keyCode != Keys.F12)
						{
							return;
						}
						if (this.f12KeyDown)
						{
							return;
						}
						this.f12KeyDown = true;
						if (this.gameMode == 1)
						{
							this.language++;
							if (this.language == ButtonLayout.LanguageCount)
							{
								this.language = ButtonLayout.SimplifiedChinese;
							}
							if (!this.director.IsStaffRollPlaying() && this.language == ButtonLayout.Japanese)
							{
								this.language = ButtonLayout.SimplifiedChinese;
							}
							if (this.director.IsStaffRollPlaying())
							{
								this.languageResetPending = true;
							}
							this.menuSimplifiedItem.Checked = false;
							this.menuTraditionalItem.Checked = false;
							this.menuJapaneseItem.Checked = false;
							switch (this.language)
							{
							case ButtonLayout.SimplifiedChinese:
								this.menuSimplifiedItem.Checked = true;
								break;
							case ButtonLayout.TraditionalChinese:
								this.menuTraditionalItem.Checked = true;
								break;
							case ButtonLayout.Japanese:
								this.menuTraditionalItem.Checked = true;
								break;
							}
							this.ApplyLanguage(this.language);
							this.director.SetButtonLayout(this.language);
						}
						break;
					}
					break;
				}
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0000EF98 File Offset: 0x0000DF98
		private void OnKeyUp(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode <= Keys.ControlKey)
			{
				if (keyCode == Keys.Return)
				{
					this.enterKeyDown = false;
					return;
				}
				if (keyCode != Keys.ControlKey)
				{
					return;
				}
				if (this.gameMode == 1)
				{
					this.director.SetSkipMode(false, true);
					return;
				}
			}
			else
			{
				if (keyCode == Keys.Space)
				{
					this.spaceKeyDown = false;
					return;
				}
				switch (keyCode)
				{
				case Keys.F2:
					this.f2KeyDown = false;
					return;
				case Keys.F3:
					this.f3KeyDown = false;
					return;
				case Keys.F4:
					this.f4KeyDown = false;
					return;
				case Keys.F5:
				case Keys.F6:
				case Keys.F7:
					break;
				case Keys.F8:
					this.f8KeyDown = false;
					return;
				default:
					if (keyCode != Keys.F12)
					{
						return;
					}
					this.f12KeyDown = false;
					break;
				}
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x0000F044 File Offset: 0x0000E044
		private void OnMouseDown(object sender, MouseEventArgs e)
		{
			switch (this.gameMode)
			{
			case 0:
				this.OnMenuMouseDown(sender, e);
				return;
			case 1:
				if (e.Button == MouseButtons.Left)
				{
					this.director.OnClick();
					return;
				}
				if (e.Button == MouseButtons.Right && !this.director.IsStaffRollPlaying())
				{
					this.director.SetPaused(true, false);
					this.scenarioTimer.Stop();
					this.contextMenu.Show((Control)sender, new Point(e.X, e.Y));
					this.director.SetPaused(false, false);
					this.scenarioTimer.Start();
					return;
				}
				break;
			case 2:
				this.OnEpisodeMouseDown(sender, e);
				return;
			case 3:
				this.OnOptionsMouseDown(sender, e);
				return;
			case 4:
				this.OnAlbumMouseDown(sender, e);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0000F128 File Offset: 0x0000E128
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			switch (this.gameMode)
			{
			case 0:
				this.OnMenuMouseMove(sender, e);
				return;
			case 1:
				break;
			case 2:
				this.OnEpisodeMouseMove(sender, e);
				return;
			case 3:
				this.OnOptionsMouseMove(sender, e);
				return;
			case 4:
				this.OnAlbumMouseMove(sender, e);
				break;
			default:
				return;
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x0000F17C File Offset: 0x0000E17C
		private void OnPaint(object sender, PaintEventArgs e)
		{
			if (this.gameMode == -2)
			{
				return;
			}
			if (this.gameMode == 1)
			{
				this.director.RenderFrame();
				return;
			}
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			for (int i = 0; i < this.layerTextures.Count; i++)
			{
				if (this.layerTextures[i] != null)
				{
					this.sprite.Draw2D((Texture)this.layerTextures[i], (Rectangle)this.layerSourceRects[i], (Rectangle)this.layerDestRects[i], Point.Empty, 0f, (Point)this.layerPositions[i], (Color)this.layerColors[i]);
				}
				else
				{
					int j = 0;
					while (j < this.overlayText.Length)
					{
						Microsoft.DirectX.Direct3D.Font font = this.d3dFont;
						Sprite sprite = this.sprite;
						string text = this.overlayText[j].ToString();
						int x = this.textOrigin.X;
						Size size = this.charCellSize;
						font.DrawText(sprite, text, x + size.Width * j + 2, this.textOrigin.Y + 2, Color.Black);
						Microsoft.DirectX.Direct3D.Font font2 = this.d3dFont;
						Sprite sprite2 = this.sprite;
						string text2 = this.overlayText[j].ToString();
						int x2 = this.textOrigin.X;
						size = this.charCellSize;
						font2.DrawText(sprite2, text2, x2 + size.Width * j, this.textOrigin.Y, Color.White);
						int index = j++;
					}
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
					this.ResetDevice();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0000F41C File Offset: 0x0000E41C
		private void OnScenarioTimerTick(object sender, EventArgs e)
		{
			bool hasInput = false;
			int joystickState = this.PollJoystick();
			if (joystickState >= 0)
			{
				switch (joystickState)
				{
				case 0:
					this.director.OnClick();
					break;
				case 1:
					this.director.ToggleMessageWindow();
					break;
				case 2:
					this.director.SetSkipMode();
					break;
				case 3:
					this.director.SetAutoMode();
					break;
				case 8:
					if (!this.director.IsStaffRollPlaying())
					{
						this.director.SetAutoMode(false);
						this.director.SetPaused(true, false);
						this.scenarioTimer.Stop();
						this.LoadOptionsResources();
						this.OpenOptions();
						hasInput = true;
					}
					break;
				case 9:
					if (!this.director.IsStaffRollPlaying())
					{
						this.ReturnToTitle();
						hasInput = true;
					}
					break;
				}
			}
			if (this.languageResetPending && this.language == ButtonLayout.Japanese && !this.director.IsStaffRollPlaying())
			{
				this.language = ButtonLayout.SimplifiedChinese;
				this.menuSimplifiedItem.Checked = true;
				this.menuTraditionalItem.Checked = false;
				this.menuJapaneseItem.Checked = false;
				this.ApplyLanguage(this.language);
				this.director.SetButtonLayout(this.language);
				this.languageResetPending = false;
			}
			if (!hasInput && !this.director.Update())
			{
				this.ReturnToTitle();
			}
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x0000F5AC File Offset: 0x0000E5AC
		private void OnMenuCancelClick(object sender, EventArgs e)
		{
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0000F5B0 File Offset: 0x0000E5B0
		private void OnMenuOptionsClick(object sender, EventArgs e)
		{
			this.director.SetAutoMode(false);
			this.director.SetPaused(true, false);
			this.scenarioTimer.Stop();
			this.LoadOptionsResources();
			this.OpenOptions();
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x0000F5E4 File Offset: 0x0000E5E4
		private void OnMenuDisplayModeClick(object sender, EventArgs e)
		{
			this.ApplyFullscreen();
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0000F5EC File Offset: 0x0000E5EC
		private void OnMenuSkipClick(object sender, EventArgs e)
		{
			this.director.SetSkipMode();
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x0000F5FC File Offset: 0x0000E5FC
		private void OnMenuAutoClick(object sender, EventArgs e)
		{
			this.director.SetAutoMode();
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0000F60C File Offset: 0x0000E60C
		private void OnMenuHideWindowClick(object sender, EventArgs e)
		{
			this.director.ToggleMessageWindow();
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x0000F61C File Offset: 0x0000E61C
		private void OnMenuSimplifiedClick(object sender, EventArgs e)
		{
			this.menuSimplifiedItem.Checked = true;
			this.menuTraditionalItem.Checked = false;
			this.menuJapaneseItem.Checked = false;
			this.language = ButtonLayout.SimplifiedChinese;
			this.ApplyLanguage(ButtonLayout.SimplifiedChinese);
			this.director.SetButtonLayout(ButtonLayout.SimplifiedChinese);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x0000F65C File Offset: 0x0000E65C
		private void OnMenuTraditionalClick(object sender, EventArgs e)
		{
			this.menuSimplifiedItem.Checked = false;
			this.menuTraditionalItem.Checked = true;
			this.menuJapaneseItem.Checked = false;
			this.language = ButtonLayout.TraditionalChinese;
			this.ApplyLanguage(ButtonLayout.TraditionalChinese);
			this.director.SetButtonLayout(ButtonLayout.TraditionalChinese);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0000F69C File Offset: 0x0000E69C
		private void OnMenuJapaneseClick(object sender, EventArgs e)
		{
			this.menuSimplifiedItem.Checked = false;
			this.menuTraditionalItem.Checked = false;
			this.menuJapaneseItem.Checked = true;
			this.language = ButtonLayout.Japanese;
			this.ApplyLanguage(ButtonLayout.Japanese);
			this.director.SetButtonLayout(ButtonLayout.Japanese);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0000F6DC File Offset: 0x0000E6DC
		private void OnMenuFontClick(object sender, EventArgs e)
		{
			if (this.fontDialog.ShowDialog() == DialogResult.OK)
			{
				this.director.SetFont(this.fontDialog.Font);
				this.fontName = this.fontDialog.Font.Name;
			}
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0000F718 File Offset: 0x0000E718
		private void OnMenuBackToTitleClick(object sender, EventArgs e)
		{
			this.ReturnToTitle();
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0000F720 File Offset: 0x0000E720
		private void OnMenuExitClick(object sender, EventArgs e)
		{
			this.director.Stop();
			this.director.Dispose();
			base.Close();
			Application.Exit();
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0000F744 File Offset: 0x0000E744
		private void LoadSettings()
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
			this.ReadSetting(xmlDocument, "sys/win/font", ref this.fontName);
			string text = null;
			this.ReadSetting(xmlDocument, "sys/win/mode", ref text);
			if (text != null)
			{
				if (text == "Fullscreen")
				{
					this.fullscreen = true;
				}
				else if (text == "Windowed")
				{
					this.fullscreen = false;
				}
			}
			text = null;
			this.ReadSetting(xmlDocument, "sys/win/language", ref text);
			if (text != null)
			{
				if (text == "SC")
				{
					this.language = ButtonLayout.SimplifiedChinese;
				}
				else if (text == "TC")
				{
					this.language = ButtonLayout.TraditionalChinese;
				}
				else if (text == "JP")
				{
					this.language = ButtonLayout.Japanese;
				}
			}
			this.ReadSetting(xmlDocument, "sys/win/show", ref this.showOptionsOnStartup);
			this.ReadSetting(xmlDocument, "sys/options/fade", ref this.fadeEnabled);
			this.ReadSetting(xmlDocument, "sys/options/env", ref this.envEnabled);
			this.ReadSetting(xmlDocument, "sys/options/sun", ref this.sunEnabled);
			this.ReadSetting(xmlDocument, "sys/options/shock", ref this.shockEnabled);
			text = null;
			this.ReadSetting(xmlDocument, "sys/options/color", ref text);
			if (text != null)
			{
				if (text == "Green")
				{
					this.textColor = Color.FromArgb(128, 255, 128);
				}
				else if (text == "Blue")
				{
					this.textColor = Color.FromArgb(128, 128, 255);
				}
				else if (text == "Red")
				{
					this.textColor = Color.FromArgb(255, 128, 128);
				}
				else if (text == "Yellow")
				{
					this.textColor = Color.FromArgb(255, 255, 128);
				}
				else if (text == "Purple")
				{
					this.textColor = Color.FromArgb(255, 128, 255);
				}
				else if (text == "Cyan")
				{
					this.textColor = Color.FromArgb(128, 255, 255);
				}
				else if (text == "Black")
				{
					this.textColor = Color.FromArgb(128, 128, 128);
				}
			}
			this.ReadSetting(xmlDocument, "sys/options/speed", 1, 6, ref this.textSpeed);
			this.ReadSetting(xmlDocument, "sys/options/synchro", ref this.voiceSynchro);
			this.ReadSetting(xmlDocument, "sys/save/episode", 0, 16, ref this.unlockedEpisodeCount);
			this.ReadSetting(xmlDocument, "sys/save/album", 0, 7, ref this.unlockedAlbumCount);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0000FA64 File Offset: 0x0000EA64
		private void SaveSettings()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("sys"));
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("win"));
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("font")).InnerText = this.fontName;
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("mode")).InnerText = (this.fullscreen ? "Fullscreen" : "Windowed");
			switch (this.language)
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
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("show")).InnerText = this.showOptionsOnStartup.ToString();
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("options"));
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("fade")).InnerText = this.fadeEnabled.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("env")).InnerText = this.envEnabled.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("sun")).InnerText = this.sunEnabled.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("shock")).InnerText = this.shockEnabled.ToString();
			string text;
			if (this.textColor == Color.FromArgb(128, 255, 128))
			{
				text = "Green";
			}
			else if (this.textColor == Color.FromArgb(128, 128, 255))
			{
				text = "Blue";
			}
			else if (this.textColor == Color.FromArgb(255, 128, 128))
			{
				text = "Red";
			}
			else if (this.textColor == Color.FromArgb(255, 255, 128))
			{
				text = "Yellow";
			}
			else if (this.textColor == Color.FromArgb(255, 128, 255))
			{
				text = "Purple";
			}
			else if (this.textColor == Color.FromArgb(128, 255, 255))
			{
				text = "Cyan";
			}
			else if (this.textColor == Color.FromArgb(128, 128, 128))
			{
				text = "Black";
			}
			else
			{
				text = "Blue";
			}
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("color")).InnerText = text;
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("speed")).InnerText = this.textSpeed.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("synchro")).InnerText = this.voiceSynchro.ToString();
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("save"));
			xmlDocument.SelectSingleNode("/sys/save").AppendChild(xmlDocument.CreateElement("episode")).InnerText = this.unlockedEpisodeCount.ToString();
			xmlDocument.SelectSingleNode("/sys/save").AppendChild(xmlDocument.CreateElement("album")).InnerText = this.unlockedAlbumCount.ToString();
			try
			{
				xmlDocument.Save("save_sys.dat");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n游戏保存可能失败");
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0000FF2C File Offset: 0x0000EF2C
		private void ReadSetting(XmlDocument document, string xpath, ref string value)
		{
			XmlNode xmlNode = document.SelectSingleNode(xpath);
			if (xmlNode != null)
			{
				value = xmlNode.InnerText;
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0000FF4C File Offset: 0x0000EF4C
		private void ReadSetting(XmlDocument document, string xpath, ref bool value)
		{
			XmlNode xmlNode = document.SelectSingleNode(xpath);
			if (xmlNode != null)
			{
				if (xmlNode.InnerText == "True")
				{
					value = true;
					return;
				}
				if (xmlNode.InnerText == "False")
				{
					value = false;
				}
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0000FF90 File Offset: 0x0000EF90
		private void ReadSetting(XmlDocument document, string xpath, int min, int max, ref int value)
		{
			XmlNode xmlNode = document.SelectSingleNode(xpath);
			if (xmlNode != null)
			{
				try
				{
					int parsedValue = int.Parse(xmlNode.InnerText);
					if (parsedValue >= min && parsedValue <= max)
					{
						value = parsedValue;
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x0000FFE4 File Offset: 0x0000EFE4
		private void ResetDevice(bool fullscreenEnabled)
		{
			PresentParameters presentParameters = new PresentParameters();
			presentParameters.SwapEffect = SwapEffect.Flip;
			presentParameters.Windowed = true;
			this.device.Reset(new PresentParameters[] { presentParameters });
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0001001C File Offset: 0x0000F01C
		private void ResetDevice()
		{
			this.ResetDevice(base.FormBorderStyle == FormBorderStyle.None);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00010030 File Offset: 0x0000F030
		private void ApplyFullscreen(bool fullscreenEnabled)
		{
			if (fullscreenEnabled)
			{
				base.TopMost = true;
				base.FormBorderStyle = FormBorderStyle.None;
				this.windowedLocation = base.Location;
				base.Location = new Point(0, 0);
				this.displayDevice.SetDisplayMode(800, 600, 32, 0, true);
				base.Width = 800;
				base.Height = 600;
				return;
			}
			this.displayDevice.RestoreDisplayMode();
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Location = this.windowedLocation;
			base.TopMost = false;
			base.ClientSize = new Size(800, 600);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000100DC File Offset: 0x0000F0DC
		private void ApplyFullscreen()
		{
			if (base.FormBorderStyle == FormBorderStyle.None)
			{
				this.ApplyFullscreen(false);
				return;
			}
			this.ApplyFullscreen(true);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000100F8 File Offset: 0x0000F0F8
		private void ApplyLanguage(ButtonLayout layout)
		{
			switch (layout)
			{
			case ButtonLayout.SimplifiedChinese:
				this.menuCancelItem.Text = "取消(&C)";
				this.menuOptionsItem.Text = "选项(&O)\tF8";
				this.menuDisplayModeItem.Text = "显示模式(&M)\tAlt+Enter";
				this.menuSkipItem.Text = "快进(&Q)\tF3";
				this.menuAutoItem.Text = "自动播放(&A)\tF4";
				this.menuHideWindowItem.Text = "隐藏对话(&H)\tF2";
				this.menuLanguageItem.Text = "语言(&L)";
				this.menuFontItem.Text = "字体(&F)";
				this.menuBackToTitleItem.Text = "返回主菜单(&B)";
				this.menuExitItem.Text = "结束游戏(&E)\tAlt+F4";
				this.Text = "秋之回忆 ～雪萤～";
				return;
			case ButtonLayout.TraditionalChinese:
				this.menuCancelItem.Text = "取消(&C)";
				this.menuOptionsItem.Text = "選項(&O)\tF8";
				this.menuDisplayModeItem.Text = "顯示模式(&M)\tAlt+Enter";
				this.menuSkipItem.Text = "快進(&Q)\tF3";
				this.menuAutoItem.Text = "自動播放(&A)\tF4";
				this.menuHideWindowItem.Text = "隱藏對話(&H)\tF2";
				this.menuLanguageItem.Text = "語言(&L)";
				this.menuFontItem.Text = "字體(&F)";
				this.menuBackToTitleItem.Text = "返回主菜單(&B)";
				this.menuExitItem.Text = "結束遊戲(&E)\tAlt+F4";
				this.Text = "秋之回憶 ～雪螢～";
				return;
			case ButtonLayout.Japanese:
				this.menuCancelItem.Text = "戻る(&C)";
				this.menuOptionsItem.Text = "オプション設定(&O)\tF8";
				this.menuDisplayModeItem.Text = "画面モード(&M)\tAlt+Enter";
				this.menuSkipItem.Text = "スキップ(&Q)\tF3";
				this.menuAutoItem.Text = "オートモード(&A)\tF4";
				this.menuHideWindowItem.Text = "ウインドウ消去(&H)\tF2";
				this.menuLanguageItem.Text = "言語(&L)";
				this.menuFontItem.Text = "フォント(&F)";
				this.menuBackToTitleItem.Text = "タイトルに戻る(&B)";
				this.menuExitItem.Text = "ゲーム終了(&E)\tAlt+F4";
				this.Text = "メモリーズオフデュエット ～雪蛍～";
				return;
			default:
				return;
			}
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x00010344 File Offset: 0x0000F344
		private int PollJoystick()
		{
			if (this.joystick != null)
			{
				try
				{
					this.joystick.Poll();
				}
				catch (InputException ex)
				{
					if (!(ex is NotAcquiredException))
					{
						if (!(ex is InputLostException))
						{
							goto IL_0045;
						}
					}
					try
					{
						this.joystick.Acquire();
					}
					catch (InputException)
					{
						return -1;
					}
					IL_0045:;
				}
				JoystickState currentJoystickState;
				try
				{
					currentJoystickState = this.joystick.CurrentJoystickState;
				}
				catch (InputException)
				{
					return -1;
				}
				byte[] buttons = currentJoystickState.GetButtons();
				int x = currentJoystickState.X;
				int y = currentJoystickState.Y;
				if (x < -500 && x < -Math.Abs(y))
				{
					buttons[4] = 128;
				}
				else if (x > 500 && x > Math.Abs(y))
				{
					buttons[5] = 128;
				}
				else if (y < -500 && y <= -Math.Abs(x))
				{
					buttons[6] = 128;
				}
				else if (y > 500 && y >= Math.Abs(x))
				{
					buttons[7] = 128;
				}
				for (int i = 0; i < 10; i++)
				{
					if ((buttons[i] & 128) != 0)
					{
						if (!this.joystickButtonStates[i])
						{
							this.joystickButtonStates[i] = true;
							return i;
						}
					}
					else
					{
						this.joystickButtonStates[i] = false;
					}
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x000104EC File Offset: 0x0000F4EC
		private void StartScenario(int episode)
		{
			try
			{
				this.director = new SceneDirector("Yukihota", episode, this.device, this.sprite, new FrameCallback(this.ResetDevice), this.whiteTexture, this.forceFeedbackEffect, new System.Drawing.Font(this.fontName, 24f), this.language, this.textColor);
				this.director.SetFadeEnabled(this.fadeEnabled);
				this.director.SetSunEffectEnabled(this.sunEnabled);
				this.director.SetEnvironmentEffectEnabled(this.envEnabled);
				this.director.SetShockEffectEnabled(this.shockEnabled);
				this.director.SetTextSpeed(this.textSpeed);
				this.director.SetSynchroEnabled(this.voiceSynchro);
				goto IL_0110;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Application.Exit();
				goto IL_0110;
			}
			IL_00D8:
			this.gameMode = 1;
			this.menuBgmPlayer.Stop();
			this.FreeTitleResources();
			this.FreeOptionsResources();
			this.FreeLogoTexture();
			this.FreeEpisodeResources();
			this.FreeAlbumResources();
			this.scenarioTimer.Start();
			return;
			IL_0110:
			this.layerTextures.Clear();
			this.layerSourceRects.Clear();
			this.layerDestRects.Clear();
			this.layerPositions.Clear();
			this.layerColors.Clear();
			this.overlayText = null;
			goto IL_00D8;
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x0001066C File Offset: 0x0000F66C
		private void ReturnToTitle()
		{
			if (this.director.GetEpisode() > this.unlockedEpisodeCount)
			{
				this.unlockedEpisodeCount = this.director.GetEpisode();
			}
			if (this.director.GetAlbumIndex() > this.unlockedAlbumCount)
			{
				this.unlockedAlbumCount = this.director.GetAlbumIndex();
			}
			this.scenarioTimer.Stop();
			this.director.Stop();
			this.director.Dispose();
			this.director = null;
			GC.Collect();
			this.LoadTitleResources();
			this.LoadOptionsResources();
			this.LoadLogoTexture();
			this.LoadEpisodeResources();
			this.LoadAlbumResources();
			this.EnterTitleMenu();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x00010718 File Offset: 0x0000F718
		private void DrawLayer(Texture texture, Rectangle sourceRect, Rectangle destRect, Point position, Color color)
		{
			this.sprite.Draw2D(texture, sourceRect, destRect, Point.Empty, 0f, position, color);
			this.layerTextures.Add(texture);
			this.layerSourceRects.Add(sourceRect);
			this.layerDestRects.Add(destRect);
			this.layerPositions.Add(position);
			this.layerColors.Add(color);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x000107A0 File Offset: 0x0000F7A0
		private void LoadLogoTexture()
		{
			this.logoTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("KID_LOGO"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			if (GalGameForm.noOperationMode)
			{
				this.logoFrame = 0;
				this.gameMode = -1;
			}
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x000107F8 File Offset: 0x0000F7F8
		private void FreeLogoTexture()
		{
			this.logoTexture.Dispose();
			this.logoTexture = null;
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0001080C File Offset: 0x0000F80C
		private void InitOpeningMovie()
		{
			this.gameMode = -2;
			if (!GalGameForm.noOperationMode)
			{
				this.movieBox.Visible = true;
				this.openingMovie = new MediaPackage("Op");
				this.openingMovie.Open("Movie\\KFC.wmv", this.movieBox);
				this.openingMovie.Play();
			}
			this.logoFadeTimer.Start();
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x00010870 File Offset: 0x0000F870
		private void OnLogoFadeTimerTick(object sender, EventArgs e)
		{
			if (this.gameMode == -2 && !GalGameForm.noOperationMode)
			{
				if (this.openingMovie.GetPosition() < this.openingMovie.GetLength() && this.openingMovie.GetLength() != -1)
				{
					return;
				}
				this.logoFrame = 0;
				this.gameMode = -1;
				this.movieBox.Visible = false;
				this.suppressLostFocus = true;
				this.openingMovie.Close();
				this.suppressLostFocus = false;
				this.openingMovie = null;
			}
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.layerTextures.Clear();
			this.layerSourceRects.Clear();
			this.layerDestRects.Clear();
			this.layerPositions.Clear();
			this.layerColors.Clear();
			this.overlayText = null;
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			int alpha;
			if (this.logoFrame < this.logoFadeThresholds[0])
			{
				alpha = (int)((double)this.logoFrame / (double)this.logoFadeThresholds[0] * 255.0);
			}
			else if (this.logoFrame < this.logoFadeThresholds[1])
			{
				alpha = 255;
			}
			else if (this.logoFrame < this.logoFadeThresholds[2])
			{
				alpha = (int)((double)(this.logoFadeThresholds[2] - this.logoFrame) / (double)(this.logoFadeThresholds[2] - this.logoFadeThresholds[1]) * 255.0);
			}
			else
			{
				this.logoFadeTimer.Stop();
				this.EnterTitleMenu();
				alpha = 0;
			}
			this.DrawLayer(this.logoTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
			this.logoFrame++;
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
					this.ResetDevice();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x00010AFC File Offset: 0x0000FAFC
		private void LoadTitleResources()
		{
			this.titleTextures[0] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("TITLESC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.titleTextures[1] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("TITLETC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.titleTextures[2] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("TITLEJP"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.menuButtonTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("MENU"), this.menuButtonSize.Width, this.menuButtonSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x00010BFC File Offset: 0x0000FBFC
		private void FreeTitleResources()
		{
			for (int i = 0; i < 3; i++)
			{
				this.titleTextures[i].Dispose();
				this.titleTextures[i] = null;
			}
			this.menuButtonTexture.Dispose();
			this.menuButtonTexture = null;
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x00010C40 File Offset: 0x0000FC40
		private void EnterTitleMenu()
		{
			this.gameMode = 0;
			this.menuFadeCounter = 10;
			this.menuFadeIn = true;
			this.menuItemEnabled[0] = (this.menuItemEnabled[2] = (this.menuItemEnabled[4] = true));
			this.menuItemEnabled[1] = this.unlockedEpisodeCount > 0;
			this.menuItemEnabled[3] = this.unlockedAlbumCount > 0;
			if (!this.menuBgmPlayer.IsPlaying())
			{
				this.menuBgmPlayer.Play(true);
			}
			this.menuTimer.Start();
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x00010CD0 File Offset: 0x0000FCD0
		private void CloseMenu()
		{
			this.menuFadeCounter = 10;
			this.menuFadeIn = false;
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x00010CE4 File Offset: 0x0000FCE4
		private void MoveMenuSelection(int direction)
		{
			if (this.menuFadeCounter > 0)
			{
				return;
			}
			switch (direction)
			{
			case 0:
				if (this.menuSelection == 0)
				{
					this.menuSelection = 5;
					return;
				}
				this.menuSelection--;
				if (this.menuSelection == 0)
				{
					this.menuSelection = 5;
					return;
				}
				break;
			case 1:
				if (this.menuSelection == 0)
				{
					this.menuSelection = 1;
					return;
				}
				this.menuSelection++;
				if (this.menuSelection == 6)
				{
					this.menuSelection = 1;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x00010D70 File Offset: 0x0000FD70
		private void OnMenuTimerTick(object sender, EventArgs e)
		{
			GC.Collect();
			int joystickState = this.PollJoystick();
			int joystickState2;
			if (joystickState == 0)
			{
				if (this.menuSelection > 0 && this.menuItemEnabled[this.menuSelection - 1])
				{
					this.CloseMenu();
					goto IL_0234;
				}
				goto IL_0234;
			}
			else
			{
				joystickState2 = joystickState;
			}
			IL_021D:
			if (joystickState2 == 6)
			{
				this.MoveMenuSelection(0);
			}
			else if (joystickState == 7)
			{
				this.MoveMenuSelection(1);
			}
			IL_0234:
			bool shouldClose = false;
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.layerTextures.Clear();
			this.layerSourceRects.Clear();
			this.layerDestRects.Clear();
			this.layerPositions.Clear();
			this.layerColors.Clear();
			this.overlayText = null;
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			this.DrawLayer(this.titleTextures[(int)this.language], Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < 5; i++)
			{
				Rectangle rectangle = new Rectangle((this.menuSelection - 1 == i) ? 0 : 180, 32 * (this.menuItemEnabled[i] ? i : 5), 180, 32);
				this.DrawLayer(this.menuButtonTexture, rectangle, Rectangle.Empty, new Point(310, 368 + 32 * i), Color.White);
			}
			if (this.menuFadeCounter > 0)
			{
				int menuAlpha = (int)((double)(this.menuFadeIn ? this.menuFadeCounter : (10 - this.menuFadeCounter)) / 10.0 * 255.0);
				this.DrawLayer(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(menuAlpha, Color.White));
				this.menuFadeCounter--;
				if (!this.menuFadeIn && this.menuFadeCounter == 0)
				{
					this.menuTimer.Stop();
					switch (this.menuSelection)
					{
					case 1:
						shouldClose = true;
						break;
					case 2:
						this.OpenEpisodeSelect();
						break;
					case 3:
						this.OpenOptions();
						break;
					case 4:
						this.OpenAlbum();
						break;
					case 5:
						base.Close();
						Application.Exit();
						break;
					}
					this.menuSelection = 0;
				}
			}
			this.sprite.End();
			this.device.EndScene();
			try
			{
				this.device.Present();
				goto IL_023F;
			}
			catch (DeviceLostException)
			{
				try
				{
					this.device.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					this.ResetDevice();
				}
				catch
				{
				}
				goto IL_023F;
			}
			goto IL_021D;
			IL_023F:
			if (shouldClose)
			{
				this.StartScenario(1);
			}
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x00011084 File Offset: 0x00010084
		private void OnMenuMouseDown(object sender, MouseEventArgs e)
		{
			if (this.menuFadeCounter == 0 && e.Button == MouseButtons.Left)
			{
				this.lastCursorPosition = Point.Empty;
				this.OnMenuMouseMove(sender, e);
				if (this.menuSelection > 0 && this.menuItemEnabled[this.menuSelection - 1])
				{
					this.CloseMenu();
				}
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000110DC File Offset: 0x000100DC
		private void OnMenuMouseMove(object sender, MouseEventArgs e)
		{
			if (Cursor.Position.Equals(this.lastCursorPosition))
			{
				return;
			}
			this.lastCursorPosition = Cursor.Position;
			if (this.menuFadeCounter == 0)
			{
				if (e.X >= 310 && e.X < 490 && e.Y >= 368 && e.Y < 528)
				{
					this.menuSelection = (e.Y - 368) / 32 + 1;
					return;
				}
				this.menuSelection = 0;
			}
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x00011170 File Offset: 0x00010170
		private void LoadOptionsResources()
		{
			this.optionCursorFrame = 0;
			this.optionsBackTextures[0] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("OPT_BGSC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.optionsBackTextures[1] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("OPT_BGTC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.optionsBackTextures[2] = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("OPT_BGJP"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] OnActivated = this.optionsLabelTextures;
			int slot0 = 0;
			Microsoft.DirectX.Direct3D.Device device = this.device;
			Stream stream = this.systemArchive.ReadEntry("OPTIONSC");
			Size size = this.optionsLabelSize;
			int width = size.Width;
			size = this.optionsLabelSize;
			OnActivated[slot0] = TextureLoader.FromStream(device, stream, width, size.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] labelTextureAlias1 = this.optionsLabelTextures;
			int slot1 = 1;
			Microsoft.DirectX.Direct3D.Device device2 = this.device;
			Stream stream2 = this.systemArchive.ReadEntry("OPTIONTC");
			size = this.optionsLabelSize;
			int width2 = size.Width;
			size = this.optionsLabelSize;
			labelTextureAlias1[slot1] = TextureLoader.FromStream(device2, stream2, width2, size.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] labelTextureAlias2 = this.optionsLabelTextures;
			int slot2 = 2;
			Microsoft.DirectX.Direct3D.Device device3 = this.device;
			Stream stream3 = this.systemArchive.ReadEntry("OPTIONJP");
			size = this.optionsLabelSize;
			int width3 = size.Width;
			size = this.optionsLabelSize;
			labelTextureAlias2[slot2] = TextureLoader.FromStream(device3, stream3, width3, size.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0001131C File Offset: 0x0001031C
		private void FreeOptionsResources()
		{
			for (int i = 0; i < 3; i++)
			{
				this.optionsBackTextures[i].Dispose();
				this.optionsBackTextures[i] = null;
				this.optionsLabelTextures[i].Dispose();
				this.optionsLabelTextures[i] = null;
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x00011368 File Offset: 0x00010368
		private void OpenOptions()
		{
			GC.Collect();
			this.returnMode = this.gameMode;
			this.gameMode = 3;
			this.optionsFadeCounter = 10;
			this.optionsFadeIn = true;
			this.optionsTimer.Start();
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0001139C File Offset: 0x0001039C
		private void CloseOptions()
		{
			this.optionsFadeCounter = 10;
			this.optionsFadeIn = false;
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000113B0 File Offset: 0x000103B0
		private void MoveOptionSelection(int direction)
		{
			if (this.optionsFadeCounter > 0)
			{
				return;
			}
			if (this.optionSelection != 0)
			{
				if (this.optionSelection >= 1 && this.optionSelection <= 8)
				{
					switch (direction)
					{
					case 0:
					case 1:
						this.optionSelection += ((this.optionSelection % 2 == 0) ? (-1) : 1);
						return;
					case 2:
						this.optionSelection -= 2;
						if (this.optionSelection <= 0)
						{
							this.optionSelection += 23;
							return;
						}
						break;
					case 3:
						this.optionSelection += 2;
						return;
					default:
						return;
					}
				}
				else if (this.optionSelection >= 9 && this.optionSelection <= 15)
				{
					switch (direction)
					{
					case 0:
						this.optionSelection--;
						if (this.optionSelection == 8)
						{
							this.optionSelection = 15;
							return;
						}
						break;
					case 1:
						this.optionSelection++;
						if (this.optionSelection == 16)
						{
							this.optionSelection = 9;
							return;
						}
						break;
					case 2:
						if (this.optionSelection == 9)
						{
							this.optionSelection = 7;
							return;
						}
						this.optionSelection = 8;
						return;
					case 3:
						if (this.optionSelection <= 14)
						{
							this.optionSelection += 7;
							return;
						}
						this.optionSelection = 21;
						return;
					default:
						return;
					}
				}
				else if (this.optionSelection >= 16 && this.optionSelection <= 21)
				{
					switch (direction)
					{
					case 0:
						this.optionSelection--;
						if (this.optionSelection == 15)
						{
							this.optionSelection = 21;
							return;
						}
						break;
					case 1:
						this.optionSelection++;
						if (this.optionSelection == 22)
						{
							this.optionSelection = 16;
							return;
						}
						break;
					case 2:
						this.optionSelection -= 7;
						return;
					case 3:
						if (this.optionSelection == 16)
						{
							this.optionSelection = 22;
							return;
						}
						this.optionSelection = 23;
						return;
					default:
						return;
					}
				}
				else if (this.optionSelection >= 22 && this.optionSelection <= 23)
				{
					switch (direction)
					{
					case 0:
					case 1:
						this.optionSelection += ((this.optionSelection % 2 == 1) ? (-1) : 1);
						return;
					case 2:
						this.optionSelection -= 6;
						return;
					case 3:
						this.optionSelection -= 21;
						break;
					default:
						return;
					}
				}
				return;
			}
			switch (direction)
			{
			case 0:
			case 2:
				this.optionSelection = 23;
				return;
			case 1:
			case 3:
				this.optionSelection = 1;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x00011684 File Offset: 0x00010684
		private void ApplyOptionValue(int optionIndex)
		{
			switch (optionIndex)
			{
			case 0:
				this.fadeEnabled = true;
				return;
			case 1:
				this.fadeEnabled = false;
				return;
			case 2:
				this.envEnabled = true;
				return;
			case 3:
				this.envEnabled = false;
				return;
			case 4:
				this.sunEnabled = true;
				return;
			case 5:
				this.sunEnabled = false;
				return;
			case 6:
				this.shockEnabled = true;
				return;
			case 7:
				this.shockEnabled = false;
				return;
			case 8:
				this.textColor = Color.FromArgb(128, 255, 128);
				return;
			case 9:
				this.textColor = Color.FromArgb(128, 128, 255);
				return;
			case 10:
				this.textColor = Color.FromArgb(255, 128, 128);
				return;
			case 11:
				this.textColor = Color.FromArgb(255, 255, 128);
				return;
			case 12:
				this.textColor = Color.FromArgb(255, 128, 255);
				return;
			case 13:
				this.textColor = Color.FromArgb(128, 255, 255);
				return;
			case 14:
				this.textColor = Color.FromArgb(128, 128, 128);
				return;
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
				this.textSpeed = optionIndex - 14;
				return;
			case 21:
				this.voiceSynchro = false;
				return;
			case 22:
				this.voiceSynchro = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0001182C File Offset: 0x0001082C
		private void OnOptionsTimerTick(object sender, EventArgs e)
		{
			int joystickState = this.PollJoystick();
			if (joystickState == 0)
			{
				if (this.optionSelection > 0)
				{
					this.ApplyOptionValue(this.optionSelection - 1);
				}
			}
			else if (joystickState == 1)
			{
				this.CloseOptions();
			}
			if (joystickState >= 4 && joystickState <= 7)
			{
				this.MoveOptionSelection(joystickState - 4);
			}
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.layerTextures.Clear();
			this.layerSourceRects.Clear();
			this.layerDestRects.Clear();
			this.layerPositions.Clear();
			this.layerColors.Clear();
			this.overlayText = null;
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			this.DrawLayer(this.optionsBackTextures[(int)this.language], Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < 23; i++)
			{
				this.optionItemVisible[i] = false;
			}
			if (this.fadeEnabled)
			{
				this.optionItemVisible[0] = true;
			}
			else
			{
				this.optionItemVisible[1] = true;
			}
			if (this.envEnabled)
			{
				this.optionItemVisible[2] = true;
			}
			else
			{
				this.optionItemVisible[3] = true;
			}
			if (this.sunEnabled)
			{
				this.optionItemVisible[4] = true;
			}
			else
			{
				this.optionItemVisible[5] = true;
			}
			if (this.shockEnabled)
			{
				this.optionItemVisible[6] = true;
			}
			else
			{
				this.optionItemVisible[7] = true;
			}
			if (this.textColor == Color.FromArgb(128, 255, 128))
			{
				this.optionItemVisible[8] = true;
			}
			else if (this.textColor == Color.FromArgb(128, 128, 255))
			{
				this.optionItemVisible[9] = true;
			}
			else if (this.textColor == Color.FromArgb(255, 128, 128))
			{
				this.optionItemVisible[10] = true;
			}
			else if (this.textColor == Color.FromArgb(255, 255, 128))
			{
				this.optionItemVisible[11] = true;
			}
			else if (this.textColor == Color.FromArgb(255, 128, 255))
			{
				this.optionItemVisible[12] = true;
			}
			else if (this.textColor == Color.FromArgb(128, 255, 255))
			{
				this.optionItemVisible[13] = true;
			}
			else if (this.textColor == Color.FromArgb(128, 128, 128))
			{
				this.optionItemVisible[14] = true;
			}
			this.optionItemVisible[this.textSpeed + 14] = true;
			if (!this.voiceSynchro)
			{
				this.optionItemVisible[21] = true;
			}
			else
			{
				this.optionItemVisible[22] = true;
			}
			for (int j = 0; j < 23; j++)
			{
				if (this.optionItemVisible[j])
				{
					Rectangle rectangle = new Rectangle(this.optionSourceX[j], this.optionSourceY[j], this.optionSourceWidth[j], this.optionSourceHeight[j]);
					this.DrawLayer(this.optionsLabelTextures[(int)this.language], rectangle, Rectangle.Empty, new Point(this.optionLabelX[j], this.optionLabelY[j]), Color.White);
				}
			}
			if (this.optionSelection > 0)
			{
				Rectangle rectangle = new Rectangle(225 + 16 * this.optionCursorFrames[this.optionCursorFrame], 25, 16, 25);
				this.DrawLayer(this.optionsLabelTextures[(int)this.language], rectangle, Rectangle.Empty, new Point(this.optionLabelX[this.optionSelection - 1] - 16, this.optionLabelY[this.optionSelection - 1]), Color.White);
				rectangle = new Rectangle(273 + 16 * this.optionCursorFrames[this.optionCursorFrame], 25, 16, 25);
				this.DrawLayer(this.optionsLabelTextures[(int)this.language], rectangle, Rectangle.Empty, new Point(this.optionLabelX[this.optionSelection - 1] + this.optionSourceWidth[this.optionSelection - 1], this.optionLabelY[this.optionSelection - 1]), Color.White);
				this.optionCursorFrame++;
				if (this.optionCursorFrame == 6)
				{
					this.optionCursorFrame = 0;
				}
			}
			bool shouldClose = false;
			if (this.optionsFadeCounter > 0)
			{
				int menuAlpha = (int)((double)(this.optionsFadeIn ? this.optionsFadeCounter : (10 - this.optionsFadeCounter)) / 10.0 * 255.0);
				this.DrawLayer(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(menuAlpha, Color.White));
				this.optionsFadeCounter--;
				if (!this.optionsFadeIn && this.optionsFadeCounter == 0)
				{
					this.optionsTimer.Stop();
					if (this.returnMode == 0)
					{
						this.EnterTitleMenu();
					}
					else if (this.returnMode == 1)
					{
						this.layerTextures.Clear();
						this.layerSourceRects.Clear();
						this.layerDestRects.Clear();
						this.layerPositions.Clear();
						this.layerColors.Clear();
						this.overlayText = null;
						this.gameMode = 1;
						this.director.SetFadeEnabled(this.fadeEnabled);
						this.director.SetSunEffectEnabled(this.sunEnabled);
						this.director.SetEnvironmentEffectEnabled(this.envEnabled);
						this.director.SetShockEffectEnabled(this.shockEnabled);
						this.director.SetTextColor(this.textColor);
						this.director.SetTextSpeed(this.textSpeed);
						this.director.SetSynchroEnabled(this.voiceSynchro);
						this.director.SetPaused(false, false);
						this.scenarioTimer.Start();
						shouldClose = true;
					}
					this.optionSelection = 0;
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
					this.ResetDevice();
				}
				catch
				{
				}
			}
			if (shouldClose)
			{
				this.FreeOptionsResources();
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x00011F70 File Offset: 0x00010F70
		private void OnOptionsMouseDown(object sender, MouseEventArgs e)
		{
			if (this.optionsFadeCounter == 0)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.lastCursorPosition = Point.Empty;
					this.OnOptionsMouseMove(sender, e);
					if (this.optionSelection > 0)
					{
						this.ApplyOptionValue(this.optionSelection - 1);
						return;
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					this.CloseOptions();
				}
			}
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x00011FD0 File Offset: 0x00010FD0
		private void OnOptionsMouseMove(object sender, MouseEventArgs e)
		{
			if (Cursor.Position.Equals(this.lastCursorPosition))
			{
				return;
			}
			this.lastCursorPosition = Cursor.Position;
			if (this.optionsFadeCounter == 0)
			{
				for (int i = 0; i < 23; i++)
				{
					if (e.X >= this.optionLabelX[i] && e.X < this.optionLabelX[i] + this.optionSourceWidth[i] && e.Y >= this.optionLabelY[i] && e.Y < this.optionLabelY[i] + this.optionSourceHeight[i])
					{
						this.optionSelection = i + 1;
						return;
					}
				}
			}
			this.optionSelection = 0;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0001208C File Offset: 0x0001108C
		private void LoadEpisodeResources()
		{
			this.episodeBackTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("SCUT_BG"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.episodeMenuTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("SCUTMENU"), this.episodeMenuSize.Width, this.episodeMenuSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x00012118 File Offset: 0x00011118
		private void FreeEpisodeResources()
		{
			this.episodeBackTexture.Dispose();
			this.episodeBackTexture = null;
			this.episodeMenuTexture.Dispose();
			this.episodeMenuTexture = null;
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x00012140 File Offset: 0x00011140
		private void OpenEpisodeSelect()
		{
			this.gameMode = 2;
			this.episodeFadeCounter = 10;
			this.episodeFadeIn = true;
			this.episodeTimer.Start();
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x00012164 File Offset: 0x00011164
		private void CloseEpisodeSelect()
		{
			this.episodeFadeCounter = 10;
			this.episodeFadeIn = false;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x00012178 File Offset: 0x00011178
		private void MoveEpisodeSelection(int direction)
		{
			if (this.episodeFadeCounter > 0)
			{
				return;
			}
			switch (direction)
			{
			case 0:
				if (this.episodeSelection == 0)
				{
					this.episodeSelection = 16;
					return;
				}
				this.episodeSelection--;
				if (this.episodeSelection == 0)
				{
					this.episodeSelection = 16;
					return;
				}
				break;
			case 1:
				if (this.episodeSelection == 0)
				{
					this.episodeSelection = 1;
					return;
				}
				this.episodeSelection++;
				if (this.episodeSelection == 17)
				{
					this.episodeSelection = 1;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x00012208 File Offset: 0x00011208
		private void OnEpisodeTimerTick(object sender, EventArgs e)
		{
			int joystickState = this.PollJoystick();
			if (joystickState == 0)
			{
				if (this.episodeSelection > 0 && this.unlockedEpisodeCount >= this.episodeSelection)
				{
					this.CloseEpisodeSelect();
				}
			}
			else if (joystickState == 1)
			{
				this.episodeSelection = 0;
				this.CloseEpisodeSelect();
			}
			else if (joystickState == 6)
			{
				this.MoveEpisodeSelection(0);
			}
			else if (joystickState == 7)
			{
				this.MoveEpisodeSelection(1);
			}
			int episodeSelection = 0;
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.layerTextures.Clear();
			this.layerSourceRects.Clear();
			this.layerDestRects.Clear();
			ArrayList arrayList = this.layerPositions;
			for (;;)
			{
				arrayList.Clear();
				this.layerColors.Clear();
				this.overlayText = null;
				this.device.BeginScene();
				this.sprite.Begin(SpriteFlags.AlphaBlend);
				this.DrawLayer(this.episodeBackTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
				for (int i = 0; i < this.unlockedEpisodeCount; i++)
				{
					Rectangle rectangle = new Rectangle((this.episodeSelection - 1 == i) ? 0 : 280, 28 * i, 280, 28);
					this.DrawLayer(this.episodeMenuTexture, rectangle, Rectangle.Empty, new Point(this.episodeColumnX[(i < 8) ? 0 : 1], 80 + 40 * (i % 8)), Color.White);
				}
				for (int j = this.unlockedEpisodeCount; j < 16; j++)
				{
					Rectangle rectangle = new Rectangle((this.episodeSelection - 1 == j) ? 0 : 280, 448, 280, 28);
					this.DrawLayer(this.episodeMenuTexture, rectangle, Rectangle.Empty, new Point(this.episodeColumnX[(j < 8) ? 0 : 1], 80 + 40 * (j % 8)), Color.White);
				}
				if (this.episodeFadeCounter > 0)
				{
					int episodeAlpha = (int)((double)(this.episodeFadeIn ? this.episodeFadeCounter : (10 - this.episodeFadeCounter)) / 10.0 * 255.0);
					this.DrawLayer(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(episodeAlpha, Color.White));
					this.episodeFadeCounter--;
					if (!this.episodeFadeIn && this.episodeFadeCounter == 0)
					{
						this.episodeTimer.Stop();
						if (this.episodeSelection == 0)
						{
							this.EnterTitleMenu();
						}
						else
						{
							episodeSelection = this.episodeSelection;
						}
						this.episodeSelection = 0;
					}
				}
				this.sprite.End();
				this.device.EndScene();
				try
				{
					this.device.Present();
					break;
				}
				catch (DeviceLostException)
				{
					try
					{
						this.device.TestCooperativeLevel();
					}
					catch (DeviceNotResetException)
					{
						this.ResetDevice();
					}
					catch
					{
					}
					break;
				}
			}
			if (episodeSelection > 0)
			{
				this.StartScenario(episodeSelection);
			}
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x00012580 File Offset: 0x00011580
		private void OnEpisodeMouseDown(object sender, MouseEventArgs e)
		{
			if (this.episodeFadeCounter == 0)
			{
				if (e.Button == MouseButtons.Left)
				{
					this.lastCursorPosition = Point.Empty;
					this.OnEpisodeMouseMove(sender, e);
					if (this.episodeSelection > 0 && this.unlockedEpisodeCount >= this.episodeSelection)
					{
						this.CloseEpisodeSelect();
						return;
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					this.episodeSelection = 0;
					this.CloseEpisodeSelect();
				}
			}
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000125F0 File Offset: 0x000115F0
		private void OnEpisodeMouseMove(object sender, MouseEventArgs e)
		{
			if (Cursor.Position.Equals(this.lastCursorPosition))
			{
				return;
			}
			this.lastCursorPosition = Cursor.Position;
			if (this.episodeFadeCounter == 0)
			{
				this.episodeSelection = 0;
				for (int i = 0; i < 16; i++)
				{
					if (e.X > this.episodeColumnX[(i < 8) ? 0 : 1] && e.X < this.episodeColumnX[(i < 8) ? 0 : 1] + 280 && e.Y >= 80 + 40 * (i % 8) && e.Y < 80 + 40 * (i % 8) + 28)
					{
						this.episodeSelection = i + 1;
						return;
					}
				}
			}
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x000126BC File Offset: 0x000116BC
		private void LoadAlbumResources()
		{
			this.albumBackTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("ALBUMBG"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			this.albumPictureTexture = TextureLoader.FromStream(this.device, this.systemArchive.ReadEntry("AL_PIC"), this.albumPictureSize.Width, this.albumPictureSize.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x00012748 File Offset: 0x00011748
		private void FreeAlbumResources()
		{
			this.albumBackTexture.Dispose();
			this.albumBackTexture = null;
			this.albumPictureTexture.Dispose();
			this.albumPictureTexture = null;
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x00012770 File Offset: 0x00011770
		private void OpenAlbum()
		{
			this.gameMode = 4;
			this.albumFadeCounter = 10;
			this.albumFadeIn = true;
			this.albumGroup = 0;
			this.albumImageIndex = 0;
			this.albumImageTransition = 0;
			this.albumPrevTexture = (this.albumCurrentTexture = null);
			this.d3dFont = new Microsoft.DirectX.Direct3D.Font(this.device, new System.Drawing.Font(this.fontName, 24f));
			this.albumTimer.Start();
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x000127EC File Offset: 0x000117EC
		private void CloseAlbum()
		{
			this.albumFadeCounter = 10;
			this.albumFadeIn = false;
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x00012800 File Offset: 0x00011800
		private void ShowAlbumImage(int groupIndex, int imageIndex)
		{
			if (imageIndex >= this.albumImageCounts[groupIndex - 1])
			{
				this.BackToAlbumGrid();
				return;
			}
			this.albumGroup = groupIndex;
			this.albumImageIndex = imageIndex;
			this.albumImageTransition = 10;
			if (this.albumPrevTexture != null)
			{
				this.albumPrevTexture.Dispose();
			}
			this.albumPrevTexture = this.albumCurrentTexture;
			this.albumCurrentTexture = TextureLoader.FromStream(this.device, this.bgArchive.ReadEntry("EV_HP0" + groupIndex.ToString() + ((char)(imageIndex + 65)).ToString()), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x000128B4 File Offset: 0x000118B4
		private void BackToAlbumGrid()
		{
			this.albumGroup = 0;
			this.albumImageIndex = 0;
			this.albumImageTransition = 10;
			if (this.albumPrevTexture != null)
			{
				this.albumPrevTexture.Dispose();
			}
			this.albumPrevTexture = this.albumCurrentTexture;
			this.albumCurrentTexture = null;
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x00012904 File Offset: 0x00011904
		private void MoveAlbumSelection(int direction)
		{
			if (this.albumFadeCounter > 0 || this.albumImageTransition > 0)
			{
				return;
			}
			switch (direction)
			{
			case 0:
				if (this.albumSelection == 0)
				{
					this.albumSelection = 7;
					return;
				}
				this.albumSelection--;
				if (this.albumSelection == 0)
				{
					this.albumSelection = 7;
					return;
				}
				break;
			case 1:
				if (this.albumSelection == 0)
				{
					this.albumSelection = 1;
					return;
				}
				this.albumSelection++;
				if (this.albumSelection == 8)
				{
					this.albumSelection = 1;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x00012998 File Offset: 0x00011998
		private void OnAlbumTimerTick(object sender, EventArgs e)
		{
			int joystickState = this.PollJoystick();
			if (joystickState == 0)
			{
				if (this.albumGroup == 0)
				{
					if (this.albumSelection > 0 && this.unlockedAlbumCount >= this.albumSelection)
					{
						this.ShowAlbumImage(this.albumSelection, 0);
					}
				}
				else
				{
					this.ShowAlbumImage(this.albumGroup, this.albumImageIndex + 1);
				}
			}
			else if (joystickState == 1)
			{
				if (this.albumGroup == 0)
				{
					this.CloseAlbum();
				}
				else
				{
					this.BackToAlbumGrid();
				}
			}
			else if (joystickState == 4)
			{
				if (this.albumGroup == 0)
				{
					this.MoveAlbumSelection(0);
				}
			}
			else if (joystickState == 5 && this.albumGroup == 0)
			{
				this.MoveAlbumSelection(1);
			}
			this.device.Clear(ClearFlags.Target, Color.White, 1f, 0);
			this.layerTextures.Clear();
			this.layerSourceRects.Clear();
			this.layerDestRects.Clear();
			this.layerPositions.Clear();
			this.layerColors.Clear();
			this.overlayText = null;
			this.device.BeginScene();
			this.sprite.Begin(SpriteFlags.AlphaBlend);
			int charIndex;
			if (this.albumGroup == 0 || (this.albumImageIndex == 0 && this.albumImageTransition > 0))
			{
				this.DrawLayer(this.albumBackTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
				for (int i = 0; i < this.unlockedAlbumCount; i++)
				{
					this.DrawLayer(this.albumPictureTexture, new Rectangle(0, 90 * i, 120, 90), Rectangle.Empty, new Point(this.albumThumbX[i], this.albumThumbY[i]), (this.albumSelection - 1 == i) ? Color.White : Color.FromArgb(this.albumThumbDimAlpha, Color.White));
				}
				for (int j = this.unlockedAlbumCount; j < 7; j++)
				{
					this.DrawLayer(this.albumPictureTexture, new Rectangle(0, 630, 120, 90), Rectangle.Empty, new Point(this.albumThumbX[j], this.albumThumbY[j]), (this.albumSelection - 1 == j) ? Color.White : Color.FromArgb(this.albumThumbDimAlpha, Color.White));
				}
				if (this.albumSelection > 0 && this.unlockedAlbumCount >= this.albumSelection)
				{
					charIndex = 0;
					goto IL_01D6;
				}
			}
			else if (this.albumImageTransition == 0)
			{
				this.DrawLayer(this.albumCurrentTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
			else
			{
				this.DrawLayer(this.albumPrevTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
			IL_001F:
			if (this.albumFadeCounter > 0)
			{
				int albumAlpha = (int)((double)(this.albumFadeIn ? this.albumFadeCounter : (10 - this.albumFadeCounter)) / 10.0 * 255.0);
				this.DrawLayer(this.whiteTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(albumAlpha, Color.White));
				this.albumFadeCounter--;
				if (!this.albumFadeIn && this.albumFadeCounter == 0)
				{
					this.albumTimer.Stop();
					this.EnterTitleMenu();
					this.albumSelection = 0;
				}
			}
			else if (this.albumImageTransition > 0)
			{
				if (this.albumGroup > 0)
				{
					this.DrawLayer(this.albumCurrentTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)((double)(10 - this.albumImageTransition) / 10.0 * 255.0), Color.White));
				}
				else
				{
					this.DrawLayer(this.albumPrevTexture, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)((double)this.albumImageTransition / 10.0 * 255.0), Color.White));
				}
				this.albumImageTransition--;
			}
			this.sprite.End();
			this.device.EndScene();
			try
			{
				this.device.Present();
				goto IL_025C;
			}
			catch (DeviceLostException)
			{
				try
				{
					this.device.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					this.ResetDevice();
				}
				catch
				{
				}
				goto IL_025C;
			}
			goto IL_01D4;
			IL_025C:
			if (this.albumGroup == 0 && this.albumImageTransition == 0 && this.albumPrevTexture != null)
			{
				this.albumPrevTexture.Dispose();
				this.albumPrevTexture = null;
			}
			return;
			IL_01D4:
			int lastChar;
			int charAdvance;
			charIndex = lastChar + charAdvance;
			IL_01D6:
			if (charIndex >= this.albumTitles[this.albumSelection - 1].Length)
			{
				this.layerTextures.Add(null);
				this.layerSourceRects.Add(null);
				this.layerDestRects.Add(null);
				this.layerPositions.Add(null);
				this.layerColors.Add(null);
				this.overlayText = this.albumTitles[this.albumSelection - 1];
				goto IL_001F;
			}
			this.d3dFont.DrawText(this.sprite, this.albumTitles[this.albumSelection - 1][charIndex].ToString(), this.textOrigin.X + this.charCellSize.Width * charIndex + 2, this.textOrigin.Y + 2, Color.Black);
			this.d3dFont.DrawText(this.sprite, this.albumTitles[this.albumSelection - 1][charIndex].ToString(), this.textOrigin.X + this.charCellSize.Width * charIndex, this.textOrigin.Y, Color.White);
			lastChar = charIndex;
			charAdvance = 1;
			goto IL_01D4;
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x00013034 File Offset: 0x00012034
		private void OnAlbumMouseDown(object sender, MouseEventArgs e)
		{
			if (this.albumFadeCounter == 0)
			{
				if (e.Button == MouseButtons.Left)
				{
					if (this.albumGroup != 0)
					{
						this.ShowAlbumImage(this.albumGroup, this.albumImageIndex + 1);
						return;
					}
					this.lastCursorPosition = Point.Empty;
					this.OnAlbumMouseMove(sender, e);
					if (this.albumSelection > 0 && this.unlockedAlbumCount >= this.albumSelection)
					{
						this.ShowAlbumImage(this.albumSelection, 0);
						return;
					}
				}
				else if (e.Button == MouseButtons.Right)
				{
					if (this.albumGroup == 0)
					{
						this.CloseAlbum();
						return;
					}
					this.BackToAlbumGrid();
				}
			}
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x000130D8 File Offset: 0x000120D8
		private void OnAlbumMouseMove(object sender, MouseEventArgs e)
		{
			if (Cursor.Position.Equals(this.lastCursorPosition))
			{
				return;
			}
			this.lastCursorPosition = Cursor.Position;
			if (this.albumFadeCounter == 0)
			{
				this.albumSelection = 0;
				int i = 0;
				while (i < 7)
				{
					if (e.X > this.albumThumbX[i] && e.X < this.albumThumbX[i] + 120 && e.Y >= this.albumThumbY[i] && e.Y < this.albumThumbY[i] + 90)
					{
						this.albumSelection = i + 1;
						return;
					}
					int index = i++;
				}
			}
		}

		// Token: 0x0400001C RID: 28
		private const int fontSize = 24;

		// Token: 0x0400001D RID: 29
		protected const int wmSysCommand = 274;

		// Token: 0x0400001E RID: 30
		protected const int scScreenSave = 61760;

		// Token: 0x0400001F RID: 31
		protected const int scMonitorPower = 61808;

		// Token: 0x04000020 RID: 32
		protected const int menuFadeFrames = 10;

		// Token: 0x04000021 RID: 33
		protected const int menuItemWidth = 180;

		// Token: 0x04000022 RID: 34
		protected const int menuItemHeight = 32;

		// Token: 0x04000023 RID: 35
		protected const int menuOriginX = 310;

		// Token: 0x04000024 RID: 36
		protected const int menuOriginY = 368;

		// Token: 0x04000025 RID: 37
		protected const int menuItemCount = 5;

		// Token: 0x04000026 RID: 38
		protected const int optionsFadeFrames = 10;

		// Token: 0x04000027 RID: 39
		protected const int optionItemCount = 23;

		// Token: 0x04000028 RID: 40
		protected const int optionCursorSourceX1 = 225;

		// Token: 0x04000029 RID: 41
		protected const int optionCursorSourceY1 = 25;

		// Token: 0x0400002A RID: 42
		protected const int optionCursorSourceX2 = 273;

		// Token: 0x0400002B RID: 43
		protected const int optionCursorSourceY2 = 25;

		// Token: 0x0400002C RID: 44
		protected const int optionCursorWidth = 16;

		// Token: 0x0400002D RID: 45
		protected const int optionCursorHeight = 25;

		// Token: 0x0400002E RID: 46
		protected const int languageTextureCount = 3;

		// Token: 0x0400002F RID: 47
		protected const int optionCursorFrameCount = 6;

		// Token: 0x04000030 RID: 48
		protected const int episodeFadeFrames = 10;

		// Token: 0x04000031 RID: 49
		protected const int episodeItemWidth = 280;

		// Token: 0x04000032 RID: 50
		protected const int episodeItemHeight = 28;

		// Token: 0x04000033 RID: 51
		protected const int episodeOriginY = 80;

		// Token: 0x04000034 RID: 52
		protected const int episodeRowHeight = 40;

		// Token: 0x04000035 RID: 53
		protected const int episodeSlotCount = 16;

		// Token: 0x04000036 RID: 54
		protected const int episodesPerColumn = 8;

		// Token: 0x04000037 RID: 55
		protected const int albumFadeFrames = 10;

		// Token: 0x04000038 RID: 56
		protected const int albumThumbWidth = 120;

		// Token: 0x04000039 RID: 57
		protected const int albumThumbHeight = 90;

		// Token: 0x0400003A RID: 58
		protected const int albumSlotCount = 7;

		// Token: 0x0400003B RID: 59
		protected const int episodeColumnCount = 2;

		// Token: 0x0400003C RID: 60
		protected const int albumSlotWidth = 24;

		// Token: 0x0400003D RID: 61
		private Microsoft.DirectX.DirectInput.Device joystick;

		// Token: 0x0400003E RID: 62
		private bool[] joystickButtonStates = new bool[10];

		// Token: 0x0400003F RID: 63
		private EffectObject forceFeedbackEffect;

		// Token: 0x04000040 RID: 64
		private Point windowedLocation;

		// Token: 0x04000041 RID: 65
		private int gameMode;

		// Token: 0x04000042 RID: 66
		private SceneDirector director;

		// Token: 0x04000043 RID: 67
		private Point lastCursorPosition;

		// Token: 0x04000044 RID: 68
		private MediaPackage menuBgmPlayer;

		// Token: 0x04000045 RID: 69
		private Microsoft.DirectX.Direct3D.Device device;

		// Token: 0x04000046 RID: 70
		private Microsoft.DirectX.DirectDraw.Device displayDevice;

		// Token: 0x04000047 RID: 71
		private bool isBorderless;

		// Token: 0x04000048 RID: 72
		private bool suppressLostFocus;

		// Token: 0x04000049 RID: 73
		private Sprite sprite;

		// Token: 0x0400004A RID: 74
		protected Texture whiteTexture;

		// Token: 0x0400004B RID: 75
		private static bool noOperationMode;

		// Token: 0x0400004C RID: 76
		private bool languageResetPending = false;

		// Token: 0x0400004D RID: 77
		private ArchiveFile systemArchive;

		// Token: 0x0400004E RID: 78
		private ArchiveFile bgmArchive;

		// Token: 0x0400004F RID: 79
		private ArchiveFile bgArchive;

		// Token: 0x04000050 RID: 80
		private string fontName = "黑体";

		// Token: 0x04000051 RID: 81
		private bool fullscreen = true;

		// Token: 0x04000052 RID: 82
		private ButtonLayout language = ButtonLayout.SimplifiedChinese;

		// Token: 0x04000053 RID: 83
		private bool showOptionsOnStartup = true;

		// Token: 0x04000054 RID: 84
		private bool fadeEnabled = true;

		// Token: 0x04000055 RID: 85
		private bool envEnabled = true;

		// Token: 0x04000056 RID: 86
		private bool sunEnabled = true;

		// Token: 0x04000057 RID: 87
		private bool shockEnabled = true;

		// Token: 0x04000058 RID: 88
		private Color textColor = Color.FromArgb(128, 128, 255);

		// Token: 0x04000059 RID: 89
		private int textSpeed = 4;

		// Token: 0x0400005A RID: 90
		private bool voiceSynchro = true;

		// Token: 0x0400005B RID: 91
		private int unlockedEpisodeCount = 0;

		// Token: 0x0400005C RID: 92
		private int unlockedAlbumCount = 0;

		// Token: 0x0400005D RID: 93
		private IContainer components;

		// Token: 0x0400005E RID: 94
		private Timer scenarioTimer;

		// Token: 0x0400005F RID: 95
		private Timer menuTimer;

		// Token: 0x04000060 RID: 96
		private Timer optionsTimer;

		// Token: 0x04000061 RID: 97
		private ContextMenu contextMenu;

		// Token: 0x04000062 RID: 98
		private MenuItem menuCancelItem;

		// Token: 0x04000063 RID: 99
		private MenuItem menuSeparator1;

		// Token: 0x04000064 RID: 100
		private MenuItem menuOptionsItem;

		// Token: 0x04000065 RID: 101
		private MenuItem menuSeparator2;

		// Token: 0x04000066 RID: 102
		private MenuItem menuDisplayModeItem;

		// Token: 0x04000067 RID: 103
		private MenuItem menuSeparator3;

		// Token: 0x04000068 RID: 104
		private MenuItem menuSkipItem;

		// Token: 0x04000069 RID: 105
		private MenuItem menuAutoItem;

		// Token: 0x0400006A RID: 106
		private MenuItem menuHideWindowItem;

		// Token: 0x0400006B RID: 107
		private MenuItem menuSeparator4;

		// Token: 0x0400006C RID: 108
		private MenuItem menuLanguageItem;

		// Token: 0x0400006D RID: 109
		private MenuItem menuSimplifiedItem;

		// Token: 0x0400006E RID: 110
		private MenuItem menuTraditionalItem;

		// Token: 0x0400006F RID: 111
		private MenuItem menuJapaneseItem;

		// Token: 0x04000070 RID: 112
		private MenuItem menuSeparator5;

		// Token: 0x04000071 RID: 113
		private MenuItem menuFontItem;

		// Token: 0x04000072 RID: 114
		private MenuItem menuSeparator6;

		// Token: 0x04000073 RID: 115
		private MenuItem menuBackToTitleItem;

		// Token: 0x04000074 RID: 116
		private MenuItem menuSeparator7;

		// Token: 0x04000075 RID: 117
		private MenuItem menuExitItem;

		// Token: 0x04000076 RID: 118
		private Timer logoFadeTimer;

		// Token: 0x04000077 RID: 119
		private Timer episodeTimer;

		// Token: 0x04000078 RID: 120
		private Timer albumTimer;

		// Token: 0x04000079 RID: 121
		private PictureBox movieBox;

		// Token: 0x0400007A RID: 122
		private FontDialog fontDialog;

		// Token: 0x0400007B RID: 123
		private bool enterKeyDown = false;

		// Token: 0x0400007C RID: 124
		private bool spaceKeyDown = false;

		// Token: 0x0400007D RID: 125
		private bool f2KeyDown = false;

		// Token: 0x0400007E RID: 126
		private bool f3KeyDown = false;

		// Token: 0x0400007F RID: 127
		private bool f4KeyDown = false;

		// Token: 0x04000080 RID: 128
		private bool f8KeyDown = false;

		// Token: 0x04000081 RID: 129
		private bool f12KeyDown = false;

		// Token: 0x04000082 RID: 130
		protected ArrayList layerTextures = new ArrayList();

		// Token: 0x04000083 RID: 131
		protected ArrayList layerSourceRects = new ArrayList();

		// Token: 0x04000084 RID: 132
		protected ArrayList layerDestRects = new ArrayList();

		// Token: 0x04000085 RID: 133
		protected ArrayList layerPositions = new ArrayList();

		// Token: 0x04000086 RID: 134
		protected ArrayList layerColors = new ArrayList();

		// Token: 0x04000087 RID: 135
		protected string overlayText;

		// Token: 0x04000088 RID: 136
		protected readonly int[] logoFadeThresholds = new int[] { 5, 15, 20 };

		// Token: 0x04000089 RID: 137
		private int logoFrame;

		// Token: 0x0400008A RID: 138
		private Texture logoTexture;

		// Token: 0x0400008B RID: 139
		private MediaPackage openingMovie;

		// Token: 0x0400008C RID: 140
		private int menuFadeCounter;

		// Token: 0x0400008D RID: 141
		private bool menuFadeIn;

		// Token: 0x0400008E RID: 142
		private Texture[] titleTextures = new Texture[3];

		// Token: 0x0400008F RID: 143
		private Texture menuButtonTexture;

		// Token: 0x04000090 RID: 144
		private int menuSelection;

		// Token: 0x04000091 RID: 145
		protected readonly Size menuButtonSize = new Size(360, 192);

		// Token: 0x04000092 RID: 146
		private bool[] menuItemEnabled = new bool[5];

		// Token: 0x04000093 RID: 147
		private int optionsFadeCounter;

		// Token: 0x04000094 RID: 148
		private bool optionsFadeIn;

		// Token: 0x04000095 RID: 149
		private Texture[] optionsBackTextures = new Texture[3];

		// Token: 0x04000096 RID: 150
		private Texture[] optionsLabelTextures = new Texture[3];

		// Token: 0x04000097 RID: 151
		protected readonly int[] optionLabelX = new int[]
		{
			500, 650, 500, 650, 500, 650, 500, 650, 275, 350,
			425, 500, 575, 650, 725, 325, 400, 475, 550, 625,
			700, 275, 525
		};

		// Token: 0x04000098 RID: 152
		protected readonly int[] optionLabelY = new int[]
		{
			35, 35, 65, 65, 95, 95, 125, 125, 170, 170,
			170, 170, 170, 170, 170, 215, 215, 215, 215, 215,
			215, 260, 260
		};

		// Token: 0x04000099 RID: 153
		protected readonly int[] optionSourceX = new int[]
		{
			0, 25, 0, 25, 0, 25, 0, 25, 50, 75,
			100, 125, 150, 175, 200, 225, 245, 265, 285, 305,
			325, 0, 125
		};

		// Token: 0x0400009A RID: 154
		protected readonly int[] optionSourceY = new int[]
		{
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			25,
			25
		};

		// Token: 0x0400009B RID: 155
		protected readonly int[] optionSourceWidth = new int[]
		{
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 20, 20, 20, 20, 20,
			20, 125, 100
		};

		// Token: 0x0400009C RID: 156
		protected readonly int[] optionSourceHeight = new int[]
		{
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25
		};

		// Token: 0x0400009D RID: 157
		private int optionCursorFrame;

		// Token: 0x0400009E RID: 158
		protected readonly int[] optionCursorFrames = new int[] { 0, 1, 2, 2, 1, 0 };

		// Token: 0x0400009F RID: 159
		protected readonly Size optionsLabelSize = new Size(345, 50);

		// Token: 0x040000A0 RID: 160
		private bool[] optionItemVisible = new bool[23];

		// Token: 0x040000A1 RID: 161
		private int optionSelection;

		// Token: 0x040000A2 RID: 162
		private int returnMode;

		// Token: 0x040000A3 RID: 163
		private int episodeFadeCounter;

		// Token: 0x040000A4 RID: 164
		private bool episodeFadeIn;

		// Token: 0x040000A5 RID: 165
		private Texture episodeBackTexture;

		// Token: 0x040000A6 RID: 166
		private Texture episodeMenuTexture;

		// Token: 0x040000A7 RID: 167
		private int episodeSelection;

		// Token: 0x040000A8 RID: 168
		protected readonly int[] episodeColumnX = new int[] { 100, 420 };

		// Token: 0x040000A9 RID: 169
		protected readonly Size episodeMenuSize = new Size(560, 476);

		// Token: 0x040000AA RID: 170
		private int albumFadeCounter;

		// Token: 0x040000AB RID: 171
		private bool albumFadeIn;

		// Token: 0x040000AC RID: 172
		private Texture albumBackTexture;

		// Token: 0x040000AD RID: 173
		private Texture albumPictureTexture;

		// Token: 0x040000AE RID: 174
		private int albumSelection;

		// Token: 0x040000AF RID: 175
		protected readonly int[] albumThumbX = new int[] { 360, 500, 640, 500, 640, 500, 640 };

		// Token: 0x040000B0 RID: 176
		protected readonly int[] albumThumbY = new int[] { 120, 120, 120, 240, 240, 360, 360 };

		// Token: 0x040000B1 RID: 177
		protected readonly int albumThumbDimAlpha = 128;

		// Token: 0x040000B2 RID: 178
		protected readonly string[] albumTitles = new string[] { "１：Ｅｉｎｓａｔｚ", "２：Ａｕｆｔａｋｔ", "３：Ａｇｏｇｉｋ", "４：ｆｏｃｏ", "５：ｃｏｎｔｅｎｅｒｅｚｚａ", "６：Ｇ．Ｐ．", "７：Ａａｂｓｔｕｆｅｎ" };

		// Token: 0x040000B3 RID: 179
		protected readonly Point textOrigin = new Point(40, 20);

		// Token: 0x040000B4 RID: 180
		protected readonly Size charCellSize = new Size(30, 38);

		// Token: 0x040000B5 RID: 181
		private Microsoft.DirectX.Direct3D.Font d3dFont;

		// Token: 0x040000B6 RID: 182
		private int albumGroup;

		// Token: 0x040000B7 RID: 183
		private int albumImageIndex;

		// Token: 0x040000B8 RID: 184
		private int albumImageTransition;

		// Token: 0x040000B9 RID: 185
		private Texture albumCurrentTexture;

		// Token: 0x040000BA RID: 186
		private Texture albumPrevTexture;

		// Token: 0x040000BB RID: 187
		protected readonly int[] albumImageCounts = new int[] { 3, 2, 1, 1, 1, 1, 2 };

		// Token: 0x040000BC RID: 188
		private readonly Size albumPictureSize = new Size(120, 720);
	}
}
