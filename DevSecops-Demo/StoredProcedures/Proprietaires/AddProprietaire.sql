--Une procédure stocké est comme une fonction que vous pouvez appeler
--pour faire une action
-- dans notre cas cette action est rajouté un propriétaire

--notre procédure stocké renvoie (on doit le coder) l'id qu'il a générer
-- dans notre cas il crée un propriétaire et il renvoie l'id du propriétaire qui vient
-- d'être crée

/* 
	Prenom NVARCHAR(100) NOT NULL, 
    [Nom] NVARCHAR(100) NOT NULL, 
    [Email] NCHAR(355) NOT NULL UNIQUE,
*/

CREATE PROCEDURE [dbo].[AddProprietaire]
	@Prenom NVARCHAR(100),
	@Nom NVARCHAR(100),
	@Email NVARCHAR(100)
AS
BEGIN
	--coupe le message (1 row affected) renvoyé aprè chaque instruction
	--ça permet de garder que l'id nouvellement recrée
	SET NOCOUNT ON

	INSERT INTO [dbo].[Proprietaire] ([Prenom], [Nom], [Email])
	VALUES (@Prenom, @Nom, @Email)

	-- SCOPE IDENTITY = le dernier id générer par notre procédure (Id = propriétaire)
	-- Pourquoi le cast ?
	-- Car les SCOPE IDENTITY renvoie des decimal 
	-- en C# on utilsier cmd.ExecuteScalar() qui va renvoyer int
	SELECT CAST(SCOPE_IDENTITY() AS INT)
END
