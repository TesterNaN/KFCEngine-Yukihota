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

		protected ScenarioScript scenario;

		protected ScreenRenderer renderer;

		protected AudioManager audio;

		protected EffectObject shakeEffect;

		protected ScenarioResult currentResult;

		protected object resultData;

		protected object resultOption;

		protected bool waitingForClick;

		protected bool skipMode;

		protected bool autoMode;

		protected bool finishingAnimation;

		protected readonly int[] waitFrames = new int[3] { 10, 20, 40 };

		protected readonly int[] shockFrames = new int[3] { 4, 6, 16 };

		private int waitFramesLeft;

		private int shockFramesLeft;

		private bool captionCommitPending;

		private bool captionDisappear;

		private bool captionHideName;

		private bool staffRollPlaying;

		private StaffRoll staffRoll;

		private Microsoft.DirectX.Direct3D.Device graphicsDevice;

		private Sprite spriteBatch;

		private FrameCallback frameHandler;

		private Texture whiteTexture;

		private System.Drawing.Font uiFont;

		private ButtonLayout buttonLayout;

		public SceneDirector(string n0348, int n0246, Microsoft.DirectX.Direct3D.Device n0349, Sprite n0350, FrameCallback n0225, Texture n0351, EffectObject n0352, System.Drawing.Font n0353, ButtonLayout n0046, Color n0266)
		{
			scenario = new ScenarioScript(n0348, n0246, n0349);
			audio = new AudioManager();
			renderer = new ScreenRenderer(n0349, n0350, n0225, n0351, n0353, n0046, n0266, audio);
			renderer.SetFont(n0353);
			renderer.SetButtonLayout(n0046);
			graphicsDevice = n0349;
			spriteBatch = n0350;
			frameHandler = n0225;
			whiteTexture = n0351;
			uiFont = n0353;
			buttonLayout = n0046;
			shakeEffect = n0352;
			waitingForClick = false;
			skipMode = false;
			autoMode = false;
			finishingAnimation = false;
			waitFramesLeft = 0;
			shockFramesLeft = 0;
			captionCommitPending = false;
			captionDisappear = false;
			captionHideName = false;
			staffRoll = null;
			staffRollPlaying = false;
			currentResult = scenario.Advance(out resultData, out resultOption, false);
		}

		public void Dispose()
		{
			audio.Dispose();
			scenario.Dispose();
			renderer.Dispose();
			GC.SuppressFinalize(this);
		}

		public bool Update()
		{
			if (staffRollPlaying)
			{
				if (!staffRoll.Update())
				{
					staffRoll.Dispose();
					staffRoll = null;
					staffRollPlaying = false;
				}
				return true;
			}
			if ((skipMode && waitingForClick) || (autoMode && waitingForClick && waitFramesLeft == 0))
			{
				waitingForClick = false;
				if (captionCommitPending)
				{
					renderer.CloseDialogue(captionDisappear, captionHideName, skipMode);
					captionCommitPending = false;
					captionDisappear = false;
					captionHideName = false;
				}
			}
			if (autoMode || !waitingForClick)
			{
				if (shockFramesLeft > 0)
				{
					shockFramesLeft--;
					if (shockFramesLeft == 0)
					{
						shakeEffect.Stop();
					}
				}
				if (waitFramesLeft > 0)
				{
					waitFramesLeft--;
				}
				else if (!renderer.IsBusy() && !audio.n0381() && !audio.n0382())
				{
					bool flag = false;
					do
					{
						switch (currentResult)
						{
						case ScenarioResult.waitClick:
							if (autoMode)
							{
								waitFramesLeft = (((bool)resultOption) ? waitFrames[0] : waitFrames[2]);
							}
							waitingForClick = true;
							flag = true;
							goto default;
						case ScenarioResult.n0385:
							if (resultData.GetType().Equals(Color.Transparent.GetType()))
							{
								renderer.SetBackground((Color)resultData, (int)resultOption);
							}
							else
							{
								renderer.SetBackground((Texture)resultData, (int)resultOption);
							}
							goto default;
						case ScenarioResult.n0387:
							if (resultOption.GetType().Equals(false.GetType()))
							{
								audio.SetBgmVolume((bool)resultOption);
							}
							else
							{
								audio.PlayBgm((string)resultData, (int)((double)resultOption * 1000.0));
							}
							goto default;
						case ScenarioResult.charaNames:
							if (((SpriteState)resultOption).action == SpriteMode.n0394)
							{
								renderer.SetCharacter((Texture)resultData, ((SpriteState)resultOption).size, ((SpriteState)resultOption).index - 1, ((SpriteState)resultOption).location, ((SpriteState)resultOption).isTransparent, ((SpriteState)resultOption).n0399);
							}
							else if (((SpriteState)resultOption).action == SpriteMode.n0400)
							{
								if (shakeEffect != null)
								{
									shakeEffect.Start(1);
									shockFramesLeft = shockFrames[((SpriteState)resultOption).level];
								}
								renderer.n0403(((SpriteState)resultOption).index - 1, ((SpriteState)resultOption).level);
							}
							goto default;
						case ScenarioResult.n0404:
							if (!skipMode)
							{
								audio.PlayVoice(((ScriptCommand)resultOption).voice);
							}
							renderer.SetMessage((string[])resultData, ((MessageCommand)resultOption).align, ((MessageCommand)resultOption).showMode, ((MessageCommand)resultOption).waitClick, ((MessageCommand)resultOption).foreColor, ((MessageCommand)resultOption).shadowColor, audio.n0414());
							if (((MessageCommand)resultOption).instant)
							{
								renderer.instant();
							}
							goto default;
						case ScenarioResult.n0415:
							captionCommitPending = true;
							captionDisappear = ((CaptionCommand)resultOption).disappear;
							captionHideName = ((CaptionCommand)resultOption).hideCharaName;
							if (!skipMode)
							{
								audio.PlayVoice(((ScriptCommand)resultOption).voice);
							}
							renderer.SetCaption((string[])resultData, ((CaptionCommand)resultOption).charaNames, audio.n0414(), ((CaptionCommand)resultOption).isContinuous, ((CaptionCommand)resultOption).instant || finishingAnimation || skipMode);
							if (((CaptionCommand)resultOption).instant)
							{
								renderer.instant();
							}
							goto default;
						case ScenarioResult.n0421:
							audio.PlaySe((string)resultData, ((InputState)resultOption).loop, ((InputState)resultOption).n0399);
							goto default;
						case ScenarioResult.n0425:
							if ((int)resultOption > 0)
							{
								waitFramesLeft = waitFrames[(int)resultOption - 1];
							}
							goto default;
						case ScenarioResult.n0426:
							renderer.n0427((bool)resultOption);
							goto default;
						case ScenarioResult.n0428:
							renderer.ShowNotice((string[])resultData);
							goto default;
						case ScenarioResult.n0430:
							switch (((DrawItem)resultOption).type)
							{
							case DrawItemType.n0434:
								renderer.ZoomBackground(((AnimItem)resultOption).left, ((AnimItem)resultOption).right, ((AnimItem)resultOption).top, ((AnimItem)resultOption).bottom, ((AnimItem)resultOption).durationMode, ((AnimItem)resultOption).n0399, ((AnimItem)resultOption).finish);
								finishingAnimation = false;
								break;
							case DrawItemType.n0443:
								renderer.n0444(((DrawItem)resultOption).n0399, ((DrawItem)resultOption).finish);
								finishingAnimation = false;
								break;
							case DrawItemType.n0445:
								renderer.n0446(((DrawItem)resultOption).finish);
								break;
							case DrawItemType.n0447:
								renderer.n0448(((DrawItem)resultOption).finish);
								break;
							case DrawItemType.n0449:
								renderer.n0450(((DrawItem)resultOption).finish);
								break;
							case DrawItemType.n0451:
								renderer.n0452(((DrawItem)resultOption).n0399, ((DrawItem)resultOption).finish);
								finishingAnimation = false;
								break;
							case DrawItemType.shadowColor:
								renderer.SetBackgroundWithWipe(((TextureItem)resultOption).texture, ((TextureItem)resultOption).blend, ((TextureItem)resultOption).n0399, ((TextureItem)resultOption).durationMode);
								finishingAnimation = false;
								break;
							case DrawItemType.n0400:
								if (shakeEffect != null)
								{
									shakeEffect.Start(1);
									if (!((FlagItem)resultOption).loop)
									{
										shockFramesLeft = shockFrames[2];
									}
									else if (!((FlagItem)resultOption).finish)
									{
										shockFramesLeft = int.MaxValue;
									}
									else
									{
										shockFramesLeft = 0;
									}
								}
								renderer.n0458(((FlagItem)resultOption).loop, ((FlagItem)resultOption).finish);
								break;
							}
							goto default;
						case ScenarioResult.n0459:
							staffRoll = (StaffRoll)resultData;
							staffRoll.Initialize(graphicsDevice, spriteBatch, new FrameCallback(frameHandler), whiteTexture, new System.Drawing.Font(uiFont.FontFamily, 24f), buttonLayout);
							staffRollPlaying = true;
							goto default;
						default:
							if (currentResult == ScenarioResult.waitClick || currentResult == ScenarioResult.n0385 || currentResult == ScenarioResult.charaNames || (currentResult == ScenarioResult.n0421 && ((InputState)resultOption).n0399) || currentResult == ScenarioResult.n0428 || (currentResult == ScenarioResult.n0430 && ((DrawItem)resultOption).n0399) || currentResult == ScenarioResult.n0459 || currentResult == ScenarioResult.finish)
							{
								finishingAnimation = false;
							}
							currentResult = scenario.Advance(out resultData, out resultOption, skipMode);
							if (flag)
							{
								break;
							}
							if (currentResult != ScenarioResult.waitClick && currentResult != ScenarioResult.n0385 && currentResult != ScenarioResult.n0404 && currentResult != ScenarioResult.n0415 && currentResult != ScenarioResult.n0425 && currentResult != ScenarioResult.n0428 && currentResult != ScenarioResult.n0459 && currentResult != ScenarioResult.finish)
							{
								if (currentResult == ScenarioResult.charaNames)
								{
									flag = !((SpriteState)resultOption).sync;
								}
								else if (currentResult == ScenarioResult.n0421)
								{
									flag = ((InputState)resultOption).n0399;
								}
								else if (currentResult == ScenarioResult.n0426)
								{
									flag = (bool)resultOption;
								}
								else if (currentResult == ScenarioResult.n0430)
								{
									flag = !((DrawItem)resultOption).sync;
								}
							}
							else
							{
								flag = true;
							}
							if (finishingAnimation || skipMode)
							{
								renderer.instant();
								audio.instant();
								if (waitFramesLeft > 0)
								{
									waitFramesLeft = 0;
								}
								if (shockFramesLeft > 0)
								{
									shockFramesLeft = 1;
								}
							}
							break;
						case ScenarioResult.finish:
							n0230();
							return false;
						}
					}
					while (!flag);
				}
			}
			if (finishingAnimation || skipMode)
			{
				renderer.instant();
				audio.instant();
				if (waitFramesLeft > 0)
				{
					waitFramesLeft = 0;
				}
				if (shockFramesLeft > 0)
				{
					shockFramesLeft = 1;
				}
			}
			renderer.n0463();
			return true;
		}

		public void n0215()
		{
			if (!staffRollPlaying)
			{
				renderer.n0215();
			}
			else
			{
				staffRoll.Update();
			}
		}

		public void n0230()
		{
			if (!staffRollPlaying)
			{
				audio.n0464();
				audio.n0465();
				audio.n0466();
			}
			else
			{
				staffRoll.n0230();
				staffRoll.Dispose();
				staffRoll = null;
				staffRollPlaying = false;
			}
		}

		public void waitClick()
		{
			if (staffRollPlaying)
			{
				return;
			}
			if (waitingForClick)
			{
				waitingForClick = false;
				audio.n0465();
				if (captionCommitPending)
				{
					renderer.CloseDialogue(captionDisappear, captionHideName, false);
					captionCommitPending = false;
					captionDisappear = false;
					captionHideName = false;
				}
			}
			else
			{
				renderer.instant();
				audio.instant();
				if (waitFramesLeft > 0)
				{
					waitFramesLeft = 0;
				}
				if (shockFramesLeft > 0)
				{
					shockFramesLeft = 1;
				}
				finishingAnimation = true;
			}
			skipMode = false;
			autoMode = false;
			renderer.n0197(false);
			renderer.n0467(0);
		}

		public void instant(bool n0241, bool n0468)
		{
			if (staffRollPlaying)
			{
				return;
			}
			audio.n0465();
			if (n0241 && !n0468)
			{
				renderer.n0467(1);
			}
			else
			{
				renderer.n0467(0);
			}
			if (skipMode == n0241)
			{
				return;
			}
			skipMode = n0241;
			autoMode = false;
			renderer.n0197(false);
			if (skipMode)
			{
				if (waitingForClick && captionCommitPending)
				{
					renderer.CloseDialogue(captionDisappear, captionHideName, true);
					captionCommitPending = false;
					captionDisappear = false;
					captionHideName = false;
				}
				renderer.instant();
				audio.instant();
				if (waitFramesLeft > 0)
				{
					waitFramesLeft = 0;
				}
				if (shockFramesLeft > 0)
				{
					shockFramesLeft = 1;
				}
				waitingForClick = false;
			}
		}

		public void instant()
		{
			if (skipMode)
			{
				instant(false, false);
			}
			else
			{
				instant(true, false);
			}
		}

		public void n0200(bool n0241)
		{
			if (staffRollPlaying)
			{
				return;
			}
			if (n0241)
			{
				renderer.n0467(2);
			}
			else
			{
				renderer.n0467(0);
			}
			autoMode = n0241;
			skipMode = false;
			renderer.n0197(false);
			if (autoMode && waitingForClick)
			{
				waitingForClick = false;
				if (captionCommitPending)
				{
					renderer.CloseDialogue(captionDisappear, captionHideName, false);
					captionCommitPending = false;
					captionDisappear = false;
					captionHideName = false;
				}
			}
		}

		public void n0200()
		{
			if (autoMode)
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
			if (!staffRollPlaying)
			{
				autoMode = false;
				skipMode = false;
				renderer.n0197(n0241);
				renderer.n0467(0);
			}
		}

		public void n0197()
		{
			if (!staffRollPlaying)
			{
				autoMode = false;
				skipMode = false;
				renderer.n0197();
				renderer.n0467(0);
			}
		}

		public void SetFont(System.Drawing.Font n0353)
		{
			if (!staffRollPlaying)
			{
				renderer.SetFont(n0353);
				uiFont = n0353;
			}
		}

		public void SetButtonLayout(ButtonLayout n0046)
		{
			if (staffRollPlaying)
			{
				staffRoll.SetButtonLayout(n0046);
			}
			renderer.SetButtonLayout(n0046);
			buttonLayout = n0046;
		}

		public int n0157()
		{
			return scenario.n0157();
		}

		public int n0159()
		{
			return scenario.n0159();
		}

		public void SetPaused(bool n0241, bool n0469)
		{
			if (!staffRollPlaying)
			{
				audio.SetPaused(n0241, n0469);
			}
			else
			{
				staffRoll.SetPaused(n0241);
			}
		}

		public bool n0188()
		{
			return staffRollPlaying;
		}

		public void n0249(bool n0049)
		{
			renderer.n0249(n0049);
		}

		public void n0250(bool n0049)
		{
			renderer.n0250(n0049);
		}

		public void n0251(bool n0049)
		{
			renderer.n0251(n0049);
		}

		public void n0252(bool n0049)
		{
			renderer.n0252(n0049);
		}

		public void SetDialogueColor(Color n0049)
		{
			renderer.SetDialogueColor(n0049);
		}

		public void SetTextSpeed(int n0049)
		{
			renderer.SetTextSpeed(n0049);
		}

		public void n0254(bool n0049)
		{
			renderer.n0254(n0049);
		}
	}
}
