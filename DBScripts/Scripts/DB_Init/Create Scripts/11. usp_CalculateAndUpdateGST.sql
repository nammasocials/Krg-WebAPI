
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anand
-- Create date: 25-02-2026
-- Description:	For Calculating and updating GST value for each invoice
-- =============================================

CREATE OR ALTER PROCEDURE usp_CalculateAndUpdateGST
    @InvoiceCode UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Update GST for ALL items of this invoice (set-based)
    UPDATE t
    SET 
        CentralGstAmount = (t.CentralGst * t.UnitCost * t.Quantity) / 100,
        StateGstAmount   = (t.StateGst   * t.UnitCost * t.Quantity) / 100
    FROM InvInvoice_Items t
    WHERE t.InvoiceCode = @InvoiceCode;

	-- 2. Update Net Amount for ALL items of this invoice (set-based)
	UPDATE t
    SET 
        NetProductAmount = Cost + CentralGstAmount + StateGstAmount
    FROM InvInvoice_Items t
    WHERE t.InvoiceCode = @InvoiceCode;

    -- 3. Update invoice-level GST total
    UPDATE InvInvoice
    SET InvInvoice.GST =
    (
        SELECT SUM(CentralGstAmount + StateGstAmount)
        FROM InvInvoice_Items
        WHERE InvoiceCode = InvInvoice.InvoiceCode
    ), 
	TotalCost = (Select SUM(NetProductAmount) from InvInvoice_Items where InvoiceCode = @InvoiceCode)
    WHERE InvInvoice.InvoiceCode = @InvoiceCode;
END
