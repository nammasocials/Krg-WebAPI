/*
    Inter-state sale: a single IGST at the combined CGST + SGST rate.
    Returns the unit cost inclusive of it.
*/
CREATE FUNCTION [dbo].[fn_CalculateInterStateGST]
(
    @UnitCost      DECIMAL (18, 2),
    @CentralGstPer DECIMAL (5, 2),
    @StateGstPer   DECIMAL (5, 2)
)
RETURNS DECIMAL (18, 2)
AS
BEGIN
    DECLARE @IGSTPer     DECIMAL (5, 2);
    DECLARE @IGSTAmount  DECIMAL (18, 2);
    DECLARE @TotalAmount DECIMAL (18, 2);

    SET @IGSTPer = @CentralGstPer + @StateGstPer;

    SET @IGSTAmount = (@UnitCost * @IGSTPer) / 100;

    SET @TotalAmount = @UnitCost + @IGSTAmount;

    RETURN @TotalAmount;
END
