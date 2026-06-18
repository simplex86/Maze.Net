namespace Maze.TApplication
{
    partial class CustomizedMazeControl
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
            label1 = new System.Windows.Forms.Label();
            filename = new System.Windows.Forms.TextBox();
            brower = new System.Windows.Forms.Button();
            labelSamples = new System.Windows.Forms.Label();
            samples = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)thickness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)samples).BeginInit();
            SuspendLayout();
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(1, 30);
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
            thickness.Location = new System.Drawing.Point(90, 28);
            thickness.Margin = new System.Windows.Forms.Padding(2);
            thickness.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            thickness.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            thickness.Name = "thickness";
            thickness.Size = new System.Drawing.Size(168, 23);
            thickness.TabIndex = 13;
            thickness.Value = new decimal(new int[] { 9, 0, 0, 0 });
            // 
            // labelSamples
            // 
            labelSamples.Location = new System.Drawing.Point(1, 58);
            labelSamples.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            labelSamples.Name = "labelSamples";
            labelSamples.Size = new System.Drawing.Size(74, 17);
            labelSamples.TabIndex = 16;
            labelSamples.Text = "Samples";
            labelSamples.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // samples
            // 
            samples.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            samples.Location = new System.Drawing.Point(90, 56);
            samples.Margin = new System.Windows.Forms.Padding(2);
            samples.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            samples.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            samples.Name = "samples";
            samples.Size = new System.Drawing.Size(168, 23);
            samples.TabIndex = 17;
            samples.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(1, 2);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(69, 17);
            label1.TabIndex = 8;
            label1.Text = "Mask Path";
            // 
            // filename
            // 
            filename.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            filename.Location = new System.Drawing.Point(90, 0);
            filename.Name = "filename";
            filename.ReadOnly = true;
            filename.Size = new System.Drawing.Size(136, 23);
            filename.TabIndex = 14;
            // 
            // brower
            // 
            brower.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            brower.Location = new System.Drawing.Point(229, 0);
            brower.Name = "brower";
            brower.Size = new System.Drawing.Size(29, 23);
            brower.TabIndex = 15;
            brower.Text = "...";
            brower.UseVisualStyleBackColor = true;
            brower.Click += OnBrowerClickedHandler;
            // 
            // CustomizedMazeControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(brower);
            Controls.Add(filename);
            Controls.Add(label10);
            Controls.Add(thickness);
            Controls.Add(label1);
            Controls.Add(labelSamples);
            Controls.Add(samples);
            Name = "CustomizedMazeControl";
            Size = new System.Drawing.Size(258, 89);
            ((System.ComponentModel.ISupportInitialize)thickness).EndInit();
            ((System.ComponentModel.ISupportInitialize)samples).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown thickness;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filename;
        private System.Windows.Forms.Button brower;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.NumericUpDown samples;
    }
}
