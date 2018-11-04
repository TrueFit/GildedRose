CREATE TABLE [inventory].[ItemsOnHand]
(
    [Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [ItemIdentifier] UNIQUEIDENTIFIER NOT NULL, 
	[StockCount] INT NOT NULL,
	[StockDate] DATETIME2 NOT NULL,
	[Created] [AuditDate] NOT NULL DEFAULT getutcdate(),
    [CreatedBy] [AuditUser] NOT NULL, 
    CONSTRAINT [PK_ItemOnHandIdentifier] PRIMARY KEY CLUSTERED ([Identifier] ASC) on [inventory],
	CONSTRAINT [FK_ItemsOnHand_Items] FOREIGN KEY ([ItemIdentifier]) REFERENCES [inventory].[Items]([Identifier]), 
);
 