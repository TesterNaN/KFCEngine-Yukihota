using System;
using System.Collections;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace n0001
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

		protected const int n0401 = 16;

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

		protected ArchiveFile n0136;

		private ButtonLayout n0363;

		private System.Drawing.Font n0362;

		private bool n0559;

		private AudioManager n0558;

		private Texture n0643;

		private int n0560;

		private Device n0006;

		private Sprite n0144;

		private FrameCallback n0552;

		protected readonly Rectangle n0663 = new Rectangle(0, 0, 800, 600);

		protected Texture n0135;

		protected ArrayList n0216 = new ArrayList();

		protected ArrayList n0217 = new ArrayList();

		protected ArrayList n0218 = new ArrayList();

		protected ArrayList n0219 = new ArrayList();

		protected ArrayList n0220 = new ArrayList();

		protected ArrayList n0570 = new ArrayList();

		protected ArrayList n0571 = new ArrayList();

		protected ArrayList n0572 = new ArrayList();

		protected ArrayList n0573 = new ArrayList();

		protected ArrayList n0574 = new ArrayList();

		protected ArrayList n0575 = new ArrayList();

		protected ArrayList n0576 = new ArrayList();

		protected ArrayList n0577 = new ArrayList();

		protected ArrayList n0578 = new ArrayList();

		protected ArrayList n0579 = new ArrayList();

		protected ArrayList n0580 = new ArrayList();

		protected ArrayList n0581 = new ArrayList();

		protected ArrayList n0582 = new ArrayList();

		protected ArrayList n0583 = new ArrayList();

		private bool n0232;

		private bool n0234;

		private bool n0233;

		private bool n0235;

		private Color n0236;

		private int n0237 = 1;

		private bool n0238;

		private int n0636;

		protected readonly int[] n0640 = new int[16]
		{
			3, 3, 2, 2, 1, 1, 0, 0, 0, 0,
			1, 1, 2, 2, 3, 3
		};

		protected readonly int[] n0641 = new int[32]
		{
			-2, -1, -1, 0, 0, 1, 1, 2, 2, 1,
			1, 0, 0, -1, -1, -2, -2, -1, -1, 0,
			0, 1, 1, 2, 2, 1, 1, 0, 0, -1,
			-1, -2
		};

		protected readonly int[] n0642 = new int[32]
		{
			-1, -1, 0, 0, 0, 0, 1, 1, 1, 1,
			2, 2, 2, 2, 1, 1, 1, 1, 0, 0,
			0, 0, -1, -1, -1, -1, -2, -2, -2, -2,
			-1, -1
		};

		private Texture n0551;

		private Texture n0654;

		private Color n0651;

		private Texture n0653;

		private Color n0652;

		private bool n0615;

		private bool n0655;

		private bool n0656;

		private bool n0657;

		private int n0597;

		private int n0658;

		protected readonly int[] n0659 = new int[3] { 1, 10, 40 };

		private bool n0616;

		private TextureBlend n0664;

		private bool n0661;

		private Rectangle n0665;

		private Rectangle n0662;

		private int n0598;

		private int n0679;

		protected readonly int[] n0681 = new int[3] { 1, 10, 40 };

		private bool n0666;

		private bool n0602;

		private int n0369;

		protected readonly int[] n0684 = new int[16]
		{
			-16, -8, 0, 8, 16, 8, 0, -8, -8, 0,
			8, 0, -4, 0, 4, 0
		};

		protected readonly int[] n0685 = new int[16]
		{
			-16, 16, 0, 0, -16, 16, 0, 0, -8, 8,
			0, 0, -4, 4, 0, 0
		};

		protected readonly int[] n0686 = new int[16]
		{
			-4, -3, -2, -1, 0, 1, 2, 3, 4, 3,
			2, 1, 0, -1, -2, -3
		};

		protected readonly int[] n0687 = new int[16]
		{
			-4, -2, 0, 2, 4, 2, 0, -2, -4, -2,
			0, 2, 4, 2, 0, -2
		};

		private bool n0689;

		private Texture n0688;

		private int[] n0690 = new int[5];

		private int[] n0691 = new int[5];

		private double[] n0692 = new double[5];

		private int[] n0693 = new int[5];

		private Random n0694;

		private bool n0700;

		private Texture n0698;

		private int[] n0701 = new int[30];

		private PointF[] n0702 = new PointF[30];

		private float[] n0703 = new float[30];

		private float[] n0704 = new float[30];

		private float[] n0705 = new float[30];

		private float[] n0706 = new float[30];

		private int[] n0707 = new int[30];

		private int[] n0708 = new int[30];

		private Random n0710;

		private int n0709;

		protected readonly Size n0699 = new Size(96, 192);

		private bool n0719;

		private Texture n0717;

		private PointF[] n0720 = new PointF[25];

		private float[] n0721 = new float[25];

		private float[] n0722 = new float[25];

		private float[] n0723 = new float[25];

		private float[] n0724 = new float[25];

		private int[] n0725 = new int[25];

		private Random n0727;

		private int n0726;

		protected readonly Size n0718 = new Size(64, 16);

		private bool n0618;

		private bool n0617;

		private int n0599;

		private bool n0621;

		private bool n0620;

		private int n0619;

		private Texture n0732;

		private Texture[] n0550 = new Texture[3];

		private Texture[] n0737 = new Texture[3];

		private Texture[] n0738 = new Texture[3];

		private Size[] n0741 = new Size[3];

		private Size[] n0742 = new Size[3];

		private int[] n0603 = new int[3];

		private int[] n0739 = new int[3];

		private int[] n0740 = new int[3];

		private bool[] n0743 = new bool[3];

		private int[] n0604 = new int[3];

		private int[] n0744 = new int[3];

		protected readonly int[] n0745 = new int[3] { 4, 6, 16 };

		protected readonly Color n0746 = Color.FromArgb(128, Color.White);

		protected int[][] n0735;

		protected int[][] n0736;

		private string[] n0747;

		private int n0748;

		protected readonly Point n0749 = new Point(300, 20);

		protected readonly Size n0750 = new Size(30, 40);

		private Microsoft.DirectX.Direct3D.Font n0593;

		private string[] n0605;

		private int[] n0608 = new int[3];

		private int n0624;

		private double n0607;

		private int n0609;

		private int n0600;

		private HorizontalAlign n0751;

		private TextRevealMode n0606;

		private bool n0752;

		private Color n0753;

		private Color n0754;

		private double n0755;

		private Point n0756;

		protected readonly Point n0757 = new Point(20, 60);

		protected readonly Size n0759 = new Size(30, 40);

		private Microsoft.DirectX.Direct3D.Font n0594;

		private bool n0623;

		private bool n0765;

		protected Texture n0761;

		protected Texture n0763;

		private bool n0622;

		private int n0601;

		protected readonly Rectangle n0762 = new Rectangle(10, 440, 780, 150);

		protected readonly Point n0853 = new Point(400, 515);

		protected readonly Point n0767 = new Point(10, 380);

		protected readonly Point n0854 = new Point(110, 410);

		protected readonly Size n0764 = new Size(200, 60);

		private string[] n0610;

		private string[] n0769;

		private int[] n0612 = new int[3];

		private int n0613;

		private double n0611;

		private double n0614;

		private bool n0770;

		private Point n0771;

		private bool n0768;

		protected readonly Point n0772 = new Point(25, 455);

		protected readonly Size n0774 = new Size(30, 40);

		protected readonly Point n0775 = new Point(110, 390);

		private Microsoft.DirectX.Direct3D.Font n0595;

		private int n0777;

		protected readonly Point n0778 = new Point(40, 20);

		protected readonly Size n0776 = new Size(128, 96);

		public ScreenRenderer(Device n0349, Sprite n0350, FrameCallback n0225, Texture n0351, System.Drawing.Font n0353, ButtonLayout n0046, Color n0266, AudioManager n0557)
		{
			n0006 = n0349;
			n0144 = n0350;
			n0552 = n0225;
			n0135 = n0351;
			n0236 = n0266;
			n0362 = n0353;
			n0363 = n0046;
			n0558 = n0557;
			n0559 = false;
			n0560 = 0;
			n0136 = new ArchiveFile("System", ArchiveMode.n0151);
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
			n0570.Clear();
			n0571.Clear();
			n0572.Clear();
			n0573.Clear();
			n0574.Clear();
			n0575.Clear();
			n0576.Clear();
			n0577.Clear();
			n0578.Clear();
			n0579.Clear();
			n0580.Clear();
			n0581.Clear();
			n0582.Clear();
			n0583.Clear();
		}

		public void n0463()
		{
			n0006.Clear(ClearFlags.Target, Color.Black, 1f, 0);
			n0216.Clear();
			n0217.Clear();
			n0218.Clear();
			n0219.Clear();
			n0220.Clear();
			n0570.Clear();
			n0571.Clear();
			n0572.Clear();
			n0573.Clear();
			n0574.Clear();
			n0575.Clear();
			n0576.Clear();
			n0577.Clear();
			n0578.Clear();
			n0579.Clear();
			n0580.Clear();
			n0581.Clear();
			n0582.Clear();
			n0583.Clear();
			n0006.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			n0584();
			n0585();
			n0586();
			n0587();
			n0588();
			if (!n0559)
			{
				n0589();
			}
			n0590();
			n0591();
			n0592();
			n0144.End();
			n0006.EndScene();
			try
			{
				n0006.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0006.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					n0552();
				}
				catch
				{
				}
			}
		}

		public void n0215()
		{
			n0006.Clear(ClearFlags.Target, Color.White, 1f, 0);
			n0006.BeginScene();
			n0144.Begin(SpriteFlags.AlphaBlend);
			int num = 0;
			for (int i = 0; i < n0216.Count; i++)
			{
				if (n0216[i] != null)
				{
					n0144.Draw2D((Texture)n0216[i], (Rectangle)n0217[i], (Rectangle)n0218[i], Point.Empty, 0f, (Point)n0219[i], (Color)n0220[i]);
					continue;
				}
				Microsoft.DirectX.Direct3D.Font n880;
				switch ((int)n0579[num])
				{
				case 1:
					n880 = n0593;
					break;
				case 2:
					n880 = n0594;
					break;
				case 3:
					n880 = n0595;
					break;
				default:
					n880 = n0593;
					break;
				}
				n0596((string)n0570[num], (int)n0571[num], (double)n0572[num], (Point)n0573[num], (int)n0574[num], (int)n0575[num], (int)n0576[num], (HorizontalAlign)n0577[num], (int)n0578[num], n880, (bool)n0580[num], (int)n0581[num], (Color)n0582[num], (Color)n0583[num], false);
				num++;
			}
			n0144.End();
			n0006.EndScene();
			try
			{
				n0006.Present();
			}
			catch (DeviceLostException)
			{
				try
				{
					n0006.TestCooperativeLevel();
				}
				catch (DeviceNotResetException)
				{
					n0552();
				}
				catch
				{
				}
			}
		}

		public bool n0380()
		{
			if (n0597 > 0 || n0598 > 0 || n0599 > 0 || n0600 > 0 || n0601 > 0 || (n0369 > 0 && !n0602))
			{
				return true;
			}
			for (int i = 0; i < 3; i++)
			{
				if (n0603[i] > 0 || n0604[i] > 0)
				{
					return true;
				}
			}
			if (n0605 != null)
			{
				switch (n0606)
				{
				case TextRevealMode.n0493:
					if (n0607 < (double)(n0605[(int)n0363].Length - n0608[(int)n0363] + 4))
					{
						return true;
					}
					break;
				case TextRevealMode.n0487:
					if (n0609 > 0)
					{
						return true;
					}
					break;
				}
			}
			return n0610 != null && n0611 < (double)(n0610[(int)n0363].Length - n0612[(int)n0363] + 4);
		}

		public void n0197(bool n0241)
		{
			n0559 = n0241;
			if (n0241 && n0610 != null)
			{
				n0611 = n0610[(int)n0363].Length - n0612[(int)n0363] + 4;
				n0613 = n0610[(int)n0363].Length - n0612[(int)n0363];
			}
		}

		public void n0197()
		{
			n0559 = !n0559;
			if (n0559 && n0610 != null && n0614 == 0.0)
			{
				n0611 = n0610[(int)n0363].Length - n0612[(int)n0363] + 4;
				n0613 = n0610[(int)n0363].Length - n0612[(int)n0363];
			}
		}

		public void n0168()
		{
			if (n0597 > 0)
			{
				n0597 = 0;
				if (n0615)
				{
					n0615 = false;
				}
				else
				{
					n0616 = true;
				}
			}
			if (n0598 > 0)
			{
				n0598 = 0;
			}
			if (n0599 > 0)
			{
				n0599 = 0;
				if (n0617)
				{
					n0618 = false;
				}
			}
			if (n0619 > 0)
			{
				n0619 = 0;
				if (n0620)
				{
					n0621 = false;
				}
			}
			if (n0601 > 0)
			{
				n0601 = 0;
				if (n0622)
				{
					n0623 = false;
				}
			}
			if (n0369 > 0 && !n0602)
			{
				n0369 = 1;
			}
			for (int i = 0; i < 3; i++)
			{
				if (n0603[i] > 0)
				{
					n0603[i] = 0;
				}
				if (n0604[i] > 0)
				{
					n0604[i] = 1;
				}
			}
			if (n0605 != null)
			{
				if (n0600 > 0)
				{
					n0600 = 0;
					n0607 = 0.0;
					n0609 = 0;
					n0605 = null;
					for (int j = 0; j < 3; j++)
					{
						n0608[j] = 0;
					}
					n0624 = 0;
				}
				else
				{
					n0624 = n0605[(int)n0363].Length - n0608[(int)n0363];
					switch (n0606)
					{
					case TextRevealMode.n0493:
						n0607 = n0605[(int)n0363].Length - n0608[(int)n0363] + 4;
						break;
					case TextRevealMode.n0487:
						n0609 = 0;
						break;
					}
				}
			}
			if (n0610 != null)
			{
				n0611 = n0610[(int)n0363].Length - n0612[(int)n0363] + 4;
				n0613 = n0610[(int)n0363].Length - n0612[(int)n0363];
			}
		}

		public void n0206(ButtonLayout n0046)
		{
			if (n0363 != n0046)
			{
				n0363 = n0046;
				n0625();
				n0626();
				n0627();
			}
		}

		public void n0229(System.Drawing.Font n0353)
		{
			if (n0362 != n0353)
			{
				n0362 = n0353;
				n0625();
				n0626();
				n0627();
			}
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

		public void n0249(bool n0049)
		{
			if (n0232 != n0049)
			{
				n0625();
				n0626();
				n0627();
			}
			n0232 = n0049;
		}

		public void n0250(bool n0049)
		{
			n0234 = n0049;
		}

		public void n0251(bool n0049)
		{
			n0233 = n0049;
		}

		public void n0252(bool n0049)
		{
			n0235 = n0049;
		}

		public void n0296(Color n0049)
		{
			n0236 = n0049;
		}

		public void n0253(int n0049)
		{
			if (n0049 > 0)
			{
				n0237 = n0049;
				return;
			}
			throw new Exception("Invalid Speed");
		}

		public void n0254(bool n0049)
		{
			n0238 = n0049;
		}

		private void n0561()
		{
			n0636 = 0;
		}

		private void n0637(int n0638, int n0639)
		{
			Rectangle n880 = new Rectangle(n0640[n0636 % 16] * 32, 0, 32, 32);
			Point n881 = new Point(n0638 + n0641[n0636], n0639 + n0642[n0636]);
			n0261(n0643, n880, Rectangle.Empty, n881, Color.White);
			n0636++;
			n0636 %= 32;
		}

		private Point n0596(string n0241, Point n0644, int n0645, int n0646, int n0534, Microsoft.DirectX.Direct3D.Font n0353, Color n0412, Color n0413)
		{
			return n0596(n0241, 0, n0241.Length, n0644, n0645, n0646, n0534, HorizontalAlign.n0437, 0, n0353, n0412, n0413);
		}

		private Point n0596(string n0241, int n0647, int n0648, Point n0644, int n0645, int n0646, int n0534, HorizontalAlign n0410, int n0649, Microsoft.DirectX.Direct3D.Font n0353, Color n0412, Color n0413)
		{
			return n0596(n0241, n0647, n0648, n0644, n0645, n0646, n0534, n0410, n0649, n0353, false, 0, n0412, n0413);
		}

		private Point n0596(string n0241, double n0648, Point n0644, int n0645, int n0646, int n0534, Microsoft.DirectX.Direct3D.Font n0353, int n0204, Color n0412, Color n0413)
		{
			return n0596(n0241, 0, n0648, n0644, n0645, n0646, n0534, HorizontalAlign.n0437, 0, n0353, true, n0204, n0412, n0413);
		}

		private Point n0596(string n0241, double n0648, Point n0644, int n0645, int n0646, int n0534, HorizontalAlign n0410, int n0649, Microsoft.DirectX.Direct3D.Font n0353, int n0204, Color n0412, Color n0413)
		{
			return n0596(n0241, 0, n0648, n0644, n0645, n0646, n0534, n0410, n0649, n0353, true, n0204, n0412, n0413);
		}

		private Point n0596(string n0241, int n0647, double n0648, Point n0644, int n0645, int n0646, int n0534, HorizontalAlign n0410, int n0649, Microsoft.DirectX.Direct3D.Font n0353, bool n0493, int n0204, Color n0412, Color n0413)
		{
			return n0596(n0241, n0647, n0648, n0644, n0645, n0646, n0534, n0410, n0649, n0353, n0493, n0204, n0412, n0413, true);
		}

		private Point n0596(string n0241, int n0647, double n0648, Point n0644, int n0645, int n0646, int n0534, HorizontalAlign n0410, int n0649, Microsoft.DirectX.Direct3D.Font n0353, bool n0493, int n0204, Color n0412, Color n0413, bool n0650)
		{
			if (n0650)
			{
				n0216.Add(null);
				n0217.Add(null);
				n0218.Add(null);
				n0219.Add(null);
				n0220.Add(null);
				n0570.Add(n0241);
				n0571.Add(n0647);
				n0572.Add(n0648);
				n0573.Add(n0644);
				n0574.Add(n0645);
				n0575.Add(n0646);
				n0576.Add(n0534);
				n0577.Add(n0410);
				n0578.Add(n0649);
				n0580.Add(n0493);
				n0581.Add(n0204);
				n0582.Add(n0412);
				n0583.Add(n0413);
			}
			int num;
			switch (n0410)
			{
			case HorizontalAlign.n0437:
				num = n0644.X;
				break;
			case HorizontalAlign.n0491:
			{
				int num3 = n0241.IndexOf('\n');
				if (num3 == -1)
				{
					num3 = n0241.Length;
				}
				num = n0644.X + n0645 * (n0649 - num3) / 2;
				break;
			}
			case HorizontalAlign.n0438:
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
					switch (n0410)
					{
					case HorizontalAlign.n0437:
						num = n0644.X;
						break;
					case HorizontalAlign.n0491:
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
					case HorizontalAlign.n0438:
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
					if (n0232)
					{
						Sprite sprite = n0144;
						n0353.DrawText(sprite, n0241[i].ToString(), num + n0534, num4 + n0534, n0413);
					}
					Sprite sprite2 = n0144;
					n0353.DrawText(sprite2, n0241[i].ToString(), num, num4, n0412);
				}
				else if (n0493 && i >= n0647 && (double)i >= n0648 && (double)i < n0648 + (double)n0204 && n0232)
				{
					int alpha = (int)(255.0 * ((double)(n0204 - i) + n0648) / (double)n0204);
					Sprite sprite3 = n0144;
					n0353.DrawText(sprite3, n0241[i].ToString(), num + n0534, num4 + n0534, (n0413 == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha, n0413));
					Sprite sprite4 = n0144;
					n0353.DrawText(sprite4, n0241[i].ToString(), num, num4, Color.FromArgb(alpha, n0412));
				}
				num += n0645;
			}
			return new Point(num, num4);
		}

		private void n0562()
		{
			n0597 = 0;
			n0651 = (n0652 = Color.White);
			n0551 = (n0653 = (n0654 = null));
			n0655 = (n0656 = true);
			n0615 = false;
			n0657 = false;
			n0616 = false;
		}

		private void n0628()
		{
			if (n0551 != null)
			{
				n0551.Dispose();
			}
			if (n0653 != null)
			{
				n0653.Dispose();
			}
			if (n0654 != null)
			{
				n0654.Dispose();
			}
		}

		public void n0386(Texture n0241, int n0441)
		{
			if (n0653 != null)
			{
				n0653.Dispose();
			}
			n0653 = n0551;
			n0652 = n0651;
			n0551 = n0241;
			n0651 = Color.White;
			n0656 = n0655;
			n0655 = false;
			n0658 = n0441;
			n0597 = n0659[n0441];
			n0615 = false;
			if (n0621)
			{
				n0660();
			}
			else if (n0654 != null)
			{
				n0654.Dispose();
				n0654 = null;
			}
		}

		public void n0386(Color n0241, int n0441)
		{
			if (n0653 != null)
			{
				n0653.Dispose();
			}
			n0653 = n0551;
			n0652 = n0651;
			n0651 = n0241;
			n0551 = null;
			n0656 = n0655;
			n0655 = true;
			n0658 = n0441;
			n0597 = n0659[n0441];
			n0615 = false;
			if (!n0621 && n0654 != null)
			{
				n0654.Dispose();
				n0654 = null;
			}
		}

		private void n0584()
		{
			if (n0597 > 0 || n0616)
			{
				if (n0615)
				{
					if (n0653 != null)
					{
						if (n0661)
						{
							n0261(n0653, n0662, n0663, Point.Empty, Color.White);
						}
						else
						{
							n0261(n0653, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						}
					}
					else
					{
						n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, n0652);
					}
					if (n0232)
					{
						Point n880 = new Point(0, 0);
						switch (n0664)
						{
						case TextureBlend.n0487:
							if (n0661)
							{
								n0261(n0551, n0665, n0663, Point.Empty, Color.FromArgb((int)(255.0 * (double)(n0659[n0658] - n0597) / (double)n0659[n0658]), Color.White));
							}
							else
							{
								n0261(n0551, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(n0659[n0658] - n0597) / (double)n0659[n0658]), Color.White));
							}
							break;
						case TextureBlend.n0437:
						{
							if (n0661)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle n882;
							for (int j = 0; j < 16; j++)
							{
								n880.X = 832 * (n0659[n0658] - n0597) / n0659[n0658] + (-j - 1) * 2;
								n882 = new Rectangle(n880.X, 0, 2, 600);
								int alpha2 = (int)((double)(j + 1) / 17.0 * 255.0);
								n0261(n0551, n882, Rectangle.Empty, n880, Color.FromArgb(alpha2, Color.White));
							}
							n880.X = 0;
							n882 = new Rectangle(n880.X, 0, 832 * (n0659[n0658] - n0597) / n0659[n0658] - 32, 600);
							n0261(n0551, n882, Rectangle.Empty, n880, Color.White);
							break;
						}
						case TextureBlend.n0438:
						{
							if (n0661)
							{
								throw new Exception("Not Implement Yet");
							}
							Rectangle n881;
							for (int i = 0; i < 16; i++)
							{
								n880.X = 832 * n0597 / n0659[n0658] + (i - 16) * 2;
								n881 = new Rectangle(n880.X, 0, 2, 600);
								int alpha = (int)((double)(i + 1) / 17.0 * 255.0);
								n0261(n0551, n881, Rectangle.Empty, n880, Color.FromArgb(alpha, Color.White));
							}
							n880.X = 832 * n0597 / n0659[n0658];
							n881 = new Rectangle(n880.X, 0, 800 - n880.X, 600);
							n0261(n0551, n881, Rectangle.Empty, n880, Color.White);
							break;
						}
						}
						n0597--;
					}
					else
					{
						n0597 = 0;
					}
					if (n0597 == 0)
					{
						n0615 = false;
					}
				}
				else if (!n0656)
				{
					if (!n0661 && (!n0666 || !n0602))
					{
						if (n0653 != null)
						{
							if (n0621)
							{
								n0261(n0654, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
							else
							{
								n0261(n0653, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, n0652);
						}
					}
					n0657 = false;
				}
				else
				{
					if (!n0661 && (!n0666 || !n0602))
					{
						if (n0551 != null)
						{
							if (n0621)
							{
								n0261(n0654, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
							else
							{
								n0261(n0551, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							}
						}
						else
						{
							n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, n0651);
						}
					}
					n0657 = true;
				}
				if (n0616)
				{
					n0616 = false;
				}
				return;
			}
			if (!n0661 && (!n0666 || !n0602))
			{
				if (n0551 != null)
				{
					if (n0621)
					{
						if (n0619 > 0)
						{
							float num = (n0620 ? ((float)n0619 / 10f) : ((float)(10 - n0619) / 10f));
							n0261(n0551, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
							n0261(n0654, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * num), Color.White));
						}
						else
						{
							n0261(n0654, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
						}
					}
					else
					{
						n0261(n0551, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
					}
				}
				else
				{
					n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, n0651);
				}
			}
			n0657 = true;
		}

		private void n0590()
		{
			if (n0597 <= 0 || n0615)
			{
				return;
			}
			if (n0232)
			{
				if (n0657 && n0656)
				{
					n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)n0597 / (double)n0659[n0658]), n0652));
				}
				else
				{
					if (n0657 || !n0655)
					{
						throw new Exception("Not Implement Yet");
					}
					n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255.0 * (double)(n0659[n0658] - n0597) / (double)n0659[n0658]), n0651));
				}
				n0597--;
			}
			else
			{
				n0597 = 0;
			}
		}

		private void n0660()
		{
			if (n0654 != null)
			{
				n0654.Dispose();
			}
			if (!(n0551 != null))
			{
				return;
			}
			n0654 = TextureLoader.FromStream(n0006, n0136.n0137("WHITE"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			uint[,] array = (uint[,])n0551.LockRectangle(typeof(uint), 0, LockFlags.None, 800, 600);
			uint[,] array2 = (uint[,])n0654.LockRectangle(typeof(uint), 0, LockFlags.None, 800, 600);
			for (int i = 0; i < 800; i++)
			{
				for (int j = 0; j < 600; j++)
				{
					uint num = array[i, j];
					uint num2 = ((num & 0xFF) + ((num >> 8) & 0xFF) + ((num >> 16) & 0xFF)) / 3;
					array2[i, j] = (uint)(-16777216 + (int)(num2 << 16) + (int)(num2 << 8)) + num2;
				}
			}
			n0551.UnlockRectangle(0);
			n0654.UnlockRectangle(0);
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
			n0664 = TextureBlend.n0487;
		}

		public void n0453(Texture n0455, TextureBlend n0456, bool n0399, int n0441)
		{
			if (n0653 != null)
			{
				n0653.Dispose();
			}
			n0653 = n0551;
			n0652 = n0651;
			n0551 = n0455;
			n0651 = Color.White;
			n0656 = n0655;
			n0655 = false;
			n0658 = n0441;
			if (n0399)
			{
				n0597 = n0659[n0441];
			}
			else
			{
				n0597 = 0;
			}
			n0615 = true;
			n0664 = n0456;
		}

		private void n0668()
		{
			n0661 = false;
			n0665 = (n0662 = n0663);
			n0598 = 0;
			n0679 = 0;
		}

		public void n0435(double n0437, double n0438, double n0439, double n0440, int n0680, bool n0399, bool n0442)
		{
			if (n0442)
			{
				n0661 = false;
				n0662 = (n0665 = n0663);
				return;
			}
			n0661 = true;
			if (n0551 != null)
			{
				n0655 = false;
			}
			else if (n0653 != null)
			{
				n0656 = false;
			}
			n0662 = n0665;
			n0665 = new Rectangle((int)(n0437 * 800.0), (int)(n0439 * 600.0), (int)((n0438 - n0437) * 800.0), (int)((n0440 - n0439) * 600.0));
			n0679 = n0680;
			if (n0399)
			{
				n0598 = n0681[n0679];
			}
			else
			{
				n0598 = 0;
			}
		}

		public void n0682()
		{
			if (!n0661 || n0615)
			{
				return;
			}
			if (n0598 > 0)
			{
				if (n0232)
				{
					if (!n0666 || !n0602)
					{
						double num = (double)n0598 / (double)n0681[n0679];
						double num2 = 1.0 - num;
						Rectangle n880 = new Rectangle((int)(num * (double)n0662.Left + num2 * (double)n0665.Left), (int)(num * (double)n0662.Top + num2 * (double)n0665.Top), (int)(num * (double)n0662.Width + num2 * (double)n0665.Width), (int)(num * (double)n0662.Height + num2 * (double)n0665.Height));
						if (n0551 != null)
						{
							n0261(n0551, n880, n0663, Point.Empty, Color.White);
						}
						else if (n0653 != null)
						{
							n0261(n0653, n880, n0663, Point.Empty, Color.White);
						}
					}
					n0598--;
				}
				else
				{
					if (!n0666 || !n0602)
					{
						if (n0551 != null)
						{
							n0261(n0551, n0665, n0663, Point.Empty, Color.White);
						}
						else if (n0653 != null)
						{
							n0261(n0653, n0665, n0663, Point.Empty, Color.White);
						}
					}
					n0598 = 0;
				}
			}
			else if (!n0666 || !n0602)
			{
				if (n0551 != null)
				{
					n0261(n0551, n0665, n0663, Point.Empty, Color.White);
				}
				else if (n0653 != null)
				{
					n0261(n0653, n0665, n0663, Point.Empty, Color.White);
				}
			}
			if (!n0666 || !n0602)
			{
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		private void n0669()
		{
			n0666 = false;
			n0602 = false;
			n0369 = 0;
		}

		public void n0458(bool n0424, bool n0442)
		{
			if (!n0442)
			{
				n0666 = true;
				n0602 = n0424;
				if (n0424)
				{
					n0369 = 0;
				}
				else
				{
					n0369 = 16;
				}
				if (n0551 != null)
				{
					n0655 = false;
				}
				else if (n0653 != null)
				{
					n0656 = false;
				}
			}
			else
			{
				n0666 = false;
			}
		}

		private void n0683()
		{
			if (!n0666)
			{
				return;
			}
			if (!n0602)
			{
				if (n0235)
				{
					if (n0551 != null)
					{
						n0261(n0551, Rectangle.Empty, Rectangle.Empty, new Point(n0684[16 - n0369], n0685[16 - n0369]), Color.FromArgb(128, Color.White));
					}
					else if (n0653 != null)
					{
						n0261(n0653, Rectangle.Empty, Rectangle.Empty, new Point(n0684[16 - n0369], n0685[16 - n0369]), Color.FromArgb(128, Color.White));
					}
				}
				n0369--;
				if (n0369 == 0)
				{
					n0666 = false;
				}
				return;
			}
			Rectangle rectangle;
			if (n0661 && !n0615)
			{
				if (n0598 > 0 && n0232)
				{
					double num = (double)n0598 / (double)n0681[n0679];
					double num2 = 1.0 - num;
					rectangle = new Rectangle((int)(num * (double)n0662.Left + num2 * (double)n0665.Left), (int)(num * (double)n0662.Top + num2 * (double)n0665.Top), (int)(num * (double)n0662.Width + num2 * (double)n0665.Width), (int)(num * (double)n0662.Height + num2 * (double)n0665.Height));
				}
				else
				{
					rectangle = n0665;
				}
			}
			else
			{
				rectangle = Rectangle.Empty;
			}
			if (n0551 != null)
			{
				n0261(n0551, rectangle, n0663, new Point(n0686[n0369], n0687[n0369]), Color.White);
			}
			else if (n0653 != null)
			{
				n0261(n0653, rectangle, n0663, new Point(n0686[n0369], n0687[n0369]), Color.White);
			}
			n0369++;
			if (n0369 == 16)
			{
				n0369 = 0;
			}
			if (rectangle != Rectangle.Empty)
			{
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
			}
		}

		private void n0670()
		{
			n0688 = TextureLoader.FromStream(n0006, n0136.n0137("SUN"), 80, 80, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0689 = false;
			for (int i = 0; i < 5; i++)
			{
				n0690[i] = 0;
				n0691[i] = 0;
				n0692[i] = 0.0;
				n0693[i] = 0;
			}
			n0694 = new Random();
		}

		private void n0675()
		{
			n0688.Dispose();
		}

		public void n0446(bool n0442)
		{
			if (!n0442)
			{
				n0689 = true;
				n0655 = false;
			}
			else
			{
				n0689 = false;
			}
		}

		private void n0695()
		{
			if (!n0689 || !n0234)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < 5; i++)
			{
				if (n0691[i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && n0694.NextDouble() < 0.25)
			{
				n0690[num] = 0;
				n0691[num] = (int)(20.0 + 60.0 * n0694.NextDouble());
				n0692[num] = 0.5 + 0.5 * n0694.NextDouble();
				n0693[num] = (int)(-75.0 + 150.0 * n0694.NextDouble());
			}
			for (int j = 0; j < 5; j++)
			{
				if (n0691[j] > 0)
				{
					if ((double)n0690[j] < (double)n0691[j] / 2.0)
					{
						n0696(n0693[j], n0692[j], (double)n0690[j] / ((double)n0691[j] / 2.0));
						int[] array;
						IntPtr intPtr;
						(array = n0690)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
						continue;
					}
					if (n0690[j] < n0691[j])
					{
						n0696(n0693[j], n0692[j], (double)(n0691[j] - n0690[j]) / ((double)n0691[j] / 2.0));
						int[] array2;
						IntPtr intPtr2;
						(array2 = n0690)[(int)(intPtr2 = (IntPtr)j)] = array2[(int)intPtr2] + 1;
						continue;
					}
					n0690[j] = 0;
					n0691[j] = 0;
					n0692[j] = 0.0;
					n0693[j] = 0;
				}
			}
			n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
		}

		private void n0696(int n0644, double n0697, double n0441)
		{
			int x = (n0644 - 18) * 80 / (int)(450.0 * n0697);
			int y = (-n0644 - 18) * 80 / (int)(450.0 * n0697);
			Rectangle n880 = new Rectangle(0, 0, 80, 80);
			Rectangle n881 = new Rectangle(0, 0, (int)(450.0 * n0697), (int)(450.0 * n0697));
			n0261(n0688, n880, n881, new Point(x, y), Color.FromArgb((int)(n0697 * n0441 * 255.0), Color.White));
		}

		private void n0671()
		{
			n0698 = TextureLoader.FromStream(n0006, n0136.n0137("SAKURA"), n0699.Width, n0699.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0700 = false;
			for (int i = 0; i < 30; i++)
			{
				n0701[i] = 0;
				n0702[i] = Point.Empty;
				n0703[i] = 0f;
				n0704[i] = 0f;
				n0705[i] = 0f;
				n0706[i] = 0f;
				n0707[i] = 0;
				n0708[i] = 0;
			}
			n0709 = 0;
			n0710 = new Random();
		}

		private void n0676()
		{
			n0698.Dispose();
		}

		public void n0448(bool n0442)
		{
			if (!n0442)
			{
				n0700 = true;
				n0655 = false;
				n0711();
			}
			else
			{
				n0700 = false;
			}
		}

		private void n0711()
		{
			n0709 = n0710.Next(20, 30);
			for (int i = 0; i < n0709; i++)
			{
				n0701[i] = n0710.Next(4);
				n0702[i] = new PointF(n0710.Next(-24, 800), n0710.Next(-24, 600));
				n0703[i] = (float)(n0710.NextDouble() * 16.0);
				n0704[i] = (float)(n0710.NextDouble() * 8.0 * 2.0 - 8.0);
				n0705[i] = n0703[i];
				n0706[i] = n0704[i];
				n0707[i] = n0710.Next(200);
				n0708[i] = n0710.Next(1, 9);
			}
			for (int j = n0709; j < 30; j++)
			{
				n0701[j] = 0;
				n0702[j] = PointF.Empty;
				n0703[j] = 0f;
				n0704[j] = 0f;
				n0705[j] = 0f;
				n0706[j] = 0f;
				n0707[j] = 0;
				n0708[j] = 0;
			}
		}

		private void n0712(bool n0713)
		{
			if (!n0700 || !n0233)
			{
				return;
			}
			int num = -1;
			for (int i = ((!n0713) ? 1 : 0); i < 30; i += 2)
			{
				if (n0708[i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && (n0709 < 20 || !(n0710.NextDouble() >= 0.2)))
			{
				n0701[num] = n0710.Next(4);
				int num2 = n0710.Next(-3297, 624);
				n0703[num] = (float)(n0710.NextDouble() * 16.0);
				n0704[num] = (float)(n0710.NextDouble() * 8.0 * 2.0 - 8.0);
				n0705[num] = n0703[num];
				n0706[num] = n0704[num];
				if (num2 < 0)
				{
					n0702[num] = new PointF((0f - (float)num2 - 1f) % 824f - 24f, -24f);
				}
				else if (n0704[num] > 0f)
				{
					n0702[num] = new PointF(-24f, num2 - 24);
				}
				else
				{
					n0702[num] = new PointF(800f, num2 - 24);
				}
				n0707[num] = n0710.Next(40, 200);
				n0708[num] = n0710.Next(1, 9);
				n0709++;
			}
			for (int j = ((!n0713) ? 1 : 0); j < 30; j += 2)
			{
				if (n0708[j] <= 0)
				{
					continue;
				}
				n0714(n0701[j], n0708[j], n0702[j]);
				int[] array;
				IntPtr intPtr;
				(array = n0701)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + 1;
				if (n0701[j] == 4)
				{
					n0701[j] = 0;
				}
				(array = n0707)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] - 1;
				if (n0707[j] == 0)
				{
					n0705[j] = (float)(n0710.NextDouble() * 16.0);
					n0706[j] = (float)(n0710.NextDouble() * 8.0 * 2.0 - 8.0);
					n0707[j] = n0710.Next(40, 200);
				}
				if (n0704[j] != n0706[j] || n0703[j] != n0705[j])
				{
					float num3 = n0705[j] - n0703[j];
					float num4 = n0706[j] - n0704[j];
					float num5 = ((num3 == 0f) ? float.NaN : Math.Abs(num4 / num3));
					if (Math.Abs(num4) > Math.Abs(num3))
					{
						if (Math.Abs(num4) > 0.1f)
						{
							float[] array2;
							(array2 = n0704)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num4 > 0f) ? 0.1f : (-0.1f));
							if (!float.IsNaN(num5))
							{
								(array2 = n0703)[(int)(intPtr = (IntPtr)j)] = array2[(int)intPtr] + ((num3 > 0f) ? 0.1f : (-0.1f)) / num5;
							}
						}
						else
						{
							n0704[j] = n0706[j];
							n0703[j] = n0705[j];
						}
					}
					else if (Math.Abs(num3) > 0.1f)
					{
						float[] array3;
						(array3 = n0703)[(int)(intPtr = (IntPtr)j)] = array3[(int)intPtr] + ((num3 > 0f) ? 0.1f : (-0.1f));
						(array3 = n0704)[(int)(intPtr = (IntPtr)j)] = array3[(int)intPtr] + ((num4 > 0f) ? 0.1f : (-0.1f)) * num5;
					}
					else
					{
						n0703[j] = n0705[j];
						n0704[j] = n0706[j];
					}
				}
				n0702[j] = new PointF(n0702[j].X + n0704[j], n0702[j].Y + n0703[j]);
				if (n0702[j].X < -24f || n0702[j].X > 800f || n0702[j].Y > 600f)
				{
					n0701[j] = 0;
					n0702[j] = PointF.Empty;
					n0703[j] = 0f;
					n0704[j] = 0f;
					n0705[j] = 0f;
					n0706[j] = 0f;
					n0707[j] = 0;
					n0708[j] = 0;
					n0709--;
				}
			}
		}

		private void n0714(int n0715, int n0716, PointF n0644)
		{
			Rectangle n880 = new Rectangle(n0715 * 24, (n0716 - 1) * 24, 24, 24);
			n0261(n0698, n880, Rectangle.Empty, new Point((int)n0644.X, (int)n0644.Y), Color.White);
		}

		private void n0672()
		{
			n0717 = TextureLoader.FromStream(n0006, n0136.n0137("SNOW"), n0718.Width, n0718.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0719 = false;
			for (int i = 0; i < 25; i++)
			{
				n0720[i] = Point.Empty;
				n0721[i] = 0f;
				n0722[i] = 0f;
				n0723[i] = 0f;
				n0724[i] = 0f;
				n0725[i] = 0;
			}
			n0726 = 0;
			n0727 = new Random();
		}

		private void n0677()
		{
			n0717.Dispose();
		}

		public void n0450(bool n0442)
		{
			if (!n0442)
			{
				n0719 = true;
				n0655 = false;
				n0728();
			}
			else
			{
				n0719 = false;
			}
		}

		private void n0728()
		{
			n0726 = n0727.Next(15, 25);
			for (int i = 0; i < n0726; i++)
			{
				n0720[i] = new PointF(n0727.Next(-16, 800), n0727.Next(-16, 600));
				n0721[i] = (float)(n0727.NextDouble() * 4.0 + 2.0);
				n0722[i] = (float)(n0727.NextDouble() * 16.0 + 8.0);
				n0723[i] = (float)(n0727.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
				n0724[i] = (float)(n0727.NextDouble() * (Math.PI * 2.0));
				n0725[i] = n0727.Next(1, 5);
			}
			for (int j = n0726; j < 25; j++)
			{
				n0720[j] = PointF.Empty;
				n0721[j] = 0f;
				n0722[j] = 0f;
				n0723[j] = 0f;
				n0724[j] = 0f;
				n0725[j] = 0;
			}
		}

		private void n0729()
		{
			if (!n0719 || !n0233)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < 25; i++)
			{
				if (n0725[i] == 0)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && (n0726 < 15 || n0727.NextDouble() < 0.2))
			{
				n0720[num] = new PointF(n0727.Next(-16, 800), -16f);
				n0721[num] = (float)(n0727.NextDouble() * 4.0 + 2.0);
				n0722[num] = (float)(n0727.NextDouble() * 16.0 + 8.0);
				n0723[num] = (float)(n0727.NextDouble() * 0.009999999776482582 + 0.009999999776482582);
				n0724[num] = (float)(n0727.NextDouble() * (Math.PI * 2.0));
				n0725[num] = n0727.Next(1, 5);
				n0726++;
			}
			for (int j = 0; j < 25; j++)
			{
				if (n0725[j] > 0)
				{
					n0730(n0725[j], new PointF((float)((double)n0720[j].X + (double)n0722[j] * Math.Sin(n0723[j] * n0720[j].Y + n0724[j])), n0720[j].Y));
					n0720[j] = new PointF(n0720[j].X, n0720[j].Y + n0721[j]);
					if (n0720[j].Y > 600f)
					{
						n0720[j] = PointF.Empty;
						n0721[j] = 0f;
						n0722[j] = 0f;
						n0723[j] = 0f;
						n0724[j] = 0f;
						n0725[j] = 0;
						n0726--;
					}
				}
			}
		}

		private void n0730(int n0716, PointF n0644)
		{
			Rectangle n880 = new Rectangle((n0716 - 1) * 16, 0, 16, 16);
			n0261(n0717, n880, Rectangle.Empty, new Point((int)n0644.X, (int)n0644.Y), Color.White);
		}

		private void n0673()
		{
			n0618 = false;
			n0617 = false;
			n0599 = 0;
		}

		public void n0444(bool n0399, bool n0442)
		{
			if (!n0442)
			{
				n0618 = true;
				n0617 = false;
				n0655 = false;
				if (n0399)
				{
					n0599 = 10;
				}
				else
				{
					n0599 = 0;
				}
			}
			else
			{
				n0617 = true;
				if (n0399)
				{
					n0599 = 10;
					return;
				}
				n0599 = 0;
				n0618 = false;
			}
		}

		private void n0731()
		{
			if (!n0618)
			{
				return;
			}
			if (n0599 > 0)
			{
				if (n0232)
				{
					float num = (n0617 ? ((float)n0599 / 10f) : ((float)(10 - n0599) / 10f));
					n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(128f * num), Color.Black));
					n0599--;
				}
				else
				{
					n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
					n0599 = 0;
				}
				if (n0617 && n0599 == 0)
				{
					n0618 = false;
				}
			}
			else
			{
				n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb(128, Color.Black));
			}
		}

		private void n0674()
		{
			n0732 = TextureLoader.FromStream(n0006, n0136.n0137("THINK"), 800, 600, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0621 = false;
			n0620 = false;
			n0619 = 0;
		}

		private void n0678()
		{
			n0732.Dispose();
		}

		public void n0452(bool n0399, bool n0442)
		{
			if (!n0442)
			{
				n0621 = true;
				n0620 = false;
				n0655 = false;
				if (n0399)
				{
					n0619 = 10;
				}
				else
				{
					n0619 = 0;
				}
				n0660();
				n0733();
			}
			else
			{
				n0620 = true;
				if (n0399)
				{
					n0619 = 10;
					return;
				}
				n0619 = 0;
				n0621 = false;
			}
		}

		private void n0734()
		{
			if (!n0621)
			{
				return;
			}
			if (n0619 > 0)
			{
				if (n0232)
				{
					float num = (n0620 ? ((float)n0619 / 10f) : ((float)(10 - n0619) / 10f));
					n0261(n0732, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.FromArgb((int)(255f * num), Color.White));
					n0619--;
				}
				else
				{
					n0261(n0732, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
					n0619 = 0;
				}
				if (n0620 && n0619 == 0)
				{
					n0621 = false;
				}
			}
			else
			{
				n0261(n0732, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.White);
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
			n0735 = new int[3][];
			n0735[0] = new int[4] { 4, -4, 4, -4 };
			n0735[1] = new int[6] { 8, 0, -8, 8, 0, -8 };
			n0735[2] = new int[16]
			{
				16, 0, -16, 0, 16, 0, -16, 0, 8, -8,
				8, -8, 4, -4, 4, -4
			};
			n0736 = new int[3][];
			n0736[0] = new int[4] { 0, 0, 4, 4 };
			n0736[1] = new int[6] { 0, 0, 4, 4, 8, 8 };
			n0736[2] = new int[16]
			{
				0, 0, 8, 8, 16, 16, 8, 8, 0, 0,
				8, 8, 0, 0, 4, 4
			};
			for (int i = 0; i < 3; i++)
			{
				n0550[i] = null;
				n0737[i] = null;
				n0738[i] = null;
				n0603[i] = 0;
				n0604[i] = 0;
				n0739[i] = 0;
				n0740[i] = 0;
				n0741[i] = Size.Empty;
				n0742[i] = Size.Empty;
			}
		}

		private void n0630()
		{
			for (int i = 0; i < 3; i++)
			{
				if (n0550[i] != null)
				{
					n0550[i].Dispose();
				}
				if (n0737[i] != null)
				{
					n0737[i].Dispose();
				}
				if (n0738[i] != null)
				{
					n0738[i].Dispose();
				}
			}
		}

		public void n0395(Texture n0241, Size n0396, int n0309, int n0397, bool n0398, bool n0399)
		{
			if (n0737[n0309] != null)
			{
				n0737[n0309].Dispose();
			}
			n0737[n0309] = n0550[n0309];
			n0742[n0309] = n0741[n0309];
			n0550[n0309] = n0241;
			n0741[n0309] = n0396;
			n0740[n0309] = n0739[n0309];
			if (n0397 != int.MaxValue)
			{
				n0739[n0309] = n0397;
			}
			if (n0241 != null)
			{
				n0743[n0309] = n0398;
			}
			if (n0399)
			{
				n0603[n0309] = 8;
			}
			if (n0621)
			{
				n0733();
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				if (n0738[i] != null)
				{
					n0738[i].Dispose();
					n0738[i] = null;
				}
			}
		}

		public void n0403(int n0309, int n0402)
		{
			n0744[n0309] = n0402;
			n0604[n0309] = n0745[n0402];
		}

		private void n0586()
		{
			for (int i = 0; i < 3; i++)
			{
				if (n0603[i] > 0)
				{
					if (n0232)
					{
						if (n0737[i] != null && n0550[i] != null)
						{
							if (n0603[i] > 2)
							{
								int alpha = (int)(Math.Pow((double)(8 - n0603[i]) / 6.0, 2.0) * 255.0 / (double)((!n0743[i]) ? 1 : 2));
								n0261(n0737[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0740[i] * 15 - n0742[i].Width / 2, 600 - n0742[i].Height), n0743[i] ? Color.FromArgb(128, Color.White) : Color.White);
								n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.FromArgb(alpha, Color.White));
							}
							else
							{
								int alpha2 = (int)(Math.Pow((double)n0603[i] / 2.0, 2.0) * 255.0 / (double)((!n0743[i]) ? 1 : 2));
								n0261(n0737[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0742[i].Width / 2, 600 - n0742[i].Height), Color.FromArgb(alpha2, Color.White));
								n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), n0743[i] ? Color.FromArgb(128, Color.White) : Color.White);
							}
						}
						else if (n0737[i] != null)
						{
							n0261(n0737[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0740[i] * 15 - n0742[i].Width / 2, 600 - n0742[i].Height), Color.FromArgb((int)((float)n0603[i] / 8f * 255f / (float)((!n0743[i]) ? 1 : 2)), Color.White));
						}
						else
						{
							if (!(n0550[i] != null))
							{
								throw new Exception("No Chara");
							}
							n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.FromArgb((int)((float)(8 - n0603[i]) / 8f * 255f / (float)((!n0743[i]) ? 1 : 2)), Color.White));
						}
						int[] array;
						IntPtr intPtr;
						(array = n0603)[(int)(intPtr = (IntPtr)i)] = array[(int)intPtr] - 1;
					}
					else
					{
						if (n0550[i] != null)
						{
							n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.White);
						}
						n0603[i] = 0;
					}
				}
				else
				{
					if (!(n0550[i] != null))
					{
						continue;
					}
					if (n0604[i] > 0)
					{
						if (n0232 && n0743[i])
						{
							n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2 + n0735[n0744[i]][n0745[n0744[i]] - n0604[i]], 600 - n0741[i].Height + n0736[n0744[i]][n0745[n0744[i]] - n0604[i]]), n0746);
						}
						else
						{
							n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2 + n0735[n0744[i]][n0745[n0744[i]] - n0604[i]], 600 - n0741[i].Height + n0736[n0744[i]][n0745[n0744[i]] - n0604[i]]), Color.White);
						}
						int[] array2;
						IntPtr intPtr2;
						(array2 = n0604)[(int)(intPtr2 = (IntPtr)i)] = array2[(int)intPtr2] - 1;
					}
					else if (n0232 && n0743[i])
					{
						n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), n0746);
					}
					else if (n0621)
					{
						if (n0619 > 0)
						{
							float num = (n0620 ? ((float)n0619 / 10f) : ((float)(10 - n0619) / 10f));
							n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.White);
							n0261(n0738[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.FromArgb((int)(255f * num), Color.White));
						}
						else
						{
							n0261(n0738[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.White);
						}
					}
					else
					{
						n0261(n0550[i], Rectangle.Empty, Rectangle.Empty, new Point(400 + n0739[i] * 15 - n0741[i].Width / 2, 600 - n0741[i].Height), Color.White);
					}
				}
			}
		}

		private void n0733()
		{
			for (int i = 0; i < 3; i++)
			{
				if (n0738[i] != null)
				{
					n0738[i].Dispose();
				}
				if (!(n0550[i] != null))
				{
					continue;
				}
				n0738[i] = TextureLoader.FromStream(n0006, n0136.n0137("WHITE"), n0741[i].Width, n0741[i].Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
				uint[,] array = (uint[,])n0550[i].LockRectangle(typeof(uint), 0, LockFlags.None, n0741[i].Width, n0741[i].Height);
				uint[,] array2 = (uint[,])n0738[i].LockRectangle(typeof(uint), 0, LockFlags.None, n0741[i].Width, n0741[i].Height);
				for (int j = 0; j < n0741[i].Width; j++)
				{
					for (int k = 0; k < n0741[i].Height; k++)
					{
						uint num = array[j, k];
						uint num2 = ((num & 0xFF) + ((num >> 8) & 0xFF) + ((num >> 16) & 0xFF)) / 3;
						uint num3 = (num >> 24) & 0xFF;
						array2[j, k] = (num3 << 24) + (num2 << 16) + (num2 << 8) + num2;
					}
				}
				n0550[i].UnlockRectangle(0);
				n0738[i].UnlockRectangle(0);
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
			n0593 = new Microsoft.DirectX.Direct3D.Font(n0006, new System.Drawing.Font(n0362.FontFamily, 24f));
		}

		public void n0429(string[] n0241)
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
				if (n0232)
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
				n0596(n0747[(int)n0363], 0, n0747[(int)n0363].Length, n0749, n0750.Width, n0750.Height, 2, HorizontalAlign.n0438, 15, n0593, Color.FromArgb(alpha, Color.White), Color.FromArgb(alpha, Color.Black));
				n0579.Add(1);
				n0748--;
			}
		}

		private void n0566()
		{
			n0605 = null;
			n0751 = HorizontalAlign.n0437;
			n0606 = TextRevealMode.n0487;
			n0607 = 0.0;
			n0609 = 0;
			n0600 = 0;
			n0624 = 0;
			n0752 = false;
			n0753 = Color.White;
			n0754 = Color.Black;
			n0755 = 0.0;
			n0756 = n0757;
			for (int i = 0; i < 3; i++)
			{
				n0608[i] = 0;
			}
			n0626();
		}

		private void n0632()
		{
			if (n0594 != null)
			{
				n0594.Dispose();
			}
		}

		private void n0626()
		{
			if (n0594 != null)
			{
				n0594.Dispose();
			}
			n0594 = new Microsoft.DirectX.Direct3D.Font(n0006, new System.Drawing.Font(n0362.FontFamily, 24f));
			if (n0605 == null || n0608[(int)n0363] + n0624 <= 0 || n0624 <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (n0608[i] + n0624 >= n0605[i].Length)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				n0624 = n0605[(int)n0363].Length - n0608[(int)n0363];
				if (n0606 == TextRevealMode.n0493)
				{
					n0607 = n0605[(int)n0363].Length - n0608[(int)n0363] + 4;
				}
			}
		}

		public void n0408(string[] n0241, HorizontalAlign n0410, TextRevealMode n0411, bool n0758, Color n0412, Color n0413, double n0533)
		{
			if (n0241 == null)
			{
				return;
			}
			n0751 = n0410;
			n0606 = n0411;
			n0752 = n0758;
			n0753 = n0412;
			n0754 = n0413;
			n0755 = n0533;
			if (n0605 == null)
			{
				n0605 = new string[3];
				for (int i = 0; i < 3; i++)
				{
					n0605[i] = "";
				}
			}
			for (int j = 0; j < 3; j++)
			{
				n0608[j] = n0605[j].Length;
				string[] array;
				IntPtr intPtr;
				(array = n0605)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + n0241[j];
			}
			n0624 = 0;
			switch (n0606)
			{
			case TextRevealMode.n0493:
				n0607 = 0.0;
				break;
			case TextRevealMode.n0487:
				n0609 = 10;
				break;
			}
		}

		public void n0427(bool n0399)
		{
			if (n0399)
			{
				n0600 = 10;
				return;
			}
			n0600 = 0;
			n0607 = 0.0;
			n0609 = 0;
			n0605 = null;
			for (int i = 0; i < 3; i++)
			{
				n0608[i] = 0;
			}
			n0624 = 0;
		}

		private void n0588()
		{
			if (n0605 == null)
			{
				return;
			}
			if (n0600 > 0)
			{
				if (n0232)
				{
					int alpha = (int)((float)n0600 / 10f * 255f);
					n0596(n0605[(int)n0363], 0, n0605[(int)n0363].Length, n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, Color.FromArgb(alpha, n0753), (n0754 == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha, n0754));
					n0579.Add(2);
					n0600--;
				}
				else
				{
					n0600 = 0;
				}
				if (n0600 == 0)
				{
					n0607 = 0.0;
					n0609 = 0;
					n0605 = null;
					for (int i = 0; i < 3; i++)
					{
						n0608[i] = 0;
					}
					n0624 = 0;
				}
				return;
			}
			switch (n0606)
			{
			case TextRevealMode.n0493:
				if (n0607 < (double)(n0605[(int)n0363].Length - n0608[(int)n0363] + 4))
				{
					if (n0608[(int)n0363] > 0)
					{
						n0756 = n0596(n0605[(int)n0363], 0, n0608[(int)n0363], n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, n0753, n0754);
						n0579.Add(2);
					}
					if (n0755 > 0.0 && n0238)
					{
						try
						{
							double num = n0558.n0760() / n0755;
							n0607 = (double)(n0605[(int)n0363].Length - n0608[(int)n0363] + 4) * num;
						}
						catch (NullReferenceException)
						{
							n0607 = n0605[(int)n0363].Length - n0608[(int)n0363] + 4;
						}
					}
					else
					{
						n0607 += (double)n0237 / 6.0;
					}
					n0756 = n0596(n0605[(int)n0363], n0608[(int)n0363], (double)n0608[(int)n0363] + n0607 - 4.0, n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, true, 4, n0753, n0754);
					n0579.Add(2);
					n0624 = (int)(n0607 - 4.0);
					if (n0624 < 0)
					{
						n0624 = 0;
					}
				}
				else
				{
					n0756 = n0596(n0605[(int)n0363], n0605[(int)n0363].Length, n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, 4, n0753, n0754);
					n0579.Add(2);
				}
				break;
			case TextRevealMode.n0487:
				if (n0608[(int)n0363] + n0624 > 0)
				{
					n0756 = n0596(n0605[(int)n0363], 0, n0608[(int)n0363] + n0624, n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, n0753, n0754);
					n0579.Add(2);
				}
				if (n0609 > 0)
				{
					if (n0232)
					{
						int alpha2 = (int)((float)(10 - n0609) / 10f * 255f);
						n0756 = n0596(n0605[(int)n0363], n0608[(int)n0363], n0605[(int)n0363].Length, n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, Color.FromArgb(alpha2, n0753), (n0754 == Color.Transparent) ? Color.Transparent : Color.FromArgb(alpha2, n0754));
						n0579.Add(2);
						n0609--;
					}
					else
					{
						n0756 = n0596(n0605[(int)n0363], n0608[(int)n0363], n0605[(int)n0363].Length, n0757, n0759.Width, n0759.Height, 2, n0751, 25, n0594, n0753, n0754);
						n0579.Add(2);
						n0609 = 0;
					}
					if (n0609 == 0)
					{
						n0624 = n0605[(int)n0363].Length - n0608[(int)n0363];
					}
				}
				break;
			}
			if (!n0752 || ((n0606 != TextRevealMode.n0493 || !n0238) && n0558.n0382()))
			{
				return;
			}
			switch (n0606)
			{
			case TextRevealMode.n0493:
				if (n0607 >= (double)n0605[(int)n0363].Length - (double)n0608[(int)n0363] + 4.0)
				{
					n0637(n0756.X, n0756.Y);
				}
				break;
			case TextRevealMode.n0487:
				if (n0609 == 0)
				{
					n0637(n0756.X, n0756.Y);
				}
				break;
			}
		}

		private void n0567()
		{
			n0761 = TextureLoader.FromStream(n0006, n0136.n0137("DIALOGUE"), n0762.Width, n0762.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0763 = TextureLoader.FromStream(n0006, n0136.n0137("CHARA"), n0764.Width, n0764.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0601 = 0;
			n0622 = false;
			n0623 = false;
			n0765 = false;
		}

		private void n0633()
		{
			n0761.Dispose();
			n0763.Dispose();
		}

		protected bool n0766(bool n0390)
		{
			if (n0390)
			{
				n0765 = true;
				n0261(n0763, Rectangle.Empty, Rectangle.Empty, n0767, n0236);
				return true;
			}
			if (!n0623)
			{
				n0623 = true;
				n0601 = ((!n0768) ? 3 : 0);
				n0622 = false;
			}
			if (n0768)
			{
				n0768 = false;
			}
			if (n0601 > 0)
			{
				if (n0232)
				{
					if (!n0622 && n0601 < 3)
					{
						float num = (float)(3 - n0601) / 3f;
						float num2 = ((float)n0762.X + (1f - num) / 2f * (float)n0762.Width) / num;
						float num3 = ((float)n0762.Y + (1f - num) / 2f * (float)n0762.Height) / num;
						float num4 = num * (float)n0762.Width;
						float num5 = num * (float)n0762.Height;
						n0261(n0761, new Rectangle(0, 0, n0762.Width, n0762.Height), new Rectangle(0, 0, (int)num4, (int)num5), new Point((int)num2, (int)num3), n0236);
						n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					else if (n0622 && n0601 > 0)
					{
						float num6 = (float)n0601 / 3f;
						float num7 = ((float)n0762.X + (1f - num6) / 2f * (float)n0762.Width) / num6;
						float num8 = ((float)n0762.Y + (1f - num6) / 2f * (float)n0762.Height) / num6;
						float num9 = num6 * (float)n0762.Width;
						float num10 = num6 * (float)n0762.Height;
						n0261(n0761, new Rectangle(0, 0, n0762.Width, n0762.Height), new Rectangle(0, 0, (int)num9, (int)num10), new Point((int)num7, (int)num8), n0236);
						n0261(n0135, Rectangle.Empty, Rectangle.Empty, Point.Empty, Color.Transparent);
					}
					n0601--;
				}
				else
				{
					n0261(n0761, Rectangle.Empty, Rectangle.Empty, n0762.Location, n0236);
					n0601 = 0;
				}
				if (n0622 && n0601 == 0)
				{
					n0623 = false;
				}
				return false;
			}
			n0261(n0761, Rectangle.Empty, Rectangle.Empty, n0762.Location, n0236);
			return true;
		}

		private void n0568()
		{
			n0610 = null;
			n0769 = null;
			n0611 = 0.0;
			n0613 = 0;
			n0614 = 0.0;
			n0770 = false;
			n0771 = n0772;
			for (int i = 0; i < 3; i++)
			{
				n0612[i] = 0;
			}
			n0627();
		}

		private void n0634()
		{
			if (n0595 != null)
			{
				n0595.Dispose();
			}
		}

		private void n0627()
		{
			if (n0595 != null)
			{
				n0595.Dispose();
			}
			n0595 = new Microsoft.DirectX.Direct3D.Font(n0006, new System.Drawing.Font(n0362.FontFamily, 24f));
			if (n0610 == null || n0612[(int)n0363] + n0613 <= 0 || n0613 <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < 3; i++)
			{
				if (n0612[i] + n0613 >= n0610[i].Length)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				n0613 = n0610[(int)n0363].Length - n0612[(int)n0363];
				n0611 = n0610[(int)n0363].Length - n0612[(int)n0363] + 4;
			}
		}

		public void n0419(string[] n0241, string[] n0390, double n0533, bool n0420, bool n0168)
		{
			if (n0241 == null)
			{
				return;
			}
			n0614 = n0533;
			n0769 = n0390;
			n0770 = n0420;
			n0768 = n0168;
			if (n0610 == null)
			{
				n0610 = new string[3];
				for (int i = 0; i < 3; i++)
				{
					n0610[i] = "";
				}
			}
			for (int j = 0; j < 3; j++)
			{
				n0612[j] = n0610[j].Length;
				string[] array;
				IntPtr intPtr;
				(array = n0610)[(int)(intPtr = (IntPtr)j)] = array[(int)intPtr] + n0241[j];
			}
			n0613 = 0;
			n0611 = 0.0;
		}

		public void n0379(bool n0413, bool n0773, bool n0168)
		{
			if (n0770)
			{
				return;
			}
			n0611 = 0.0;
			n0610 = null;
			n0769 = null;
			n0768 = n0168 && n0413;
			for (int i = 0; i < 3; i++)
			{
				n0612[i] = 0;
			}
			n0613 = 0;
			if (n0413)
			{
				n0601 = ((!n0768) ? 3 : 0);
				if (n0768)
				{
					n0623 = false;
				}
				n0622 = true;
			}
			if (n0773)
			{
				n0765 = false;
			}
		}

		private void n0589()
		{
			if (n0610 == null)
			{
				if (n0623)
				{
					n0766(false);
				}
				if (n0765)
				{
					n0766(true);
				}
			}
			else
			{
				if (!n0766(false))
				{
					return;
				}
				if (n0611 < (double)(n0610[(int)n0363].Length - n0612[(int)n0363] + 4))
				{
					if (n0614 > 0.0 && n0238)
					{
						try
						{
							double num = n0558.n0760() / n0614;
							n0611 = (double)(n0610[(int)n0363].Length - n0612[(int)n0363] + 4) * num;
						}
						catch (NullReferenceException)
						{
							n0611 = n0610[(int)n0363].Length - n0612[(int)n0363] + 4;
						}
					}
					else
					{
						n0611 += (double)n0237 / 6.0;
					}
					n0771 = n0596(n0610[(int)n0363], (double)n0612[(int)n0363] + n0611 - 4.0, n0772, n0774.Width, n0774.Height, 2, n0595, 4, Color.White, Color.Black);
					n0579.Add(3);
					n0613 = (int)(n0611 - 4.0);
					if (n0613 < 0)
					{
						n0613 = 0;
					}
				}
				else
				{
					n0771 = n0596(n0610[(int)n0363], n0610[(int)n0363].Length, n0772, n0774.Width, n0774.Height, 2, n0595, 4, Color.White, Color.Black);
					n0579.Add(3);
				}
				if ((n0238 || !n0558.n0382()) && n0611 >= (double)n0610[(int)n0363].Length - (double)n0612[(int)n0363] + 4.0)
				{
					n0637(n0771.X, n0771.Y);
				}
				if (n0769 != null && n0766(true))
				{
					n0596(n0769[(int)n0363], new Point(n0775.X - n0774.Width * n0769[(int)n0363].Length / 2, n0775.Y), n0774.Width, n0774.Height, 2, n0595, Color.White, Color.Black);
					n0579.Add(3);
				}
			}
		}

		public void n0467(int n0241)
		{
			n0560 = n0241;
		}

		private void n0569()
		{
			n0643 = TextureLoader.FromStream(n0006, n0136.n0137("ICON"), n0776.Width, n0776.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);
			n0777 = 0;
		}

		private void n0635()
		{
			n0643.Dispose();
		}

		private void n0592()
		{
			if (n0560 > 0)
			{
				Rectangle n880 = new Rectangle(n0777 / 3 * 32, n0560 * 32, 32, 32);
				n0261(n0643, n880, Rectangle.Empty, n0778, Color.White);
				n0777++;
				if (n0777 == 9)
				{
					n0777 = 0;
				}
			}
		}
	}
}
