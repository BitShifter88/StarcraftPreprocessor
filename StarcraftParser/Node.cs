using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarcraftParser
{
    public class Node<T>
    {
        private T data;
        private NodeList<T> neighbors = null;
        public long occurances {get;set;}

        public Node() { }
        public Node(T data) : this(0, data, null) { }
        public Node(long occurances, T data, NodeList<T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
            this.occurances = occurances;
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public NodeList<T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }
    }
}
