CREATE TRIGGER [dbo].[trg_InvProducts_Audit]
ON [dbo].[InvProducts]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert audit for inserted rows (INSERT)
    INSERT INTO [dbo].[InvProducts_Audit] (
        [ProductCode], [ProductName], [UnitCost], [HSNCode], [CentralGstPer], [StateGstPer],
        [CurrentStock], [ProductLogo], [ProductLogoMime], [isActive],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        i.[ProductCode], i.[ProductName], i.[UnitCost], i.[HSNCode], i.[CentralGstPer], i.[StateGstPer],
        i.[CurrentStock], i.[ProductLogo], i.[ProductLogoMime], i.[isActive],
        i.[CreatedOn], i.[CreatedBy], i.[ModifiedOn], i.[ModifiedBy], 'I'
    FROM inserted i
    LEFT JOIN deleted d ON i.[ProductCode] = d.[ProductCode]
    WHERE d.[ProductCode] IS NULL; -- Only rows newly inserted

    -- Insert audit for deleted rows (DELETE)
    INSERT INTO [dbo].[InvProducts_Audit] (
        [ProductCode], [ProductName], [UnitCost], [HSNCode], [CentralGstPer], [StateGstPer],
        [CurrentStock], [ProductLogo], [ProductLogoMime], [isActive],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        d.[ProductCode], d.[ProductName], d.[UnitCost], d.[HSNCode], d.[CentralGstPer], d.[StateGstPer],
        d.[CurrentStock], d.[ProductLogo], d.[ProductLogoMime], d.[isActive],
        d.[CreatedOn], d.[CreatedBy], d.[ModifiedOn], d.[ModifiedBy], 'D'
    FROM deleted d
    LEFT JOIN inserted i ON i.[ProductCode] = d.[ProductCode]
    WHERE i.[ProductCode] IS NULL; -- Only rows deleted

    -- Insert audit for updated rows (UPDATE)
    INSERT INTO [dbo].[InvProducts_Audit] (
        [ProductCode], [ProductName], [UnitCost], [HSNCode], [CentralGstPer], [StateGstPer],
        [CurrentStock], [ProductLogo], [ProductLogoMime], [isActive],
        [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [OperationType]
    )
    SELECT
        i.[ProductCode], i.[ProductName], i.[UnitCost], i.[HSNCode], i.[CentralGstPer], i.[StateGstPer],
        i.[CurrentStock], i.[ProductLogo], i.[ProductLogoMime], i.[isActive],
        i.[CreatedOn], i.[CreatedBy], i.[ModifiedOn], i.[ModifiedBy], 'U'
    FROM inserted i
    JOIN deleted d ON i.[ProductCode] = d.[ProductCode];
END
