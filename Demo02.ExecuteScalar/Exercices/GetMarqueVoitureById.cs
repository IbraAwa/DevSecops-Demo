using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace Demo02.ExecuteScalar.Exercices;

internal class GetMarqueVoitureById
{
    // Quand vous crée une méthode Asynchrone
    // il est à 99% accompagné d'un CancellationToken cancellationToken
    //Le CancellationToken permet d'arrêter via un délay crée par vous
    // ou via un Http.Abort la requête
    // imaginons la requête est bloqué car trop longue ou elle a bug
    // Le cancellationToken vous donne la possiblité d'annulé la méthode
    public static async Task<string?> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT Marque FROM Voiture WHERE Id = {id}";

        //Facultative mais tu peux dire le type de commande
        //pourquoi facultative car le type par défaut est du texte
        command.CommandType = CommandType.Text;

        // Pourquoi ExecuteScalar ?
        // Car nous avons besoin d'une seul donnée (la marque)
        // et ExecuteScalar récupère bien qu'une seul donnée (même si plusieurs sont renvoyé)
        object? marqueObj = await command.ExecuteScalarAsync();

        return marqueObj switch
        {
            null or DBNull => null,
            string marque => marque,
            _ => $"la requête à renvoyer {marqueObj.GetType().Name} alors qu'on attendait un string"
        };
    }
}
