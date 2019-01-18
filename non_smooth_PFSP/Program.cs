using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;
using algorithms;
using generator;
using report;

namespace non_smooth_PFSP
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator gen = new Generator();
            //gen.Create(12, 10, @"c:\test\diploma_middle1.txt");
            //gen.Create(12, 10, @"c:\test\diploma_middle2.txt");
            //gen.Create(12, 10, @"c:\test\diploma_middle3.txt");
            //gen.Create(12, 10, @"c:\test\diploma_middle4.txt");
            //gen.Create(12, 10, @"c:\test\diploma_middle5.txt");
            VFRParser p = new VFRParser();
            PFSP t1 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_1_Gap.txt"));
            PFSP t2 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_2_Gap.txt"));
            PFSP t3 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_3_Gap.txt"));
            PFSP t4 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_4_Gap.txt"));
            PFSP t5 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_5_Gap.txt"));
            PFSP t6 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_6_Gap.txt"));
            PFSP t7 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_7_Gap.txt"));
            PFSP t8 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_8_Gap.txt"));
            PFSP t9 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_9_Gap.txt"));
            PFSP t10 = new PFSP(p.Parse(@"c:\test\diploma\VFR20_10_10_Gap.txt"));
            List <IProblem> problems = new List<IProblem>() { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10 };
            
            // TODO: set correct path
            AccuracyMeasureReport report = new AccuracyMeasureReport(problems, @"c:\test\diploma\BestSolutionsAndBounds.xlsx");
            report.Create();

            //Parser p = new Parser();
            //IProblem t = new PFSP(p.Parse(@"c:\test\diploma_medium.txt"));
            //t.FillJobSequence();
            

            //NEH neh = new NEH(new BiasedRandomization());
            //var s1 = neh.Run(t1);
            //var makespan = s1.GetMakespan();
            //var makespanWithPenalty = s1.GetMakespanWithPenalty();
            //IAlgorithm alg = new BRILS(neh, 5);
            //var s2 = alg.Run(t);
            //var makespan1 = s2.GetMakespan();
            //var makespanWithPenalty2 = s2.GetMakespanWithPenalty();
        }
    }
}
