using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using data;
using algorithms;

namespace report
{
    public class AccuracyMeasureReport : IExcelReport
    {
        private Excel.Application m_excel;
        private List<IProblem> m_problems;
        private int m_iterationsCount;
        private int m_runsCount;
        private Dictionary<string, double> m_lowerBounds;
        public AccuracyMeasureReport(List <IProblem> problems, string lowerBoundPath, int runsCount = 10, int iterationCount = 20)
        {
            m_lowerBounds = new Dictionary<string, double>();
            ParseLowerBound(lowerBoundPath);
            m_problems = problems;
            m_iterationsCount = iterationCount;
            m_runsCount = runsCount;
            m_excel = new Excel.Application();
        }
        public void Create()
        {
            var workbook = m_excel.Workbooks.Add(Type.Missing);        
            var sheetAverage = workbook.Sheets.Add();
            var sheetMin = workbook.Sheets.Add();
            sheetAverage.Cells[1, 2] = "LB";
            sheetAverage.Cells[1, 3] = "NEH";
            sheetAverage.Cells[1, 4] = "BR_NE_H";
            sheetAverage.Cells[1, 5] = "BR_N_E_H(moments)";
            sheetAverage.Cells[1, 6] = "ILS";
            sheetAverage.Cells[1, 7] = "IL_S(moments)";
            sheetAverage.Cells[1, 8] = "BRI_L_S";
            sheetAverage.Cells[1, 9] = "BR_IL_S_(moments)";
            sheetAverage.Cells[1, 10] = "Best algorithm";
            sheetMin.Cells[1, 2] = "LB";
            sheetMin.Cells[1, 3] = "NEH";
            sheetMin.Cells[1, 4] = "BR_NE_H";
            sheetMin.Cells[1, 5] = "BR_N_E_H(moments)";
            sheetMin.Cells[1, 6] = "ILS";
            sheetMin.Cells[1, 7] = "IL_S(moments)";
            sheetMin.Cells[1, 8] = "BRI_L_S";
            sheetMin.Cells[1, 9] = "BR_IL_S_(moments)";
            sheetMin.Cells[1, 10] = "Best algorithm";
            var neh = new NEH();
            List<IAlgorithm> algs = new List<IAlgorithm>() {
                new NEH(new BiasedRandomization()),
                new NEH(new BiasedRandomization(), new SortByMoments()),
                new BRILS(new NEH(), m_iterationsCount),
                new BRILS(new NEH(null, new SortByMoments()), m_iterationsCount),
                new BRILS(new NEH(new BiasedRandomization()), m_iterationsCount),
                new BRILS(new NEH(new BiasedRandomization(), new SortByMoments()), m_iterationsCount),
            };
            for (int p = 0; p < m_problems.Count(); ++p)
            {
                List<double> averageResults = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
                List<double> minResults = new List<double>() { 0, 0, 0, 0, 0, 0, 0 };
                averageResults[0] = neh.Run(m_problems[p]).GetMakespanWithPenalty();
                minResults[0] = averageResults[0];
                double curResult = 0;
                var lowerBound = m_lowerBounds[m_problems[p].GetData().Name];
                sheetAverage.Cells[p + 2, 2] = lowerBound;
                sheetMin.Cells[p + 2, 2] = lowerBound;
                for (int i = 0; i < m_runsCount; ++i)
                {
                    for(int a = 0; a < algs.Count(); ++a)
                    {
                        curResult = algs[a].Run(m_problems[p]).GetMakespanWithPenalty();
                        averageResults[a + 1] += curResult/m_runsCount;
                        if ((curResult < minResults[a + 1]) || (minResults[a + 1] == 0))
                        {
                            minResults[a + 1] = curResult;
                        }
                    }
                }
                sheetAverage.Cells[p+2, 1] = String.Format("Task {0}", p + 1);
                for (int a = 0; a < averageResults.Count; ++a)
                {
                    sheetAverage.Cells[p + 2, a + 3] =  (averageResults[a] - lowerBound) / averageResults[a];
                }
                var avgBest = averageResults.Min();
                //Console.WriteLine(avgBest);
                var algorithmsStr = "";
                for (var j = 0; j < averageResults.Count; ++j)
                {
                    //Console.WriteLine("Alg {0} {1} {2}", sheetAverage.Cells[1, j + 3].Value, averageResults[j], averageResults[j] == avgBest);
                    if (averageResults[j] - avgBest <= 0e-3)
                    {
                        //Console.WriteLine("BEST ALG {0}", sheetAverage.Cells[1, j + 3].Value);
                        algorithmsStr += sheetAverage.Cells[1, j + 3].Value + '\n';
                    }
                }
                sheetAverage.Cells[p + 2, 10] = algorithmsStr;

                sheetMin.Cells[p + 2, 1] = String.Format("Task {0}", p + 1);
                for (int m = 0; m < minResults.Count(); ++m)
                {
                    sheetMin.Cells[p + 2, m + 3] = (minResults[m] - lowerBound) / minResults[m];
                }
                var minBest = minResults.Min();
                algorithmsStr = "";
                for (var j = 0; j < minResults.Count; ++j)
                {
                    if (minResults[j] - minBest <= 0e-3)
                    {
                        algorithmsStr += sheetMin.Cells[1, j + 3].Value + '\n';
                    }
                }
                sheetMin.Cells[p + 2, 10] = algorithmsStr;
            }
            workbook.SaveAs( @"c:\test\diploma\report_20_10.xlsx");
            workbook.Close();
            Console.WriteLine("DONE!");
        }

        static Excel.Application GetActiveExcel()
        {
            Excel.Application excel = null;
            try
            {
                excel = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }
            catch (Exception)
            {
            }
            return excel;
        }

        void ParseLowerBound(string path)
        {
            System.Diagnostics.Process.Start(path);

            var excel = GetActiveExcel();
            if (excel == null)
            {
                Console.WriteLine("No open excel found. =(");
                return;
            }
            excel.Visible = false;
            var workbook = excel.Workbooks.get_Item(Path.GetFileName(path));

            Excel._Worksheet largeWorkSheet = workbook.Sheets[1];
            Excel.Range largeRange = largeWorkSheet.UsedRange;

            Excel._Worksheet smallWorkSheet = workbook.Sheets[2];
            Excel.Range smallRange = smallWorkSheet.UsedRange;

            const int StartRow = 2;
            const int EndRow = 241;
            const int pathCol = 1;
            const int largeLBCol = 6;
            const int smallLBCol = 3;

            for (int i = StartRow; i <= EndRow; ++i)
            {
                string key = largeRange.Cells[i, pathCol].Value + ".txt";
                m_lowerBounds.Add(key, largeRange.Cells[i, largeLBCol].Value);
                key = smallRange.Cells[i, pathCol].Value + ".txt";
                m_lowerBounds.Add(key, smallRange.Cells[i, smallLBCol].Value);
            }
            workbook.Close(false);
            excel.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
        }
    }
}
