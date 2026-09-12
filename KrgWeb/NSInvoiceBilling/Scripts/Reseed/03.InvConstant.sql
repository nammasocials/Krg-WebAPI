/*
    Reference data: [dbo].[InvConstant].

    The StockTxnType rows are looked up by [Name] ('Stock-In' / 'Stock-Out') in
    [dbo].[InvProducts_StockUpdate], so those names must not drift. Matched on
    the natural key (EntityId, Category, Key). Idempotent.
*/
IF '$(SeedReferenceData)' <> '0'
BEGIN
    PRINT '  -> Seeding [dbo].[InvConstant]';

    DECLARE @CreatedBy UNIQUEIDENTIFIER = (SELECT TOP 1 [UserCode] FROM [dbo].[InvUser] ORDER BY [CreatedOn]);

    MERGE INTO [dbo].[InvConstant] AS T
    USING (VALUES
        (N'InvProducts', N'UnitType',     1, N'1',  N'Piece',     N'Pieces', N'pc',  N'pcs', N'a single unit'),
        (N'InvProducts', N'UnitType',     2, N'10', N'Pack',      N'Packs',  N'pk',  N'pks', N'10 units of Bags'),
        (N'InvProducts', N'StockTxnType', 1, N'1',  N'Stock-In',  NULL,      N'In',  NULL,   N'Production of Stock Inventory'),
        (N'InvProducts', N'StockTxnType', 2, N'-1', N'Stock-Out', NULL,      N'Out', NULL,   N'Sales of Stock Inventory')
    ) AS S ([EntityId], [Category], [Key], [ConstantValue], [Name], [PluralName], [ShName], [ShPluralName], [Description])
        ON  T.[EntityId] = S.[EntityId]
        AND T.[Category] = S.[Category]
        AND T.[Key]      = S.[Key]
    WHEN MATCHED THEN
        UPDATE SET T.[ConstantValue] = S.[ConstantValue],
                   T.[Name]          = S.[Name],
                   T.[PluralName]    = S.[PluralName],
                   T.[ShName]        = S.[ShName],
                   T.[ShPluralName]  = S.[ShPluralName],
                   T.[Description]   = S.[Description]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([EntityId], [Category], [Key], [ConstantValue], [Name], [PluralName], [ShName], [ShPluralName], [Description], [CreatedBy])
        VALUES (S.[EntityId], S.[Category], S.[Key], S.[ConstantValue], S.[Name], S.[PluralName], S.[ShName], S.[ShPluralName], S.[Description], @CreatedBy);
END
