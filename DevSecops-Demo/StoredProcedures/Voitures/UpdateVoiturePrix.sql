CREATE PROCEDURE [dbo].[UpdateVoiturePrix]
	@Id int,
	@Prix DECIMAL(15,2)
AS
BEGIN
	SET NOCOUNT ON; --pas de messages "(1 row afected)" donc pas besoin de gérer ça

	UPDATE [dbo].[Voiture]
	SET [Prix] = @Prix
	WHERE [Id] = @Id

	-- ROWCOUNT = nombre de ligné touchées par l'update 
	-- de notre requête juste au dessus
	-- C# on aura une méthode executeScalar qui va récupérer le rowCount en INT 
	RETURN @@ROWCOUNT 
END
