using System;
using System.Collections.Generic;
using System.Text;

namespace PrimsAlgorithmForMST
{
    public class GraphN
    {
        public LinkedList<StateNode>[] adj;
        public GraphN(int V)
        {
            adj = new LinkedList<StateNode>[V];

            for (int i = 0; i < V; i++)
            {
                adj[i] = new LinkedList<StateNode>();
            }
        }

        public void AddEdge(int source, int destination, int weight)
        {
            StateNode destinationNode = new StateNode()
            {
                Index = destination,
                Weight = weight
            };

            StateNode sourceNode = new StateNode()
            {
                Index = source,
                Weight = weight
            };

            adj[source].AddLast(destinationNode);
            adj[destination].AddLast(sourceNode);
        }
    }

    public class StateNode
    {
        public int Index;
        public int Weight;
    }
    
}
