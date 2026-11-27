using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo05.ExecuteNonQuery.Steps;

internal class AppelProcedureStocker
{
    // Prénom, nom et email arrivent en paramètre
    // => la méthode peut ajouter n'importe quel propriétaire
    public static async Task ExecuteAsync(string prenom, string nom, string email, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // quand on fait appel à une procédure stocké 
        // TOUJOURS METTRE LE COMMANDE TYPE A STORED PROCEDURE
        // Sinon il va prendre votre commandTexte comme une requête SQL
        // Et il ne va pas chercher la procédure stocker
        command.CommandType = CommandType.StoredProcedure;

        // Avec StoredProcedure, le CommandText = juste le NOM de la procédure
        // pas de "EXEC", pas de parenthèses, pas de paramètres dans le texte
        // ADO.NET construit lui-même le "EXEC AddProprietaire @Prenom, @Nom, @Email"
        command.CommandText = "AddProprietaire";

        // initilaiser les paramètre dans une procédure stocker
        /* 
         	@Prenom NVARCHAR(100),
	        @Nom NVARCHAR(100),
	        @Email NVARCHAR(100)
         */
        // Le nom des paramètres doit être EXACTEMENT celui de la procédure (@Prenom, @Nom, @Email)
        // Si un paramètre manque => SqlException "expects parameter '@Prenom', which was not supplied"
        // L'ordre n'a pas d'importance, c'est le NOM qui compte

        // 100 = la taille, la même que dans la procédure NVARCHAR(100)
        command.Parameters.Add("@Prenom", SqlDbType.NVarChar, 100).Value = prenom;
        command.Parameters.Add("@Nom", SqlDbType.NVarChar, 100).Value = nom;
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = email;

        // Comme la procédure renvoie un id
        // l'id qui vient d'être ajouté
        // on reçois que une seul valeur donc Scalar
        // Pas de executeNonQuery car notre procédure stocké ne renvoie pas les row affecté
        //   => elle fait SET NOCOUNT ON donc ExecuteNonQuery renverrait -1
        //   => et ExecuteNonQuery ignore le SELECT, on perdrait l'id
        // Pas de sql data reader car pas de ligne à lire
        //
        // Si la procédure faisait RETURN au lieu de SELECT
        // ExecuteScalar renverrait null => il faut un paramètre ReturnValue (voir Demo06 UpdateVoiturePrix)

        object? IdInsert = await command.ExecuteScalarAsync(cancellationToken);

        Console.WriteLine($"nouveau id insérer {(int)IdInsert}");



    }
}
