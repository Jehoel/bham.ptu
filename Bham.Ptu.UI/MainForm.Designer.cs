namespace Bham.Ptu.UI {
	partial class MainForm {
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Label @__acclLbl1;
			System.Windows.Forms.Label @__speedLbl1;
			System.Windows.Forms.Label @__baseLbl1;
			System.Windows.Forms.Label @__acclLbl2;
			System.Windows.Forms.Label @__speedLbl2;
			System.Windows.Forms.Label @__baseLbl2;
			System.Windows.Forms.Label @__environmentLbl;
			this.@__firmwareLbl = new System.Windows.Forms.Label();
			this.@__panGrp = new System.Windows.Forms.GroupBox();
			this.@__accl1Lbl = new System.Windows.Forms.Label();
			this.@__base1Lbl = new System.Windows.Forms.Label();
			this.@__speed1Lbl = new System.Windows.Forms.Label();
			this.@__pan = new Bham.Ptu.UI.AngleControlOffset();
			this.@__panAccl = new Bham.Ptu.UI.TrackBar2();
			this.@__panBase = new Bham.Ptu.UI.TrackBar2();
			this.@__panSpeed = new Bham.Ptu.UI.TrackBar2();
			this.@__tiltGrp = new System.Windows.Forms.GroupBox();
			this.@__accl2Lbl = new System.Windows.Forms.Label();
			this.@__base2Lbl = new System.Windows.Forms.Label();
			this.@__speed2Lbl = new System.Windows.Forms.Label();
			this.@__tiltAccl = new Bham.Ptu.UI.TrackBar2();
			this.@__tiltBase = new Bham.Ptu.UI.TrackBar2();
			this.@__tiltSpeed = new Bham.Ptu.UI.TrackBar2();
			this.@__tilt = new Bham.Ptu.UI.AngleControlOffset();
			this.@__timer = new System.Windows.Forms.Timer(this.components);
			this.@__home = new System.Windows.Forms.Button();
			this.@__joystick = new System.Windows.Forms.CheckBox();
			this.@__joystickPollThread = new System.ComponentModel.BackgroundWorker();
			this.@__state = new System.Windows.Forms.PropertyGrid();
			this.@__reset = new System.Windows.Forms.Button();
			this.@__deviceState = new System.Windows.Forms.GroupBox();
			this.@__configSet = new System.Windows.Forms.Button();
			this.@__refreshInit = new System.Windows.Forms.Button();
			this.@__configFactory = new System.Windows.Forms.Button();
			this.@__configSave = new System.Windows.Forms.Button();
			this.@__configLoad = new System.Windows.Forms.Button();
			this.@__firmware = new System.Windows.Forms.TextBox();
			this.@__environment = new System.Windows.Forms.TextBox();
			this.@__status = new System.Windows.Forms.StatusBar();
			this.@__tooltip = new System.Windows.Forms.ToolTip(this.components);
			@__acclLbl1 = new System.Windows.Forms.Label();
			@__speedLbl1 = new System.Windows.Forms.Label();
			@__baseLbl1 = new System.Windows.Forms.Label();
			@__acclLbl2 = new System.Windows.Forms.Label();
			@__speedLbl2 = new System.Windows.Forms.Label();
			@__baseLbl2 = new System.Windows.Forms.Label();
			@__environmentLbl = new System.Windows.Forms.Label();
			this.@__panGrp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.@__panAccl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.@__panBase)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.@__panSpeed)).BeginInit();
			this.@__tiltGrp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.@__tiltAccl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.@__tiltBase)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.@__tiltSpeed)).BeginInit();
			this.@__deviceState.SuspendLayout();
			this.SuspendLayout();
			// 
			// __acclLbl1
			// 
			@__acclLbl1.AutoSize = true;
			@__acclLbl1.Location = new System.Drawing.Point(299, 16);
			@__acclLbl1.Name = "__acclLbl1";
			@__acclLbl1.Size = new System.Drawing.Size(34, 13);
			@__acclLbl1.TabIndex = 6;
			@__acclLbl1.Text = "Accel";
			// 
			// __speedLbl1
			// 
			@__speedLbl1.AutoSize = true;
			@__speedLbl1.Location = new System.Drawing.Point(209, 16);
			@__speedLbl1.Name = "__speedLbl1";
			@__speedLbl1.Size = new System.Drawing.Size(38, 13);
			@__speedLbl1.TabIndex = 4;
			@__speedLbl1.Text = "Speed";
			// 
			// __baseLbl1
			// 
			@__baseLbl1.AutoSize = true;
			@__baseLbl1.Location = new System.Drawing.Point(255, 16);
			@__baseLbl1.Name = "__baseLbl1";
			@__baseLbl1.Size = new System.Drawing.Size(31, 13);
			@__baseLbl1.TabIndex = 5;
			@__baseLbl1.Text = "Base";
			@__baseLbl1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// __acclLbl2
			// 
			@__acclLbl2.AutoSize = true;
			@__acclLbl2.Location = new System.Drawing.Point(299, 16);
			@__acclLbl2.Name = "__acclLbl2";
			@__acclLbl2.Size = new System.Drawing.Size(34, 13);
			@__acclLbl2.TabIndex = 11;
			@__acclLbl2.Text = "Accel";
			// 
			// __speedLbl2
			// 
			@__speedLbl2.AutoSize = true;
			@__speedLbl2.Location = new System.Drawing.Point(209, 16);
			@__speedLbl2.Name = "__speedLbl2";
			@__speedLbl2.Size = new System.Drawing.Size(38, 13);
			@__speedLbl2.TabIndex = 9;
			@__speedLbl2.Text = "Speed";
			// 
			// __baseLbl2
			// 
			@__baseLbl2.AutoSize = true;
			@__baseLbl2.Location = new System.Drawing.Point(255, 16);
			@__baseLbl2.Name = "__baseLbl2";
			@__baseLbl2.Size = new System.Drawing.Size(31, 13);
			@__baseLbl2.TabIndex = 10;
			@__baseLbl2.Text = "Base";
			@__baseLbl2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// __environmentLbl
			// 
			@__environmentLbl.Location = new System.Drawing.Point(2, 34);
			@__environmentLbl.Name = "__environmentLbl";
			@__environmentLbl.Size = new System.Drawing.Size(76, 17);
			@__environmentLbl.TabIndex = 15;
			@__environmentLbl.Text = "Environment:";
			@__environmentLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// __firmwareLbl
			// 
			this.@__firmwareLbl.Location = new System.Drawing.Point(12, 9);
			this.@__firmwareLbl.Name = "__firmwareLbl";
			this.@__firmwareLbl.Size = new System.Drawing.Size(66, 17);
			this.@__firmwareLbl.TabIndex = 14;
			this.@__firmwareLbl.Text = "Device:";
			this.@__firmwareLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// __panGrp
			// 
			this.@__panGrp.Controls.Add(this.@__accl1Lbl);
			this.@__panGrp.Controls.Add(this.@__base1Lbl);
			this.@__panGrp.Controls.Add(this.@__speed1Lbl);
			this.@__panGrp.Controls.Add(this.@__pan);
			this.@__panGrp.Controls.Add(@__acclLbl1);
			this.@__panGrp.Controls.Add(@__speedLbl1);
			this.@__panGrp.Controls.Add(@__baseLbl1);
			this.@__panGrp.Controls.Add(this.@__panAccl);
			this.@__panGrp.Controls.Add(this.@__panBase);
			this.@__panGrp.Controls.Add(this.@__panSpeed);
			this.@__panGrp.Location = new System.Drawing.Point(12, 57);
			this.@__panGrp.Name = "__panGrp";
			this.@__panGrp.Size = new System.Drawing.Size(351, 240);
			this.@__panGrp.TabIndex = 4;
			this.@__panGrp.TabStop = false;
			this.@__panGrp.Text = "Pan";
			// 
			// __accl1Lbl
			// 
			this.@__accl1Lbl.AutoSize = true;
			this.@__accl1Lbl.Location = new System.Drawing.Point(299, 220);
			this.@__accl1Lbl.Name = "__accl1Lbl";
			this.@__accl1Lbl.Size = new System.Drawing.Size(23, 13);
			this.@__accl1Lbl.TabIndex = 9;
			this.@__accl1Lbl.Text = "u/s";
			// 
			// __base1Lbl
			// 
			this.@__base1Lbl.AutoSize = true;
			this.@__base1Lbl.Location = new System.Drawing.Point(255, 220);
			this.@__base1Lbl.Name = "__base1Lbl";
			this.@__base1Lbl.Size = new System.Drawing.Size(23, 13);
			this.@__base1Lbl.TabIndex = 8;
			this.@__base1Lbl.Text = "u/s";
			// 
			// __speed1Lbl
			// 
			this.@__speed1Lbl.AutoSize = true;
			this.@__speed1Lbl.Location = new System.Drawing.Point(209, 220);
			this.@__speed1Lbl.Name = "__speed1Lbl";
			this.@__speed1Lbl.Size = new System.Drawing.Size(23, 13);
			this.@__speed1Lbl.TabIndex = 7;
			this.@__speed1Lbl.Text = "u/s";
			// 
			// __pan
			// 
			this.@__pan.Angle0 = 0;
			this.@__pan.AngleColors = new System.Drawing.Color[] {
        System.Drawing.Color.Red,
        System.Drawing.Color.Blue,
        System.Drawing.Color.Green};
			this.@__pan.BackColor = System.Drawing.Color.Transparent;
			this.@__pan.BaseAngle = 0;
			this.@__pan.Location = new System.Drawing.Point(6, 19);
			this.@__pan.Name = "__pan";
			this.@__pan.RangeMax = 360;
			this.@__pan.RangeMin = 0;
			this.@__pan.ShowAngle = false;
			this.@__pan.Size = new System.Drawing.Size(200, 200);
			this.@__pan.TabIndex = 0;
			// 
			// __panAccl
			// 
			this.@__panAccl.Location = new System.Drawing.Point(302, 27);
			this.@__panAccl.Maximum = 200000;
			this.@__panAccl.Minimum = 501;
			this.@__panAccl.Name = "__panAccl";
			this.@__panAccl.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.@__panAccl.Size = new System.Drawing.Size(45, 190);
			this.@__panAccl.TabIndex = 2;
			this.@__panAccl.TickFrequency = 100;
			this.@__panAccl.TickStyle = System.Windows.Forms.TickStyle.None;
			this.@__panAccl.Value = 501;
			// 
			// __panBase
			// 
			this.@__panBase.Location = new System.Drawing.Point(257, 27);
			this.@__panBase.Maximum = 1000;
			this.@__panBase.Name = "__panBase";
			this.@__panBase.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.@__panBase.Size = new System.Drawing.Size(45, 190);
			this.@__panBase.TabIndex = 1;
			this.@__panBase.TickFrequency = 100;
			this.@__panBase.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// __panSpeed
			// 
			this.@__panSpeed.Location = new System.Drawing.Point(212, 27);
			this.@__panSpeed.Maximum = 1000;
			this.@__panSpeed.Name = "__panSpeed";
			this.@__panSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.@__panSpeed.Size = new System.Drawing.Size(45, 190);
			this.@__panSpeed.TabIndex = 0;
			this.@__panSpeed.TickFrequency = 100;
			this.@__panSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// __tiltGrp
			// 
			this.@__tiltGrp.Controls.Add(this.@__accl2Lbl);
			this.@__tiltGrp.Controls.Add(@__acclLbl2);
			this.@__tiltGrp.Controls.Add(this.@__base2Lbl);
			this.@__tiltGrp.Controls.Add(@__speedLbl2);
			this.@__tiltGrp.Controls.Add(this.@__speed2Lbl);
			this.@__tiltGrp.Controls.Add(@__baseLbl2);
			this.@__tiltGrp.Controls.Add(this.@__tiltAccl);
			this.@__tiltGrp.Controls.Add(this.@__tiltBase);
			this.@__tiltGrp.Controls.Add(this.@__tiltSpeed);
			this.@__tiltGrp.Controls.Add(this.@__tilt);
			this.@__tiltGrp.Location = new System.Drawing.Point(12, 303);
			this.@__tiltGrp.Name = "__tiltGrp";
			this.@__tiltGrp.Size = new System.Drawing.Size(351, 240);
			this.@__tiltGrp.TabIndex = 5;
			this.@__tiltGrp.TabStop = false;
			this.@__tiltGrp.Text = "Tilt";
			// 
			// __accl2Lbl
			// 
			this.@__accl2Lbl.AutoSize = true;
			this.@__accl2Lbl.Location = new System.Drawing.Point(299, 220);
			this.@__accl2Lbl.Name = "__accl2Lbl";
			this.@__accl2Lbl.Size = new System.Drawing.Size(23, 13);
			this.@__accl2Lbl.TabIndex = 12;
			this.@__accl2Lbl.Text = "u/s";
			// 
			// __base2Lbl
			// 
			this.@__base2Lbl.AutoSize = true;
			this.@__base2Lbl.Location = new System.Drawing.Point(255, 220);
			this.@__base2Lbl.Name = "__base2Lbl";
			this.@__base2Lbl.Size = new System.Drawing.Size(23, 13);
			this.@__base2Lbl.TabIndex = 11;
			this.@__base2Lbl.Text = "u/s";
			// 
			// __speed2Lbl
			// 
			this.@__speed2Lbl.AutoSize = true;
			this.@__speed2Lbl.Location = new System.Drawing.Point(209, 220);
			this.@__speed2Lbl.Name = "__speed2Lbl";
			this.@__speed2Lbl.Size = new System.Drawing.Size(23, 13);
			this.@__speed2Lbl.TabIndex = 10;
			this.@__speed2Lbl.Text = "u/s";
			// 
			// __tiltAccl
			// 
			this.@__tiltAccl.Location = new System.Drawing.Point(302, 27);
			this.@__tiltAccl.Maximum = 200000;
			this.@__tiltAccl.Minimum = 501;
			this.@__tiltAccl.Name = "__tiltAccl";
			this.@__tiltAccl.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.@__tiltAccl.Size = new System.Drawing.Size(45, 190);
			this.@__tiltAccl.TabIndex = 2;
			this.@__tiltAccl.TickFrequency = 100;
			this.@__tiltAccl.TickStyle = System.Windows.Forms.TickStyle.None;
			this.@__tiltAccl.Value = 501;
			// 
			// __tiltBase
			// 
			this.@__tiltBase.Location = new System.Drawing.Point(257, 27);
			this.@__tiltBase.Maximum = 1000;
			this.@__tiltBase.Name = "__tiltBase";
			this.@__tiltBase.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.@__tiltBase.Size = new System.Drawing.Size(45, 190);
			this.@__tiltBase.TabIndex = 1;
			this.@__tiltBase.TickFrequency = 100;
			this.@__tiltBase.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// __tiltSpeed
			// 
			this.@__tiltSpeed.Location = new System.Drawing.Point(212, 27);
			this.@__tiltSpeed.Maximum = 1000;
			this.@__tiltSpeed.Name = "__tiltSpeed";
			this.@__tiltSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.@__tiltSpeed.Size = new System.Drawing.Size(45, 190);
			this.@__tiltSpeed.TabIndex = 0;
			this.@__tiltSpeed.TickFrequency = 100;
			this.@__tiltSpeed.TickStyle = System.Windows.Forms.TickStyle.None;
			// 
			// __tilt
			// 
			this.@__tilt.Angle0 = 0;
			this.@__tilt.AngleColors = new System.Drawing.Color[] {
        System.Drawing.Color.Red,
        System.Drawing.Color.Blue,
        System.Drawing.Color.Green};
			this.@__tilt.BackColor = System.Drawing.Color.Transparent;
			this.@__tilt.BaseAngle = 0;
			this.@__tilt.Location = new System.Drawing.Point(6, 19);
			this.@__tilt.Name = "__tilt";
			this.@__tilt.RangeMax = 360;
			this.@__tilt.RangeMin = 0;
			this.@__tilt.ShowAngle = false;
			this.@__tilt.Size = new System.Drawing.Size(200, 200);
			this.@__tilt.TabIndex = 1;
			// 
			// __timer
			// 
			this.@__timer.Interval = 150;
			// 
			// __home
			// 
			this.@__home.Location = new System.Drawing.Point(18, 549);
			this.@__home.Name = "__home";
			this.@__home.Size = new System.Drawing.Size(165, 23);
			this.@__home.TabIndex = 2;
			this.@__home.Text = "Home";
			this.@__home.UseVisualStyleBackColor = true;
			// 
			// __joystick
			// 
			this.@__joystick.Appearance = System.Windows.Forms.Appearance.Button;
			this.@__joystick.Location = new System.Drawing.Point(17, 607);
			this.@__joystick.Name = "__joystick";
			this.@__joystick.Size = new System.Drawing.Size(166, 24);
			this.@__joystick.TabIndex = 4;
			this.@__joystick.Text = "Bham.PTU Joystick Control";
			this.@__joystick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.@__tooltip.SetToolTip(this.@__joystick, "Slaves the PTU\'s movement to a joystick attached to this computer (if selected du" +
					"ring application startup). Note this is not the same as the PTU\'s built-in seria" +
					"l joystick support.");
			this.@__joystick.UseVisualStyleBackColor = true;
			// 
			// __joystickPollThread
			// 
			this.@__joystickPollThread.WorkerSupportsCancellation = true;
			// 
			// __state
			// 
			this.@__state.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__state.CommandsVisibleIfAvailable = false;
			this.@__state.HelpVisible = false;
			this.@__state.Location = new System.Drawing.Point(6, 48);
			this.@__state.Name = "__state";
			this.@__state.Size = new System.Drawing.Size(619, 578);
			this.@__state.TabIndex = 1;
			this.@__state.ToolbarVisible = false;
			// 
			// __reset
			// 
			this.@__reset.Location = new System.Drawing.Point(17, 578);
			this.@__reset.Name = "__reset";
			this.@__reset.Size = new System.Drawing.Size(166, 23);
			this.@__reset.TabIndex = 3;
			this.@__reset.Text = "Perform Calibration Reset";
			this.@__reset.UseVisualStyleBackColor = true;
			// 
			// __deviceState
			// 
			this.@__deviceState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__deviceState.Controls.Add(this.@__configSet);
			this.@__deviceState.Controls.Add(this.@__configFactory);
			this.@__deviceState.Controls.Add(this.@__configSave);
			this.@__deviceState.Controls.Add(this.@__configLoad);
			this.@__deviceState.Controls.Add(this.@__state);
			this.@__deviceState.Location = new System.Drawing.Point(369, 57);
			this.@__deviceState.Name = "__deviceState";
			this.@__deviceState.Size = new System.Drawing.Size(631, 632);
			this.@__deviceState.TabIndex = 13;
			this.@__deviceState.TabStop = false;
			this.@__deviceState.Text = "PTU State and Configuration";
			// 
			// __configSet
			// 
			this.@__configSet.Location = new System.Drawing.Point(6, 19);
			this.@__configSet.Name = "__configSet";
			this.@__configSet.Size = new System.Drawing.Size(56, 23);
			this.@__configSet.TabIndex = 6;
			this.@__configSet.Text = "Set";
			this.@__tooltip.SetToolTip(this.@__configSet, "Saves the configuration state from the property grid to the PTU.");
			this.@__configSet.UseVisualStyleBackColor = true;
			// 
			// __refreshInit
			// 
			this.@__refreshInit.Location = new System.Drawing.Point(18, 637);
			this.@__refreshInit.Name = "__refreshInit";
			this.@__refreshInit.Size = new System.Drawing.Size(165, 23);
			this.@__refreshInit.TabIndex = 5;
			this.@__refreshInit.Text = "Refresh";
			this.@__refreshInit.UseVisualStyleBackColor = true;
			// 
			// __configFactory
			// 
			this.@__configFactory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.@__configFactory.Location = new System.Drawing.Point(529, 19);
			this.@__configFactory.Name = "__configFactory";
			this.@__configFactory.Size = new System.Drawing.Size(96, 23);
			this.@__configFactory.TabIndex = 4;
			this.@__configFactory.Text = "Factory Restore";
			this.@__tooltip.SetToolTip(this.@__configFactory, "Restores the PTU to its original factory state.");
			this.@__configFactory.UseVisualStyleBackColor = true;
			// 
			// __configSave
			// 
			this.@__configSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.@__configSave.Location = new System.Drawing.Point(451, 19);
			this.@__configSave.Name = "__configSave";
			this.@__configSave.Size = new System.Drawing.Size(72, 23);
			this.@__configSave.TabIndex = 3;
			this.@__configSave.Text = "Save";
			this.@__tooltip.SetToolTip(this.@__configSave, "Saves the currently set configuration to the PTU\'s memory. Be sure to click \'Set\'" +
					" before saving.");
			this.@__configSave.UseVisualStyleBackColor = true;
			// 
			// __configLoad
			// 
			this.@__configLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.@__configLoad.Location = new System.Drawing.Point(376, 19);
			this.@__configLoad.Name = "__configLoad";
			this.@__configLoad.Size = new System.Drawing.Size(69, 23);
			this.@__configLoad.TabIndex = 2;
			this.@__configLoad.Text = "Reload";
			this.@__tooltip.SetToolTip(this.@__configLoad, "Reloads the PTU\'s configuration from its on-board memory.");
			this.@__configLoad.UseVisualStyleBackColor = true;
			// 
			// __firmware
			// 
			this.@__firmware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__firmware.Location = new System.Drawing.Point(84, 6);
			this.@__firmware.Name = "__firmware";
			this.@__firmware.ReadOnly = true;
			this.@__firmware.Size = new System.Drawing.Size(916, 20);
			this.@__firmware.TabIndex = 0;
			// 
			// __environment
			// 
			this.@__environment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.@__environment.Location = new System.Drawing.Point(84, 31);
			this.@__environment.Name = "__environment";
			this.@__environment.ReadOnly = true;
			this.@__environment.Size = new System.Drawing.Size(916, 20);
			this.@__environment.TabIndex = 1;
			// 
			// __status
			// 
			this.@__status.Location = new System.Drawing.Point(0, 695);
			this.@__status.Name = "__status";
			this.@__status.Size = new System.Drawing.Size(1012, 22);
			this.@__status.TabIndex = 16;
			this.@__status.Text = "Ready";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1012, 717);
			this.Controls.Add(this.@__status);
			this.Controls.Add(this.@__refreshInit);
			this.Controls.Add(this.@__deviceState);
			this.Controls.Add(this.@__environment);
			this.Controls.Add(this.@__firmware);
			this.Controls.Add(@__environmentLbl);
			this.Controls.Add(this.@__firmwareLbl);
			this.Controls.Add(this.@__reset);
			this.Controls.Add(this.@__joystick);
			this.Controls.Add(this.@__home);
			this.Controls.Add(this.@__tiltGrp);
			this.Controls.Add(this.@__panGrp);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Pan Tilt Controller";
			this.@__panGrp.ResumeLayout(false);
			this.@__panGrp.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.@__panAccl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.@__panBase)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.@__panSpeed)).EndInit();
			this.@__tiltGrp.ResumeLayout(false);
			this.@__tiltGrp.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.@__tiltAccl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.@__tiltBase)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.@__tiltSpeed)).EndInit();
			this.@__deviceState.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private AngleControlOffset __pan;
		private AngleControlOffset __tilt;
		private Bham.Ptu.UI.TrackBar2 __panSpeed;
		private System.Windows.Forms.GroupBox __panGrp;
		private System.Windows.Forms.GroupBox __tiltGrp;
		private System.Windows.Forms.Timer __timer;
		private System.Windows.Forms.Button __home;
		private System.Windows.Forms.CheckBox __joystick;
		private System.ComponentModel.BackgroundWorker __joystickPollThread;
		private System.Windows.Forms.PropertyGrid __state;
		private Bham.Ptu.UI.TrackBar2 __panAccl;
		private Bham.Ptu.UI.TrackBar2 __panBase;
		private Bham.Ptu.UI.TrackBar2 __tiltAccl;
		private Bham.Ptu.UI.TrackBar2 __tiltBase;
		private Bham.Ptu.UI.TrackBar2 __tiltSpeed;
		private System.Windows.Forms.Button __reset;
		private System.Windows.Forms.GroupBox __deviceState;
		private System.Windows.Forms.TextBox __firmware;
		private System.Windows.Forms.TextBox __environment;
		private System.Windows.Forms.Label __accl1Lbl;
		private System.Windows.Forms.Label __base1Lbl;
		private System.Windows.Forms.Label __speed1Lbl;
		private System.Windows.Forms.Label __accl2Lbl;
		private System.Windows.Forms.Label __base2Lbl;
		private System.Windows.Forms.Label __speed2Lbl;
		private System.Windows.Forms.Button __configSave;
		private System.Windows.Forms.Button __configLoad;
		private System.Windows.Forms.Button __configFactory;
		private System.Windows.Forms.Label __firmwareLbl;
		private System.Windows.Forms.Button __refreshInit;
		private System.Windows.Forms.StatusBar __status;
		private System.Windows.Forms.Button __configSet;
		private System.Windows.Forms.ToolTip __tooltip;
	}
}