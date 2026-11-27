using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo03.RequeteParametrer.Steps;

internal class Moderne
{
    // 3ème manière : on construit soi-même l'objet SqlParameter
    // Type SQL explicite => plus de devinette comme avec AddWithValue
    // Avantage : on a accès à TOUTES les propriétés (Size, Precision, Scale, Direction...)
    // Inconvénient : super long à écrire pour un simple paramètre
    public static async Task ExecuteAsync(string voitureId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = connection.CreateCommand();

        command.CommandText = "SELECT Marque FROM Voiture WHERE Id = @voitureId";

        // On dit clairement au SGBD : @voitureId est un INT
        // La conversion string -> int est faite par ADO.NET AVANT l'envoi
        // => "0 OR '1' = '1' -- " n'est pas un int : exception côté C#,
        //    la requête n'est même pas envoyée à SQL Server
        command.Parameters.Add(new SqlParameter("@voitureId", SqlDbType.Int)
        {
            Value = voitureId
        });

        // Exemple utile de la syntaxe longue : un DECIMAL(15,2) comme la colonne Prix
        // command.Parameters.Add(new SqlParameter("@prix", SqlDbType.Decimal)
        // {
        //     Precision = 15,
        //     Scale = 2,
        //     Value = prix
        // });

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
