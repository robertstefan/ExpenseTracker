CREATE TABLE [Transactions] (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    Description NVARCHAR(255) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Date DATE NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    IsRecurrent BIT NULL DEFAULT 0,
    TransactionType INT DEFAULT 0,
)

-- Create the stored procedure in the specified schema
CREATE OR ALTER PROCEDURE dbo.RemoveUser
  @userId int
AS
BEGIN
  -- body of the stored procedure
  BEGIN TRANSACTION

    BEGIN TRY

    DELETE FROM [Transactions] WHERE UserId = @userId
    DELETE FROM [Users] WHERE Id = @userId

    COMMIT TRANSACTION
    END TRY
    BEGIN CATCH

      ROLLBACk;
      THROW;

    END CATCH
END