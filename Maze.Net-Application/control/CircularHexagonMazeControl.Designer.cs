namespace Maze.TApplication
{
    partial class CircularHexagonControl
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
            rings = new System.Windows.Forms.NumericUpDown();
            label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)thickness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rings).BeginInit();
            SuspendLayout();
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(1, 28);
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
            thickness.Location = new System.Drawing.Point(89, 26);
            thickness.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            thickness.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            thickness.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            thickness.Name = "thickness";
            thickness.Size = new System.Drawing.Size(168, 23);
            thickness.TabIndex = 13;
            thickness.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // rings
            // 
            rings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            rings.Location = new System.Drawing.Point(89, 0);
            rings.Name = "rings";
            rings.Size = new System.Drawing.Size(168, 23);
            rings.TabIndex = 10;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(1, 2);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(40, 17);
            label1.TabIndex = 8;
            label1.Text = "Rings";
            // 
            // CircularHexagonControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(label10);
            Controls.Add(thickness);
            Controls.Add(rings);
            Controls.Add(label1);
            Name = "CircularHexagonControl";
            Size = new System.Drawing.Size(258, 55);
            ((System.ComponentModel.ISupportInitialize)thickness).EndInit();
            ((System.ComponentModel.ISupportInitialize)rings).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown thickness;
        private System.Windows.Forms.NumericUpDown rings;
        private System.Windows.Forms.Label label1;
    }
}
