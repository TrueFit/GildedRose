DECLARE @Organization TABLE (
	[Identifier] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[AdminEmail] [dbo].[Email] NOT NULL,
    [CreatedBy] [AuditUser]
);

 
INSERT INTO 
	@Organization ([Identifier], [Name], [AdminEmail], [CreatedBy]) 
VALUES
('9C1ACC19-30B6-4C4F-ABB7-161482542709', 'Truefit', 'admin@truefit.com', 1);

 
-- Merge Statement Used to ensure list of items maintained in the table variable are persisted into the database
MERGE membership.Organization AS t
USING @Organization as s
	on 	(t.[Identifier] = s.[Identifier])
WHEN NOT MATCHED BY TARGET
    THEN INSERT (Identifier, [Name], AdminEmail, CreatedBy)
        VALUES (s.Identifier, s.[Name], s.AdminEmail, s.CreatedBy)
WHEN MATCHED
    THEN UPDATE SET 
				t.[Name] = s.Name, 
				t.AdminEmail = s.AdminEmail
WHEN NOT MATCHED BY SOURCE 
    THEN DELETE;