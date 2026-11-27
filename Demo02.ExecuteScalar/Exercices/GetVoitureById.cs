using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo02.ExecuteScalar.Exercices;

internal class GetVoitureById
{
    // Une méthode async est très souvent voir tout le temps
    // Accompagné d'un cancellationToken
    // Le CancellationToken permet d'arrêter via un délay ou HTTP.Abort la requête
    // Si requête bloqué ou prenant trop de temps


    // !!! DISCLAIMER : INJECTION SQL !!!
    // Mauvais exemple d'une requête ADO 
    // Les autres exercices montre le bon exemple
    public static async Task<string?> ExecuteAsync(string id, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();

        // Attention mettre un texte d'interpolation laisse place au injection SQL 
        // Pourquoi ?
        // Car votre SGBD considère comme du texte votre {id}
        // et non comme un paramètre après le Id = ?
        // Nous verrons après comment se protéger des Sql Injection
        // qui est l'une des failles de cybersecurité les plus critique au monde
        command.CommandText = $"SELECT Marque FROM Voiture WHERE Id = '{id}'";

        object? marqueObj = await command.ExecuteScalarAsync();

        return marqueObj switch
        {
            null or DBNull => null,
            string marque => marque,
            _ => throw new InvalidCastException(
                $"La requête à renvoyer {marqueObj.GetType().Name} alors qu'on attendait" +
                $"un type string"
                )
        };

    }
}
