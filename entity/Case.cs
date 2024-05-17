using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class Case
    {
        
        private int _caseID;
        private string _caseDescription;
        private List<Incident> _incidents;

        
        public Case()
        {
            
            _incidents = new List<Incident>();
        }

        
        public Case(int caseID, string caseDescription, List<Incident> incidents)
        {
            _caseID = caseID;
            _caseDescription = caseDescription;
            _incidents = incidents;
        }

     
        public int CaseID
        {
            get { return _caseID; }
            set { _caseID = value; }
        }

        public string CaseDescription
        {
            get { return _caseDescription; }
            set { _caseDescription = value; }
        }

        public List<Incident> Incidents
        {
            get { return _incidents; }
            set { _incidents = value; }
        }
    }
}
