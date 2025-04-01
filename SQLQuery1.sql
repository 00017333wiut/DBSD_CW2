-- Dropping tables in the correct order to handle dependencies
DROP TABLE IF EXISTS CustomerContact;
DROP TABLE IF EXISTS Promotion;
DROP TABLE IF EXISTS Payment;
DROP TABLE IF EXISTS RentalRecords;
DROP TABLE IF EXISTS Manager;
DROP TABLE IF EXISTS Assistant;
DROP TABLE IF EXISTS Artwork;
DROP TABLE IF EXISTS Staff;
DROP TABLE IF EXISTS ArtworkArtists;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Location;

-- CREATING TABLES
-- Create Customers Table
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Contact VARCHAR(100),
    Email VARCHAR(100),
    Street VARCHAR(100),
    City VARCHAR(50),
    State VARCHAR(50),
    PostalCode VARCHAR(10)
);

-- Create Category Table
CREATE TABLE Category (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL
);

-- Create ArtworkArtists Table
CREATE TABLE ArtworkArtists (
    ArtistName VARCHAR(200) PRIMARY KEY
);

-- Create Staff Table
CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    Contact VARCHAR(15)
);

-- Create Artwork Table (now with Artist as FK to ArtworkArtists)
-- Dropping tables in the correct order to handle dependencies
DROP TABLE IF EXISTS CustomerContact;
DROP TABLE IF EXISTS Promotion;
DROP TABLE IF EXISTS Payment;
DROP TABLE IF EXISTS RentalRecords;
DROP TABLE IF EXISTS Manager;
DROP TABLE IF EXISTS Assistant;
DROP TABLE IF EXISTS Artwork;
DROP TABLE IF EXISTS Staff;
DROP TABLE IF EXISTS ArtworkArtists;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Location;

-- CREATING TABLES
-- Create Customers Table
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Contact VARCHAR(100),
    Email VARCHAR(100),
    Street VARCHAR(100),
    City VARCHAR(50),
    State VARCHAR(50),
    PostalCode VARCHAR(10)
);

-- Create Category Table
CREATE TABLE Category (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(100) NOT NULL
);

-- Create ArtworkArtists Table
CREATE TABLE ArtworkArtists (
    ArtistName VARCHAR(200) PRIMARY KEY
);

-- Create Staff Table
CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    Contact VARCHAR(15)
);

-- Create Artwork Table (now with Artist as FK to ArtworkArtists)
CREATE TABLE Artwork (
    ArtworkID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Artist VARCHAR(200) NOT NULL,
    CategoryID INT,
    Year INT,
    RentalPrice DECIMAL(10,2) CHECK (RentalPrice > 0),
    Availability DATE,
    IsAvailable BIT DEFAULT 1,
    ArtworkImage VARBINARY(MAX),
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID),
    FOREIGN KEY (Artist) REFERENCES ArtworkArtists(ArtistName)
);

