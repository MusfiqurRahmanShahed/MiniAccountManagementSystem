-- 1. Create Table Type
CREATE TYPE dbo.VoucherLineType AS TABLE
(
    AccountId INT,
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2)
);
GO

-- 2. Create Vouchers Table
CREATE TABLE Vouchers (
    Id INT PRIMARY KEY IDENTITY,
    Date DATE,
    ReferenceNo NVARCHAR(100)
);
GO

-- 3. Create VoucherDetails Table
CREATE TABLE VoucherDetails (
    Id INT PRIMARY KEY IDENTITY,
    VoucherId INT FOREIGN KEY REFERENCES Vouchers(Id),
    AccountId INT,
    Debit DECIMAL(18,2),
    Credit DECIMAL(18,2)
);
GO

-- 4. Create Stored Procedure
CREATE PROCEDURE sp_SaveVoucher
    @Date DATE,
    @ReferenceNo NVARCHAR(100),
    @VoucherLines dbo.VoucherLineType READONLY
AS
BEGIN
    -- Insert into Vouchers table
    INSERT INTO Vouchers (Date, ReferenceNo)
    VALUES (@Date, @ReferenceNo);

    DECLARE @VoucherId INT = SCOPE_IDENTITY();

    -- Insert into VoucherDetails table
    INSERT INTO VoucherDetails (VoucherId, AccountId, Debit, Credit)
    SELECT @VoucherId, AccountId, Debit, Credit
    FROM @VoucherLines;
END
GO
