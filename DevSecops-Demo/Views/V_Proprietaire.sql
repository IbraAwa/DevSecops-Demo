CREATE VIEW [dbo].[V_Proprietaire]
AS 
SELECT 
	[Id], [Prenom], [Nom], [Email] 
FROM 
	[dbo].[Proprietaire]
WHERE
	Actif = 1;

--une view est une requete sql prè-enregister
--qui peut être appelé partout
--permet d'avoir un code uniforme si on doit écrire la requête
--a plusieurs endroit du code
-- et d'écrire le long code que une fois