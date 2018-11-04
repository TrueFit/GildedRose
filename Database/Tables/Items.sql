CREATE TABLE [inventory].[Items]
(
    [Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Category] INT NOT NULL, 
	[ShelfLife] INT NOT NULL,
	[MaxQuality] INT NOT NULL,
	[IsLegendary] BIT NOT NULL,
	[IsDeleted] BIT NOT NULL,
	[Created] [AuditDate] NOT NULL DEFAULT getutcdate(),
    [CreatedBy] [AuditUser] NOT NULL, 
    CONSTRAINT [PK_ItemIdentifier] PRIMARY KEY CLUSTERED ([Identifier] ASC) on [inventory],
	CONSTRAINT [FK_Items_Category] FOREIGN KEY ([Category]) REFERENCES [inventory].[Categories]([Id]), 
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [UIX_inventory_item_name]
    ON [inventory].[Items]([Name] ASC)
    ON [inventory];

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Items_lookup] ON [inventory].[Items]([Identifier]) 
INCLUDE ([Name], [Category])
WHERE IsDeleted = 0;