CREATE VIEW [dbo].[VStock]
AS
SELECT
      Stock.[ProductCode],
      P.[ProductName],
      TxnType.[Name] AS TransactionType,
      Stock.[Quantity],
      Stock.[CreatedOn],
      Stock.[CreatedBy]
FROM [dbo].[InvProducts_Stock] Stock
INNER JOIN [dbo].[InvProducts] P
       ON P.[ProductCode] = Stock.[ProductCode]
INNER JOIN [dbo].[InvConstant] TxnType
       ON TxnType.[Key] = Stock.[TxnType]
      AND TxnType.[EntityId] = 'InvProducts'
      AND TxnType.[Category] = 'StockTxnType';
