using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;
using algorithms;
using generator;

namespace non_smooth_PFSP
{
    class Program
    {
        static void Main(string[] args)
        {
            

            //Data d = new Data(2, 3);

            //double[,] input = new double[2, 3]
            //{
            //    { 10, 7, 8 },
            //    { 4, 5, 6 }
            //};

            //for(int i = 0; i < 2; ++i)
            //{
            //    for(int j = 0; j < 3; ++j)
            //    {
            //        d.Jobs[i].Tasks[j] = input[i, j];
            //    }
            //}
            //Generator gen = new Generator();
            //gen.Create(4, 3, @"c:\test\diploma_small.txt");

            Parser p = new Parser();
            IProblem t = new PFSP(p.Parse(@"c:\test\diploma_small.txt"));
            t.FillJobSequence();

            //var a1 = t.GetMakespan();
            //var a2 = t.GetMakespanWithPenalty();

            NEH neh = new NEH(new BiasedRendomization());
            var s1 = neh.Run(t);
            var makespan = s1.GetMakespan();
            var makespanWithPenalty = s1.GetMakespanWithPenalty();
            IAlgorithm alg = new BRILS(neh, 5);
            var s2 = alg.Run(t);
            var makespan1 = s2.GetMakespan();
            var makespanWithPenalty2 = s2.GetMakespanWithPenalty();
        }
    }
}
