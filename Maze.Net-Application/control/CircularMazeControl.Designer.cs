namespace Maze.TApplication
{
    partial class CircularMazeControl
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
            sectors = new System.Windows.Forms.NumericUpDown();
            rings = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            strategy = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)thickness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sectors).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rings).BeginInit();
            SuspendLayout();
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(0, 114);
            label10.Name = "label10";
            label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label10.Size = new System.Drawing.Size(116, 24);
            label10.TabIndex = 12;
            label10.Text = "Thickness";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // thickness
            // 
            thickness.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            thickness.Location = new System.Drawing.Point(140, 112);
            thickness.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            thickness.Name = "thickness";
            thickness.Size = new System.Drawing.Size(264, 30);
            thickness.TabIndex = 13;
            thickness.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // sectors
            // 
            sectors.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            sectors.Location = new System.Drawing.Point(140, 38);
            sectors.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            sectors.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            sectors.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            sectors.Name = "sectors";
            sectors.Size = new System.Drawing.Size(264, 30);
            sectors.TabIndex = 11;
            sectors.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // rings
            // 
            rings.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            rings.Location = new System.Drawing.Point(140, 0);
            rings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            rings.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            rings.Name = "rings";
            rings.Size = new System.Drawing.Size(264, 30);
            rings.TabIndex = 10;
            rings.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(0, 41);
            label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 24);
            label2.TabIndex = 9;
            label2.Text = "Sectors";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(0, 3);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(58, 24);
            label1.TabIndex = 8;
            label1.Text = "Rings";
            // 
            // strategy
            // 
            strategy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            strategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            strategy.FormattingEnabled = true;
            strategy.Items.AddRange(new object[] { "Arc", "Area" });
            strategy.Location = new System.Drawing.Point(140, 74);
            strategy.Name = "strategy";
            strategy.Size = new System.Drawing.Size(264, 32);
            strategy.TabIndex = 25;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(0, 77);
            label3.Name = "label3";
            label3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label3.Size = new System.Drawing.Size(135, 24);
            label3.TabIndex = 24;
            label3.Text = "Strategy";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CircularMazeControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(strategy);
            Controls.Add(label3);
            Controls.Add(label10);
            Controls.Add(thickness);
            Controls.Add(sectors);
            Controls.Add(rings);
            Controls.Add(label2);
            Controls.Add(label1);
            Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            Name = "CircularMazeControl";
            Size = new System.Drawing.Size(405, 151);
            ((System.ComponentModel.ISupportInitialize)thickness).EndInit();
            ((System.ComponentModel.ISupportInitialize)sectors).EndInit();
            ((System.ComponentModel.ISupportInitialize)rings).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown thickness;
        private System.Windows.Forms.NumericUpDown sectors;
        private System.Windows.Forms.NumericUpDown rings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox strategy;
        private System.Windows.Forms.Label label3;
    }
}
