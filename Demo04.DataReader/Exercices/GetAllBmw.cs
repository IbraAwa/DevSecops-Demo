using Domain.Shared;
using Domain.Shared.Models;
using Microsoft.Data.SqlClient;

namespace Demo04.DataReader.Exercices;

internal class GetAllBmw
{
    // La marque arrive en paramètre ("BMW" est choisi dans Program.cs)
    // => la même méthode marche pour Audi, Tesla...
    public static async Task ExecuteAsync(string marque, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Marque FROM Voiture WHERE Marque = @Marque";

        // Marque NVARCHAR(50) en DB => même taille
        command.Parameters.Add("@Marque", System.Data.SqlDbType.NVarChar, 50).Value = marque;

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        List<Voiture> voitures = new();

        while (await reader.ReadAsync(cancellationToken))
        {
            voitures.Add(MapperAmeliorer.ToVoiture(reader));
        }

        foreach (Voiture voiture in voitures)
        {
            Console.WriteLine($"voiture {voiture.Id} {voiture.Marque}");
        }
    }
}
