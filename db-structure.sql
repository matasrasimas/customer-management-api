IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308120641_InitialMigration'
)
BEGIN
    CREATE TABLE [Customers] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Address] nvarchar(450) NOT NULL,
        [PostCode] nvarchar(max) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308120641_InitialMigration'
)
BEGIN
    CREATE TABLE [CustomerLogs] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [Action] nvarchar(max) NOT NULL,
        [PerformedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_CustomerLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CustomerLogs_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308120641_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_CustomerLogs_CustomerId] ON [CustomerLogs] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308120641_InitialMigration'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Customers_Name_Address] ON [Customers] ([Name], [Address]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260308120641_InitialMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260308120641_InitialMigration', N'10.0.3');
END;

COMMIT;
GO