-- Create RentalRecords Table
CREATE TABLE RentalRecords (
    RentalID INT IDENTITY(1,1) PRIMARY KEY,
    RentalDate DATE NOT NULL DEFAULT GETDATE(),
    ReturnDate DATE,
    Fine DECIMAL(10,2) DEFAULT 0,
    CustomerID INT NOT NULL,
    ArtworkID INT NOT NULL,
    StaffID INT NOT NULL,
    PaymentID INT,
    CONSTRAINT FK_RentalRecords_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_RentalRecords_Artwork FOREIGN KEY (ArtworkID) REFERENCES Artwork(ArtworkID),
    CONSTRAINT FK_RentalRecords_Staff FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create Payment Table
CREATE TABLE Payment (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    PaymentDate DATE NOT NULL DEFAULT GETDATE(),
    Amount DECIMAL(10,2) NOT NULL CHECK (Amount > 0),
    PaymentMethod VARCHAR(50) NOT NULL,
    RentalID INT NOT NULL,
    CONSTRAINT FK_Payment_Rental FOREIGN KEY (RentalID) REFERENCES RentalRecords(RentalID)
);

-- Create Manager Table
CREATE TABLE Manager (
    StaffID INT PRIMARY KEY,
    ManagedTeams INT CHECK (ManagedTeams >= 0),
    AnnualBonus DECIMAL(10,2),
    AuthorityLevel VARCHAR(50),
    CONSTRAINT FK_Manager_Staff FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create Assistant Table
CREATE TABLE Assistant (
    StaffID INT PRIMARY KEY,
    AssignedTo INT NOT NULL,
    ShiftSchedule VARCHAR(50),
    TrainingLevel VARCHAR(50),
    CONSTRAINT FK_Assistant_Staff FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create Promotion Table
CREATE TABLE Promotion (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,
    DiscountPercentage DECIMAL(5,2) CHECK (DiscountPercentage BETWEEN 0 AND 100),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    CustomerID INT NOT NULL,
    CONSTRAINT FK_Promotion_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Create Location Table
CREATE TABLE Location (
    LocationID INT IDENTITY(1,1) PRIMARY KEY,
    LocationName VARCHAR(100) NOT NULL,
    Address VARCHAR(200),
    ContactNumber VARCHAR(15)
);

-- Create CustomerContact Table
CREATE TABLE CustomerContact (
    CustomerID INT PRIMARY KEY,
    Contact VARCHAR(100),
    CONSTRAINT FK_CustomerContact_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- INSERTING SAMPLE DATA (remain unchanged)
-- ... [rest of the insert statements remain the same]
-- Create RentalRecords Table
CREATE TABLE RentalRecords (
    RentalID INT IDENTITY(1,1) PRIMARY KEY,
    RentalDate DATE NOT NULL DEFAULT GETDATE(),
    ReturnDate DATE,
    Fine DECIMAL(10,2) DEFAULT 0,
    CustomerID INT NOT NULL,
    ArtworkID INT NOT NULL,
    StaffID INT NOT NULL,
    PaymentID INT,
    CONSTRAINT FK_RentalRecords_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    CONSTRAINT FK_RentalRecords_Artwork FOREIGN KEY (ArtworkID) REFERENCES Artwork(ArtworkID),
    CONSTRAINT FK_RentalRecords_Staff FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create Payment Table
CREATE TABLE Payment (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    PaymentDate DATE NOT NULL DEFAULT GETDATE(),
    Amount DECIMAL(10,2) NOT NULL CHECK (Amount > 0),
    PaymentMethod VARCHAR(50) NOT NULL,
    RentalID INT NOT NULL,
    CONSTRAINT FK_Payment_Rental FOREIGN KEY (RentalID) REFERENCES RentalRecords(RentalID)
);

-- Create Manager Table
CREATE TABLE Manager (
    StaffID INT PRIMARY KEY,
    ManagedTeams INT CHECK (ManagedTeams >= 0),
    AnnualBonus DECIMAL(10,2),
    AuthorityLevel VARCHAR(50),
    CONSTRAINT FK_Manager_Staff FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create Assistant Table
CREATE TABLE Assistant (
    StaffID INT PRIMARY KEY,
    AssignedTo INT NOT NULL,
    ShiftSchedule VARCHAR(50),
    TrainingLevel VARCHAR(50),
    CONSTRAINT FK_Assistant_Staff FOREIGN KEY (StaffID) REFERENCES Staff(StaffID)
);

-- Create Promotion Table
CREATE TABLE Promotion (
    PromotionID INT IDENTITY(1,1) PRIMARY KEY,
    DiscountPercentage DECIMAL(5,2) CHECK (DiscountPercentage BETWEEN 0 AND 100),
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    CustomerID INT NOT NULL,
    CONSTRAINT FK_Promotion_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Create Location Table
CREATE TABLE Location (
    LocationID INT IDENTITY(1,1) PRIMARY KEY,
    LocationName VARCHAR(100) NOT NULL,
    Address VARCHAR(200),
    ContactNumber VARCHAR(15)
);

-- Create CustomerContact Table
CREATE TABLE CustomerContact (
    CustomerID INT PRIMARY KEY,
    Contact VARCHAR(100),
    CONSTRAINT FK_CustomerContact_Customer FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- INSERTING SAMPLE DATA (remain unchanged)
-- ... [rest of the insert statements remain the same]