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
            customizedMazeControl = new CustomizedMazeControl();
            stairwayMazeControl = new StairwayMazeControl();
            circularHexagonMazeControl = new CircularHexagonControl();
            hexagonalMazeControl = new HexagonalMazeControl();
            triangularMazeControl = new TriangularMazeControl();
            honeycombMazeControl = new HoneycombMazeControl();
            circularMazeControl = new CircularMazeControl();
            rectangularMazeControl = new RectangularMazeControl();
            algorithm = new ComboBox();
            algorithmLabel = new Label();
            shape = new ComboBox();
            shapeLabel = new Label();
            generation = new Button();
            showSolution = new CheckBox();
            showMarkers = new CheckBox();
            canvas = new PictureBox();
            showGates = new CheckBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)canvas).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // customizedMazeControl
            // 
            customizedMazeControl.Location = new Point(3, 548);
            customizedMazeControl.Name = "customizedMazeControl";
            customizedMazeControl.Size = new Size(284, 61);
            customizedMazeControl.TabIndex = 31;
            // 
            // stairwayMazeControl
            // 
            stairwayMazeControl.Location = new Point(3, 479);
            stairwayMazeControl.Name = "stairwayMazeControl";
            stairwayMazeControl.Size = new Size(284, 63);
            stairwayMazeControl.TabIndex = 30;
            // 
            // circularHexagonMazeControl
            // 
            circularHexagonMazeControl.Location = new Point(3, 410);
            circularHexagonMazeControl.Name = "circularHexagonMazeControl";
            circularHexagonMazeControl.Size = new Size(284, 63);
            circularHexagonMazeControl.TabIndex = 29;
            // 
            // hexagonalMazeControl
            // 
            hexagonalMazeControl.Location = new Point(3, 343);
            hexagonalMazeControl.Name = "hexagonalMazeControl";
            hexagonalMazeControl.Size = new Size(284, 61);
            hexagonalMazeControl.TabIndex = 28;
            // 
            // triangularMazeControl
            // 
            triangularMazeControl.Location = new Point(3, 255);
            triangularMazeControl.Name = "triangularMazeControl";
            triangularMazeControl.Size = new Size(284, 82);
            triangularMazeControl.TabIndex = 27;
            // 
            // honeycombMazeControl
            // 
            honeycombMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            honeycombMazeControl.Location = new Point(3, 187);
            honeycombMazeControl.Name = "honeycombMazeControl";
            honeycombMazeControl.Size = new Size(284, 62);
            honeycombMazeControl.TabIndex = 26;
            // 
            // circularMazeControl
            // 
            circularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            circularMazeControl.Location = new Point(3, 91);
            circularMazeControl.Name = "circularMazeControl";
            circularMazeControl.Size = new Size(284, 90);
            circularMazeControl.TabIndex = 1;
            // 
            // rectangularMazeControl
            // 
            rectangularMazeControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            rectangularMazeControl.Location = new Point(3, 3);
            rectangularMazeControl.Name = "rectangularMazeControl";
            rectangularMazeControl.Size = new Size(284, 82);
            rectangularMazeControl.TabIndex = 0;
            // 
            // algorithm
            // 
            algorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            algorithm.FormattingEnabled = true;
            algorithm.Items.AddRange(new object[] { "DFS", "BFS", "Prim", "Kruskal", "Wilson", "Eller", "AldousBroder", "HuntAndKill" });
            algorithm.Location = new Point(95, 31);
            algorithm.Margin = new Padding(2);
            algorithm.Name = "algorithm";
            algorithm.Size = new Size(194, 25);
            algorithm.TabIndex = 25;
            // 
            // algorithmLabel
            // 
            algorithmLabel.Location = new Point(7, 34);
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
            shape.DropDownStyle = ComboBoxStyle.DropDownList;
            shape.FormattingEnabled = true;
            shape.Items.AddRange(new object[] { "Rectangular", "Circular", "Honeycomb", "Triangular", "Hexagonal", "CircularHexagon", "Stairway", "Customized" });
            shape.Location = new Point(95, 3);
            shape.Name = "shape";
            shape.Size = new Size(194, 25);
            shape.TabIndex = 2;
            shape.SelectedIndexChanged += OnShapeChangedHandler;
            // 
            // shapeLabel
            // 
            shapeLabel.AutoSize = true;
            shapeLabel.Location = new Point(7, 9);
            shapeLabel.Name = "shapeLabel";
            shapeLabel.Size = new Size(44, 17);
            shapeLabel.TabIndex = 1;
            shapeLabel.Text = "Shape";
            // 
            // generation
            // 
            generation.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            generation.BackColor = Color.SkyBlue;
            generation.Font = new Font("Microsoft YaHei UI", 16F, FontStyle.Regular, GraphicsUnit.Point, 134);
            generation.Location = new Point(3, 689);
            generation.Name = "generation";
            generation.Size = new Size(287, 90);
            generation.TabIndex = 0;
            generation.Text = "Generate";
            generation.UseVisualStyleBackColor = false;
            generation.Click += OnGenerationClickedHandler;
            // 
            // showSolution
            // 
            showSolution.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            showSolution.AutoSize = true;
            showSolution.Location = new Point(834, 758);
            showSolution.Name = "showSolution";
            showSolution.Size = new Size(131, 21);
            showSolution.TabIndex = 32;
            showSolution.Text = "Show the Solution";
            showSolution.UseVisualStyleBackColor = true;
            showSolution.CheckedChanged += OnSolutionChangedHandler;
            // 
            // showMarkers
            // 
            showMarkers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            showMarkers.AutoSize = true;
            showMarkers.Location = new Point(496, 758);
            showMarkers.Name = "showMarkers";
            showMarkers.Size = new Size(237, 21);
            showMarkers.TabIndex = 31;
            showMarkers.Text = "Show the Entrance and Exit Markers";
            showMarkers.UseVisualStyleBackColor = true;
            showMarkers.CheckedChanged += OnMarkersChangedHandler;
            // 
            // canvas
            // 
            canvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            canvas.BackColor = Color.White;
            canvas.Location = new Point(296, 3);
            canvas.Name = "canvas";
            canvas.Size = new Size(885, 749);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += OnCanvasPaintHandler;
            // 
            // showGates
            // 
            showGates.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            showGates.AutoSize = true;
            showGates.Checked = true;
            showGates.CheckState = CheckState.Checked;
            showGates.Location = new Point(295, 758);
            showGates.Name = "showGates";
            showGates.Size = new Size(184, 21);
            showGates.TabIndex = 30;
            showGates.Text = "Show the Entrance and Exit";
            showGates.UseVisualStyleBackColor = true;
            showGates.CheckedChanged += OnGatesChangedHandler;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            flowLayoutPanel1.Controls.Add(rectangularMazeControl);
            flowLayoutPanel1.Controls.Add(circularMazeControl);
            flowLayoutPanel1.Controls.Add(honeycombMazeControl);
            flowLayoutPanel1.Controls.Add(triangularMazeControl);
            flowLayoutPanel1.Controls.Add(hexagonalMazeControl);
            flowLayoutPanel1.Controls.Add(circularHexagonMazeControl);
            flowLayoutPanel1.Controls.Add(stairwayMazeControl);
            flowLayoutPanel1.Controls.Add(customizedMazeControl);
            flowLayoutPanel1.Location = new Point(3, 57);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(291, 622);
            flowLayoutPanel1.TabIndex = 33;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1184, 781);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(canvas);
            Controls.Add(showMarkers);
            Controls.Add(showSolution);
            Controls.Add(showGates);
            Controls.Add(generation);
            Controls.Add(shapeLabel);
            Controls.Add(shape);
            Controls.Add(algorithm);
            Controls.Add(algorithmLabel);
            MinimumSize = new Size(1080, 780);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Maze Generator v1.3.123";
            ((System.ComponentModel.ISupportInitialize)canvas).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label shapeLabel;
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
        private StairwayMazeControl stairwayMazeControl;
        private CheckBox showGates;
        private CheckBox showMarkers;
        private CheckBox showSolution;
        private CustomizedMazeControl customizedMazeControl;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
