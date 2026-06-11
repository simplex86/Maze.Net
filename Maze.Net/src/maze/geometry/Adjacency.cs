namespace SimplexLab.Maze
{
    internal class Adjacency
    {
        public int Neighbor { get; }

        public IMazeBorder? Border { get; internal set; }

        public bool IsOpen { get; internal set; }

        public Adjacency(int neighbor, IMazeBorder? border)
        {
            Neighbor = neighbor;
            Border = border;
        }
    }
}
