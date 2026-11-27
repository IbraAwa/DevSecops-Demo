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

        // Je crée une liste de ce que je vais récuperer dans la DB
        // pour la stocker et pouvoir la traiter ou l'afficher corectement
        List<Voiture> voitures = new();

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync())
        {
            voitures.Add(VoitureMapper.VersVoiture(reader));
        }

        Console.WriteLine($"Il y'a {voitures.Count} dans la table voiture");
        foreach (Voiture voiture in voitures)
        {
            Console.WriteLine($"La voiture {voiture.Id} est");
            Console.WriteLine($"une {voiture.Marque} du modèle {voiture.Modele} et coûte {voiture.Prix}");
        }
    }
}
