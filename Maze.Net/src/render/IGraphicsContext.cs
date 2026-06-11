using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public struct MazePoint
    {
        public float X;
        public float Y;

        public MazePoint(float x, float y) { X = x; Y = y; }
    }

    public struct MazeSize
    {
        public float Width;
        public float Height;

        public MazeSize(float width, float height) { Width = width; Height = height; }
    }

    public struct MazeColor
    {
        public byte A;
        public byte R;
        public byte G;
        public byte B;

        public MazeColor(byte r, byte g, byte b) : this(255, r, g, b) { }

        public MazeColor(byte a, byte r, byte g, byte b) { A = a; R = r; G = g; B = b; }

        public static MazeColor Black => new MazeColor(0, 0, 0);
        public static MazeColor White => new MazeColor(255, 255, 255);
        public static MazeColor Red => new MazeColor(255, 0, 0);
        public static MazeColor Green => new MazeColor(0, 128, 0);
        public static MazeColor Yellow => new MazeColor(255, 255, 0);
    }

    public interface IGraphicsContext
    {
        void DrawLine(MazePoint a, MazePoint b, MazeColor color, double width);

        void DrawArc(MazePoint center, double radius, double startAngleDeg, double sweepAngleDeg, MazeColor color, double width);

        void FillRectangle(MazePoint pt, MazeSize size, MazeColor color);

        void FillPolygon(List<MazePoint> points, MazeColor color);

        void FillAnnularSector(MazePoint center, double outerRadius, double innerRadius, double startAngleDeg, double sweepAngleDeg, MazeColor color);

        void FillArcWedge(MazePoint center, double arcRadius, double startAngleDeg, double sweepAngleDeg, MazePoint closingPoint, MazeColor color);
    }
}
