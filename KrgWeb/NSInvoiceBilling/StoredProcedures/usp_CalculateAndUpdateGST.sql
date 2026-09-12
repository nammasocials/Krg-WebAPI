-- =============================================
-- Author:      Anand
-- Create date: 25-02-2026
-- Description: For Calculating and updating GST value for each invoice
-- =============================================
CREATE PROCEDURE [dbo].[usp_CalculateAndUpdateGST]
    @InvoiceCode UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Update GST for ALL items of this invoice (set-based)
    UPDATE t
    SET
        [CentralGstAmount] = (t.[CentralGst] * t.[UnitCost] * t.[Quantity]) / 100,
        [StateGstAmount]   = (t.[StateGst]   * t.[UnitCost] * t.[Quantity]) / 100
    FROM [dbo].[InvInvoice_Items] t
    WHERE t.[InvoiceCode] = @InvoiceCode;

    -- 2. Update Net Amount for ALL items of this invoice (set-based)
    UPDATE t
    SET
        [NetProductAmount] = [Cost] + [CentralGstAmount] + [StateGstAmount]
    FROM [dbo].[InvInvoice_Items] t
    WHERE t.[InvoiceCode] = @InvoiceCode;

    -- 3. Update invoice-level GST total
    UPDATE [dbo].[InvInvoice]
    SET [GST] =
    (
        SELECT SUM([CentralGstAmount] + [StateGstAmount])
        FROM [dbo].[InvInvoice_Items]
        WHERE [InvoiceCode] = [dbo].[InvInvoice].[InvoiceCode]
    ),
    [TotalCost] =
    (
        SELECT SUM([NetProductAmount])
        FROM [dbo].[InvInvoice_Items]
        WHERE [InvoiceCode] = @InvoiceCode
    )
    WHERE [dbo].[InvInvoice].[InvoiceCode] = @InvoiceCode;
END
