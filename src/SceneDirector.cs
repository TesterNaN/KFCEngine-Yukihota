using System;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;

namespace Yukihota
{
	// Token: 0x0200002A RID: 42
	public class SceneDirector : IDisposable
	{
		// Token: 0x06001F53 RID: 8019 RVA: 0x00019630 File Offset: 0x00018630
		public SceneDirector(string scriptPath, int episode, Microsoft.DirectX.Direct3D.Device device, Sprite sprite, FrameCallback frameCallback, Texture overlayTexture, EffectObject effectObject, System.Drawing.Font font, ButtonLayout layout, Color textColor)
		{
			this.scenario = new ScenarioScript(scriptPath, episode, device);
			this.audio = new AudioManager();
			this.renderer = new ScreenRenderer(device, sprite, frameCallback, overlayTexture, font, layout, textColor, this.audio);
			this.renderer.SetFont(font);
			this.renderer.SetButtonLayout(layout);
			this.graphicsDevice = device;
			this.spriteBatch = sprite;
			this.frameHandler = frameCallback;
			this.whiteTexture = overlayTexture;
			this.uiFont = font;
			this.buttonLayout = layout;
			this.shakeEffect = effectObject;
			this.waitingForClick = false;
			this.skipMode = false;
			this.autoMode = false;
			this.finishingAnimation = false;
			this.waitFramesLeft = 0;
			this.shockFramesLeft = 0;
			this.captionCommitPending = false;
			this.captionDisappear = false;
			this.captionHideName = false;
			this.staffRoll = null;
			this.staffRollPlaying = false;
			this.currentResult = this.scenario.Advance(out this.resultData, out this.resultOption, false);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x00019780 File Offset: 0x00018780
		public void Dispose()
		{
			this.audio.Dispose();
			this.scenario.Dispose();
			this.renderer.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x000197AC File Offset: 0x000187AC
		public bool Update()
		{
			if (this.staffRollPlaying)
			{
				if (!this.staffRoll.Update())
				{
					this.staffRoll.Dispose();
					this.staffRoll = null;
					this.staffRollPlaying = false;
				}
				return true;
			}
			if ((this.skipMode && this.waitingForClick) || (this.autoMode && this.waitingForClick && this.waitFramesLeft == 0))
			{
				this.waitingForClick = false;
				if (this.captionCommitPending)
				{
					this.renderer.CloseDialogue(this.captionDisappear, this.captionHideName, this.skipMode);
					this.captionCommitPending = false;
					this.captionDisappear = false;
					this.captionHideName = false;
				}
			}
			if (this.autoMode || !this.waitingForClick)
			{
				if (this.shockFramesLeft > 0)
				{
					this.shockFramesLeft--;
					if (this.shockFramesLeft == 0)
					{
						this.shakeEffect.Stop();
					}
				}
				if (this.waitFramesLeft > 0)
				{
					this.waitFramesLeft--;
				}
				else if (!this.renderer.IsBusy() && !this.audio.IsSeBlocking() && !this.audio.IsVoiceBlocking())
				{
					bool flag = false;
					for (;;)
					{
						switch (this.currentResult)
						{
						case ScenarioResult.Click:
							if (this.autoMode)
							{
								this.waitFramesLeft = (((bool)this.resultOption) ? this.waitFrames[0] : this.waitFrames[2]);
							}
							this.waitingForClick = true;
							flag = true;
							break;
						case ScenarioResult.Scene:
							if (this.resultData.GetType().Equals(Color.Transparent.GetType()))
							{
								this.renderer.SetBackground((Color)this.resultData, (int)this.resultOption);
							}
							else
							{
								this.renderer.SetBackground((Texture)this.resultData, (int)this.resultOption);
							}
							break;
						case ScenarioResult.Bgm:
							if (this.resultOption.GetType().Equals(false.GetType()))
							{
								this.audio.SetBgmVolume((bool)this.resultOption);
							}
							else
							{
								this.audio.PlayBgm((string)this.resultData, (int)((double)this.resultOption * 1000.0));
							}
							break;
						case ScenarioResult.Character:
							if (((SpriteState)this.resultOption).action == SpriteMode.Static)
							{
								this.renderer.SetCharacter((Texture)this.resultData, ((SpriteState)this.resultOption).size, ((SpriteState)this.resultOption).index - 1, ((SpriteState)this.resultOption).location, ((SpriteState)this.resultOption).isTransparent, ((SpriteState)this.resultOption).isAnimated);
							}
							else if (((SpriteState)this.resultOption).action == SpriteMode.Shaking)
							{
								if (this.shakeEffect != null)
								{
									this.shakeEffect.Start(1);
									this.shockFramesLeft = this.shockFrames[((SpriteState)this.resultOption).level];
								}
								this.renderer.SetCharacterShake(((SpriteState)this.resultOption).index - 1, ((SpriteState)this.resultOption).level);
							}
							break;
						case ScenarioResult.Message:
							if (!this.skipMode)
							{
								this.audio.PlayVoice(((ScriptCommand)this.resultOption).voice);
							}
							this.renderer.SetMessage((string[])this.resultData, ((MessageCommand)this.resultOption).align, ((MessageCommand)this.resultOption).showMode, ((MessageCommand)this.resultOption).waitClick, ((MessageCommand)this.resultOption).foreColor, ((MessageCommand)this.resultOption).shadowColor, this.audio.GetVoiceLength());
							if (((MessageCommand)this.resultOption).instant)
							{
								this.renderer.FastForward();
							}
							break;
						case ScenarioResult.Caption:
							this.captionCommitPending = true;
							this.captionDisappear = ((CaptionCommand)this.resultOption).disappear;
							this.captionHideName = ((CaptionCommand)this.resultOption).hideCharaName;
							if (!this.skipMode)
							{
								this.audio.PlayVoice(((ScriptCommand)this.resultOption).voice);
							}
							this.renderer.SetCaption((string[])this.resultData, ((CaptionCommand)this.resultOption).charaNames, this.audio.GetVoiceLength(), ((CaptionCommand)this.resultOption).isContinuous, ((CaptionCommand)this.resultOption).instant || this.finishingAnimation || this.skipMode);
							if (((CaptionCommand)this.resultOption).instant)
							{
								this.renderer.FastForward();
							}
							break;
						case ScenarioResult.Sound:
							this.audio.PlaySe((string)this.resultData, ((InputState)this.resultOption).loop, ((InputState)this.resultOption).wait);
							break;
						case ScenarioResult.Jump:
							if ((int)this.resultOption > 0)
							{
								this.waitFramesLeft = this.waitFrames[(int)this.resultOption - 1];
							}
							break;
						case ScenarioResult.Dismiss:
							this.renderer.DismissMessage((bool)this.resultOption);
							break;
						case ScenarioResult.Notice:
							this.renderer.ShowNotice((string[])this.resultData);
							break;
						case ScenarioResult.Draw:
							switch (((DrawItem)this.resultOption).type)
							{
							case DrawItemType.Zoom:
								this.renderer.ZoomBackground(((AnimItem)this.resultOption).left, ((AnimItem)this.resultOption).right, ((AnimItem)this.resultOption).top, ((AnimItem)this.resultOption).bottom, ((AnimItem)this.resultOption).durationMode, ((AnimItem)this.resultOption).isAnimated, ((AnimItem)this.resultOption).finish);
								this.finishingAnimation = false;
								break;
							case DrawItemType.Dim:
								this.renderer.SetDimActive(((DrawItem)this.resultOption).isAnimated, ((DrawItem)this.resultOption).finish);
								this.finishingAnimation = false;
								break;
							case DrawItemType.Sun:
								this.renderer.SetSunActive(((DrawItem)this.resultOption).finish);
								break;
							case DrawItemType.Petals:
								this.renderer.SetPetalsActive(((DrawItem)this.resultOption).finish);
								break;
							case DrawItemType.Snow:
								this.renderer.SetSnowActive(((DrawItem)this.resultOption).finish);
								break;
							case DrawItemType.Think:
								this.renderer.SetThinkActive(((DrawItem)this.resultOption).isAnimated, ((DrawItem)this.resultOption).finish);
								this.finishingAnimation = false;
								break;
							case DrawItemType.Background:
								this.renderer.SetBackgroundWithWipe(((TextureItem)this.resultOption).texture, ((TextureItem)this.resultOption).blend, ((TextureItem)this.resultOption).isAnimated, ((TextureItem)this.resultOption).durationMode);
								this.finishingAnimation = false;
								break;
							case DrawItemType.Shake:
								if (this.shakeEffect != null)
								{
									this.shakeEffect.Start(1);
									if (!((FlagItem)this.resultOption).loop)
									{
										this.shockFramesLeft = this.shockFrames[2];
									}
									else if (!((FlagItem)this.resultOption).finish)
									{
										this.shockFramesLeft = int.MaxValue;
									}
									else
									{
										this.shockFramesLeft = 0;
									}
								}
								this.renderer.SetShake(((FlagItem)this.resultOption).loop, ((FlagItem)this.resultOption).finish);
								break;
							}
							break;
						case ScenarioResult.StaffRoll:
							this.staffRoll = (StaffRoll)this.resultData;
							this.staffRoll.Initialize(this.graphicsDevice, this.spriteBatch, this.frameHandler, this.whiteTexture, new System.Drawing.Font(this.uiFont.FontFamily, 24f), this.buttonLayout);
							this.staffRollPlaying = true;
							break;
						case ScenarioResult.Error:
							goto IL_022A;
						}
						if (this.currentResult == ScenarioResult.Click || this.currentResult == ScenarioResult.Scene || this.currentResult == ScenarioResult.Character || (this.currentResult == ScenarioResult.Sound && ((InputState)this.resultOption).wait) || this.currentResult == ScenarioResult.Notice || (this.currentResult == ScenarioResult.Draw && ((DrawItem)this.resultOption).finish) || this.currentResult == ScenarioResult.StaffRoll || this.currentResult == ScenarioResult.Error)
						{
							this.finishingAnimation = false;
						}
						this.currentResult = this.scenario.Advance(out this.resultData, out this.resultOption, this.skipMode);
						if (!flag)
						{
							if (this.currentResult == ScenarioResult.Click || this.currentResult == ScenarioResult.Scene || this.currentResult == ScenarioResult.Message || this.currentResult == ScenarioResult.Caption || this.currentResult == ScenarioResult.Jump || this.currentResult == ScenarioResult.Notice || this.currentResult == ScenarioResult.StaffRoll)
							{
								goto IL_0122;
							}
							if (this.currentResult == ScenarioResult.Error)
							{
								goto IL_0122;
							}
							if (this.currentResult == ScenarioResult.Character)
							{
								flag = !((SpriteState)this.resultOption).sync;
							}
							else if (this.currentResult == ScenarioResult.Sound)
							{
								flag = ((InputState)this.resultOption).wait;
							}
							else if (this.currentResult == ScenarioResult.Dismiss)
							{
								flag = (bool)this.resultOption;
							}
							else if (this.currentResult == ScenarioResult.Draw)
							{
								flag = !((DrawItem)this.resultOption).sync;
							}
							IL_005E:
							if (!this.finishingAnimation && !this.skipMode)
							{
								goto IL_0078;
							}
							this.renderer.FastForward();
							this.audio.ReleaseAudioWait();
							if (this.waitFramesLeft > 0)
							{
								this.waitFramesLeft = 0;
							}
							if (this.shockFramesLeft > 0)
							{
								this.shockFramesLeft = 1;
								goto IL_0078;
							}
							goto IL_0078;
							IL_0122:
							flag = true;
							goto IL_005E;
						}
						IL_0078:
						if (flag)
						{
							goto IL_007E;
						}
					}
					IL_022A:
					this.Stop();
					return false;
				}
			}
			IL_007E:
			if (this.finishingAnimation || this.skipMode)
			{
				this.renderer.FastForward();
				this.audio.ReleaseAudioWait();
				if (this.waitFramesLeft > 0)
				{
					this.waitFramesLeft = 0;
				}
				if (this.shockFramesLeft > 0)
				{
					this.shockFramesLeft = 1;
				}
			}
			this.renderer.Render();
			return true;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x0001A3E8 File Offset: 0x000193E8
		public void RenderFrame()
		{
			if (!this.staffRollPlaying)
			{
				this.renderer.Repaint();
				return;
			}
			this.staffRoll.Update();
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x0001A40C File Offset: 0x0001940C
		public void Stop()
		{
			if (!this.staffRollPlaying)
			{
				this.audio.StopBgm();
				this.audio.StopVoice();
				this.audio.StopSe();
				return;
			}
			this.staffRoll.Stop();
			this.staffRoll.Dispose();
			this.staffRoll = null;
			this.staffRollPlaying = false;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x0001A468 File Offset: 0x00019468
		public void OnClick()
		{
			if (this.staffRollPlaying)
			{
				return;
			}
			if (this.waitingForClick)
			{
				this.waitingForClick = false;
				this.audio.StopVoice();
				if (this.captionCommitPending)
				{
					this.renderer.CloseDialogue(this.captionDisappear, this.captionHideName, false);
					this.captionCommitPending = false;
					this.captionDisappear = false;
					this.captionHideName = false;
				}
			}
			else
			{
				this.renderer.FastForward();
				this.audio.ReleaseAudioWait();
				if (this.waitFramesLeft > 0)
				{
					this.waitFramesLeft = 0;
				}
				if (this.shockFramesLeft > 0)
				{
					this.shockFramesLeft = 1;
				}
				this.finishingAnimation = true;
			}
			this.skipMode = false;
			this.autoMode = false;
			this.renderer.HideDialogue(false);
			this.renderer.SetModeIcon(0);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x0001A53C File Offset: 0x0001953C
		public void SetSkipMode(bool enabled, bool isHold)
		{
			if (this.staffRollPlaying)
			{
				return;
			}
			this.audio.StopVoice();
			if (enabled && !isHold)
			{
				this.renderer.SetModeIcon(1);
			}
			else
			{
				this.renderer.SetModeIcon(0);
			}
			if (this.skipMode == enabled)
			{
				return;
			}
			this.skipMode = enabled;
			this.autoMode = false;
			this.renderer.HideDialogue(false);
			if (this.skipMode)
			{
				if (this.waitingForClick && this.captionCommitPending)
				{
					this.renderer.CloseDialogue(this.captionDisappear, this.captionHideName, true);
					this.captionCommitPending = false;
					this.captionDisappear = false;
					this.captionHideName = false;
				}
				this.renderer.FastForward();
				this.audio.ReleaseAudioWait();
				if (this.waitFramesLeft > 0)
				{
					this.waitFramesLeft = 0;
				}
				if (this.shockFramesLeft > 0)
				{
					this.shockFramesLeft = 1;
				}
				this.waitingForClick = false;
			}
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x0001A644 File Offset: 0x00019644
		public void SetSkipMode()
		{
			if (this.skipMode)
			{
				this.SetSkipMode(false, false);
				return;
			}
			this.SetSkipMode(true, false);
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x0001A660 File Offset: 0x00019660
		public void SetAutoMode(bool enabled)
		{
			if (this.staffRollPlaying)
			{
				return;
			}
			if (enabled)
			{
				this.renderer.SetModeIcon(2);
			}
			else
			{
				this.renderer.SetModeIcon(0);
			}
			this.autoMode = enabled;
			this.skipMode = false;
			this.renderer.HideDialogue(false);
			if (this.autoMode && this.waitingForClick)
			{
				this.waitingForClick = false;
				if (this.captionCommitPending)
				{
					this.renderer.CloseDialogue(this.captionDisappear, this.captionHideName, false);
					this.captionCommitPending = false;
					this.captionDisappear = false;
					this.captionHideName = false;
				}
			}
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x0001A708 File Offset: 0x00019708
		public void SetAutoMode()
		{
			if (this.autoMode)
			{
				this.SetAutoMode(false);
				return;
			}
			this.SetAutoMode(true);
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x0001A724 File Offset: 0x00019724
		public void ToggleMessageWindow(bool enabled)
		{
			if (this.staffRollPlaying)
			{
				return;
			}
			this.autoMode = false;
			this.skipMode = false;
			this.renderer.HideDialogue(enabled);
			this.renderer.SetModeIcon(0);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x0001A758 File Offset: 0x00019758
		public void ToggleMessageWindow()
		{
			if (this.staffRollPlaying)
			{
				return;
			}
			this.autoMode = false;
			this.skipMode = false;
			this.renderer.HideDialogue();
			this.renderer.SetModeIcon(0);
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x0001A788 File Offset: 0x00019788
		public void SetFont(System.Drawing.Font font)
		{
			if (this.staffRollPlaying)
			{
				return;
			}
			this.renderer.SetFont(font);
			this.uiFont = font;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x0001A7A8 File Offset: 0x000197A8
		public void SetButtonLayout(ButtonLayout layout)
		{
			if (this.staffRollPlaying)
			{
				this.staffRoll.SetButtonLayout(layout);
			}
			this.renderer.SetButtonLayout(layout);
			this.buttonLayout = layout;
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x0001A7D4 File Offset: 0x000197D4
		public int GetEpisode()
		{
			return this.scenario.GetEpisode();
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x0001A7E4 File Offset: 0x000197E4
		public int GetAlbumIndex()
		{
			return this.scenario.GetAlbumIndex();
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x0001A7F4 File Offset: 0x000197F4
		public void SetPaused(bool enabled, bool includeBgm)
		{
			if (!this.staffRollPlaying)
			{
				this.audio.SetPaused(enabled, includeBgm);
				return;
			}
			this.staffRoll.SetPaused(enabled);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x0001A818 File Offset: 0x00019818
		public bool IsStaffRollPlaying()
		{
			return this.staffRollPlaying;
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x0001A820 File Offset: 0x00019820
		public void SetFadeEnabled(bool newValue)
		{
			this.renderer.SetFadeEnabled(newValue);
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x0001A830 File Offset: 0x00019830
		public void SetSunEffectEnabled(bool newValue)
		{
			this.renderer.SetSunEffectEnabled(newValue);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x0001A840 File Offset: 0x00019840
		public void SetEnvironmentEffectEnabled(bool newValue)
		{
			this.renderer.SetEffectEnabled(newValue);
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x0001A850 File Offset: 0x00019850
		public void SetShockEffectEnabled(bool newValue)
		{
			this.renderer.SetShockEffectEnabled(newValue);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x0001A860 File Offset: 0x00019860
		public void SetTextColor(Color newValue)
		{
			this.renderer.SetDialogueColor(newValue);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x0001A870 File Offset: 0x00019870
		public void SetTextSpeed(int newValue)
		{
			this.renderer.SetMessageSpeed(newValue);
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x0001A880 File Offset: 0x00019880
		public void SetSynchroEnabled(bool newValue)
		{
			this.renderer.SetSyncVoice(newValue);
		}

		// Token: 0x0400011F RID: 287
		public const int ScreenWidth = 800;

		// Token: 0x04000120 RID: 288
		public const int ScreenHeight = 600;

		// Token: 0x04000121 RID: 289
		protected ScenarioScript scenario;

		// Token: 0x04000122 RID: 290
		protected ScreenRenderer renderer;

		// Token: 0x04000123 RID: 291
		protected AudioManager audio;

		// Token: 0x04000124 RID: 292
		protected EffectObject shakeEffect;

		// Token: 0x04000125 RID: 293
		protected ScenarioResult currentResult;

		// Token: 0x04000126 RID: 294
		protected object resultData;

		// Token: 0x04000127 RID: 295
		protected object resultOption;

		// Token: 0x04000128 RID: 296
		protected bool waitingForClick;

		// Token: 0x04000129 RID: 297
		protected bool skipMode;

		// Token: 0x0400012A RID: 298
		protected bool autoMode;

		// Token: 0x0400012B RID: 299
		protected bool finishingAnimation;

		// Token: 0x0400012C RID: 300
		protected readonly int[] waitFrames = new int[] { 10, 20, 40 };

		// Token: 0x0400012D RID: 301
		protected readonly int[] shockFrames = new int[] { 4, 6, 16 };

		// Token: 0x0400012E RID: 302
		private int waitFramesLeft;

		// Token: 0x0400012F RID: 303
		private int shockFramesLeft;

		// Token: 0x04000130 RID: 304
		private bool captionCommitPending;

		// Token: 0x04000131 RID: 305
		private bool captionDisappear;

		// Token: 0x04000132 RID: 306
		private bool captionHideName;

		// Token: 0x04000133 RID: 307
		private bool staffRollPlaying;

		// Token: 0x04000134 RID: 308
		private StaffRoll staffRoll;

		// Token: 0x04000135 RID: 309
		private Microsoft.DirectX.Direct3D.Device graphicsDevice;

		// Token: 0x04000136 RID: 310
		private Sprite spriteBatch;

		// Token: 0x04000137 RID: 311
		private FrameCallback frameHandler;

		// Token: 0x04000138 RID: 312
		private Texture whiteTexture;

		// Token: 0x04000139 RID: 313
		private System.Drawing.Font uiFont;

		// Token: 0x0400013A RID: 314
		private ButtonLayout buttonLayout;
	}
}
