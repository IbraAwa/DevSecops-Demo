using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo05.ExecuteNonQuery.Steps;

internal class SupprimerUneVoiture
{
    // La marque à supprimer arrive en paramètre
    public static async Task ExecuteAsync(string marque, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // un delete doit toujours être accompagné d'un where
        // sinon vous supprimez TOUTE la table
        command.CommandText = "DELETE FROM Voiture WHERE Marque = @Marque";

        // Marque NVARCHAR(50)
        command.Parameters.Add("@Marque", SqlDbType.NVarChar, 50).Value = marque;

        // Execute Non Query
        // Execute une commande
        // et informe combien de ligne a été affecté
        int ligne = await command.ExecuteNonQueryAsync(cancellationToken);

        if(ligne > 1)
        {
            Console.WriteLine($"au total plusieurs lignes ont été modifie : {ligne}");
        }

        if(ligne == 1 )
        {
            Console.WriteLine($"au total juste une ligne a été modifie : {ligne}");
        }

        if(ligne < 0)
        {
            Console.WriteLine($"Probleme moin que 0 lignes ont été modifie : {ligne}");
        }

        if(ligne == 0)
        {
            Console.WriteLine($"Aucune ligne modifié : {ligne}");
        }
    }
}
