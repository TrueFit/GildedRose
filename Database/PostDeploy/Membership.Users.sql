DECLARE @Users TABLE (
	[Id] INT NOT NULL, 
    [UserName] NVARCHAR(100) NOT NULL,
	[Email] [Email] NOT NULL,
	[PasswordHash] [PWD] NOT NULL,
    [CreatedBy] [AuditUser]
);

 
INSERT INTO 
	@Users ([Id], [UserName], [Email], [PasswordHash], [CreatedBy]) 
VALUES
(1, 'talkersoft', 'talkersoft@gmail.com', 'abc', 1),
(2, 'jgretz', 'jgretz@truefit.io', 'efg', 1)

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE membership.Users AS t
USING @Users as s
	on 	(t.[Id] = s.[Id])
WHEN NOT MATCHED BY TARGET
    THEN INSERT (UserName, Email, PasswordHash, CreatedBy)
        VALUES (s.UserName, s.Email, s.PasswordHash, s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET 
				t.UserName = s.UserName, 
				t.Email = s.Email, 
				t.PasswordHash = s.PasswordHash
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;