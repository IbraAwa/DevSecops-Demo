using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo03.RequeteParametrer.Steps;

internal class SansParametre
{
    public static async Task ExecuteAsync(string voitureId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = $"SELECT Marque FROM Voiture WHERE Id = {voitureId}";

        // ExecuteScalar quand on récupère une seule valeur
        // un seul champs dans une table/colonne/ligne
        object? marqueObj = await command.ExecuteScalarAsync();

        string message = marqueObj switch
        {
            null or DBNull => $"Aucune maruqe trouver avec l'id {voitureId}",
            string marque => $"la marque de la voiture numéro {voitureId} est {marque}",
            _ => throw new InvalidCastException("casting de la marque en string impossible")
        };

        Console.WriteLine(message);
    }
}
