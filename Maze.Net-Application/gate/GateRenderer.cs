using System;
using System.Drawing;
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

        public GateRenderer<TField> SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            return this;
        }

        public GateRenderer<TField> SetThickness(int thickness)
        {
            this.thickness = thickness;
            return this;
        }

        public GateRenderer<TField> SetOffset(int x, int y)
        {
            this.offsetx = x;
            this.offsety = y;
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

        protected float TransformX(double x, CoordinateBounds bounds, float scale, float offsetX)
        {
            return (float)((x - bounds.MinX) * scale) + offsetX;
        }

        protected float TransformY(double y, CoordinateBounds bounds, float scale, float offsetY, bool flipY)
        {
            return flipY ? (float)((bounds.MaxY - y) * scale) + offsetY
                            : (float)((y - bounds.MinY) * scale) + offsetY;
        }
    }
}
