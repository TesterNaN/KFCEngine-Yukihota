using System;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

namespace n0001
{
	public class n0247 : IDisposable
	{
		public const int n0470 = 800;

		public const int n0471 = 600;

		protected n0355 n0354;

		protected n0359 n0358;

		protected n0357 n0356;

		protected EffectObject n0009;

		protected n0383 n0375;

		protected object n0377;

		protected object n0378;

		protected bool n0364;

		protected bool n0365;

		protected bool n0366;

		protected bool n0367;

		protected readonly int[] n0384 = new int[3] { 10, 20, 40 };

		protected readonly int[] n0401 = new int[3] { 4, 6, 16 };

		private int n0368;

		private int n0369;

		private bool n0370;

		private bool n0371;

		private bool n0372;

		private bool n0374;

		private n0460 n0373;

		private Microsoft.DirectX.Direct3D.Device n0133;

		private Sprite n0360;

		private n0248 n0361;

		private Texture n0135;

		private System.Drawing.Font n0362;

		private n0040 n0363;

		public n0247(string n0348, int n0246, Microsoft.DirectX.Direct3D.Device n0349, Sprite n0350, n0248 n0225, Texture n0351, EffectObject n0352, System.Drawing.Font n0353, n0040 n0046, Color n0266)
		{
			n0354 = new n0355(n0348, n0246, n0349);
			n0356 = new n0357();
			n0358 = new n0359(n0349, n0350, n0225, n0351, n0353, n0046, n0266, n0356);
			n0358.n0229(n0353);
			n0358.n0206(n0046);
			n0133 = n0349;
			n0360 = n0350;
			n0361 = n0225;
			n0135 = n0351;
			n0362 = n0353;
			n0363 = n0046;
			n0009 = n0352;
			n0364 = false;
			n0365 = false;
			n0366 = false;
			n0367 = false;
			n0368 = 0;
			n0369 = 0;
			n0370 = false;
			n0371 = false;
			n0372 = false;
			n0373 = null;
			n0374 = false;
			n0375 = n0354.n0376(out n0377, out n0378, false);
		}

		public void Dispose()
		{
			n0356.Dispose();
			n0354.Dispose();
			n0358.Dispose();
			GC.SuppressFinalize(this);
		}

