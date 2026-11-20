CREATE TABLE [dbo].[Proprietaire]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	Prenom NVARCHAR(100) NOT NULL, 
    [Nom] NVARCHAR(100) NOT NULL, 
    [Email] NCHAR(355) NOT NULL UNIQUE,
	[Actif] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [CK_Proprietaire_Email] CHECK ([email] LIKE '%@%.%')
)

GO
CREATE INDEX [IX_Proprietaire_Email] ON [dbo].[Proprietaire] ([Email])

