namespace Bham.Ptu.UI {
	partial class PortSelectForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.@__port = new System.Windows.Forms.ListBox();
			this.@__portLbl = new System.Windows.Forms.Label();
			this.@__ok = new System.Windows.Forms.Button();
			this.@__joystickLbl = new System.Windows.Forms.Label();
			this.@__joystick = new System.Windows.Forms.ListBox();
			this.@__test = new System.Windows.Forms.Button();
			this.@__layout = new System.Windows.Forms.TableLayoutPanel();
			this.@__layout.SuspendLayout();
			this.SuspendLayout();
			// 
			// __port
			// 
			this.@__port.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__port.FormattingEnabled = true;
			this.@__port.IntegralHeight = false;
			this.@__port.Location = new System.Drawing.Point(3, 23);
			this.@__port.Name = "__port";
			this.@__port.Size = new System.Drawing.Size(410, 86);
			this.@__port.TabIndex = 0;
			// 
			// __portLbl
			// 
			this.@__portLbl.AutoSize = true;
			this.@__portLbl.Location = new System.Drawing.Point(3, 0);
			this.@__portLbl.Name = "__portLbl";
			this.@__portLbl.Size = new System.Drawing.Size(247, 13);
			this.@__portLbl.TabIndex = 1;
			this.@__portLbl.Text = "Select COM Port - Ports must be tested before use.";
			// 
			// __ok
			// 
			this.@__ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.@__ok.Enabled = false;
			this.@__ok.Location = new System.Drawing.Point(353, 242);
			this.@__ok.Name = "__ok";
			this.@__ok.Size = new System.Drawing.Size(75, 23);
			this.@__ok.TabIndex = 1;
			this.@__ok.Text = "OK";
			this.@__ok.UseVisualStyleBackColor = true;
			// 
			// __joystickLbl
			// 
			this.@__joystickLbl.AutoSize = true;
			this.@__joystickLbl.Location = new System.Drawing.Point(3, 112);
			this.@__joystickLbl.Name = "__joystickLbl";
			this.@__joystickLbl.Size = new System.Drawing.Size(223, 13);
			this.@__joystickLbl.TabIndex = 4;
			this.@__joystickLbl.Text = "Select Joystick - Joystick selection is optional.";
			// 
			// __joystick
			// 
			this.@__joystick.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__joystick.FormattingEnabled = true;
			this.@__joystick.IntegralHeight = false;
			this.@__joystick.Location = new System.Drawing.Point(3, 135);
			this.@__joystick.Name = "__joystick";
			this.@__joystick.Size = new System.Drawing.Size(410, 86);
			this.@__joystick.TabIndex = 3;
			// 
			// __test
			// 
			this.@__test.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.@__test.Location = new System.Drawing.Point(12, 242);
			this.@__test.Name = "__test";
			this.@__test.Size = new System.Drawing.Size(112, 23);
			this.@__test.TabIndex = 0;
			this.@__test.Text = "Test COM Ports";
			this.@__test.UseVisualStyleBackColor = true;
			// 
			// __layout
			// 
			this.@__layout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__layout.ColumnCount = 1;
			this.@__layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.@__layout.Controls.Add(this.@__portLbl, 0, 0);
			this.@__layout.Controls.Add(this.@__port, 0, 1);
			this.@__layout.Controls.Add(this.@__joystick, 0, 3);
			this.@__layout.Controls.Add(this.@__joystickLbl, 0, 2);
			this.@__layout.Location = new System.Drawing.Point(12, 12);
			this.@__layout.Name = "__layout";
			this.@__layout.RowCount = 4;
			this.@__layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.@__layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.@__layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.@__layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.@__layout.Size = new System.Drawing.Size(416, 224);
			this.@__layout.TabIndex = 6;
			// 
			// PortSelectForm
			// 
			this.AcceptButton = this.@__ok;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(440, 277);
			this.Controls.Add(this.@__layout);
			this.Controls.Add(this.@__test);
			this.Controls.Add(this.@__ok);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PortSelectForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.Text = "Port Selection";
			this.@__layout.ResumeLayout(false);
			this.@__layout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox __port;
		private System.Windows.Forms.Label __portLbl;
		private System.Windows.Forms.Button __ok;
		private System.Windows.Forms.Label __joystickLbl;
		private System.Windows.Forms.ListBox __joystick;
		private System.Windows.Forms.Button __test;
		private System.Windows.Forms.TableLayoutPanel __layout;
	}
}