CREATE VIEW [dbo].[VCustomers]
AS
SELECT
      [CustomerCode],
      [CustomerName],
      [CustomerAddress],
      [CustomerEmail],
      [ContactNo],
      [SecnContactNo],
      [GST],
      [CreatedOn],
      [CreatedBy],
      [ModifiedOn],
      [ModifiedBy]
FROM [dbo].[InvCustomers];
