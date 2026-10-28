using System;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

namespace Yukihota
{
	public class SceneDirector : IDisposable
	{
		public const int ScreenWidth = 800;

		public const int ScreenHeight = 600;

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

		public SceneDirector(string scriptPath, int episode, Microsoft.DirectX.Direct3D.Device device, Sprite sprite, FrameCallback frameCallback, Texture overlayTexture, EffectObject effectObject, System.Drawing.Font font, ButtonLayout layout, Color textColor)
		{
			scenario = new ScenarioScript(scriptPath, episode, device);
			audio = new AudioManager();
			renderer = new ScreenRenderer(device, sprite, frameCallback, overlayTexture, font, layout, textColor, audio);
			renderer.SetFont(font);
			renderer.SetButtonLayout(layout);
			graphicsDevice = device;
			spriteBatch = sprite;
			frameHandler = frameCallback;
			whiteTexture = overlayTexture;
			uiFont = font;
			buttonLayout = layout;
			shakeEffect = effectObject;
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
				else if (!renderer.IsBusy() && !audio.IsSeBlocking() && !audio.IsVoiceBlocking())
				{
					bool waiting = false;
					do
					{
						switch (currentResult)
						{
						case ScenarioResult.Click:
							if (autoMode)
							{
								waitFramesLeft = (((bool)resultOption) ? waitFrames[0] : waitFrames[2]);
							}
							waitingForClick = true;
							waiting = true;
							goto default;
						case ScenarioResult.Scene:
							if (resultData.GetType().Equals(Color.Transparent.GetType()))
							{
								renderer.SetBackground((Color)resultData, (int)resultOption);
							}
							else
							{
								renderer.SetBackground((Texture)resultData, (int)resultOption);
							}
							goto default;
						case ScenarioResult.Bgm:
							if (resultOption.GetType().Equals(false.GetType()))
							{
								audio.SetBgmVolume((bool)resultOption);
							}
							else
							{
								audio.PlayBgm((string)resultData, (int)((double)resultOption * 1000.0));
							}
							goto default;
						case ScenarioResult.Character:
							if (((SpriteState)resultOption).action == SpriteMode.Static)
							{
								renderer.SetCharacter((Texture)resultData, ((SpriteState)resultOption).size, ((SpriteState)resultOption).index - 1, ((SpriteState)resultOption).location, ((SpriteState)resultOption).isTransparent, ((SpriteState)resultOption).isAnimated);
							}
							else if (((SpriteState)resultOption).action == SpriteMode.Shaking)
							{
								if (shakeEffect != null)
								{
									shakeEffect.Start(1);
									shockFramesLeft = shockFrames[((SpriteState)resultOption).level];
								}
								renderer.SetCharacterShake(((SpriteState)resultOption).index - 1, ((SpriteState)resultOption).level);
							}
							goto default;
						case ScenarioResult.Message:
							if (!skipMode)
							{
								audio.PlayVoice(((ScriptCommand)resultOption).voice);
							}
							renderer.SetMessage((string[])resultData, ((MessageCommand)resultOption).align, ((MessageCommand)resultOption).showMode, ((MessageCommand)resultOption).waitClick, ((MessageCommand)resultOption).foreColor, ((MessageCommand)resultOption).shadowColor, audio.GetVoiceLength());
							if (((MessageCommand)resultOption).instant)
							{
								renderer.instant();
							}
							goto default;
						case ScenarioResult.Caption:
							captionCommitPending = true;
							captionDisappear = ((CaptionCommand)resultOption).disappear;
							captionHideName = ((CaptionCommand)resultOption).hideCharaName;
							if (!skipMode)
							{
								audio.PlayVoice(((ScriptCommand)resultOption).voice);
							}
							renderer.SetCaption((string[])resultData, ((CaptionCommand)resultOption).charaNames, audio.GetVoiceLength(), ((CaptionCommand)resultOption).isContinuous, ((CaptionCommand)resultOption).instant || finishingAnimation || skipMode);
							if (((CaptionCommand)resultOption).instant)
							{
								renderer.instant();
							}
							goto default;
						case ScenarioResult.Sound:
							audio.PlaySe((string)resultData, ((InputState)resultOption).loop, ((InputState)resultOption).wait);
							goto default;
						case ScenarioResult.Jump:
							if ((int)resultOption > 0)
							{
								waitFramesLeft = waitFrames[(int)resultOption - 1];
							}
							goto default;
						case ScenarioResult.Dismiss:
							renderer.DismissMessage((bool)resultOption);
							goto default;
						case ScenarioResult.Notice:
							renderer.ShowNotice((string[])resultData);
							goto default;
						case ScenarioResult.Draw:
							switch (((DrawItem)resultOption).type)
							{
							case DrawItemType.Zoom:
								renderer.ZoomBackground(((AnimItem)resultOption).left, ((AnimItem)resultOption).right, ((AnimItem)resultOption).top, ((AnimItem)resultOption).bottom, ((AnimItem)resultOption).durationMode, ((AnimItem)resultOption).isAnimated, ((AnimItem)resultOption).finish);
								finishingAnimation = false;
								break;
							case DrawItemType.Dim:
								renderer.SetDimActive(((DrawItem)resultOption).isAnimated, ((DrawItem)resultOption).finish);
								finishingAnimation = false;
								break;
							case DrawItemType.Sun:
								renderer.SetSunActive(((DrawItem)resultOption).finish);
								break;
							case DrawItemType.Petals:
								renderer.SetPetalsActive(((DrawItem)resultOption).finish);
								break;
							case DrawItemType.Snow:
								renderer.SetSnowActive(((DrawItem)resultOption).finish);
								break;
							case DrawItemType.Think:
								renderer.SetThinkActive(((DrawItem)resultOption).isAnimated, ((DrawItem)resultOption).finish);
								finishingAnimation = false;
								break;
							case DrawItemType.Background:
								renderer.SetBackgroundWithWipe(((TextureItem)resultOption).texture, ((TextureItem)resultOption).blend, ((TextureItem)resultOption).isAnimated, ((TextureItem)resultOption).durationMode);
								finishingAnimation = false;
								break;
							case DrawItemType.Shake:
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
								renderer.SetShake(((FlagItem)resultOption).loop, ((FlagItem)resultOption).finish);
								break;
							}
							goto default;
						case ScenarioResult.StaffRoll:
							staffRoll = (StaffRoll)resultData;
							staffRoll.Initialize(graphicsDevice, spriteBatch, new FrameCallback(frameHandler), whiteTexture, new System.Drawing.Font(uiFont.FontFamily, 24f), buttonLayout);
							staffRollPlaying = true;
							goto default;
						default:
							if (currentResult == ScenarioResult.Click || currentResult == ScenarioResult.Scene || currentResult == ScenarioResult.Character || (currentResult == ScenarioResult.Sound && ((InputState)resultOption).wait) || currentResult == ScenarioResult.Notice || (currentResult == ScenarioResult.Draw && ((DrawItem)resultOption).isAnimated) || currentResult == ScenarioResult.StaffRoll || currentResult == ScenarioResult.Error)
							{
								finishingAnimation = false;
							}
							currentResult = scenario.Advance(out resultData, out resultOption, skipMode);
							if (waiting)
							{
								break;
							}
							if (currentResult != ScenarioResult.Click && currentResult != ScenarioResult.Scene && currentResult != ScenarioResult.Message && currentResult != ScenarioResult.Caption && currentResult != ScenarioResult.Jump && currentResult != ScenarioResult.Notice && currentResult != ScenarioResult.StaffRoll && currentResult != ScenarioResult.Error)
							{
								if (currentResult == ScenarioResult.Character)
								{
									waiting = !((SpriteState)resultOption).sync;
								}
								else if (currentResult == ScenarioResult.Sound)
								{
									waiting = ((InputState)resultOption).wait;
								}
								else if (currentResult == ScenarioResult.Dismiss)
								{
									waiting = (bool)resultOption;
								}
								else if (currentResult == ScenarioResult.Draw)
								{
									waiting = !((DrawItem)resultOption).sync;
								}
							}
							else
							{
								waiting = true;
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
						case ScenarioResult.Error:
							Stop();
							return false;
						}
					}
					while (!waiting);
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
			renderer.Render();
			return true;
		}

		public void Repaint()
		{
			if (!staffRollPlaying)
			{
				renderer.Repaint();
			}
			else
			{
				staffRoll.Update();
			}
		}

		public void Stop()
		{
			if (!staffRollPlaying)
			{
				audio.StopBgm();
				audio.StopVoice();
				audio.StopSe();
			}
			else
			{
				staffRoll.Stop();
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
				audio.StopVoice();
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
			renderer.HideDialogue(false);
			renderer.SetMessageSpeed(0);
		}

		public void instant(bool enabled, bool isHold)
		{
			if (staffRollPlaying)
			{
				return;
			}
			audio.StopVoice();
			if (enabled && !isHold)
			{
				renderer.SetMessageSpeed(1);
			}
			else
			{
				renderer.SetMessageSpeed(0);
			}
			if (skipMode == enabled)
			{
				return;
			}
			skipMode = enabled;
			autoMode = false;
			renderer.HideDialogue(false);
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

		public void SetAutoMode(bool enabled)
		{
			if (staffRollPlaying)
			{
				return;
			}
			if (enabled)
			{
				renderer.SetMessageSpeed(2);
			}
			else
			{
				renderer.SetMessageSpeed(0);
			}
			autoMode = enabled;
			skipMode = false;
			renderer.HideDialogue(false);
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

		public void SetAutoMode()
		{
			if (autoMode)
			{
				SetAutoMode(false);
			}
			else
			{
				SetAutoMode(true);
			}
		}

		public void HideDialogue(bool enabled)
		{
			if (!staffRollPlaying)
			{
				autoMode = false;
				skipMode = false;
				renderer.HideDialogue(enabled);
				renderer.SetMessageSpeed(0);
			}
		}

		public void HideDialogue()
		{
			if (!staffRollPlaying)
			{
				autoMode = false;
				skipMode = false;
				renderer.HideDialogue();
				renderer.SetMessageSpeed(0);
			}
		}

		public void SetFont(System.Drawing.Font font)
		{
			if (!staffRollPlaying)
			{
				renderer.SetFont(font);
				uiFont = font;
			}
		}

		public void SetButtonLayout(ButtonLayout layout)
		{
			if (staffRollPlaying)
			{
				staffRoll.SetButtonLayout(layout);
			}
			renderer.SetButtonLayout(layout);
			buttonLayout = layout;
		}

		public int GetEpisode()
		{
			return scenario.GetEpisode();
		}

		public int GetAlbumIndex()
		{
			return scenario.GetAlbumIndex();
		}

		public void SetPaused(bool enabled, bool includeBgm)
		{
			if (!staffRollPlaying)
			{
				audio.SetPaused(enabled, includeBgm);
			}
			else
			{
				staffRoll.SetPaused(enabled);
			}
		}

		public bool IsStaffRollPlaying()
		{
			return staffRollPlaying;
		}

		public void SetFadeEnabled(bool extensionName)
		{
			renderer.SetFadeEnabled(extensionName);
		}

		public void SetSunEffectEnabled(bool extensionName)
		{
			renderer.SetSunEffectEnabled(extensionName);
		}

		public void SetEffectEnabled(bool extensionName)
		{
			renderer.SetEffectEnabled(extensionName);
		}

		public void SetShockEffectEnabled(bool extensionName)
		{
			renderer.SetShockEffectEnabled(extensionName);
		}

		public void SetDialogueColor(Color extensionName)
		{
			renderer.SetDialogueColor(extensionName);
		}

		public void SetTextSpeed(int extensionName)
		{
			renderer.SetTextSpeed(extensionName);
		}

		public void SetSyncVoice(bool extensionName)
		{
			renderer.SetSyncVoice(extensionName);
		}
	}
}
