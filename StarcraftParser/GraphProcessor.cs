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
                result.Add(new Node<ScEvent>(1, r, buildTree(counter, game, games)));
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
                    }
                }
                if (roots.Count == 0 || counter == 0) roots.Add(node);
            }

            NodeList<ScEvent> result = roots;
            CountOccurances(result, allgames);
            return result;
        }

        private void CountOccurances(NodeList<ScEvent> roots, NodeList<ScEvent> allgames)
        {
            foreach (Node<ScEvent> root in roots)
            {
                foreach (Node<ScEvent> game in allgames)
                {
                    if (root.Value.Unit == game.Value.Unit)
                    {
                        root.occurances++;
                        //NodeList<ScEvent> tmp = (Node<ScEvent>)allgames.Skip(allgames.IndexOf(game));
                        CountOccurances(root.Neighbors, game.Neighbors);
                        //CountNeighbourOccurance(root, game.Neighbors);
                    }
                }
            }
        }

        private void CountNeighbourOccurance(Node<ScEvent> root, NodeList<ScEvent> gameNeighbors)
        {
            if (root.Neighbors == null || gameNeighbors == null) return;

            Node<ScEvent> notFoundGame = new Node<ScEvent>();
            Node<ScEvent> notFoundRoot = new Node<ScEvent>();
            bool found = false;
            foreach (Node<ScEvent> r in root.Neighbors)
            {                
                foreach (Node<ScEvent> game in gameNeighbors)
                {
                    if (root.Value.Unit == game.Value.Unit)
                    {
                        found = true;
                        root.occurances++;
                        CountNeighbourOccurance(r, game.Neighbors);
                    }
                    else
                    {
                        notFoundGame = game;
                        notFoundRoot = r;
                    }
                }
                
            }
            if (!found)
            {
                root.Neighbors.Add(notFoundGame);
                //CountNeighbourOccurance(root, gameNeighbors);
            }
        }

        
    }
}
