using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StarcraftParser
{
    class GraphProcessor : Processor
    {
        public NodeList<ScEvent> buildTree(int counter, ScGame game, List<ScGame> games)
        {
            NodeList<ScEvent> result = new NodeList<ScEvent>();
            if (++counter < game.Events.Count)
            {
                ScEvent r = game.Events[counter];
                result.Add(new Node<ScEvent>(1, r, buildTree(counter, game, games)));
            }

            return result;
        }

        public string ExportToGraphviz(string result, NodeList<ScEvent> roots)
        {
            //digraph G {

            //    subgraph cluster_0 {
            //        style=filled;
            //        color=lightgrey;
            //        node [style=filled,color=white];
            //        a0 -> a1 -> a2 -> a3;
            //        label = "process #1";
            //    }

            //    subgraph cluster_1 {
            //        node [style=filled];
            //        b0 -> b1 -> b2 -> b3;
            //        label = "process #2";
            //        color=blue
            //    }
            //    start -> a0;
            //    start -> b0;
            //    a1 -> b3;
            //    b2 -> a3;
            //    a3 -> a0;
            //    a3 -> end;
            //    b3 -> end;

            //    start [shape=Mdiamond];
            //    end [shape=Msquare];
            //}

                // Writes the CSV Header
                //result += "digraph G {" + " \r\n";
            result = "";
                for(int i=0;i<roots.Count;i++)
                {
                    for (int j = 0; j < roots[i].Neighbors.Count; j++)
                    {
                        result += "start -> " + "\"" + roots[i].Value.Unit + "\"" + "; \r\n";
                        result += "\"" + roots[i].Value.Unit + "\"" + " -> " + "\"" + roots[i].Neighbors[j].Value.Unit + "\"" + "; \r\n";
                        string tmp = "";
                        //result += ExportToGraphviz(tmp, roots[i].Neighbors);
                    }

                }
                //result += "start [shape=Mdiamond];" + "\r\n" + "}" + " \r\n";
                return result;
            
        
        }

        public NodeList<ScEvent> ProcessGames(List<ScGame> games)
        {
            NodeList<ScEvent> roots = new NodeList<ScEvent>();
            NodeList<ScEvent> allgames = new NodeList<ScEvent>();
            foreach (ScGame game in games)
            {
                Node<ScEvent> node = new Node<ScEvent>(1, game.Events[0], buildTree(0, game, games));
                allgames.Add(node);

                long counter = 0;
                foreach (Node<ScEvent> root in roots)
                {
                    if (root.Value.Unit == node.Value.Unit)
                    {
                        counter++;
                        foreach (Node<ScEvent> n in node.Neighbors)
                        {
                            List<Node<ScEvent>> q = root.Neighbors.Where(e => e.Value.Unit == n.Value.Unit).ToList();
                            if (q.Count == 0) root.Neighbors.Add(n);
                        }
                    }
                }
                if (roots.Count == 0 || counter == 0) roots.Add(node);
            }

            CountOccurances(roots, allgames);
            return roots;
        }

        private void CountOccurances(NodeList<ScEvent> roots, NodeList<ScEvent> allgames)
        {
            foreach (Node<ScEvent> root in roots)
            {
                foreach (Node<ScEvent> game in allgames)
                {
                    if (root.Value.Unit == game.Value.Unit)
                    {
                        root.Occurances++;
                        CountOccurances(root.Neighbors, game.Neighbors);
                    }
                }
            }
        }
    }
}
