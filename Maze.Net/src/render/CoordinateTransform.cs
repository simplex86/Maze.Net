namespace SimplexLab.Maze
{
    internal class CoordinateTransform
    {
        public int Width;
        public int Height;
        public int Scale;
        public int Dx;
        public int Dy;

        public float GetOffsetX(CoordinateBounds bounds)
        {
            return (float)((Width - bounds.Width * Scale) / 2) + Dx;
        }

        public float GetOffsetY(CoordinateBounds bounds)
        {
            return (float)((Height - bounds.Height * Scale) / 2) + Dy;
        }

        public float TransformX(double x, CoordinateBounds bounds, float offsetx)
        {
            return (float)((x - bounds.MinX) * Scale) + offsetx;
        }

        public float TransformY(double y, CoordinateBounds bounds, float offsety, bool flipy)
        {
            return flipy ? (float)((bounds.MaxY - y) * Scale) + offsety
                         : (float)((y - bounds.MinY) * Scale) + offsety;
        }
    }
}