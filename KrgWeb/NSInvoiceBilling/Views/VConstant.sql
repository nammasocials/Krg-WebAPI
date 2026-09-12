CREATE VIEW [dbo].[VConstant]
AS
SELECT
      [ConstantId],
      [EntityId],
      [Category],
      [Key],
      [ConstantValue],
      [Name],
      [PluralName],
      [ShName],
      [ShPluralName],
      [Description],
      [isActive],
      [CreatedOn],
      [CreatedBy]
FROM [dbo].[InvConstant];
