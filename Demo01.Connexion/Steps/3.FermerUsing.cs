using Microsoft.Data.SqlClient;

namespace Demo01.Connexion.Steps;

public static class FermerUsing
{
    // TASK = VOID
    // TASK<Voiture> = Voiture
    // (en async on renvoie Task à la place de void, et Task<Voiture> à la place de Voiture)
    public static async Task ExecuterAsync()
    {
        // on garde une référence vers la connexion pour pouvoir
        // regarder son state APRÈS le using
        SqlConnection referenceGarder;

        // un très beau magic string en espérant ne pas changer ma connectionString un jour
        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;";

        // Je crée un sqlConnection dans la parenthèse qui vivra seulement entre les
        // deux accolades {}
        await using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // pas besoin de DisposeAsync() car mon using le fait tout seul
            await connection.OpenAsync();
            Console.WriteLine($"dans notre using => state : {connection.State}");
            referenceGarder = connection;
        }

        // affiche Closed : le using a bien fermé la connexion tout seul
        Console.WriteLine($"après mon using le state de referenceGarder est");
        Console.WriteLine(referenceGarder.State);
    }
}
