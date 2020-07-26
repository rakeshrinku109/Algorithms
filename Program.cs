using C5;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PrimsAlgorithmForMST
{
    class Program
    {
        static GraphN graph = null;
        static void Main(string[] args)
        {
            graph = new GraphN(9);
            //graph.AddEdge(0,1,10);
            //graph.AddEdge(0,2,20);
            //graph.AddEdge(1,2,30);
            //graph.AddEdge(1, 3,5);
            //graph.AddEdge(3, 4, 8);
            //graph.AddEdge(3, 2, 15);
            //graph.AddEdge(2, 4, 6);
            graph.AddEdge(0, 1, 4);
            graph.AddEdge( 0, 7, 8);
            graph.AddEdge( 1, 2, 8);
            graph.AddEdge( 1, 7, 11);
            graph.AddEdge( 2, 3, 7);
            graph.AddEdge( 2, 8, 2);
            graph.AddEdge( 2, 5, 4);
            graph.AddEdge( 3, 4, 9);
            graph.AddEdge( 3, 5, 14);
            graph.AddEdge( 4, 5, 10);
            graph.AddEdge( 5, 6, 2);
            graph.AddEdge( 6, 7, 1);
            graph.AddEdge( 6, 8, 6);
            graph.AddEdge( 7, 8, 7);
            Console.WriteLine("Hello World!");
          PrimsAlgorithm(9);
        }

        static void PrimsAlgorithm(int v)
        {
            int[] Parent = new int[v];
            Parent[0] = -1;

            MinHeapMap PriorityQueue = new MinHeapMap(v);
            
            var keyNode = new KeyNode { Weight = 0, Index = 0 };
            PriorityQueue.Add(keyNode);

            for (int i = 1; i < v; i++)
            {   
                Parent[i] = -1;
                keyNode  = new KeyNode { Weight = int.MaxValue, Index = i };
                PriorityQueue.Add(keyNode);
                
            }

            while (PriorityQueue.Size > 0)
            {
                KeyNode Node = PriorityQueue.ExtractMin();
                var childNodes = graph.adj[Node.Index];

                foreach (var childNode in childNodes)
                {
                    int ReferenceWeight = PriorityQueue.GetWeight(childNode.Index);
                    int OriginalWeight = childNode.Weight;

                    if (PriorityQueue.Contains(childNode.Index) && ReferenceWeight > OriginalWeight)
                    {
                        PriorityQueue.Update(childNode.Index, OriginalWeight);
                        Parent[childNode.Index] = Node.Index;
                    }
                }
            }
            for (int i = 0; i < v; i++)
            {
                Console.WriteLine(Parent[i] +" is Parent of"+ "Index: " + i);
            }

        }
    }

}
