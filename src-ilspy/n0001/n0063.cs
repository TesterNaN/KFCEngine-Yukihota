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
using n0062;

namespace n0001
{
	public class n0063 : Form
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

		private Microsoft.DirectX.DirectInput.Device n0131;

		private bool[] n0130 = new bool[10];

		private EffectObject n0132;

		private Point n0145;

		private int n0066;

		private n0247 n0156;

		private Point n0134;

		private n0068 n0067;

		private Microsoft.DirectX.Direct3D.Device n0133;

		private Microsoft.DirectX.DirectDraw.Device n0146;

		private bool n0148;

		private bool n0149;

		private Sprite n0144;

		protected Texture n0135;

		private static bool n0125;

		private bool n0205 = false;

		private n0128 n0136;

		private n0128 n0152;

		private n0128 n0153;

		private string n0037 = "黑体";

		private bool n0038 = true;

		private n0040 n0039 = n0040.n0041;

		private bool n0045 = true;

		private bool n0232 = true;

		private bool n0233 = true;

		private bool n0234 = true;

		private bool n0235 = true;

		private Color n0236 = Color.FromArgb(128, 128, 255);

		private int n0237 = 4;

		private bool n0238 = true;

		private int n0158 = 0;

		private int n0160 = 0;

		private IContainer n0011;

		private Timer n0069;

		private Timer n0070;

		private Timer n0071;

		private ContextMenu n0072;

		private MenuItem n0073;

		private MenuItem n0074;

		private MenuItem n0075;

		private MenuItem n0076;

		private MenuItem n0077;

		private MenuItem n0078;

		private MenuItem n0079;

		private MenuItem n0080;

		private MenuItem n0081;

		private MenuItem n0082;

		private MenuItem n0083;

		private MenuItem n0084;

		private MenuItem n0085;

		private MenuItem n0086;

		private MenuItem n0087;

		private MenuItem n0088;

		private MenuItem n0089;

		private MenuItem n0090;

		private MenuItem n0091;

		private MenuItem n0092;

		private Timer n0093;

		private Timer n0094;

		private Timer n0095;

		private PictureBox n0096;

		private FontDialog n0017;

		private bool n0169 = false;

		private bool n0187 = false;

		private bool n0196 = false;

		private bool n0198 = false;

		private bool n0199 = false;

		private bool n0201 = false;

		private bool n0203 = false;

		protected ArrayList n0216 = new ArrayList();

		protected ArrayList n0217 = new ArrayList();

		protected ArrayList n0218 = new ArrayList();

		protected ArrayList n0219 = new ArrayList();

		protected ArrayList n0220 = new ArrayList();

		protected string n0221;

		protected readonly int[] n0271 = new int[3] { 5, 15, 20 };

		private int n0268;

		private Texture n0267;

		private n0068 n0163;

		private int n0170;

		private bool n0275;

		private Texture[] n0272 = new Texture[3];

		private Texture n0273;

		private int n0171;

		protected readonly Size n0274 = new Size(360, 192);

		private bool[] n0172 = new bool[5];

		private int n0178;

		private bool n0286;

		private Texture[] n0281 = new Texture[3];

		private Texture[] n0282 = new Texture[3];

		protected readonly int[] n0293 = new int[23]
		{
			500, 650, 500, 650, 500, 650, 500, 650, 275, 350,
			425, 500, 575, 650, 725, 325, 400, 475, 550, 625,
			700, 275, 525
		};

		protected readonly int[] n0294 = new int[23]
		{
			35, 35, 65, 65, 95, 95, 125, 125, 170, 170,
			170, 170, 170, 170, 170, 215, 215, 215, 215, 215,
			215, 260, 260
		};

		protected readonly int[] n0289 = new int[23]
		{
			0, 25, 0, 25, 0, 25, 0, 25, 50, 75,
			100, 125, 150, 175, 200, 225, 245, 265, 285, 305,
			325, 0, 125
		};

