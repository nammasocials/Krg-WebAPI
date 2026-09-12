CREATE TRIGGER [dbo].[trg_InvExpenses_Audit]
ON [dbo].[InvExpenses]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO [dbo].[InvExpenses_Audit] (
        [ExpenseCode], [ExpenseType], [ExpenseDate], [Description], [Amount], [InvoiceCode],
        [VendorName], [VendorInvoiceNo], [VendorInvoiceDate], [isActive],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        i.[ExpenseCode], i.[ExpenseType], i.[ExpenseDate], i.[Description], i.[Amount], i.[InvoiceCode],
        i.[VendorName], i.[VendorInvoiceNo], i.[VendorInvoiceDate], i.[isActive],
        i.[CreatedOn], i.[CreatedBy], i.[ModifiedOn], i.[ModifiedBy], 'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[ExpenseCode] = d.[ExpenseCode]
    WHERE d.[ExpenseCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO [dbo].[InvExpenses_Audit] (
        [ExpenseCode], [ExpenseType], [ExpenseDate], [Description], [Amount], [InvoiceCode],
        [VendorName], [VendorInvoiceNo], [VendorInvoiceDate], [isActive],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        d.[ExpenseCode], d.[ExpenseType], d.[ExpenseDate], d.[Description], d.[Amount], d.[InvoiceCode],
        d.[VendorName], d.[VendorInvoiceNo], d.[VendorInvoiceDate], d.[isActive],
        d.[CreatedOn], d.[CreatedBy], d.[ModifiedOn], d.[ModifiedBy], 'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[ExpenseCode] = d.[ExpenseCode]
    WHERE i.[ExpenseCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO [dbo].[InvExpenses_Audit] (
        [ExpenseCode], [ExpenseType], [ExpenseDate], [Description], [Amount], [InvoiceCode],
        [VendorName], [VendorInvoiceNo], [VendorInvoiceDate], [isActive],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        i.[ExpenseCode], i.[ExpenseType], i.[ExpenseDate], i.[Description], i.[Amount], i.[InvoiceCode],
        i.[VendorName], i.[VendorInvoiceNo], i.[VendorInvoiceDate], i.[isActive],
        i.[CreatedOn], i.[CreatedBy], i.[ModifiedOn], i.[ModifiedBy], 'U'
    FROM inserted i
    JOIN deleted d ON i.[ExpenseCode] = d.[ExpenseCode];
END
