/*
--------------------------------------------------------------------------------
Post-Deployment Script
--------------------------------------------------------------------------------
Runs after every deployment of NSInvoiceBilling. Everything included here must
be idempotent -- it will run again on the next deploy.

Reference data is seeded by default. A DACPAC only declares its SQLCMD
variables -- it cannot bake in a value -- so the seed guards are written as
"<> '0'": seeding happens unless it is explicitly turned off.

    sqlpackage /Action:Publish ... /v:SeedReferenceData=0
--------------------------------------------------------------------------------
*/

PRINT 'Post-deployment: SeedReferenceData = [$(SeedReferenceData)] (blank = default, seeding enabled)';
GO

:r .\Reseed\01.UserType.sql
GO

:r .\Reseed\02.InvUser.sql
GO

:r .\Reseed\03.InvConstant.sql
GO

:r .\Reseed\04.InvConstant.Expenses.sql
GO

:r .\Reseed\05.InvMenu.sql
GO

--------------------------------------------------------------------------------
-- Security. Applied after the schema exists so object-level DENY can bind, and
-- guarded so a missing server login warns instead of failing the deployment.
--------------------------------------------------------------------------------
PRINT 'Post-deployment: applying security';
GO

:r ..\Security\KrgApiUser.sql
GO

:r ..\Security\RoleMemberships.sql
GO

:r ..\Security\Permissions.sql
GO

PRINT 'Post-deployment complete.';
GO