		protected readonly int[] n0290 = new int[23]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 25, 25
		};

		protected readonly int[] n0291 = new int[23]
		{
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 20, 20, 20, 20, 20,
			20, 125, 100
		};

		protected readonly int[] n0292 = new int[23]
		{
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25, 25, 25, 25, 25, 25, 25, 25,
			25, 25, 25
		};

		private int n0280;

		protected readonly int[] n0295 = new int[6] { 0, 1, 2, 2, 1, 0 };

		protected readonly Size n0283 = new Size(345, 50);

		private bool[] n0288 = new bool[23];

		private int n0179;

		private int n0167;

		private int n0175;

		private bool n0300;

		private Texture n0297;

		private Texture n0298;

		private int n0176;

		protected readonly int[] n0301 = new int[2] { 100, 420 };

		protected readonly Size n0299 = new Size(560, 476);

		private int n0181;

		private bool n0305;

		private Texture n0302;

		private Texture n0303;

		private int n0186;

		protected readonly int[] n0311 = new int[7] { 360, 500, 640, 500, 640, 500, 640 };

		protected readonly int[] n0312 = new int[7] { 120, 120, 120, 240, 240, 360, 360 };

		protected readonly int n0313 = 128;

		protected readonly string[] n0314 = new string[7] { "１：Ｅｉｎｓａｔｚ", "２：Ａｕｆｔａｋｔ", "３：Ａｇｏｇｉｋ", "４：ｆｏｃｏ", "５：ｃｏｎｔｅｎｅｒｅｚｚａ", "６：Ｇ．Ｐ．", "７：Ａａｂｓｔｕｆｅｎ" };

		protected readonly Point n0223 = new Point(40, 20);

		protected readonly Size n0224 = new Size(30, 38);

		private Microsoft.DirectX.Direct3D.Font n0222;

		private int n0183;

		private int n0185;

		private int n0182;

		private Texture n0307;

		private Texture n0306;

		protected readonly int[] n0310 = new int[7] { 3, 2, 1, 1, 1, 1, 2 };

		private readonly Size n0304 = new Size(120, 720);

		public n0063()
		{
			n0003();
			base.GotFocus += n0064;
			base.LostFocus += n0065;
			n0066 = -2;
			n0067 = new n0068("MENU");
		}

		protected override void Dispose(bool n0010)
		{
			if (n0010 && n0011 != null)
			{
				n0011.Dispose();
			}
			base.Dispose(n0010);
		}

		private void n0003()
		{
			n0011 = new Container();
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(n0063));
			n0069 = new Timer(n0011);
			n0070 = new Timer(n0011);
			n0071 = new Timer(n0011);
			n0072 = new ContextMenu();
			n0073 = new MenuItem();
			n0074 = new MenuItem();
			n0075 = new MenuItem();
			n0076 = new MenuItem();
			n0077 = new MenuItem();
			n0078 = new MenuItem();
			n0079 = new MenuItem();
			n0080 = new MenuItem();
			n0081 = new MenuItem();
			n0082 = new MenuItem();
			n0083 = new MenuItem();
			n0084 = new MenuItem();
			n0085 = new MenuItem();
			n0086 = new MenuItem();
			n0087 = new MenuItem();
			n0088 = new MenuItem();
			n0089 = new MenuItem();
			n0090 = new MenuItem();
			n0091 = new MenuItem();
			n0092 = new MenuItem();
			n0017 = new FontDialog();
			n0093 = new Timer(n0011);
			n0094 = new Timer(n0011);
			n0095 = new Timer(n0011);
			n0096 = new PictureBox();
			base.SuspendLayout();
			n0069.Interval = 50;
			n0069.Tick += n0097;
			n0070.Interval = 50;
			n0070.Tick += n0098;
			n0071.Interval = 50;
			n0071.Tick += n0099;
			n0072.MenuItems.AddRange(new MenuItem[17]
			{
				n0073, n0074, n0075, n0076, n0077, n0078, n0079, n0080, n0081, n0082,
				n0083, n0087, n0088, n0089, n0090, n0091, n0092
			});
			n0073.Index = 0;
			n0073.Text = "取消(&C)";
			n0073.Click += n0100;
			n0074.Index = 1;
			n0074.Text = "-";
			n0075.Index = 2;
			n0075.Text = "选项(&O)\tF8";
			n0075.Click += n0101;
			n0076.Index = 3;
			n0076.Text = "-";
			n0077.Index = 4;
			n0077.Text = "显示模式(&M)\tAlt+Enter";
			n0077.Click += n0102;
			n0078.Index = 5;
			n0078.Text = "-";
			n0079.Index = 6;
			n0079.Text = "快进(&Q)\tF3";
			n0079.Click += n0103;
			n0080.Index = 7;
			n0080.Text = "自动播放(&A)\tF4";
			n0080.Click += n0104;
			n0081.Index = 8;
			n0081.Text = "隐藏对话(&H)\tF2";
			n0081.Click += n0105;
			n0082.Index = 9;
			n0082.Text = "-";
			n0083.Index = 10;
			n0083.MenuItems.AddRange(new MenuItem[3] { n0084, n0085, n0086 });
			n0083.Text = "语言(&L)\tF12";
			n0084.Index = 0;
			n0084.RadioCheck = true;
			n0084.Text = "简体中文(&S)";
			n0084.Click += n0106;
			n0085.Index = 1;
			n0085.RadioCheck = true;
			n0085.Text = "繁體中文(&T)";
			n0085.Click += n0107;
			n0086.Enabled = false;
			n0086.Index = 2;
			n0086.RadioCheck = true;
			n0086.Text = "日本語(&J)";
			n0086.Click += n0108;
			n0087.Index = 11;
			n0087.Text = "-";
			n0088.Index = 12;
			n0088.Text = "字体(&F)";
			n0088.Click += n0109;
			n0089.Index = 13;
			n0089.Text = "-";
			n0090.Index = 14;
			n0090.Text = "返回主菜单(&B)";
			n0090.Click += n0110;
			n0091.Index = 15;
			n0091.Text = "-";
			n0092.Index = 16;
			n0092.Text = "结束游戏(&E)\tAlt+F4";
			n0092.Click += n0111;
			n0017.AllowVerticalFonts = false;
			n0017.Font = new System.Drawing.Font("黑体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			n0017.FontMustExist = true;
			n0017.MaxSize = 24;
			n0017.MinSize = 24;
			n0017.ShowEffects = false;
			n0093.Interval = 50;
			n0093.Tick += n0112;
			n0094.Interval = 50;
			n0094.Tick += n0113;
			n0095.Interval = 50;
			n0095.Tick += n0114;
			n0096.BackColor = Color.White;
			n0096.Location = new Point(0, 0);
			n0096.Name = "ptbMovie";
			n0096.Size = new Size(800, 600);
			n0096.TabIndex = 0;
			n0096.TabStop = false;
			n0096.Visible = false;
			AutoScaleBaseSize = new Size(6, 14);
			BackColor = Color.White;
			ClientSize = new Size(800, 600);
			base.Controls.Add(n0096);
			Font = new System.Drawing.Font("黑体", 9f, FontStyle.Regular, GraphicsUnit.Point, 134);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			Icon = (Icon)resourceManager.GetObject("$this.Icon");
			KeyPreview = true;
			MaximizeBox = false;
			base.Name = "GalGame";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "秋之回忆 ～雪萤～";
			base.KeyDown += n0115;
			base.MouseDown += n0116;
			Closing += n0117;
			Load += n0118;
			base.KeyUp += n0119;
			Activated += n0120;
			base.Paint += n0121;
			base.MouseMove += n0122;
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
			Application.Run(new n0063());
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
			n0002 n880 = new n0002();
			n880.n0048(new System.Drawing.Font(n0037, 24f));
			n880.n0055(n0038);
			n880.n0057(n0039);
			n880.n0059(n0045);
			if (n0045 && n880.ShowDialog() == DialogResult.Cancel)
			{
				Application.Exit();
				return;
			}
			n0037 = n880.n0047().Name;
			n0038 = n880.n0054();
			n0039 = n880.n0056();
			n0045 = n880.n0058();
			n0128.n0129("Mullen");
			for (int i = 0; i < 8; i++)
			{
				n0130[i] = false;
			}
			n0131 = n880.n0050();
			if (n0131 != null)
			{
				try
				{
					n0131.SetDataFormat(DeviceDataFormat.Joystick);
					n0131.SetCooperativeLevel(this, Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Foreground | Microsoft.DirectX.DirectInput.CooperativeLevelFlags.Exclusive);
					foreach (object @object in n0131.Objects)
					{
						DeviceObjectInstance deviceObjectInstance = (DeviceObjectInstance)@object;
						if ((deviceObjectInstance.ObjectId & 3) != 0)
						{
							n0131.Properties.SetRange(ParameterHow.ById, deviceObjectInstance.ObjectId, new InputRange(-1000, 1000));
						}
					}
					if (n880.n0052())
					{
						int[] array = null;
						foreach (object object2 in n0131.GetObjects(DeviceObjectTypeFlags.Axis))
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
						IEnumerator enumerator2 = n0131.GetEffects(EffectType.All).GetEnumerator();
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
								n0132 = new EffectObject(effectInformation.EffectGuid, die, n0131);
							}
						}
					}
					else
					{
						n0132 = null;
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
				n0133 = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, this, Microsoft.DirectX.Direct3D.CreateFlags.SoftwareVertexProcessing, presentParameters);
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.Message);
				Application.Exit();
				return;
			}
			n0144 = new Sprite(n0133);
			n0145 = base.Location;
			n0146 = new Microsoft.DirectX.DirectDraw.Device();
			n0147(n880.n0054());
			n0148 = false;
			n0149 = false;
			try
			{
				n0136 = new n0128("System", n0150.n0151);
				n0152 = new n0128("Bgm", n0150.n0151);
				n0153 = new n0128("Bg", n0150.n0151);
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
				n0152.n0154("BGM12", tempPath);
				n0067.n0155(tempPath + "\\BGM12.wav");
			}
			catch (Exception ex4)
			{
				MessageBox.Show(ex4.Message);
				Application.Exit();
				return;
			}
			n0044(n0039);
			switch (n0039)
			{
			case n0040.n0041:
				n0084.Checked = true;
				break;
			case n0040.n0042:
				n0085.Checked = true;
				break;
			case n0040.n0043:
				n0086.Checked = true;
				break;
			}
			n0134 = Cursor.Position;
			n0135 = TextureLoader.FromStream(n0133, n0136.n0137("WHITE"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
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

		private void n0117(object n0035, CancelEventArgs n0036)
		{
			if (n0066 == 1)
			{
				if (n0156.n0157() > n0158)
				{
					n0158 = n0156.n0157();
				}
				if (n0156.n0159() > n0160)
				{
					n0160 = n0156.n0159();
				}
			}
			n0161();
			if (n0131 != null)
			{
				try
				{
					if (null != n0132)
					{
						n0132.Stop();
					}
					n0131.Unacquire();
					n0131.Properties.AutoCenter = true;
				}
				catch (InputException)
				{
				}
			}
			string tempPath = Path.GetTempPath();
			n0067.n0162();
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
			if (n0131 != null)
			{
				try
				{
					n0131.Acquire();
				}
				catch (InputException)
				{
				}
			}
		}

		private void n0064(object n0035, EventArgs n0036)
		{
			if (n0148)
			{
				n0147(true);
				WindowState = FormWindowState.Normal;
			}
			switch (n0066)
			{
			case -2:
				if (n0163 != null)
				{
					n0163.n0164();
				}
				break;
			case -1:
				n0093.Start();
				break;
			case 0:
				n0070.Start();
				n0067.n0165();
				break;
			case 1:
				n0069.Start();
				n0156.n0166(false, true);
				break;
			case 2:
				n0094.Start();
				n0067.n0165();
				break;
			case 3:
				n0071.Start();
				if (n0167 == 0)
				{
					n0067.n0165();
				}
				else if (n0167 == 1)
				{
					n0156.n0166(false, true);
				}
				break;
			case 4:
				n0095.Start();
				n0067.n0165();
				break;
			}
		}

		protected override void OnLostFocus(EventArgs n0036)
		{
			if (!n0149)
			{
				base.OnLostFocus(n0036);
			}
		}

		private void n0065(object n0035, EventArgs n0036)
		{
			n0148 = FormBorderStyle == FormBorderStyle.None;
			if (n0148)
			{
				n0147(false);
				WindowState = FormWindowState.Minimized;
			}
			switch (n0066)
			{
			case -2:
				if (n0163 != null)
				{
					n0163.n0166();
				}
				break;
			case -1:
				n0093.Stop();
				break;
			case 0:
				n0070.Stop();
				n0067.n0166();
				break;
			case 1:
				n0069.Stop();
				n0156.n0166(true, true);
				break;
			case 2:
				n0094.Stop();
				n0067.n0166();
				break;
			case 3:
				n0071.Stop();
				if (n0167 == 0)
				{
					n0067.n0166();
				}
				else if (n0167 == 1)
				{
					n0156.n0166(true, true);
				}
				break;
			case 4:
				n0095.Stop();
				n0067.n0166();
				break;
			}
		}

		private void n0115(object n0035, KeyEventArgs n0036)
		{
			switch (n0036.KeyCode)
			{
			case Keys.ControlKey:
				if (n0066 == 1)
				{
					n0156.n0168(true, true);
				}
				break;
			case Keys.Return:
				if (n0169)
				{
					break;
				}
				n0169 = true;
				if (n0036.Alt)
				{
					n0147();
					break;
				}
				switch (n0066)
				{
				case 0:
					if (n0170 == 0 && n0171 > 0 && n0172[n0171 - 1])
					{
						n0173();
					}
					break;
				case 1:
					n0156.n0174();
					break;
				case 2:
					if (n0175 == 0 && n0176 > 0 && n0158 >= n0176)
					{
						n0177();
					}
					break;
				case 3:
					if (n0178 == 0 && n0179 > 0)
					{
						n0180(n0179 - 1);
					}
					break;
				case 4:
					if (n0181 == 0 && n0182 == 0)
					{
						if (n0183 != 0)
						{
							n0184(n0183, n0185 + 1);
						}
						else if (n0186 > 0 && n0160 >= n0186)
						{
							n0184(n0186, 0);
						}
					}
					break;
				}
				break;
			case Keys.Space:
				if (n0187)
				{
					break;
				}
				n0187 = true;
				switch (n0066)
				{
				case 1:
					if (!n0156.n0188())
					{
						n0072.Show(this, new Point(400, 300));
					}
					break;
				case 2:
					if (n0175 == 0)
					{
						n0176 = 0;
						n0177();
					}
					break;
				case 3:
					if (n0178 == 0)
					{
						n0189();
					}
					break;
				case 4:
					if (n0181 == 0 && n0182 == 0)
					{
						if (n0183 == 0)
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
				switch (n0066)
				{
				case 3:
					n0192(0);
					break;
				case 4:
					if (n0183 == 0)
					{
						n0193(0);
					}
					break;
				}
				break;
			case Keys.Up:
				switch (n0066)
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
				switch (n0066)
				{
				case 3:
					n0192(1);
					break;
				case 4:
					if (n0183 == 0)
					{
						n0193(1);
					}
					break;
				}
				break;
			case Keys.Down:
				switch (n0066)
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
				if (!n0196)
				{
					n0196 = true;
					if (n0066 == 1)
					{
						n0156.n0197();
					}
				}
				break;
			case Keys.F3:
				if (!n0198)
				{
					n0198 = true;
					if (n0066 == 1)
					{
						n0156.n0168();
					}
				}
				break;
			case Keys.F4:
				if (!n0199)
				{
					n0199 = true;
					if (n0066 == 1)
					{
						n0156.n0200();
					}
				}
				break;
			case Keys.F8:
				if (!n0201)
				{
					n0201 = true;
					if (n0066 == 1 && !n0156.n0188())
					{
						n0156.n0200(false);
						n0156.n0166(true, false);
						n0069.Stop();
						n0140();
						n0202();
					}
				}
				break;
			case Keys.F12:
				if (n0203)
				{
					break;
				}
				n0203 = true;
				if (n0066 == 1)
				{
					n0039++;
					if (n0039 == n0040.n0204)
					{
						n0039 = n0040.n0041;
					}
					if (!n0156.n0188() && n0039 == n0040.n0043)
					{
						n0039 = n0040.n0041;
					}
					if (n0156.n0188())
					{
						n0205 = true;
					}
					n0084.Checked = false;
					n0085.Checked = false;
					n0086.Checked = false;
					switch (n0039)
					{
					case n0040.n0041:
						n0084.Checked = true;
						break;
					case n0040.n0042:
						n0085.Checked = true;
						break;
					case n0040.n0043:
						n0085.Checked = true;
						break;
					}
					n0044(n0039);
					n0156.n0206(n0039);
				}
				break;
			}
		}

		private void n0119(object n0035, KeyEventArgs n0036)
		{
			switch (n0036.KeyCode)
			{
			case Keys.Return:
				n0169 = false;
				break;
			case Keys.ControlKey:
				if (n0066 == 1)
				{
					n0156.n0168(false, true);
				}
				break;
			case Keys.Space:
				n0187 = false;
				break;
			case Keys.F2:
				n0196 = false;
				break;
			case Keys.F3:
				n0198 = false;
				break;
			case Keys.F4:
				n0199 = false;
				break;
			case Keys.F8:
				n0201 = false;
				break;
			case Keys.F12:
				n0203 = false;
				break;
			}
		}

		private void n0116(object n0035, MouseEventArgs n0036)
		{
			switch (n0066)
			{
			case 0:
				n0207(n0035, n0036);
				break;
			case 1:
				if (n0036.Button == MouseButtons.Left)
				{
					n0156.n0174();
				}
				else if (n0036.Button == MouseButtons.Right && !n0156.n0188())
				{
					n0156.n0166(true, false);
					n0069.Stop();
					n0072.Show((Control)n0035, new Point(n0036.X, n0036.Y));
					n0156.n0166(false, false);
					n0069.Start();
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
			switch (n0066)
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

		private void n0121(object n0035, PaintEventArgs n0036)
		{
			if (n0066 == -2)
			{
				return;
			}
			if (n0066 == 1)
			{
				n0156.n0215();
				return;
			}
			n0133.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0133.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			for (int i = 0; i < n0216.Count; i++)
			{
				if (n0216[i] != null)
				{
					n0144.Draw2D((Texture)n0216[i], (Rectangle)n0217[i], (Rectangle)n0218[i], Point.Empty, 0f, (Point)n0219[i], (Color)n0220[i]);
					continue;
				}
				for (int j = 0; j < n0221.Length; j++)
				{
					Microsoft.DirectX.Direct3D.Font font = n0222;
					Sprite sprite = n0144;
					string text = n0221[j].ToString();
					int num = n0223.X;
					font.DrawText(sprite, text, num + n0224.Width * j + 2, n0223.Y + 2, Color.Black);
					Microsoft.DirectX.Direct3D.Font font2 = n0222;
					Sprite sprite2 = n0144;
					string text2 = n0221[j].ToString();
					int num2 = n0223.X;
					font2.DrawText(sprite2, text2, num2 + n0224.Width * j, n0223.Y, Color.White);
				}
			}
			n0144.End();
			n0133.EndScene();
			try
			{
				n0133.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0133.TestCooperativeLevel();
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
			switch (n0226())
			{
			case 0:
				n0156.n0174();
				break;
			case 1:
				n0156.n0197();
				break;
			case 2:
				n0156.n0168();
				break;
			case 3:
				n0156.n0200();
				break;
			case 8:
				if (!n0156.n0188())
				{
					n0156.n0200(false);
					n0156.n0166(true, false);
					n0069.Stop();
					n0140();
					n0202();
					flag = true;
				}
				break;
			case 9:
				if (!n0156.n0188())
				{
					n0227();
					flag = true;
				}
				break;
			}
			if (n0205 && n0039 == n0040.n0043 && !n0156.n0188())
			{
				n0039 = n0040.n0041;
				n0084.Checked = true;
				n0085.Checked = false;
				n0086.Checked = false;
				n0044(n0039);
				n0156.n0206(n0039);
				n0205 = false;
			}
			if (!flag && !n0156.n0228())
			{
				n0227();
			}
		}

		private void n0100(object n0035, EventArgs n0036)
		{
		}

		private void n0101(object n0035, EventArgs n0036)
		{
			n0156.n0200(false);
			n0156.n0166(true, false);
			n0069.Stop();
			n0140();
			n0202();
		}

		private void n0102(object n0035, EventArgs n0036)
		{
			n0147();
		}

		private void n0103(object n0035, EventArgs n0036)
		{
			n0156.n0168();
		}

		private void n0104(object n0035, EventArgs n0036)
		{
			n0156.n0200();
		}

		private void n0105(object n0035, EventArgs n0036)
		{
			n0156.n0197();
		}

		private void n0106(object n0035, EventArgs n0036)
		{
			n0084.Checked = true;
			n0085.Checked = false;
			n0086.Checked = false;
			n0039 = n0040.n0041;
			n0044(n0040.n0041);
			n0156.n0206(n0040.n0041);
		}

		private void n0107(object n0035, EventArgs n0036)
		{
			n0084.Checked = false;
			n0085.Checked = true;
			n0086.Checked = false;
			n0039 = n0040.n0042;
			n0044(n0040.n0042);
			n0156.n0206(n0040.n0042);
		}

		private void n0108(object n0035, EventArgs n0036)
		{
			n0084.Checked = false;
			n0085.Checked = false;
			n0086.Checked = true;
			n0039 = n0040.n0043;
			n0044(n0040.n0043);
			n0156.n0206(n0040.n0043);
		}

		private void n0109(object n0035, EventArgs n0036)
		{
			if (n0017.ShowDialog() == DialogResult.OK)
			{
				n0156.n0229(n0017.Font);
				n0037 = n0017.Font.Name;
			}
		}

		private void n0110(object n0035, EventArgs n0036)
		{
			n0227();
		}

		private void n0111(object n0035, EventArgs n0036)
		{
			n0156.n0230();
			n0156.Dispose();
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
			n0231(xmlDocument, "sys/win/font", ref n0037);
			string n880 = null;
			n0231(xmlDocument, "sys/win/mode", ref n880);
			switch (n880)
			{
			case "Fullscreen":
				n0038 = true;
				break;
			case "Windowed":
				n0038 = false;
				break;
			}
			n880 = null;
			n0231(xmlDocument, "sys/win/language", ref n880);
			switch (n880)
			{
			case "SC":
				n0039 = n0040.n0041;
				break;
			case "TC":
				n0039 = n0040.n0042;
				break;
			case "JP":
				n0039 = n0040.n0043;
				break;
			}
			n0231(xmlDocument, "sys/win/show", ref n0045);
			n0231(xmlDocument, "sys/options/fade", ref n0232);
			n0231(xmlDocument, "sys/options/env", ref n0233);
			n0231(xmlDocument, "sys/options/sun", ref n0234);
			n0231(xmlDocument, "sys/options/shock", ref n0235);
			n880 = null;
			n0231(xmlDocument, "sys/options/color", ref n880);
			switch (n880)
			{
			case "Green":
				n0236 = Color.FromArgb(128, 255, 128);
				break;
			case "Blue":
				n0236 = Color.FromArgb(128, 128, 255);
				break;
			case "Red":
				n0236 = Color.FromArgb(255, 128, 128);
				break;
			case "Yellow":
				n0236 = Color.FromArgb(255, 255, 128);
				break;
			case "Purple":
				n0236 = Color.FromArgb(255, 128, 255);
				break;
			case "Cyan":
				n0236 = Color.FromArgb(128, 255, 255);
				break;
			case "Black":
				n0236 = Color.FromArgb(128, 128, 128);
				break;
			}
			n0231(xmlDocument, "sys/options/speed", 1, 6, ref n0237);
			n0231(xmlDocument, "sys/options/synchro", ref n0238);
			n0231(xmlDocument, "sys/save/episode", 0, 16, ref n0158);
			n0231(xmlDocument, "sys/save/album", 0, 7, ref n0160);
		}

		private void n0161()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("sys"));
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("win"));
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("font")).InnerText = n0037;
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("mode")).InnerText = (n0038 ? "Fullscreen" : "Windowed");
			switch (n0039)
			{
			case n0040.n0041:
				xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("language")).InnerText = "SC";
				break;
			case n0040.n0042:
				xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("language")).InnerText = "TC";
				break;
			default:
				xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("language")).InnerText = "SC";
				break;
			}
			xmlDocument.SelectSingleNode("/sys/win").AppendChild(xmlDocument.CreateElement("show")).InnerText = n0045.ToString();
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("options"));
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("fade")).InnerText = n0232.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("env")).InnerText = n0233.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("sun")).InnerText = n0234.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("shock")).InnerText = n0235.ToString();
			string innerText;
			if (n0236 == Color.FromArgb(128, 255, 128))
			{
				innerText = "Green";
			}
			else if (n0236 == Color.FromArgb(128, 128, 255))
			{
				innerText = "Blue";
			}
			else if (n0236 == Color.FromArgb(255, 128, 128))
			{
				innerText = "Red";
			}
			else if (n0236 == Color.FromArgb(255, 255, 128))
			{
				innerText = "Yellow";
			}
			else if (n0236 == Color.FromArgb(255, 128, 255))
			{
				innerText = "Purple";
			}
			else if (n0236 == Color.FromArgb(128, 255, 255))
			{
				innerText = "Cyan";
			}
			else
			{
				innerText = ((!(n0236 == Color.FromArgb(128, 128, 128))) ? "Blue" : "Black");
			}
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("color")).InnerText = innerText;
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("speed")).InnerText = n0237.ToString();
			xmlDocument.SelectSingleNode("/sys/options").AppendChild(xmlDocument.CreateElement("synchro")).InnerText = n0238.ToString();
			xmlDocument.DocumentElement.AppendChild(xmlDocument.CreateElement("save"));
			xmlDocument.SelectSingleNode("/sys/save").AppendChild(xmlDocument.CreateElement("episode")).InnerText = n0158.ToString();
			xmlDocument.SelectSingleNode("/sys/save").AppendChild(xmlDocument.CreateElement("album")).InnerText = n0160.ToString();
			try
			{
				xmlDocument.Save("save_sys.dat");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n游戏保存可能失败");
			}
		}

		private void n0231(XmlDocument n0239, string n0240, ref string n0241)
		{
			XmlNode xmlNode = n0239.SelectSingleNode(n0240);
			if (xmlNode != null)
			{
				n0241 = xmlNode.InnerText;
			}
		}

		private void n0231(XmlDocument n0239, string n0240, ref bool n0241)
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

		private void n0231(XmlDocument n0239, string n0240, int n0242, int n0243, ref int n0241)
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
			n0133.Reset(presentParameters);
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
				n0145 = base.Location;
				base.Location = new Point(0, 0);
				n0146.SetDisplayMode(800, 600, 32, 0, true);
				base.Width = 800;
				base.Height = 600;
			}
			else
			{
				n0146.RestoreDisplayMode();
				FormBorderStyle = FormBorderStyle.FixedSingle;
				base.Location = n0145;
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

		private void n0044(n0040 n0046)
		{
			switch (n0046)
			{
			case n0040.n0041:
				n0073.Text = "取消(&C)";
				n0075.Text = "选项(&O)\tF8";
				n0077.Text = "显示模式(&M)\tAlt+Enter";
				n0079.Text = "快进(&Q)\tF3";
				n0080.Text = "自动播放(&A)\tF4";
				n0081.Text = "隐藏对话(&H)\tF2";
				n0083.Text = "语言(&L)";
				n0088.Text = "字体(&F)";
				n0090.Text = "返回主菜单(&B)";
				n0092.Text = "结束游戏(&E)\tAlt+F4";
				Text = "秋之回忆 ～雪萤～";
				break;
			case n0040.n0042:
				n0073.Text = "取消(&C)";
				n0075.Text = "選項(&O)\tF8";
				n0077.Text = "顯示模式(&M)\tAlt+Enter";
				n0079.Text = "快進(&Q)\tF3";
				n0080.Text = "自動播放(&A)\tF4";
				n0081.Text = "隱藏對話(&H)\tF2";
				n0083.Text = "語言(&L)";
				n0088.Text = "字體(&F)";
				n0090.Text = "返回主菜單(&B)";
				n0092.Text = "結束遊戲(&E)\tAlt+F4";
				Text = "秋之回憶 ～雪螢～";
				break;
			case n0040.n0043:
				n0073.Text = "戻る(&C)";
				n0075.Text = "オプション設定(&O)\tF8";
				n0077.Text = "画面モード(&M)\tAlt+Enter";
				n0079.Text = "スキップ(&Q)\tF3";
				n0080.Text = "オートモード(&A)\tF4";
				n0081.Text = "ウインドウ消去(&H)\tF2";
				n0083.Text = "言語(&L)";
				n0088.Text = "フォント(&F)";
				n0090.Text = "タイトルに戻る(&B)";
				n0092.Text = "ゲーム終了(&E)\tAlt+F4";
				Text = "メモリーズオフデュエット ～雪蛍～";
				break;
			}
		}

		private int n0226()
		{
			if (n0131 != null)
			{
				try
				{
					n0131.Poll();
				}
				catch (InputException ex)
				{
					if (ex is NotAcquiredException || ex is InputLostException)
					{
						try
						{
							n0131.Acquire();
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
					currentJoystickState = n0131.CurrentJoystickState;
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
						if (!n0130[i])
						{
							n0130[i] = true;
							return i;
						}
					}
					else
					{
						n0130[i] = false;
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
				n0156 = new n0247("Yukihota", n0246, n0133, n0144, n0225, n0135, n0132, new System.Drawing.Font(n0037, 24f), n0039, n0236);
				n0156.n0249(n0232);
				n0156.n0250(n0234);
				n0156.n0251(n0233);
				n0156.n0252(n0235);
				n0156.n0253(n0237);
				n0156.n0254(n0238);
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
			n0221 = null;
			n0066 = 1;
			n0067.n0230();
			n0255();
			n0256();
			n0257();
			n0258();
			n0259();
			n0069.Start();
		}

		private void n0227()
		{
			if (n0156.n0157() > n0158)
			{
				n0158 = n0156.n0157();
			}
			if (n0156.n0159() > n0160)
			{
				n0160 = n0156.n0159();
			}
			n0069.Stop();
			n0156.n0230();
			n0156.Dispose();
			n0156 = null;
			GC.Collect();
			n0139();
			n0140();
			n0141();
			n0142();
			n0143();
			n0260();
		}

		private void n0261(Texture n0262, Rectangle n0263, Rectangle n0264, Point n0265, Color n0266)
		{
			n0144.Draw2D(n0262, n0263, n0264, Point.Empty, 0f, n0265, n0266);
			n0216.Add(n0262);
			n0217.Add(n0263);
			n0218.Add(n0264);
			n0219.Add(n0265);
			n0220.Add(n0266);
		}

		private void n0141()
		{
			n0267 = TextureLoader.FromStream(n0133, n0136.n0137("KID_LOGO"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			if (n0125)
			{
				n0268 = 0;
				n0066 = -1;
			}
		}

		private void n0257()
		{
			n0267.Dispose();
			n0267 = null;
		}

		private void n0138()
		{
			n0066 = -2;
			if (!n0125)
			{
				n0096.Visible = true;
				n0163 = new n0068("Op");
				n0163.n0155("Movie\\KFC.wmv", n0096);
				n0163.n0164();
			}
			n0093.Start();
		}

		private void n0112(object n0035, EventArgs n0036)
		{
			if (n0066 == -2 && !n0125)
			{
				if (n0163.n0269() < n0163.n0270() && n0163.n0270() != -1)
				{
					return;
				}
				n0268 = 0;
				n0066 = -1;
				n0096.Visible = false;
				n0149 = true;
				n0163.n0162();
				n0149 = false;
				n0163 = null;
			}
			n0133.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			n0221 = null;
			n0133.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			int alpha;
			if (n0268 < n0271[0])
			{
				alpha = (int)((double)n0268 / (double)n0271[0] * 255.0);
			}
			else if (n0268 < n0271[1])
			{
				alpha = 255;
			}
			else if (n0268 < n0271[2])
			{
				alpha = (int)((double)(n0271[2] - n0268) / (double)(n0271[2] - n0271[1]) * 255.0);
			}
			else
			{
				n0093.Stop();
				n0260();
				alpha = 0;
			}
			n0261(n0267, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
			n0268++;
			n0144.End();
			n0133.EndScene();
			try
			{
				n0133.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0133.TestCooperativeLevel();
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
			n0272[0] = TextureLoader.FromStream(n0133, n0136.n0137("TITLESC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0272[1] = TextureLoader.FromStream(n0133, n0136.n0137("TITLETC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0272[2] = TextureLoader.FromStream(n0133, n0136.n0137("TITLEJP"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0273 = TextureLoader.FromStream(n0133, n0136.n0137("MENU"), n0274.Width, n0274.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0255()
		{
			for (int i = 0; i < 3; i++)
			{
				n0272[i].Dispose();
				n0272[i] = null;
			}
			n0273.Dispose();
			n0273 = null;
		}

		private void n0260()
		{
			n0066 = 0;
			n0170 = 10;
			n0275 = true;
			bool[] array = n0172;
			bool flag;
			n0172[2] = (flag = (n0172[4] = true));
			array[0] = flag;
			n0172[1] = n0158 > 0;
			n0172[3] = n0160 > 0;
			if (!n0067.n0276())
			{
				n0067.n0164(true);
			}
			n0070.Start();
		}

		private void n0173()
		{
			n0170 = 10;
			n0275 = false;
		}

		private void n0194(int n0277)
		{
			if (n0170 > 0)
			{
				return;
			}
			switch (n0277)
			{
			case 0:
				if (n0171 == 0)
				{
					n0171 = 5;
					break;
				}
				n0171--;
				if (n0171 == 0)
				{
					n0171 = 5;
				}
				break;
			case 1:
				if (n0171 == 0)
				{
					n0171 = 1;
					break;
				}
				n0171++;
				if (n0171 == 6)
				{
					n0171 = 1;
				}
				break;
			}
		}

		private void n0098(object n0035, EventArgs n0036)
		{
			GC.Collect();
			int num = n0226();
			if (num == 0)
			{
				if (n0171 > 0 && n0172[n0171 - 1])
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
			n0133.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			n0221 = null;
			n0133.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			n0261(n0272[(int)n0039], Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < 5; i++)
			{
				Rectangle n880 = new Rectangle((n0171 - 1 != i) ? 180 : 0, 32 * (n0172[i] ? i : 5), 180, 32);
				n0261(n0273, n880, Rectangle.Empty, new Point(310, 368 + 32 * i), Color.White);
			}
			if (n0170 > 0)
			{
				int alpha = (int)((double)(n0275 ? n0170 : (10 - n0170)) / 10.0 * 255.0);
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				n0170--;
				if (!n0275 && n0170 == 0)
				{
					n0070.Stop();
					switch (n0171)
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
					n0171 = 0;
				}
			}
			n0144.End();
			n0133.EndScene();
			try
			{
				n0133.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0133.TestCooperativeLevel();
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
			if (n0170 == 0 && n0036.Button == MouseButtons.Left)
			{
				n0134 = Point.Empty;
				n0211(n0035, n0036);
				if (n0171 > 0 && n0172[n0171 - 1])
				{
					n0173();
				}
			}
		}

		private void n0211(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(n0134))
			{
				return;
			}
			n0134 = Cursor.Position;
			if (n0170 == 0)
			{
				if (n0036.X >= 310 && n0036.X < 490 && n0036.Y >= 368 && n0036.Y < 528)
				{
					n0171 = (n0036.Y - 368) / 32 + 1;
				}
				else
				{
					n0171 = 0;
				}
			}
		}

		private void n0140()
		{
			n0280 = 0;
			n0281[0] = TextureLoader.FromStream(n0133, n0136.n0137("OPT_BGSC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0281[1] = TextureLoader.FromStream(n0133, n0136.n0137("OPT_BGTC"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0281[2] = TextureLoader.FromStream(n0133, n0136.n0137("OPT_BGJP"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] array = n0282;
			int num = 0;
			Microsoft.DirectX.Direct3D.Device device = n0133;
			Stream stream = n0136.n0137("OPTIONSC");
			int num2 = n0283.Width;
			array[num] = TextureLoader.FromStream(device, stream, num2, n0283.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] array2 = n0282;
			int num3 = 1;
			Microsoft.DirectX.Direct3D.Device device2 = n0133;
			Stream stream2 = n0136.n0137("OPTIONTC");
			int num4 = n0283.Width;
			array2[num3] = TextureLoader.FromStream(device2, stream2, num4, n0283.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			Texture[] array3 = n0282;
			int num5 = 2;
			Microsoft.DirectX.Direct3D.Device device3 = n0133;
			Stream stream3 = n0136.n0137("OPTIONJP");
			int num6 = n0283.Width;
			array3[num5] = TextureLoader.FromStream(device3, stream3, num6, n0283.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0256()
		{
			for (int i = 0; i < 3; i++)
			{
				n0281[i].Dispose();
				n0281[i] = null;
				n0282[i].Dispose();
				n0282[i] = null;
			}
		}

		private void n0202()
		{
			GC.Collect();
			n0167 = n0066;
			n0066 = 3;
			n0178 = 10;
			n0286 = true;
			n0071.Start();
		}

		private void n0189()
		{
			n0178 = 10;
			n0286 = false;
		}

		private void n0192(int n0277)
		{
			if (n0178 > 0)
			{
				return;
			}
			if (n0179 != 0)
			{
				if (n0179 >= 1 && n0179 <= 8)
				{
					switch (n0277)
					{
					case 0:
					case 1:
						n0179 += ((n0179 % 2 != 0) ? 1 : (-1));
						break;
					case 2:
						n0179 -= 2;
						if (n0179 <= 0)
						{
							n0179 += 23;
						}
						break;
					case 3:
						n0179 += 2;
						break;
					}
				}
				else if (n0179 >= 9 && n0179 <= 15)
				{
					switch (n0277)
					{
					case 0:
						n0179--;
						if (n0179 == 8)
						{
							n0179 = 15;
						}
						break;
					case 1:
						n0179++;
						if (n0179 == 16)
						{
							n0179 = 9;
						}
						break;
					case 2:
						if (n0179 == 9)
						{
							n0179 = 7;
						}
						else
						{
							n0179 = 8;
						}
						break;
					case 3:
						if (n0179 <= 14)
						{
							n0179 += 7;
						}
						else
						{
							n0179 = 21;
						}
						break;
					}
				}
				else if (n0179 >= 16 && n0179 <= 21)
				{
					switch (n0277)
					{
					case 0:
						n0179--;
						if (n0179 == 15)
						{
							n0179 = 21;
						}
						break;
					case 1:
						n0179++;
						if (n0179 == 22)
						{
							n0179 = 16;
						}
						break;
					case 2:
						n0179 -= 7;
						break;
					case 3:
						if (n0179 == 16)
						{
							n0179 = 22;
						}
						else
						{
							n0179 = 23;
						}
						break;
					}
				}
				else if (n0179 >= 22 && n0179 <= 23)
				{
					switch (n0277)
					{
					case 0:
					case 1:
						n0179 += ((n0179 % 2 != 1) ? 1 : (-1));
						break;
					case 2:
						n0179 -= 6;
						break;
					case 3:
						n0179 -= 21;
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
					n0179 = 23;
					break;
				case 1:
				case 3:
					n0179 = 1;
					break;
				}
			}
		}

		private void n0180(int n0287)
		{
			switch (n0287)
			{
			case 0:
				n0232 = true;
				break;
			case 1:
				n0232 = false;
				break;
			case 2:
				n0233 = true;
				break;
			case 3:
				n0233 = false;
				break;
			case 4:
				n0234 = true;
				break;
			case 5:
				n0234 = false;
				break;
			case 6:
				n0235 = true;
				break;
			case 7:
				n0235 = false;
				break;
			case 8:
				n0236 = Color.FromArgb(128, 255, 128);
				break;
			case 9:
				n0236 = Color.FromArgb(128, 128, 255);
				break;
			case 10:
				n0236 = Color.FromArgb(255, 128, 128);
				break;
			case 11:
				n0236 = Color.FromArgb(255, 255, 128);
				break;
			case 12:
				n0236 = Color.FromArgb(255, 128, 255);
				break;
			case 13:
				n0236 = Color.FromArgb(128, 255, 255);
				break;
			case 14:
				n0236 = Color.FromArgb(128, 128, 128);
				break;
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
				n0237 = n0287 - 14;
				break;
			case 21:
				n0238 = false;
				break;
			case 22:
				n0238 = true;
				break;
			}
		}

		private void n0099(object n0035, EventArgs n0036)
		{
			int num = n0226();
			switch (num)
			{
			case 0:
				if (n0179 > 0)
				{
					n0180(n0179 - 1);
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
			n0133.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			n0221 = null;
			n0133.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			n0261(n0281[(int)n0039], Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < 23; i++)
			{
				n0288[i] = false;
			}
			if (n0232)
			{
				n0288[0] = true;
			}
			else
			{
				n0288[1] = true;
			}
			if (n0233)
			{
				n0288[2] = true;
			}
			else
			{
				n0288[3] = true;
			}
			if (n0234)
			{
				n0288[4] = true;
			}
			else
			{
				n0288[5] = true;
			}
			if (n0235)
			{
				n0288[6] = true;
			}
			else
			{
				n0288[7] = true;
			}
			if (n0236 == Color.FromArgb(128, 255, 128))
			{
				n0288[8] = true;
			}
			else if (n0236 == Color.FromArgb(128, 128, 255))
			{
				n0288[9] = true;
			}
			else if (n0236 == Color.FromArgb(255, 128, 128))
			{
				n0288[10] = true;
			}
			else if (n0236 == Color.FromArgb(255, 255, 128))
			{
				n0288[11] = true;
			}
			else if (n0236 == Color.FromArgb(255, 128, 255))
			{
				n0288[12] = true;
			}
			else if (n0236 == Color.FromArgb(128, 255, 255))
			{
				n0288[13] = true;
			}
			else if (n0236 == Color.FromArgb(128, 128, 128))
			{
				n0288[14] = true;
			}
			n0288[n0237 + 14] = true;
			if (!n0238)
			{
				n0288[21] = true;
			}
			else
			{
				n0288[22] = true;
			}
			for (int j = 0; j < 23; j++)
			{
				if (n0288[j])
				{
					Rectangle n880 = new Rectangle(n0289[j], n0290[j], n0291[j], n0292[j]);
					n0261(n0282[(int)n0039], n880, Rectangle.Empty, new Point(n0293[j], n0294[j]), Color.White);
				}
			}
			if (n0179 > 0)
			{
				Rectangle n881 = new Rectangle(225 + 16 * n0295[n0280], 25, 16, 25);
				n0261(n0282[(int)n0039], n881, Rectangle.Empty, new Point(n0293[n0179 - 1] - 16, n0294[n0179 - 1]), Color.White);
				n881 = new Rectangle(273 + 16 * n0295[n0280], 25, 16, 25);
				n0261(n0282[(int)n0039], n881, Rectangle.Empty, new Point(n0293[n0179 - 1] + n0291[n0179 - 1], n0294[n0179 - 1]), Color.White);
				n0280++;
				if (n0280 == 6)
				{
					n0280 = 0;
				}
			}
			bool flag = false;
			if (n0178 > 0)
			{
				int alpha = (int)((double)(n0286 ? n0178 : (10 - n0178)) / 10.0 * 255.0);
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				n0178--;
				if (!n0286 && n0178 == 0)
				{
					n0071.Stop();
					if (n0167 == 0)
					{
						n0260();
					}
					else if (n0167 == 1)
					{
						n0216.Clear();
						n0217.Clear();
						n0218.Clear();
						n0219.Clear();
						n0220.Clear();
						n0221 = null;
						n0066 = 1;
						n0156.n0249(n0232);
						n0156.n0250(n0234);
						n0156.n0251(n0233);
						n0156.n0252(n0235);
						n0156.n0296(n0236);
						n0156.n0253(n0237);
						n0156.n0254(n0238);
						n0156.n0166(false, false);
						n0069.Start();
						flag = true;
					}
					n0179 = 0;
				}
			}
			n0144.End();
			n0133.EndScene();
			try
			{
				n0133.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0133.TestCooperativeLevel();
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
			if (n0178 != 0)
			{
				return;
			}
			if (n0036.Button == MouseButtons.Left)
			{
				n0134 = Point.Empty;
				n0213(n0035, n0036);
				if (n0179 > 0)
				{
					n0180(n0179 - 1);
				}
			}
			else if (n0036.Button == MouseButtons.Right)
			{
				n0189();
			}
		}

		private void n0213(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(n0134))
			{
				return;
			}
			n0134 = Cursor.Position;
			if (n0178 == 0)
			{
				for (int i = 0; i < 23; i++)
				{
					if (n0036.X >= n0293[i] && n0036.X < n0293[i] + n0291[i] && n0036.Y >= n0294[i] && n0036.Y < n0294[i] + n0292[i])
					{
						n0179 = i + 1;
						return;
					}
				}
			}
			n0179 = 0;
		}

		private void n0142()
		{
			n0297 = TextureLoader.FromStream(n0133, n0136.n0137("SCUT_BG"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0298 = TextureLoader.FromStream(n0133, n0136.n0137("SCUTMENU"), n0299.Width, n0299.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0258()
		{
			n0297.Dispose();
			n0297 = null;
			n0298.Dispose();
			n0298 = null;
		}

		private void n0278()
		{
			n0066 = 2;
			n0175 = 10;
			n0300 = true;
			n0094.Start();
		}

		private void n0177()
		{
			n0175 = 10;
			n0300 = false;
		}

		private void n0195(int n0277)
		{
			if (n0175 > 0)
			{
				return;
			}
			switch (n0277)
			{
			case 0:
				if (n0176 == 0)
				{
					n0176 = 16;
					break;
				}
				n0176--;
				if (n0176 == 0)
				{
					n0176 = 16;
				}
				break;
			case 1:
				if (n0176 == 0)
				{
					n0176 = 1;
					break;
				}
				n0176++;
				if (n0176 == 17)
				{
					n0176 = 1;
				}
				break;
			}
		}

		private void n0113(object n0035, EventArgs n0036)
		{
			switch (n0226())
			{
			case 0:
				if (n0176 > 0 && n0158 >= n0176)
				{
					n0177();
				}
				break;
			case 1:
				n0176 = 0;
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
			n0133.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			ArrayList arrayList = n0219;
			arrayList.Clear();
			n0220.Clear();
			n0221 = null;
			n0133.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			n0261(n0297, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			for (int i = 0; i < n0158; i++)
			{
				Rectangle n880 = new Rectangle((n0176 - 1 != i) ? 280 : 0, 28 * i, 280, 28);
				n0261(n0298, n880, Rectangle.Empty, new Point(n0301[(i >= 8) ? 1 : 0], 80 + 40 * (i % 8)), Color.White);
			}
			for (int j = n0158; j < 16; j++)
			{
				Rectangle n881 = new Rectangle((n0176 - 1 != j) ? 280 : 0, 448, 280, 28);
				n0261(n0298, n881, Rectangle.Empty, new Point(n0301[(j >= 8) ? 1 : 0], 80 + 40 * (j % 8)), Color.White);
			}
			if (n0175 > 0)
			{
				int alpha = (int)((double)(n0300 ? n0175 : (10 - n0175)) / 10.0 * 255.0);
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				n0175--;
				if (!n0300 && n0175 == 0)
				{
					n0094.Stop();
					if (n0176 == 0)
					{
						n0260();
					}
					else
					{
						num = n0176;
					}
					n0176 = 0;
				}
			}
			n0144.End();
			n0133.EndScene();
			try
			{
				n0133.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0133.TestCooperativeLevel();
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
			if (n0175 != 0)
			{
				return;
			}
			if (n0036.Button == MouseButtons.Left)
			{
				n0134 = Point.Empty;
				n0212(n0035, n0036);
				if (n0176 > 0 && n0158 >= n0176)
				{
					n0177();
				}
			}
			else if (n0036.Button == MouseButtons.Right)
			{
				n0176 = 0;
				n0177();
			}
		}

		private void n0212(object n0035, MouseEventArgs n0036)
		{
			if (Cursor.Position.Equals(n0134))
			{
				return;
			}
			n0134 = Cursor.Position;
			if (n0175 != 0)
			{
				return;
			}
			n0176 = 0;
			for (int i = 0; i < 16; i++)
			{
				if (n0036.X > n0301[(i >= 8) ? 1 : 0] && n0036.X < n0301[(i >= 8) ? 1 : 0] + 280 && n0036.Y >= 80 + 40 * (i % 8) && n0036.Y < 80 + 40 * (i % 8) + 28)
				{
					n0176 = i + 1;
					break;
				}
			}
		}

		private void n0143()
		{
			n0302 = TextureLoader.FromStream(n0133, n0136.n0137("ALBUMBG"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0303 = TextureLoader.FromStream(n0133, n0136.n0137("AL_PIC"), n0304.Width, n0304.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0259()
		{
			n0302.Dispose();
			n0302 = null;
			n0303.Dispose();
			n0303 = null;
		}

		private void n0279()
		{
			n0066 = 4;
			n0181 = 10;
			n0305 = true;
			n0183 = 0;
			n0185 = 0;
			n0182 = 0;
			n0306 = (n0307 = null);
			n0222 = new Microsoft.DirectX.Direct3D.Font(n0133, new System.Drawing.Font(n0037, 24f));
			n0095.Start();
		}

		private void n0190()
		{
			n0181 = 10;
			n0305 = false;
		}

		private void n0184(int n0308, int n0309)
		{
			if (n0309 >= n0310[n0308 - 1])
			{
				n0191();
				return;
			}
			n0183 = n0308;
			n0185 = n0309;
			n0182 = 10;
			if (n0306 != null)
			{
				n0306.Dispose();
			}
			n0306 = n0307;
			n0307 = TextureLoader.FromStream(n0133, n0153.n0137("EV_HP0" + n0308 + (char)(n0309 + 65)), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
		}

		private void n0191()
		{
			n0183 = 0;
			n0185 = 0;
			n0182 = 10;
			if (n0306 != null)
			{
				n0306.Dispose();
			}
			n0306 = n0307;
			n0307 = null;
		}

		private void n0193(int n0277)
		{
			if (n0181 > 0 || n0182 > 0)
			{
				return;
			}
			switch (n0277)
			{
			case 0:
				if (n0186 == 0)
				{
					n0186 = 7;
					break;
				}
				n0186--;
				if (n0186 == 0)
				{
					n0186 = 7;
				}
				break;
			case 1:
				if (n0186 == 0)
				{
					n0186 = 1;
					break;
				}
				n0186++;
				if (n0186 == 8)
				{
					n0186 = 1;
				}
				break;
			}
		}

		private void n0114(object n0035, EventArgs n0036)
		{
			switch (n0226())
			{
			case 0:
				if (n0183 == 0)
				{
					if (n0186 > 0 && n0160 >= n0186)
					{
						n0184(n0186, 0);
					}
				}
				else
				{
					n0184(n0183, n0185 + 1);
				}
				break;
			case 1:
				if (n0183 == 0)
				{
					n0190();
				}
				else
				{
					n0191();
				}
				break;
			case 4:
				if (n0183 == 0)
				{
					n0193(0);
				}
				break;
			case 5:
				if (n0183 == 0)
				{
					n0193(1);
				}
				break;
			}
			n0133.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			n0221 = null;
			n0133.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			if (n0183 == 0 || (n0185 == 0 && n0182 > 0))
			{
				n0261(n0302, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
				for (int i = 0; i < n0160; i++)
				{
					n0261(n0303, new Rectangle(0, 90 * i, 120, 90), Rectangle.Empty, new Point(n0311[i], n0312[i]), (n0186 - 1 == i) ? Color.White : Color.FromArgb(n0313, Color.White));
				}
				for (int j = n0160; j < 7; j++)
				{
					n0261(n0303, new Rectangle(0, 630, 120, 90), Rectangle.Empty, new Point(n0311[j], n0312[j]), (n0186 - 1 == j) ? Color.White : Color.FromArgb(n0313, Color.White));
				}
				if (n0186 > 0 && n0160 >= n0186)
				{
					int num = 0;
					while (num < n0314[n0186 - 1].Length)
					{
						n0222.DrawText(n0144, n0314[n0186 - 1][num].ToString(), n0223.X + n0224.Width * num + 2, n0223.Y + 2, Color.Black);
						n0222.DrawText(n0144, n0314[n0186 - 1][num].ToString(), n0223.X + n0224.Width * num, n0223.Y, Color.White);
						int num2 = num;
						int num3 = 1;
						num = num2 + num3;
					}
					n0216.Add(null);
					n0217.Add(null);
					n0218.Add(null);
					n0219.Add(null);
					n0220.Add(null);
					n0221 = n0314[n0186 - 1];
				}
			}
			else if (n0182 == 0)
			{
				n0261(n0307, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
			else
			{
				n0261(n0306, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
			}
			if (n0181 > 0)
			{
				int alpha = (int)((double)(n0305 ? n0181 : (10 - n0181)) / 10.0 * 255.0);
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(alpha, Color.White));
				n0181--;
				if (!n0305 && n0181 == 0)
				{
					n0095.Stop();
					n0260();
					n0186 = 0;
				}
			}
			else if (n0182 > 0)
			{
				if (n0183 > 0)
				{
					n0261(n0307, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)((double)(10 - n0182) / 10.0 * 255.0), Color.White));
				}
				else
				{
					n0261(n0306, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)((double)n0182 / 10.0 * 255.0), Color.White));
				}
				n0182--;
			}
			n0144.End();
			n0133.EndScene();
			try
			{
				n0133.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0133.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					n0225();
				}
				catch
				{
				}
			}
			if (n0183 == 0 && n0182 == 0 && n0306 != null)
			{
				n0306.Dispose();
				n0306 = null;
			}
		}

		private void n0210(object n0035, MouseEventArgs n0036)
		{
			if (n0181 != 0)
			{
				return;
			}
			if (n0036.Button == MouseButtons.Left)
			{
				if (n0183 != 0)
				{
					n0184(n0183, n0185 + 1);
					return;
				}
				n0134 = Point.Empty;
				n0214(n0035, n0036);
				if (n0186 > 0 && n0160 >= n0186)
				{
					n0184(n0186, 0);
				}
			}
			else if (n0036.Button == MouseButtons.Right)
			{
				if (n0183 == 0)
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
			if (Cursor.Position.Equals(n0134))
			{
				return;
			}
			n0134 = Cursor.Position;
			if (n0181 != 0)
			{
				return;
			}
			n0186 = 0;
			for (int i = 0; i < 7; i++)
			{
				if (n0036.X > n0311[i] && n0036.X < n0311[i] + 120 && n0036.Y >= n0312[i] && n0036.Y < n0312[i] + 90)
				{
					n0186 = i + 1;
					break;
				}
			}
		}
	}
}
