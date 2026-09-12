/*
    Intra-state sale: CGST + SGST are charged separately.
    Returns the unit cost inclusive of both.
*/
CREATE FUNCTION [dbo].[fn_CalculateIntraStateGST]
(
    @UnitCost      DECIMAL (18, 2),
    @CentralGstPer DECIMAL (5, 2),
    @StateGstPer   DECIMAL (5, 2)
)
RETURNS DECIMAL (18, 2)
AS
BEGIN
    DECLARE @CGSTAmount  DECIMAL (18, 2);
    DECLARE @SGSTAmount  DECIMAL (18, 2);
    DECLARE @TotalAmount DECIMAL (18, 2);

    SET @CGSTAmount = (@UnitCost * @CentralGstPer) / 100;
    SET @SGSTAmount = (@UnitCost * @StateGstPer) / 100;

    SET @TotalAmount = @UnitCost + @CGSTAmount + @SGSTAmount;

    RETURN @TotalAmount;
END
