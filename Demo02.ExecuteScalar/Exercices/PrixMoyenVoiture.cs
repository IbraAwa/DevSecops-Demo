using Domain.Shared;
using Microsoft.Data.SqlClient;
using System.Threading.Channels;

namespace Demo02.ExecuteScalar.Exercices;

internal class PrixMoyenVoiture
{
    public static async Task ExecuteAsync()
    {
        using SqlConnection connection = new(ConnectionString.Value);
        await connection.OpenAsync();   
        using SqlCommand command = connection.CreateCommand();


        // Si la colonne est de type int => Scalar renvoie un int
        // Vérifier type de la colonne ou on applique le AVG
        // Ou cast directement par le type attendu
        command.CommandText = "SELECT AVG(CAST(Prix AS DECIMAL(18, 2))) FROM Voiture";

        // CommandType par défaut
        // utile de précisé que pour procédure stocké
        command.CommandType = System.Data.CommandType.Text;

        object? prixMoyenStock = await command.ExecuteScalarAsync();

        // Pattern matching
        // Comme si
        // if(prixMoyenStock is null || prixMoyenStock is DbNull) return null
        // if(prixMoyenStock is decimal prix) return prix
        string message = prixMoyenStock switch
        {
            null or DBNull => "La requête a renvoyer une valeur null",
            decimal prix => $"Le prix moyen du stock est de ${prix} EUR",
            _ => throw new InvalidCastException(
                $"la requête à renvoyer un {prixMoyenStock.GetType().Name}, decimal attendu"
                )
        };

        Console.WriteLine(message);
    }
}
