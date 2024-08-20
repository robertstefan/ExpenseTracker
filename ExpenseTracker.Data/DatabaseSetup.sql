CREATE TABLE [Transactions] (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    Description NVARCHAR(255) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Date DATE NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    IsRecurrent BIT NULL DEFAULT 0,
    TransactionType INT DEFAULT 0,
)

CREATE TABLE [dbo].[Users] (
    [Id]           INT                IDENTITY (1, 1) NOT NULL,
    [Username]     NVARCHAR (100)     NOT NULL,
    [Email]        NVARCHAR (100)     NOT NULL,
    [PasswordHash] NVARCHAR (100)     NOT NULL,
    [CreateDate]   DATETIMEOFFSET (7) NOT NULL,
    [UpdateDate]   DATETIMEOFFSET (7) NULL,
    [LastName]     NVARCHAR (20)      NOT NULL,
    [FirstName]    NVARCHAR (20)      NOT NULL,
    [LockedOut]    BIT                CONSTRAINT [DEFAULT_Users_LockedOut] DEFAULT ((0)) NOT NULL,
    [LoginTries]   SMALLINT           CONSTRAINT [DEFAULT_Users_LoginTies] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

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