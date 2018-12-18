using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;
using System.IO;
namespace generator
{
    public class Generator
    {
        public void Create(int jobs, int machines, string path)
        {
            Random rand = new Random();
            List <Job> Jobs = new List<Job>();
            for (int j = 0; j < jobs; ++j)
            {
                Job job = new Job(machines);
                for (int m = 0; m < machines; ++m)
                {
                    job.Tasks[m] = rand.Next(1, 100);
                }
                Jobs.Add(job);
            }
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(jobs);
            sw.WriteLine(machines);
            for (int j = 0; j < jobs; ++j)
            {
                sw.WriteLine(string.Join(";", Jobs[j].Tasks));
            }
            sw.Close();
        }
    }
}
