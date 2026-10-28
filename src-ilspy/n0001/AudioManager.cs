using System;
using n0062;

namespace n0001
{
	public class AudioManager : IDisposable
	{
		private MediaPackage n0857;

		private MediaPackage n0067;

		private MediaPackage n0858;

		protected bool n0855;

		protected bool n0856;

		public AudioManager()
		{
			n0855 = false;
			n0856 = false;
			n0857 = new MediaPackage("VOICE");
			n0067 = new MediaPackage("BGM");
			n0858 = new MediaPackage("SE");
		}

		~AudioManager()
		{
			Dispose();
		}

		public void Dispose()
		{
			n0857.n0162();
			n0067.n0162();
			n0858.n0162();
			GC.SuppressFinalize(this);
		}

		public void PlayVoice(string n0241)
		{
			n0856 = false;
			n0857.n0162();
			if (n0241 != null)
			{
				n0857.Open(n0241);
				n0857.Play();
			}
		}

		public void n0465()
		{
			n0857.n0162();
		}

		public double n0414()
		{
			return n0857.n0270();
		}

		public double n0760()
		{
			if (n0857.n0859())
			{
				return n0857.n0269();
			}
			throw new NullReferenceException();
		}

		public bool n0382()
		{
			return n0857.n0276() && !n0856;
		}

		public void PlayBgm(string n0241, int n0860)
		{
			n0067.n0162();
			if (n0241 != null)
			{
				n0067.Open(n0241);
				n0067.Play(true, n0860);
			}
		}

		public void n0464()
		{
			n0067.n0162();
		}

		public void SetBgmVolume(bool n0861)
		{
			n0067.SetVolume(n0861 ? 1000 : 200);
		}

		public void PlaySe(string n0241, bool n0424, bool n0399)
		{
			n0858.n0162();
			if (n0241 != null)
			{
				n0858.Open(n0241);
				n0858.Play(n0424);
				n0855 = n0399 && !n0424;
			}
		}

		public void n0466()
		{
			n0858.n0162();
		}

		public bool n0381()
		{
			return n0858.n0276() && n0855;
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
				if (n0857.n0859())
				{
					n0857.SetPaused();
				}
				if (n0858.n0859())
				{
					n0858.SetPaused();
				}
				if (n0469 && n0067.n0859())
				{
					n0067.SetPaused();
				}
			}
			else
			{
				if (n0857.n0859())
				{
					n0857.n0165();
				}
				if (n0858.n0859())
				{
					n0858.n0165();
				}
				if (n0469 && n0067.n0859())
				{
					n0067.n0165();
				}
			}
		}
	}
}
