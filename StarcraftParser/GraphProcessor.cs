using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    class GraphProcessor : Processor
    {
        public NodeList<ScEvent> buildTree(int counter, ScGame game)
        {
            NodeList<ScEvent> result = new NodeList<ScEvent>();
            if (++counter < game.Events.Count)
            {
                result.Add(new Node<ScEvent>(game.Events[counter], buildTree(counter, game)));
            }

            return result;

        }

        public NodeList<ScEvent> ProcessGames(List<ScGame> games)
        {
            NodeList<ScEvent> roots = new NodeList<ScEvent>();
            List<ScEvent> possibleRoots = new List<ScEvent>();
            foreach (ScGame game in games)
            {
                ScEvent r = game.Events[0];
                if (r == null) continue;
                IEnumerable<ScEvent> q = possibleRoots.Where(e => e.Unit == r.Unit);
                if (q.Count() == 0)
                {
                    possibleRoots.Add(r);
                    roots.Add(new Node<ScEvent>(r, buildTree(1, game)));
                }
            }

            // Roots found, build tree



            return result;
        }

    }
}
