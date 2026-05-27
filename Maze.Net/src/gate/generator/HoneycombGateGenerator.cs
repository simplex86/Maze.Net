using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class HoneycombGateGenerator : GateGenerator<HoneycombMazeField>
    {
        public HoneycombGateGenerator() 
        { 
        
        }

        public HoneycombGateGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public override MazeGate Generate(HoneycombMazeField field)
        {
            var sides = new List<int>[6];
            for (int i = 0; i < 6; i++) sides[i] = new List<int>();

            for (int u = -field.Length + 1; u < field.Length; u++)
            {
                var (vmin, vmax) = field.VExtent(u);
                for (int v = vmin; v <= vmax; v++)
                {
                    var node = field.VertexIndex(u, v);
                    for (int n = 0; n < 6; n++)
                    {
                        var uu = u + HoneycombMazeField.Neighbors[n][0];
                        var vv = v + HoneycombMazeField.Neighbors[n][1];
                        if (!field.IsValidNode(uu, vv))
                            sides[n].Add(node);
                    }
                }
            }

            var pair = random.Next(3);
            var entranceSide = pair;
            var exitSide = pair + 3;

            if (random.Next(2) == 0)
                (entranceSide, exitSide) = (exitSide, entranceSide);

            if (sides[entranceSide].Count == 0 || sides[exitSide].Count == 0)
            {
                var edgeVertices = FindEdgeVertices(field);
                var entrance = edgeVertices[random.Next(edgeVertices.Count)];
                var exitCandidates = new List<int>(edgeVertices);
                if (exitCandidates.Count > 1) exitCandidates.Remove(entrance);
                var exit = exitCandidates[random.Next(exitCandidates.Count)];
                return new MazeGate(entrance, exit);
            }

            var gateEntrance = sides[entranceSide][random.Next(sides[entranceSide].Count)];
            var gateExit = sides[exitSide][random.Next(sides[exitSide].Count)];

            return new MazeGate(gateEntrance, gateExit);
        }

        private static List<int> FindEdgeVertices(HoneycombMazeField field)
        {
            var list = new List<int>();
            for (int v = 0; v < field.VertexCount; v++)
            {
                foreach (var edge in field.Graph[v])
                {
                    if (edge.Neighbor == -1)
                    {
                        list.Add(v);
                        break;
                    }
                }
            }
            return list;
        }
    }
}
