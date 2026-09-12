/*
    Append-only stock ledger. [dbo].[InvProducts].[CurrentStock] is kept in sync
    by [dbo].[trg_InvProducts_Stock_Quantity]; the API is DENY'd UPDATE on this
    table from the post-deployment security script.
*/
CREATE TABLE [dbo].[InvProducts_Stock] (
    [StockTnxId]  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
    [ProductCode] UNIQUEIDENTIFIER NOT NULL,
    [Quantity]    INT              NOT NULL,
    [TxnType]     INT              NOT NULL,
    [CreatedOn]   DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]   UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_InvProducts_Stock_InvProducts] FOREIGN KEY ([ProductCode]) REFERENCES [dbo].[InvProducts] ([ProductCode]),
    CONSTRAINT [Chk_InvProducts_Stock_Quantity_not_zero] CHECK ([Quantity] <> 0)
);
