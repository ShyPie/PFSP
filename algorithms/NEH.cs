using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;

namespace algorithms
{
    public class Stub : IAlgorithm
    {
        public Stub() { }

        public IProblem Run(IProblem problem)
        {
            return problem.Copy();
        }
    }

    public class SortByTime : IAlgorithm
    {
        public SortByTime() { }

        public IProblem Run(IProblem problem)
        {
            problem = problem.Copy();
            problem.Sort();
            return problem;
        }
    }

    public class SortByMoments : IAlgorithm
    {
        public SortByMoments() { }
        public IProblem Run(IProblem problem)
        {
            problem = problem.Copy();
            problem.SortBySumOfMoments();
            return problem;
        }
    }

    public class BiasedRandomization: IAlgorithm
    {
        public BiasedRandomization() { }

        public IProblem Run(IProblem problem)
        {
            problem = problem.Copy();
            var rand = new Random();
            var data = problem.GetData();
            double totalTime = problem.GetSumOfJobs();
            var sequence = problem.GetJobSequence();
            problem.ClearJobSequence();
            while (totalTime != 0)
            {
                for (int i = 0; i < sequence.Count; ++i)
                {
                    if (sequence[i] != -1)
                    {
                        var val = rand.Next(0, Convert.ToInt32(totalTime + 0.5));
                        var curTime = data.Jobs[sequence[i]].GetSumOfTasks();
                        if (val <= curTime)
                        {
                            problem.PushJob(sequence[i]);
                            sequence[i] = -1;
                            totalTime -= curTime;
                        }
                    }
                }
            }
            return problem;
        }
    }
    public class NEH : IAlgorithm
    {
        private IAlgorithm m_randomization;
        private IAlgorithm m_sort;
        public NEH(IAlgorithm randomization = null, IAlgorithm sort = null)
        {
            m_randomization = randomization != null ? randomization : new Stub();
            m_sort = sort != null ? sort : new SortByTime();
        }

        public IProblem Run(IProblem problem)
        {
            problem = problem.Copy();
            problem = m_sort.Run(problem);
            problem = m_randomization.Run(problem);
            List<int> sortedSequence = problem.GetJobSequence();
            problem.ClearJobSequence();
            problem.PushJob(sortedSequence[0]);
            for (int i = 1; i < sortedSequence.Count; ++i)
            {
                problem.InsertJob2BestPlace(sortedSequence[i]);
            }
            
            return problem;
        }
    }
}
