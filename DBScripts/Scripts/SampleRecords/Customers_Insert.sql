INSERT INTO [dbo].[InvCustomers]
           ([CustomerName]
           ,[CustomerEmail]
           ,[ContactNo]
           --,[SecnContactNo]
           ,[CustomerAddress]
           ,[GST]
		   ,[createdBy]
		   ,[createdOn])
     VALUES
           ('Swaminathan-Tandon', 'uvarkey@handa.net', '6035380793', '87/809, Krishna Street, Chapra-731943', '24QQVUW1360V3ZV',
		   '9673061F-E544-4BDC-A4A2-2641E3B226FA',GETDATE());

Select * from InvCustomers;