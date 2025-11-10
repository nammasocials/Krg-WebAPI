use NSinvoiceBilling
go
CREATE TRIGGER trg_InvCustomers_Audit
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
