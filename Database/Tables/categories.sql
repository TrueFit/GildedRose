CREATE TABLE [inventory].[Categories]
(
    [Id] INT NOT NULL IDENTITY(1,1), 
    [Name] NVARCHAR(100) NOT NULL,
	[Created] [AuditDate] NOT NULL DEFAULT getutcdate(),
    [CreatedBy] [AuditUser] NOT NULL, 
    CONSTRAINT [PK_CategoryIdentifier] PRIMARY KEY CLUSTERED ([Id] ASC) on [inventory]
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [UIX_inventory_category_name]
    ON 
		[inventory].[Categories]([Name] ASC)
    ON
		[inventory];