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
            canvas = new System.Windows.Forms.PictureBox();
            generation = new System.Windows.Forms.Button();
            label10 = new System.Windows.Forms.Label();
            thickness = new System.Windows.Forms.NumericUpDown();
            height = new System.Windows.Forms.NumericUpDown();
            width = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            algorithm = new System.Windows.Forms.ComboBox();
            label13 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thickness).BeginInit();
            ((System.ComponentModel.ISupportInitialize)height).BeginInit();
            ((System.ComponentModel.ISupportInitialize)width).BeginInit();
            SuspendLayout();
            // 
            // canvas
            // 
            canvas.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            canvas.Location = new System.Drawing.Point(218, 3);
            canvas.Name = "canvas";
            canvas.Size = new System.Drawing.Size(529, 574);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += OnCanvasPaintHandler;
            // 
            // generation
            // 
            generation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            generation.Location = new System.Drawing.Point(3, 512);
            generation.Name = "generation";
            generation.Size = new System.Drawing.Size(210, 65);
            generation.TabIndex = 1;
            generation.Text = "Generate";
            generation.UseVisualStyleBackColor = true;
            generation.Click += OnGenerationClickedHandler;
            // 
            // label10
            // 
            label10.Location = new System.Drawing.Point(3, 89);
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
            thickness.Location = new System.Drawing.Point(93, 87);
            thickness.Margin = new System.Windows.Forms.Padding(2);
            thickness.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            thickness.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            thickness.Name = "thickness";
            thickness.Size = new System.Drawing.Size(120, 23);
            thickness.TabIndex = 13;
            thickness.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // height
            // 
            height.Location = new System.Drawing.Point(93, 60);
            height.Name = "height";
            height.Size = new System.Drawing.Size(120, 23);
            height.TabIndex = 11;
            // 
            // width
            // 
            width.Location = new System.Drawing.Point(93, 33);
            width.Name = "width";
            width.Size = new System.Drawing.Size(120, 23);
            width.TabIndex = 10;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 62);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(46, 17);
            label2.TabIndex = 9;
            label2.Text = "Height";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(3, 35);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 17);
            label1.TabIndex = 8;
            label1.Text = "Width";
            // 
            // algorithm
            // 
            algorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            algorithm.FormattingEnabled = true;
            algorithm.Items.AddRange(new object[] { "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder" });
            algorithm.Location = new System.Drawing.Point(93, 3);
            algorithm.Margin = new System.Windows.Forms.Padding(2);
            algorithm.Name = "algorithm";
            algorithm.Size = new System.Drawing.Size(120, 25);
            algorithm.TabIndex = 23;
            // 
            // label13
            // 
            label13.Location = new System.Drawing.Point(3, 6);
            label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            label13.Size = new System.Drawing.Size(86, 17);
            label13.TabIndex = 22;
            label13.Text = "Algorithm";
            label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RectangularMazeControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(algorithm);
            Controls.Add(label13);
            Controls.Add(label10);
            Controls.Add(thickness);
            Controls.Add(height);
            Controls.Add(width);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(generation);
            Controls.Add(canvas);
            Name = "RectangularMazeControl";
            Size = new System.Drawing.Size(750, 580);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ((System.ComponentModel.ISupportInitialize)thickness).EndInit();
            ((System.ComponentModel.ISupportInitialize)height).EndInit();
            ((System.ComponentModel.ISupportInitialize)width).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Button generation;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown thickness;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox algorithm;
        private System.Windows.Forms.Label label13;
    }
}
