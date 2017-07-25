namespace CameraLib_Test
{
    partial class FormCameraControlTool
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
            this.comboBoxCameraList = new System.Windows.Forms.ComboBox();
            this.comboBoxResolutionList = new System.Windows.Forms.ComboBox();
            this.cameraControl = new Camera_NET.CameraControl();
            this.pictureBoxScreenshot = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chromaBox = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tolABox = new System.Windows.Forms.TextBox();
            this.tolBBox = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreenshot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chromaBox)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxCameraList
            // 
            this.comboBoxCameraList.FormattingEnabled = true;
            this.comboBoxCameraList.Location = new System.Drawing.Point(12, 12);
            this.comboBoxCameraList.Name = "comboBoxCameraList";
            this.comboBoxCameraList.Size = new System.Drawing.Size(121, 21);
            this.comboBoxCameraList.TabIndex = 0;
            this.comboBoxCameraList.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameraList_SelectedIndexChanged);
            // 
            // comboBoxResolutionList
            // 
            this.comboBoxResolutionList.FormattingEnabled = true;
            this.comboBoxResolutionList.Location = new System.Drawing.Point(12, 51);
            this.comboBoxResolutionList.Name = "comboBoxResolutionList";
            this.comboBoxResolutionList.Size = new System.Drawing.Size(121, 21);
            this.comboBoxResolutionList.TabIndex = 1;
            this.comboBoxResolutionList.SelectedIndexChanged += new System.EventHandler(this.comboBoxResolutionList_SelectedIndexChanged);
            // 
            // cameraControl
            // 
            this.cameraControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraControl.DirectShowLogFilepath = "";
            this.cameraControl.Location = new System.Drawing.Point(151, 12);
            this.cameraControl.MinimumSize = new System.Drawing.Size(1, 1);
            this.cameraControl.Name = "cameraControl";
            this.cameraControl.Size = new System.Drawing.Size(297, 243);
            this.cameraControl.TabIndex = 2;
            // 
            // pictureBoxScreenshot
            // 
            this.pictureBoxScreenshot.Location = new System.Drawing.Point(151, 310);
            this.pictureBoxScreenshot.Name = "pictureBoxScreenshot";
            this.pictureBoxScreenshot.Size = new System.Drawing.Size(400, 300);
            this.pictureBoxScreenshot.TabIndex = 3;
            this.pictureBoxScreenshot.TabStop = false;
            this.pictureBoxScreenshot.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxScreenshot_MouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chromaBox
            // 
            this.chromaBox.Location = new System.Drawing.Point(12, 126);
            this.chromaBox.Name = "chromaBox";
            this.chromaBox.Size = new System.Drawing.Size(47, 46);
            this.chromaBox.TabIndex = 5;
            this.chromaBox.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 232);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Do Chroma";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tolABox
            // 
            this.tolABox.Location = new System.Drawing.Point(12, 269);
            this.tolABox.Name = "tolABox";
            this.tolABox.Size = new System.Drawing.Size(45, 20);
            this.tolABox.TabIndex = 7;
            this.tolABox.Text = "40";
            // 
            // tolBBox
            // 
            this.tolBBox.Location = new System.Drawing.Point(12, 310);
            this.tolBBox.Name = "tolBBox";
            this.tolBBox.Size = new System.Drawing.Size(48, 20);
            this.tolBBox.TabIndex = 8;
            this.tolBBox.Text = "100";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 351);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 26);
            this.button3.TabIndex = 9;
            this.button3.Text = "Save Chroma";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 202);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(121, 24);
            this.button4.TabIndex = 10;
            this.button4.Text = "Select Chroma Image";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // FormCameraControlTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 630);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.tolBBox);
            this.Controls.Add(this.tolABox);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.chromaBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBoxScreenshot);
            this.Controls.Add(this.cameraControl);
            this.Controls.Add(this.comboBoxResolutionList);
            this.Controls.Add(this.comboBoxCameraList);
            this.Name = "FormCameraControlTool";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCameraControlTool_FormClosed);
            this.Load += new System.EventHandler(this.FormCameraControlTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxScreenshot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chromaBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxCameraList;
        private System.Windows.Forms.ComboBox comboBoxResolutionList;
        private Camera_NET.CameraControl cameraControl;
        private System.Windows.Forms.PictureBox pictureBoxScreenshot;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox chromaBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox tolABox;
        private System.Windows.Forms.TextBox tolBBox;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}

