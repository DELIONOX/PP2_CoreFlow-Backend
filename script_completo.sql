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
CREATE TABLE [Categorias] (
    [IdCategoria] int NOT NULL IDENTITY,
    [NombreCategoria] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NOT NULL,
    [Activa] bit NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([IdCategoria])
);

CREATE TABLE [Clientes] (
    [IdCliente] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Apellido] nvarchar(max) NOT NULL,
    [Correo] nvarchar(max) NOT NULL,
    [Telefono] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([IdCliente])
);

CREATE TABLE [Proveedores] (
    [IdProveedor] int NOT NULL IDENTITY,
    [NombreEmpresa] nvarchar(max) NOT NULL,
    [Contacto] nvarchar(max) NOT NULL,
    [Correo] nvarchar(max) NOT NULL,
    [Telefono] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY ([IdProveedor])
);

CREATE TABLE [Productos] (
    [IdProducto] int NOT NULL IDENTITY,
    [NombreProducto] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [Stock] int NOT NULL,
    [IdProveedor] int NOT NULL,
    [IdCategoria] int NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([IdProducto]),
    CONSTRAINT [FK_Productos_Categorias_IdCategoria] FOREIGN KEY ([IdCategoria]) REFERENCES [Categorias] ([IdCategoria]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Productos_Proveedores_IdProveedor] FOREIGN KEY ([IdProveedor]) REFERENCES [Proveedores] ([IdProveedor]) ON DELETE NO ACTION
);

CREATE TABLE [Pedidos] (
    [IdPedido] int NOT NULL IDENTITY,
    [FechaPedido] datetime2 NOT NULL,
    [Cantidad] int NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [IdCliente] int NOT NULL,
    [IdProducto] int NOT NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([IdPedido]),
    CONSTRAINT [FK_Pedidos_Clientes_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [Clientes] ([IdCliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Pedidos_Productos_IdProducto] FOREIGN KEY ([IdProducto]) REFERENCES [Productos] ([IdProducto]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Pedidos_IdCliente] ON [Pedidos] ([IdCliente]);

CREATE INDEX [IX_Pedidos_IdProducto] ON [Pedidos] ([IdProducto]);

CREATE INDEX [IX_Productos_IdCategoria] ON [Productos] ([IdCategoria]);

CREATE INDEX [IX_Productos_IdProveedor] ON [Productos] ([IdProveedor]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260706212700_Inicial', N'10.0.9');

COMMIT;
GO

