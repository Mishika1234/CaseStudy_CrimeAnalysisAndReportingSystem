using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class Evidence
    {
        
        private int _evidenceID;
        private string _description;
        private string _locationFound;
        private int _incidentID;


        public Evidence(int evidenceID, string description, string locationFound, int incidentID)
        {
            _evidenceID = evidenceID;
            _description = description;
            _locationFound = locationFound;
            _incidentID = incidentID;
        }

       
        public int EvidenceID
        {
            get { return _evidenceID; }
            set { _evidenceID = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string LocationFound
        {
            get { return _locationFound; }
            set { _locationFound = value; }
        }

        public int IncidentID
        {
            get { return _incidentID; }
            set { _incidentID = value; }
        }
    }
}
