use NSinvoiceBilling
Go

CREATE OR ALTER FUNCTION dbo.fn_CalculateIntraStateGST
(
    @UnitCost DECIMAL(18,2),
    @CentralGstPer DECIMAL(5,2),
    @StateGstPer DECIMAL(5,2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @CGSTAmount DECIMAL(18,2)
    DECLARE @SGSTAmount DECIMAL(18,2)
    DECLARE @TotalAmount DECIMAL(18,2)
    
    -- Calculate CGST
    SET @CGSTAmount = (@UnitCost * @CentralGstPer) / 100
    
    -- Calculate SGST
    SET @SGSTAmount = (@UnitCost * @StateGstPer) / 100
    
    -- Total = UnitCost + CGST + SGST
    SET @TotalAmount = @UnitCost + @CGSTAmount + @SGSTAmount
    
    RETURN @TotalAmount
END

GO

CREATE OR ALTER FUNCTION dbo.fn_CalculateInterStateGST
(
    @UnitCost DECIMAL(18,2),
    @CentralGstPer DECIMAL(5,2),
    @StateGstPer DECIMAL(5,2)
)
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE @IGSTAmount DECIMAL(18,2)
    DECLARE @TotalAmount DECIMAL(18,2)
    
    -- IGST = CGST% + SGST%
    DECLARE @IGSTPer DECIMAL(5,2)
    SET @IGSTPer = @CentralGstPer + @StateGstPer
    
    -- Calculate IGST Amount
    SET @IGSTAmount = (@UnitCost * @IGSTPer) / 100
    
    -- Total = UnitCost + IGST
    SET @TotalAmount = @UnitCost + @IGSTAmount
    
    RETURN @TotalAmount
END