using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace n0062
{
	public class MediaPackage
	{
		private bool opened;

		private bool playing;

		private bool paused;

		private string alias;

		private Timer timer;

		private bool loopEnabled;

		private string fileName;

		private bool isWaveAudio;

		[DllImport("winmm.dll")]
		private static extern long mciSendString(string n0863, StringBuilder n0864, int n0865, IntPtr n0866);

		public MediaPackage(string n0867)
		{
			opened = false;
			playing = false;
			paused = false;
			alias = n0867;
			timer = new Timer();
			timer.Interval = 50;
			timer.Enabled = true;
			timer.Tick += new EventHandler(this.OnTimerTick);
			fileName = "";
			isWaveAudio = true;
		}

		~MediaPackage()
		{
			n0162();
		}

		public bool n0859()
		{
			return opened;
		}

		public void Open(string n0494)
		{
			if (opened)
			{
				n0162();
			}
			string n495 = "open \"" + n0494 + "\" type waveaudio alias " + alias;
			mciSendString(n495, null, 0, IntPtr.Zero);
			opened = true;
			playing = false;
			paused = false;
			fileName = n0494;
			isWaveAudio = true;
		}

		public void Open(string n0494, PictureBox n0876)
		{
			if (opened)
			{
				n0162();
			}
			string n877 = "open \"" + n0494 + "\" type mpegvideo alias " + alias + " style child parent " + n0876.Handle.ToInt32();
			mciSendString(n877, null, 0, IntPtr.Zero);
			n877 = "put " + alias + " window at 0 0 " + n0876.Width + " " + n0876.Height;
			mciSendString(n877, null, 0, IntPtr.Zero);
			opened = true;
			playing = false;
			paused = false;
			fileName = n0494;
			isWaveAudio = false;
		}

		public void n0162()
		{
			if (opened)
			{
				n0230();
				string n69 = "close " + alias + " wait";
				mciSendString(n69, null, 0, IntPtr.Zero);
				opened = false;
				playing = false;
				paused = false;
			}
		}

		public void Play(bool n0424, int n0860)
		{
			if (opened)
			{
				string text = "play " + alias;
				loopEnabled = n0424;
				text = text + " from " + n0860;
				mciSendString(text, null, 0, IntPtr.Zero);
				playing = true;
				paused = false;
			}
		}

		public void Play(bool n0424)
		{
			Play(n0424, 0);
		}

		public void Play()
		{
			Play(false, 0);
		}

		public void n0230()
		{
			if (opened && playing)
			{
				string n69 = "stop " + alias;
				mciSendString(n69, null, 0, IntPtr.Zero);
				playing = false;
				paused = false;
			}
		}

		public void SetPaused()
		{
			if (opened && playing && !paused)
			{
				string n69 = "pause " + alias;
				mciSendString(n69, null, 0, IntPtr.Zero);
				paused = true;
			}
		}

		public void n0165()
		{
			if (opened && playing && paused)
			{
				string n69 = "resume " + alias;
				mciSendString(n69, null, 0, IntPtr.Zero);
				paused = false;
			}
		}

		public bool n0276()
		{
			return playing && opened;
		}

		public int n0270()
		{
			if (opened)
			{
				string n69 = "status " + alias + " length";
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
			if (!opened)
			{
				return 0;
			}
			if (playing)
			{
				string n69 = "status " + alias + " position";
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
			if (opened && playing && !paused)
			{
				string n69 = "status " + alias + " volume";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(n69, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				return int.Parse(stringBuilder.ToString());
			}
			return 0;
		}

		public void SetVolume(int n0049)
		{
			if (opened && playing && !paused && n0049 >= 0 && n0049 <= 1000)
			{
				string n69;
				if (isWaveAudio)
				{
					int num = n0269();
					n0162();
					n69 = "open \"" + fileName + "\" type mpegvideo alias " + alias;
					mciSendString(n69, null, 0, IntPtr.Zero);
					isWaveAudio = false;
					n69 = "play " + alias + " from " + num;
					mciSendString(n69, null, 0, IntPtr.Zero);
					opened = true;
					playing = true;
					paused = false;
				}
				n69 = "setaudio " + alias + " volume to " + n0049;
				mciSendString(n69, null, 0, IntPtr.Zero);
			}
			else if (n0049 < 0 && n0049 > 1000)
			{
				throw new ArgumentOutOfRangeException("Volume value must be 0 to 1000");
			}
		}

		private void OnTimerTick(object n0035, EventArgs n0036)
		{
			if (playing && !paused && n0269() >= n0270())
			{
				if (loopEnabled)
				{
					n0230();
					Play(true);
				}
				else
				{
					n0230();
				}
			}
		}
	}
}
