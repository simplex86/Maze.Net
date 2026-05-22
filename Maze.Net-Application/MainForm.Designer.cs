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
            tabs = new TabControl();
            rectangularMazePage = new TabPage();
            rectangularMazeControl1 = new RectangularMazeControl();
            circularMazePage = new TabPage();
            circularMazeControl1 = new CircularMazeControl();
            tabs.SuspendLayout();
            rectangularMazePage.SuspendLayout();
            circularMazePage.SuspendLayout();
            SuspendLayout();
            // 
            // tabs
            // 
            tabs.Controls.Add(rectangularMazePage);
            tabs.Controls.Add(circularMazePage);
            tabs.Dock = DockStyle.Fill;
            tabs.Location = new Point(0, 0);
            tabs.Margin = new Padding(5, 4, 5, 4);
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(1558, 1024);
            tabs.TabIndex = 0;
            // 
            // rectangularMazePage
            // 
            rectangularMazePage.Controls.Add(rectangularMazeControl1);
            rectangularMazePage.Location = new Point(4, 33);
            rectangularMazePage.Margin = new Padding(5, 4, 5, 4);
            rectangularMazePage.Name = "rectangularMazePage";
            rectangularMazePage.Padding = new Padding(5, 4, 5, 4);
            rectangularMazePage.Size = new Size(1550, 987);
            rectangularMazePage.TabIndex = 0;
            rectangularMazePage.Text = "Rectangular Maze";
            rectangularMazePage.UseVisualStyleBackColor = true;
            // 
            // rectangularMazeControl1
            // 
            rectangularMazeControl1.Dock = DockStyle.Fill;
            rectangularMazeControl1.Location = new Point(5, 4);
            rectangularMazeControl1.Margin = new Padding(8, 6, 8, 6);
            rectangularMazeControl1.Name = "rectangularMazeControl1";
            rectangularMazeControl1.Size = new Size(1540, 979);
            rectangularMazeControl1.TabIndex = 3;
            // 
            // circularMazePage
            // 
            circularMazePage.Controls.Add(circularMazeControl1);
            circularMazePage.Location = new Point(4, 33);
            circularMazePage.Margin = new Padding(5, 4, 5, 4);
            circularMazePage.Name = "circularMazePage";
            circularMazePage.Padding = new Padding(5, 4, 5, 4);
            circularMazePage.Size = new Size(1890, 987);
            circularMazePage.TabIndex = 1;
            circularMazePage.Text = "Circular Maze";
            circularMazePage.UseVisualStyleBackColor = true;
            // 
            // circularMazeControl1
            // 
            circularMazeControl1.Dock = DockStyle.Fill;
            circularMazeControl1.Location = new Point(5, 4);
            circularMazeControl1.Margin = new Padding(8, 6, 8, 6);
            circularMazeControl1.Name = "circularMazeControl1";
            circularMazeControl1.Size = new Size(1880, 979);
            circularMazeControl1.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1558, 1024);
            Controls.Add(tabs);
            Margin = new Padding(5, 4, 5, 4);
            MinimumSize = new Size(1200, 800);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Maze Generator v0.6.25";
            tabs.ResumeLayout(false);
            rectangularMazePage.ResumeLayout(false);
            circularMazePage.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabs;
        private TabPage rectangularMazePage;
        private TabPage circularMazePage;
        private RectangularMazeControl rectangularMazeControl1;
        private CircularMazeControl circularMazeControl1;
    }
}
