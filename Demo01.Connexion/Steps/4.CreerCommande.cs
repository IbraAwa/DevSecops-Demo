using Domain.Shared;
using Microsoft.Data.SqlClient;
namespace Demo01.Connexion.Steps;

public static class CreerCommande
{
    public static async Task ExecuteAsync()
    {
        Console.WriteLine();
        await using (SqlConnection connection = new SqlConnection(ConnectionString.Value))
        {
            await connection.OpenAsync();

            // SqlCommand permet de créer notre requête SQL
            // CreateCommand() permet de créer la commande
            // avec les informations de connexion
            await using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM Voiture";

                // ExecuteScalar renvoie une seule valeur,
                // de type object

                // ExecuteScalar ne sait pas quel type il va renvoyer : decimal ? string ? int ?
                // Comme il ne le sait pas et qu'en C# tout est objet,
                // il renvoie un object.
                // En C#, on peut passer de int à object sans souci,
                // mais pour passer de object à int, il faut caster.
                // Pourquoi ?
                // Parce qu'un object n'est pas forcément un int,
                // ALORS QU'un int est forcément un object.
                object? nombreLignesVoitureData = await command.ExecuteScalarAsync(); // Renvoie une seule valeur

                string? nombreLignesVoiture = null;
                if (nombreLignesVoitureData is not null)
                {
                    nombreLignesVoiture = nombreLignesVoitureData.ToString();
                    Console.WriteLine("Nombre de lignes dans la table Voiture : ");
                    Console.WriteLine($"{nombreLignesVoiture}");
                }
                else
                {
                    Console.WriteLine("Aucun résultat renvoyé");
                }
            }
        }
    }
}