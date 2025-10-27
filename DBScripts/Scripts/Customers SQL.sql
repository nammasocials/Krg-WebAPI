Use NSinvoiceBilling;
GO

CREATE TABLE [dbo].[InvCustomers] (
    [CustomerCode] Integer IDENTITY(1,1) PRIMARY KEY,
    [CustomerName] NVARCHAR(100) NOT NULL,
    [CustomerEmail] NVARCHAR(100) NOT NULL,
    [ContactNo] NVARCHAR(15) NOT NULL,
    [SecnContactNo] NVARCHAR(15) NULL,
    [CustomerAddress] NVARCHAR(500) NOT NULL,
    [GST] NVARCHAR(30) NOT NULL,
    [CustomerLogo] VARBINARY(MAX),
    [CreatedOn] DATETIME NOT NULL DEFAULT(GETDATE()),
    [CreatedBy] UNIQUEIDENTIFIER
);
Go
INSERT INTO [dbo].[InvCustomers]
       ([CustomerName], [CustomerEmail], [ContactNo], [SecnContactNo], [CustomerAddress], [GST], [CustomerLogo], [CreatedOn], [CreatedBy])
VALUES
('Alice Johnson', 'alice.johnson@email.com', '1234567890', '0987654321', '123 Elm Street, Springfield', 'GST123456', NULL, '2025-10-27T10:00:00', '00000000-0000-0000-0000-000000000001'),
('Bob Smith', 'bob.smith@email.com', '2345678901', '1098765432', '456 Oak Avenue, Shelbyville', 'GST234567', NULL, '2025-10-27T11:00:00', '00000000-0000-0000-0000-000000000002'),
('Carol White', 'carol.white@email.com', '3456789012', '2109876543', '789 Pine Road, Capital City', 'GST345678', NULL, '2025-10-27T12:00:00', '00000000-0000-0000-0000-000000000003'),
('David Brown', 'david.brown@email.com', '4567890123', '3210987654', '101 Maple Lane, Springfield', 'GST456789', NULL, '2025-10-27T13:00:00', '00000000-0000-0000-0000-000000000004'),
('Eve Davis', 'eve.davis@email.com', '5678901234', '4321098765', '202 Birch Blvd, Shelbyville', 'GST567890', NULL, '2025-10-27T14:00:00', '00000000-0000-0000-0000-000000000005'),
('Frank Miller', 'frank.miller@email.com', '6789012345', '5432109876', '303 Cedar Court, Capital City', 'GST678901', NULL, '2025-10-27T15:00:00', '00000000-0000-0000-0000-000000000006'),
('Grace Wilson', 'grace.wilson@email.com', '7890123456', '6543210987', '404 Spruce Street, Springfield', 'GST789012', NULL, '2025-10-27T16:00:00', '00000000-0000-0000-0000-000000000007'),
('Henry Moore', 'henry.moore@email.com', '8901234567', '7654321098', '505 Fir Lane, Shelbyville', 'GST890123', NULL, '2025-10-27T17:00:00', '00000000-0000-0000-0000-000000000008'),
('Isabel Taylor', 'isabel.taylor@email.com', '9012345678', '8765432109', '606 Elm Street, Capital City', 'GST901234', NULL, '2025-10-27T18:00:00', '00000000-0000-0000-0000-000000000009'),
('Jack Anderson', 'jack.anderson@email.com', '0123456789', '9876543210', '707 Oak Avenue, Springfield', 'GST012345', NULL, '2025-10-27T19:00:00', '00000000-0000-0000-0000-000000000010'),
('Kathy Thomas', 'kathy.thomas@email.com', '1234509876', '0987612345', '808 Pine Road, Shelbyville', 'GST123450', NULL, '2025-10-27T20:00:00', '00000000-0000-0000-0000-000000000011'),
('Larry Jackson', 'larry.jackson@email.com', '2345610987', '1098723456', '909 Maple Lane, Capital City', 'GST234561', NULL, '2025-10-27T21:00:00', '00000000-0000-0000-0000-000000000012'),
('Mona Harris', 'mona.harris@email.com', '3456721098', '2109834567', '1010 Birch Blvd, Springfield', 'GST345672', NULL, '2025-10-27T22:00:00', '00000000-0000-0000-0000-000000000013'),
('Nick Martin', 'nick.martin@email.com', '4567832109', '3210945678', '1111 Cedar Court, Shelbyville', 'GST456783', NULL, '2025-10-27T23:00:00', '00000000-0000-0000-0000-000000000014'),
('Olivia Lee', 'olivia.lee@email.com', '5678943210', '4321056789', '1212 Spruce Street, Capital City', 'GST567894', NULL, '2025-10-28T00:00:00', '00000000-0000-0000-0000-000000000015'),
('Paul Walker', 'paul.walker@email.com', '6789054321', '5432167890', '1313 Fir Lane, Springfield', 'GST678905', NULL, '2025-10-28T01:00:00', '00000000-0000-0000-0000-000000000016'),
('Quincy Young', 'quincy.young@email.com', '7890165432', '6543278901', '1414 Elm Street, Shelbyville', 'GST789016', NULL, '2025-10-28T02:00:00', '00000000-0000-0000-0000-000000000017'),
('Rachel King', 'rachel.king@email.com', '8901276543', '7654389012', '1515 Oak Avenue, Capital City', 'GST890127', NULL, '2025-10-28T03:00:00', '00000000-0000-0000-0000-000000000018'),
('Steve Wright', 'steve.wright@email.com', '9012387654', '8765490123', '1616 Pine Road, Springfield', 'GST901238', NULL, '2025-10-28T04:00:00', '00000000-0000-0000-0000-000000000019'),
('Tracy Scott', 'tracy.scott@email.com', '0123498765', '9876501234', '1717 Maple Lane, Shelbyville', 'GST012349', NULL, '2025-10-28T05:00:00', '00000000-0000-0000-0000-000000000020'),
('Uma Patel', 'uma.patel@email.com', '1234509876', '0987612345', '1818 Birch Blvd, Capital City', 'GST123450', NULL, '2025-10-28T06:00:00', '00000000-0000-0000-0000-000000000021'),
('Victor Brooks', 'victor.brooks@email.com', '2345610987', '1098723456', '1919 Cedar Court, Springfield', 'GST234561', NULL, '2025-10-28T07:00:00', '00000000-0000-0000-0000-000000000022'),
('Wendy Green', 'wendy.green@email.com', '3456721098', '2109834567', '2020 Spruce Street, Shelbyville', 'GST345672', NULL, '2025-10-28T08:00:00', '00000000-0000-0000-0000-000000000023'),
('Xander Hughes', 'xander.hughes@email.com', '4567832109', '3210945678', '2121 Fir Lane, Capital City', 'GST456783', NULL, '2025-10-28T09:00:00', '00000000-0000-0000-0000-000000000024'),
('Yara Collins', 'yara.collins@email.com', '5678943210', '4321056789', '2222 Elm Street, Springfield', 'GST567894', NULL, '2025-10-28T10:00:00', '00000000-0000-0000-0000-000000000025');

go

/****** Object:  View [dbo].[VCustomers]    Script Date: 24/10/2025 14:00:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[VCustomers] AS
    Select 
        [CustomerCode],
        [CustomerName],
        [CustomerAddress],
        [CustomerEmail],
        [ContactNo],
        [SecnContactNo],
        [GST],
        [CreatedOn],
        [CreatedBy] 
    from InvCustomers;
GO
