using System;
using System.Collections.Generic;

namespace SimplexLab.Maze
{
    public class HoneycombMazeGateGenerator : MazeGateGenerator<HoneycombMazeField>
    {
        public HoneycombMazeGateGenerator() 
        { 
        
        }

        public HoneycombMazeGateGenerator(Random random) 
            : base(random) 
        { 
        
        }

        public override MazeGate Generate(HoneycombMazeField field)
        {
            var L = field.Length;

            var sides = new List<int>[6];
            for (int i = 0; i < 6; i++) sides[i] = new List<int>();

            for (int u = -L + 1; u < L; u++)
            {
                var (vmin, vmax) = field.VExtent(u);
                for (int v = vmin; v <= vmax; v++)
                {
                    var onSides = new List<int>();
                    if (u == -L + 1) onSides.Add(0);
                    if (v == L - 1) onSides.Add(1);
                    if (u + v == L - 1) onSides.Add(2);
                    if (u == L - 1) onSides.Add(3);
                    if (v == -L + 1) onSides.Add(4);
                    if (u + v == -L + 1) onSides.Add(5);

                    if (onSides.Count != 1) continue;

                    var node = field.VertexIndex(u, v);
                    sides[onSides[0]].Add(node);
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
                return new MazeGate(entrance, exit)
                {
                    EntranceBorder = PickOuterBorder(field, entrance),
                    ExitBorder = PickOuterBorder(field, exit)
                };
            }

            var gateEntrance = sides[entranceSide][random.Next(sides[entranceSide].Count)];
            var gateExit = sides[exitSide][random.Next(sides[exitSide].Count)];

            return new MazeGate(gateEntrance, gateExit)
            {
                EntranceBorder = PickOuterBorder(field, gateEntrance),
                ExitBorder = PickOuterBorder(field, gateExit)
            };
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
