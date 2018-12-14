using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;
using algorithms;

namespace non_smooth_PFSP
{
    class Program
    {
        static void Main(string[] args)
        {
         //   Parser p = new Parser();
         //   ITask t = new PFSPTask(p.Parse("C:/PFSP/test.txt"));

            Data d = new Data(2, 3);

            double[,] input = new double[2, 3]
            {
                { 10, 7, 8 },
                { 4, 5, 6 }
            };

            for(int i = 0; i < 2; ++i)
            {
                for(int j = 0; j < 3; ++j)
                {
                    d.Jobs[i].Tasks[j] = input[i, j];
                }
            }

            IProblem t = new PFSP(d);
            t.FillJobSequence();

            var a1 = t.GetMakespan();
            var a2 = t.GetMakespanWithPenalty();
            IAlgorithm alg = new BRILS();
            var solution = alg.Run(t);
        }
    }
}
