CREATE TABLE [membership].[Users]
(
    [Id] INT NOT NULL IDENTITY(1,1), 
    [UserName] NVARCHAR(100) NOT NULL,
	[FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
	[Email] [Email] NOT NULL,
	[PasswordHash] [PWD] NOT NULL,
	[OrganizationIdentifier] UNIQUEIDENTIFIER,
	[Created] [AuditDate] DEFAULT getutcdate(),
    [CreatedBy] [AuditUser],
	[Modified] [AuditDate] NULL,
    [ModifiedBy] [AuditUser] NULL, 
    CONSTRAINT [PK_UserIdentifier] PRIMARY KEY CLUSTERED ([Id] ASC) on [membership],
	CONSTRAINT [FK_User_Organization] FOREIGN KEY ([OrganizationIdentifier]) REFERENCES [membership].[Organization]([Identifier]), 
);