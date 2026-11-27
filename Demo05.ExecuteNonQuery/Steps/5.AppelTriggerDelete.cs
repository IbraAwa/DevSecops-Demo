using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo05.ExecuteNonQuery.Steps
{
    internal class DeleteProprietaire
    {
        // L'id du propriétaire à supprimer arrive en paramètre
        public static async Task ExecuteAsync(int proprietaireId, CancellationToken cancellationToken = default)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString.Value);
            await connection.OpenAsync(cancellationToken);

            await using SqlCommand command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Proprietaire WHERE Id = @Id";
            command.Parameters.Add("@Id", SqlDbType.Int).Value = proprietaireId;

            // Execute Non Query
            // Execute une commande
            // et informe combien de ligne a été affecté
            //
            // ATTENTION : Proprietaire a un trigger INSTEAD OF DELETE
            // => la ligne n'est PAS supprimée, le trigger met Actif = 0 à la place
            // => mais SQL Server répond quand même "1 ligne affectée"
            // (voir Demo06 AppelTriggerDeleteProprietaire pour la preuve)
            int ligne = await command.ExecuteNonQueryAsync(cancellationToken);

            if (ligne > 1)
            {
                Console.WriteLine($"au total plusieurs lignes ont été modifie : {ligne}");
            }

            if (ligne == 1)
            {
                Console.WriteLine($"au total juste une ligne a été modifie : {ligne}");
            }

            if (ligne < 0)
            {
                Console.WriteLine($"Probleme moin que 0 lignes ont été modifie : {ligne}");
            }

            if (ligne == 0)
            {
                Console.WriteLine($"Aucune ligne modifié : {ligne}");
            }
        }
    }
}
