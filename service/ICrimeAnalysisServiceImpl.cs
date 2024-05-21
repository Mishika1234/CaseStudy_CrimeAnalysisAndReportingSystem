using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrimeAnalysisAndReportingSystem.dao;
using CrimeAnalysisAndReportingSystem.entity;

namespace CrimeAnalysisAndReportingSystem.service
{
    public interface ICrimeAnalysisServiceImpl
    {
        bool CreateIncident();
        bool UpdateIncidentStatus(int incidentId, string status);
        void GetIncidentsInDateRange();
        void SearchIncidents();
        void GenerateIncidentReport();
        void CreateCase();
        void  GetCaseDetails(int caseId);
        void UpdateCaseDetails();
        void GetAllCases();

    }
}
