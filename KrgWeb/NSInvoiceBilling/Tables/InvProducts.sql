/*
    Per-product GST percentages live on the product row (CentralGstPer /
    StateGstPer) and are projected through [dbo].[VProducts] by the
    fn_Calculate*StateGST functions.

    NOTE: "06.InvProducts.sql" in the DB_Init folder models this table with a
    [UnitType] column instead. That variant is NOT what is deployed: the newer
    "05.InvProducts.sql", the "07.InvProducts_Stock.sql" that drops UnitType
    from the stock table, and the scaffolded EF model in
    DBLayer\Models\InvProduct.cs all use the CentralGstPer / StateGstPer shape
    below, so that is what this project reproduces.
*/
CREATE TABLE [dbo].[InvProducts] (
    [ProductCode]     UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [ProductName]     NVARCHAR (100)   NOT NULL,
    [UnitCost]        DECIMAL (10, 2)  NOT NULL,
    [HSNCode]         NVARCHAR (10)    NOT NULL,
    [CentralGstPer]   DECIMAL (10, 2)  NOT NULL,
    [StateGstPer]     DECIMAL (10, 2)  NOT NULL,
    [CurrentStock]    INT              NOT NULL DEFAULT 1,
    [ProductLogo]     VARBINARY (MAX)  NULL,
    [ProductLogoMime] NVARCHAR (30)    NULL,
    [isActive]        BIT              NOT NULL DEFAULT 1,
    [CreatedOn]       DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]       UNIQUEIDENTIFIER NULL,
    [ModifiedOn]      DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]      UNIQUEIDENTIFIER NULL
);
