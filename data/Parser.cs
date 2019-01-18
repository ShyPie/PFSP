using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace data
{
    public class Parser
    {
        public Data Parse(string path)
        {
            StreamReader sr = new StreamReader(path);
            int jobs = Convert.ToInt32( sr.ReadLine());
            int machines = Convert.ToInt32(sr.ReadLine());
            Data data = new Data(jobs, machines);

            for (int j = 0; j < jobs; ++j)
            {
                Job job = new Job(machines);
                job.Tasks = sr.ReadLine().Split(' ', ';').Select(Double.Parse).ToArray();
                data.Jobs[j] = job;
            }
            data.Name = Path.GetFileName(path);
            sr.Close();
            return data;
        }
    }

    public class VFRParser
    {
        public Data Parse(string path)
        {
            StreamReader sr = new StreamReader(path);
            
            var stuff = sr.ReadLine().Split(new string[] { "  " }, StringSplitOptions.None).Select(Int32.Parse).ToArray();
            int jobs = stuff[0];
            int machines = stuff[1];
            Data data = new Data(jobs, machines);

            for (int j = 0; j < jobs; ++j)
            {
                Job job = new Job(machines);
                double[] tasks = sr.ReadLine().Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries).Select(Double.Parse).ToArray();
                for (int i = 0; i < machines; ++i)
                {
                    job.Tasks[i] = tasks[2 * i + 1];
                }
                data.Jobs[j] = job;
            }
            data.Name = Path.GetFileName(path);
            sr.Close();
            return data;
        }
    }
}
