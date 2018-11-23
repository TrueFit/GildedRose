CREATE TABLE [membership].[Organization]
(
    [Identifier] UNIQUEIDENTIFIER NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL,
	[Address] Address NULL,
	[Address2] Address NULL,
	[City] City NULL,
	[State] State NULL,
	[PostalCode] PostalCode NULL,
	[AdminEmail] [Email] NOT NULL,
	[Created] [AuditDate] DEFAULT getutcdate(),
    [CreatedBy] [AuditUser],
	[Modified] [AuditDate] NULL,
    [ModifiedBy] [AuditUser] NULL, 
    CONSTRAINT [PK_OrganizationIdentifier] PRIMARY KEY CLUSTERED ([Identifier] ASC) on [membership]
);