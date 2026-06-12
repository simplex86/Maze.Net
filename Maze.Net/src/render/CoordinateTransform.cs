namespace SimplexLab.Maze
{
    internal class CoordinateTransform
    {
        public int Width;
        public int Height;
        public float ScaleX;
        public float ScaleY;
        public int Dx;
        public int Dy;
        public int PaddingX;
        public int PaddingY;

        public float GetOffsetX(CoordinateBounds bounds)
        {
            return (float)((Width - 2 * PaddingX - bounds.Width * ScaleX) / 2) + PaddingX + Dx;
        }

        public float GetOffsetY(CoordinateBounds bounds)
        {
            return (float)((Height - 2 * PaddingY - bounds.Height * ScaleY) / 2) + PaddingY + Dy;
        }

        public float TransformX(double x, CoordinateBounds bounds, float offsetx)
        {
            return (float)((x - bounds.MinX) * ScaleX) + offsetx;
        }

        public float TransformY(double y, CoordinateBounds bounds, float offsety, bool flipy)
        {
            return flipy ? (float)((bounds.MaxY - y) * ScaleY) + offsety
                         : (float)((y - bounds.MinY) * ScaleY) + offsety;
        }
    }
}
