using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo02.ExecuteScalar.Exercices;

internal class GetProprietaireById
{
    // TOUJOURS CANCELLATIONTOKEN QUAND METHODE ASYNC
    // DISCLAIMER INJECTION SQL POSSIBLE 
    public static async Task<string?> ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        // TRUE OR FALSE => TRUE
        // FALSE OR TRUE => TRUE
        // TRUE OR TRUE =>> TRUE
        // Je fais exprès OR comme ça ma condition est toujours vrai

        // Faire de l'interpolation de chaine de caractère en ADO ou même partout
        // n'est pas bon
        // Car il donne la possiblité de faire de l'injection SQL
        // Injection SQL => Injecter du SQL non voulu par l'application
        // Grâce à ça récupérer des info confidentiel dans la DB

        /* 
         SELECT Nom FROM Proprietaire WHERE Id = '' OR '1' = '1' 
         UNION SELECT Marque FROM Voiture -- '
         */

        // A Cause du dollars devant votre string
        // pour Sql {id} est du texte brute et non un paramètre à mettre après le Id =
        command.CommandText = $"SELECT Nom FROM Proprietaire WHERE Id = '{id}'";

        object? nomObj = await command.ExecuteScalarAsync();

        return nomObj switch
        {
            null or DBNull => null,
            string nom => nom,
            _ => $"La requête à renvoyer {nomObj.GetType().Name} alors qu'on attendait un stirng"
        };
    }
}
