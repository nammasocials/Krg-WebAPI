/*
    Fixed database role memberships for [KrgApiUser].
    Idempotent; skipped entirely when the user was not created.
*/
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE [name] = N'KrgApiUser')
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM sys.database_role_members rm
        INNER JOIN sys.database_principals r ON r.[principal_id] = rm.[role_principal_id]
        INNER JOIN sys.database_principals m ON m.[principal_id] = rm.[member_principal_id]
        WHERE r.[name] = N'db_datareader' AND m.[name] = N'KrgApiUser')
    BEGIN
        PRINT '  -> Adding [KrgApiUser] to [db_datareader]';
        ALTER ROLE [db_datareader] ADD MEMBER [KrgApiUser];
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.database_role_members rm
        INNER JOIN sys.database_principals r ON r.[principal_id] = rm.[role_principal_id]
        INNER JOIN sys.database_principals m ON m.[principal_id] = rm.[member_principal_id]
        WHERE r.[name] = N'db_datawriter' AND m.[name] = N'KrgApiUser')
    BEGIN
        PRINT '  -> Adding [KrgApiUser] to [db_datawriter]';
        ALTER ROLE [db_datawriter] ADD MEMBER [KrgApiUser];
    END
END
