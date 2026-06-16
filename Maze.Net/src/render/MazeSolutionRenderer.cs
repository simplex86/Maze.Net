using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class MazeSolutionRenderer
    {
        private MazeField field;
        private MazeSolution solution;
        private MazeGate gate;
        private CoordinateTransform transform = new CoordinateTransform();

        public MazeSolutionRenderer SetField(MazeField field)
        {
            this.field = field;
            return this;
        }

        public MazeSolutionRenderer SetSolution(MazeSolution solution)
        {
            this.solution = solution;
            return this;
        }

        public MazeSolutionRenderer SetGate(MazeGate gate)
        {
            this.gate = gate;
            return this;
        }

        public MazeSolutionRenderer SetSize(int width, int height)
        {
            transform.Width = width;
            transform.Height = height;
            return this;
        }

        public MazeSolutionRenderer SetThickness(int thickness)
        {
            transform.ScaleX = thickness;
            transform.ScaleY = thickness;
            return this;
        }

        public MazeSolutionRenderer SetThickness(float scaleX, float scaleY)
        {
            transform.ScaleX = scaleX;
            transform.ScaleY = scaleY;
            return this;
        }

        public MazeSolutionRenderer SetOffset(int dx, int dy)
        {
            transform.Dx = dx;
            transform.Dy = dy;
            return this;
        }

        public MazeSolutionRenderer SetPadding(int x, int y)
        {
            transform.PaddingX = x;
            transform.PaddingY = y;
            return this;
        }

        public void Draw(IGraphicsContext context)
        {
            if (field == null || field.VertexCount == 0) return;
            if (solution.Count < 2) return;

            var bounds = field.Bounds;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            var offsetx = transform.GetOffsetX(bounds);
            var offsety = transform.GetOffsetY(bounds);
            var flipy = field.FlipY;

            var points = new List<MazePoint>();

            foreach (var vertex in solution)
            {
                var centroid = ComputeCellCentroid(vertex);
                points.Add(new MazePoint(
                    transform.TransformX(centroid.X, bounds, offsetx),
                    transform.TransformY(centroid.Y, bounds, offsety, flipy)));
            }

            if (points.Count < 2) return;

            var width = Math.Max(2f, Math.Min(Math.Min(transform.ScaleX, transform.ScaleY), 2));
            for (int i = 1; i < points.Count; i++)
            {
                context.DrawLine(points[i - 1], points[i], MazeColor.Red, width);
            }
        }

        private Vertex ComputeCellCentroid(int vertex)
        {
            var shape = field.GetCellShape(vertex);

            switch (shape.Type)
            {
                case CellShapeType.AnnularSector:
                    return ComputeAnnularSectorCentroid(shape.Sector);
                case CellShapeType.CurvedTriangle:
                    return ComputeCurvedTriangleCentroid(shape.CurvedTriangle);
                default:
                    return ComputePolygonCentroid(vertex);
            }
        }

        private Vertex ComputeAnnularSectorCentroid(AnnularSector sector)
        {
            var r1 = sector.InnerRadius;
            var r2 = sector.OuterRadius;
            var theta = sector.SweepAngle;
            var midAngle = sector.StartAngle + theta / 2.0;

            double rBar;
            if (r1 > 0)
            {
                // 环形扇区质心径向距离: r̄ = (2/3) * (r2³ - r1³) / (r2² - r1²) * sin(θ/2) / (θ/2)
                rBar = (2.0 / 3.0) * (r2 * r2 * r2 - r1 * r1 * r1) / (r2 * r2 - r1 * r1)
                     * Math.Sin(theta / 2.0) / (theta / 2.0);
            }
            else
            {
                // 扇形质心径向距离: r̄ = (2r2/3) * sin(θ/2) / (θ/2)
                rBar = (2.0 * r2 / 3.0) * Math.Sin(theta / 2.0) / (theta / 2.0);
            }

            return new Vertex(rBar * Math.Cos(midAngle), rBar * Math.Sin(midAngle));
        }

        private Vertex ComputeCurvedTriangleCentroid(CurvedTriangle ct)
        {
            // 近似：使用弧线中点与对侧顶点的中间位置
            var midAngle = ct.ArcStartAngle + ct.ArcSweepAngle / 2.0;
            var midR = ct.ArcRadius * 2.0 / 3.0 * Math.Sin(ct.ArcSweepAngle / 2.0) / (ct.ArcSweepAngle / 2.0);
            var arcMidX = midR * Math.Cos(midAngle);
            var arcMidY = midR * Math.Sin(midAngle);

            double tipX, tipY;
            if (ct.Upward)
            {
                var innerAngle = ct.InnerAngle;
                tipX = ct.InnerRadius * Math.Cos(innerAngle);
                tipY = ct.InnerRadius * Math.Sin(innerAngle);
            }
            else
            {
                var outerAngle = ct.OuterAngle;
                tipX = ct.OuterRadius * Math.Cos(outerAngle);
                tipY = ct.OuterRadius * Math.Sin(outerAngle);
            }

            return new Vertex((arcMidX + tipX) / 2.0, (arcMidY + tipY) / 2.0);
        }

        private Vertex ComputePolygonCentroid(int vertex)
        {
            double sumX = 0, sumY = 0;
            int count = 0;

            foreach (var edge in field.Graph[vertex])
            {
                if (edge.Border is LineBorder line)
                {
                    sumX += line.X1 + line.X2;
                    sumY += line.Y1 + line.Y2;
                    count += 2;
                }
            }

            return count > 0 ? new Vertex(sumX / count, sumY / count) : new Vertex(0, 0);
        }
    }
}
