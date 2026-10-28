using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Microsoft.DirectX.DirectInput;

namespace n0001
{
	public class OptionsForm : Form
	{
		private struct InputDeviceInfo
		{
			private DeviceInstance n0061;

			public InputDeviceInfo(DeviceInstance n0060)
			{
				n0061 = n0060;
			}

			public Guid GetInstanceGuid()
			{
				return n0061.InstanceGuid;
			}

			public override string ToString()
			{
				return n0061.ProductName;
			}
		}

		private Font n0037;

		private Device n0006;

		private bool n0009;

		private bool n0038;

		private ButtonLayout n0039;

		private bool n0045;

		private Container n0011 = null;

		private System.Windows.Forms.Button n0012;

		private System.Windows.Forms.Button n0013;

		private PictureBox n0014;

		private Label n0015;

		private ComboBox n0004;

		private System.Windows.Forms.Button n0016;

		private FontDialog n0017;

		private GroupBox n0018;

		private RadioButton n0019;

		private RadioButton n0020;

		private GroupBox n0021;

		private RadioButton n0022;

		private RadioButton n0023;

		private RadioButton n0024;

		private LinkLabel n0026;

		private CheckBox n0008;

		private CheckBox n0025;

		public OptionsForm()
		{
			InitializeComponent();
			foreach (object device in Manager.GetDevices(DeviceClass.GameControl, EnumDevicesFlags.AttachedOnly))
			{
				DeviceInstance n536 = (DeviceInstance)device;
				n0004.Items.Add(new InputDeviceInfo(n536));
			}
			if (n0004.Items.Count > 0)
			{
				n0004.SelectedIndex = 0;
				InputDeviceInfo n537 = (InputDeviceInfo)n0004.SelectedItem;
				n0006 = new Device(n537.GetInstanceGuid());
				Control control = n0008;
				control.Enabled = n0006.Caps.ForceFeedback;
				CheckBox checkBox = n0008;
				checkBox.Checked = n0006.Caps.ForceFeedback;
				n0009 = n0006.Caps.ForceFeedback;
			}
			else
			{
				n0008.Enabled = false;
				n0008.Checked = false;
				n0009 = false;
			}
		}

		protected override void Dispose(bool n0010)
		{
			if (n0010 && n0011 != null)
			{
				n0011.Dispose();
			}
			base.Dispose(n0010);
		}

