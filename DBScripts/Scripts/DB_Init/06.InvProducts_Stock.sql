--CREATE DATABASE NSinvoiceBilling;
--GO
CREATE TABLE [dbo].[InvProducts_Stock] (
    [StockTnxId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [ProductCode] UNIQUEIDENTIFIER NOT NULL ,
    [UnitType] int NOT NULL,
    [Quantity] Integer NOT NULL,
	[TxnType] int NOT NULL,
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER,
    CONSTRAINT FK_InvProducts_Stock_InvProducts 
    FOREIGN KEY (ProductCode) REFERENCES InvProducts(ProductCode),
	CONSTRAINT Chk_InvProducts_Stock_Quantity_not_zero CHECK (Quantity <> 0)
);
Go
DENY UPDATE ON [dbo].[InvProducts_Stock] TO [KrgApiUser];
GO

Create OR Alter View VStock AS
SELECT
      Stock.[ProductCode],
      P.[ProductName],
      P.[UnitType],
      unit.[Name] AS UnitName,
      unit.[ShName] AS ShortName,
      unit.[PluralName] AS PluralUnitName,
      unit.[ShPluralName] AS PluralShortName,
      CONCAT(unit.[Name], ' (', unit.[ShName], ')') AS UnitNameDetail,
      TxnType.Name as TransactionType,
      Stock.Quantity,
      Stock.[CreatedOn],
      Stock.[CreatedBy],

      -- ⭐ NEW COLUMN: StockDisplay
      CASE 
          WHEN Stock.Quantity <= 1 
               THEN CONCAT(Stock.Quantity, ' ', unit.[Name], ' (', unit.[ShName], ')')
          ELSE CONCAT(Stock.Quantity, ' ', unit.[PluralName], ' (', unit.[ShPluralName], ')')
      END AS StockDisplay

FROM InvProducts_Stock Stock
INNER JOIN InvProducts P 
       ON P.ProductCode = Stock.ProductCode 
INNER JOIN InvConstant TxnType 
       ON TxnType.[Key] = Stock.TxnType 
      AND TxnType.EntityId = 'InvProducts' 
      AND TxnType.Category = 'StockTxnType'
INNER JOIN InvConstant unit 
       ON unit.[Key] = Stock.UnitType 
      AND unit.EntityId = 'InvProducts' 
      AND unit.Category = 'UnitType';
GO

CREATE OR ALTER TRIGGER trg_InvProducts_Stock_Quantity
ON InvProducts_Stock
FOR INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProductCode UNIQUEIDENTIFIER;

    -- Handle multi-row inserts safely
    SELECT @ProductCode = ProductCode
    FROM inserted;

    -- Calculate total stock AFTER the insert
    DECLARE @TotalStock INT;

    SELECT @TotalStock = SUM(Quantity)
    FROM InvProducts_Stock
    WHERE ProductCode = @ProductCode;

    -- If total stock becomes negative, block the insert
    IF (@TotalStock < 0)
    BEGIN
        RAISERROR ('Total stock for this product cannot be negative.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
	UPDATE InvProducts
    SET CurrentStock = @TotalStock
    WHERE ProductCode = @ProductCode;
END;
GO