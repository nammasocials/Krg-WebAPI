/*
    Application navigation for [dbo].[InvMenu].

    Deliberately NOT guarded by $(SeedReferenceData): menus are application
    structure, not sample data. With no rows here the UI has no top menu and no
    sidebars at all, so a deployment that skipped them would come up unusable.

    Modules are inserted before their children so the self-referencing
    [FK_InvMenu_Parent] always has its parent in place.

    Matched on [MenuId], so renaming a caption or repointing a route updates the
    existing row and any customisation of [isActive] survives. Idempotent.
*/
PRINT '  -> Seeding [dbo].[InvMenu] modules';

DECLARE @CreatedBy UNIQUEIDENTIFIER = (SELECT TOP 1 [UserCode] FROM [dbo].[InvUser] ORDER BY [CreatedOn]);

-- Modules: the top menu. ParentCode NULL, IsParent 1.
MERGE INTO [dbo].[InvMenu] AS T
USING (VALUES
    (N'DASHBOARD',    N'Dashboard',    N'/dashboard',    N'dashboard',    1),
    (N'MASTERS',      N'Masters',      N'/masters',      N'masters',      2),
    (N'TRANSACTIONS', N'Transactions', N'/transactions', N'transactions', 3),
    (N'EXPENSES',     N'Expenses',     N'/expenses',     N'expenses',     4),
    (N'REPORTS',      N'Reports',      N'/reports',      N'reports',      5)
) AS S ([MenuId], [MenuName], [MenuUrl], [MenuIcon], [DisplayOrder])
    ON T.[MenuId] = S.[MenuId]
WHEN MATCHED THEN
    UPDATE SET T.[MenuName]     = S.[MenuName],
               T.[MenuUrl]      = S.[MenuUrl],
               T.[MenuIcon]     = S.[MenuIcon],
               T.[DisplayOrder] = S.[DisplayOrder],
               T.[ParentCode]   = NULL,
               T.[IsParent]     = 1,
               T.[ModifiedOn]   = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([MenuId], [MenuName], [MenuUrl], [MenuIcon], [ParentCode], [IsParent], [DisplayOrder], [CreatedBy])
    VALUES (S.[MenuId], S.[MenuName], S.[MenuUrl], S.[MenuIcon], NULL, 1, S.[DisplayOrder], @CreatedBy);
GO

PRINT '  -> Seeding [dbo].[InvMenu] sidebar entries';

DECLARE @CreatedBy UNIQUEIDENTIFIER = (SELECT TOP 1 [UserCode] FROM [dbo].[InvUser] ORDER BY [CreatedOn]);

-- Sidebar entries: ParentCode points at the module, IsParent stays NULL.
-- Dashboard is intentionally childless; it is the common landing page and
-- carries no sidebar of its own.
MERGE INTO [dbo].[InvMenu] AS T
USING (VALUES
    -- Masters
    (N'MASTERS_PRODUCTS',  N'Products',        N'/masters/products',          N'product',        N'MASTERS',      1),
    (N'MASTERS_CUSTOMERS', N'Customers',       N'/masters/customers',         N'customer',       N'MASTERS',      2),

    -- Transactions
    (N'TXN_INVOICES',      N'Invoices',        N'/transactions/invoices',     N'invoice',        N'TRANSACTIONS', 1),
    (N'TXN_NEW_INVOICE',   N'New Invoice',     N'/transactions/new-invoice',  N'invoice-add',    N'TRANSACTIONS', 2),
    (N'TXN_ADD_STOCK',     N'Add Stock',       N'/transactions/add-stock',    N'stock',          N'TRANSACTIONS', 3),

    -- Expenses
    (N'EXP_LIST',          N'All Expenses',    N'/expenses/list',             N'expense',        N'EXPENSES',     1),
    (N'EXP_NEW',           N'File Expense',    N'/expenses/new',              N'expense-add',    N'EXPENSES',     2),

    -- Reports
    (N'RPT_SALES',         N'Sales Register',  N'/reports/sales-register',    N'report-sales',   N'REPORTS',      1),
    (N'RPT_STOCK',         N'Stock Ledger',    N'/reports/stock-ledger',      N'report-stock',   N'REPORTS',      2),
    (N'RPT_EXPENSE',       N'Expense Report',  N'/reports/expense',           N'report-expense', N'REPORTS',      3),
    (N'RPT_PROFIT',        N'Profit Summary',  N'/reports/profit-summary',    N'report-profit',  N'REPORTS',      4)
) AS S ([MenuId], [MenuName], [MenuUrl], [MenuIcon], [ParentCode], [DisplayOrder])
    ON T.[MenuId] = S.[MenuId]
WHEN MATCHED THEN
    UPDATE SET T.[MenuName]     = S.[MenuName],
               T.[MenuUrl]      = S.[MenuUrl],
               T.[MenuIcon]     = S.[MenuIcon],
               T.[ParentCode]   = S.[ParentCode],
               T.[IsParent]     = NULL,
               T.[DisplayOrder] = S.[DisplayOrder],
               T.[ModifiedOn]   = GETDATE()
WHEN NOT MATCHED BY TARGET THEN
    INSERT ([MenuId], [MenuName], [MenuUrl], [MenuIcon], [ParentCode], [IsParent], [DisplayOrder], [CreatedBy])
    VALUES (S.[MenuId], S.[MenuName], S.[MenuUrl], S.[MenuIcon], S.[ParentCode], NULL, S.[DisplayOrder], @CreatedBy);
