using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace n0062
{
	public class MediaPackage
	{
		private bool n0868;

		private bool n0869;

		private bool n0870;

		private string n0871;

		private Timer n0872;

		private bool n0877;

		private string n0874;

		private bool n0875;

		[DllImport("winmm.dll")]
		private static extern long mciSendString(string n0863, StringBuilder n0864, int n0865, IntPtr n0866);

		public MediaPackage(string n0867)
		{
			n0868 = false;
			n0869 = false;
			n0870 = false;
			n0871 = n0867;
			n0872 = new Timer();
			n0872.Interval = 50;
			n0872.Enabled = true;
			n0872.Tick += new EventHandler(this.n0873);
			n0874 = "";
			n0875 = true;
		}

		~MediaPackage()
		{
			n0162();
		}

		public bool n0859()
		{
			return n0868;
		}

		public void n0155(string n0494)
		{
			if (n0868)
			{
				n0162();
			}
			string n495 = "open \"" + n0494 + "\" type waveaudio alias " + n0871;
			mciSendString(n495, null, 0, IntPtr.Zero);
			n0868 = true;
			n0869 = false;
			n0870 = false;
			n0874 = n0494;
			n0875 = true;
		}

		public void n0155(string n0494, PictureBox n0876)
		{
			if (n0868)
			{
				n0162();
			}
			string n877 = "open \"" + n0494 + "\" type mpegvideo alias " + n0871 + " style child parent " + n0876.Handle.ToInt32();
			mciSendString(n877, null, 0, IntPtr.Zero);
			n877 = "put " + n0871 + " window at 0 0 " + n0876.Width + " " + n0876.Height;
			mciSendString(n877, null, 0, IntPtr.Zero);
			n0868 = true;
			n0869 = false;
			n0870 = false;
			n0874 = n0494;
			n0875 = false;
		}

		public void n0162()
		{
			if (n0868)
			{
				n0230();
				string n69 = "close " + n0871 + " wait";
				mciSendString(n69, null, 0, IntPtr.Zero);
				n0868 = false;
				n0869 = false;
				n0870 = false;
			}
		}

		public void n0164(bool n0424, int n0860)
		{
			if (n0868)
			{
				string text = "play " + n0871;
				n0877 = n0424;
				text = text + " from " + n0860;
				mciSendString(text, null, 0, IntPtr.Zero);
				n0869 = true;
				n0870 = false;
			}
		}

		public void n0164(bool n0424)
		{
			n0164(n0424, 0);
		}

		public void n0164()
		{
			n0164(false, 0);
		}

		public void n0230()
		{
			if (n0868 && n0869)
			{
				string n69 = "stop " + n0871;
				mciSendString(n69, null, 0, IntPtr.Zero);
				n0869 = false;
				n0870 = false;
			}
		}

		public void n0166()
		{
			if (n0868 && n0869 && !n0870)
			{
				string n69 = "pause " + n0871;
				mciSendString(n69, null, 0, IntPtr.Zero);
				n0870 = true;
			}
		}

		public void n0165()
		{
			if (n0868 && n0869 && n0870)
			{
				string n69 = "resume " + n0871;
				mciSendString(n69, null, 0, IntPtr.Zero);
				n0870 = false;
			}
		}

		public bool n0276()
		{
			return n0869 && n0868;
		}

		public int n0270()
		{
			if (n0868)
			{
				string n69 = "status " + n0871 + " length";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(n69, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				int result = -1;
				try
				{
					result = int.Parse(stringBuilder.ToString());
				}
				catch
				{
				}
				return result;
			}
			return 0;
		}

		public int n0269()
		{
			if (!n0868)
			{
				return 0;
			}
			if (n0869)
			{
				string n69 = "status " + n0871 + " position";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(n69, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				int result = -1;
				try
				{
					result = int.Parse(stringBuilder.ToString());
				}
				catch
				{
				}
				return result;
			}
			return n0270();
		}

		public int n0878()
		{
			if (n0868 && n0869 && !n0870)
			{
				string n69 = "status " + n0871 + " volume";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(n69, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				return int.Parse(stringBuilder.ToString());
			}
			return 0;
		}

		public void n0862(int n0049)
		{
			if (n0868 && n0869 && !n0870 && n0049 >= 0 && n0049 <= 1000)
			{
				string n69;
				if (n0875)
				{
					int num = n0269();
					n0162();
					n69 = "open \"" + n0874 + "\" type mpegvideo alias " + n0871;
					mciSendString(n69, null, 0, IntPtr.Zero);
					n0875 = false;
					n69 = "play " + n0871 + " from " + num;
					mciSendString(n69, null, 0, IntPtr.Zero);
					n0868 = true;
					n0869 = true;
					n0870 = false;
				}
				n69 = "setaudio " + n0871 + " volume to " + n0049;
				mciSendString(n69, null, 0, IntPtr.Zero);
			}
			else if (n0049 < 0 && n0049 > 1000)
			{
				throw new ArgumentOutOfRangeException("Volume value must be 0 to 1000");
			}
		}

		private void n0873(object n0035, EventArgs n0036)
		{
			if (n0869 && !n0870 && n0269() >= n0270())
			{
				if (n0877)
				{
					n0230();
					n0164(true);
				}
				else
				{
					n0230();
				}
			}
		}
	}
}
