using System;
using n0062;

namespace n0001
{
	public class AudioManager : IDisposable
	{
		private MediaPackage voicePackage;

		private MediaPackage bgmPackage;

		private MediaPackage sePackage;

		protected bool n0855;

		protected bool n0856;

		public AudioManager()
		{
			n0855 = false;
			n0856 = false;
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
			voicePackage.n0162();
			bgmPackage.n0162();
			sePackage.n0162();
			GC.SuppressFinalize(this);
		}

		public void PlayVoice(string n0241)
		{
			n0856 = false;
			voicePackage.n0162();
			if (n0241 != null)
			{
				voicePackage.Open(n0241);
				voicePackage.Play();
			}
		}

		public void n0465()
		{
			voicePackage.n0162();
		}

		public double n0414()
		{
			return voicePackage.n0270();
		}

		public double n0760()
		{
			if (voicePackage.n0859())
			{
				return voicePackage.n0269();
			}
			throw new NullReferenceException();
		}

		public bool n0382()
		{
			return voicePackage.n0276() && !n0856;
		}

		public void PlayBgm(string n0241, int n0860)
		{
			bgmPackage.n0162();
			if (n0241 != null)
			{
				bgmPackage.Open(n0241);
				bgmPackage.Play(true, n0860);
			}
		}

		public void n0464()
		{
			bgmPackage.n0162();
		}

		public void SetBgmVolume(bool n0861)
		{
			bgmPackage.SetVolume(n0861 ? 1000 : 200);
		}

		public void PlaySe(string n0241, bool n0424, bool n0399)
		{
			sePackage.n0162();
			if (n0241 != null)
			{
				sePackage.Open(n0241);
				sePackage.Play(n0424);
				n0855 = n0399 && !n0424;
			}
		}

		public void n0466()
		{
			sePackage.n0162();
		}

		public bool n0381()
		{
			return sePackage.n0276() && n0855;
		}

		public void n0168()
		{
			n0855 = false;
			n0856 = true;
		}

		public void SetPaused(bool n0241, bool n0469)
		{
			if (n0241)
			{
				if (voicePackage.n0859())
				{
					voicePackage.SetPaused();
				}
				if (sePackage.n0859())
				{
					sePackage.SetPaused();
				}
				if (n0469 && bgmPackage.n0859())
				{
					bgmPackage.SetPaused();
				}
			}
			else
			{
				if (voicePackage.n0859())
				{
					voicePackage.n0165();
				}
				if (sePackage.n0859())
				{
					sePackage.n0165();
				}
				if (n0469 && bgmPackage.n0859())
				{
					bgmPackage.n0165();
				}
			}
		}
	}
}
