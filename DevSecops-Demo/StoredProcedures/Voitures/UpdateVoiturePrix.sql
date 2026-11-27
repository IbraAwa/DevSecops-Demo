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
	-- ATTENTION en C# : un RETURN n'est PAS un résultat (pas un SELECT)
	-- executeScalar renverrait null
	-- executeNonQuery renverrait -1 (à cause du SET NOCOUNT ON)
	-- => on récupère la valeur avec un paramètre Direction = ParameterDirection.ReturnValue
	-- (voir Demo06 AppelProcedureStockerUpdate)
	-- RETURN renvoie TOUJOURS un INT
	RETURN @@ROWCOUNT
END