		private void InitializeComponent()
		{
			ResourceManager resourceManager = new ResourceManager(typeof(OptionsForm));
			n0012 = new System.Windows.Forms.Button();
			n0013 = new System.Windows.Forms.Button();
			n0014 = new PictureBox();
			n0015 = new Label();
			n0004 = new ComboBox();
			n0016 = new System.Windows.Forms.Button();
			n0017 = new FontDialog();
			n0018 = new GroupBox();
			n0019 = new RadioButton();
			n0020 = new RadioButton();
			n0021 = new GroupBox();
			n0022 = new RadioButton();
			n0023 = new RadioButton();
			n0024 = new RadioButton();
			n0025 = new CheckBox();
			n0026 = new LinkLabel();
			n0008 = new CheckBox();
			n0018.SuspendLayout();
			n0021.SuspendLayout();
			base.SuspendLayout();
			n0012.DialogResult = DialogResult.OK;
			n0012.Location = new Point(32, 280);
			n0012.Name = "btnOk";
			n0012.Size = new Size(96, 23);
			n0012.TabIndex = 0;
			n0012.Text = "确定(&O)";
			n0012.Click += new EventHandler(this.n0027);
			n0013.DialogResult = DialogResult.Cancel;
			n0013.Location = new Point(184, 280);
			n0013.Name = "btnCancel";
			n0013.Size = new Size(96, 23);
			n0013.TabIndex = 1;
			n0013.Text = "取消(&C)";
			n0014.Image = (Image)resourceManager.GetObject("picTitle.Image");
			n0014.Location = new Point(64, 8);
			n0014.Name = "picTitle";
			n0014.Size = new Size(160, 50);
			n0014.SizeMode = PictureBoxSizeMode.StretchImage;
			n0014.TabIndex = 4;
			n0014.TabStop = false;
			n0015.BackColor = Color.Transparent;
			n0015.Location = new Point(16, 64);
			n0015.Name = "lblGamepad";
			n0015.Size = new Size(144, 16);
			n0015.TabIndex = 2;
			n0015.Text = "手柄选择(&G)";
			n0004.DropDownStyle = ComboBoxStyle.DropDownList;
			n0004.Location = new Point(16, 80);
			n0004.Name = "cbbGamepad";
			n0004.Size = new Size(288, 20);
			n0004.TabIndex = 3;
			n0004.SelectedIndexChanged += new EventHandler(this.n0028);
			n0016.Location = new Point(104, 112);
			n0016.Name = "btnFont";
			n0016.Size = new Size(104, 23);
			n0016.TabIndex = 5;
			n0016.Text = "字体(&F)";
			n0016.Click += new EventHandler(this.n0029);
			n0017.AllowVerticalFonts = false;
			n0017.Font = new Font("黑体", 24f, FontStyle.Regular, GraphicsUnit.Point, 134);
			n0017.FontMustExist = true;
			n0017.MaxSize = 24;
			n0017.MinSize = 24;
			n0017.ShowEffects = false;
			n0018.BackColor = Color.Transparent;
			n0018.Controls.Add(n0019);
			n0018.Controls.Add(n0020);
			n0018.Location = new Point(8, 144);
			n0018.Name = "gpbMode";
			n0018.Size = new Size(144, 80);
			n0018.TabIndex = 6;
			n0018.TabStop = false;
			n0018.Text = "显示模式(&M)";
			n0019.BackColor = Color.Transparent;
			n0019.Checked = true;
			n0019.Location = new Point(8, 24);
			n0019.Name = "rbtFullScreen";
			n0019.Size = new Size(136, 24);
			n0019.TabIndex = 0;
			n0019.TabStop = true;
			n0019.Text = "全屏幕(&F)";
			n0020.BackColor = Color.Transparent;
			n0020.Location = new Point(8, 48);
			n0020.Name = "rbtWindowed";
			n0020.Size = new Size(136, 24);
			n0020.TabIndex = 1;
			n0020.Text = "窗口模式(&W)";
			n0021.BackColor = Color.Transparent;
			n0021.Controls.Add(n0022);
			n0021.Controls.Add(n0023);
			n0021.Controls.Add(n0024);
			n0021.Location = new Point(160, 144);
			n0021.Name = "gpbLanguage";
			n0021.Size = new Size(144, 104);
			n0021.TabIndex = 7;
			n0021.TabStop = false;
			n0021.Text = "语言(&L)";
			n0022.BackColor = Color.Transparent;
			n0022.Checked = true;
			n0022.Location = new Point(8, 24);
			n0022.Name = "rbtSC";
			n0022.Size = new Size(136, 24);
			n0022.TabIndex = 0;
			n0022.TabStop = true;
			n0022.Text = "简体中文(&S)";
			n0022.CheckedChanged += new EventHandler(this.n0030);
			n0023.BackColor = Color.Transparent;
			n0023.Location = new Point(8, 48);
			n0023.Name = "rbtTC";
			n0023.Size = new Size(136, 24);
			n0023.TabIndex = 1;
			n0023.Text = "繁體中文(&T)";
			n0023.CheckedChanged += new EventHandler(this.n0031);
			n0024.BackColor = Color.Transparent;
			n0024.Enabled = false;
			n0024.Location = new Point(8, 72);
			n0024.Name = "rbtJP";
			n0024.Size = new Size(136, 24);
			n0024.TabIndex = 2;
			n0024.Text = "日本語(&J)";
			n0024.CheckedChanged += new EventHandler(this.n0032);
			n0025.BackColor = Color.Transparent;
			n0025.Location = new Point(8, 232);
			n0025.Name = "ckbDontShow";
			n0025.Size = new Size(152, 24);
			n0025.TabIndex = 8;
			n0025.Text = "不再显示(&S)";
			n0026.BackColor = Color.Transparent;
			n0026.Location = new Point(112, 256);
			n0026.Name = "llbManual";
			n0026.Size = new Size(96, 16);
			n0026.TabIndex = 9;
			n0026.TabStop = true;
			n0026.Text = "手册(&M)";
			n0026.TextAlign = ContentAlignment.MiddleCenter;
			n0026.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnManualLinkClicked);
			n0008.BackColor = Color.Transparent;
			n0008.Location = new Point(208, 64);
			n0008.Name = "ckbEffect";
			n0008.Size = new Size(88, 16);
			n0008.TabIndex = 4;
			n0008.Text = "振动(&E)";
			AutoScaleBaseSize = new Size(6, 14);
			ClientSize = new Size(314, 312);
			base.Controls.Add(n0014);
			base.Controls.Add(n0015);
			base.Controls.Add(n0004);
			base.Controls.Add(n0008);
			base.Controls.Add(n0016);
			base.Controls.Add(n0018);
			base.Controls.Add(n0021);
			base.Controls.Add(n0025);
			base.Controls.Add(n0026);
			base.Controls.Add(n0012);
			base.Controls.Add(n0013);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Icon = (Icon)resourceManager.GetObject("$this.Icon");
			MaximizeBox = false;
			MinimizeBox = false;
			base.Name = "Options";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "选项";
			Load += new EventHandler(this.n0034);
			n0018.ResumeLayout(false);
			n0021.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void n0034(object n0035, EventArgs n0036)
		{
			n0017.Font = n0037;
			if (n0038)
			{
				n0019.Checked = true;
			}
			else
			{
				n0020.Checked = true;
			}
			switch (n0039)
			{
			case ButtonLayout.n0041:
				n0022.Checked = true;
				break;
			case ButtonLayout.n0042:
				n0023.Checked = true;
				break;
			case ButtonLayout.n0043:
				n0024.Checked = true;
				break;
			}
			ApplyLanguage(n0039);
		}

