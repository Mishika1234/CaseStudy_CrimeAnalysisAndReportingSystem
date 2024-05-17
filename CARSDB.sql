--Case Study: Crime Analysis and Reporting System (C.A.R.S.)
CREATE DATABASE CARS_DB;
USE CARS_DB;

CREATE TABLE LawEnforcementAgencies (
    AgencyID INT PRIMARY KEY IDENTITY(100,1),
    AgencyName VARCHAR(100) NOT NULL,
    Jurisdiction VARCHAR(100) NOT NULL,
    ContactInformation VARCHAR(25)
);

CREATE TABLE Officers (
    OfficerID INT PRIMARY KEY IDENTITY(200,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    BadgeNumber VARCHAR(20) NOT NULL,
    Rank VARCHAR(50),
    ContactInformation VARCHAR(25),
    AgencyID INT,
    FOREIGN KEY (AgencyID) REFERENCES LawEnforcementAgencies(AgencyID)
);

CREATE TABLE Victims (
    VictimID INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE,
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')), -- Check constraint for Gender
    ContactInformation VARCHAR(25)
);

CREATE TABLE Suspects (
    SuspectID INT PRIMARY KEY IDENTITY(1000,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE,
    Gender CHAR(1) CHECK (Gender IN ('M', 'F')), -- Check constraint for Gender
    ContactInformation VARCHAR(25)
);

CREATE TABLE Incidents (
    IncidentID INT PRIMARY KEY IDENTITY(1,1),
	CaseID INT,   
    IncidentType VARCHAR(100) NOT NULL,
    IncidentDate DATE NOT NULL,
    Location GEOGRAPHY,
    Description TEXT,
    Status VARCHAR(50) NOT NULL CHECK (Status IN ('Open', 'Closed', 'Under Investigation')), -- Check constraint for Status
    VictimID INT,
    SuspectID INT,
    AgencyID INT,
	FOREIGN KEY (CaseID) REFERENCES [Cases](CaseID),
    FOREIGN KEY (VictimID) REFERENCES Victims(VictimID),
    FOREIGN KEY (SuspectID) REFERENCES Suspects(SuspectID),
    FOREIGN KEY (AgencyID) REFERENCES LawEnforcementAgencies(AgencyID)
);

CREATE TABLE Evidence (
    EvidenceID INT PRIMARY KEY IDENTITY(1,1),
    Description TEXT,
    LocationFound VARCHAR(50),
    IncidentID INT,
    FOREIGN KEY (IncidentID) REFERENCES Incidents(IncidentID)
);

CREATE TABLE Reports (
    ReportID INT PRIMARY KEY IDENTITY(1,1),
    IncidentID INT,
    ReportingOfficer INT,
    ReportDate DATETIME,
    ReportDetails VARCHAR(50),
    Status VARCHAR(25) NOT NULL CHECK (Status IN ('Draft', 'Finalized')), 
    FOREIGN KEY (IncidentID) REFERENCES Incidents(IncidentID),
    FOREIGN KEY (ReportingOfficer) REFERENCES Officers(OfficerID)
);

-- LawEnforcementAgencies
INSERT INTO LawEnforcementAgencies (AgencyName, Jurisdiction, ContactInformation)
VALUES 
('City Police Department', 'City of ABC', '1234567890'),
('County Sheriff Office', 'XYZ County', '2345678901'),
('State Highway Patrol', 'PQR', '3456789012'),
('FBI Field Office', ' Metropolitan Area', '4567890123'),
('xyzTeam', ' Metropolitan Area', '5678901234');

-- Officers
INSERT INTO Officers (FirstName, LastName, BadgeNumber, Rank, ContactInformation, AgencyID)
VALUES 
('Manas', 'Soni', 'PD123', 'Detective', '1112223333', 100),
('Vidhya', 'Nahar', 'SO456', 'Deputy Sheriff', '2223334444', 101),
('Mahira', 'Vyas', 'HP789', 'Trooper', '3334445555', 102),
('Suhani', 'Pathak', 'FB123', 'Special Agent', '1234541123', 103),
('Divya', 'Jain', 'SW789', 'Operator', '5655487651', 104);

-- Victims
INSERT INTO Victims (FirstName, LastName, DateOfBirth, Gender, ContactInformation)
VALUES 
('Sonal', 'Sharma', '1990-05-15', 'F', '6655789123'),
('Mohini', 'Vyas', '2000-10-20', 'M', '8899456712'),
('Nikhil', 'Jain', '1989-02-28', 'M', '7788123456'),
('Shruti', 'Pathak', '1972-08-12', 'F', '9988564453'),
('Meher', 'Vyas', '1990-12-25', 'M', '6677234512');

-- Suspects
INSERT INTO Suspects (FirstName, LastName, DateOfBirth, Gender, ContactInformation)
VALUES 
('Neha', 'Yadav', '1988-08-12', 'M', '777-888-9999'),
('Mehek', 'Sharma', '1990-12-25', 'F', '888-999-0000'),
('Parul', 'Soni', '1985-06-30', 'M', '999-000-1111'),
('Payal', 'Purohit', '1979-09-05', 'F', '000-111-2222'),
('Manas', 'Soni', '1997-03-18', 'M', '111-222-3333');

INSERT INTO [Cases] (CaseDescription)
VALUES 
('Investigation into multiple crimes in ABC City.'),
('Financial crime ring targeting seniors.'),
('Multiple thefts and robberies at the new area.'),
('Arson and domestic violence incidents in PQR city.');


-- Incidents
INSERT INTO Incidents (CaseID, IncidentType, IncidentDate, Location, Description, Status, VictimID, SuspectID, AgencyID)
VALUES 
(3,'Robbery', '2023-04-15', NULL, 'Armed robbery at the bank.', 'Open', 1, 1001, 100),
(2,'Assault', '2024-04-20', NULL, 'Physical altercation at the xyz', 'Closed', 2, NULL, 101),
(1,'Hit and Run', '2023-04-15', NULL, 'A car collided with a pedestrian and sped away from the scene', 'Under Investigation', NULL, 1002, 102),
(1,'Kidnapping', '2023-07-24', NULL, 'Child abduction in the park.', 'Open', 3, 1003, 103),
(3,'Burglary', '2023-03-25', NULL, 'Residential break-in.', 'Closed', NULL, 1004, 100),
(4, 'Domestic Violence', '2023-08-25', NULL , 'An incident of domestic violence reported.', 'Closed', 4, 1002, 104);
-- Evidence
INSERT INTO Evidence (Description, LocationFound, IncidentID)
VALUES 
('Gun found', 'Bank', 19),
('CCTV footage from the location', 'xyz', 19),
('Witness statement regarding the vehicle.', 'Street', 22),
('Ransom note left at the scene.', 'Park', 22),
('Fingerprints found at the crime scene.', 'Residence', 23);

-- Reports
INSERT INTO Reports (IncidentID, ReportingOfficer, ReportDate, ReportDetails, Status)
VALUES 
(19, 200, '2023-04-16 08:30:00', 'Initial report filed.', 'Draft'),
(20, 201, '2024-04-22 10:45:00', 'Investigation concluded.', 'Finalized'),
(21, 202, '2023-04-16 13:15:00', 'Investigation ongoing.', 'Draft'),
(22, 203, '2024-07-26 09:00:00', 'Initial report filed.', 'Draft'),
(23, 204, '2023-03-25 11:30:00', 'Investigation concluded.', 'Finalized');

SELECT * FROM LawEnforcementAgencies;
SELECT * FROM Officers;
SELECT * FROM Victims;
SELECT * FROM Suspects;
SELECT * FROM Incidents;
SELECT * FROM Evidence;
SELECT * FROM Reports;
select * from cases;

CREATE TABLE [Cases](
    CaseID INT PRIMARY KEY IDENTITY(1,1),
    CaseDescription VARCHAR(255),
);




