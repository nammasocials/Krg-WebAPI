CREATE OR ALTER TRIGGER trg_InvInvoice_Audit
ON InvInvoice
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvInvoice_Audit (
        [InvoiceCode],
		[InvoiceNo],
		[CustomerCode],
		[TotalCost],
		[GST],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		OperationType
    )
    SELECT
        i.[InvoiceCode],
        i.[InvoiceNo],
        i.[CustomerCode],
        i.[TotalCost],
        i.[GST],
        i.[CreatedOn],
        i.[CreatedBy],
        i.[ModifiedOn],
        i.[ModifiedBy],
        'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[InvoiceCode] = d.[InvoiceCode]
    WHERE d.[InvoiceCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO InvInvoice_Audit (
        [InvoiceCode],
		[InvoiceNo],
		[CustomerCode],
		[TotalCost],
		[GST],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		OperationType
    )
    SELECT
        d.[InvoiceCode],
        d.[InvoiceNo],
        d.[CustomerCode],
        d.[TotalCost],
        d.[GST],
        d.[CreatedOn],
        d.[CreatedBy],
        d.[ModifiedOn],
        d.[ModifiedBy],
        'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[InvoiceCode] = d.[InvoiceCode]
    WHERE i.[InvoiceCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO InvInvoice_Audit (
        [InvoiceCode],
		[InvoiceNo],
		[CustomerCode],
		[TotalCost],
		[GST],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		OperationType
    )
    SELECT
        i.[InvoiceCode],
        i.[InvoiceNo],
        i.[CustomerCode],
        i.[TotalCost],
        i.[GST],
        i.[CreatedOn],
        i.[CreatedBy],
        i.[ModifiedOn],
        i.[ModifiedBy],
        'U'
    FROM inserted i
    JOIN deleted d ON i.[InvoiceCode] = d.[InvoiceCode];
END
Go