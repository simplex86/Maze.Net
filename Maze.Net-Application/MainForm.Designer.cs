using System.Drawing;
using System.Windows.Forms;

namespace Maze.TApplication
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            flowLayoutPanel1 = new FlowLayoutPanel();
            rectangularMazeControl = new RectangularMazeControl();
            circularMazeControl = new CircularMazeControl();
            algorithm = new ComboBox();
            label13 = new Label();
            shape = new ComboBox();
            label1 = new Label();
            generation = new Button();
            canvas = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(flowLayoutPanel1);
            splitContainer1.Panel1.Controls.Add(algorithm);
            splitContainer1.Panel1.Controls.Add(label13);
            splitContainer1.Panel1.Controls.Add(shape);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(generation);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(canvas);
            splitContainer1.Size = new Size(1184, 761);
            splitContainer1.SplitterDistance = 291;
            splitContainer1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(rectangularMazeControl);
            flowLayoutPanel1.Controls.Add(circularMazeControl);
            flowLayoutPanel1.Location = new Point(0, 72);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(291, 347);
            flowLayoutPanel1.TabIndex = 26;
            // 
            // rectangularMazeControl
            // 
            rectangularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rectangularMazeControl.Location = new Point(3, 3);
            rectangularMazeControl.Name = "rectangularMazeControl";
            rectangularMazeControl.Size = new Size(285, 82);
            rectangularMazeControl.TabIndex = 0;
            // 
            // circularMazeControl
            // 
            circularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            circularMazeControl.Location = new Point(3, 91);
            circularMazeControl.Name = "circularMazeControl";
            circularMazeControl.Size = new Size(285, 115);
            circularMazeControl.TabIndex = 1;
            // 
            // algorithm
            // 
            algorithm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            algorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithm.FormattingEnabled = true;
            algorithm.Items.AddRange(new object[] { "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder" });
            algorithm.Location = new Point(93, 42);
            algorithm.Margin = new Padding(2);
            algorithm.Name = "algorithm";
            algorithm.Size = new Size(195, 25);
            algorithm.TabIndex = 25;
            // 
            // label13
            // 
            label13.Location = new Point(5, 45);
            label13.Margin = new Padding(2, 0, 2, 0);
            label13.Name = "label13";
            label13.RightToLeft = RightToLeft.No;
            label13.Size = new Size(86, 17);
            label13.TabIndex = 24;
            label13.Text = "Algorithm";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // shape
            // 
            shape.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            shape.DropDownStyle = ComboBoxStyle.DropDownList;
            shape.FormattingEnabled = true;
            shape.Items.AddRange(new object[] { "Rectangular", "Circular" });
            shape.Location = new Point(93, 12);
            shape.Name = "shape";
            shape.Size = new Size(195, 25);
            shape.TabIndex = 2;
            shape.SelectedIndexChanged += OnShapeChangedHandler;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(5, 15);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 1;
            label1.Text = "Shape";
            // 
            // generation
            // 
            generation.Dock = DockStyle.Bottom;
            generation.Location = new Point(0, 691);
            generation.Name = "generation";
            generation.Size = new Size(291, 70);
            generation.TabIndex = 0;
            generation.Text = "Generate";
            generation.UseVisualStyleBackColor = true;
            generation.Click += OnGenerationClickedHandler;
            // 
            // canvas
            // 
            canvas.Dock = DockStyle.Fill;
            canvas.Location = new Point(0, 0);
            canvas.Name = "canvas";
            canvas.Size = new Size(889, 761);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += OnCanvasPaintHandler;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 761);
            Controls.Add(splitContainer1);
            MinimumSize = new Size(769, 578);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Maze Generator v0.7.66";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Label label1;
        private Button generation;
        private ComboBox shape;
        private PictureBox canvas;
        private Label label13;
        private ComboBox algorithm;
        private FlowLayoutPanel flowLayoutPanel1;
        private RectangularMazeControl rectangularMazeControl;
        private CircularMazeControl circularMazeControl;
    }
}
