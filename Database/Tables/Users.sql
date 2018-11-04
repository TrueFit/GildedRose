CREATE TABLE [membership].[Users]
(
    [Id] INT NOT NULL IDENTITY(1,1), 
    [UserName] NVARCHAR(100) NOT NULL,
	[Email] [Email] NOT NULL,
	[PasswordHash] [PWD] NOT NULL,
	[Created] [AuditDate] DEFAULT getutcdate(),
    [CreatedBy] [AuditUser], 
    CONSTRAINT [PK_CategoryIdentifier] PRIMARY KEY CLUSTERED ([Id] ASC) on [membership]
);