		public bool n0228()
		{
			if (n0374)
			{
				if (!n0373.n0228())
				{
					n0373.Dispose();
					n0373 = null;
					n0374 = false;
				}
				return true;
			}
			if ((n0365 && n0364) || (n0366 && n0364 && n0368 == 0))
			{
				n0364 = false;
				if (n0370)
				{
					n0358.n0379(n0371, n0372, n0365);
					n0370 = false;
					n0371 = false;
					n0372 = false;
				}
			}
			if (n0366 || !n0364)
			{
				if (n0369 > 0)
				{
					n0369--;
					if (n0369 == 0)
					{
						n0009.Stop();
					}
				}
				if (n0368 > 0)
				{
					n0368--;
				}
				else if (!n0358.n0380() && !n0356.n0381() && !n0356.n0382())
				{
					bool flag = false;
					do
					{
						switch (n0375)
						{
						case n0383.n0174:
							if (n0366)
							{
								n0368 = (((bool)n0378) ? n0384[0] : n0384[2]);
							}
							n0364 = true;
							flag = true;
							goto default;
						case n0383.n0385:
							if (n0377.GetType().Equals(Color.Transparent.GetType()))
							{
								n0358.n0386((Color)n0377, (int)n0378);
							}
							else
							{
								n0358.n0386((Texture)n0377, (int)n0378);
							}
							goto default;
						case n0383.n0387:
							if (n0378.GetType().Equals(false.GetType()))
							{
								n0356.n0388((bool)n0378);
							}
							else
							{
								n0356.n0389((string)n0377, (int)((double)n0378 * 1000.0));
							}
							goto default;
						case n0383.n0390:
							if (((n0391)n0378).n0392 == n0393.n0394)
							{
								n0358.n0395((Texture)n0377, ((n0391)n0378).n0396, ((n0391)n0378).n0309 - 1, ((n0391)n0378).n0397, ((n0391)n0378).n0398, ((n0391)n0378).n0399);
							}
							else if (((n0391)n0378).n0392 == n0393.n0400)
							{
								if (n0009 != null)
								{
									n0009.Start(1);
									n0369 = n0401[((n0391)n0378).n0402];
								}
								n0358.n0403(((n0391)n0378).n0309 - 1, ((n0391)n0378).n0402);
							}
							goto default;
						case n0383.n0404:
							if (!n0365)
							{
								n0356.n0405(((n0406)n0378).n0407);
							}
							n0358.n0408((string[])n0377, ((n0409)n0378).n0410, ((n0409)n0378).n0411, ((n0409)n0378).n0174, ((n0409)n0378).n0412, ((n0409)n0378).n0413, n0356.n0414());
							if (((n0409)n0378).n0168)
							{
								n0358.n0168();
							}
							goto default;
						case n0383.n0415:
							n0370 = true;
							n0371 = ((n0416)n0378).n0417;
							n0372 = ((n0416)n0378).n0418;
							if (!n0365)
							{
								n0356.n0405(((n0406)n0378).n0407);
							}
							n0358.n0419((string[])n0377, ((n0416)n0378).n0390, n0356.n0414(), ((n0416)n0378).n0420, ((n0416)n0378).n0168 || n0367 || n0365);
							if (((n0416)n0378).n0168)
							{
								n0358.n0168();
							}
							goto default;
						case n0383.n0421:
							n0356.n0422((string)n0377, ((n0423)n0378).n0424, ((n0423)n0378).n0399);
							goto default;
						case n0383.n0425:
							if ((int)n0378 > 0)
							{
								n0368 = n0384[(int)n0378 - 1];
							}
							goto default;
						case n0383.n0426:
							n0358.n0427((bool)n0378);
							goto default;
						case n0383.n0428:
							n0358.n0429((string[])n0377);
							goto default;
						case n0383.n0430:
							switch (((n0431)n0378).n0432)
							{
							case n0433.n0434:
								n0358.n0435(((n0436)n0378).n0437, ((n0436)n0378).n0438, ((n0436)n0378).n0439, ((n0436)n0378).n0440, ((n0436)n0378).n0441, ((n0436)n0378).n0399, ((n0436)n0378).n0442);
								n0367 = false;
								break;
							case n0433.n0443:
								n0358.n0444(((n0431)n0378).n0399, ((n0431)n0378).n0442);
								n0367 = false;
								break;
							case n0433.n0445:
								n0358.n0446(((n0431)n0378).n0442);
								break;
							case n0433.n0447:
								n0358.n0448(((n0431)n0378).n0442);
								break;
							case n0433.n0449:
								n0358.n0450(((n0431)n0378).n0442);
								break;
							case n0433.n0451:
								n0358.n0452(((n0431)n0378).n0399, ((n0431)n0378).n0442);
								n0367 = false;
								break;
							case n0433.n0413:
								n0358.n0453(((n0454)n0378).n0455, ((n0454)n0378).n0456, ((n0454)n0378).n0399, ((n0454)n0378).n0441);
								n0367 = false;
								break;
							case n0433.n0400:
								if (n0009 != null)
								{
									n0009.Start(1);
									if (!((n0457)n0378).n0424)
									{
										n0369 = n0401[2];
									}
									else if (!((n0457)n0378).n0442)
									{
										n0369 = int.MaxValue;
									}
									else
									{
										n0369 = 0;
									}
								}
								n0358.n0458(((n0457)n0378).n0424, ((n0457)n0378).n0442);
								break;
							}
							goto default;
						case n0383.n0459:
							n0373 = (n0460)n0377;
							n0373.n0461(n0133, n0360, n0361, n0135, new System.Drawing.Font(n0362.FontFamily, 24f), n0363);
							n0374 = true;
							goto default;
						default:
							if (n0375 == n0383.n0174 || n0375 == n0383.n0385 || n0375 == n0383.n0390 || (n0375 == n0383.n0421 && ((n0423)n0378).n0399) || n0375 == n0383.n0428 || (n0375 == n0383.n0430 && ((n0431)n0378).n0399) || n0375 == n0383.n0459 || n0375 == n0383.n0442)
							{
								n0367 = false;
							}
							n0375 = n0354.n0376(out n0377, out n0378, n0365);
							if (flag)
							{
								break;
							}
							if (n0375 != n0383.n0174 && n0375 != n0383.n0385 && n0375 != n0383.n0404 && n0375 != n0383.n0415 && n0375 != n0383.n0425 && n0375 != n0383.n0428 && n0375 != n0383.n0459 && n0375 != n0383.n0442)
							{
								if (n0375 == n0383.n0390)
								{
									flag = !((n0391)n0378).n0462;
								}
								else if (n0375 == n0383.n0421)
								{
									flag = ((n0423)n0378).n0399;
								}
								else if (n0375 == n0383.n0426)
								{
									flag = (bool)n0378;
								}
								else if (n0375 == n0383.n0430)
								{
									flag = !((n0431)n0378).n0462;
								}
							}
							else
							{
								flag = true;
							}
							if (n0367 || n0365)
							{
								n0358.n0168();
								n0356.n0168();
								if (n0368 > 0)
								{
									n0368 = 0;
								}
								if (n0369 > 0)
								{
									n0369 = 1;
								}
							}
							break;
						case n0383.n0442:
							n0230();
							return false;
						}
					}
					while (!flag);
				}
			}
			if (n0367 || n0365)
			{
				n0358.n0168();
				n0356.n0168();
				if (n0368 > 0)
				{
					n0368 = 0;
				}
				if (n0369 > 0)
				{
					n0369 = 1;
				}
			}
			n0358.n0463();
			return true;
		}

