using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo04.DataReader.Steps;

internal class LireAvecGetter
{
    // quand on appel une ressource externe
    // méthode async pour asynchrone
    
    // static que pour la démo pas obligatoire
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        using SqlCommand command = connection.CreateCommand();

        // Pas d'executeScalar
        // quand ma requête renvoie plusieurs lignes et informations
        command.CommandText = "SELECT Id, Marque, Prix, Modele FROM Voiture";

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        int positionId = reader.GetOrdinal("Id");
        int positonMarque = reader.GetOrdinal("Marque");
        int positionModele = reader.GetOrdinal("Modele");
        int positionPrix = reader.GetOrdinal("Prix");

        while (await reader.ReadAsync(cancellationToken))
        {
            int id = reader.GetInt32(positionId);
            string marque = reader.GetString(positonMarque);
            string modele = reader.GetString(positionModele);
            decimal prix = reader.GetDecimal(positionPrix);

            Console.WriteLine($" {id}, {marque}, {modele}, {prix}");
        }


    }
}
