using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo06.AppelStoreProcedureAndTrigger.Steps;

internal class AppelTriggerDeleteProprietaire
{

    // Un trigger ON NE L'APPELLE JAMAIS en C#
    // Pas de CommandType.StoredProcedure, pas de nom de trigger dans le CommandText
    // On écrit un DELETE tout à fait normal
    // et c'est SQL Server qui déclenche le trigger tout seul
    //
    // INSTEAD OF DELETE = "à la place du DELETE"
    // => le DELETE n'a jamais lieu
    // => le trigger fait un UPDATE Actif = 0 à la place (suppression logique / "soft delete")
    // => le propriétaire reste dans la table, ses voitures aussi (pas de souci avec la clé étrangère)
    public static async Task ExecuteAsync(int proprietaireId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        // ---------- 1. LE DELETE ----------
        await using SqlCommand deleteCommand = connection.CreateCommand();

        deleteCommand.CommandText = "DELETE FROM Proprietaire WHERE Id = @Id";
        deleteCommand.Parameters.Add("@Id", SqlDbType.Int).Value = proprietaireId;

        // ExecuteNonQuery renvoie le nombre de lignes affectées par NOTRE DELETE
        // PIÈGE : avec un trigger INSTEAD OF, SQL Server répond quand même "1 ligne affectée"
        // alors qu'aucune ligne n'a été supprimée !
        // => ExecuteNonQuery dit "combien de lignes étaient visées par le DELETE"
        //    pas "combien de lignes ont vraiment disparu"
        //
        // Le trigger fait SET NOCOUNT ON
        // => son UPDATE n'est PAS ajouté au total
        // Sans SET NOCOUNT ON dans le trigger, ExecuteNonQuery additionnerait
        // le DELETE + l'UPDATE du trigger => on aurait 2 au lieu de 1
        int ligne = await deleteCommand.ExecuteNonQueryAsync(cancellationToken);

        if (ligne == 1)
        {
            Console.WriteLine($"[DELETE] SQL Server dit : {ligne} ligne affectée pour le propriétaire {proprietaireId}");
        }

        if (ligne == 0)
        {
            Console.WriteLine($"[DELETE] Aucun propriétaire trouvé avec l'id {proprietaireId}");
            return;
        }

        // ---------- 2. ON VÉRIFIE CE QUE LE TRIGGER A FAIT ----------
        // Si le DELETE avait vraiment supprimé la ligne, ce SELECT ne trouverait rien (null)
        // Grâce au trigger la ligne existe toujours, avec Actif = 0
        await using SqlCommand verifCommand = connection.CreateCommand();
        verifCommand.CommandText = "SELECT Actif FROM Proprietaire WHERE Id = @Id";
        verifCommand.Parameters.Add("@Id", SqlDbType.Int).Value = proprietaireId;

        // Une seule valeur => ExecuteScalar
        // BIT en SQL => bool en C#
        object? actifObj = await verifCommand.ExecuteScalarAsync(cancellationToken);

        string message = actifObj switch
        {
            null or DBNull => $"[TRIGGER] Le propriétaire {proprietaireId} a vraiment été supprimé (le trigger n'a pas fonctionné ?)",
            false => $"[TRIGGER] Le propriétaire {proprietaireId} est toujours dans la table mais avec Actif = 0 => suppression logique",
            true => $"[TRIGGER] Le propriétaire {proprietaireId} est toujours actif ???",
            _ => "Valeur inattendue"
        };

        Console.WriteLine(message);
    }
}
