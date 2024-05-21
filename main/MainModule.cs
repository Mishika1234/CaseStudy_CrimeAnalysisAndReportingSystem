using CrimeAnalysisAndReportingSystem.entity;
using CrimeAnalysisAndReportingSystem.dao;
using CrimeAnalysisAndReportingSystem.service;


namespace CrimeAnalysisAndReportingSystem

{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICrimeAnalysisServiceImpl crimeAnalysisServiceImpl = new CrimeAnalysisServiceImpl();

            Console.WriteLine("\n------------Crime Analysis and Reporting System----------\n");

            while (true)
            {
                DisplayMenu();

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        crimeAnalysisServiceImpl.CreateIncident();
                        break;
                    case 2:
                        Console.Write("Enter Incident ID: ");
                        int incidentId = int.Parse(Console.ReadLine());
                        Console.Write("Enter new status: ");
                        string status = Console.ReadLine();
                        crimeAnalysisServiceImpl.UpdateIncidentStatus(incidentId, status);
                        break;
                    case 3:
                        crimeAnalysisServiceImpl.GetIncidentsInDateRange();
                        break;
                    case 4:
                        crimeAnalysisServiceImpl.SearchIncidents();
                        break;
                    case 5:
                        crimeAnalysisServiceImpl.GenerateIncidentReport();
                        break;
                    case 6:
                        crimeAnalysisServiceImpl.CreateCase();
                        break;
                    case 7:
                        Console.Write("Enter Case ID: ");
                        int caseId = int.Parse(Console.ReadLine());
                        crimeAnalysisServiceImpl.GetCaseDetails(caseId);
                        break;
                    case 8:
                        crimeAnalysisServiceImpl.UpdateCaseDetails();
                        break;
                    case 9:
                        crimeAnalysisServiceImpl.GetAllCases();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void DisplayMenu()
        {
            
            Console.WriteLine("1. Create Incident");
            Console.WriteLine("2. Update Incident Status");
            Console.WriteLine("3. Get Incidents in Date Range");
            Console.WriteLine("4. Search Incidents");
            Console.WriteLine("5. Generate Incident Report");
            Console.WriteLine("6. Create Case");
            Console.WriteLine("7. Get Case Details");
            Console.WriteLine("8. Update Case Details");
            Console.WriteLine("9. Get All Cases");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
        }
    }
}
