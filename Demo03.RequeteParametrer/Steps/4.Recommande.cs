using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo03.RequeteParametrer.Steps;

internal class Recommande
{
    // 4ème manière : la recommandée
    // Parameters.Add(nom, type).Value = valeur
    // => Type SQL explicite (comme Moderne)
    // => Court et lisible (contrairement à Moderne)
    // => Pas de type deviné (contrairement à AddWithValue)
    public static async Task ExecuteAsync(string voitureId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = "SELECT Marque FROM Voiture WHERE Id = @voitureId";

        // Le type correspond EXACTEMENT à la colonne Id (INT)
        // Pour un NVARCHAR(50) on préciserait aussi la taille :
        // command.Parameters.Add("@marque", SqlDbType.NVarChar, 50).Value = marque;
        command.Parameters.Add("@voitureId", SqlDbType.Int).Value = voitureId;

        // Encore mieux : recevoir directement un int dans la signature de la méthode
        // => l'injection ne compile même pas (première ligne de défense)
        // => le paramètre SQL reste quand même obligatoire (deuxième ligne de défense)

        object? marqueObj = await command.ExecuteScalarAsync(cancellationToken);

        string message = marqueObj switch
        {
            null or DBNull => $"Aucune marque trouvée avec l'id {voitureId}",
            string marque => $"la marque de la voiture numéro {voitureId} est {marque}",
            _ => throw new InvalidCastException("casting de la marque en string impossible")
        };

        Console.WriteLine(message);
    }
}
