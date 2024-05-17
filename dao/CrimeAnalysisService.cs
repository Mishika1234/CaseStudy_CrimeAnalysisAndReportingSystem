using CrimeAnalysisAndReportingSystem.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrimeAnalysisAndReportingSystem.util;
using Microsoft.SqlServer.Types;
using CrimeAnalysisAndReportingSystem.exception;

namespace CrimeAnalysisAndReportingSystem.dao
{
    public class CrimeAnalysisService : ICrimeAnalysisService
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;

       
        public CrimeAnalysisService()
        {
            sqlConnection = new SqlConnection(DbConnUtil.GetConnection());
            cmd = new SqlCommand();
        }
        public bool CreateIncident(Incident incident)
        {
            try
            {
                Console.WriteLine("Enter the details for the new incident:");
                Console.Write("Case ID (press Enter if not applicable): ");
                string caseIdInput = Console.ReadLine();
                int? caseID = string.IsNullOrEmpty(caseIdInput) ? null : (int?)Convert.ToInt32(caseIdInput);

                Console.Write("Incident Type: ");
                string incidentType = Console.ReadLine();

                Console.Write("Incident Date (YYYY-MM-DD): ");
                DateTime incidentDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Location: ");
                string location = Console.ReadLine();

                Console.Write("Description: ");
                string description = Console.ReadLine();

                Console.Write("Status (Open/Closed/Under Investigation): ");
                string status = Console.ReadLine();

                Console.Write("Victim ID (if any, press Enter if not applicable): ");
                string victimIdInput = Console.ReadLine();
                int? victimID = string.IsNullOrEmpty(victimIdInput) ? null : (int?)Convert.ToInt32(victimIdInput);

                Console.Write("Suspect ID (if any, press Enter if not applicable): ");
                string suspectIdInput = Console.ReadLine();
                int? suspectID = string.IsNullOrEmpty(suspectIdInput) ? null : (int?)Convert.ToInt32(suspectIdInput);

                Console.Write("Agency ID: ");
                int agencyID = Convert.ToInt32(Console.ReadLine());

                sqlConnection.Open();
                string query = "INSERT INTO Incidents (CaseID, IncidentType, IncidentDate, Location, Description, Status, VictimID, SuspectID, AgencyID) " +
                               "VALUES (@CaseID, @IncidentType, @IncidentDate, @Location, @Description, @Status, @VictimID, @SuspectID, @AgencyID)";

                cmd.Connection = sqlConnection;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@CaseID", caseID ?? (object)DBNull.Value); // Assuming CaseID is nullable
                cmd.Parameters.AddWithValue("@IncidentType", incidentType);
                cmd.Parameters.AddWithValue("@IncidentDate", incidentDate);
                cmd.Parameters.AddWithValue("@Location", location ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@VictimID", victimID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@SuspectID", suspectID ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AgencyID", agencyID);

                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        public bool UpdateIncidentStatus(int incidentId, string status)
        {
            try
            {
                sqlConnection.Open();

             
                string checkQuery = "SELECT COUNT(*) FROM Incidents WHERE IncidentID = @IncidentID";
                cmd.Connection = sqlConnection;
                cmd.CommandText = checkQuery;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@IncidentID", incidentId);

                int incidentCount = (int)cmd.ExecuteScalar();

                
                if (incidentCount == 0)
                {
                    throw new IncidentNumberNotFoundException($"Incident with ID {incidentId} not found.");
                }

               
                string updateQuery = "UPDATE Incidents SET Status = @Status WHERE IncidentID = @IncidentID";
                cmd.CommandText = updateQuery;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@IncidentID", incidentId);

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
            catch (IncidentNumberNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }


        public ICollection<Incident> SearchIncidents(string incidentType)
        {
            List<Incident> incidents = new List<Incident>();
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Incidents WHERE IncidentType = @IncidentType";
                cmd.Connection = sqlConnection;
                cmd.CommandText = query;
               
                cmd.Parameters.AddWithValue("@IncidentType", incidentType);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incidents.Add(new Incident
                    {
                        IncidentID = Convert.ToInt32(reader["IncidentID"]),
                        IncidentType = reader["IncidentType"].ToString(),
                        IncidentDate = Convert.ToDateTime(reader["IncidentDate"]),
                        Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString(),
                        VictimID = reader["VictimID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["VictimID"]) : null,
                        SuspectID = reader["SuspectID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SuspectID"]) : null,
                        AgencyID = Convert.ToInt32(reader["AgencyID"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return incidents;
        }

        public ICollection<Incident> GetIncidentsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<Incident> incidents = new List<Incident>();
            try
            {
                sqlConnection.Open();
                string query = "SELECT * FROM Incidents WHERE IncidentDate BETWEEN @StartDate AND @EndDate";
                cmd.Connection = sqlConnection;
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incidents.Add(new Incident
                    {
                        IncidentID = Convert.ToInt32(reader["IncidentID"]),
                        CaseID = reader["CaseID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["CaseID"]) : null,
                        IncidentType = reader["IncidentType"].ToString(),
                        IncidentDate = Convert.ToDateTime(reader["IncidentDate"]),
                        Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString(),
                        VictimID = reader["VictimID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["VictimID"]) : null,
                        SuspectID = reader["SuspectID"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SuspectID"]) : null,
                        AgencyID = Convert.ToInt32(reader["AgencyID"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return incidents;
        }

        public Case CreateCase(string caseDescription, ICollection<Incident> incidents)
        {
            try
            {
                sqlConnection.Open();

                
                string insertCaseQuery = "INSERT INTO Cases (CaseDescription) VALUES (@CaseDescription); SELECT SCOPE_IDENTITY();";
                cmd.Connection = sqlConnection;
                cmd.CommandText = insertCaseQuery;
                cmd.Parameters.AddWithValue("@CaseDescription", caseDescription);
                int caseId = Convert.ToInt32(cmd.ExecuteScalar());

               
                if (incidents != null && incidents.Any())
                {
                    foreach (var incident in incidents)
                    {
                       
                        incident.CaseID = caseId;
                    }
                }

              
                return new Case
                {
                    CaseID = caseId,
                    CaseDescription = caseDescription,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating case: " + ex.Message);
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }
        }




        public Case GetCaseDetails(int caseId)
        {
            Case caseDetails = null;
            try
            {
                sqlConnection.Open();

                string query = @"SELECT c.CaseID, c.CaseDescription, i.IncidentID, i.IncidentType, i.IncidentDate, i.Location, i.Description, i.Status, i.VictimID, i.SuspectID, i.AgencyID
                         FROM Cases c
                         LEFT JOIN Incidents i ON c.CaseID = i.CaseID
                         WHERE c.CaseID = @CaseIdParam";

                cmd.Connection = sqlConnection;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@CaseIdParam", caseId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    caseDetails = new Case();
                    caseDetails.Incidents = new List<Incident>();

                    while (reader.Read())
                    {
                        if (caseDetails.CaseID == 0)
                        {
                            caseDetails.CaseID = (int)reader["CaseID"];
                            caseDetails.CaseDescription = (string)reader["CaseDescription"];
                        }

                        Incident incident = new Incident
                        {
                            IncidentID = (int)reader["IncidentID"],
                            IncidentType = (string)reader["IncidentType"],
                            IncidentDate = (DateTime)reader["IncidentDate"],
                            Location = reader["Location"] != DBNull.Value ? (string)reader["Location"] : null,
                            Description = (string)reader["Description"],
                            Status = (string)reader["Status"],
                            VictimID = reader["VictimID"] != DBNull.Value ? (int?)reader["VictimID"] : null,
                            SuspectID = reader["SuspectID"] != DBNull.Value ? (int?)reader["SuspectID"] : null,
                            AgencyID = (int)reader["AgencyID"]
                        };
                        caseDetails.Incidents.Add(incident);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return caseDetails;
        }



        public bool UpdateCaseDetails(Case caseToUpdate)
        {
            try
            {
                sqlConnection.Open();
                string query = "UPDATE Cases SET CaseDescription = @CaseDescriptionParam WHERE CaseId = @CaseIdParam";

                cmd.Connection = sqlConnection;
                cmd.CommandText = query;

               
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@CaseDescriptionParam", caseToUpdate.CaseDescription); // Updated parameter name
                cmd.Parameters.AddWithValue("@CaseIdParam", caseToUpdate.CaseID); 

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public ICollection<Case> GetAllCases()
        {
            List<Case> cases = new List<Case>();
            Dictionary<int, Case> caseDictionary = new Dictionary<int, Case>();

            try
            {
                sqlConnection.Open();

                string query = @"
            SELECT c.CaseID, c.CaseDescription, i.IncidentID, i.IncidentType, i.IncidentDate
            FROM Cases c
            LEFT JOIN Incidents i ON c.CaseID = i.CaseID";

                cmd.Connection = sqlConnection;
                cmd.CommandText = query;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int caseId = reader.GetInt32(0);
                        string caseDescription = reader.GetString(1);

                        if (!caseDictionary.ContainsKey(caseId))
                        {
                            Case newCase = new Case
                            {
                                CaseID = caseId,
                                CaseDescription = caseDescription,
                                Incidents = new List<Incident>()
                            };
                            caseDictionary.Add(caseId, newCase);
                            cases.Add(newCase);
                        }

                        if (!reader.IsDBNull(2))
                        {
                            int incidentId = reader.GetInt32(2);
                            string incidentType = reader.GetString(3);
                            DateTime incidentDate = reader.GetDateTime(4);

                            Incident newIncident = new Incident
                            {
                                IncidentID = incidentId,
                                IncidentType = incidentType,
                                IncidentDate = incidentDate
                            };

                            caseDictionary[caseId].Incidents.Add(newIncident);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                sqlConnection.Close();
            }

            return cases;
        }




        public int GenerateReportId()
        {
        Random random = new Random();
        return random.Next(1, 1000);
        }

        Report ICrimeAnalysisService.GenerateIncidentReport(Incident incident)
        {
            throw new NotImplementedException();
        }
    }
}
