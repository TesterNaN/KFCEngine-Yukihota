using System;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

namespace n0001
{
	public class SceneDirector : IDisposable
	{
		public const int n0470 = 800;

		public const int n0471 = 600;

		protected ScenarioScript n0354;

		protected ScreenRenderer n0358;

		protected AudioManager n0356;

		protected EffectObject n0009;

		protected ScenarioResult n0375;

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

		private StaffRoll n0373;

		private Microsoft.DirectX.Direct3D.Device n0133;

		private Sprite n0360;

		private FrameCallback n0361;

		private Texture n0135;

		private System.Drawing.Font n0362;

		private ButtonLayout n0363;

		public SceneDirector(string n0348, int n0246, Microsoft.DirectX.Direct3D.Device n0349, Sprite n0350, FrameCallback n0225, Texture n0351, EffectObject n0352, System.Drawing.Font n0353, ButtonLayout n0046, Color n0266)
		{
			n0354 = new ScenarioScript(n0348, n0246, n0349);
			n0356 = new AudioManager();
			n0358 = new ScreenRenderer(n0349, n0350, n0225, n0351, n0353, n0046, n0266, n0356);
			n0358.SetFont(n0353);
			n0358.SetButtonLayout(n0046);
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
			n0375 = n0354.Advance(out n0377, out n0378, false);
		}

		public void Dispose()
		{
			n0356.Dispose();
			n0354.Dispose();
			n0358.Dispose();
			GC.SuppressFinalize(this);
		}

