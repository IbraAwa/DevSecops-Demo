using Domain.Shared;
using Domain.Shared.Models;
using Microsoft.Data.SqlClient;

namespace Demo04.DataReader.Steps;

internal class TransformerEnObjet
{
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Marque, Modele, Prix FROM Voiture";

        // Je crée une liste pour stocker ce que je vais récupérer dans la DB
        // et pouvoir le traiter ou l'afficher correctement
        List<Voiture> voitures = new();

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            // Chaque ligne lue est transformée en objet Voiture grâce au mapper
            voitures.Add(VoitureMapper.VersVoiture(reader));
        }

        Console.WriteLine($"Il y a {voitures.Count} voitures dans la table Voiture");
        foreach (Voiture voiture in voitures)
        {
            Console.WriteLine($"La voiture {voiture.Id} est");
            Console.WriteLine($"une {voiture.Marque} du modèle {voiture.Modele} et coûte {voiture.Prix}");
        }
    }
}
