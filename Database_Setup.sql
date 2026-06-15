-- CourierManager Database Setup Script
-- Run this in SQL Server Management Studio against your SQL Server instance.
-- It drops and recreates CourierDB so you can re-run it any time.

IF DB_ID('CourierDB') IS NOT NULL
BEGIN
    ALTER DATABASE CourierDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CourierDB;
END
GO

CREATE DATABASE CourierDB;
GO

USE CourierDB;
GO

-- ============ Table 1: Customer ============
CREATE TABLE Customer (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Name      NVARCHAR(100)  NOT NULL,
    Phone     NVARCHAR(20)   NOT NULL,
    Email     NVARCHAR(100)  NULL,
    Address   NVARCHAR(255)  NOT NULL,
    City      NVARCHAR(100)  NOT NULL,
    CreatedAt DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

-- ============ Table 2: Driver ============
CREATE TABLE Driver (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Name        NVARCHAR(100) NOT NULL,
    Phone       NVARCHAR(20)  NOT NULL,
    VehicleType NVARCHAR(20)  NOT NULL,
    VehicleNo   NVARCHAR(20)  NOT NULL,
    City        NVARCHAR(100) NOT NULL,
    IsActive    BIT           NOT NULL DEFAULT 1,
    CreatedAt   DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

-- ============ Table 3: Shipment ============
CREATE TABLE Shipment (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId   INT             NOT NULL,
    CustomerName NVARCHAR(100)   NOT NULL,
    DriverId     INT             NULL,
    DriverName   NVARCHAR(100)   NULL,
    TrackingNo   NVARCHAR(30)    NOT NULL,
    Origin       NVARCHAR(100)   NOT NULL,
    Destination  NVARCHAR(100)   NOT NULL,
    WeightKg     DECIMAL(8,2)    NOT NULL,
    Cost         DECIMAL(10,0)   NOT NULL,
    Status       NVARCHAR(20)    NOT NULL DEFAULT 'Pending',
    PickupDate   DATE            NOT NULL,
    DeliveryDate DATE            NULL,
    Notes        NVARCHAR(500)   NULL,
    CreatedAt    DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Shipment_Customer FOREIGN KEY (CustomerId) REFERENCES Customer(Id),
    CONSTRAINT FK_Shipment_Driver   FOREIGN KEY (DriverId)   REFERENCES Driver(Id)
);
GO

-- ============ Table 4: Payment ============
CREATE TABLE Payment (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    ShipmentId   INT             NOT NULL,
    CustomerId   INT             NOT NULL,
    CustomerName NVARCHAR(100)   NOT NULL,
    TrackingNo   NVARCHAR(30)    NOT NULL,
    Amount       DECIMAL(10,0)   NOT NULL,
    Status       NVARCHAR(20)    NOT NULL DEFAULT 'Unpaid',
    PaymentDate  DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Payment_Shipment FOREIGN KEY (ShipmentId) REFERENCES Shipment(Id)
);
GO

-- ============ Sample data ============
INSERT INTO Customer (Name, Phone, Email, Address, City) VALUES
('Ahmed Raza',    '03001234567', 'ahmed@gmail.com',  '12 Gulberg III',        'Lahore'),
('Sara Khan',     '03112345678', 'sara@hotmail.com', '45 F-7 Markaz',         'Islamabad'),
('Usman Tariq',   '03219876543', NULL,               '78 Clifton Block 5',    'Karachi'),
('Hira Baig',     '03331122334', 'hira@gmail.com',   '3 Model Town',          'Lahore'),
('Bilal Hussain', '03451234567', NULL,               '22 Satellite Town G-9', 'Islamabad');
GO

INSERT INTO Driver (Name, Phone, VehicleType, VehicleNo, City, IsActive) VALUES
('Imran Ali',    '03007654321', 'Bike',  'LEB-1234', 'Lahore',    1),
('Kamran Shah',  '03118765432', 'Van',   'ICT-5678', 'Islamabad', 1),
('Naveed Iqbal', '03219988776', 'Truck', 'KHI-9012', 'Karachi',   1),
('Faisal Mehmood','03331239876', 'Car',  'LEC-3456', 'Lahore',    0);
GO

INSERT INTO Shipment (CustomerId, CustomerName, DriverId, DriverName, TrackingNo, Origin, Destination, WeightKg, Cost, Status, PickupDate, CreatedAt) VALUES
(1, 'Ahmed Raza',    1, 'Imran Ali',    'CR-2026-0001', 'Lahore',    'Karachi',  2.5, 850,  'Delivered',  '2026-05-10', '2026-05-10'),
(2, 'Sara Khan',     2, 'Kamran Shah',  'CR-2026-0002', 'Islamabad', 'Lahore',   0.8, 450,  'In Transit', '2026-05-15', '2026-05-15'),
(3, 'Usman Tariq',   NULL, 'Unassigned','CR-2026-0003', 'Karachi',   'Peshawar', 5.0, 1200, 'Pending',    '2026-06-01', '2026-06-01'),
(4, 'Hira Baig',     1, 'Imran Ali',    'CR-2026-0004', 'Lahore',    'Multan',   1.2, 550,  'Delivered',  '2026-06-05', '2026-06-05'),
(5, 'Bilal Hussain', 2, 'Kamran Shah',  'CR-2026-0005', 'Islamabad', 'Quetta',   3.0, 950,  'In Transit', '2026-06-10', '2026-06-10');
GO

INSERT INTO Payment (ShipmentId, CustomerId, CustomerName, TrackingNo, Amount, Status, PaymentDate) VALUES
(1, 1, 'Ahmed Raza', 'CR-2026-0001', 850, 'Paid',   '2026-05-12'),
(4, 4, 'Hira Baig',  'CR-2026-0004', 550, 'Unpaid', '2026-06-06');
GO
