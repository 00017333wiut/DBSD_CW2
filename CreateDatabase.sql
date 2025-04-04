DROP TABLE IF EXISTS CustomerContact;
DROP TABLE IF EXISTS Promotion;
DROP TABLE IF EXISTS Payment;
DROP TABLE IF EXISTS Artwork;
DROP TABLE IF EXISTS RentalRecords;
DROP TABLE IF EXISTS Manager;
DROP TABLE IF EXISTS Assistant;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS Artists;
DROP TABLE IF EXISTS Staff;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Location;

-- Now create tables
-- Create Customers Table
CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
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

-- Create Artists Table
CREATE TABLE Artists (
    ArtistID INT IDENTITY(1,1) PRIMARY KEY,
    ArtistName VARCHAR(200) NOT NULL
);

-- Create Staff Table
CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Role VARCHAR(50) NOT NULL,
    Contact VARCHAR(15)
);

-- Create Artwork Table
CREATE TABLE Artwork (
    ArtworkID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(100) NOT NULL,
    ArtistID INT,
    CategoryID INT,
    Year INT,
    RentalPrice DECIMAL(10,2) CHECK (RentalPrice > 0),
    Availability DATE,
    IsAvailable BIT DEFAULT 1,
    ArtworkImage VARBINARY(MAX),
    FOREIGN KEY (ArtistID) REFERENCES Artists(ArtistID),
    FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
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

-- INSERTING SAMPLE DATA

-- Insert Customers
INSERT INTO Customers (Title, LastName, Contact, Email, Street, City, State, PostalCode) VALUES
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

-- Insert Staff
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

-- Insert Categories
INSERT INTO Category (CategoryName) VALUES
('Abstract'), ('Modern'), ('Classic'), ('Surrealism'), ('Pop Art'),
('Impressionism'), ('Minimalism'), ('Cubism'), ('Realism'), ('Expressionism');

-- Insert Artists
INSERT INTO Artists (ArtistName) VALUES
('Pablo Picasso'),
('Vincent van Gogh'),
('Leonardo da Vinci'),
('Claude Monet'),
('Salvador Dali'),
('Andy Warhol'),
('Frida Kahlo'),
('Jackson Pollock'),
('Georgia O''Keeffe'),
('Michelangelo');

-- Insert Artwork (corrected column order and values)
INSERT INTO Artwork (Title, ArtistID, CategoryID, Year, RentalPrice, Availability, IsAvailable) VALUES
('Starry Night', 2, 6, 1889, 100.00, '2025-04-01', 1),
('Mona Lisa', 3, 3, 1503, 200.00, '2025-04-01', 1),
('The Persistence of Memory', 5, 4, 1931, 150.00, '2025-04-01', 1),
('Campbell''s Soup Cans', 6, 5, 1962, 120.00, '2025-04-01', 1),
('The Two Fridas', 7, 9, 1939, 130.00, '2025-04-01', 1),
('No. 5, 1948', 8, 1, 1948, 140.00, '2025-04-01', 1),
('Jimson Weed', 9, 2, 1936, 110.00, '2025-04-01', 1),
('David', 10, 3, 1504, 180.00, '2025-04-01', 1),
('Guernica', 1, 8, 1937, 170.00, '2025-04-01', 1),
('Water Lilies', 4, 6, 1919, 160.00, '2025-04-01', 1);

-- Insert RentalRecords
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

-- Insert Payment
INSERT INTO Payment (PaymentDate, Amount, PaymentMethod, RentalID) VALUES
('2025-03-22', 50.00, 'Credit Card', 1),
('2025-03-25', 65.00, 'PayPal', 2),
('2025-03-29', 80.00, 'Cash', 4),
('2025-04-03', 70.00, 'Credit Card', 6),
('2025-04-05', 55.00, 'Debit Card', 7),
('2025-04-10', 90.00, 'Wire Transfer', 9);

-- Update RentalRecords with PaymentID
UPDATE RentalRecords SET PaymentID = 1 WHERE RentalID = 1;
UPDATE RentalRecords SET PaymentID = 2 WHERE RentalID = 2;
UPDATE RentalRecords SET PaymentID = 3 WHERE RentalID = 4;
UPDATE RentalRecords SET PaymentID = 4 WHERE RentalID = 6;
UPDATE RentalRecords SET PaymentID = 5 WHERE RentalID = 7;
UPDATE RentalRecords SET PaymentID = 6 WHERE RentalID = 9;

-- Insert Promotions
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

-- Insert Manager records
INSERT INTO Manager (StaffID, ManagedTeams, AnnualBonus, AuthorityLevel) VALUES
(1, 2, 5000.00, 'Senior'),
(6, 3, 6000.00, 'Executive'),
(8, 1, 4500.00, 'Junior');

-- Insert Assistant records
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

-- Insert CustomerContact records
INSERT INTO CustomerContact (CustomerID, Contact) VALUES
(1, '123-456-7890'),
(2, '987-654-3210'),
(3, '555-333-2222'),
(4, '222-444-5555'),
(5, '333-666-9999');