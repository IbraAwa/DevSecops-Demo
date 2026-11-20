CREATE TABLE [dbo].[Voiture]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, -- IDENTITY incrémentation auto des Ids 
    [Marque] NVARCHAR(50) NOT NULL, 
    [Modele] NVARCHAR(100) NOT NULL, 
    [Prix] DECIMAL(15, 2) NOT NULL, 
    [ProprietaireId] INT NOT NULL
    CONSTRAINT [FK_Voiture_Proprietaire] FOREIGN KEY ([ProprietaireId]) REFERENCES [Proprietaire]([Id])
)
