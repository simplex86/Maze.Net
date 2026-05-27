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
            circularHexagonMazeControl = new CircularHexagonControl();
            hexagonalMazeControl = new HexagonalMazeControl();
            triangularMazeControl = new TriangularMazeControl();
            honeycombMazeControl = new HoneycombMazeControl();
            circularMazeControl = new CircularMazeControl();
            rectangularMazeControl = new RectangularMazeControl();
            algorithm = new ComboBox();
            algorithmLabel = new Label();
            shape = new ComboBox();
            label1 = new Label();
            generation = new Button();
            canvas = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
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
            splitContainer1.Panel1.Controls.Add(circularHexagonMazeControl);
            splitContainer1.Panel1.Controls.Add(hexagonalMazeControl);
            splitContainer1.Panel1.Controls.Add(triangularMazeControl);
            splitContainer1.Panel1.Controls.Add(honeycombMazeControl);
            splitContainer1.Panel1.Controls.Add(circularMazeControl);
            splitContainer1.Panel1.Controls.Add(rectangularMazeControl);
            splitContainer1.Panel1.Controls.Add(algorithm);
            splitContainer1.Panel1.Controls.Add(algorithmLabel);
            splitContainer1.Panel1.Controls.Add(shape);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(generation);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(canvas);
            splitContainer1.Size = new Size(1004, 740);
            splitContainer1.SplitterDistance = 291;
            splitContainer1.TabIndex = 0;
            // 
            // circularHexagonMazeControl
            // 
            circularHexagonMazeControl.Location = new Point(6, 457);
            circularHexagonMazeControl.Name = "circularHexagonMazeControl";
            circularHexagonMazeControl.Size = new Size(284, 82);
            circularHexagonMazeControl.TabIndex = 29;
            // 
            // hexagonalMazeControl
            // 
            hexagonalMazeControl.Location = new Point(6, 393);
            hexagonalMazeControl.Name = "hexagonalMazeControl";
            hexagonalMazeControl.Size = new Size(284, 82);
            hexagonalMazeControl.TabIndex = 28;
            // 
            // triangularMazeControl
            // 
            triangularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            triangularMazeControl.Location = new Point(6, 308);
            triangularMazeControl.Name = "triangularMazeControl";
            triangularMazeControl.Size = new Size(284, 82);
            triangularMazeControl.TabIndex = 27;
            // 
            // honeycombMazeControl
            // 
            honeycombMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            honeycombMazeControl.Location = new Point(6, 252);
            honeycombMazeControl.Name = "honeycombMazeControl";
            honeycombMazeControl.Size = new Size(284, 82);
            honeycombMazeControl.TabIndex = 26;
            // 
            // circularMazeControl
            // 
            circularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            circularMazeControl.Location = new Point(6, 145);
            circularMazeControl.Name = "circularMazeControl";
            circularMazeControl.Size = new Size(284, 109);
            circularMazeControl.TabIndex = 1;
            // 
            // rectangularMazeControl
            // 
            rectangularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rectangularMazeControl.Location = new Point(6, 65);
            rectangularMazeControl.Name = "rectangularMazeControl";
            rectangularMazeControl.Size = new Size(284, 82);
            rectangularMazeControl.TabIndex = 0;
            // 
            // algorithm
            // 
            algorithm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            algorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithm.FormattingEnabled = true;
            algorithm.Items.AddRange(new object[] { "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder" });
            algorithm.Location = new Point(94, 37);
            algorithm.Margin = new Padding(2);
            algorithm.Name = "algorithm";
            algorithm.Size = new Size(196, 25);
            algorithm.TabIndex = 25;
            // 
            // algorithmLabel
            // 
            algorithmLabel.Location = new Point(7, 40);
            algorithmLabel.Margin = new Padding(2, 0, 2, 0);
            algorithmLabel.Name = "algorithmLabel";
            algorithmLabel.RightToLeft = RightToLeft.No;
            algorithmLabel.Size = new Size(86, 17);
            algorithmLabel.TabIndex = 24;
            algorithmLabel.Text = "Algorithm";
            algorithmLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // shape
            // 
            shape.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            shape.DropDownStyle = ComboBoxStyle.DropDownList;
            shape.FormattingEnabled = true;
            shape.Items.AddRange(new object[] { "Rectangular", "Circular", "Honeycomb", "Triangular", "Hexagonal", "CircularHexagon" });
            shape.Location = new Point(94, 9);
            shape.Name = "shape";
            shape.Size = new Size(196, 25);
            shape.TabIndex = 2;
            shape.SelectedIndexChanged += OnShapeChangedHandler;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 15);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 1;
            label1.Text = "Shape";
            // 
            // generation
            // 
            generation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            generation.BackColor = Color.SkyBlue;
            generation.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 134);
            generation.Location = new Point(0, 648);
            generation.Name = "generation";
            generation.Size = new Size(291, 90);
            generation.TabIndex = 0;
            generation.Text = "Generate";
            generation.UseVisualStyleBackColor = false;
            generation.Click += OnGenerationClickedHandler;
            // 
            // canvas
            // 
            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            canvas.BackColor = Color.White;
            canvas.Location = new Point(0, 0);
            canvas.Name = "canvas";
            canvas.Size = new Size(705, 736);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += OnCanvasPaintHandler;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1004, 740);
            Controls.Add(splitContainer1);
            MinimumSize = new Size(767, 573);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Maze Generator v0.8.85";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Label label1;
        private Button generation;
        private ComboBox shape;
        private PictureBox canvas;
        private Label algorithmLabel;
        private ComboBox algorithm;
        private RectangularMazeControl rectangularMazeControl;
        private CircularMazeControl circularMazeControl;
        private HoneycombMazeControl honeycombMazeControl;
        private TriangularMazeControl triangularMazeControl;
        private HexagonalMazeControl hexagonalMazeControl;
        private CircularHexagonControl circularHexagonMazeControl;
    }
}
