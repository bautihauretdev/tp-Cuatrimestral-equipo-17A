USE master;
GO

CREATE DATABASE EQUIPO17A_GYM_DB;
GO

USE EQUIPO17A_GYM_DB;
GO

CREATE TABLE PLANES (
    IdPlan INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    PrecioMensual DECIMAL(10,2) NOT NULL,
    MaxHorasSemana INT NOT NULL,
    Activo BIT DEFAULT 1 NOT NULL
);
GO

CREATE TABLE SOCIOS (
    IdSocio INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50),
    Apellido NVARCHAR(50),
    Dni NVARCHAR(20),
    FechaNacimiento DATE,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    IdPlan INT NOT NULL FOREIGN KEY REFERENCES PLANES(IdPlan),  -- ahora plan esta aca
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

CREATE TABLE CUOTAS (
    IdCuota INT IDENTITY(1,1) PRIMARY KEY,
    IdSocio INT NOT NULL FOREIGN KEY REFERENCES SOCIOS(IdSocio),
    IdPago INT NULL,
    Anio INT NOT NULL,
    Mes INT NOT NULL CHECK (Mes BETWEEN 1 AND 12),
    Monto DECIMAL(10,2) NOT NULL,
    Recargo DECIMAL(10,2) NOT NULL DEFAULT 0,
    Estado NVARCHAR(20) NOT NULL CHECK (Estado IN ('Pagado', 'ConRecargo', 'Deudor')),
    FechaPago DATETIME NULL,
    FormaPago NVARCHAR(20) NULL
);
GO

CREATE TABLE TURNOS
(
    IdTurno INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME NOT NULL,
    CapacidadMaxima INT NOT NULL,
    Ocupados INT NOT NULL DEFAULT 0
);
GO

-- Indice para que no se repitan
CREATE UNIQUE INDEX UX_Turnos_Fecha
    ON TURNOS (Fecha);
GO

-- Tabla de relación entre Turnos - Socios
CREATE TABLE TURNOS_SOCIOS (
    IdTurno INT NOT NULL,
    IdSocio INT NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY (IdTurno, IdSocio),
    FOREIGN KEY (IdTurno) REFERENCES TURNOS(IdTurno),
    FOREIGN KEY (IdSocio) REFERENCES SOCIOS(IdSocio)
);
GO

CREATE TABLE NOTIFICACIONES (
    IdNotificacion INT IDENTITY(1,1) PRIMARY KEY,
    IdSocio INT NOT NULL,
    Mensaje NVARCHAR(500) NOT NULL,
    FechaEnvio DATETIME NOT NULL DEFAULT GETDATE(),
    Leido BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (IdSocio) REFERENCES SOCIOS(IdSocio)
);
GO



-- VISTAS Y PROCEDIMIENTOS ALMACENADOS PARA REPORTES

-- REPORTE #01 - TOTAL DE SOCIOS ACTIVOS 
CREATE VIEW vw_TotalSociosActivos AS
SELECT COUNT(*) AS TotalSociosActivos
FROM SOCIOS
WHERE Activo = 1;
GO

-- REPORTE #02 - INGRESOS DEL MES
CREATE VIEW vw_IngresosMes AS
SELECT 
    SUM(Monto + Recargo) AS TotalIngresosMes
FROM CUOTAS
WHERE Estado IN ('Pagado', 'ConRecargo')
  AND FechaPago IS NOT NULL
  AND MONTH(FechaPago) = MONTH(GETDATE())
  AND YEAR(FechaPago) = YEAR(GETDATE());
  GO

  -- REPORTE #03 - SOCIOS MOROSOS
  CREATE OR ALTER PROCEDURE sp_Reporte_SociosMorosos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(DISTINCT IdSocio) AS CantidadMorosos
    FROM CUOTAS
    WHERE Estado = 'Deudor';
END;
GO

