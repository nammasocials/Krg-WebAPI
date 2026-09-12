CREATE VIEW [dbo].[VInvoices]
AS
SELECT
      Inv.[InvoiceCode],
      Inv.[InvoiceNo],
      C.[CustomerName],
      C.[CustomerCode],
      C.[GST] AS GSTNumber,
      C.[CustomerEmail],
      Inv.[isEwayBillAvailable],
      Inv.[TotalCost],
      Inv.[GST],
      Inv.[CreatedOn],
      Inv.[CreatedBy],
      Inv.[ModifiedOn],
      Inv.[ModifiedBy]
FROM [dbo].[InvInvoice] Inv
INNER JOIN [dbo].[InvCustomers] C
       ON Inv.[CustomerCode] = C.[CustomerCode];
