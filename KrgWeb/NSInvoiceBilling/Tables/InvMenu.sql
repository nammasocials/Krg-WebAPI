/*
    Application navigation, one row per menu entry.

      * A module is a top menu entry: [ParentCode] IS NULL and [IsParent] = 1.
      * A sidebar entry sets [ParentCode] to its module's [MenuId], and leaves
        [IsParent] NULL. That is why [IsParent] is nullable rather than a plain
        0/1 flag: only modules assert it.

    [MenuUrl] is the Angular route path, so the front end can build both the top
    menu and each module's sidebar from this table without hard-coding either.
    [MenuIcon] holds an icon key (for example 'users', 'invoice'), not markup;
    the UI maps the key to an inline SVG.

    [DisplayOrder] is not in the original column list but menus have to render in
    a defined order -- without it the order is whatever the query planner returns.
*/
CREATE TABLE [dbo].[InvMenu] (
    [MenuId]       NVARCHAR (50)    NOT NULL PRIMARY KEY,
    [MenuName]     NVARCHAR (100)   NOT NULL,
    [MenuUrl]      NVARCHAR (200)   NULL,
    [MenuIcon]     NVARCHAR (100)   NULL,
    [ParentCode]   NVARCHAR (50)    NULL,
    [IsParent]     BIT              NULL,
    [DisplayOrder] INT              NOT NULL DEFAULT 0,
    [isActive]     BIT              NOT NULL DEFAULT 1,
    [CreatedOn]    DATETIME         NOT NULL DEFAULT (GETDATE()),
    [CreatedBy]    UNIQUEIDENTIFIER NULL,
    [ModifiedOn]   DATETIME         NOT NULL DEFAULT (GETDATE()),
    [ModifiedBy]   UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_InvMenu_Parent] FOREIGN KEY ([ParentCode]) REFERENCES [dbo].[InvMenu] ([MenuId]),
    -- A row is either a module (no parent, flagged) or a child of one, never both.
    CONSTRAINT [Chk_InvMenu_Parentage] CHECK (
        ([ParentCode] IS NULL AND [IsParent] = 1)
        OR ([ParentCode] IS NOT NULL AND [IsParent] IS NULL)
    )
);
GO

CREATE NONCLUSTERED INDEX [IX_InvMenu_ParentCode]
    ON [dbo].[InvMenu] ([ParentCode], [DisplayOrder]);