-- REPORTE #04 - DIA CON MAS CONCURRENCIA
CREATE OR ALTER PROCEDURE sp_Reporte_DiaMasConcurrencia
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        DATENAME(WEEKDAY, T.Fecha) AS DiaSemana,
        COUNT(TS.IdSocio) AS TotalAsistencias
    FROM TURNOS T
    INNER JOIN TURNOS_SOCIOS TS ON TS.IdTurno = T.IdTurno
    GROUP BY DATENAME(WEEKDAY, T.Fecha)
    ORDER BY TotalAsistencias DESC;
END;
GO

-- REPORTE #05 - FRANJA HORARIA CON MAS CONCURRENCIA
CREATE OR ALTER PROCEDURE sp_Reporte_FranjaHorariaMasConcurrida
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        CONCAT(
            RIGHT('0' + CAST(DATEPART(HOUR, T.Fecha) AS VARCHAR(2)), 2), ':00 - ',
            RIGHT('0' + CAST((DATEPART(HOUR, T.Fecha) + 1) AS VARCHAR(2)), 2), ':00'
        ) AS FranjaHoraria,
        COUNT(TS.IdSocio) AS TotalAsistencias
    FROM TURNOS T
    INNER JOIN TURNOS_SOCIOS TS ON TS.IdTurno = T.IdTurno
    GROUP BY DATEPART(HOUR, T.Fecha)
    ORDER BY TotalAsistencias DESC;
END;
GO

-- RPORTE #06 - DIA CON MENOS FRECUENCIA 
CREATE OR ALTER PROCEDURE sp_Reporte_DiaMenosConcurrencia
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        DATENAME(WEEKDAY, T.Fecha) AS DiaSemana,
        COUNT(TS.IdSocio) AS TotalAsistencias
    FROM TURNOS T
    INNER JOIN TURNOS_SOCIOS TS ON TS.IdTurno = T.IdTurno
    GROUP BY DATENAME(WEEKDAY, T.Fecha)
    HAVING COUNT(TS.IdSocio) > 0
    ORDER BY TotalAsistencias ASC;  -- Menor concurrencia
END;
GO

-- REPORTE #07 - FRANJA HORARIA CON MENOS FRECUENCIA 
CREATE OR ALTER PROCEDURE sp_Reporte_FranjaHorariaMenosConcurrida
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        CONCAT(
            RIGHT('0' + CAST(DATEPART(HOUR, T.Fecha) AS VARCHAR(2)), 2), ':00 - ',
            RIGHT('0' + CAST((DATEPART(HOUR, T.Fecha) + 1) AS VARCHAR(2)), 2), ':00'
        ) AS FranjaHoraria,
        COUNT(TS.IdSocio) AS TotalAsistencias
    FROM TURNOS T
    INNER JOIN TURNOS_SOCIOS TS ON TS.IdTurno = T.IdTurno
    GROUP BY DATEPART(HOUR, T.Fecha)
    HAVING COUNT(TS.IdSocio) > 0
    ORDER BY TotalAsistencias ASC;  -- Menor concurrencia
END;
GO

-- REPORTE #08 - PROMEDIO DE OCUPACION DE TURNOS
CREATE OR ALTER PROCEDURE sp_Reporte_OcupacionPromedioTurnos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(AVG(CAST(Ocupados AS DECIMAL(10,4)) / CapacidadMaxima) * 100 AS DECIMAL(10,2))
        AS OcupacionPromedioPorcentaje
    FROM TURNOS
    WHERE Ocupados > 0;   -- Solo turnos utilizados
END;
GO

-- REPORTE #09 - TOP 5 SOCIOS CON MAS RESERVAS (HISTÓRICO)
CREATE OR ALTER PROCEDURE sp_Reporte_TopSociosReservas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5 
        S.IdSocio,
        S.Nombre,
        S.Apellido,
        COUNT(TS.IdSocio) AS TotalReservas
    FROM TURNOS_SOCIOS TS
    INNER JOIN SOCIOS S ON S.IdSocio = TS.IdSocio
    GROUP BY S.IdSocio, S.Nombre, S.Apellido
    ORDER BY TotalReservas DESC;
