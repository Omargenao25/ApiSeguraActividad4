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
GO

CREATE TABLE [Autores] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(100) NOT NULL,
    [Nacionalidad] nvarchar(50) NOT NULL,
    [AnioNacimiento] int NOT NULL,
    CONSTRAINT [PK_Autores] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Libros] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(200) NOT NULL,
    [AnioPublicacion] int NOT NULL,
    [Genero] nvarchar(50) NOT NULL,
    [NumeroPaginas] int NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Disponible] bit NOT NULL,
    [AutorId] int NOT NULL,
    CONSTRAINT [PK_Libros] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Libros_Autores_AutorId] FOREIGN KEY ([AutorId]) REFERENCES [Autores] ([Id]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_Libros_AutorId] ON [Libros] ([AutorId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260706235035_BibliotecaActualizada', N'8.0.28');
GO

COMMIT;
GO