		private void n0027(object n0035, EventArgs n0036)
		{
			if (n0004.SelectedItem != null)
			{
				InputDeviceInfo n536 = (InputDeviceInfo)n0004.SelectedItem;
				n0006 = new Device(n536.GetInstanceGuid());
			}
			else
			{
				n0006 = null;
			}
			n0009 = n0008.Checked;
			n0038 = n0019.Checked;
			if (n0024.Checked)
			{
				n0039 = ButtonLayout.n0043;
			}
			else if (n0023.Checked)
			{
				n0039 = ButtonLayout.n0042;
			}
			else
			{
				n0039 = ButtonLayout.n0041;
			}
			n0045 = !n0025.Checked;
		}

		private void n0028(object n0035, EventArgs n0036)
		{
			if (((ComboBox)n0035).SelectedItem != null)
			{
				InputDeviceInfo n536 = (InputDeviceInfo)((ComboBox)n0035).SelectedItem;
				Device device = new Device(n536.GetInstanceGuid());
				n0008.Enabled = device.Caps.ForceFeedback;
				n0008.Checked = device.Caps.ForceFeedback;
			}
			else
			{
				n0008.Enabled = false;
				n0008.Checked = false;
			}
		}

		private void n0029(object n0035, EventArgs n0036)
		{
			if (n0017.ShowDialog() == DialogResult.OK)
			{
				n0037 = n0017.Font;
			}
		}

		private void n0030(object n0035, EventArgs n0036)
		{
			ApplyLanguage(ButtonLayout.n0041);
		}

		private void n0031(object n0035, EventArgs n0036)
		{
			ApplyLanguage(ButtonLayout.n0042);
		}

		private void n0032(object n0035, EventArgs n0036)
		{
			ApplyLanguage(ButtonLayout.n0043);
		}

		private void OnManualLinkClicked(object n0035, LinkLabelLinkClickedEventArgs n0036)
		{
			try
			{
				if (n0024.Checked)
				{
					Process.Start("Manual\\Index_jp.html");
				}
				else if (n0023.Checked)
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

		private void ApplyLanguage(ButtonLayout n0046)
		{
			switch (n0046)
			{
			case ButtonLayout.n0041:
				n0012.Text = "确定(&O)";
				n0013.Text = "取消(&C)";
				n0016.Text = "字体(&F)";
				n0015.Text = "手柄选择(&G)";
				n0008.Text = "振动(&E)";
				n0018.Text = "显示模式(&M)";
				n0020.Text = "窗口模式(&W)";
				n0019.Text = "全屏幕(&F)";
				n0021.Text = "语言(&L)";
				n0025.Text = "不再显示(&S)";
				n0026.Text = "手册(&M)";
				Text = "选项";
				break;
			case ButtonLayout.n0042:
				n0012.Text = "確定(&O)";
				n0013.Text = "取消(&C)";
				n0016.Text = "字體(&F)";
				n0015.Text = "手柄選擇(&G)";
				n0008.Text = "振動(&E)";
				n0018.Text = "顯示模式(&M)";
				n0020.Text = "窗口模式(&W)";
				n0019.Text = "全屏幕(&F)";
				n0021.Text = "語言(&L)";
				n0025.Text = "不再顯示(&S)";
				n0026.Text = "手冊(&M)";
				Text = "選項";
				break;
			case ButtonLayout.n0043:
				n0012.Text = "開始(&O)";
				n0013.Text = "キャンセル(&C)";
				n0016.Text = "フォント(&F)";
				n0015.Text = "コントローラ選択(&G)";
				n0008.Text = "振動(&E)";
				n0018.Text = "画面モード(&M)";
				n0020.Text = "窓モード(&W)";
				n0019.Text = "フルスクリーン(&F)";
				n0021.Text = "言語(&L)";
				n0025.Text = "以降は提示しない(&S)";
				n0026.Text = "マニュアル(&M)";
				Text = "メニュー";
				break;
			}
		}

		public Font GetFontName()
		{
			return n0037;
		}

		public void SetFontName(Font n0049)
		{
			n0037 = n0049;
		}

		public Device GetDevice()
		{
			return n0006;
		}

		public void SetDevice(Device n0049)
		{
			n0006 = n0049;
		}

		public bool n0052()
		{
			return n0009;
		}

		public void n0053(bool n0049)
		{
			n0009 = n0049;
		}

		public bool n0054()
		{
			return n0038;
		}

		public void n0055(bool n0049)
		{
			n0038 = n0049;
		}

		public ButtonLayout GetButtonLayout()
		{
			return n0039;
		}

		public void n0057(ButtonLayout n0049)
		{
			n0039 = n0049;
		}

		public bool n0058()
		{
			return n0045;
		}

		public void n0059(bool n0049)
		{
			n0045 = n0049;
		}
	}
}
