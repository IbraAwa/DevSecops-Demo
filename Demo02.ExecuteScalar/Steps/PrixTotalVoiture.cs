using Microsoft.Data.SqlClient;
namespace Demo02.ExecuteScalar.Steps;

public static class PrixTotalVoiture
{
    public static async Task ExecuteAsync()
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;";
        await using SqlConnection connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT SUM(Prix) FROM Voiture";

        object? resultat = await command.ExecuteScalarAsync();

        if(resultat is not null)
        {
            bool estconverti = decimal.TryParse(resultat.ToString(), out decimal SommeTotalPrixVoiture);

            if(estconverti)
            {
                Console.WriteLine("Conversion réussi");
                Console.WriteLine($"La somme total du prix des voiture est de : {SommeTotalPrixVoiture}");
            } else
            {
                Console.WriteLine("Erreur de conversion");
            }
        } else
        {
            Console.WriteLine("Aucun résultat quand on a essayé d'avoir la somme des prix des voitures");
        }
    }
}
