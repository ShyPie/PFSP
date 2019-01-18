using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace data
{
    public struct Job
    {
        public double [] Tasks;

        public Job(int machines)
        {
            Tasks = new double[machines];
        }

        public double GetSumOfTasks()
        {
            double sumOfTasks = 0;
            foreach (var time in Tasks)
            {
                sumOfTasks += time;
            }
            return sumOfTasks;
        }

        public double GetMaxTask()
        {
            double maxTask = 0;
            foreach (var task in Tasks)
            {
                if (task > maxTask)
                {
                    maxTask = task;            
                }
            }
            return maxTask;
        }

        public double GetAVG()
        {
            double avg = 0;
            avg = GetSumOfTasks() / Tasks.Length;
            return avg;
        }

        public double GetSTD()
        {
            double std = 0;
            for (int i = 0; i < Tasks.Length; ++i)
            {
                std += Math.Pow(Tasks[i] - GetAVG(), 2);
            }
            std = Math.Pow((std / (Tasks.Length - 1)), 0.5);
            return std;
        }

        public double GetSKE()
        {
            double ske = 0;
            for (int i = 0; i < Tasks.Length; ++i)
            {
                ske += Math.Pow(Tasks[i] - GetAVG(), 3);
            }
            ske = ske / (Tasks.Length * Math.Pow(GetSTD(), 3));  
            return Math.Abs(ske);
        }
    }

    public class Data
    {
        public int Machines { get; set; }
        public List<Job> Jobs { get; set; }
        public string Name { get; set; }
        public Data(int jobs, int machines)
        {
            Machines = machines;
            Jobs = new List<Job>();
            for (int i = 0; i < jobs; ++i)
            {
                Jobs.Add(new Job(machines));
            }
        }
    }
}
