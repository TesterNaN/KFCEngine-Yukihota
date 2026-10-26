using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Yukihota.Audio
{
	// Token: 0x02000040 RID: 64
	public class MediaPackage
	{
		// Token: 0x06001FEE RID: 8174
		[DllImport("winmm.dll")]
		private static extern long mciSendString(string command, StringBuilder returnText, int returnCapacity, IntPtr callback);

		// Token: 0x06001FEF RID: 8175 RVA: 0x00021880 File Offset: 0x00020880
		public MediaPackage(string mediaAlias)
		{
			this.opened = false;
			this.playing = false;
			this.paused = false;
			this.alias = mediaAlias;
			this.timer = new Timer();
			this.timer.Interval = 50;
			this.timer.Enabled = true;
			this.timer.Tick += new EventHandler(this.OnTimerTick);
			this.fileName = "";
			this.isWaveAudio = true;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00021904 File Offset: 0x00020904
		~MediaPackage()
		{
			this.Close();
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x0002193C File Offset: 0x0002093C
		public bool IsOpen()
		{
			return this.opened;
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x00021944 File Offset: 0x00020944
		public void Open(string path)
		{
			if (this.opened)
			{
				this.Close();
			}
			string text = "open \"" + path + "\" type waveaudio alias " + this.alias;
			MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
			this.opened = true;
			this.playing = false;
			this.paused = false;
			this.fileName = path;
			this.isWaveAudio = true;
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000219B0 File Offset: 0x000209B0
		public void Open(string path, PictureBox displayBox)
		{
			if (this.opened)
			{
				this.Close();
			}
			string text = string.Concat(new object[]
			{
				"open \"",
				path,
				"\" type mpegvideo alias ",
				this.alias,
				" style child parent ",
				displayBox.Handle.ToInt32()
			});
			MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
			text = string.Concat(new object[] { "put ", this.alias, " window at 0 0 ", displayBox.Width, " ", displayBox.Height });
			MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
			this.opened = true;
			this.playing = false;
			this.paused = false;
			this.fileName = path;
			this.isWaveAudio = false;
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00021AB8 File Offset: 0x00020AB8
		public void Close()
		{
			if (this.opened)
			{
				this.Stop();
				string text = "close " + this.alias + " wait";
				MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
				this.opened = false;
				this.playing = false;
				this.paused = false;
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x00021B0C File Offset: 0x00020B0C
		public void Play(bool loop, int start)
		{
			if (this.opened)
			{
				string text = "play " + this.alias;
				this.loopEnabled = loop;
				text = text + " from " + start;
				MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
				this.playing = true;
				this.paused = false;
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00021B70 File Offset: 0x00020B70
		public void Play(bool loop)
		{
			this.Play(loop, 0);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x00021B7C File Offset: 0x00020B7C
		public void Play()
		{
			this.Play(false, 0);
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x00021B88 File Offset: 0x00020B88
		public void Stop()
		{
			if (this.opened && this.playing)
			{
				string text = "stop " + this.alias;
				MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
				this.playing = false;
				this.paused = false;
			}
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x00021BD4 File Offset: 0x00020BD4
		public void Pause()
		{
			if (this.opened && this.playing && !this.paused)
			{
				string text = "pause " + this.alias;
				MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
				this.paused = true;
			}
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00021C20 File Offset: 0x00020C20
		public void Resume()
		{
			if (this.opened && this.playing && this.paused)
			{
				string text = "resume " + this.alias;
				MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
				this.paused = false;
			}
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00021C6C File Offset: 0x00020C6C
		public bool IsPlaying()
		{
			return this.playing && this.opened;
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x00021C84 File Offset: 0x00020C84
		public int GetLength()
		{
			if (this.opened)
			{
				string text = "status " + this.alias + " length";
				StringBuilder stringBuilder = new StringBuilder(128);
				MediaPackage.mciSendString(text, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				int parsed = -1;
				try
				{
					parsed = int.Parse(stringBuilder.ToString());
				}
				catch
				{
				}
				return parsed;
			}
			return 0;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00021D00 File Offset: 0x00020D00
		public int GetPosition()
		{
			if (!this.opened)
			{
				return 0;
			}
			if (this.playing)
			{
				string text = "status " + this.alias + " position";
				StringBuilder stringBuilder = new StringBuilder(128);
				MediaPackage.mciSendString(text, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				int parsed = -1;
				try
				{
					parsed = int.Parse(stringBuilder.ToString());
				}
				catch
				{
				}
				return parsed;
			}
			return this.GetLength();
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00021D90 File Offset: 0x00020D90
		public int GetVolume()
		{
			if (this.opened && this.playing && !this.paused)
			{
				string text = "status " + this.alias + " volume";
				StringBuilder stringBuilder = new StringBuilder(128);
				MediaPackage.mciSendString(text, stringBuilder, stringBuilder.Capacity, IntPtr.Zero);
				return int.Parse(stringBuilder.ToString());
			}
			return 0;
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00021DF8 File Offset: 0x00020DF8
		public void SetVolume(int volume)
		{
			if (this.opened && this.playing && !this.paused && volume >= 0 && volume <= 1000)
			{
				string text;
				if (this.isWaveAudio)
				{
					int start = this.GetPosition();
					this.Close();
					text = "open \"" + this.fileName + "\" type mpegvideo alias " + this.alias;
					MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
					this.isWaveAudio = false;
					text = string.Concat(new object[] { "play ", this.alias, " from ", start });
					MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
					this.opened = true;
					this.playing = true;
					this.paused = false;
				}
				text = "setaudio " + this.alias + " volume to " + volume.ToString();
				MediaPackage.mciSendString(text, null, 0, IntPtr.Zero);
				return;
			}
			if (volume < 0 && volume > 1000)
			{
				throw new ArgumentOutOfRangeException("Volume value must be 0 to 1000");
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x00021F30 File Offset: 0x00020F30
		private void OnTimerTick(object sender, EventArgs eventArgs)
		{
			if (this.playing && !this.paused && this.GetPosition() >= this.GetLength())
			{
				if (this.loopEnabled)
				{
					this.Stop();
					this.Play(true);
					return;
				}
				this.Stop();
			}
		}

		// Token: 0x04000278 RID: 632
		private bool opened;

		// Token: 0x04000279 RID: 633
		private bool playing;

		// Token: 0x0400027A RID: 634
		private bool paused;

		// Token: 0x0400027B RID: 635
		private string alias;

		// Token: 0x0400027C RID: 636
		private Timer timer;

		// Token: 0x0400027D RID: 637
		private bool loopEnabled;

		// Token: 0x0400027E RID: 638
		private string fileName;

		// Token: 0x0400027F RID: 639
		private bool isWaveAudio;
	}
}
