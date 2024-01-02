CREATE TABLE Users (
    UserID SERIAL PRIMARY KEY,
    Name VARCHAR(255),
    Address VARCHAR(255)
);

CREATE TABLE Accounts (
    AccountID SERIAL PRIMARY KEY,
    UserID INT REFERENCES Users(UserID),
    Type VARCHAR(50),
    Balance DECIMAL(10, 2)
);

CREATE TABLE Transactions (
    TransactionID SERIAL PRIMARY KEY,
    FromAccountID INT REFERENCES Accounts(AccountID),
    ToAccountID INT REFERENCES Accounts(AccountID),
    Amount DECIMAL(10, 2),
    TransactionFee DECIMAL(10, 2) DEFAULT 1.00,
    Timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE TopUps (
    TopUpID SERIAL PRIMARY KEY,
    AccountID INT REFERENCES Accounts(AccountID),
    Amount DECIMAL(10, 2),
    Timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
