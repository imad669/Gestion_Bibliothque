USE master;
GO
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'BibliothequeTP2') DROP DATABASE BibliothequeTP2;
GO
CREATE DATABASE BibliothequeTP2;
GO
USE BibliothequeTP2;
GO

CREATE TABLE Livres (
    IdLivre INT IDENTITY(1,1) PRIMARY KEY,
    Titre NVARCHAR(100) NOT NULL,
    Auteur NVARCHAR(80) NOT NULL,
    Annee INT,
    ISBN VARCHAR(20),
    Categorie NVARCHAR(50),
    QuantiteEnStock INT NOT NULL DEFAULT 1,
    QuantiteDisponible INT NOT NULL DEFAULT 1
);

CREATE TABLE Usagers (
    IdUsager INT IDENTITY(1,1) PRIMARY KEY,
    Nom NVARCHAR(80) NOT NULL,
    Email NVARCHAR(100),
    Telephone VARCHAR(20)
);

CREATE TABLE Emprunts (
    IdEmprunt INT IDENTITY(1,1) PRIMARY KEY,
    DateEmprunt DATE NOT NULL DEFAULT GETDATE(),
    DateRetourPrev DATE NOT NULL,
    DateRetourReel DATE NULL,
    IdLivre INT NOT NULL,
    IdUsager INT NOT NULL,
    CONSTRAINT FK_Emprunts_Livre FOREIGN KEY (IdLivre) REFERENCES Livres(IdLivre),
    CONSTRAINT FK_Emprunts_Usager FOREIGN KEY (IdUsager) REFERENCES Usagers(IdUsager)
);
