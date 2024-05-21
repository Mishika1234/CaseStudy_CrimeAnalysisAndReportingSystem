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

        
        public bool CreateIncident()
        {
            try
            {
                Console.WriteLine("Enter the details for the new incident:");

                Console.Write("Case ID (press Enter if not applicable): ");
                int? caseID = ReadNullableInt();

                Console.Write("Incident Type: ");
                string incidentType = Console.ReadLine();

                Console.Write("Incident Date (YYYY-MM-DD): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime incidentDate))
                {
                    Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format.");
                    return false;
                }

                Console.Write("Location (press Enter for null): ");
                string location = Console.ReadLine();

                Console.Write("Description: ");
                string description = Console.ReadLine();

                Console.Write("Status (Open/Closed/Under Investigation): ");
                string status = Console.ReadLine();

                Console.Write("Victim ID (if any, press Enter if not applicable): ");
                int? victimID = ReadNullableInt();

                Console.Write("Suspect ID (if any, press Enter if not applicable): ");
                int? suspectID = ReadNullableInt();

                Console.Write("Agency ID: ");
                if (!int.TryParse(Console.ReadLine(), out int agencyID))
                {
                    Console.WriteLine("Invalid input for Agency ID. Please enter a valid integer.");
                    return false;
                }

                Incident newIncident = new Incident
                {
                    CaseID = caseID,
                    IncidentType = incidentType,
                    IncidentDate = incidentDate,
                    Location = string.IsNullOrEmpty(location) ? null : location,
                    Description = description,
                    Status = status,
                    VictimID = victimID,
                    SuspectID = suspectID,
                    AgencyID = agencyID
                };

                bool success = _crimeAnalysis.CreateIncident(newIncident);

                if (success)
                {
                    Console.WriteLine("\n Incident added successfully.\n ");
                    return true;
                }
                else
                {
                    Console.WriteLine("\n Failed to create incident.\n ");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        private int? ReadNullableInt()
        {
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            else if (!int.TryParse(input, out int parsedValue))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                return null;
            }
            else
            {
                return parsedValue;
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
                    Console.WriteLine("\n Incident status updated successfully.\n");
                else
                   Console.WriteLine("\n Failed to update incident status.\n ");

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
                        Console.WriteLine($"\n Incident ID: {incident.IncidentID}, Case ID: {incident.CaseID}, Type: {incident.IncidentType}, Date: {incident.IncidentDate}, Location: {incident.Location}, Description: {incident.Description}, Status: {incident.Status} \n --------------------------------------------------------");
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
                        Console.WriteLine($"\n Incident ID: {incident.IncidentID},Case ID: {incident.CaseID}, Type: {incident.IncidentType}, Date: {incident.IncidentDate}, Status: {incident.Status}\n ---------------------------------------------------------");
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
                Console.WriteLine("Enter the Incident ID:");
                int incidentId = Convert.ToInt32(Console.ReadLine());

                
                string report = _crimeAnalysis.GenerateIncidentReport(incidentId);

                Console.WriteLine(report);
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

                    caseDetails.CaseDescription = newDescription;

                    bool isUpdated = _crimeAnalysis.UpdateCaseDetails(caseDetails);

                    if (isUpdated)
                    {
                        Console.WriteLine("\nCase details updated successfully.\n");
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to update case details.\n");
                    }
                }
                else
                {
                    Console.WriteLine("\nCase not found.\n");
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
                    Console.WriteLine("\nList of all cases:");

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
                            Console.WriteLine($"    \n");
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
                        Console.WriteLine($" \n\n - Incident ID: {incident.IncidentID}\n    Type: {incident.IncidentType}\n    Date: {incident.IncidentDate}\n    Location: {incident.Location}\n    Description: {incident.Description}\n    Status: {incident.Status}\n    Agency ID: {incident.AgencyID}\n  ----------------------------------------------------------");
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

        public void CreateCase()
        {
            try
            {
                Console.WriteLine("Enter the case description:");
                string caseDescription = Console.ReadLine();

                List<Incident> incidents = new List<Incident>();
                Console.WriteLine("Enter the number of incidents to associate with this case:");
                if (!int.TryParse(Console.ReadLine(), out int numberOfIncidents))
                {
                    Console.WriteLine("Invalid number. Please enter a valid integer.");
                    return;
                }

                for (int i = 0; i < numberOfIncidents; i++)
                {
                    Console.WriteLine($"\nEntering details for incident {i + 1}:");

                    Console.Write("Incident Type: ");
                    string incidentType = Console.ReadLine();

                    Console.Write("Incident Date (YYYY-MM-DD): ");
                    if (!DateTime.TryParse(Console.ReadLine(), out DateTime incidentDate))
                    {
                        Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format.");
                        return;
                    }

                    Console.Write("Location (press Enter for null): ");
                    string location = Console.ReadLine();

                    Console.Write("Description: ");
                    string description = Console.ReadLine();

                    Console.Write("Status (Open/Closed/Under Investigation): ");
                    string status = Console.ReadLine();

                    Console.Write("Victim ID (if any, press Enter if not applicable): ");
                    int? victimID = ReadNullableInt();

                    Console.Write("Suspect ID (if any, press Enter if not applicable): ");
                    int? suspectID = ReadNullableInt();

                    Console.Write("Agency ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int agencyID))
                    {
                        Console.WriteLine("Invalid input for Agency ID. Please enter a valid integer.");
                        return;
                    }

                    Incident incident = new Incident
                    {
                        IncidentType = incidentType,
                        IncidentDate = incidentDate,
                        Location = string.IsNullOrEmpty(location) ? null : location,
                        Description = description,
                        Status = status,
                        VictimID = victimID,
                        SuspectID = suspectID,
                        AgencyID = agencyID
                    };

                    incidents.Add(incident);
                }

                Case newCase = _crimeAnalysis.CreateCase(caseDescription, incidents);
                if (newCase != null)
                {
                    Console.WriteLine($"\nCase created successfully with Case ID: {newCase.CaseID}");
                }
                else
                {
                    Console.WriteLine("\nFailed to create case.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        
    }

}

