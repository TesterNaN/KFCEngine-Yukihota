using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace Yukihota
{
	public class OptionsForm : Form
	{
		private struct InputDeviceInfo
		{
			private DeviceInstance deviceInstance;

			public InputDeviceInfo(DeviceInstance instance)
			{
				deviceInstance = instance;
			}

			public Guid GetInstanceGuid()
			{
				return deviceInstance.InstanceGuid;
			}

			public override string ToString()
			{
				return deviceInstance.ProductName;
			}
		}

		private Font font;

		private Device device;

		private bool forceFeedback;

		private bool fullscreen;

		private ButtonLayout buttonLayout;

		private bool showOptions;

		private Container components = null;

		private System.Windows.Forms.Button btnOk;

		private System.Windows.Forms.Button btnCancel;

		private PictureBox picLogo;

		private Label lblGamepad;

		private ComboBox cmbDevice;

		private System.Windows.Forms.Button btnFont;

		private FontDialog fontDialog;

		private GroupBox grpDisplay;

		private RadioButton rdoFullscreen;

		private RadioButton rdoWindowed;

		private GroupBox grpLanguage;

		private RadioButton rdoSimplified;

		private RadioButton rdoTraditional;

		private RadioButton rdoJapanese;

		private LinkLabel lnkManual;

		private CheckBox chkVibration;

		private CheckBox chkDontShow;

		public OptionsForm()
		{
			InitializeComponent();
			foreach (object device in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly))
			{
				DeviceInstance instance = (DeviceInstance)device;
				cmbDevice.Items.Add(new InputDeviceInfo(instance));
			}
			if (cmbDevice.Items.Count > 0)
			{
				cmbDevice.SelectedIndex = 0;
				InputDeviceInfo inputInfo = (InputDeviceInfo)cmbDevice.SelectedItem;
				device = new Device(inputInfo.GetInstanceGuid());
				Control control = chkVibration;
				control.Enabled = device.Caps.ForceFeedback;
				CheckBox checkBox = chkVibration;
				checkBox.Checked = device.Caps.ForceFeedback;
				forceFeedback = device.Caps.ForceFeedback;
			}
			else
			{
				chkVibration.Enabled = false;
				chkVibration.Checked = false;
				forceFeedback = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ResourceManager resourceManager = new ResourceManager(typeof(OptionsForm));
			btnOk = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			picLogo = new PictureBox();
			lblGamepad = new Label();
			cmbDevice = new ComboBox();
			btnFont = new System.Windows.Forms.Button();
			fontDialog = new FontDialog();
			grpDisplay = new GroupBox();
			rdoFullscreen = new RadioButton();
			rdoWindowed = new RadioButton();
			grpLanguage = new GroupBox();
			rdoSimplified = new RadioButton();
			rdoTraditional = new RadioButton();
			rdoJapanese = new RadioButton();
			chkDontShow = new CheckBox();
			lnkManual = new LinkLabel();
			chkVibration = new CheckBox();
			grpDisplay.SuspendLayout();
			grpLanguage.SuspendLayout();
			base.SuspendLayout();
			btnOk.DialogResult = DialogResult.OK;
			btnOk.Location = new Point(32, 280);
			btnOk.Name = "btnOk";
			btnOk.Size = new Size(96, 23);
			btnOk.TabIndex = 0;
			btnOk.Text = "确定(&O)";
			btnOk.Click += new EventHandler(this.OnSimplifiedCheckedChanged);
			btnCancel.DialogResult = DialogResult.Cancel;
			btnCancel.Location = new Point(184, 280);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new Size(96, 23);
			btnCancel.TabIndex = 1;
			btnCancel.Text = "取消(&C)";
			picLogo.Image = (Image)resourceManager.GetObject("picTitle.Image");
			picLogo.Location = new Point(64, 8);
			picLogo.Name = "picTitle";
			picLogo.Size = new Size(160, 50);
			picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
			picLogo.TabIndex = 4;
			picLogo.TabStop = false;
			lblGamepad.BackColor = Color.Transparent;
			lblGamepad.Location = new Point(16, 64);
			lblGamepad.Name = "lblGamepad";
			lblGamepad.Size = new Size(144, 16);
			lblGamepad.TabIndex = 2;
			lblGamepad.Text = "手柄选择(&G)";
			cmbDevice.DropDownStyle = ComboBoxStyle.DropDownList;
			cmbDevice.Location = new Point(16, 80);
			cmbDevice.Name = "cbbGamepad";
			cmbDevice.Size = new Size(288, 20);
			cmbDevice.TabIndex = 3;
			cmbDevice.SelectedIndexChanged += new EventHandler(this.OnTraditionalCheckedChanged);
			btnFont.Location = new Point(104, 112);
			btnFont.Name = "btnFont";
			btnFont.Size = new Size(104, 23);
			btnFont.TabIndex = 5;
			btnFont.Text = "字体(&F)";
			btnFont.Click += new EventHandler(this.OnFontClick);
			fontDialog.AllowVerticalFonts = false;
			fontDialog.Font = new Font("黑体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			fontDialog.FontMustExist = true;
			fontDialog.MaxSize = 24;
			fontDialog.MinSize = 24;
			fontDialog.ShowEffects = false;
			grpDisplay.BackColor = Color.Transparent;
			grpDisplay.Controls.Add(rdoFullscreen);
			grpDisplay.Controls.Add(rdoWindowed);
			grpDisplay.Location = new Point(8, 144);
			grpDisplay.Name = "gpbMode";
			grpDisplay.Size = new Size(144, 80);
			grpDisplay.TabIndex = 6;
			grpDisplay.TabStop = false;
			grpDisplay.Text = "显示模式(&M)";
			rdoFullscreen.BackColor = Color.Transparent;
			rdoFullscreen.Checked = true;
			rdoFullscreen.Location = new Point(8, 24);
			rdoFullscreen.Name = "rbtFullScreen";
			rdoFullscreen.Size = new Size(136, 24);
			rdoFullscreen.TabIndex = 0;
			rdoFullscreen.TabStop = true;
			rdoFullscreen.Text = "全屏幕(&F)";
			rdoWindowed.BackColor = Color.Transparent;
			rdoWindowed.Location = new Point(8, 48);
			rdoWindowed.Name = "rbtWindowed";
			rdoWindowed.Size = new Size(136, 24);
			rdoWindowed.TabIndex = 1;
			rdoWindowed.Text = "窗口模式(&W)";
			grpLanguage.BackColor = Color.Transparent;
			grpLanguage.Controls.Add(rdoSimplified);
			grpLanguage.Controls.Add(rdoTraditional);
			grpLanguage.Controls.Add(rdoJapanese);
			grpLanguage.Location = new Point(160, 144);
			grpLanguage.Name = "gpbLanguage";
			grpLanguage.Size = new Size(144, 104);
			grpLanguage.TabIndex = 7;
			grpLanguage.TabStop = false;
			grpLanguage.Text = "语言(&L)";
			rdoSimplified.BackColor = Color.Transparent;
			rdoSimplified.Checked = true;
			rdoSimplified.Location = new Point(8, 24);
			rdoSimplified.Name = "rbtSC";
			rdoSimplified.Size = new Size(136, 24);
			rdoSimplified.TabIndex = 0;
			rdoSimplified.TabStop = true;
			rdoSimplified.Text = "简体中文(&S)";
			rdoSimplified.CheckedChanged += new EventHandler(this.OnManualLinkClicked);
			rdoTraditional.BackColor = Color.Transparent;
			rdoTraditional.Location = new Point(8, 48);
			rdoTraditional.Name = "rbtTC";
			rdoTraditional.Size = new Size(136, 24);
			rdoTraditional.TabIndex = 1;
			rdoTraditional.Text = "繁體中文(&T)";
			rdoTraditional.CheckedChanged += new EventHandler(this.ApplyButtonLayout);
			rdoJapanese.BackColor = Color.Transparent;
			rdoJapanese.Enabled = false;
			rdoJapanese.Location = new Point(8, 72);
			rdoJapanese.Name = "rbtJP";
			rdoJapanese.Size = new Size(136, 24);
			rdoJapanese.TabIndex = 2;
			rdoJapanese.Text = "日本語(&J)";
			rdoJapanese.CheckedChanged += new EventHandler(this.OnJapaneseCheckedChanged);
			chkDontShow.BackColor = Color.Transparent;
			chkDontShow.Location = new Point(8, 232);
			chkDontShow.Name = "ckbDontShow";
			chkDontShow.Size = new Size(152, 24);
			chkDontShow.TabIndex = 8;
			chkDontShow.Text = "不再显示(&S)";
			lnkManual.BackColor = Color.Transparent;
			lnkManual.Location = new Point(112, 256);
			lnkManual.Name = "llbManual";
			lnkManual.Size = new Size(96, 16);
			lnkManual.TabIndex = 9;
			lnkManual.TabStop = true;
			lnkManual.Text = "手册(&M)";
			lnkManual.TextAlign = ContentAlignment.MiddleCenter;
			lnkManual.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnManualLinkClicked);
			chkVibration.BackColor = Color.Transparent;
			chkVibration.Location = new Point(208, 64);
			chkVibration.Name = "ckbEffect";
			chkVibration.Size = new Size(88, 16);
			chkVibration.TabIndex = 4;
			chkVibration.Text = "振动(&E)";
			AutoScaleBaseSize = new Size(6, 14);
			ClientSize = new Size(314, 312);
			base.Controls.Add(picLogo);
			base.Controls.Add(lblGamepad);
			base.Controls.Add(cmbDevice);
			base.Controls.Add(chkVibration);
			base.Controls.Add(btnFont);
			base.Controls.Add(grpDisplay);
			base.Controls.Add(grpLanguage);
			base.Controls.Add(chkDontShow);
			base.Controls.Add(lnkManual);
			base.Controls.Add(btnOk);
			base.Controls.Add(btnCancel);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Icon = (Icon)resourceManager.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			base.Name = "Options";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "选项";
			Load += new EventHandler(this.OnFormLoad);
			grpDisplay.ResumeLayout(false);
			grpLanguage.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void OnFormLoad(object sender, EventArgs e)
		{
			fontDialog.Font = font;
			if (fullscreen)
			{
				rdoFullscreen.Checked = true;
			}
			else
			{
				rdoWindowed.Checked = true;
			}
			switch (buttonLayout)
			{
			case ButtonLayout.SimplifiedChinese:
				rdoSimplified.Checked = true;
				break;
			case ButtonLayout.TraditionalChinese:
				rdoTraditional.Checked = true;
				break;
			case ButtonLayout.Japanese:
				rdoJapanese.Checked = true;
				break;
			}
			ApplyLanguage(buttonLayout);
		}

		private void OnSimplifiedCheckedChanged(object sender, EventArgs e)
		{
			if (cmbDevice.SelectedItem != null)
			{
				InputDeviceInfo instance = (InputDeviceInfo)cmbDevice.SelectedItem;
				device = new Device(instance.GetInstanceGuid());
			}
			else
			{
				device = null;
			}
			forceFeedback = chkVibration.Checked;
			fullscreen = rdoFullscreen.Checked;
			if (rdoJapanese.Checked)
			{
				buttonLayout = ButtonLayout.Japanese;
			}
			else if (rdoTraditional.Checked)
			{
				buttonLayout = ButtonLayout.TraditionalChinese;
			}
			else
			{
				buttonLayout = ButtonLayout.SimplifiedChinese;
			}
			showOptions = !chkDontShow.Checked;
		}

		private void OnTraditionalCheckedChanged(object sender, EventArgs e)
		{
			if (((ComboBox)sender).SelectedItem != null)
			{
				InputDeviceInfo instance = (InputDeviceInfo)((ComboBox)sender).SelectedItem;
				Device device = new Device(instance.GetInstanceGuid());
				chkVibration.Enabled = device.Caps.ForceFeedback;
				chkVibration.Checked = device.Caps.ForceFeedback;
			}
			else
			{
				chkVibration.Enabled = false;
				chkVibration.Checked = false;
			}
		}

		private void OnFontClick(object sender, EventArgs e)
		{
			if (fontDialog.ShowDialog() == DialogResult.OK)
			{
				font = fontDialog.Font;
			}
		}

		private void OnManualLinkClicked(object sender, EventArgs e)
		{
			ApplyLanguage(ButtonLayout.SimplifiedChinese);
		}

		private void ApplyButtonLayout(object sender, EventArgs e)
		{
			ApplyLanguage(ButtonLayout.TraditionalChinese);
		}

		private void OnJapaneseCheckedChanged(object sender, EventArgs e)
		{
			ApplyLanguage(ButtonLayout.Japanese);
		}

		private void OnManualLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				if (rdoJapanese.Checked)
				{
					Process.Start("Manual\\Index_jp.html");
				}
				else if (rdoTraditional.Checked)
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

		private void ApplyLanguage(ButtonLayout layout)
		{
			switch (layout)
			{
			case ButtonLayout.SimplifiedChinese:
				btnOk.Text = "确定(&O)";
				btnCancel.Text = "取消(&C)";
				btnFont.Text = "字体(&F)";
				lblGamepad.Text = "手柄选择(&G)";
				chkVibration.Text = "振动(&E)";
				grpDisplay.Text = "显示模式(&M)";
				rdoWindowed.Text = "窗口模式(&W)";
				rdoFullscreen.Text = "全屏幕(&F)";
				grpLanguage.Text = "语言(&L)";
				chkDontShow.Text = "不再显示(&S)";
				lnkManual.Text = "手册(&M)";
				Text = "选项";
				break;
			case ButtonLayout.TraditionalChinese:
				btnOk.Text = "確定(&O)";
				btnCancel.Text = "取消(&C)";
				btnFont.Text = "字體(&F)";
				lblGamepad.Text = "手柄選擇(&G)";
				chkVibration.Text = "振動(&E)";
				grpDisplay.Text = "顯示模式(&M)";
				rdoWindowed.Text = "窗口模式(&W)";
				rdoFullscreen.Text = "全屏幕(&F)";
				grpLanguage.Text = "語言(&L)";
				chkDontShow.Text = "不再顯示(&S)";
				lnkManual.Text = "手冊(&M)";
				Text = "選項";
				break;
			case ButtonLayout.Japanese:
				btnOk.Text = "開始(&O)";
				btnCancel.Text = "キャンセル(&C)";
				btnFont.Text = "フォント(&F)";
				lblGamepad.Text = "コントローラ選択(&G)";
				chkVibration.Text = "振動(&E)";
				grpDisplay.Text = "画面モード(&M)";
				rdoWindowed.Text = "窓モード(&W)";
				rdoFullscreen.Text = "フルスクリーン(&F)";
				grpLanguage.Text = "言語(&L)";
				chkDontShow.Text = "以降は提示しない(&S)";
				lnkManual.Text = "マニュアル(&M)";
				Text = "メニュー";
				break;
			}
		}

		public Font GetFontName()
		{
			return font;
		}

		public void SetFontName(Font extensionName)
		{
			font = extensionName;
		}

		public Device GetDevice()
		{
			return device;
		}

		public void SetDevice(Device extensionName)
		{
			device = extensionName;
		}

		public bool GetForceFeedback()
		{
			return forceFeedback;
		}

		public void SetForceFeedback(bool extensionName)
		{
			forceFeedback = extensionName;
		}

		public bool GetFullscreen()
		{
			return fullscreen;
		}

		public void SetFullscreen(bool extensionName)
		{
			fullscreen = extensionName;
		}

		public ButtonLayout GetButtonLayout()
		{
			return buttonLayout;
		}

		public void SetButtonLayout(ButtonLayout extensionName)
		{
			buttonLayout = extensionName;
		}

		public bool GetShowOptions()
		{
			return showOptions;
		}

		public void SetShowOptions(bool extensionName)
		{
			showOptions = extensionName;
		}
	}
}
