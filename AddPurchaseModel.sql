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
CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

CREATE TABLE [Location] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY ([Id])
);

CREATE TABLE [Owner] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NULL,
    CONSTRAINT [PK_Owner] PRIMARY KEY ([Id])
);

CREATE TABLE [Show] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [CategoryId] int NOT NULL,
    [LocationId] int NOT NULL,
    [OwnerId] int NOT NULL,
    [Date] datetime2 NOT NULL,
    [Time] time NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [CreateDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Show] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Show_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Show_Location_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Location] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Show_Owner_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Owner] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Show_CategoryId] ON [Show] ([CategoryId]);

CREATE INDEX [IX_Show_LocationId] ON [Show] ([LocationId]);

CREATE INDEX [IX_Show_OwnerId] ON [Show] ([OwnerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251003233054_InitialCreate', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251004011742_UpdateShowCreateandEdit', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251004013414_UpdatedAllViews', N'9.0.0');

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Show]') AND [c].[name] = N'ImageUrl');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Show] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Show] DROP COLUMN [ImageUrl];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251007142505_RemoveImageUrl', N'9.0.0');

ALTER TABLE [Show] ADD [ImageFilename] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251008221519_AddImageFilenameToShow', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251008224558_RemoveMigration', N'9.0.0');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251015224912_AddedImageFile', N'9.0.0');

COMMIT;
GO

