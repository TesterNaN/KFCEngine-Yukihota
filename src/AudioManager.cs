using System;
using Yukihota.Audio;

namespace Yukihota
{
	// Token: 0x02000022 RID: 34
	public class AudioManager : IDisposable
	{
		// Token: 0x06001F29 RID: 7977 RVA: 0x000177D0 File Offset: 0x000167D0
		public AudioManager()
		{
			this.seBlocksScript = false;
			this.voiceSkipped = false;
			this.voicePackage = new MediaPackage("VOICE");
			this.bgmPackage = new MediaPackage("BGM");
			this.sePackage = new MediaPackage("SE");
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x00017824 File Offset: 0x00016824
		~AudioManager()
		{
			this.Dispose();
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0001785C File Offset: 0x0001685C
		public void Dispose()
		{
			this.voicePackage.Close();
			this.bgmPackage.Close();
			this.sePackage.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x00017888 File Offset: 0x00016888
		public void PlayVoice(string name)
		{
			this.voiceSkipped = false;
			this.voicePackage.Close();
			if (name != null)
			{
				this.voicePackage.Open(name);
				this.voicePackage.Play();
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000178B8 File Offset: 0x000168B8
		public void StopVoice()
		{
			this.voicePackage.Close();
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000178C8 File Offset: 0x000168C8
		public double GetVoiceLength()
		{
			return (double)this.voicePackage.GetLength();
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000178D8 File Offset: 0x000168D8
		public double GetVoicePosition()
		{
			if (this.voicePackage.IsOpen())
			{
				return (double)this.voicePackage.GetPosition();
			}
			throw new NullReferenceException();
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000178FC File Offset: 0x000168FC
		public bool IsVoiceBlocking()
		{
			return this.voicePackage.IsPlaying() && !this.voiceSkipped;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x00017918 File Offset: 0x00016918
		public void PlayBgm(string name, int startMilliseconds)
		{
			this.bgmPackage.Close();
			if (name != null)
			{
				this.bgmPackage.Open(name);
				this.bgmPackage.Play(true, startMilliseconds);
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00017944 File Offset: 0x00016944
		public void StopBgm()
		{
			this.bgmPackage.Close();
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x00017954 File Offset: 0x00016954
		public void SetBgmVolume(bool loud)
		{
			this.bgmPackage.SetVolume(loud ? 1000 : 200);
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x00017970 File Offset: 0x00016970
		public void PlaySe(string name, bool loop, bool waitForCompletion)
		{
			this.sePackage.Close();
			if (name != null)
			{
				this.sePackage.Open(name);
				this.sePackage.Play(loop);
				this.seBlocksScript = waitForCompletion && !loop;
			}
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000179A8 File Offset: 0x000169A8
		public void StopSe()
		{
			this.sePackage.Close();
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000179B8 File Offset: 0x000169B8
		public bool IsSeBlocking()
		{
			return this.sePackage.IsPlaying() && this.seBlocksScript;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000179D0 File Offset: 0x000169D0
		public void ReleaseAudioWait()
		{
			this.seBlocksScript = false;
			this.voiceSkipped = true;
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000179E0 File Offset: 0x000169E0
		public void SetPaused(bool name, bool includeBgm)
		{
			if (name)
			{
				if (this.voicePackage.IsOpen())
				{
					this.voicePackage.Pause();
				}
				if (this.sePackage.IsOpen())
				{
					this.sePackage.Pause();
				}
				if (includeBgm && this.bgmPackage.IsOpen())
				{
					this.bgmPackage.Pause();
					return;
				}
			}
			else
			{
				if (this.voicePackage.IsOpen())
				{
					this.voicePackage.Resume();
				}
				if (this.sePackage.IsOpen())
				{
					this.sePackage.Resume();
				}
				if (includeBgm && this.bgmPackage.IsOpen())
				{
					this.bgmPackage.Resume();
				}
			}
		}

		// Token: 0x040000E6 RID: 230
		private MediaPackage voicePackage;

		// Token: 0x040000E7 RID: 231
		private MediaPackage bgmPackage;

		// Token: 0x040000E8 RID: 232
		private MediaPackage sePackage;

		// Token: 0x040000E9 RID: 233
		protected bool seBlocksScript;

		// Token: 0x040000EA RID: 234
		protected bool voiceSkipped;
	}
}
