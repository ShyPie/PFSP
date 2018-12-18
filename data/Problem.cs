using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data
{
    public interface IProblem
    {
        double GetMakespan();
        double GetMakespanWithPenalty();
        List<int> GetJobSequence();
        void Perturbate(int index1, int index2);
        void InsertJob(int jobId, int index);
        void InsertJob2BestPlace(int jobId);
        void PushJob(int jobId);
        void RemoveJob(int jobId);
        void ClearJobSequence();
        void FillJobSequence();
        Data GetData();
        void Sort();
        double GetSumOfJobs();
        IProblem Copy();
    }

    public class PFSP: IProblem
    {
        private Data m_data = null;
        private List<int> m_jobSequence = null;

        public PFSP(Data data)
        {
            m_data = data;
            m_jobSequence = new List<int>();
        }

        public double GetMakespan()
        {
            return GetMakespan(m_jobSequence.Count - 1, m_data.Machines - 1);
        }

        public double GetMakespan(int job, int machine)
        {
            if (job == -1 || machine == -1)
            {
                return 0.0;
            }
            double makespan = m_data.Jobs[m_jobSequence[job]].Tasks[machine];
            return makespan + Helpers.GetMaxValue(GetMakespan(job, machine - 1), GetMakespan(job - 1, machine));
        }
        public double GetMakespanWithPenalty()
        {
            double maxTask = 0;
            foreach (Job job in m_data.Jobs)
            {
                var task = job.GetMaxTask();
                if (task > maxTask)
                {
                    maxTask = task;
                }
            }
            double totalPenalty = 0.0;
            for (int i = 0; i < m_data.Machines; ++i)
            {
                double machinePenalty = 0.0;
                List<int> JobIdsAfterBreak = new List<int>() { 0 };
                for (int j = 1; j < m_jobSequence.Count; ++j)
                {
                    //if (GetMakespan(m_jobSequence[j], i - 1) > GetMakespan(m_jobSequence[j] - 1, i))
                    if (GetMakespan(j, i - 1) > GetMakespan(j - 1, i))
                    {
                       JobIdsAfterBreak.Add(j);
                    }
                }
                JobIdsAfterBreak.Add(m_jobSequence.Count);

                double[] penalties = new double[JobIdsAfterBreak.Count];
                for (int j = 0; j < JobIdsAfterBreak.Count - 1; ++j)
                {
                    var penalty = 0.0;
                    for (int k = JobIdsAfterBreak[j]; k < JobIdsAfterBreak[j + 1]; ++k)
                    {
                        penalty += m_data.Jobs[m_jobSequence[k]].Tasks[i];
                    }
                    machinePenalty += getPenalty(penalty, maxTask);
                }
                totalPenalty += machinePenalty / m_data.Machines;
            }
            return totalPenalty + GetMakespan();
        }
       
        public List<int> GetJobSequence()
        {
            return m_jobSequence.ToList();
        }
        public void Perturbate(int index1, int index2)
        {
            int temp = m_jobSequence[index1];
            m_jobSequence[index1] = m_jobSequence[index2];
            m_jobSequence[index2] = temp;
        }
        public void InsertJob(int jobId, int index)
        {
            m_jobSequence.Insert(index, jobId);
        }

        public void InsertJob2BestPlace(int jobId)
        {
            int positions = m_jobSequence.Count() + 1;
            int bestPosition = 0;
            InsertJob(jobId, 0);
            double bestMakespan = GetMakespanWithPenalty();
            RemoveJob(jobId);
            for (int i = 1; i < positions; ++i)
            {
                InsertJob(jobId, i);
                double curMakespan = GetMakespanWithPenalty();
                if (curMakespan < bestMakespan)
                {
                    bestMakespan = curMakespan;
                    bestPosition = i;
                }
                RemoveJob(jobId);
            }
            InsertJob(jobId, bestPosition);
        }

        public void PushJob(int jobId)
        {
            m_jobSequence.Add(jobId);
        }

        public void RemoveJob(int jobId)
        {
            m_jobSequence.Remove(jobId);
        }

        public void ClearJobSequence()
        {
            m_jobSequence.Clear();
        }

        public Data GetData()
        {
            return m_data;
        }

        public void FillJobSequence()
        {
            for (int i = 0; i < m_data.Jobs.Count(); ++i)
            {
                m_jobSequence.Add(i);
            }
        }

        private double getPenalty(double penalty, double maxTask)
        {
            if (penalty > 0 && penalty <= maxTask)
            {
                penalty = 0;
            }
            else if (penalty > maxTask && penalty <= 2 * maxTask)
            {
                penalty = 0.2 * maxTask;
            }
            else if (penalty > maxTask * 2 && penalty <= 3 * maxTask)
            {
                penalty = 0.3 * maxTask;
            }
            else
            {
                penalty = 0.5 * maxTask;
            }
            return penalty;
        }

        public void Sort()
        {
            m_jobSequence.Sort((a, b) =>
            {
                var duration1 = m_data.Jobs[a].GetSumOfTasks();
                var duration2 = m_data.Jobs[b].GetSumOfTasks();
                return duration2.CompareTo(duration1);
            });
        }
        public double GetSumOfJobs()
        {
            double sum = 0;
            foreach (var job in m_data.Jobs)
            {
                sum += job.GetSumOfTasks();
            }
            return sum;
        }

        public IProblem Copy()
        {
            var copy = new PFSP(m_data);
            copy.m_jobSequence = m_jobSequence.ToList();
            return copy;
        }
    }
}
