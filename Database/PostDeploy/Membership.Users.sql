DECLARE @Users TABLE (
	[Id] INT NOT NULL, 
    [UserName] NVARCHAR(100) NOT NULL,
	[Email] [Email] NOT NULL,
	[PasswordHash] [PWD] NOT NULL,
	[OrganizationIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy] [AuditUser]
);

 
INSERT INTO 
	@Users ([Id], [UserName], [Email], [PasswordHash], [OrganizationIdentifier], [CreatedBy]) 
VALUES
(1, 'talkersoft', 'talkersoft@gmail.com', 'abc', '9C1ACC19-30B6-4C4F-ABB7-161482542709', 1),
(2, 'jgretz', 'jgretz@truefit.io', 'efg', '9C1ACC19-30B6-4C4F-ABB7-161482542709', 1)

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE membership.Users AS t
USING @Users as s
	on 	(t.[Id] = s.[Id])
WHEN NOT MATCHED BY TARGET
    THEN INSERT (UserName, Email, PasswordHash, OrganizationIdentifier, CreatedBy)
        VALUES (s.UserName, s.Email, s.PasswordHash, s.OrganizationIdentifier, s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET 
				t.UserName = s.UserName, 
				t.Email = s.Email, 
				t.OrganizationIdentifier = s.OrganizationIdentifier,
				t.PasswordHash = s.PasswordHash
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;