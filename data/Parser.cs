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
            sr.Close();
            return data;
        }
    }
}
