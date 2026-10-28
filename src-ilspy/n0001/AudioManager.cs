using System;
using Yukihota.Audio;

namespace Yukihota
{
	public class AudioManager : IDisposable
	{
		private MediaPackage voicePackage;

		private MediaPackage bgmPackage;

		private MediaPackage sePackage;

		protected bool seBlocksScript;

		protected bool voiceSkipped;

		public AudioManager()
		{
			seBlocksScript = false;
			voiceSkipped = false;
			voicePackage = new MediaPackage("VOICE");
			bgmPackage = new MediaPackage("BGM");
			sePackage = new MediaPackage("SE");
		}

		~AudioManager()
		{
			Dispose();
		}

		public void Dispose()
		{
			voicePackage.Close();
			bgmPackage.Close();
			sePackage.Close();
			GC.SuppressFinalize(this);
		}

		public void PlayVoice(string name)
		{
			voiceSkipped = false;
			voicePackage.Close();
			if (name != null)
			{
				voicePackage.Open(name);
				voicePackage.Play();
			}
		}

		public void StopVoice()
		{
			voicePackage.Close();
		}

		public double GetVoiceLength()
		{
			return voicePackage.GetLength();
		}

		public double GetVoicePosition()
		{
			if (voicePackage.IsOpen())
			{
				return voicePackage.GetPosition();
			}
			throw new NullReferenceException();
		}

		public bool IsVoiceBlocking()
		{
			return voicePackage.IsPlaying() && !voiceSkipped;
		}

		public void PlayBgm(string name, int start)
		{
			bgmPackage.Close();
			if (name != null)
			{
				bgmPackage.Open(name);
				bgmPackage.Play(true, start);
			}
		}

		public void StopBgm()
		{
			bgmPackage.Close();
		}

		public void SetBgmVolume(bool loud)
		{
			bgmPackage.SetVolume(loud ? 1000 : 200);
		}

		public void PlaySe(string name, bool loop, bool waitForCompletion)
		{
			sePackage.Close();
			if (name != null)
			{
				sePackage.Open(name);
				sePackage.Play(loop);
				seBlocksScript = waitForCompletion && !loop;
			}
		}

		public void StopSe()
		{
			sePackage.Close();
		}

		public bool IsSeBlocking()
		{
			return sePackage.IsPlaying() && seBlocksScript;
		}

		public void instant()
		{
			seBlocksScript = false;
			voiceSkipped = true;
		}

		public void SetPaused(bool name, bool includeBgm)
		{
			if (name)
			{
				if (voicePackage.IsOpen())
				{
					voicePackage.Pause();
				}
				if (sePackage.IsOpen())
				{
					sePackage.Pause();
				}
				if (includeBgm && bgmPackage.IsOpen())
				{
					bgmPackage.Pause();
				}
			}
			else
			{
				if (voicePackage.IsOpen())
				{
					voicePackage.Resume();
				}
				if (sePackage.IsOpen())
				{
					sePackage.Resume();
				}
				if (includeBgm && bgmPackage.IsOpen())
				{
					bgmPackage.Resume();
				}
			}
		}
	}
}
