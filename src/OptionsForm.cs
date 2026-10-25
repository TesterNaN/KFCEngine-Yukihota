using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

// 说明：窗体资源不使用 ResourceManager 的 resx 编译产物（.NET 1.1 不认 ResGen 4.x 的 v2 格式），
// 而是直接嵌入原版 exe 中的 v1 格式 .resources（见 resources\res1.bin、res2.bin）。
namespace Yukihota
{
	// Token: 0x02000018 RID: 24
	public class OptionsForm : Form
	{
		// Token: 0x06001F0A RID: 7946 RVA: 0x00013180 File Offset: 0x00012180
		public OptionsForm()
		{
			this.InitializeComponent();
			foreach (object obj in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly))
			{
				DeviceInstance deviceInstance = (DeviceInstance)obj;
				this.cmbDevice.Items.Add(new OptionsForm.InputDeviceInfo(deviceInstance));
			}
			if (this.cmbDevice.Items.Count > 0)
			{
				this.cmbDevice.SelectedIndex = 0;
				OptionsForm.InputDeviceInfo InputDeviceInfo = (OptionsForm.InputDeviceInfo)this.cmbDevice.SelectedItem;
				this.device = new Device(InputDeviceInfo.GetInstanceGuid());
				Control u085A = this.chkVibration;
				DeviceCaps deviceCaps = this.device.Caps;
				u085A.Enabled = deviceCaps.ForceFeedback;
				CheckBox u085A2 = this.chkVibration;
				deviceCaps = this.device.Caps;
				u085A2.Checked = deviceCaps.ForceFeedback;
				deviceCaps = this.device.Caps;
				this.forceFeedback = deviceCaps.ForceFeedback;
				return;
			}
			this.chkVibration.Enabled = false;
			this.chkVibration.Checked = false;
			this.forceFeedback = false;
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x000132F4 File Offset: 0x000122F4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x00013314 File Offset: 0x00012314
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager(typeof(OptionsForm));
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.picLogo = new PictureBox();
			this.lblGamepad = new Label();
			this.cmbDevice = new ComboBox();
			this.btnFont = new System.Windows.Forms.Button();
			this.fontDialog = new FontDialog();
			this.grpDisplay = new GroupBox();
			this.rdoFullscreen = new RadioButton();
			this.rdoWindowed = new RadioButton();
			this.grpLanguage = new GroupBox();
			this.rdoSimplified = new RadioButton();
			this.rdoTraditional = new RadioButton();
			this.rdoJapanese = new RadioButton();
			this.chkDontShow = new CheckBox();
			this.lnkManual = new LinkLabel();
			this.chkVibration = new CheckBox();
			this.grpDisplay.SuspendLayout();
			this.grpLanguage.SuspendLayout();
			base.SuspendLayout();
			this.btnOk.DialogResult = DialogResult.OK;
			this.btnOk.Location = new Point(32, 280);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(96, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "确定(&O)";
			this.btnOk.Click += new EventHandler(this.OnOkClick);
			this.btnCancel.DialogResult = DialogResult.Cancel;
			this.btnCancel.Location = new Point(184, 280);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(96, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "取消(&C)";
			this.picLogo.Image = (Image)resourceManager.GetObject("picTitle.Image");
			this.picLogo.Location = new Point(64, 8);
			this.picLogo.Name = "picTitle";
			this.picLogo.Size = new Size(160, 50);
			this.picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
			this.picLogo.TabIndex = 4;
			this.picLogo.TabStop = false;
			this.lblGamepad.BackColor = Color.Transparent;
			this.lblGamepad.Location = new Point(16, 64);
			this.lblGamepad.Name = "lblGamepad";
			this.lblGamepad.Size = new Size(144, 16);
			this.lblGamepad.TabIndex = 2;
			this.lblGamepad.Text = "手柄选择(&G)";
			this.cmbDevice.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cmbDevice.Location = new Point(16, 80);
			this.cmbDevice.Name = "cbbGamepad";
			this.cmbDevice.Size = new Size(288, 20);
			this.cmbDevice.TabIndex = 3;
			this.cmbDevice.SelectedIndexChanged += new EventHandler(this.OnDeviceSelectedIndexChanged);
			this.btnFont.Location = new Point(104, 112);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new Size(104, 23);
			this.btnFont.TabIndex = 5;
			this.btnFont.Text = "字体(&F)";
			this.btnFont.Click += new EventHandler(this.OnFontClick);
			this.fontDialog.AllowVerticalFonts = false;
			this.fontDialog.Font = new Font("黑体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			this.fontDialog.FontMustExist = true;
			this.fontDialog.MaxSize = 24;
			this.fontDialog.MinSize = 24;
			this.fontDialog.ShowEffects = false;
			this.grpDisplay.BackColor = Color.Transparent;
			this.grpDisplay.Controls.Add(this.rdoFullscreen);
			this.grpDisplay.Controls.Add(this.rdoWindowed);
			this.grpDisplay.Location = new Point(8, 144);
			this.grpDisplay.Name = "gpbMode";
			this.grpDisplay.Size = new Size(144, 80);
			this.grpDisplay.TabIndex = 6;
			this.grpDisplay.TabStop = false;
			this.grpDisplay.Text = "显示模式(&M)";
			this.rdoFullscreen.BackColor = Color.Transparent;
			this.rdoFullscreen.Checked = true;
			this.rdoFullscreen.Location = new Point(8, 24);
			this.rdoFullscreen.Name = "rbtFullScreen";
			this.rdoFullscreen.Size = new Size(136, 24);
			this.rdoFullscreen.TabIndex = 0;
			this.rdoFullscreen.TabStop = true;
			this.rdoFullscreen.Text = "全屏幕(&F)";
			this.rdoWindowed.BackColor = Color.Transparent;
			this.rdoWindowed.Location = new Point(8, 48);
			this.rdoWindowed.Name = "rbtWindowed";
			this.rdoWindowed.Size = new Size(136, 24);
			this.rdoWindowed.TabIndex = 1;
			this.rdoWindowed.Text = "窗口模式(&W)";
			this.grpLanguage.BackColor = Color.Transparent;
			this.grpLanguage.Controls.Add(this.rdoSimplified);
			this.grpLanguage.Controls.Add(this.rdoTraditional);
			this.grpLanguage.Controls.Add(this.rdoJapanese);
			this.grpLanguage.Location = new Point(160, 144);
			this.grpLanguage.Name = "gpbLanguage";
			this.grpLanguage.Size = new Size(144, 104);
			this.grpLanguage.TabIndex = 7;
			this.grpLanguage.TabStop = false;
			this.grpLanguage.Text = "语言(&L)";
			this.rdoSimplified.BackColor = Color.Transparent;
			this.rdoSimplified.Checked = true;
			this.rdoSimplified.Location = new Point(8, 24);
			this.rdoSimplified.Name = "rbtSC";
			this.rdoSimplified.Size = new Size(136, 24);
			this.rdoSimplified.TabIndex = 0;
			this.rdoSimplified.TabStop = true;
			this.rdoSimplified.Text = "简体中文(&S)";
			this.rdoSimplified.CheckedChanged += new EventHandler(this.OnSimplifiedCheckedChanged);
			this.rdoTraditional.BackColor = Color.Transparent;
			this.rdoTraditional.Location = new Point(8, 48);
			this.rdoTraditional.Name = "rbtTC";
			this.rdoTraditional.Size = new Size(136, 24);
			this.rdoTraditional.TabIndex = 1;
			this.rdoTraditional.Text = "繁體中文(&T)";
			this.rdoTraditional.CheckedChanged += new EventHandler(this.OnTraditionalCheckedChanged);
			this.rdoJapanese.BackColor = Color.Transparent;
			this.rdoJapanese.Enabled = false;
			this.rdoJapanese.Location = new Point(8, 72);
			this.rdoJapanese.Name = "rbtJP";
			this.rdoJapanese.Size = new Size(136, 24);
			this.rdoJapanese.TabIndex = 2;
			this.rdoJapanese.Text = "日本語(&J)";
			this.rdoJapanese.CheckedChanged += new EventHandler(this.OnJapaneseCheckedChanged);
			this.chkDontShow.BackColor = Color.Transparent;
			this.chkDontShow.Location = new Point(8, 232);
			this.chkDontShow.Name = "ckbDontShow";
			this.chkDontShow.Size = new Size(152, 24);
			this.chkDontShow.TabIndex = 8;
			this.chkDontShow.Text = "不再显示(&S)";
			this.lnkManual.BackColor = Color.Transparent;
			this.lnkManual.Location = new Point(112, 256);
			this.lnkManual.Name = "llbManual";
			this.lnkManual.Size = new Size(96, 16);
			this.lnkManual.TabIndex = 9;
			this.lnkManual.TabStop = true;
			this.lnkManual.Text = "手册(&M)";
			this.lnkManual.TextAlign = ContentAlignment.MiddleCenter;
			this.lnkManual.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnManualLinkClicked);
			this.chkVibration.BackColor = Color.Transparent;
			this.chkVibration.Location = new Point(208, 64);
			this.chkVibration.Name = "ckbEffect";
			this.chkVibration.Size = new Size(88, 16);
			this.chkVibration.TabIndex = 4;
			this.chkVibration.Text = "振动(&E)";
			this.AutoScaleBaseSize = new Size(6, 14);
			base.ClientSize = new Size(314, 312);
			base.Controls.Add(this.picLogo);
			base.Controls.Add(this.lblGamepad);
			base.Controls.Add(this.cmbDevice);
			base.Controls.Add(this.chkVibration);
			base.Controls.Add(this.btnFont);
			base.Controls.Add(this.grpDisplay);
			base.Controls.Add(this.grpLanguage);
			base.Controls.Add(this.chkDontShow);
			base.Controls.Add(this.lnkManual);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.btnCancel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)resourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Options";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "选项";
			base.Load += new EventHandler(this.OnFormLoad);
			this.grpDisplay.ResumeLayout(false);
			this.grpLanguage.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x00013ECC File Offset: 0x00012ECC
		private void OnFormLoad(object sender, EventArgs e)
		{
			this.fontDialog.Font = this.font;
			if (this.fullscreen)
			{
				this.rdoFullscreen.Checked = true;
			}
			else
			{
				this.rdoWindowed.Checked = true;
			}
			switch (this.buttonLayout)
			{
			case ButtonLayout.SimplifiedChinese:
				this.rdoSimplified.Checked = true;
				break;
			case ButtonLayout.TraditionalChinese:
				this.rdoTraditional.Checked = true;
				break;
			case ButtonLayout.Japanese:
				this.rdoJapanese.Checked = true;
				break;
			}
			this.ApplyButtonLayout(this.buttonLayout);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x00013F60 File Offset: 0x00012F60
		private void OnOkClick(object sender, EventArgs e)
		{
			if (this.cmbDevice.SelectedItem != null)
			{
				OptionsForm.InputDeviceInfo InputDeviceInfo = (OptionsForm.InputDeviceInfo)this.cmbDevice.SelectedItem;
				this.device = new Device(InputDeviceInfo.GetInstanceGuid());
			}
			else
			{
				this.device = null;
			}
			this.forceFeedback = this.chkVibration.Checked;
			this.fullscreen = this.rdoFullscreen.Checked;
			if (this.rdoJapanese.Checked)
			{
				this.buttonLayout = ButtonLayout.Japanese;
			}
			else if (this.rdoTraditional.Checked)
			{
				this.buttonLayout = ButtonLayout.TraditionalChinese;
			}
			else
			{
				this.buttonLayout = ButtonLayout.SimplifiedChinese;
			}
			this.showOptions = !this.chkDontShow.Checked;
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x00014020 File Offset: 0x00013020
		private void OnDeviceSelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ComboBox)sender).SelectedItem != null)
			{
				OptionsForm.InputDeviceInfo InputDeviceInfo = (OptionsForm.InputDeviceInfo)((ComboBox)sender).SelectedItem;
				Device device = new Device(InputDeviceInfo.GetInstanceGuid());
				this.chkVibration.Enabled = device.Caps.ForceFeedback;
				this.chkVibration.Checked = device.Caps.ForceFeedback;
				return;
			}
			this.chkVibration.Enabled = false;
			this.chkVibration.Checked = false;
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x000140B0 File Offset: 0x000130B0
		private void OnFontClick(object sender, EventArgs e)
		{
			if (this.fontDialog.ShowDialog() == DialogResult.OK)
			{
				this.font = this.fontDialog.Font;
			}
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x000140D4 File Offset: 0x000130D4
		private void OnSimplifiedCheckedChanged(object sender, EventArgs e)
		{
			this.ApplyButtonLayout(ButtonLayout.SimplifiedChinese);
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x000140E0 File Offset: 0x000130E0
		private void OnTraditionalCheckedChanged(object sender, EventArgs e)
		{
			this.ApplyButtonLayout(ButtonLayout.TraditionalChinese);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x000140EC File Offset: 0x000130EC
		private void OnJapaneseCheckedChanged(object sender, EventArgs e)
		{
			this.ApplyButtonLayout(ButtonLayout.Japanese);
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x000140F8 File Offset: 0x000130F8
		private void OnManualLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				if (this.rdoJapanese.Checked)
				{
					Process.Start("Manual\\Index_jp.html");
				}
				else if (this.rdoTraditional.Checked)
				{
					Process.Start("Manual\\Index_tc.html");
				}
				else
				{
					Process.Start("Manual\\Index_sc.html");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x00014174 File Offset: 0x00013174
		private void ApplyButtonLayout(ButtonLayout layout)
		{
			switch (layout)
			{
			case ButtonLayout.SimplifiedChinese:
				this.btnOk.Text = "确定(&O)";
				this.btnCancel.Text = "取消(&C)";
				this.btnFont.Text = "字体(&F)";
				this.lblGamepad.Text = "手柄选择(&G)";
				this.chkVibration.Text = "振动(&E)";
				this.grpDisplay.Text = "显示模式(&M)";
				this.rdoWindowed.Text = "窗口模式(&W)";
				this.rdoFullscreen.Text = "全屏幕(&F)";
				this.grpLanguage.Text = "语言(&L)";
				this.chkDontShow.Text = "不再显示(&S)";
				this.lnkManual.Text = "手册(&M)";
				this.Text = "选项";
				return;
			case ButtonLayout.TraditionalChinese:
				this.btnOk.Text = "確定(&O)";
				this.btnCancel.Text = "取消(&C)";
				this.btnFont.Text = "字體(&F)";
				this.lblGamepad.Text = "手柄選擇(&G)";
				this.chkVibration.Text = "振動(&E)";
				this.grpDisplay.Text = "顯示模式(&M)";
				this.rdoWindowed.Text = "窗口模式(&W)";
				this.rdoFullscreen.Text = "全屏幕(&F)";
				this.grpLanguage.Text = "語言(&L)";
				this.chkDontShow.Text = "不再顯示(&S)";
				this.lnkManual.Text = "手冊(&M)";
				this.Text = "選項";
				return;
			case ButtonLayout.Japanese:
				this.btnOk.Text = "開始(&O)";
				this.btnCancel.Text = "キャンセル(&C)";
				this.btnFont.Text = "フォント(&F)";
				this.lblGamepad.Text = "コントローラ選択(&G)";
				this.chkVibration.Text = "振動(&E)";
				this.grpDisplay.Text = "画面モード(&M)";
				this.rdoWindowed.Text = "窓モード(&W)";
				this.rdoFullscreen.Text = "フルスクリーン(&F)";
				this.grpLanguage.Text = "言語(&L)";
				this.chkDontShow.Text = "以降は提示しない(&S)";
				this.lnkManual.Text = "マニュアル(&M)";
				this.Text = "メニュー";
				return;
			default:
				return;
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x000143EC File Offset: 0x000133EC
		public Font GetFontName()
		{
			return this.font;
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000143F4 File Offset: 0x000133F4
		public void SetFontName(Font value)
		{
			this.font = value;
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x00014400 File Offset: 0x00013400
		public Device GetDevice()
		{
			return this.device;
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x00014408 File Offset: 0x00013408
		public void SetDevice(Device value)
		{
			this.device = value;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00014414 File Offset: 0x00013414
		public bool GetForceFeedback()
		{
			return this.forceFeedback;
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0001441C File Offset: 0x0001341C
		public void SetForceFeedback(bool value)
		{
			this.forceFeedback = value;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x00014428 File Offset: 0x00013428
		public bool GetFullscreen()
		{
			return this.fullscreen;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x00014430 File Offset: 0x00013430
		public void SetFullscreen(bool value)
		{
			this.fullscreen = value;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0001443C File Offset: 0x0001343C
		public ButtonLayout GetButtonLayout()
		{
			return this.buttonLayout;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00014444 File Offset: 0x00013444
		public void SetButtonLayout(ButtonLayout value)
		{
			this.buttonLayout = value;
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00014450 File Offset: 0x00013450
		public bool GetShowOptions()
		{
			return this.showOptions;
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x00014458 File Offset: 0x00013458
		public void SetShowOptions(bool value)
		{
			this.showOptions = value;
		}

		// Token: 0x040000BD RID: 189
		private Font font;

		// Token: 0x040000BE RID: 190
		private Device device;

		// Token: 0x040000BF RID: 191
		private bool forceFeedback;

		// Token: 0x040000C0 RID: 192
		private bool fullscreen;

		// Token: 0x040000C1 RID: 193
		private ButtonLayout buttonLayout;

		// Token: 0x040000C2 RID: 194
		private bool showOptions;

		// Token: 0x040000C3 RID: 195
		private Container components = null;

		// Token: 0x040000C4 RID: 196
		private System.Windows.Forms.Button btnOk;

		// Token: 0x040000C5 RID: 197
		private System.Windows.Forms.Button btnCancel;

		// Token: 0x040000C6 RID: 198
		private PictureBox picLogo;

		// Token: 0x040000C7 RID: 199
		private Label lblGamepad;

		// Token: 0x040000C8 RID: 200
		private ComboBox cmbDevice;

		// Token: 0x040000C9 RID: 201
		private System.Windows.Forms.Button btnFont;

		// Token: 0x040000CA RID: 202
		private FontDialog fontDialog;

		// Token: 0x040000CB RID: 203
		private GroupBox grpDisplay;

		// Token: 0x040000CC RID: 204
		private RadioButton rdoFullscreen;

		// Token: 0x040000CD RID: 205
		private RadioButton rdoWindowed;

		// Token: 0x040000CE RID: 206
		private GroupBox grpLanguage;

		// Token: 0x040000CF RID: 207
		private RadioButton rdoSimplified;

		// Token: 0x040000D0 RID: 208
		private RadioButton rdoTraditional;

		// Token: 0x040000D1 RID: 209
		private RadioButton rdoJapanese;

		// Token: 0x040000D2 RID: 210
		private LinkLabel lnkManual;

		// Token: 0x040000D3 RID: 211
		private CheckBox chkVibration;

		// Token: 0x040000D4 RID: 212
		private CheckBox chkDontShow;

		// Token: 0x02000028 RID: 40
		private struct InputDeviceInfo
		{
			// Token: 0x06001F43 RID: 8003 RVA: 0x00018E30 File Offset: 0x00017E30
			public InputDeviceInfo(DeviceInstance instance)
			{
				this.deviceInstance = instance;
			}

			// Token: 0x06001F44 RID: 8004 RVA: 0x00018E3C File Offset: 0x00017E3C
			public Guid GetInstanceGuid()
			{
				return this.deviceInstance.InstanceGuid;
			}

			// Token: 0x06001F45 RID: 8005 RVA: 0x00018E4C File Offset: 0x00017E4C
			public override string ToString()
			{
				return this.deviceInstance.ProductName;
			}

			// Token: 0x04000117 RID: 279
			private DeviceInstance deviceInstance;
		}
	}
}
