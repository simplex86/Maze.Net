using SimplexLab.Maze;

namespace Maze.TApplication
{
    internal class CoordinateTransform
    {
        public int width;
        public int height;
        public int scale;
        public int dx;
        public int dy;

        public float GetOffsetX(CoordinateBounds bounds)
        {
            return (float)((width - bounds.Width * scale) / 2) + dx;
        }

        public float GetOffsetY(CoordinateBounds bounds)
        {
            return (float)((height - bounds.Height * scale) / 2) + dy;
        }

        public float TransformX(double x, CoordinateBounds bounds, float offsetx)
        {
            return (float)((x - bounds.MinX) * scale) + offsetx;
        }

        /// <summary>
        /// Y坐标变换：field坐标 → 屏幕坐标
        /// </summary>
        public float TransformY(double y, CoordinateBounds bounds, float offsety, bool flipy)
        {
            return flipy ? (float)((bounds.MaxY - y) * scale) + offsety
                         : (float)((y - bounds.MinY) * scale) + offsety;
        }
    }
}
