namespace FaceController
{
    partial class DebugPanel
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
            this.frame = new Emgu.CV.UI.ImageBox();
            this.diffFrame = new Emgu.CV.UI.ImageBox();
            this.helperBox1 = new Emgu.CV.UI.ImageBox();
            this.helperBox2 = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.frame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helperBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helperBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // frame
            // 
            this.frame.Location = new System.Drawing.Point(12, 12);
            this.frame.Name = "frame";
            this.frame.Size = new System.Drawing.Size(431, 545);
            this.frame.TabIndex = 3;
            this.frame.TabStop = false;
            // 
            // diffFrame
            // 
            this.diffFrame.Location = new System.Drawing.Point(449, 12);
            this.diffFrame.Name = "diffFrame";
            this.diffFrame.Size = new System.Drawing.Size(431, 545);
            this.diffFrame.TabIndex = 4;
            this.diffFrame.TabStop = false;
            // 
            // helperBox1
            // 
            this.helperBox1.Location = new System.Drawing.Point(886, 300);
            this.helperBox1.Name = "helperBox1";
            this.helperBox1.Size = new System.Drawing.Size(283, 257);
            this.helperBox1.TabIndex = 5;
            this.helperBox1.TabStop = false;
            // 
            // helperBox2
            // 
            this.helperBox2.Location = new System.Drawing.Point(886, 12);
            this.helperBox2.Name = "helperBox2";
            this.helperBox2.Size = new System.Drawing.Size(283, 282);
            this.helperBox2.TabIndex = 6;
            this.helperBox2.TabStop = false;
            // 
            // DebugPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1181, 795);
            this.Controls.Add(this.helperBox2);
            this.Controls.Add(this.helperBox1);
            this.Controls.Add(this.diffFrame);
            this.Controls.Add(this.frame);
            this.Name = "DebugPanel";
            this.Text = "DebugPanel";
            ((System.ComponentModel.ISupportInitialize)(this.frame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diffFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helperBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helperBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox frame;
        private Emgu.CV.UI.ImageBox diffFrame;
        private Emgu.CV.UI.ImageBox helperBox1;
        private Emgu.CV.UI.ImageBox helperBox2;
    }
}