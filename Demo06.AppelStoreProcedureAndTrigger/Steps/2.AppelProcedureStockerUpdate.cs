using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo06.AppelStoreProcedureAndTrigger.Steps;

internal class AppelProcedureStockerUpdate
{

    // ATTENTION : différence avec AddProprietaire
    // AddProprietaire     => SELECT ...  => la valeur arrive comme un RÉSULTAT (une ligne)  => ExecuteScalar
    // UpdateVoiturePrix   => RETURN ...  => la valeur arrive comme un CODE RETOUR          => paramètre ReturnValue
    //
    // Un RETURN n'est PAS un résultat
    // ExecuteScalar ici renverrait null car la procédure ne fait aucun SELECT
    public static async Task ExecuteAsync(int voitureId, decimal nouveauPrix, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // Toujours : on appelle une procédure stockée
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "UpdateVoiturePrix";

        command.Parameters.Add("@Id", SqlDbType.Int).Value = voitureId;

        // Pour un DECIMAL on précise la précision et l'échelle comme dans la procédure : DECIMAL(15,2)
        // Precision = nombre total de chiffres (15)
        // Scale     = nombre de chiffres après la virgule (2)
        SqlParameter prixParam = command.Parameters.Add("@Prix", SqlDbType.Decimal);
        prixParam.Precision = 15;
        prixParam.Scale = 2;
        prixParam.Value = nouveauPrix;

        // Paramètre de RETOUR
        // C'est ici qu'ADO.NET va ranger la valeur du "RETURN @@ROWCOUNT"
        // Le nom n'a pas d'importance (il n'existe pas dans la procédure), on met @RETURN_VALUE par convention
        // Ce qui compte c'est Direction = ReturnValue
        // Un RETURN en SQL Server renvoie TOUJOURS un INT
        SqlParameter retourParam = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
        retourParam.Direction = ParameterDirection.ReturnValue;

        int ligne = await command.ExecuteNonQueryAsync(cancellationToken);
        Console.WriteLine($"[UPDATE] ExecuteNonQuery a renvoyé : {ligne} (à cause du SET NOCOUNT ON)");

        // C'est pour ça que la procédure fait RETURN @@ROWCOUNT
        // on récupère quand même le nombre de lignes modifiées, mais via le paramètre de retou        // La valeur n'est remplie QU'APRÈS l'exécution de la commande
        int lignesModifiees = (int)retourParam.Value;

        if (lignesModifiees == 1)
        {
            Console.WriteLine($"[UPDATE] La voiture {voitureId} a maintenant un prix de {nouveauPrix}");
        }

        if (lignesModifiees == 0)
        {
            Console.WriteLine($"[UPDATE] Aucune voiture trouvée avec l'id {voitureId}");
        }

        if (lignesModifiees > 1)
        {
            // Normalement impossible car Id est la clé primaire
            Console.WriteLine($"[UPDATE] Plusieurs voitures modifiées : {lignesModifiees}");
        }
    }
}
