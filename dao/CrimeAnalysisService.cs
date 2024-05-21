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
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnection()))
                {
                    sqlConnection.Open();

                    string query = "INSERT INTO Incidents (CaseID, IncidentType, IncidentDate, Location, Description, Status, VictimID, SuspectID, AgencyID) " +
                                   "VALUES (@CaseID, @IncidentType, @IncidentDate, @Location, @Description, @Status, @VictimID, @SuspectID, @AgencyID)";

                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@CaseID", (object)incident.CaseID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@IncidentType", incident.IncidentType);
                        cmd.Parameters.AddWithValue("@IncidentDate", incident.IncidentDate);
                        cmd.Parameters.AddWithValue("@Location", string.IsNullOrEmpty(incident.Location) ? DBNull.Value : (object)incident.Location);
                        cmd.Parameters.AddWithValue("@Description", incident.Description);
                        cmd.Parameters.AddWithValue("@Status", incident.Status);
                        cmd.Parameters.AddWithValue("@VictimID", (object)incident.VictimID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SuspectID", (object)incident.SuspectID ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@AgencyID", incident.AgencyID);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return false;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Input Format Error: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
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
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@IncidentType", incidentType);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    incidents.Add(new Incident
                    {
                        IncidentID = Convert.ToInt32(reader["IncidentID"]),
                        CaseID = Convert.ToInt32(reader["CaseID"]),
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
                cmd.Parameters.Clear();

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

        public Case CreateCase(string caseDescription, List<Incident> incidents)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnection()))
                {
                    sqlConnection.Open();

                    // Insert the new case into the Cases table
                    string insertCaseQuery = "INSERT INTO Cases (CaseDescription) VALUES (@CaseDescription); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(insertCaseQuery, sqlConnection);
                    cmd.Parameters.AddWithValue("@CaseDescription", caseDescription);
                    int caseID = Convert.ToInt32(cmd.ExecuteScalar());

                    // Associate the case with the specified incidents
                    foreach (Incident incident in incidents)
                    {
                        string updateIncidentQuery = "UPDATE Incidents SET CaseID = @CaseID WHERE IncidentID = @IncidentID";
                        SqlCommand updateCmd = new SqlCommand(updateIncidentQuery, sqlConnection);
                        updateCmd.Parameters.AddWithValue("@CaseID", caseID);
                        updateCmd.Parameters.AddWithValue("@IncidentID", incident.IncidentID);
                        updateCmd.ExecuteNonQuery();
                    }

                    // Create and return the Case object
                    Case newCase = new Case
                    {
                        CaseID = caseID,
                        CaseDescription = caseDescription,
                        Incidents = incidents
                    };

                    return newCase;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the case: {ex.Message}");
                return null;
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
                cmd.Parameters.Clear();
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

                cmd.Parameters.AddWithValue("@CaseDescriptionParam", caseToUpdate.CaseDescription); 
                // Updated parameter name
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






        public string GenerateIncidentReport(int incidentId)
        {
            string report = string.Empty;

            try
            {
                sqlConnection.Open();

                string query = @"
                    SELECT 
                        Incidents.IncidentID,
                        Incidents.IncidentType,
                        Incidents.IncidentDate,
                        Incidents.Description,
                        Incidents.Status,
                        Reports.ReportingOfficer,
                        Officers.FirstName,
                        Officers.LastName,
                        Cases.CaseDescription
                    FROM 
                        Incidents
                    JOIN 
                        Reports ON Incidents.IncidentID = Reports.IncidentID
                    JOIN 
                        Officers ON Reports.ReportingOfficer = Officers.OfficerID
                    JOIN 
                        Cases ON Incidents.CaseID = Cases.CaseID
                    WHERE
                        Incidents.IncidentID = @IncidentId";

                cmd.Connection = sqlConnection;
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@IncidentId", incidentId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        string incidentType = reader.GetString(1);
                        DateTime incidentDate = reader.GetDateTime(2);
                        string description = reader.GetString(3);
                        string status = reader.GetString(4);
                        int reportingOfficerId = reader.GetInt32(5);
                        string officerFirstName = reader.GetString(6);
                        string officerLastName = reader.GetString(7);
                        string caseDescription = reader.GetString(8);

                        report = $"\n" +
                                 $"Incident ID: {incidentId}\n" +
                                 $"Incident Type: {incidentType}\n" +
                                 $"Incident Date: {incidentDate}\n" +
                                 $"Description: {description}\n" +
                                 $"Status: {status}\n" +
                                 $"Reporting Officer ID: {reportingOfficerId}\n" +
                                 $"Reporting Officer Name: {officerFirstName} {officerLastName}\n" +
                                 $"Associated Case Description: {caseDescription}\n" +
                                 "----------------------------------";
                    }
                    else
                    {
                        report = $"No incident found with ID {incidentId}.";
                    }
                }
            }
            catch (Exception ex)
            {
                report = $"An error occurred: {ex.Message}";
            }
            finally
            {
                sqlConnection.Close();
            }

            return report;
        }

       

    }
}
    
