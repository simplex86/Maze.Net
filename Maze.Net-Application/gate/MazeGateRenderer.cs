using System;
using System.Drawing;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal abstract class MazeGateRenderer<TField> where TField : MazeField
    {
        protected TField? field;
        protected MazeGate gate;
        protected CoordinateTransform transform = new CoordinateTransform();

        public MazeGateRenderer<TField> SetSize(int width, int height)
        {
            transform.width = width;
            transform.height = height;
            return this;
        }

        public MazeGateRenderer<TField> SetThickness(int thickness)
        {
            transform.scale = thickness;
            return this;
        }

        public MazeGateRenderer<TField> SetOffset(int dx, int dy)
        {
            transform.dx = dx;
            transform.dy = dy;
            return this;
        }

        public MazeGateRenderer<TField> SetField(TField? field)
        {
            this.field = field;
            return this;
        }

        public MazeGateRenderer<TField> SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public abstract void Draw(Graphics grap);
    }
}
