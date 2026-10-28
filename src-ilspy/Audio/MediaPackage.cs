using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Yukihota.Audio
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
		private static extern long mciSendString(string command, StringBuilder returnBuffer, int returnLength, IntPtr hwnd);

		public MediaPackage(string mediaAlias)
		{
			opened = false;
			playing = false;
			paused = false;
			alias = mediaAlias;
			timer = new Timer();
			timer.Interval = 50;
			timer.Enabled = true;
			timer.Tick += new EventHandler(this.OnTimerTick);
			fileName = "";
			isWaveAudio = true;
		}

		~MediaPackage()
		{
			Close();
		}

		public bool IsOpen()
		{
			return opened;
		}

		public void Open(string path)
		{
			if (opened)
			{
				Close();
			}
			string command = "open \"" + path + "\" type waveaudio alias " + alias;
			mciSendString(command, null, 0, IntPtr.Zero);
			opened = true;
			playing = false;
			paused = false;
			fileName = path;
			isWaveAudio = true;
		}

		public void Open(string path, PictureBox displayBox)
		{
			if (opened)
			{
				Close();
			}
			string command = "open \"" + path + "\" type mpegvideo alias " + alias + " style child parent " + displayBox.Handle.ToInt32();
			mciSendString(command, null, 0, IntPtr.Zero);
			command = "put " + alias + " window at 0 0 " + displayBox.Width + " " + displayBox.Height;
			mciSendString(command, null, 0, IntPtr.Zero);
			opened = true;
			playing = false;
			paused = false;
			fileName = path;
			isWaveAudio = false;
		}

		public void Close()
		{
			if (opened)
			{
				Stop();
				string command = "close " + alias + " wait";
				mciSendString(command, null, 0, IntPtr.Zero);
				opened = false;
				playing = false;
				paused = false;
			}
		}

		public void Play(bool loop, int start)
		{
			if (opened)
			{
				string command = "play " + alias;
				loopEnabled = loop;
				command = command + " from " + start;
				mciSendString(command, null, 0, IntPtr.Zero);
				playing = true;
				paused = false;
			}
		}

		public void Play(bool loop)
		{
			Play(loop, 0);
		}

		public void Play()
		{
			Play(false, 0);
		}

		public void Stop()
		{
			if (opened && playing)
			{
				string command = "stop " + alias;
				mciSendString(command, null, 0, IntPtr.Zero);
				playing = false;
				paused = false;
			}
		}

		public void Pause()
		{
			if (opened && playing && !paused)
			{
				string command = "pause " + alias;
				mciSendString(command, null, 0, IntPtr.Zero);
				paused = true;
			}
		}

		public void Resume()
		{
			if (opened && playing && paused)
			{
				string command = "resume " + alias;
				mciSendString(command, null, 0, IntPtr.Zero);
				paused = false;
			}
		}

		public bool IsPlaying()
		{
			return playing && opened;
		}

		public int GetLength()
		{
			if (opened)
			{
				string command = "status " + alias + " length";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(command, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
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

		public int GetPosition()
		{
			if (!opened)
			{
				return 0;
			}
			if (playing)
			{
				string command = "status " + alias + " position";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(command, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
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
			return GetLength();
		}

		public int GetVolume()
		{
			if (opened && playing && !paused)
			{
				string command = "status " + alias + " volume";
				StringBuilder stringBuilder = new StringBuilder(128);
				mciSendString(command, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				return int.Parse(stringBuilder.ToString());
			}
			return 0;
		}

		public void SetVolume(int volume)
		{
			if (opened && playing && !paused && volume >= 0 && volume <= 1000)
			{
				string command;
				if (isWaveAudio)
				{
					int startPosition = GetPosition();
					Close();
					command = "open \"" + fileName + "\" type mpegvideo alias " + alias;
					mciSendString(command, null, 0, IntPtr.Zero);
					isWaveAudio = false;
					command = "play " + alias + " from " + startPosition;
					mciSendString(command, null, 0, IntPtr.Zero);
					opened = true;
					playing = true;
					paused = false;
				}
				command = "setaudio " + alias + " volume to " + volume;
				mciSendString(command, null, 0, IntPtr.Zero);
			}
			else if (volume < 0 && volume > 1000)
			{
				throw new ArgumentOutOfRangeException("Volume value must be 0 to 1000");
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			if (playing && !paused && GetPosition() >= GetLength())
			{
				if (loopEnabled)
				{
					Stop();
					Play(true);
				}
				else
				{
					Stop();
				}
			}
		}
	}
}
