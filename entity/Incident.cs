using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class Incident
    {
        private int _incidentID;
        public int? _caseID;
        private string _incidentType;
        private DateTime _incidentDate;
        private string _location;
        private string _description;
        private string _status;
        private int? _victimID;
        private int? _suspectID;
        private int _agencyID;

        public Incident() { }

        public Incident(int incidentID, string incidentType, DateTime incidentDate, string location, string description, string status, int? victimID, int? suspectID, int agencyID)
        {
            _incidentID = incidentID;
            _incidentType = incidentType;
            _incidentDate = incidentDate;
            _location = location;
            _description = description;
            _status = status;
            _victimID = victimID;
            _suspectID = suspectID;
            _agencyID = agencyID;
        }

        public int IncidentID
        {
            get { return _incidentID; }
            set { _incidentID = value; }
        }

        public string IncidentType
        {
            get { return _incidentType; }
            set { _incidentType = value; }
        }

        public DateTime IncidentDate
        {
            get { return _incidentDate; }
            set { _incidentDate = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int? VictimID
        {
            get { return _victimID; }
            set { _victimID = value; }
        }

        public int? SuspectID
        {
            get { return _suspectID; }
            set { _suspectID = value; }
        }

        public int AgencyID
        {
            get { return _agencyID; }
            set { _agencyID = value; }
        }

        public int? CaseID
        {
            get { return _caseID; }
            set { _caseID = value; }
        }

    }
}

