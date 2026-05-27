namespace Maze.TApplication
{
    partial class RectangularMazeControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            label10 = new System.Windows.Forms.Label();
            thickness = new System.Windows.Forms.NumericUpDown();
            height = new System.Windows.Forms.NumericUpDown();
            width = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)thickness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)width).BeginInit();
            SuspendLayout();
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(1, 56);
            label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label10.Name = "label10";
            label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label10.Size = new System.Drawing.Size(74, 17);
            label10.TabIndex = 12;
            label10.Text = "Thickness";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // thickness
            // 
            thickness.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            thickness.Location = new System.Drawing.Point(89, 54);
            thickness.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            thickness.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            thickness.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            thickness.Name = "thickness";
            thickness.Size = new System.Drawing.Size(168, 23);
            thickness.TabIndex = 13;
            thickness.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // height
            // 
            height.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            height.Location = new System.Drawing.Point(89, 27);
            height.Name = "height";
            height.Size = new System.Drawing.Size(168, 23);
            height.TabIndex = 11;
            // 
            // width
            // 
            width.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            width.Location = new System.Drawing.Point(89, 0);
            width.Name = "width";
            width.Size = new System.Drawing.Size(168, 23);
            width.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(1, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(46, 17);
            label2.TabIndex = 9;
            label2.Text = "Height";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(1, 2);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 17);
            label1.TabIndex = 8;
            label1.Text = "Width";
            // 
            // RectangularMazeControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label10);
            Controls.Add(thickness);
            Controls.Add(height);
            Controls.Add(width);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "RectangularMazeControl";
            Size = new System.Drawing.Size(258, 82);
            ((System.ComponentModel.ISupportInitialize)thickness).EndInit();
            ((System.ComponentModel.ISupportInitialize)height).EndInit();
            ((System.ComponentModel.ISupportInitialize)width).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown thickness;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
