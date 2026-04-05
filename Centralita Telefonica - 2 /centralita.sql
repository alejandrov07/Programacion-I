-- CREAR LA BASE DE DATOS
CREATE DATABASE CentralitaDB;

-- SELECCIONAR LA BASE DE DATOS PARA TRABAJAR
USE CentralitaDB;

-- CREAR LA TABLA DE LLAMADAS
CREATE TABLE Llamadas (
                          id_llamada INT AUTO_INCREMENT PRIMARY KEY,
                          numero_origen VARCHAR(20) NOT NULL,
                          numero_destino VARCHAR(20) NOT NULL,
                          duracion INT NOT NULL, -- Duración en segundos
                          costo_llamada DECIMAL(10, 2) NOT NULL,
                          fecha_registro TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO Llamadas (numero_origen, numero_destino, duracion, costo_llamada) VALUES
                                                                                  ('+34600111222', '+34912345678', 125, 0.55),
                                                                                  ('+14155552671', '+12125550199', 450, 2.25),
                                                                                  ('+525512345678', '+34600999888', 930, 15.40),
                                                                                  ('900123123', '+34611222333', 45, 0.00),
                                                                                  ('+5491144445555', '+5491166667777', 312, 1.10);

SELECT * FROM Llamadas;
