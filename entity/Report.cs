using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.entity
{
    public class Report
    {
        private int _reportId; 
        private int _incidentId;
        private int _reportingOfficer;
        private DateTime _reportDate;
        private string _reportDetails;
        private string _status;

        public Report() { }

        public Report(int reportId, int incidentId, int reportingOfficer, DateTime reportDate, string reportDetails, string status)
        {
            _reportId = reportId;
            _incidentId = incidentId;
            _reportingOfficer = reportingOfficer;
            _reportDate = reportDate;
            _reportDetails = reportDetails;
            _status = status;
        }

        public int ReportId
        { 
            get { return _reportId; }
            set { _reportId = value; }
        }

        public int IncidentId 
        {
            get { return _incidentId; }
            set { _incidentId = value; }
        }

        public int ReportingOfficer
        {
            get { return _reportingOfficer; }
            set { _reportingOfficer = value; }
        }

        public DateTime ReportDate
        {
            get { return _reportDate; }
            set { _reportDate = value; }
        }

        public string ReportDetails
        {
            get { return _reportDetails; }
            set { _reportDetails = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
