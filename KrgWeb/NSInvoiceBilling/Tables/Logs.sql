/*
    Serilog MSSqlServer sink target table. The sink can create this itself on
    first run, but declaring it here keeps the deployed database self-contained.
*/
CREATE TABLE [dbo].[Logs] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL PRIMARY KEY,
    [Message]         NVARCHAR (MAX)  NULL,
    [MessageTemplate] NVARCHAR (MAX)  NULL,
    [Level]           NVARCHAR (128)  NULL,
    [TimeStamp]       DATETIME        NULL,
    [Exception]       NVARCHAR (MAX)  NULL,
    [Properties]      NVARCHAR (MAX)  NULL
);
