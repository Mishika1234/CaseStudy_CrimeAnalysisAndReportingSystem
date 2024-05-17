using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class LawEnforcementAgency
    {
       
        private int _agencyID;
        private string _agencyName;
        private string _jurisdiction;
        private string _contactInformation;


        
        public LawEnforcementAgency(int agencyID, string agencyName, string jurisdiction, string contactInformation)
        {
            _agencyID = agencyID;
            _agencyName = agencyName;
            _jurisdiction = jurisdiction;
            _contactInformation = contactInformation;
        }

      
        public int AgencyID
        {
            get { return _agencyID; }
            set { _agencyID = value; }
        }

        public string AgencyName
        {
            get { return _agencyName; }
            set { _agencyName = value; }
        }

        public string Jurisdiction
        {
            get { return _jurisdiction; }
            set { _jurisdiction = value; }
        }

        public string ContactInformation
        {
            get { return _contactInformation; }
            set { _contactInformation = value; }
        }
    }
}