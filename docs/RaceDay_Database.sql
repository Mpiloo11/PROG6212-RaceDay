CREATE DATABASE RaceDayDB;
GO

USE RaceDayDB;
GO

-- Users table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
    Role VARCHAR(20) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Event types table
CREATE TABLE EventTypes (
    EventTypeID INT IDENTITY(1,1) PRIMARY KEY,
    TypeName VARCHAR(30) NOT NULL UNIQUE
);
GO

-- Events table
CREATE TABLE Events (
    EventID INT IDENTITY(1,1) PRIMARY KEY,
    EventName VARCHAR(150) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    EventDate DATE NOT NULL,
    Location VARCHAR(200) NOT NULL,
    DistanceKM DECIMAL(6,2) NOT NULL,
    EventTypeID INT NOT NULL,
    OrganiserID INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (EventTypeID) REFERENCES EventTypes(EventTypeID),
    FOREIGN KEY (OrganiserID) REFERENCES Users(UserID)
);
GO

-- Categories table
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    EventID INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    MinimumAge INT,
    MaximumAge INT,
    DistanceKM DECIMAL(6,2),

    FOREIGN KEY (EventID) REFERENCES Events(EventID)
);
GO

-- Enrolments table
CREATE TABLE Enrolments (
    EnrolmentID INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    EventID INT NOT NULL,
    CategoryID INT NOT NULL,
    EnrolmentDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(20) DEFAULT 'Active',

    FOREIGN KEY (ParticipantID) REFERENCES Users(UserID),
    FOREIGN KEY (EventID) REFERENCES Events(EventID),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),

    UNIQUE (ParticipantID, EventID)
);
GO

-- Results table
CREATE TABLE Results (
    ResultID INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentID INT NOT NULL,
    FinishTime TIME NOT NULL,
    FinishingPosition INT NOT NULL,
    ResultDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (EnrolmentID) REFERENCES Enrolments(EnrolmentID),

    UNIQUE (EnrolmentID)
);
GO

-- Add event types
INSERT INTO EventTypes (TypeName)
VALUES ('Run'), ('Walk'), ('Cycle');
GO

-- Add users
INSERT INTO Users
    (FirstName, LastName, Email, PasswordHash, PhoneNumber, Role)
VALUES
    ('Thabo', 'Mokoena', 'thabo.organiser@raceday.co.za',
     'passwordhash1', '0710000001', 'Organiser'),

    ('Lerato', 'Dlamini', 'lerato.organiser@raceday.co.za',
     'passwordhash2', '0710000002', 'Organiser'),

    ('Sipho', 'Nkosi', 'sipho.participant@raceday.co.za',
     'passwordhash3', '0720000001', 'Participant'),

    ('Amahle', 'Khumalo', 'amahle.participant@raceday.co.za',
     'passwordhash4', '0720000002', 'Participant');
GO

-- Add events
INSERT INTO Events
    (EventName, Description, EventDate, Location,
     DistanceKM, EventTypeID, OrganiserID)
VALUES
    ('Johannesburg City Run',
     'Road running event through Johannesburg.',
     '2026-10-10',
     'Johannesburg, Gauteng',
     10.00,
     1,
     1),

    ('Krugersdorp Community Walk',
     'Community road walking event.',
     '2026-10-24',
     'Krugersdorp, Gauteng',
     5.00,
     2,
     2),

    ('Gauteng Cycle Challenge',
     'Road cycling challenge for participants.',
     '2026-11-07',
     'Gauteng',
     40.00,
     3,
     1);
GO

-- Add categories
INSERT INTO Categories
    (EventID, CategoryName, MinimumAge, MaximumAge, DistanceKM)
VALUES
    (1, 'Under 20', 0, 19, NULL),
    (1, '20-39', 20, 39, NULL),
    (2, 'Open 5KM', NULL, NULL, 5.00),
    (3, 'Open 40KM', NULL, NULL, 40.00);
GO

-- Add sample enrolments
INSERT INTO Enrolments
    (ParticipantID, EventID, CategoryID, Status)
VALUES
    (3, 1, 2, 'Completed'),
    (4, 2, 3, 'Active'),
    (3, 3, 4, 'Active');
GO

-- Add sample result
INSERT INTO Results
    (EnrolmentID, FinishTime, FinishingPosition)
VALUES
    (1, '00:52:35', 14);
GO

-- Check the tables
SELECT * FROM Users;
SELECT * FROM EventTypes;
SELECT * FROM Events;
SELECT * FROM Categories;
SELECT * FROM Enrolments;
SELECT * FROM Results;
GO
