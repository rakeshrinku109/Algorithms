using C5;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace PrimsAlgorithmForMST
{
    public class KeyNode : IComparable
    {
        public int Index;
        public int Weight;

        public int CompareTo(object obj)
        {
            return this.Weight.CompareTo(((KeyNode)obj).Weight);
        }
    }
    public class MinHeapMap 
    {
        KeyNode[] HeapArray;
        public int Size = 0;
        public int currentIndex = 0;
        Dictionary<int, int> IndexeMap = new Dictionary<int, int>();

        public MinHeapMap(int numberOfElements)
        {
            HeapArray = new KeyNode[numberOfElements];
            this.Size = numberOfElements;
        }

        public void Add(KeyNode node)
        {
            HeapArray[currentIndex] = node;
            IndexeMap[node.Index] = currentIndex;
            HeapifyUp(currentIndex);
            ++currentIndex;
        }

        public KeyNode ExtractMin()
        {
            KeyNode minNode = null;
            if (Size > 0)
            {
                Swap(0, Size - 1);
                minNode = HeapArray[Size-1];
                --Size;
                Heapify(0);
            }
            return minNode ;
        }

        public void HeapifyUp(int index)
        {
            int Parent = (index - 1) / 2;

            if (Parent >=0 & HeapArray[index].CompareTo(HeapArray[Parent]) < 0)
            {
                Swap(index, Parent);
                HeapifyUp(Parent);
            }
        }

        public void Heapify(int index)
        {
            int LeftChild = 2 * index + 1;
            int RightChild = 2 * index + 2;
            int SmallestIndex = index;

            if (LeftChild < Size && HeapArray[SmallestIndex].CompareTo(HeapArray[LeftChild]) > 0)
            {
                SmallestIndex = LeftChild;
            }

            if (RightChild < Size && HeapArray[SmallestIndex].CompareTo(HeapArray[RightChild]) > 0)
            {
                SmallestIndex = RightChild;
            }

            if (index !=SmallestIndex)
            {
                Swap(index, SmallestIndex);
                Heapify(SmallestIndex);
            }
        }

        private void Swap(int x, int y)
        {
            var temp = HeapArray[y];

            IndexeMap[HeapArray[y].Index] = x;
            IndexeMap[HeapArray[x].Index] = y;

            HeapArray[y] = HeapArray[x];
            HeapArray[x] = temp;
        }


        public bool Contains(int Index)
        {
          int currentIndex;
          IndexeMap.TryGetValue(Index, out currentIndex);
            if (currentIndex > Size - 1)
            {
                return false;
            }
            return true;
        }

        public int GetWeight(int Index)
        {
            int Value;
            IndexeMap.TryGetValue(Index, out Value);
            return HeapArray[Value].Weight;
        }

        public void Update(int Index, int Weight)
        {
            int updatedIndex;
            IndexeMap.TryGetValue(Index, out updatedIndex);
            HeapArray[updatedIndex].Weight =Weight;
            Swap(updatedIndex, Size - 1);
            HeapifyUp(Size - 1);
        }
    }
}
