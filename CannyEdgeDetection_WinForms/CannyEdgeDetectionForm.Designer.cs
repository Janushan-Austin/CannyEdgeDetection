
namespace CannyEdgeDetection
{
    partial class CannyEdgeDetectionForm
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
            this.LoadImageButton = new System.Windows.Forms.Button();
            this.ImagePanel = new System.Windows.Forms.Panel();
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.DetectEdgesButton = new System.Windows.Forms.Button();
            this.ImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadImageButton
            // 
            this.LoadImageButton.Location = new System.Drawing.Point(12, 12);
            this.LoadImageButton.Name = "LoadImageButton";
            this.LoadImageButton.Size = new System.Drawing.Size(117, 25);
            this.LoadImageButton.TabIndex = 2;
            this.LoadImageButton.Text = "Load Image";
            this.LoadImageButton.UseVisualStyleBackColor = true;
            this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
            // 
            // ImagePanel
            // 
            this.ImagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagePanel.AutoScroll = true;
            this.ImagePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ImagePanel.Controls.Add(this.ImagePictureBox);
            this.ImagePanel.Location = new System.Drawing.Point(137, 12);
            this.ImagePanel.Name = "ImagePanel";
            this.ImagePanel.Size = new System.Drawing.Size(689, 773);
            this.ImagePanel.TabIndex = 18;
            // 
            // ImagePictureBox
            // 
            this.ImagePictureBox.Location = new System.Drawing.Point(0, 0);
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.Size = new System.Drawing.Size(686, 770);
            this.ImagePictureBox.TabIndex = 0;
            this.ImagePictureBox.TabStop = false;
            // 
            // DetectEdgesButton
            // 
            this.DetectEdgesButton.Location = new System.Drawing.Point(12, 44);
            this.DetectEdgesButton.Name = "DetectEdgesButton";
            this.DetectEdgesButton.Size = new System.Drawing.Size(117, 28);
            this.DetectEdgesButton.TabIndex = 19;
            this.DetectEdgesButton.Text = "Detect Edges";
            this.DetectEdgesButton.UseVisualStyleBackColor = true;
            this.DetectEdgesButton.Click += new System.EventHandler(this.DetectEdgesButton_Click);
            // 
            // CannyEdgeDetectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(842, 797);
            this.Controls.Add(this.DetectEdgesButton);
            this.Controls.Add(this.ImagePanel);
            this.Controls.Add(this.LoadImageButton);
            this.Name = "CannyEdgeDetectionForm";
            this.Text = "Canny Edge Detection";
            this.ImagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Panel ImagePanel;
        private System.Windows.Forms.PictureBox ImagePictureBox;
        private System.Windows.Forms.Button DetectEdgesButton;
    }
}

