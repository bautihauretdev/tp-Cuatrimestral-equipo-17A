USE master
GO

CREATE DATABASE EQUIPO17A_GYM_DB
GO

USE EQUIPO17A_GYM_DB
GO

CREATE TABLE SOCIOS (
    IdSocio INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50),
    Apellido NVARCHAR(50),
    Dni NVARCHAR(20),
    FechaNacimiento DATE,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    Activo BIT DEFAULT 1
);
GO

CREATE TABLE USUARIOS (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(20) NOT NULL CHECK (Rol IN ('Administrador', 'Socio')),
    Activo BIT DEFAULT 1,
    IdSocio INT NULL FOREIGN KEY REFERENCES SOCIOS(IdSocio)
);
GO


-- ESTO ES SOLO PARA HACER UN INSERT RAPIDO Y TESTEAR LA BASE DE DATOS Y SU CONEXION CON EL PROYECTO (BORRAR + ADELANTE)
INSERT INTO SOCIOS (Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, Activo)
VALUES ('Juan', 'Pérez', '12345678', '1990-05-10', '1122334455', 'juanperez@mail.com', 1);

-- PasswordHash temporal sin encriptar (ejemplo: admin123)
INSERT INTO USUARIOS (Email, PasswordHash, Rol, Activo, IdSocio)
VALUES ('admin@gym.com', 'admin123', 'Administrador', 1, NULL),
       ('juanperez@mail.com', 'socio123', 'Socio', 1, 1);
GO