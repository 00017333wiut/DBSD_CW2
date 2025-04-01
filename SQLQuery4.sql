--DELETING RECORDS, CONSTAINTS FIRST for the purpose of checking the operations
--Disabling foreign key constraints
ALTER TABLE RentalRecords DROP CONSTRAINT IF EXISTS FK_RentalRecords_Customer;
ALTER TABLE RentalRecords DROP CONSTRAINT IF EXISTS FK_RentalRecords_Artwork;
ALTER TABLE RentalRecords DROP CONSTRAINT IF EXISTS FK_RentalRecords_Staff;
ALTER TABLE Payment DROP CONSTRAINT IF EXISTS FK_Payment_Rental;
ALTER TABLE Manager DROP CONSTRAINT IF EXISTS FK_Manager_Staff;
ALTER TABLE Assistant DROP CONSTRAINT IF EXISTS FK_Assistant_Staff;
ALTER TABLE Promotion DROP CONSTRAINT IF EXISTS FK_Promotion_Customer;
ALTER TABLE ArtworkArtists DROP CONSTRAINT IF EXISTS FK_ArtworkArtists_Artwork;
ALTER TABLE Artwork DROP CONSTRAINT IF EXISTS FK_Artwork_Category;

--Dropping tables in the correct order
DROP TABLE IF EXISTS CustomerContact;
DROP TABLE IF EXISTS Promotion;
DROP TABLE IF EXISTS Payment;
DROP TABLE IF EXISTS RentalRecords;
DROP TABLE IF EXISTS ArtworkArtists;
DROP TABLE IF EXISTS Manager;
DROP TABLE IF EXISTS Assistant;
DROP TABLE IF EXISTS Staff;
DROP TABLE IF EXISTS Artwork;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Location;

--Checking for Orphaned Constraints and Dropping if necessary
IF OBJECT_ID('FK_RentalRecords_Customer', 'F') IS NOT NULL
    ALTER TABLE RentalRecords DROP CONSTRAINT FK_RentalRecords_Customer;
IF OBJECT_ID('FK_RentalRecords_Artwork', 'F') IS NOT NULL
    ALTER TABLE RentalRecords DROP CONSTRAINT FK_RentalRecords_Artwork;
IF OBJECT_ID('FK_RentalRecords_Staff', 'F') IS NOT NULL
    ALTER TABLE RentalRecords DROP CONSTRAINT FK_RentalRecords_Staff;
IF OBJECT_ID('FK_Payment_Rental', 'F') IS NOT NULL
    ALTER TABLE Payment DROP CONSTRAINT FK_Payment_Rental;
IF OBJECT_ID('FK_Manager_Staff', 'F') IS NOT NULL
    ALTER TABLE Manager DROP CONSTRAINT FK_Manager_Staff;
IF OBJECT_ID('FK_Assistant_Staff', 'F') IS NOT NULL
    ALTER TABLE Assistant DROP CONSTRAINT FK_Assistant_Staff;
IF OBJECT_ID('FK_Promotion_Customer', 'F') IS NOT NULL
    ALTER TABLE Promotion DROP CONSTRAINT FK_Promotion_Customer;
IF OBJECT_ID('FK_ArtworkArtists_Artwork', 'F') IS NOT NULL
    ALTER TABLE ArtworkArtists DROP CONSTRAINT FK_ArtworkArtists_Artwork;
IF OBJECT_ID('FK_Artwork_Category', 'F') IS NOT NULL
    ALTER TABLE Artwork DROP CONSTRAINT FK_Artwork_Category;



--CREATING TABLES
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

-- Create Artwork Table
CREATE TABLE Artwork (
    ArtworkID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    Artist VARCHAR(200),
    CategoryID INT,
    Year INT,
    RentalPrice DECIMAL(10,2) CHECK (RentalPrice > 0),
    Availability DATE,
    IsAvailable BIT DEFAULT 1, -- Boolean Field
    ArtworkImage VARBINARY(MAX), -- Binary Field
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);

-- Create Staff Table
CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    Contact VARCHAR(15)
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

