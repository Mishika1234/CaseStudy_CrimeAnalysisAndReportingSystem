using CrimeAnalysisAndReportingSystem.dao;
using CrimeAnalysisAndReportingSystem.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using CrimeAnalysisAndReportingSystem.exception;



namespace CrimeAnalysisAndReportingSystem.service
{
    internal class CrimeAnalysisServiceImpl : ICrimeAnalysisServiceImpl
    {

        readonly ICrimeAnalysisService _crimeAnalysis;
        public CrimeAnalysisServiceImpl()
        {
            _crimeAnalysis = new CrimeAnalysisService();
        }

        public bool CreateIncident(Incident incident)
        {
            try
            {
                Console.WriteLine("Enter the details for the new incident:");

                Console.Write("Case ID (press Enter if not applicable): ");
                string caseIDInput = Console.ReadLine();
                int? caseID = string.IsNullOrEmpty(caseIDInput) ? null : TryParseNullableInt(caseIDInput);

                Console.Write("Incident Type: ");
                string incidentType = Console.ReadLine();

                Console.Write("Incident Date (YYYY-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime incidentDate))
                {
                    Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format.");
                    return false;
                }

                Console.Write("Location (press Enter for null): ");
                string locationInput = Console.ReadLine();
                string location = string.IsNullOrEmpty(locationInput) ? null : locationInput;

                Console.Write("Description: ");
                string description = Console.ReadLine();

                Console.Write("Status (Open/Closed/Under Investigation): ");
                string status = Console.ReadLine();

                Console.Write("Victim ID (if any, press Enter if not applicable): ");
                int? victimID = TryParseNullableInt(Console.ReadLine());

                Console.Write("Suspect ID (if any, press Enter if not applicable): ");
                int? suspectID = TryParseNullableInt(Console.ReadLine());

                Console.Write("Agency ID: ");
                int agencyID = int.Parse(Console.ReadLine());

                Incident newIncident = new Incident
                {
                    CaseID = caseID,
                    IncidentType = incidentType,
                    IncidentDate = incidentDate,
                    Location = location,
                    Description = description,
                    Status = status,
                    VictimID = victimID,
                    SuspectID = suspectID,
                    AgencyID = agencyID
                };

                bool success = _crimeAnalysis.CreateIncident(newIncident);


                if (success)
                {
                    Console.WriteLine("Incident added successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to create incident.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public bool UpdateIncidentStatus(int incidentId, string status)
        {
            try
            {
                Console.WriteLine("Enter the new status for the incident:");
                string newStatus = Console.ReadLine();

             

                bool success = _crimeAnalysis.UpdateIncidentStatus(incidentId, newStatus);

                if (success)
                    Console.WriteLine("Incident status updated successfully.");
                else
                   Console.WriteLine("Failed to update incident status.");

                return success;
            }

            
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }




        public void GetIncidentsInDateRange()
        {
            try
            {
                Console.WriteLine("Enter the start date (yyyy-MM-dd):");
                DateTime startDate = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Enter the end date (yyyy-MM-dd):");
                DateTime endDate = DateTime.Parse(Console.ReadLine());

                ICollection<Incident> incidents = _crimeAnalysis.GetIncidentsInDateRange(startDate, endDate);

                if (incidents.Count > 0)
                {
                    Console.WriteLine("Incidents within the specified date range:");
                    foreach (var incident in incidents)
                    {
                        Console.WriteLine($"Incident ID: {incident.IncidentID}, Case ID: {incident.CaseID}, Type: {incident.IncidentType}, Date: {incident.IncidentDate}, Location: {incident.Location}, Description: {incident.Description}, Status: {incident.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("No incidents found within the specified date range.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void SearchIncidents()
        {
            try
            {
                Console.WriteLine("Enter the incident type to search:");
                string incidentType = Console.ReadLine();

                ICollection<Incident> incidents = _crimeAnalysis.SearchIncidents(incidentType);

                if (incidents.Count > 0)
                {
                    Console.WriteLine($"Incidents of type '{incidentType}':");
                    foreach (var incident in incidents)
                    {
                        Console.WriteLine($"Incident ID: {incident.IncidentID},Case ID: {incident.CaseID}, Type: {incident.IncidentType}, Date: {incident.IncidentDate}, Status: {incident.Status}");
                    }
                }
           
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void GenerateIncidentReport()
        {
            try
            {
                Console.WriteLine("Enter details for the incident report:");

                
                Console.Write("Incident Type: ");
                string incidentType = Console.ReadLine();
               
               
                Incident newIncident = new Incident
                {
                    IncidentType = incidentType,
                   
                };

                
                Report report = _crimeAnalysis.GenerateIncidentReport(newIncident);

                
                Console.WriteLine("Generated Incident Report:");
                Console.WriteLine($"Report ID: {report.ReportId}");
                Console.WriteLine($"Report Date: {report.ReportDate}");
                Console.WriteLine($"Details: {report.ReportDetails}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void CreateCase()
        {
            try
            {
                Console.WriteLine("Enter details for the new case:");

                Console.Write("Case Description: ");
                string caseDescription = Console.ReadLine();

                // Create a new case object
                Case newCase = new Case
                {
                    CaseDescription = caseDescription,
                    Incidents = new List<Incident>() // Initialize the Incidents property
                };

                Console.WriteLine("Enter incident details for the case (press Enter to stop):");
                while (true)
                {
                    Console.WriteLine("Incident:");
                    Console.Write("Incident Type: ");
                    string incidentType = Console.ReadLine();

                    
                    Incident newIncident = new Incident
                    {
                        IncidentType = incidentType
                    };

                   
                    newCase.Incidents.Add(newIncident);

                    Console.Write("Add another incident? (Y/N): ");
                    string response = Console.ReadLine().ToUpper();
                    if (response != "Y")
                        break;
                }

                
               
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }



        public void UpdateCaseDetails()
        {
            try
            {
                Console.Write("Enter the Case ID: ");
                int caseId = Convert.ToInt32(Console.ReadLine());

                Case caseDetails = _crimeAnalysis.GetCaseDetails(caseId);

                if (caseDetails != null)
                {
                    Console.WriteLine("Current Case Details:");
                    Console.WriteLine($"Case ID: {caseDetails.CaseID}");
                    Console.WriteLine($"Case Description: {caseDetails.CaseDescription}");

                    Console.WriteLine("Enter the new Case Description:");
                    string newDescription = Console.ReadLine();

                    // Update the case description in the retrieved case object
                    caseDetails.CaseDescription = newDescription;

                    // Call the service method to update the case details in the database
                    bool isUpdated = _crimeAnalysis.UpdateCaseDetails(caseDetails);

                    if (isUpdated)
                    {
                        Console.WriteLine("Case details updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update case details.");
                    }
                }
                else
                {
                    Console.WriteLine("Case not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public void GetAllCases()
        {
            try
            {
                ICollection<Case> allCases = _crimeAnalysis.GetAllCases();

                if (allCases.Count > 0)
                {
                    Console.WriteLine("List of all cases:");

                    foreach (var caseDetails in allCases)
                    {
                        Console.WriteLine($"Case ID: {caseDetails.CaseID}");
                        Console.WriteLine($"Case Description: {caseDetails.CaseDescription}");

                        // Print associated incidents
                        Console.WriteLine("Associated Incidents:");
                        foreach (var incident in caseDetails.Incidents)
                        {
                            Console.WriteLine($"  - Incident ID: {incident.IncidentID}");
                            Console.WriteLine($"    Type: {incident.IncidentType}");
                            Console.WriteLine($"    Date: {incident.IncidentDate}");
                            Console.WriteLine($"    Location: {incident.Location}");
                            Console.WriteLine($"    Description: {incident.Description}");
                            Console.WriteLine($"    Status: {incident.Status}");
                            Console.WriteLine($"    Agency ID: {incident.AgencyID}");
                        }

                        Console.WriteLine("----------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("No cases found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void GetCaseDetails(int caseId)
        {
            try
            {
                Case caseDetails = _crimeAnalysis.GetCaseDetails(caseId);

                if (caseDetails != null)
                {
                    Console.WriteLine($"Case Details:\nCase ID: {caseDetails.CaseID}\nCase Description: {caseDetails.CaseDescription}");

                    Console.WriteLine("Associated Incidents:");
                    foreach (var incident in caseDetails.Incidents)
                    {
                        Console.WriteLine($"  - Incident ID: {incident.IncidentID}\n    Type: {incident.IncidentType}\n    Date: {incident.IncidentDate}\n    Location: {incident.Location}\n    Description: {incident.Description}\n    Status: {incident.Status}\n    Agency ID: {incident.AgencyID}");
                    }
                }
                else
                {
                    Console.WriteLine($"Case with ID {caseId} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

















        private int? TryParseNullableInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }

            return null;
        }


    }
}
