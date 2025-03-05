USE master;  
GO  

CREATE DATABASE FirmsDB;  
GO  

USE FirmsDB;  
GO  

CREATE TABLE SupplyTypes  
(  
    TypeID INT IDENTITY(1,1) PRIMARY KEY,   
    TypeName NVARCHAR(50) NOT NULL  
);  
GO  

CREATE TABLE OfficeSupplies  
(  
    SupplyID INT IDENTITY(1,1) PRIMARY KEY,  
    Name NVARCHAR(100) NOT NULL,  
    SupplyTypeID INT NOT NULL,  
    Quantity INT NOT NULL,  
    UnitCost DECIMAL(10,2) NOT NULL,  
    Description NVARCHAR(255),  
    CONSTRAINT FK_OfficeSupplies_SupplyTypes  
        FOREIGN KEY (SupplyTypeID) REFERENCES SupplyTypes(TypeID)  
);  
GO  

CREATE TABLE SalesManagers  
(  
    ManagerID INT IDENTITY(1,1) PRIMARY KEY,  
    FirstName NVARCHAR(50) NOT NULL,  
    LastName NVARCHAR(50) NOT NULL,  
    Phone NVARCHAR(20)  
);  
GO  

CREATE TABLE BuyerCompanies  
(  
    BuyerID INT IDENTITY(1,1) PRIMARY KEY,  
    CompanyName NVARCHAR(100) NOT NULL,  
    Address NVARCHAR(255)  
);  
GO  

CREATE TABLE Sales  
(  
    SaleID INT IDENTITY(1,1) PRIMARY KEY,  
    SupplyID INT NOT NULL,  
    ManagerID INT NOT NULL,  
    BuyerID INT NOT NULL,  
    SaleDate DATETIME NOT NULL,  
    QuantitySold INT NOT NULL,  
    SalePrice DECIMAL(10,2) NOT NULL,  
    CONSTRAINT FK_Sales_OfficeSupplies  
        FOREIGN KEY (SupplyID) REFERENCES OfficeSupplies(SupplyID),  
    CONSTRAINT FK_Sales_SalesManagers  
        FOREIGN KEY (ManagerID) REFERENCES SalesManagers(ManagerID),  
    CONSTRAINT FK_Sales_BuyerCompanies  
        FOREIGN KEY (BuyerID) REFERENCES BuyerCompanies(BuyerID)  
);  
GO  

INSERT INTO SupplyTypes (TypeName) 
VALUES (N'Ручки'),
       (N'Блокноты'),
       (N'Степлеры'),
       (N'Карандаши');

INSERT INTO OfficeSupplies (Name, SupplyTypeID, Quantity, UnitCost, Description)
VALUES (N'Ручка шариковая синяя', 1, 200, 8.50, N'Шариковая, синяя'),
       (N'Блокнот A5',           2, 50,  25.00, N'Блокнот в твердом переплете'),
       (N'Степлер №10',          3, 20,  150.00, N'Стандартный металлический степлер'),
       (N'Карандаш HB',          4, 100, 5.00,  N'Классический карандаш HB');

INSERT INTO SalesManagers (FirstName, LastName, Phone)
VALUES (N'Иван', N'Иванов', N'111-222-333'),
       (N'Петр', N'Петров', N'222-333-444'),
       (N'Мария', N'Сидорова', N'333-444-555');

INSERT INTO BuyerCompanies (CompanyName, Address)
VALUES (N'Acme Office Supplies', N'123 Main St, New York, NY, USA'),
       (N'ТОВ "КанцЛід"', N'вул. Хрещатик, 1, Київ, Україна'),
       (N'Staples Inc.', N'456 Market St, San Francisco, CA, USA');

INSERT INTO Sales (SupplyID, ManagerID, BuyerID, SaleDate, QuantitySold, SalePrice)
VALUES 
  (1, 1, 1, GETDATE(), 10,  10.00),
  (2, 2, 2, DATEADD(DAY, -1, GETDATE()), 5, 30.00),
  (4, 3, 3, DATEADD(DAY, -2, GETDATE()), 15, 8.00);

-------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE InsertSupplyType
    @TypeName NVARCHAR(50)
AS
BEGIN
    INSERT INTO SupplyTypes(TypeName)
    VALUES (@TypeName);
    SELECT * FROM SupplyTypes WHERE TypeID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE UpdateSupplyType
    @TypeID INT,
    @TypeName NVARCHAR(50)
AS
BEGIN
    UPDATE SupplyTypes
    SET TypeName = @TypeName
    WHERE TypeID = @TypeID;
END
GO

CREATE PROCEDURE DeleteSupplyType
    @TypeID INT
AS
BEGIN
    DELETE FROM SupplyTypes
    WHERE TypeID = @TypeID;
END
GO

CREATE PROCEDURE InsertSalesManager
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Phone NVARCHAR(20)
AS
BEGIN
    INSERT INTO SalesManagers(FirstName, LastName, Phone)
    VALUES(@FirstName, @LastName, @Phone);
    SELECT * FROM SalesManagers WHERE ManagerID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE UpdateSalesManager
    @ManagerID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Phone NVARCHAR(20)
AS
BEGIN
    UPDATE SalesManagers
    SET FirstName = @FirstName,
        LastName  = @LastName,
        Phone     = @Phone
    WHERE ManagerID = @ManagerID;
END
GO

CREATE PROCEDURE DeleteSalesManager
    @ManagerID INT
AS
BEGIN
    DELETE FROM Sales WHERE ManagerID = @ManagerID;
    DELETE FROM SalesManagers WHERE ManagerID = @ManagerID;