-- Create ArtworkArtists Table (Many-to-Many Relationship)
CREATE TABLE ArtworkArtists (
    ArtworkID INT NOT NULL,
    ArtistName VARCHAR(50) NOT NULL,
    PRIMARY KEY (ArtworkID, ArtistName),
    CONSTRAINT FK_ArtworkArtists_Artwork FOREIGN KEY (ArtworkID) REFERENCES Artwork(ArtworkID)
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



--INSERTING SAMPLE DATA
-- Insert Customers (10 Entries) - Modified to use IDENTITY
INSERT INTO Customers (FirstName, LastName, Contact, Email, Street, City, State, PostalCode) VALUES
('John', 'Doe', '123-456-7890', 'john.doe@example.com', '123 Main St', 'New York', 'NY', '10001'),
('Alice', 'Smith', '987-654-3210', 'alice.smith@example.com', '234 Oak St', 'Los Angeles', 'CA', '90001'),
('Michael', 'Brown', '555-333-2222', 'michael.b@example.com', '789 Pine St', 'Chicago', 'IL', '60601'),
('Emma', 'Johnson', '222-444-5555', 'emma.j@example.com', '456 Elm St', 'Houston', 'TX', '77001'),
('Daniel', 'Wilson', '333-666-9999', 'daniel.w@example.com', '567 Cedar St', 'Phoenix', 'AZ', '85001'),
('Sophia', 'Anderson', '777-888-0000', 'sophia.a@example.com', '678 Maple St', 'San Diego', 'CA', '92101'),
('James', 'Miller', '444-111-2222', 'james.m@example.com', '789 Birch St', 'Miami', 'FL', '33101'),
('Olivia', 'Davis', '999-777-6666', 'olivia.d@example.com', '890 Palm St', 'Seattle', 'WA', '98101'),
('Lucas', 'Martinez', '111-222-3333', 'lucas.m@example.com', '901 Redwood St', 'Denver', 'CO', '80201'),
('Isabella', 'Taylor', '555-888-7777', 'isabella.t@example.com', '902 Pineapple St', 'Boston', 'MA', '02101');

-- Insert Staff - Modified to use IDENTITY
INSERT INTO Staff (Name, Role, Contact) VALUES
('David Johnson', 'Manager', '111-222-3333'),
('Emily White', 'Assistant', '444-555-6666'),
('James Smith', 'Clerk', '777-888-9999'),
('Sophie Green', 'Clerk', '123-456-7890'),
('Michael Brown', 'Assistant', '987-654-3210'),
('Anna Black', 'Manager', '555-666-7777'),
('Robert King', 'Assistant', '222-333-4444'),
('Lucy Hall', 'Manager', '888-999-0000'),
('Ethan Carter', 'Clerk', '666-777-8888'),
('Olivia Scott', 'Assistant', '999-000-1111');


-- Insert Categories (10 Entries) - Modified to use IDENTITY
INSERT INTO Category (CategoryName) VALUES
('Abstract'), ('Modern'), ('Classic'), ('Surrealism'), ('Pop Art'),
('Impressionism'), ('Minimalism'), ('Cubism'), ('Realism'), ('Expressionism');

-- Insert Artwork (10 Entries) - Modified to use IDENTITY
INSERT INTO Artwork (Title, Artist, CategoryID, Year, RentalPrice, Availability) VALUES
('Sunset Dreams', 'Emily Johnson', 1, 2020, 50.00, '2025-04-01'),
('The Silent River', 'Daniel Martinez', 2, 2018, 65.00, '2025-04-02'),
('Golden Reflections', 'Sophia Lee', 3, 2022, 80.00, '2025-04-03'),
('Moonlight Sonata', 'James Carter', 4, 2015, 70.00, '2025-04-04'),
('Urban Chaos', 'Alex White', 5, 2019, 55.00, '2025-04-05'),
('Melancholy Fields', 'Hannah Green', 6, 2017, 60.00, '2025-04-06'),
('Neon Dreams', 'Michael Blue', 7, 2021, 75.00, '2025-04-07'),
('Fragmented Visions', 'Olivia Clark', 8, 2016, 90.00, '2025-04-08'),
('Hyperrealistic Portrait', 'Ethan Scott', 9, 2014, 100.00, '2025-04-09'),
('Emotional Brushstrokes', 'Emma Davis', 10, 2013, 85.00, '2025-04-10');

-- We need to update the INSERT statements to work with IDENTITY columns
-- First insert RentalRecords without PaymentID
INSERT INTO RentalRecords (RentalDate, ReturnDate, Fine, CustomerID, ArtworkID, StaffID) VALUES
('2025-03-15', '2025-03-22', 0.00, 1, 1, 1),
('2025-03-18', '2025-03-25', 0.00, 2, 2, 2),
('2025-03-20', NULL, 5.00, 3, 3, 3),
('2025-03-22', '2025-03-29', 0.00, 4, 4, 4),
('2025-03-25', NULL, 10.00, 5, 5, 5),
('2025-03-27', '2025-04-03', 0.00, 6, 6, 6),
('2025-03-29', '2025-04-05', 0.00, 7, 7, 7),
('2025-04-01', NULL, 0.00, 8, 8, 8),
('2025-04-03', '2025-04-10', 15.00, 9, 9, 9),
('2025-04-05', NULL, 0.00, 10, 10, 10);

-- Insert Payment - Modified to use IDENTITY
INSERT INTO Payment (PaymentDate, Amount, PaymentMethod, RentalID) VALUES
('2025-03-22', 50.00, 'Credit Card', 1),
('2025-03-25', 65.00, 'PayPal', 2),
('2025-03-29', 80.00, 'Cash', 4),
('2025-04-03', 70.00, 'Credit Card', 6),
('2025-04-05', 55.00, 'Debit Card', 7),
('2025-04-10', 90.00, 'Wire Transfer', 9);

-- Update RentalRecords with PaymentID after payment records have been inserted
UPDATE RentalRecords SET PaymentID = 1 WHERE RentalID = 1;
UPDATE RentalRecords SET PaymentID = 2 WHERE RentalID = 2;
UPDATE RentalRecords SET PaymentID = 3 WHERE RentalID = 4;
UPDATE RentalRecords SET PaymentID = 4 WHERE RentalID = 6;
UPDATE RentalRecords SET PaymentID = 5 WHERE RentalID = 7;
UPDATE RentalRecords SET PaymentID = 6 WHERE RentalID = 9;

-- Insert Promotions (10 Entries) - Modified to use IDENTITY
INSERT INTO Promotion (DiscountPercentage, StartDate, EndDate, CustomerID) VALUES
(10.00, '2025-04-01', '2025-04-10', 1),
(15.00, '2025-04-05', '2025-04-15', 2),
(20.00, '2025-04-07', '2025-04-20', 3),
(12.00, '2025-04-09', '2025-04-18', 4),
(8.00, '2025-04-12', '2025-04-22', 5),
(5.00, '2025-04-15', '2025-04-25', 6),
(18.00, '2025-04-18', '2025-04-28', 7),
(25.00, '2025-04-20', '2025-04-30', 8),
(30.00, '2025-04-22', '2025-05-02', 9),
(35.00, '2025-04-25', '2025-05-05', 10);

-- Insert Manager records after Staff records exist
INSERT INTO Manager (StaffID, ManagedTeams, AnnualBonus, AuthorityLevel) VALUES
(1, 2, 5000.00, 'Senior'),
(6, 3, 6000.00, 'Executive'),
(8, 1, 4500.00, 'Junior');

-- Insert Assistant records after Staff records exist
INSERT INTO Assistant (StaffID, AssignedTo, ShiftSchedule, TrainingLevel) VALUES
(2, 1, 'Morning', 'Advanced'),
(5, 6, 'Afternoon', 'Intermediate'),
(7, 8, 'Evening', 'Beginner'),
(10, 1, 'Morning', 'Advanced');

-- Insert Location records
INSERT INTO Location (LocationName, Address, ContactNumber) VALUES
('Main Gallery', '123 Art Street, New York, NY', '111-222-3333'),
('Downtown Branch', '456 Culture Avenue, Los Angeles, CA', '444-555-6666'),
('Suburban Exhibition', '789 Gallery Road, Chicago, IL', '777-888-9999');

-- Insert ArtworkArtists relationships
INSERT INTO ArtworkArtists (ArtworkID, ArtistName) VALUES
(1, 'Emily Johnson'),
(2, 'Daniel Martinez'),
(3, 'Sophia Lee'),
(4, 'James Carter'),
(5, 'Alex White'),
(6, 'Hannah Green'),
(7, 'Michael Blue'),
(8, 'Olivia Clark'),
(9, 'Ethan Scott'),
(10, 'Emma Davis');

-- Insert CustomerContact records
INSERT INTO CustomerContact (CustomerID, Contact) VALUES
(1, '123-456-7890'),
(2, '987-654-3210'),
(3, '555-333-2222'),
(4, '222-444-5555'),
(5, '333-666-9999');