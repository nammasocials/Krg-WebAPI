CREATE TRIGGER [dbo].[trg_InvInvoice_Audit]
ON [dbo].[InvInvoice]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO [dbo].[InvInvoice_Audit] (
        [InvoiceCode], [InvoiceNo], [CustomerCode], [TotalCost], [isEwayBillAvailable],
        [EWayBillLogo], [EWayBillLogoMime], [GST],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        i.[InvoiceCode], i.[InvoiceNo], i.[CustomerCode], i.[TotalCost], i.[isEwayBillAvailable],
        i.[EWayBillLogo], i.[EWayBillLogoMime], i.[GST],
        i.[CreatedOn], i.[CreatedBy], i.[ModifiedOn], i.[ModifiedBy], 'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[InvoiceCode] = d.[InvoiceCode]
    WHERE d.[InvoiceCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO [dbo].[InvInvoice_Audit] (
        [InvoiceCode], [InvoiceNo], [CustomerCode], [TotalCost], [isEwayBillAvailable],
        [EWayBillLogo], [EWayBillLogoMime], [GST],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        d.[InvoiceCode], d.[InvoiceNo], d.[CustomerCode], d.[TotalCost], d.[isEwayBillAvailable],
        d.[EWayBillLogo], d.[EWayBillLogoMime], d.[GST],
        d.[CreatedOn], d.[CreatedBy], d.[ModifiedOn], d.[ModifiedBy], 'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[InvoiceCode] = d.[InvoiceCode]
    WHERE i.[InvoiceCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO [dbo].[InvInvoice_Audit] (
        [InvoiceCode], [InvoiceNo], [CustomerCode], [TotalCost], [isEwayBillAvailable],
        [EWayBillLogo], [EWayBillLogoMime], [GST],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        i.[InvoiceCode], i.[InvoiceNo], i.[CustomerCode], i.[TotalCost], i.[isEwayBillAvailable],
        i.[EWayBillLogo], i.[EWayBillLogoMime], i.[GST],
        i.[CreatedOn], i.[CreatedBy], i.[ModifiedOn], i.[ModifiedBy], 'U'
    FROM inserted i
    JOIN deleted d ON i.[InvoiceCode] = d.[InvoiceCode];
END