END;
GO

-- REPORTE #10 - TOP 5 SOCIOS MOROSOS (HISTÓRICO)
CREATE OR ALTER PROCEDURE sp_Reporte_TopMorosos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        S.IdSocio,
        S.Nombre,
        S.Apellido,
        COUNT(C.IdCuota) AS CuotasAdeudadas
    FROM CUOTAS C
    INNER JOIN SOCIOS S ON S.IdSocio = C.IdSocio
    WHERE C.Estado = 'Deudor'
    GROUP BY S.IdSocio, S.Nombre, S.Apellido
    ORDER BY CuotasAdeudadas DESC;
END;
GO

-- REPORTE #11 - PRÓXIMOS PAGOS A VENCER (TOP 5)
CREATE OR ALTER PROCEDURE sp_Reporte_ProxPagosVencer
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5
        S.IdSocio,
        S.Nombre,
        S.Apellido,
        C.Anio,
        C.Mes,
        C.Monto + C.Recargo AS Monto,
        DATEFROMPARTS(C.Anio, C.Mes, 10) AS FechaVencimiento,
        DATEDIFF(DAY, GETDATE(), DATEFROMPARTS(C.Anio, C.Mes, 10)) AS DiasRestantes
    FROM CUOTAS C
    INNER JOIN SOCIOS S ON S.IdSocio = C.IdSocio
    WHERE C.Estado = 'Deudor'
      AND DATEADD(DAY, -7, DATEFROMPARTS(C.Anio, C.Mes, 10)) <= GETDATE()
      AND DATEFROMPARTS(C.Anio, C.Mes, 10) >= GETDATE()
    ORDER BY FechaVencimiento ASC;
END;
GO






--   DATOS DE PRUEBA
INSERT INTO PLANES (Nombre, PrecioMensual, MaxHorasSemana, Activo)
VALUES 
('Básico', 10000.00, 2, 1),
('Media Máqina', 15000.00, 6, 1),
('Premium', 28000.00, 12, 1),
('Crack', 38000.00, 18, 1);
GO

INSERT INTO SOCIOS (Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, IdPlan, Activo) VALUES
('Juan',      'Pérez',        '12345678', '1990-05-10', '1122334455',   'juanperez@mail.com',          2, 1),
('María',     'Gómez',        '28999888', '1988-03-25', '11-5555-2222', 'maria.gomez@example.com',     2, 1),
('Lucía',     'Fernández',    '33222444', '1995-11-03', '11-5555-3333', 'lucia.fernandez@example.com', 1, 1),
('Carlos',    'Rodríguez',    '30555333', '1987-07-19', '11-5555-4444', 'carlos.rodriguez@example.com',2, 1),
('Sofía',     'López',        '31222777', '1992-09-10', '11-5555-5555', 'sofia.lopez@example.com',     3, 1),
('Diego',     'Martínez',     '29888777', '1991-01-30', '11-5555-6666', 'diego.martinez@example.com',  1, 1),
('Valentina', 'Romero',       '32777111', '1996-12-15', '11-5555-7777', 'valentina.romero@example.com',2, 1),
('Mateo',     'Sánchez',      '30123456', '1993-04-08', '11-5555-8888', 'mateo.sanchez@example.com',   3, 1),
('Camila',    'Torres',       '31555666', '1994-02-22', '11-5555-9999', 'camila.torres@example.com',   1, 1),
('Nicolás',   'Flores',       '32222111', '1989-06-17', '11-5555-1010', 'nicolas.flores@example.com',  2, 1),
('Brenda',    'Silva',        '30444999', '1997-08-05', '11-5555-1112', 'brenda.silva@example.com',    3, 1),
('Tomás',     'Acosta',       '29999111', '1990-10-28', '11-5555-1313', 'tomas.acosta@example.com',    1, 1),
('Julieta',   'Molina',       '33333777', '1998-01-11', '11-5555-1414', 'julieta.molina@example.com',  2, 1),
('Gonzalo',   'Castro',       '29666123', '1986-09-02', '11-5555-1515', 'gonzalo.castro@example.com',  3, 1),
('Carolina',  'Vega',         '31000999', '1991-03-19', '11-5555-1616', 'carolina.vega@example.com',   1, 1);
GO

