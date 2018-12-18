using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;

namespace algorithms
{
    public class BRILS : IAlgorithm
    {
        private IAlgorithm m_initAlgorithm;
        private int m_iterations;

        public BRILS(IAlgorithm alg = null, int iterations = 5)
        {
            m_initAlgorithm = alg != null ? alg : new Stub();
            m_iterations = iterations;
        }

        public IProblem Run(IProblem problem)
        {
            problem = problem.Copy();
            int temp = -1;
            problem = m_initAlgorithm.Run(problem);
            List <int> bestSolution = problem.GetJobSequence();
            List<int> baseSolution = bestSolution;
            double baseSolutionResult = problem.GetMakespanWithPenalty();
            double bestSolutionResult = baseSolutionResult;
            double credit = 0;
            for (int i = 0; i < m_iterations; ++i)
            {
                Random rand = new Random();
                int left = rand.Next(0, problem.GetJobSequence().Count);
                int right = rand.Next(0, problem.GetJobSequence().Count);
                if (left > right)
                {
                    temp = left;
                    left = right;
                    right = temp;
                }
                problem.Perturbate(left, right);
                baseSolution = problem.GetJobSequence();
                problem.ClearJobSequence();
                for (int j = 0; j < left; ++j)
                {
                    problem.PushJob(baseSolution[j]);
                }
                problem.InsertJob2BestPlace(baseSolution[left]);
                for (int j = left + 1; j < right; ++j)
                {
                    problem.PushJob(baseSolution[j]);
                }
                problem.InsertJob2BestPlace(baseSolution[right]);
                for (int j = right + 1; j < baseSolution.Count; ++j)
                {
                    problem.PushJob(baseSolution[j]);
                }
                double curResult = problem.GetMakespanWithPenalty();
                double delta = curResult - baseSolutionResult;

                if (delta == 0) continue;

                if (delta < 0)
                {
                    credit = (-1) * delta;
                    baseSolution = problem.GetJobSequence();
                    baseSolutionResult = curResult;
                    if (baseSolutionResult < bestSolutionResult)
                    {
                        bestSolution = baseSolution;
                        bestSolutionResult = baseSolutionResult;
                    }
                }
                else if (delta <= credit)
                {
                    credit -= delta;
                    baseSolution = problem.GetJobSequence();
                    baseSolutionResult = curResult;
                }
                else
                {
                    problem.ClearJobSequence();
                    for (int j = 0; j < bestSolution.Count(); ++j)
                    {
                        problem.PushJob(bestSolution[j]);
                    }
                    return problem;
                }
            }
            problem.ClearJobSequence();
            for (int j = 0; j < bestSolution.Count(); ++j)
            {
                problem.PushJob(bestSolution[j]);
            }
            return problem;
        }
    }
}
