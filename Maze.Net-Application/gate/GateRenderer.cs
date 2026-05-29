using System;
using System.Drawing;
using System.Security.Cryptography.Xml;
using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal abstract class GateRenderer<TField> where TField : MazeField
    {
        protected int width;
        protected int height;
        protected int thickness;
        protected TField? field;
        protected MazeGate gate;
        protected int offsetx;
        protected int offsety;

        protected CoordinateTransform transform = new CoordinateTransform();

        public GateRenderer<TField> SetSize(int width, int height)
        {
            transform.width = width;
            transform.height = height;
            return this;
        }

        public GateRenderer<TField> SetThickness(int thickness)
        {
            transform.scale = thickness;
            return this;
        }

        public GateRenderer<TField> SetOffset(int dx, int dy)
        {
            transform.dx = dx;
            transform.dy = dy;
            return this;
        }

        public GateRenderer<TField> SetField(TField? field)
        {
            this.field = field;
            return this;
        }

        public GateRenderer<TField> SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public abstract void Draw(Graphics grap);
    }
}
