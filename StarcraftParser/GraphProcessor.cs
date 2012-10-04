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
            long counter = 0;
            result = "";
            foreach (Node<ScEvent> root in roots)
            {
                counter++;
                foreach (Node<ScEvent> node in root.Neighbors)
                {
                    result += "\"" + root.Value.Unit + "\"" + " -> " + "\"" + node.Value.Unit + "\"" + "; \r\n";
                    result += ExportToGraphviz(result, root.Neighbors);
                }
            }
            return result;
        }

        public string ExportToGraphvizSimple(string result, NodeList<ScEvent> roots)
        {
            result = "";
            foreach (Node<ScEvent> root in roots)
            {
                foreach (Node<ScEvent> node in root.Neighbors)
                {
                    result += "\"" + root.Value.Unit + "\"" + " -> " + "\"" + node.Value.Unit + "\"" + "; \r\n";
                }
            }
            return result;
        }

        public string ExportToGraphvizAlt(string result, NodeList<ScEvent> roots)
        {
            result = "";
            for(int i=0;i<roots.Count;i++)
            //foreach (Node<ScEvent> root in roots)
            {
                for(int j=0;j<roots[i].Neighbors.Count; j++)
                //foreach (Node<ScEvent> node in root.Neighbors)
                {
                    result += i + " -> " + i.ToString() + j.ToString() + "; \r\n";
                    result += ExportToGraphvizAlt(result, roots[i].Neighbors);
                }
            }

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
