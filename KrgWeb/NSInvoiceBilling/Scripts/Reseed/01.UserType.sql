/*
    Reference data: [dbo].[UserType].
    Ids are written explicitly because [dbo].[InvUser].[UserType] rows below
    reference them by value. Idempotent.
*/
IF '$(SeedReferenceData)' <> '0'
BEGIN
    PRINT '  -> Seeding [dbo].[UserType]';

    SET IDENTITY_INSERT [dbo].[UserType] ON;

    MERGE INTO [dbo].[UserType] AS T
    USING (VALUES
        (1, N'SuperAdmin', N'Super Admin users for maintenance purpose only'),
        (2, N'Admin',      N'Admin users for making Entry purpose only')
    ) AS S ([UserTypeId], [UserTypeName], [Description])
        ON T.[UserTypeId] = S.[UserTypeId]
    WHEN MATCHED THEN
        UPDATE SET T.[UserTypeName] = S.[UserTypeName],
                   T.[Description]  = S.[Description]
    WHEN NOT MATCHED BY TARGET THEN
        INSERT ([UserTypeId], [UserTypeName], [Description])
        VALUES (S.[UserTypeId], S.[UserTypeName], S.[Description]);

    SET IDENTITY_INSERT [dbo].[UserType] OFF;
END