		public bool Update()
		{
			if (n0374)
			{
				if (!n0373.Update())
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
					n0358.CloseDialogue(n0371, n0372, n0365);
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
				else if (!n0358.IsBusy() && !n0356.n0381() && !n0356.n0382())
				{
					bool flag = false;
					do
					{
						switch (n0375)
						{
						case ScenarioResult.n0174:
							if (n0366)
							{
								n0368 = (((bool)n0378) ? n0384[0] : n0384[2]);
							}
							n0364 = true;
							flag = true;
							goto default;
						case ScenarioResult.n0385:
							if (n0377.GetType().Equals(Color.Transparent.GetType()))
							{
								n0358.SetBackground((Color)n0377, (int)n0378);
							}
							else
							{
								n0358.SetBackground((Texture)n0377, (int)n0378);
							}
							goto default;
						case ScenarioResult.n0387:
							if (n0378.GetType().Equals(false.GetType()))
							{
								n0356.SetBgmVolume((bool)n0378);
							}
							else
							{
								n0356.PlayBgm((string)n0377, (int)((double)n0378 * 1000.0));
							}
							goto default;
						case ScenarioResult.n0390:
							if (((SpriteState)n0378).n0392 == SpriteMode.n0394)
							{
								n0358.SetCharacter((Texture)n0377, ((SpriteState)n0378).n0396, ((SpriteState)n0378).n0309 - 1, ((SpriteState)n0378).n0397, ((SpriteState)n0378).n0398, ((SpriteState)n0378).n0399);
							}
							else if (((SpriteState)n0378).n0392 == SpriteMode.n0400)
							{
								if (n0009 != null)
								{
									n0009.Start(1);
									n0369 = n0401[((SpriteState)n0378).n0402];
								}
								n0358.n0403(((SpriteState)n0378).n0309 - 1, ((SpriteState)n0378).n0402);
							}
							goto default;
						case ScenarioResult.n0404:
							if (!n0365)
							{
								n0356.PlayVoice(((ScriptCommand)n0378).n0407);
							}
							n0358.SetMessage((string[])n0377, ((MessageCommand)n0378).n0410, ((MessageCommand)n0378).n0411, ((MessageCommand)n0378).n0174, ((MessageCommand)n0378).n0412, ((MessageCommand)n0378).n0413, n0356.n0414());
							if (((MessageCommand)n0378).n0168)
							{
								n0358.n0168();
							}
							goto default;
						case ScenarioResult.n0415:
							n0370 = true;
							n0371 = ((CaptionCommand)n0378).n0417;
							n0372 = ((CaptionCommand)n0378).n0418;
							if (!n0365)
							{
								n0356.PlayVoice(((ScriptCommand)n0378).n0407);
							}
							n0358.SetCaption((string[])n0377, ((CaptionCommand)n0378).n0390, n0356.n0414(), ((CaptionCommand)n0378).n0420, ((CaptionCommand)n0378).n0168 || n0367 || n0365);
							if (((CaptionCommand)n0378).n0168)
							{
								n0358.n0168();
							}
							goto default;
						case ScenarioResult.n0421:
							n0356.PlaySe((string)n0377, ((InputState)n0378).n0424, ((InputState)n0378).n0399);
							goto default;
						case ScenarioResult.n0425:
							if ((int)n0378 > 0)
							{
								n0368 = n0384[(int)n0378 - 1];
							}
							goto default;
						case ScenarioResult.n0426:
							n0358.n0427((bool)n0378);
							goto default;
						case ScenarioResult.n0428:
							n0358.ShowNotice((string[])n0377);
							goto default;
						case ScenarioResult.n0430:
							switch (((DrawItem)n0378).n0432)
							{
							case DrawItemType.n0434:
								n0358.ZoomBackground(((AnimItem)n0378).n0437, ((AnimItem)n0378).n0438, ((AnimItem)n0378).n0439, ((AnimItem)n0378).n0440, ((AnimItem)n0378).n0441, ((AnimItem)n0378).n0399, ((AnimItem)n0378).n0442);
								n0367 = false;
								break;
							case DrawItemType.n0443:
								n0358.n0444(((DrawItem)n0378).n0399, ((DrawItem)n0378).n0442);
								n0367 = false;
								break;
							case DrawItemType.n0445:
								n0358.n0446(((DrawItem)n0378).n0442);
								break;
							case DrawItemType.n0447:
								n0358.n0448(((DrawItem)n0378).n0442);
								break;
							case DrawItemType.n0449:
								n0358.n0450(((DrawItem)n0378).n0442);
								break;
							case DrawItemType.n0451:
								n0358.n0452(((DrawItem)n0378).n0399, ((DrawItem)n0378).n0442);
								n0367 = false;
								break;
							case DrawItemType.n0413:
								n0358.SetBackgroundWithWipe(((TextureItem)n0378).n0455, ((TextureItem)n0378).n0456, ((TextureItem)n0378).n0399, ((TextureItem)n0378).n0441);
								n0367 = false;
								break;
							case DrawItemType.n0400:
								if (n0009 != null)
								{
									n0009.Start(1);
									if (!((FlagItem)n0378).n0424)
									{
										n0369 = n0401[2];
									}
									else if (!((FlagItem)n0378).n0442)
									{
										n0369 = int.MaxValue;
									}
									else
									{
										n0369 = 0;
									}
								}
								n0358.n0458(((FlagItem)n0378).n0424, ((FlagItem)n0378).n0442);
								break;
							}
							goto default;
						case ScenarioResult.n0459:
							n0373 = (StaffRoll)n0377;
							n0373.Initialize(n0133, n0360, new FrameCallback(n0361), n0135, new System.Drawing.Font(n0362.FontFamily, 24f), n0363);
							n0374 = true;
							goto default;
						default:
							if (n0375 == ScenarioResult.n0174 || n0375 == ScenarioResult.n0385 || n0375 == ScenarioResult.n0390 || (n0375 == ScenarioResult.n0421 && ((InputState)n0378).n0399) || n0375 == ScenarioResult.n0428 || (n0375 == ScenarioResult.n0430 && ((DrawItem)n0378).n0399) || n0375 == ScenarioResult.n0459 || n0375 == ScenarioResult.n0442)
							{
								n0367 = false;
							}
							n0375 = n0354.Advance(out n0377, out n0378, n0365);
							if (flag)
							{
								break;
							}
							if (n0375 != ScenarioResult.n0174 && n0375 != ScenarioResult.n0385 && n0375 != ScenarioResult.n0404 && n0375 != ScenarioResult.n0415 && n0375 != ScenarioResult.n0425 && n0375 != ScenarioResult.n0428 && n0375 != ScenarioResult.n0459 && n0375 != ScenarioResult.n0442)
							{
								if (n0375 == ScenarioResult.n0390)
								{
									flag = !((SpriteState)n0378).n0462;
								}
								else if (n0375 == ScenarioResult.n0421)
								{
									flag = ((InputState)n0378).n0399;
								}
								else if (n0375 == ScenarioResult.n0426)
								{
									flag = (bool)n0378;
								}
								else if (n0375 == ScenarioResult.n0430)
								{
									flag = !((DrawItem)n0378).n0462;
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
						case ScenarioResult.n0442:
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
				n0373.Update();
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
					n0358.CloseDialogue(n0371, n0372, false);
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
					n0358.CloseDialogue(n0371, n0372, true);
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
					n0358.CloseDialogue(n0371, n0372, false);
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

		public void SetFont(System.Drawing.Font n0353)
		{
			if (!n0374)
			{
				n0358.SetFont(n0353);
				n0362 = n0353;
			}
		}

		public void SetButtonLayout(ButtonLayout n0046)
		{
			if (n0374)
			{
				n0373.SetButtonLayout(n0046);
			}
			n0358.SetButtonLayout(n0046);
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

		public void SetPaused(bool n0241, bool n0469)
		{
			if (!n0374)
			{
				n0356.SetPaused(n0241, n0469);
			}
			else
			{
				n0373.SetPaused(n0241);
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

		public void SetDialogueColor(Color n0049)
		{
			n0358.SetDialogueColor(n0049);
		}

		public void SetTextSpeed(int n0049)
		{
			n0358.SetTextSpeed(n0049);
		}

		public void n0254(bool n0049)
		{
			n0358.n0254(n0049);
		}
	}
}
