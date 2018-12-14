using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;

namespace algorithms
{
    public interface IAlgorithm
    {
        IProblem Run(IProblem problem);
    }
}
