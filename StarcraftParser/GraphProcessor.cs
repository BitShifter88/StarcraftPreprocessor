using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                result.Add(new Node<ScEvent>(r, buildTree(counter, game, games)));
            }

            return result;
        }

        public NodeList<ScEvent> ProcessGames(List<ScGame> games)
        {
            List<ScEvent> possibleRoots = new List<ScEvent>();
            NodeList<ScEvent> allgames = new NodeList<ScEvent>();
            foreach (ScGame game in games)
            {
                allgames.Add(new Node<ScEvent>(game.Events[0], buildTree(0, game, games)));
            }
            //}
            //    for (int i = 0; i < game.Events.Count; i++)
            //    {
            //        ScEvent r = game.Events[i];
            //        if (r == null) continue;
            //        IEnumerable<ScEvent> q = possibleRoots.Where(e => e.Unit == r.Unit);
            //        if (q.Count() == 0)
            //        {
            //            possibleRoots.Add(r);
            //            //roots.Add(new Node<ScEvent>(r, buildTree(r, game, games)));
            //        }
            //    }
            //}

            return allgames;
            //return roots;
        }

    }
}
