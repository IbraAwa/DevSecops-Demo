using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo04.DataReader.Steps;

internal class LireAvecGetter
{
    // Méthode async (asynchrone) car on appelle une ressource externe (la DB)
    // static uniquement pour la démo, pas obligatoire
    public static async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        using SqlCommand command = connection.CreateCommand();

        // Pas d'ExecuteScalar quand ma requête renvoie plusieurs lignes et colonnes
        // (ExecuteScalar ne renvoie que la 1re colonne de la 1re ligne)
        command.CommandText = "SELECT Id, Marque, Prix, Modele FROM Voiture";

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        // GetOrdinal donne la position d'une colonne à partir de son nom
        // On le fait une seule fois AVANT la boucle plutôt qu'à chaque ligne
        int positionId = reader.GetOrdinal("Id");
        int positionMarque = reader.GetOrdinal("Marque");
        int positionModele = reader.GetOrdinal("Modele");
        int positionPrix = reader.GetOrdinal("Prix");

        while (await reader.ReadAsync(cancellationToken))
        {
            // Les GetXxx renvoient directement le bon type, pas besoin de cast
            // ATTENTION :
            // - le getter doit correspondre au type SQL (int => GetInt32, decimal => GetDecimal...) sinon InvalidCastException
            // - ils throw une exception si la valeur est NULL en DB
            //   => pour une colonne nullable, vérifier avant avec reader.IsDBNull(position)
            int id = reader.GetInt32(positionId);
            string marque = reader.GetString(positionMarque);
            string modele = reader.GetString(positionModele);
            decimal prix = reader.GetDecimal(positionPrix);

            Console.WriteLine($" {id}, {marque}, {modele}, {prix}");
        }


    }
}
