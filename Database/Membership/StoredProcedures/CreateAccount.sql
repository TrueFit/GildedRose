CREATE PROCEDURE [membership].[CreateAccount]
	@userName nvarchar(100),
	@firstName nvarchar(100),
	@lastName nvarchar(100),
	@email nvarchar(255),
	@passwordHash nvarchar(255),
	@organization uniqueidentifier = null
AS 
SET NOCOUNT ON
BEGIN TRY
	DECLARE @Transaction VARCHAR(20);  
	SELECT @Transaction = 'CreateAccountTransaction'; 
	DECLARE @Id AS INT = 0;

	BEGIN TRANSACTION @Transaction
		INSERT INTO 
			[membership].[Users] 
				(
					UserName,
					FirstName,
					LastName,
					Email,
					PasswordHash,
					OrganizationIdentifier,
					Created,
					CreatedBy
				) VALUES 
				(
					@userName,
					@firstName,
					@lastName,
					@email,
					@passwordHash,
					@organization,
					getdate(),
					@Id
				);

	COMMIT TRANSACTION @Transaction
	SET @Transaction = NULL;

	SET @Id = SCOPE_IDENTITY();

		UPDATE 
			[membership].[Users]
		SET
			CreatedBy = Id
		WHERE
			Id = @Id;

		SELECT @Id as Id;
END TRY  
BEGIN CATCH   

IF @Transaction IS NOT NULL
BEGIN
	ROLLBACK;
END
	PRINT 'error';
		INSERT INTO [logs].[SQLException] (ProcedureName, ErrorMessage, ErrorNumber, ErrorLine, ErrorSeverity, ErrorState)
		VALUES (ERROR_PROCEDURE(), ERROR_MESSAGE(), ERROR_NUMBER(), ERROR_LINE(), ERROR_SEVERITY(), ERROR_STATE());
	THROW;
END CATCH

