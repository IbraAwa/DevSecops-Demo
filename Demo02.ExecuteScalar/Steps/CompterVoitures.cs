using Microsoft.Data.SqlClient;

namespace Demo02.ExecuteScalar.Steps;

public static class CompterVoitures
{
    public static async Task ExecuteAsync()
    {
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;";

        // using sans accoldade ouvrante et fermante
        // se ferme à la fin du bloc de code ou il est
        // Si il est dans un if il se ferme à la fin du if
        // si dans une méthode i lse ferme a la fin de la méthode
        await using SqlConnection connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        await using SqlCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Voiture";

        Console.WriteLine("Request SQL :");
        Console.WriteLine(command.CommandText);
        Console.WriteLine();

        object? resultat = await command.ExecuteScalarAsync();
        int nombreVoiture = (int)resultat!; // ! => dire a C# je gère le nulle ne me donne pas d'avertissement*

        Console.WriteLine($"type reçu :  {resultat.GetType().Name}");
        Console.WriteLine($"Nombre de voiture : {nombreVoiture}");


    }
}
