namespace PushToTalk
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                m_GlobalHook.MouseDownExt -= mouseDown;
                m_GlobalHook.KeyDown -= keyDown;
                m_GlobalHook.MouseUp -= mouseUp;
                m_GlobalHook.KeyUp -= keyUp;

                m_GlobalHook.Dispose();

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.keyText = new System.Windows.Forms.TextBox();
            this.mouseText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.setKey = new System.Windows.Forms.Button();
            this.setMouse = new System.Windows.Forms.Button();
            this.clearKey = new System.Windows.Forms.Button();
            this.clearMouse = new System.Windows.Forms.Button();
            this.cancelMouse = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.delay = new System.Windows.Forms.NumericUpDown();
            this.msec = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.delay)).BeginInit();
            this.SuspendLayout();
            // 
            // keyText
            // 
            this.keyText.Location = new System.Drawing.Point(195, 45);
            this.keyText.Name = "keyText";
            this.keyText.ReadOnly = true;
            this.keyText.Size = new System.Drawing.Size(241, 26);
            this.keyText.TabIndex = 0;
            // 
            // mouseText
            // 
            this.mouseText.Location = new System.Drawing.Point(195, 85);
            this.mouseText.Name = "mouseText";
            this.mouseText.ReadOnly = true;
            this.mouseText.Size = new System.Drawing.Size(241, 26);
            this.mouseText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Bind to key combination";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bind to mouse buttons";
            // 
            // setKey
            // 
            this.setKey.Location = new System.Drawing.Point(442, 42);
            this.setKey.Name = "setKey";
            this.setKey.Size = new System.Drawing.Size(75, 32);
            this.setKey.TabIndex = 4;
            this.setKey.Text = "Set";
            this.setKey.UseVisualStyleBackColor = true;
            this.setKey.Click += new System.EventHandler(this.setKey_Click);
            // 
            // setMouse
            // 
            this.setMouse.Location = new System.Drawing.Point(442, 82);
            this.setMouse.Name = "setMouse";
            this.setMouse.Size = new System.Drawing.Size(75, 32);
            this.setMouse.TabIndex = 5;
            this.setMouse.Text = "Set";
            this.setMouse.UseVisualStyleBackColor = true;
            this.setMouse.Click += new System.EventHandler(this.setMouse_Click);
            // 
            // clearKey
            // 
            this.clearKey.Location = new System.Drawing.Point(523, 42);
            this.clearKey.Name = "clearKey";
            this.clearKey.Size = new System.Drawing.Size(75, 32);
            this.clearKey.TabIndex = 6;
            this.clearKey.Text = "Clear";
            this.clearKey.UseVisualStyleBackColor = true;
            this.clearKey.Click += new System.EventHandler(this.clearKey_Click);
            // 
            // clearMouse
            // 
            this.clearMouse.Location = new System.Drawing.Point(523, 82);
            this.clearMouse.Name = "clearMouse";
            this.clearMouse.Size = new System.Drawing.Size(75, 32);
            this.clearMouse.TabIndex = 7;
            this.clearMouse.Text = "Clear";
            this.clearMouse.UseVisualStyleBackColor = true;
            this.clearMouse.Click += new System.EventHandler(this.clearMouse_Click);
            // 
            // cancelMouse
            // 
            this.cancelMouse.AutoSize = true;
            this.cancelMouse.Location = new System.Drawing.Point(264, 120);
            this.cancelMouse.Name = "cancelMouse";
            this.cancelMouse.Size = new System.Drawing.Size(334, 24);
            this.cancelMouse.TabIndex = 8;
            this.cancelMouse.Text = "Cancel Mouse Events for selected buttons";
            this.cancelMouse.UseVisualStyleBackColor = true;
            this.cancelMouse.CheckedChanged += new System.EventHandler(this.cancelMouse_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Mute delay on release";
            // 
            // delay
            // 
            this.delay.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.delay.Location = new System.Drawing.Point(195, 7);
            this.delay.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.delay.Name = "delay";
            this.delay.Size = new System.Drawing.Size(97, 26);
            this.delay.TabIndex = 10;
            this.delay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.delay.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.delay.ValueChanged += new System.EventHandler(this.delay_ValueChanged);
            // 
            // msec
            // 
            this.msec.AutoSize = true;
            this.msec.Location = new System.Drawing.Point(314, 9);
            this.msec.Name = "msec";
            this.msec.Size = new System.Drawing.Size(47, 20);
            this.msec.TabIndex = 11;
            this.msec.Text = "msec";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 174);
            this.Controls.Add(this.msec);
            this.Controls.Add(this.delay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cancelMouse);
            this.Controls.Add(this.clearMouse);
            this.Controls.Add(this.clearKey);
            this.Controls.Add(this.setMouse);
            this.Controls.Add(this.setKey);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mouseText);
            this.Controls.Add(this.keyText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PushToTalk Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.delay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox keyText;
        private System.Windows.Forms.TextBox mouseText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button setKey;
        private System.Windows.Forms.Button setMouse;
        private System.Windows.Forms.Button clearKey;
        private System.Windows.Forms.Button clearMouse;
        private System.Windows.Forms.CheckBox cancelMouse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown delay;
        private System.Windows.Forms.Label msec;
    }
}

