/*
    Reference data: expense types for [dbo].[InvExpenses].

    Kept separate from 03.InvConstant.sql so the invoice/product constants and
    the expense constants can move independently. Matched on the natural key
    (EntityId, Category, Key), so renaming a type here updates it in place and
    existing expense rows follow. Idempotent.
*/
IF '$(SeedReferenceData)' <> '0'
BEGIN
    PRINT '  -> Seeding [dbo].[InvConstant] expense types';

    DECLARE @CreatedBy UNIQUEIDENTIFIER = (SELECT TOP 1 [UserCode] FROM [dbo].[InvUser] ORDER BY [CreatedOn]);

    MERGE INTO [dbo].[InvConstant] AS T
    USING (VALUES
        (N'InvExpenses', N'ExpenseType', 1, N'1', N'Electricity',      N'Power',    N'Electricity and power bills'),
        (N'InvExpenses', N'ExpenseType', 2, N'2', N'Cleaning',         N'Clean',    N'Cleaning and housekeeping, including maid wages'),
        (N'InvExpenses', N'ExpenseType', 3, N'3', N'Rent',             N'Rent',     N'Premises rent and lease charges'),
        (N'InvExpenses', N'ExpenseType', 4, N'4', N'Salary',           N'Salary',   N'Staff salaries and wages'),
        (N'InvExpenses', N'ExpenseType', 5, N'5', N'Freight',          N'Freight',  N'Transport and delivery charges, often against a specific invoice'),
        (N'InvExpenses', N'ExpenseType', 6, N'6', N'Raw Material',     N'Material', N'Material purchased against a vendor bill'),
        (N'InvExpenses', N'ExpenseType', 7, N'7', N'Office Supplies',  N'Office',   N'Stationery, consumables and office running costs'),
        (N'InvExpenses', N'ExpenseType', 8, N'8', N'Repairs',          N'Repairs',  N'Repairs and maintenance'),
        (N'InvExpenses', N'ExpenseType', 9, N'9', N'Miscellaneous',    N'Misc',     N'Anything that does not fit the categories above')
    ) AS S ([EntityId], [Category], [Key], [ConstantValue], [Name], [ShName], [Description])
        ON  T.[EntityId] = S.[EntityId]
        AND T.[Category] = S.[Category]
        AND T.[Key]      = S.[Key]
    WHEN MATCHED THEN
        UPDATE SET T.[ConstantValue] = S.[ConstantValue],
                   T.[Name]          = S.[Name],
                   T.[ShName]        = S.[ShName],
                   T.[Description]   = S.[Description]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([EntityId], [Category], [Key], [ConstantValue], [Name], [ShName], [Description], [CreatedBy])
        VALUES (S.[EntityId], S.[Category], S.[Key], S.[ConstantValue], S.[Name], S.[ShName], S.[Description], @CreatedBy);
END
