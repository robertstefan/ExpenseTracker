CREATE TABLE [Transactions] (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    Description NVARCHAR(255) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Date DATE NOT NULL,
    IsRecurrent BIT NOT NULL DEFAULT 0,
    TransactionType INT DEFAULT 0,
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE CASCADE
);


CREATE TABLE Categories (
    Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID() PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL
)

