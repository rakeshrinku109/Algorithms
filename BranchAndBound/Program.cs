using C5;
using Module4Examples;
using System;
using System.Collections.Generic;

namespace DivideAndConquer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        static void RunBranchAndBoundAssignment()
        {
            var costMatrix = new int[4, 4]
            {
                { 3, 5, 9, 2},
                { 9, 3, 3, 4},
                { 1, 4, 2, 6},
                { 5, 3, 7, 2}
            };

            var solver = new BranchAndBound(costMatrix);
            var solutionCost = 0;
            var assignment = solver.FindAssignment(out solutionCost);

            Console.WriteLine("Assignment cost: {0}", solutionCost);
            for (var agent = 0; agent < assignment.Count; agent++)
                Console.WriteLine("   Agent {0}: Task {1}", agent, assignment[agent]);
        }

    }

    public class BnB
    {
        int[,] CostMatrix;
        int NumberOfAgents;
        int NumberOFTasks;
        int[] MinimumCosts;
        IntervalHeap<SolutionCandidate> SolutionCandidatePriorityQueue = new IntervalHeap<SolutionCandidate>();

        public BnB(int[,] costMatrix, int m, int n)
        {
            this.CostMatrix = costMatrix;
            this.NumberOFTasks = n;
            this.NumberOfAgents = m;
            this.MinimumCosts = new int[NumberOfAgents];
        }

        public void EvaluateCost()
        {
            BuildMinimumCostTable(NumberOfAgents,NumberOFTasks);

            SolutionCandidate currentSolution = new SolutionCandidate();
            currentSolution.Assignments = new System.Collections.Generic.List<int>();

            for (int i = 0; i < NumberOfAgents; i++)
            {
                currentSolution.Assignments.Add(i);
                currentSolution.LowerBound += this.CostMatrix[NumberOfAgents, NumberOfAgents];
            }

            SolutionCandidatePriorityQueue.Add(new SolutionCandidate() { Assignments = new System.Collections.Generic.List<int>() });

            while (!SolutionCandidatePriorityQueue.IsEmpty)
            {
                // Get next candidate. If a priority queue is used, the "next" candidate is the
                // most promising (with lowest lower bound):
                var candidate = SolutionCandidatePriorityQueue.DeleteMin();

                // Branch out:
                for (int i = 0; i < NumberOFTasks; i++)
                {
                    // Only create the branch if the current task is not already taken by an agent:
                    if (!candidate.Assignments.Contains(i))
                    {
                        var branch = (SolutionCandidate)candidate.Clone();
                        branch.Assignments.Add(i);
                        branch.LowerBound = CalculateLowerBound(branch);

                        // Only use the generated branch if its lower bound is strictly better than
                        // the currently found solution cost.
                        // This is the "bound" part in "Branch and Bound":
                        if (currentSolution.LowerBound < branch.LowerBound)
                            continue;

                        if (branch.Assignments.Count == NumberOfAgents)
                        {
                            currentSolution = branch;
                        }
                        else
                        {
                            this.SolutionCandidatePriorityQueue.Add(branch);
                        }
                    }
                }
            }


        }

        private int CalculateLowerBound(SolutionCandidate candidate)
        {
            int lowerBound = 0;
            int Agent = 0;
            // Add the costs for the already fixed agents:
            for (; Agent < candidate.Assignments.Count; Agent++)
            {
                var task = candidate.Assignments[Agent];
                lowerBound += this.CostMatrix[Agent, task];
            }

            for (int j = Agent; j < MinimumCosts.Length; j++)
            {
                lowerBound += MinimumCosts[j];
            }

            return lowerBound;
        }

        private void BuildMinimumCostTable(int row, int column)
        {
            while(row >= 0)
            {
                int min = this.CostMatrix[row, 0];
                for (int i = 1; i < column; i++)
                {
                   min = Math.Min(min, this.CostMatrix[row, i]);
                }
                this.MinimumCosts[row] = min;
            }
        }
    }

    public class SolutionCandidate : IComparable
    {
        public int LowerBound = 0;
        public List<int> Assignments { get; set; }

        public int CompareTo(object obj)
        {
            return this.LowerBound.CompareTo(((SolutionCandidate)obj).LowerBound);
        }

        public SolutionCandidate Clone()
        {
            SolutionCandidate Clone = new SolutionCandidate()
            {
                LowerBound = this.LowerBound,
                Assignments = this.Assignments
            };
            
            return Clone;
        }
    }


}
