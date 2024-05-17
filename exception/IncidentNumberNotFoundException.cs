using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.exception
{
    internal class IncidentNumberNotFoundException : Exception
    {
        public IncidentNumberNotFoundException() { }

        public IncidentNumberNotFoundException(string message) : base(message) { }
    
    }
}
