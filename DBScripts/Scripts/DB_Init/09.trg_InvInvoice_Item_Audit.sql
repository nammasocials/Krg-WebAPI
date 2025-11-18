CREATE OR ALTER TRIGGER trg_InvInvoice_Item_Audit
ON InvInvoice_Items
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO InvInvoice_Items_Audit (
        [ItemCode],
		[InvoiceCode],
		[InvoiceNo],
		[ProductCode],
		[UnitCost],
		[Cost],
		[HSNCode],
		[Quantity],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		OperationType
    )
    SELECT
        i.[ItemCode],
		i.[InvoiceCode],
		i.[InvoiceNo],
		i.[ProductCode],
		i.[UnitCost],
		i.[Cost],
		i.[HSNCode],
		i.[Quantity],
		i.[CreatedOn],
		i.[CreatedBy],
		i.[ModifiedOn],
		i.[ModifiedBy],
        'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[ItemCode] = d.[ItemCode]
    WHERE d.[ItemCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO InvInvoice_Items_Audit (
         [ItemCode],
		[InvoiceCode],
		[InvoiceNo],
		[ProductCode],
		[UnitCost],
		[Cost],
		[HSNCode],
		[Quantity],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		OperationType
    )
    SELECT
        d.[ItemCode],
		d.[InvoiceCode],
		d.[InvoiceNo],
		d.[ProductCode],
		d.[UnitCost],
		d.[Cost],
		d.[HSNCode],
		d.[Quantity],
		d.[CreatedOn],
		d.[CreatedBy],
		d.[ModifiedOn],
		d.[ModifiedBy],
        'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[ItemCode] = d.[ItemCode]
    WHERE i.[ItemCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO InvInvoice_Items_Audit (
         [ItemCode],
		[InvoiceCode],
		[InvoiceNo],
		[ProductCode],
		[UnitCost],
		[Cost],
		[HSNCode],
		[Quantity],
		[CreatedOn],
		[CreatedBy],
		[ModifiedOn],
		[ModifiedBy],
		OperationType
    )
    SELECT
        i.[ItemCode],
		i.[InvoiceCode],
		i.[InvoiceNo],
		i.[ProductCode],
		i.[UnitCost],
		i.[Cost],
		i.[HSNCode],
		i.[Quantity],
		i.[CreatedOn],
		i.[CreatedBy],
		i.[ModifiedOn],
		i.[ModifiedBy],
        'U'
    FROM inserted i
    JOIN deleted d ON i.[ItemCode] = d.[ItemCode];
END
Go