using System;
using System.Collections.Generic;
using C5;

namespace Module4Examples
{
    class BranchAndBound
    {
        int[,] costMatrix;
        int numTasks;
        int numAgents;
        int[] minimumCosts;

        class SolutionCandidate : IComparable, ICloneable
        {
            public int LowerBound;
            public List<int> Assignments;

            public int CompareTo(object obj)
            {
                return LowerBound.CompareTo(((SolutionCandidate)obj).LowerBound);
            }

            public object Clone()
            {
                var clone = new SolutionCandidate
                {
                    LowerBound = this.LowerBound,
                    Assignments = new List<int>(this.Assignments)
                };
                return clone;
            }
        }

        public BranchAndBound(int[,] costMatrix)
        {
            this.costMatrix = costMatrix;
            numAgents = costMatrix.GetLength(0);
            numTasks = numAgents; // For this problem, these two are always equal.

            // Find minimum costs for each column:
            minimumCosts = (int[])Array.CreateInstance(typeof(int), numAgents);
            for (var agent = 0; agent < numAgents; agent++)
            {
                minimumCosts[agent] = costMatrix[agent, 0];
                for (var task = 1; task < numTasks; task++)
                    minimumCosts[agent] = Math.Min(minimumCosts[agent], costMatrix[agent, task]);
            }
        }

        public List<int> FindAssignment(out int solutionCost)
        {
            // Initialize first solution candidate:
            var currentSolution = new SolutionCandidate() { Assignments = new List<int>() };
            for (var i = 0; i < numAgents; i++)
            {
                currentSolution.Assignments.Add(i);
                currentSolution.LowerBound += costMatrix[i, i];
            }

            // Prepare queue and initialize it with an empty candidate:
            var queue = new IntervalHeap<SolutionCandidate>(); // A queue or a stack could be used instead.
            queue.Add(new SolutionCandidate() { Assignments = new List<int>() });

            // Here is the main algorithm...
            while (!queue.IsEmpty)
            {
                // Get next candidate. If a priority queue is used, the "next" candidate is the
                // most promising (with lowest lower bound):
                var candidate = queue.DeleteMin();

                // Branch out:
                for (var task = 0; task < numTasks; task++)
                {
                    // Only create the branch if the current task is not already taken by an agent:
                    if (!candidate.Assignments.Contains(task)) // A HashSet on the side could speed up the .Contains() call...
                    {
                        var branch = (SolutionCandidate)candidate.Clone();
                        branch.Assignments.Add(task);
                        branch.LowerBound = CalculateLowerBound(branch);

                        // Only use the generated branch if its lower bound is strictly better than
                        // the currently found solution cost.
                        // This is the "bound" part in "Branch and Bound":
                        if (branch.LowerBound >= currentSolution.LowerBound)
                            continue;

                        // Do we have a solution?
                        if (branch.Assignments.Count == numAgents)
                            currentSolution = branch;
                        else // If not, use it for further extension:
                            queue.Add(branch);
                    }
                }
            }
            solutionCost = currentSolution.LowerBound;
            return currentSolution.Assignments;
        }

        int CalculateLowerBound(SolutionCandidate branch)
        {
            var lowerBound = 0;
            // Add the costs for the already fixed agents:
            for (var agent = 0; agent < branch.Assignments.Count; agent++)
            {
                var task = branch.Assignments[agent];
                lowerBound += costMatrix[agent, task];
            }

            // Add "minimum cost" for remaining agents:
            for (var agent = branch.Assignments.Count; agent < numAgents; agent++)
                lowerBound += minimumCosts[agent];

            return lowerBound;
        }
    }
}
