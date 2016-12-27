using System;

namespace PolygonUnion
{
    public class CircularDoublyLinkedList<T>
    {
        public Node<T> tail;
        private int count;

        public CircularDoublyLinkedList()
        {
            count = 0;
        }

        public void InsertAfter(Node<T> node, Node<T> newNode)
        {
            newNode.next = node.next;
            newNode.prev = node;
            node.next = newNode;
            node.next.prev = newNode;
            count++;
        }

        public void InsertBefore(Node<T> node, Node<T> newNode)
        {
            InsertAfter(node.prev, newNode);
        }

        public void InsertEnd(Node<T> node)
        {
            // Handle empty list
            // Single node list points to itself
            if (this.tail == null)
            {
                node.prev = node;
                node.next = node;
                this.tail = node;
                count++;
            }
            else
            {
                // Inserts the new tail and changes tail ref.
                InsertAfter(this.tail, node);
                this.tail = node;
            }
        }

        public void Remove(Node<T> node)
        {
            if (node.next.Equals(node))
            {
                this.tail = null;
            }
            else
            {
                node.prev.next = node.next;
                node.next.prev = node.prev;

                if (node.Equals(this.tail))
                {
                    this.tail = node.prev;
                }
            }
            count--;

        }

        public int Count()
        {
            return count;
        }
    }    

    public class Node<T>
    {
        public Node<T> next;
        public Node<T> prev;
        public Node<T> friend;

        // if not entry, then exit
        public T data;
        public Guid id;

        public bool IsEntry { set; get; }
        public bool IsIntersection { set; get; }
        public bool Processed { set; get; }

        public Node()
        {
            id = Guid.NewGuid();
            IsEntry = false;
            Processed = false;
        }

        public Node(T data)
        {
            this.data = data;
            id = Guid.NewGuid();
            IsEntry = false;
            Processed = false; 
        }

        public override bool Equals(object other)
        {
            return ((Node<T>) other).id.Equals(this.id);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
