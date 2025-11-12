use NSinvoiceBilling
go
CREATE OR ALTER TRIGGER trg_InvCustomers_Audit
ON InvCustomers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvCustomers_Audit (
        [CustomerCode],
        [CustomerName],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [CustomerAddress],
        [GST],
        [CustomerLogo],
        [CustomerLogoMime],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy],
        OperationType
    )
    SELECT
        i.[CustomerCode],
        i.[CustomerName],
        i.[CustomerEmail],
        i.[ContactNo],
        i.[SecnContactNo],
        i.[CustomerAddress],
        i.[GST],
        i.[CustomerLogo],
        i.[CustomerLogoMime],
        i.[CreatedOn],
        i.CreatedBy,
        i.[ModifiedOn],
        i.ModifiedBy,
        'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[CustomerCode] = d.[CustomerCode]
    WHERE d.[CustomerCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO InvCustomers_Audit (
        [CustomerCode],
        [CustomerName],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [CustomerAddress],
        [GST],
        [CustomerLogo],
        [CustomerLogoMime],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy],
        OperationType
    )
    SELECT
        d.[CustomerCode],
        d.[CustomerName],
        d.[CustomerEmail],
        d.[ContactNo],
        d.[SecnContactNo],
        d.[CustomerAddress],
        d.[GST],
        d.[CustomerLogo],
        d.[CustomerLogoMime],
        d.[CreatedOn],
        d.CreatedBy,
        d.[ModifiedOn],
        d.ModifiedBy,
        'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[CustomerCode] = d.[CustomerCode]
    WHERE i.[CustomerCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO InvCustomers_Audit (
        [CustomerCode],
        [CustomerName],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [CustomerAddress],
        [GST],
        [CustomerLogo],
        [CustomerLogoMime],
        [CreatedOn],
        [CreatedBy],
        [ModifiedOn],
        [ModifiedBy],
        OperationType
    )
    SELECT
        i.[CustomerCode],
        i.[CustomerName],
        i.[CustomerEmail],
        i.[ContactNo],
        i.[SecnContactNo],
        i.[CustomerAddress],
        i.[GST],
        i.[CustomerLogo],
        i.[CustomerLogoMime],
        i.[CreatedOn],
        i.CreatedBy,
        i.[ModifiedOn],
        i.ModifiedBy,
        'U'
    FROM inserted i
    JOIN deleted d ON i.[CustomerCode] = d.[CustomerCode];
END
Go

CREATE OR ALTER TRIGGER trg_InvProducts_Audit
ON InvProducts
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvProducts_Audit (
        [ProductCode]
        ,[ProductName]
        ,[StockCount]
        ,[UnitName]
        ,[UnitCost]
        ,[ProductLogo]
        ,[isActive]
        ,[CreatedOn]
        ,[CreatedBy]
        ,[ModifiedOn]
        ,[ModifiedBy]
        ,[OperationType]
    )
    SELECT
        i.[ProductCode]
        ,i.[ProductName]
        ,i.[StockCount]
        ,i.[UnitName]
        ,i.[UnitCost]
        ,i.[ProductLogo]
        ,i.[isActive]
        ,i.[CreatedOn]
        ,i.[CreatedBy]
        ,i.[ModifiedOn]
        ,i.[ModifiedBy]
        ,'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[ProductCode] = d.[ProductCode]
    WHERE d.[ProductCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO InvProducts_Audit (
        [ProductCode]
        ,[ProductName]
        ,[StockCount]
        ,[UnitName]
        ,[UnitCost]
        ,[ProductLogo]
        ,[isActive]
        ,[CreatedOn]
        ,[CreatedBy]
        ,[ModifiedOn]
        ,[ModifiedBy]
        ,[OperationType]
    )
    SELECT
        d.[ProductCode]
        ,d.[ProductName]
        ,d.[StockCount]
        ,d.[UnitName]
        ,d.[UnitCost]
        ,d.[ProductLogo]
        ,d.[isActive]
        ,d.[CreatedOn]
        ,d.[CreatedBy]
        ,d.[ModifiedOn]
        ,d.[ModifiedBy]
        ,'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[ProductCode] = d.[ProductCode]
    WHERE i.[ProductCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO InvProducts_Audit (
        [ProductCode]
        ,[ProductName]
        ,[StockCount]
        ,[UnitName]
        ,[UnitCost]
        ,[ProductLogo]
        ,[isActive]
        ,[CreatedOn]
        ,[CreatedBy]
        ,[ModifiedOn]
        ,[ModifiedBy]
        ,[OperationType]
    )
    SELECT
        i.[ProductCode]
        ,i.[ProductName]
        ,i.[StockCount]
        ,i.[UnitName]
        ,i.[UnitCost]
        ,i.[ProductLogo]
        ,i.[isActive]
        ,i.[CreatedOn]
        ,i.[CreatedBy]
        ,i.[ModifiedOn]
        ,i.[ModifiedBy]
        ,'U'
    FROM inserted i
    JOIN deleted d ON i.[ProductCode] = d.[ProductCode];
END

