CREATE VIEW [dbo].[VActivityLog]
AS
SELECT
      [ActivityId],
      [EntityType],
      [EntityId],
      [ActionType],
      [Description],
      [CreatedBy],
      [CreatedDate],
      [RedirectUrl]
FROM [dbo].[ActivityLog];
