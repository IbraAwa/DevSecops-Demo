using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Demo05.ExecuteNonQuery.Steps;

internal class ModifierUneVoiture
{
    // La marque ciblée et le nouveau prix arrivent en paramètre
    // => plus rien de codé en dur dans la méthode
    public static async Task ExecuteAsync(string marque, decimal nouveauPrix, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new SqlConnection(ConnectionString.Value);
        await connection.OpenAsync(cancellationToken);

        await using SqlCommand command = connection.CreateCommand();

        // un update doit toujours être accompagné d'un where
        // sinon vous modifier TOUTE els lignes
        // Les valeurs du SET et du WHERE sont des paramètres
        command.CommandText = "UPDATE Voiture SET Prix = @Prix WHERE Marque = @Marque";

        // Prix DECIMAL(15,2) => on donne la même précision / échelle
        SqlParameter prixParam = command.Parameters.Add("@Prix", SqlDbType.Decimal);
        prixParam.Precision = 15;
        prixParam.Scale = 2;
        prixParam.Value = nouveauPrix;

        // Marque NVARCHAR(50)
        command.Parameters.Add("@Marque", SqlDbType.NVarChar, 50).Value = marque;


        // Execute la requête
        // et retourne les ligne affecté
        int ligne = await command.ExecuteNonQueryAsync(cancellationToken);

        if (ligne > 1)
        {
            Console.WriteLine("Plusieurs voitures ont été modifié");
        }
        if (ligne == 0)
        {
            Console.WriteLine("Aucune voiture modifié");
        }
        
        if (ligne < 0)
        {
            Console.WriteLine("Problème");
        }

        if(ligne == 1)
        {
            Console.WriteLine("une seul voiture a été modifié");
        }
        
    }
}
