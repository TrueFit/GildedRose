DECLARE @Users TABLE (
	[Id] INT NOT NULL, 
    [UserName] NVARCHAR(100) NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
	[Email] [Email] NOT NULL,
	[PasswordHash] [PWD] NOT NULL,
	[OrganizationIdentifier] UNIQUEIDENTIFIER NULL,
    [CreatedBy] [AuditUser]
);

 
INSERT INTO 
	@Users ([Id], [UserName], [FirstName], [LastName], [Email], [PasswordHash], [OrganizationIdentifier], [CreatedBy]) 
VALUES
(1, 'talkersoft', 'Todd', 'Alker', 'talkersoft@gmail.com', 'EHP4Q+HQ4O3ZLLQObMij4+nVHcZd4B8HDz0w8CTmkB/aWFwq84EJ27NoDu7yCaAVe9ohH8MppfnvsXfuz5nQLVwUTcFC/X8LF2rjlCzW+OY=', '9C1ACC19-30B6-4C4F-ABB7-161482542709', 1),
(2, 'jgretz', 'Josh', 'Gretz', 'jgretz@truefit.io', 'EHP4Q+HQ4O3ZLLQObMij4+nVHcZd4B8HDz0w8CTmkB/aWFwq84EJ27NoDu7yCaAVe9ohH8MppfnvsXfuz5nQLVwUTcFC/X8LF2rjlCzW+OY=', '9C1ACC19-30B6-4C4F-ABB7-161482542709', 1)

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE membership.Users AS t
USING @Users as s
	on 	(t.[Id] = s.[Id])
WHEN NOT MATCHED BY TARGET
    THEN INSERT (UserName, FirstName, LastName, Email, PasswordHash, OrganizationIdentifier, CreatedBy)
        VALUES (s.UserName, s.FirstName, s.LastName, s.Email, s.PasswordHash, s.OrganizationIdentifier, s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET 
				t.UserName = s.UserName, 
				t.FirstName = s.FirstName,
				t.LastName = s.LastName,
				t.Email = s.Email, 
				t.OrganizationIdentifier = s.OrganizationIdentifier,
				t.PasswordHash = s.PasswordHash
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;