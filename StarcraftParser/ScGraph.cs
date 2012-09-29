using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace StarcraftParser
{
    class Node<T>
    {
        private T           data;
        private NodeList<T> children = null;

        public Node() {}
        public Node(T data) : this( data: data,
                                    children: null) { }

        public Node(T data, NodeList<T> children)
        {
            this.data       = data;
            this.children   = children;
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

        protected NodeList<T> Children
        {
            get
            {
                return children;
            }
            set
            {
                children = value;
            }
        }
    }

    class NodeList<T> : Collection<Node<T>>
    {
        private List<ScEvent> possibleRoots;

        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        public NodeList(List<T> roots)
        {
            foreach (T root in roots)
            {
                base.Items.Add(new Node<T>(root));
            }
        }

        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }



    //class ScGraph<T>
    //{
    //    public int occurances { get; set; }
    //    public T data { get; set; }
    //    public List<ScGraph<T>> children { get; set; }

    //    public ScGraph(T value)
    //    {
    //        this.data = value;
    //        this.occurances = 1;
    //    }

    //    public void AddChild(ScGraph<T> child)
    //    {
    //        this.children.Add(child);
    //    }

    //    public void RemoveChild(ScGraph<T> child)
    //    {
    //        this.children.Remove(child);
    //    }
    //}
}
