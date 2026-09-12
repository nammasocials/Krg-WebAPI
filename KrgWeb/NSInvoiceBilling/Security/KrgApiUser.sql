/*
    Application database user for KrgWebAPI.

    This runs from the post-deployment script rather than as part of the schema
    model: [KrgApiUser] maps to a SERVER-level login, which lives outside the
    database and therefore cannot be resolved when the DACPAC model is built.
    Keeping it here also means a deployment to an environment where the login
    has not been provisioned yet degrades to a warning instead of failing.

    Provision the login once per server, before the first deployment:

        CREATE LOGIN [KrgApiUser] WITH PASSWORD = '<set-per-environment>';

    Idempotent.
*/
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE [name] = N'KrgApiUser')
BEGIN
    PRINT '  -> WARNING: server login [KrgApiUser] does not exist. Skipping user, role and permission setup.';
    PRINT '     Create the login, then redeploy to finish wiring up API access.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE [name] = N'KrgApiUser')
    BEGIN
        PRINT '  -> Creating database user [KrgApiUser]';
        CREATE USER [KrgApiUser] FOR LOGIN [KrgApiUser];
    END
    ELSE
    BEGIN
        PRINT '  -> Database user [KrgApiUser] already exists';
    END
END
