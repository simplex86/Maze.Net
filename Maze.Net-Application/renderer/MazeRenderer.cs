using System;
using System.Collections.Generic;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal abstract class MazeRenderer<TField> where TField : MazeField
    {
        protected int width;
        protected int height;
        protected int thickness;
        protected int offsetx;
        protected int offsety;

        public MazeRenderer<TField> SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        public MazeRenderer<TField> SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        public MazeRenderer<TField> SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
            return this;
        }

        public void Draw(Graphics grap, TField? field)
        {
            DrawBackground(grap);

            if (field == null) return;
            DrawField(grap, field);
        }

        private void DrawBackground(Graphics grap)
        {
            var brush = new SolidBrush(Color.White);
            grap.FillRectangle(brush, 0, 0, width, height);
            brush.Dispose();
        }

        protected abstract void DrawField(Graphics grap, TField field);

        protected void IterateBorders(TField field, Action<IMazeBorder> onBorder)
        {
            var graph = field.graph;
            for (int v = 0; v < graph.Count; v++)
            {
                foreach (var edge in graph[v])
                {
                    if (edge.Neighbor != -1 && edge.Neighbor <= v)
                        continue;

                    if (edge.Border != null)
                        onBorder(edge.Border);
                }
            }
        }
    }
}