END
GO

CREATE PROCEDURE GetAllOfficeSupplies
AS
BEGIN
    SELECT * FROM OfficeSupplies;
END
GO

CREATE PROCEDURE GetAllSupplyTypes
AS
BEGIN
    SELECT * FROM SupplyTypes;
END
GO

CREATE PROCEDURE GetAllManagers
AS
BEGIN
    SELECT * FROM SalesManagers;
END
GO

CREATE PROCEDURE GetAllBuyerCompanies
AS
BEGIN
    SELECT * FROM BuyerCompanies;
END
GO

CREATE PROCEDURE GetMaxQuantitySupplies
AS
BEGIN
    SELECT * FROM OfficeSupplies
    WHERE Quantity = (SELECT MAX(Quantity) FROM OfficeSupplies);
END
GO

CREATE PROCEDURE GetMinQuantitySupplies
AS
BEGIN
    SELECT * FROM OfficeSupplies
    WHERE Quantity = (SELECT MIN(Quantity) FROM OfficeSupplies);
END
GO

CREATE PROCEDURE GetMaxUnitCostSupplies
AS
BEGIN
    SELECT * FROM OfficeSupplies
    WHERE UnitCost = (SELECT MAX(UnitCost) FROM OfficeSupplies);
END
GO

CREATE PROCEDURE GetMinUnitCostSupplies
AS
BEGIN
    SELECT * FROM OfficeSupplies
    WHERE UnitCost = (SELECT MIN(UnitCost) FROM OfficeSupplies);
END
GO

CREATE PROCEDURE ShowOfficeSuppliesByType
    @typeId INT
AS
BEGIN
    SELECT * FROM OfficeSupplies
    WHERE SupplyTypeID = @typeId;
END
GO

CREATE PROCEDURE ShowOfficeSuppliesByManager
    @managerId INT
AS
BEGIN
    SELECT s.* 
    FROM OfficeSupplies s
    JOIN Sales sal ON s.SupplyID = sal.SupplyID
    WHERE sal.ManagerID = @managerId;
END
GO

CREATE PROCEDURE ShowOfficeSuppliesByBuyer
    @buyerId INT
AS
BEGIN
    SELECT s.* 
    FROM OfficeSupplies s
    JOIN Sales sal ON s.SupplyID = sal.SupplyID
    WHERE sal.BuyerID = @buyerId;
END
GO

CREATE PROCEDURE GetLatestSale
AS
BEGIN
    SELECT TOP(1) * FROM Sales
    ORDER BY SaleDate DESC;
END
GO

CREATE PROCEDURE GetAverageQuantityByType
AS
BEGIN
    SELECT st.TypeID, st.TypeName,
           AVG(CONVERT(DECIMAL(18,2), os.Quantity)) AS AvgQuantity
    FROM SupplyTypes st
    JOIN OfficeSupplies os ON st.TypeID = os.SupplyTypeID
    GROUP BY st.TypeID, st.TypeName;
END
GO

CREATE PROCEDURE InsertOfficeSupply
    @Name NVARCHAR(100),
    @SupplyTypeID INT,
    @Quantity INT,
    @UnitCost DECIMAL(10,2),
    @Description NVARCHAR(255)
AS
BEGIN
    INSERT INTO OfficeSupplies(Name, SupplyTypeID, Quantity, UnitCost, Description)
    VALUES(@Name, @SupplyTypeID, @Quantity, @UnitCost, @Description);
    SELECT * FROM OfficeSupplies WHERE SupplyID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE UpdateOfficeSupply
    @SupplyID INT,
    @Name NVARCHAR(100),
    @SupplyTypeID INT,
    @Quantity INT,
    @UnitCost DECIMAL(10,2),
    @Description NVARCHAR(255)
AS
BEGIN
    UPDATE OfficeSupplies
    SET Name = @Name,
        SupplyTypeID = @SupplyTypeID,
        Quantity = @Quantity,
        UnitCost = @UnitCost,
        Description = @Description
    WHERE SupplyID = @SupplyID;
END
GO

CREATE PROCEDURE DeleteOfficeSupply
    @SupplyID INT
AS
BEGIN
    DELETE FROM Sales WHERE SupplyID = @SupplyID;
    DELETE FROM OfficeSupplies WHERE SupplyID = @SupplyID;
END
GO

CREATE PROCEDURE InsertBuyerCompany
    @CompanyName NVARCHAR(100),
    @Address NVARCHAR(255)
AS
BEGIN
    INSERT INTO BuyerCompanies(CompanyName, Address)
    VALUES(@CompanyName, @Address);
    SELECT * FROM BuyerCompanies WHERE BuyerID = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE UpdateBuyerCompany
    @BuyerID INT,
    @CompanyName NVARCHAR(100),
    @Address NVARCHAR(255)
AS
BEGIN
    UPDATE BuyerCompanies
    SET CompanyName = @CompanyName,
        Address = @Address
    WHERE BuyerID = @BuyerID;
END
GO

CREATE PROCEDURE DeleteBuyerCompany
    @BuyerID INT
AS
BEGIN
    DELETE FROM Sales WHERE BuyerID = @BuyerID;
    DELETE FROM BuyerCompanies WHERE BuyerID = @BuyerID;
END
GO

CREATE FUNCTION dbo.ShowAllSalesManagers()
RETURNS TABLE
AS
RETURN (
    SELECT ManagerID, FirstName, LastName, Phone
    FROM SalesManagers
);
GO