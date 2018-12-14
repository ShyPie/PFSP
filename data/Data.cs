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
    }

    public class Data
    {
        public int Machines { get; set; }
        public List<Job> Jobs { get; set; }

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
