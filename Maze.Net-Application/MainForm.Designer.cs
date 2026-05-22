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
            tabs.Name = "tabs";
            tabs.SelectedIndex = 0;
            tabs.Size = new Size(1478, 978);
            tabs.TabIndex = 0;
            // 
            // rectangularMazePage
            // 
            rectangularMazePage.Controls.Add(rectangularMazeControl1);
            rectangularMazePage.Location = new Point(4, 26);
            rectangularMazePage.Name = "rectangularMazePage";
            rectangularMazePage.Padding = new Padding(3);
            rectangularMazePage.Size = new Size(1470, 948);
            rectangularMazePage.TabIndex = 0;
            rectangularMazePage.Text = "Rectangular Maze";
            rectangularMazePage.UseVisualStyleBackColor = true;
            // 
            // rectangularMazeControl1
            // 
            rectangularMazeControl1.Dock = DockStyle.Fill;
            rectangularMazeControl1.Location = new Point(3, 3);
            rectangularMazeControl1.Name = "rectangularMazeControl1";
            rectangularMazeControl1.Size = new Size(1464, 942);
            rectangularMazeControl1.TabIndex = 3;
            // 
            // circularMazePage
            // 
            circularMazePage.Controls.Add(circularMazeControl1);
            circularMazePage.Location = new Point(4, 26);
            circularMazePage.Name = "circularMazePage";
            circularMazePage.Padding = new Padding(3);
            circularMazePage.Size = new Size(1470, 948);
            circularMazePage.TabIndex = 1;
            circularMazePage.Text = "Circular Maze";
            circularMazePage.UseVisualStyleBackColor = true;
            // 
            // circularMazeControl1
            // 
            circularMazeControl1.Dock = DockStyle.Fill;
            circularMazeControl1.Location = new Point(3, 3);
            circularMazeControl1.Name = "circularMazeControl1";
            circularMazeControl1.Size = new Size(1464, 942);
            circularMazeControl1.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1478, 978);
            Controls.Add(tabs);
            MinimumSize = new Size(1276, 710);
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
