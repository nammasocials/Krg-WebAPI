CREATE VIEW [dbo].[VProducts]
AS
SELECT
      P.[ProductCode],
      P.[ProductName],
      P.[CurrentStock],
      P.[HSNCode],
      P.[UnitCost],
      P.[CentralGstPer],
      P.[StateGstPer],
      -- Intra-State (CGST + SGST)
      [dbo].[fn_CalculateIntraStateGST](P.[UnitCost], P.[CentralGstPer], P.[StateGstPer]) AS IntraStateTotal,
      -- Inter-State (IGST)
      [dbo].[fn_CalculateInterStateGST](P.[UnitCost], P.[CentralGstPer], P.[StateGstPer]) AS InterStateTotal,
      P.[isActive],
      P.[CreatedOn],
      P.[CreatedBy],
      P.[ModifiedOn],
      P.[ModifiedBy]
FROM [dbo].[InvProducts] P;
