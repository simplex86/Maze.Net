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
            canvas = new System.Windows.Forms.PictureBox();
            generation = new System.Windows.Forms.Button();
            label10 = new System.Windows.Forms.Label();
            thickness = new System.Windows.Forms.NumericUpDown();
            sectors = new System.Windows.Forms.NumericUpDown();
            rings = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            algorithm = new System.Windows.Forms.ComboBox();
            label13 = new System.Windows.Forms.Label();
            strategy = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thickness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sectors).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rings).BeginInit();
            SuspendLayout();
            // 
            // canvas
            // 
            canvas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            canvas.Location = new System.Drawing.Point(343, 4);
            canvas.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            canvas.Name = "canvas";
            canvas.Size = new System.Drawing.Size(831, 810);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += OnCanvasPaintHandler;
            // 
            // generation
            // 
            generation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            generation.Location = new System.Drawing.Point(5, 723);
            generation.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            generation.Name = "generation";
            generation.Size = new System.Drawing.Size(330, 92);
            generation.TabIndex = 1;
            generation.Text = "Generate";
            generation.UseVisualStyleBackColor = true;
            generation.Click += OnGenerationClickedHandler;
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(5, 167);
            label10.Name = "label10";
            label10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label10.Size = new System.Drawing.Size(116, 24);
            label10.TabIndex = 12;
            label10.Text = "Thickness";
            label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // thickness
            // 
            thickness.Location = new System.Drawing.Point(146, 164);
            thickness.Minimum = new decimal(new int[] { 15, 0, 0, 0 });
            thickness.Name = "thickness";
            thickness.Size = new System.Drawing.Size(189, 30);
            thickness.TabIndex = 13;
            thickness.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // sectors
            // 
            sectors.Location = new System.Drawing.Point(146, 83);
            sectors.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            sectors.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
            sectors.Minimum = new decimal(new int[] { 3, 0, 0, 0 });
            sectors.Name = "sectors";
            sectors.Size = new System.Drawing.Size(189, 30);
            sectors.TabIndex = 11;
            sectors.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // rings
            // 
            rings.Location = new System.Drawing.Point(146, 45);
            rings.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            rings.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            rings.Name = "rings";
            rings.Size = new System.Drawing.Size(189, 30);
            rings.TabIndex = 10;
            rings.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(5, 86);
            label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(72, 24);
            label2.TabIndex = 9;
            label2.Text = "Sectors";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(5, 48);
            label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(58, 24);
            label1.TabIndex = 8;
            label1.Text = "Rings";
            // 
            // algorithm
            // 
            algorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            algorithm.FormattingEnabled = true;
            algorithm.Items.AddRange(new object[] { "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder" });
            algorithm.Location = new System.Drawing.Point(146, 4);
            algorithm.Name = "algorithm";
            algorithm.Size = new System.Drawing.Size(186, 32);
            algorithm.TabIndex = 23;
            // 
            // label13
            // 
            label13.Location = new System.Drawing.Point(5, 8);
            label13.Name = "label13";
            label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label13.Size = new System.Drawing.Size(135, 24);
            label13.TabIndex = 22;
            label13.Text = "Algorithm";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // strategy
            // 
            strategy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            strategy.FormattingEnabled = true;
            strategy.Items.AddRange(new object[] { "Arc", "Area" });
            strategy.Location = new System.Drawing.Point(146, 123);
            strategy.Name = "strategy";
            strategy.Size = new System.Drawing.Size(186, 32);
            strategy.TabIndex = 25;
            // 
            // label3
            // 
            label3.Location = new System.Drawing.Point(5, 127);
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
            Controls.Add(algorithm);
            Controls.Add(label13);
            Controls.Add(label10);
            Controls.Add(thickness);
            Controls.Add(sectors);
            Controls.Add(rings);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(generation);
            Controls.Add(canvas);
            Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            Name = "CircularMazeControl";
            Size = new System.Drawing.Size(1179, 819);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)thickness).EndInit();
            ((System.ComponentModel.ISupportInitialize)sectors).EndInit();
            ((System.ComponentModel.ISupportInitialize)rings).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button generation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown thickness;
        private System.Windows.Forms.NumericUpDown sectors;
        private System.Windows.Forms.NumericUpDown rings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox algorithm;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox strategy;
        private System.Windows.Forms.Label label3;
    }
}
