CREATE VIEW [dbo].[VInvoiceDetail]
AS
SELECT
      Inv.[InvoiceCode],
      InvItems.[ItemCode],
      Inv.[InvoiceNo],
      C.[CustomerName],
      C.[CustomerCode],
      C.[GST] AS GSTNumber,
      C.[CustomerEmail],
      InvItems.[ProductCode],
      P.[ProductName],
      P.[ProductLogo],
      P.[ProductLogoMime],
      InvItems.[UnitCost],
      InvItems.[Quantity],
      InvItems.[Cost],
      InvItems.[CentralGst],
      InvItems.[CentralGstAmount],
      InvItems.[StateGst],
      InvItems.[StateGstAmount],
      InvItems.[HSNCode],
      Inv.[TotalCost],
      Inv.[GST],
      InvItems.[CreatedOn],
      InvItems.[CreatedBy],
      InvItems.[ModifiedOn],
      InvItems.[ModifiedBy]
FROM [dbo].[InvInvoice_Items] InvItems
INNER JOIN [dbo].[InvProducts] P
       ON InvItems.[ProductCode] = P.[ProductCode]
INNER JOIN [dbo].[InvInvoice] Inv
       ON Inv.[InvoiceCode] = InvItems.[InvoiceCode]
INNER JOIN [dbo].[InvCustomers] C
       ON Inv.[CustomerCode] = C.[CustomerCode];
