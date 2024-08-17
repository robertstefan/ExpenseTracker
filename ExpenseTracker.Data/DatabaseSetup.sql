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

CREATE TABLE [dbo].[Subcategories] (
    [Id]              UNIQUEIDENTIFIER   NOT NULL,
    [Name]            NVARCHAR (255)     NOT NULL,
    [CreatedDateTime] DATETIMEOFFSET (7) NOT NULL,
    [UpdatedDateTime] DATETIMEOFFSET (7) NULL,
    [IsDeleted]       BIT                CONSTRAINT [DEFAULT_Subcategories_IsDeleted] DEFAULT ((0)) NOT NULL,
    [CategoryId]      UNIQUEIDENTIFIER   NOT NULL,
    CONSTRAINT [PK_Subcategories] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Subcategories_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
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
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]),
    CONSTRAINT [FK_Transactions_Subcategories] FOREIGN KEY ([SubcategoryId]) REFERENCES [dbo].[Subcategories] ([Id])
);

-- =============================================
-- Author:      Andrei Popescu
-- Create date: 15.08.2024, 02:26AM
-- Description: Creates a transaction and verifies that the specified subcategory belongs to the given category, ensuring data consistency.
-- Parameters:
--   @Id               - The unique identifier for the transaction. This is a primary key and should be a GUID.
--                       Type: UNIQUEIDENTIFIER
--
--   @Description      - A brief description of the transaction. This typically explains the nature or purpose of the transaction.
--                       Type: NVARCHAR(255)
--
--   @Amount           - The monetary amount of the transaction. The value is stored with up to two decimal places.
--                       Type: DECIMAL(18,2)
--
--   @Date             - The date and time when the transaction occurred.
--                       Type: DATETIME
--
--   @CategoryId       - The unique identifier for the category associated with the transaction. This is a foreign key reference.
--                       Type: UNIQUEIDENTIFIER
--
--   @SubcategoryId    - The unique identifier for the subcategory associated with the transaction. This is a foreign key reference.
--                       Type: UNIQUEIDENTIFIER
--
--   @IsRecurrent      - Indicates whether the transaction is recurrent. 
--                       1 (TRUE) if recurrent, 0 (FALSE) if not.
--                       Type: BIT
--
--   @TransactionType  - An integer representing the type of transaction (e.g., income, expense).
--                       Type: INT
-- Returns:    Number of affected rows or an error message if the subcategory does not belong to the specified category.
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateTransaction]
    @Id UNIQUEIDENTIFIER, 
    @Description NVARCHAR(255), 
    @Amount DECIMAL(18,2), 
    @Date DATETIME, 
    @CategoryId UNIQUEIDENTIFIER, 
    @SubcategoryId UNIQUEIDENTIFIER, 
    @IsRecurrent BIT, 
    @TransactionType INT
AS
BEGIN
    IF EXISTS (SELECT 1 
               FROM Subcategories 
               WHERE Id = @SubcategoryId 
               AND CategoryId = @CategoryId)
    --Should be a single row containing a categoryId and Id(SubcategoryId).
    --Duplicates are prevented by primary key constraints
    BEGIN
        INSERT INTO Transactions (Id, Description, Amount, Date, CategoryId, SubcategoryId, IsRecurrent, TransactionType, CreatedDateTime)
        VALUES (@Id, @Description, @Amount, @Date, @CategoryId, @SubcategoryId, @IsRecurrent, @TransactionType, GETDATE());
    END
    ELSE
    BEGIN
        PRINT 'Subcategory does not exist in the specified category. Transaction not inserted.';
    END
END
GO

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

-- =============================================
-- Author:        Andrei Popescu
-- Create date:   15.08.2024, 02:45AM
-- Description:   Updates the details of a subcategory, including its name and associated category.
--                If the category association is changed, it also updates all related transactions
--                to ensure they reflect the new category.
-- Parameters:
--   @Id               - The unique identifier for the subcategory to be updated. This is a primary key and should be a GUID.
--                       Type: UNIQUEIDENTIFIER
--
--   @Name             - The new name for the subcategory.
--                       Type: NVARCHAR(255)
--
--   @CategoryId       - The unique identifier for the new category associated with the subcategory. 
--                       This is a foreign key reference to the Categories table.
--                       Type: UNIQUEIDENTIFIER
-- Returns:    Number of affected rows
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateSubcategory]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(255),
    @CategoryId UNIQUEIDENTIFIER
AS
BEGIN
    -- Declare variable to hold the current CategoryId associated with the Subcategory
    DECLARE @OldCategoryId UNIQUEIDENTIFIER

    -- Retrieve the current CategoryId associated with the Subcategory
    SELECT @OldCategoryId = CategoryId
    FROM [Subcategories]
    WHERE Id = @Id

    -- Update the Subcategory details
    UPDATE [Subcategories] SET
        Name = @Name,
        CategoryId = @CategoryId,
        UpdatedDateTime = GETDATE()
    WHERE Id = @Id

    -- If the CategoryId has changed, update all related Transactions
    IF @OldCategoryId <> @CategoryId
    BEGIN
        UPDATE [Transactions] SET
            CategoryId = @CategoryId,
            UpdatedDateTime = GETDATE()
        WHERE SubcategoryId = @Id
    END
