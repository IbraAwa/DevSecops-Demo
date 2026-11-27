using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo03.RequeteParametrer.Steps;

internal class AddWithValue
{
    // 2ème manière : AddWithValue
    // Protège contre l'injection SQL (la valeur est envoyée à part, jamais concaténée)
    // MAIS peu recommandé : le type SQL est DEVINÉ à partir du type .NET
    public static async Task ExecuteAsync(string voitureId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = connection.CreateCommand();

        // Plus d'interpolation : on met un "trou" nommé @voitureId
        // Le SGBD compile la requête AVANT de recevoir la valeur
        // => la valeur ne peut plus jamais devenir du code SQL
        command.CommandText = "SELECT Marque FROM Voiture WHERE Id = @voitureId";

        // AddWithValue infère le type SQL via le type .NET :
        // string  -> NVARCHAR
        // decimal -> DECIMAL(précision déduite de la valeur, ex: 3.12 -> DECIMAL(3,2))
        // Ici la colonne Id est INT mais on envoie un NVARCHAR
        // => SQL Server fait une conversion implicite (ça marche avec "5"...)
        // => Si colonne VARCHAR et paramètre NVARCHAR : l'index n'est plus utilisé (BUG SILENCIEUX de perf)
        // => Avec "0 OR '1' = '1' -- " : pas d'injection, mais erreur de conversion côté SQL Server
        command.Parameters.AddWithValue("@voitureId", voitureId);

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
