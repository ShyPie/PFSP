using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using data;

namespace algorithms
{
    public class BRILS : IAlgorithm
    {
        private IAlgorithm m_initAlgorithm;
        public BRILS(IAlgorithm alg = null)
        {
            m_initAlgorithm = alg != null ? alg : new SimpleAlgorithm();
        }

        public IProblem Run(IProblem problem)
        {
            if (m_initAlgorithm != null)
            {
                problem = m_initAlgorithm.Run(problem);
            }

            return null;
        }
    }
}