END
GO

-- =============================================
-- Author:        Popescu Andrei
-- Create date:   15.08.2024, 02:55AM
-- Description:   Updates the details of an existing transaction in the database. 
--                Before updating, the procedure verifies that the specified subcategory 
--                belongs to the specified category to ensure data consistency.
-- Parameters:
--   @Id               - The unique identifier for the transaction. This is a primary key and should be a GUID.
--                       Type: UNIQUEIDENTIFIER
--
--   @Description      - A brief description of the transaction. This typically explains the nature or purpose of the transaction.
--                       Type: NVARCHAR(255)
--
--   @Amount           - The monetary amount of the transaction. The value is stored with up to two decimal places.
--                       Type: DECIMAL(18,2)
--
--   @Date             - The date and time when the transaction occurred.
--                       Type: DATETIME
--
--   @CategoryId       - The unique identifier for the category associated with the transaction. This is a foreign key reference.
--                       Type: UNIQUEIDENTIFIER
--
--   @SubcategoryId    - The unique identifier for the subcategory associated with the transaction. This is a foreign key reference.
--                       Type: UNIQUEIDENTIFIER
--
--   @IsRecurrent      - Indicates whether the transaction is recurrent. 
--                       1 (TRUE) if recurrent, 0 (FALSE) if not.
--                       Type: BIT
--
--   @TransactionType  - An integer representing the type of transaction (e.g., income, expense).
--                       Type: INT
-- Returns:    Number of affected rows or an error message if the subcategory does not belong to the specified category.
-- =============================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateTransaction]
    @Id UNIQUEIDENTIFIER, 
    @Description NVARCHAR(255), 
    @Amount DECIMAL(18,2), 
    @Date DATETIME, 
    @CategoryId UNIQUEIDENTIFIER, 
    @SubcategoryId UNIQUEIDENTIFIER, 
    @IsRecurrent BIT, 
    @TransactionType INT
AS
BEGIN
    -- Check if the specified subcategory belongs to the specified category
    IF EXISTS (SELECT 1 
               FROM Subcategories 
               WHERE Id = @SubcategoryId 
               AND CategoryId = @CategoryId)
    BEGIN
        -- Update the transaction with the provided details
        UPDATE Transactions SET 
            Description = @Description,
            Amount = @Amount,
            Date = @Date,
            CategoryId = @CategoryId,
            SubcategoryId = @SubcategoryId,
            IsRecurrent = @IsRecurrent,
            TransactionType = @TransactionType,
            UpdatedDateTime = GETDATE()
        WHERE ID = @Id
    END
    ELSE
    BEGIN
        -- Output a message if the subcategory does not match the category
        PRINT 'Subcategory does not exist in the specified category. Transaction not updated.';
    END
END
GO

-- =============================================
-- Author:        Popescu Andrei
-- Create date:   15.08.2024, 02:55AM
-- Description: This trigger is intended to handle transaction updates by first updating all relevant fields. 
-- It then checks if the subcategory has been modified and verifies its association with the category. If the subcategory is not a 
-- valid child of the category, the trigger should roll back all changes to ensure data integrity. However, the trigger is not 
-- functioning as expected, which is why an alternative stored procedure is being used. (Update Transaction SP)
-- Expected Behaviour: On updating a transaction, all fields should be updated initially. If the subcategory is modified, 
-- its relationship with the category should be validated. If the subcategory does not belong to the category, the transaction should 
-- be a atomic one and rollback if a single error occured.
-- Current Behaviour: I don't even know honestly but I think I'm onto something.
-- =============================================


-- CREATE TRIGGER TRG_TRANSACTIONS_UPDATE_InsteadOf
-- ON [Transactions]
-- INSTEAD OF UPDATE
-- AS
-- BEGIN
--     SET NOCOUNT ON

--     IF EXISTS (
--         SELECT 1
--         FROM INSERTED i
--         JOIN DELETED d ON i.Id = d.Id
--         WHERE i.SubcategoryId <> d.SubcategoryId
--     )
--     BEGIN
--         UPDATE t
--         SET
--             t.Description = i.Description,
--             t.Amount = i.Amount,
--             t.Date = i.Date,
--             t.CategoryId = i.CategoryId,
--             t.SubcategoryId = i.SubcategoryId,
--             t.IsRecurrent = i.IsRecurrent,
--             t.TransactionType = i.TransactionType
--         FROM [Transactions] t
--         JOIN INSERTED i on t.Id = i.Id;
--     END
--     ELSE
--     BEGIN
--         RAISERROR('Subcategory does not match the specified Category. Updated operation halted',16,1);
--         ROLLBACK TRANSACTION
--     END;
-- END;

