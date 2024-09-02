CREATE TABLE Categories
(
    Id   INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL
);

ALTER TABLE Transactions
    ADD CategoryId INT;

ALTER TABLE Transactions
DROP
COLUMN Category;

ALTER TABLE Transactions
    ADD CONSTRAINT FK_Transactions_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories (Id);