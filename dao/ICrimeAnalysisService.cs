using CrimeAnalysisAndReportingSystem.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrimeAnalysisAndReportingSystem.dao
{
    public interface ICrimeAnalysisService
    {
        // Create a new incident
        bool CreateIncident(Incident incident);

        // Update the status of an incident
        bool UpdateIncidentStatus(int incidentId, string status);

        // Get a list of incidents within a date range
        ICollection<Incident> GetIncidentsInDateRange(DateTime startDate, DateTime endDate);

        // Search for incidents based on various criteria
        ICollection<Incident> SearchIncidents(string IncidentType );

        // Generate incident reports
        Report GenerateIncidentReport(Incident incident);

        // Create a new case and associate it with incidents
       Case CreateCase(string caseDescription, ICollection<Incident> incidents);

        // Get details of a specific case
        Case GetCaseDetails(int caseId);

        // Update case details
        bool UpdateCaseDetails(Case caseToUpdate);

        // Get a list of all cases
        ICollection<Case> GetAllCases();
    }
}