INSERT INTO USUARIOS (Email, PasswordHash, Rol, Activo, IdSocio)
VALUES ('admin@gym.com', 'admin123', 'Administrador', 1, NULL),
       ('juanperez@mail.com', 'socio123', 'Socio', 1, 1),
       ('maria.gomez@example.com', 'socio123', 'Socio', 1, 2),
       ('lucia.fernandez@example.com', 'socio123', 'Socio', 1, 3);
GO

INSERT INTO CUOTAS (IdSocio, Anio, Mes, Monto, Recargo, Estado, FechaPago, FormaPago)
VALUES
-- SEPTIEMBRE
(1, 2025, 9, 15000, 0, 'Pagado', '2025-09-02', 'Transferencia'),
(2, 2025, 9, 28000, 0, 'Pagado', '2025-09-03', 'Efectivo'),
(3, 2025, 9, 38000, 0, 'Pagado', '2025-09-03', 'Efectivo'),
-- OCTUBRE
(1, 2025, 10, 15000, 0, 'Deudor', NULL, NULL),
(2, 2025, 10, 28000, 0, 'Pagado', '2025-10-03', 'Transferencia'),
(3, 2025, 10, 38000, 1500, 'ConRecargo', '2025-10-15', 'Efectivo'),
-- NOVIEMBRE
(1, 2025, 11, 15000, 0, 'Deudor', NULL, NULL),
(2, 2025, 11, 28000, 0, 'Pagado', '2025-11-03', 'Transferencia'),
(3, 2025, 11, 38000, 1800, 'ConRecargo', '2025-11-13', 'Transferencia');
GO

INSERT INTO TURNOS_SOCIOS (IdTurno, IdSocio) VALUES
-- SOCIO 2 (Media Máquina - 6/sem)
(7, 2), (8, 2), (9, 2),
(10, 2), (11, 2), (12, 2),
-- SOCIO 3 (Básico - 2/sem)
(7, 3), (9, 3),
-- SOCIO 4 (Media Máquina)
(8, 4), (10, 4), (12, 4),
(13, 4), (14, 4), (15, 4),
-- SOCIO 5 (Premium)
(7, 5), (8, 5), (9, 5), (10, 5),
(11, 5), (12, 5), (13, 5), (14, 5),
(15, 5), (16, 5),
-- SOCIO 6 (Básico)
(11, 6), (13, 6),
-- SOCIO 7 (Media Máquina)
(7, 7), (10, 7), (12, 7),
(13, 7), (14, 7), (16, 7),
-- SOCIO 8 (Premium)
(8, 8), (9, 8), (10, 8), (11, 8),
(12, 8), (13, 8), (14, 8), (15, 8),
-- SOCIO 9 (Básico)
(7, 9), (8, 9),
-- SOCIO 10 (Media Máquina)
(9, 10), (10, 10), (11, 10),
(12, 10), (13, 10), (14, 10),
-- SOCIO 11 (Premium)
(7, 11), (8, 11), (9, 11), (10, 11),
(11, 11), (12, 11), (13, 11), (14, 11),
-- SOCIO 12 (Básico)
(15, 12), (16, 12),
-- SOCIO 13 (Media Máquina)
(7, 13), (8, 13), (11, 13),
(12, 13), (14, 13), (15, 13),
-- SOCIO 14 (Premium)
(7, 14), (9, 14), (11, 14), (13, 14),
(14, 14), (15, 14), (16, 14),
-- SOCIO 15 (Básico)
(12, 15), (13, 15);
GO
