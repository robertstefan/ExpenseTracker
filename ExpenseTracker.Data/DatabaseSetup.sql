CREATE DATABASE ExpenseManager

USE ExpenseManager

CREATE TABLE [dbo].[Categories] (
    [Id]              UNIQUEIDENTIFIER   NOT NULL,
    [Name]            NVARCHAR (255)     DEFAULT ('') NOT NULL,
    [CreatedDateTime] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDateTIme] DATETIMEOFFSET (7) NULL,
    [IsDeleted]       BIT                CONSTRAINT [DEFAULT_Categories_IsDeleted] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Transactions] (
    [Id]              UNIQUEIDENTIFIER   NOT NULL,
    [Description]     NVARCHAR (255)     DEFAULT ('') NOT NULL,
    [Amount]          DECIMAL (18, 2)    CONSTRAINT [DEFAULT_Transactions_Amount] DEFAULT ((0)) NOT NULL,
    [Date]            DATETIME           NOT NULL,
    [CategoryId]      UNIQUEIDENTIFIER   NOT NULL,
    [IsRecurrent]     BIT                CONSTRAINT [DEFAULT_Transactions_IsRecurrent] DEFAULT ((0)) NOT NULL,
    [TransactionType] INT                NOT NULL,
    [CreatedDateTime] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDateTime] DATETIMEOFFSET (7) NULL,
    [SubcategoryId]   UNIQUEIDENTIFIER   NOT NULL,
    [IsDeleted]       BIT                CONSTRAINT [DEFAULT_Transactions_IsDeleted] DEFAULT ((0)) NOT NULL,
    [UserId]          UNIQUEIDENTIFIER   NOT NULL
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_Transactions_Subcategories] FOREIGN KEY ([SubcategoryId]) REFERENCES [dbo].[Subcategories] ([Id])
);

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
-- ===========================================================================================
-- Author:      Andrei Popescu
-- Create date: 15.08.2024, 02:26AM
-- Description:
--   Toggles the deletion status of a record in a specified table. It can
--   either perform a soft delete (toggle the 'IsDeleted' flag) or a hard delete (permanently 
--   remove the record from the table), based on the value of the @IsSoft flag.
--
-- Parameters:
--   @Id - The unique identifier for the record. This is a primary key and should be a GUID.
--         Type: UNIQUEIDENTIFIER
--
--   @TableName - The name of the table from which the record should be deleted or toggled. The table
--                name is provided as a string.
--                Type: NVARCHAR(255)
--
--   @IsSoft - A flag indicating the type of deletion:
--           - 1: Perform a soft delete by toggling the 'IsDeleted' field in the record.
--           - 0: Perform a hard delete by permanently removing the record from the table.
--           Type: BIT
--
-- Procedure Logic:
--   1. If @IsSoft = 1 (Soft Delete):
--      - The procedure checks the current value of the 'IsDeleted' field for the specified
--        record.
--      - If the record is already soft deleted (IsDeleted = 1), it will be undeleted (IsDeleted = 0).
--      - If the record is not deleted, it will be marked as deleted (IsDeleted = 1).
--      - The 'UpdatedDateTime' field is also updated to the current date and time.
--
--   2. If @IsSoft = 0 (Hard Delete):
--      - The procedure deletes the specified record from the table.
--
-- Execution:
--   The procedure dynamically constructs SQL statements using the provided table name and
--   executes them using the 'sp_executesql' system stored procedure.
--
-- ===========================================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ToggleDelete]
    @Id UNIQUEIDENTIFIER,
    @TableName NVARCHAR(255),
    @IsSoft BIT
AS
BEGIN
    DECLARE @SQL NVARCHAR(MAX);
    DECLARE @AlreadyDeleted BIT;

    IF @IsSoft = 1
    -- Construct SQL to check if the record is already deleted
    BEGIN
        SET @SQL = 'SELECT @AlreadyDeleted = IsDeleted FROM ' + QUOTENAME(@TableName) + ' WHERE Id = @Id';

        EXEC sp_executesql @SQL, N'@Id UNIQUEIDENTIFIER, @AlreadyDeleted BIT OUTPUT', @Id, @AlreadyDeleted OUTPUT;
        
        IF @AlreadyDeleted = 1
        -- If already deleted, undelete the record
        BEGIN
            SET @SQL = 'UPDATE ' + QUOTENAME(@TableName) + 
                       ' SET IsDeleted = 0, UpdatedDateTime = GETDATE()' +
                       ' WHERE Id = @Id';
        END
        ELSE
        -- If not deleted, mark the record as deleted
        BEGIN
            SET @SQL = 'UPDATE ' + QUOTENAME(@TableName) + 
                       ' SET IsDeleted = 1, UpdatedDateTime = GETDATE()' +
                       ' WHERE Id = @Id';
        END;
    END
    ELSE
    -- If a hard delete, construct SQL to permanently delete the record
    BEGIN
        SET @SQL = 'DELETE FROM ' + QUOTENAME(@TableName) +
                   ' WHERE Id = @Id';  
    END
    EXEC sp_executesql @SQL, N'@Id UNIQUEIDENTIFIER', @Id;
END;
GO


CREATE OR ALTER PROCEDURE dbo.RemoveUser
  @userId INT,
  @softDelete BIT
AS
BEGIN
  BEGIN TRANSACTION;

  BEGIN TRY
    IF (@softDelete = 0)
    BEGIN
      DELETE FROM [Transactions] WHERE UserId = @userId;
      DELETE FROM [Users] WHERE Id = @userId;
    END
    ELSE
    BEGIN
      UPDATE [Transactions] SET IsDeleted = 1 WHERE UserId = @userId;
      UPDATE [Users] SET IsDeleted = 1 WHERE Id = @userId;
    END
    COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    ROLLBACK TRANSACTION;
    THROW;
  END CATCH
END