		public void n0215()
		{
			if (!n0374)
			{
				n0358.n0215();
			}
			else
			{
				n0373.n0228();
			}
		}

		public void n0230()
		{
			if (!n0374)
			{
				n0356.n0464();
				n0356.n0465();
				n0356.n0466();
			}
			else
			{
				n0373.n0230();
				n0373.Dispose();
				n0373 = null;
				n0374 = false;
			}
		}

		public void n0174()
		{
			if (n0374)
			{
				return;
			}
			if (n0364)
			{
				n0364 = false;
				n0356.n0465();
				if (n0370)
				{
					n0358.n0379(n0371, n0372, false);
					n0370 = false;
					n0371 = false;
					n0372 = false;
				}
			}
			else
			{
				n0358.n0168();
				n0356.n0168();
				if (n0368 > 0)
				{
					n0368 = 0;
				}
				if (n0369 > 0)
				{
					n0369 = 1;
				}
				n0367 = true;
			}
			n0365 = false;
			n0366 = false;
			n0358.n0197(false);
			n0358.n0467(0);
		}

		public void n0168(bool n0241, bool n0468)
		{
			if (n0374)
			{
				return;
			}
			n0356.n0465();
			if (n0241 && !n0468)
			{
				n0358.n0467(1);
			}
			else
			{
				n0358.n0467(0);
			}
			if (n0365 == n0241)
			{
				return;
			}
			n0365 = n0241;
			n0366 = false;
			n0358.n0197(false);
			if (n0365)
			{
				if (n0364 && n0370)
				{
					n0358.n0379(n0371, n0372, true);
					n0370 = false;
					n0371 = false;
					n0372 = false;
				}
				n0358.n0168();
				n0356.n0168();
				if (n0368 > 0)
				{
					n0368 = 0;
				}
				if (n0369 > 0)
				{
					n0369 = 1;
				}
				n0364 = false;
			}
		}

		public void n0168()
		{
			if (n0365)
			{
				n0168(false, false);
			}
			else
			{
				n0168(true, false);
			}
		}

		public void n0200(bool n0241)
		{
			if (n0374)
			{
				return;
			}
			if (n0241)
			{
				n0358.n0467(2);
			}
			else
			{
				n0358.n0467(0);
			}
			n0366 = n0241;
			n0365 = false;
			n0358.n0197(false);
			if (n0366 && n0364)
			{
				n0364 = false;
				if (n0370)
				{
					n0358.n0379(n0371, n0372, false);
					n0370 = false;
					n0371 = false;
					n0372 = false;
				}
			}
		}

		public void n0200()
		{
			if (n0366)
			{
				n0200(false);
			}
			else
			{
				n0200(true);
			}
		}

		public void n0197(bool n0241)
		{
			if (!n0374)
			{
				n0366 = false;
				n0365 = false;
				n0358.n0197(n0241);
				n0358.n0467(0);
			}
		}

		public void n0197()
		{
			if (!n0374)
			{
				n0366 = false;
				n0365 = false;
				n0358.n0197();
				n0358.n0467(0);
			}
		}

		public void n0229(System.Drawing.Font n0353)
		{
			if (!n0374)
			{
				n0358.n0229(n0353);
				n0362 = n0353;
			}
		}

		public void n0206(n0040 n0046)
		{
			if (n0374)
			{
				n0373.n0206(n0046);
			}
			n0358.n0206(n0046);
			n0363 = n0046;
		}

		public int n0157()
		{
			return n0354.n0157();
		}

		public int n0159()
		{
			return n0354.n0159();
		}

		public void n0166(bool n0241, bool n0469)
		{
			if (!n0374)
			{
				n0356.n0166(n0241, n0469);
			}
			else
			{
				n0373.n0166(n0241);
			}
		}

		public bool n0188()
		{
			return n0374;
		}

		public void n0249(bool n0049)
		{
			n0358.n0249(n0049);
		}

		public void n0250(bool n0049)
		{
			n0358.n0250(n0049);
		}

		public void n0251(bool n0049)
		{
			n0358.n0251(n0049);
		}

		public void n0252(bool n0049)
		{
			n0358.n0252(n0049);
		}

		public void n0296(Color n0049)
		{
			n0358.n0296(n0049);
		}

		public void n0253(int n0049)
		{
			n0358.n0253(n0049);
		}

		public void n0254(bool n0049)
		{
			n0358.n0254(n0049);
		}
	}
}
