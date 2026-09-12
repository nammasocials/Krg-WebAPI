/*
    Expenses with their type name resolved and the linked invoice number pulled
    through. [ExpenseNature] is what the UI groups on: an expense is invoice
    based when it carries either of the two links.
*/
CREATE VIEW [dbo].[VExpenses]
AS
SELECT
      E.[ExpenseCode],
      E.[ExpenseType],
      T.[Name]        AS ExpenseTypeName,
      T.[ShName]      AS ExpenseTypeShortName,
      E.[ExpenseDate],
      E.[Description],
      E.[Amount],
      E.[InvoiceCode],
      Inv.[InvoiceNo],
      C.[CustomerName],
      E.[VendorName],
      E.[VendorInvoiceNo],
      E.[VendorInvoiceDate],
      CASE
          WHEN E.[InvoiceCode] IS NOT NULL OR E.[VendorInvoiceNo] IS NOT NULL
              THEN 'Invoice Based'
          ELSE 'Normal'
      END AS ExpenseNature,
      E.[isActive],
      E.[CreatedOn],
      E.[CreatedBy],
      E.[ModifiedOn],
      E.[ModifiedBy]
FROM [dbo].[InvExpenses] E
INNER JOIN [dbo].[InvConstant] T
       ON T.[Key] = E.[ExpenseType]
      AND T.[EntityId] = 'InvExpenses'
      AND T.[Category] = 'ExpenseType'
LEFT JOIN [dbo].[InvInvoice] Inv
       ON Inv.[InvoiceCode] = E.[InvoiceCode]
LEFT JOIN [dbo].[InvCustomers] C
       ON C.[CustomerCode] = Inv.[CustomerCode];
