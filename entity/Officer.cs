using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class Officer
    {
        private int _officerID;
        private string _firstName;
        private string _lastName;
        private string _badgeNumber;
        private string _rank;
        private string _contactInformation;
        private int _agencyID;


        public Officer(int officerID, string firstName, string lastName, string badgeNumber, string rank, string contactInformation, int agencyID)
        {
            _officerID = officerID;
            _firstName = firstName;
            _lastName = lastName;
            _badgeNumber = badgeNumber;
            _rank = rank;
            _contactInformation = contactInformation;
            _agencyID = agencyID;
        }

        public int OfficerID
        {
            get { return _officerID; }
            set { _officerID = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string BadgeNumber
        {
            get { return _badgeNumber; }
            set { _badgeNumber = value; }
        }

        public string Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        public string ContactInformation
        {
            get { return _contactInformation; }
            set { _contactInformation = value; }
        }

        public int AgencyID
        {
            get { return _agencyID; }
            set { _agencyID = value; }
        }
    }
}
