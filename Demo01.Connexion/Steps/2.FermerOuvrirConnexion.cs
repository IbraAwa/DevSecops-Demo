using Microsoft.Data.SqlClient;

namespace Demo01.Connexion.Steps
{
    internal class FermerOuvrirConnexion
    {
        // Async permet d'utiliser dans la méthode de l'asynchrone
        // Task c'est une promesse/tache qui permet de dire je te préviens quand j'ai terminé
        public static async Task ExecuterAsync()
        {
            Console.WriteLine("Ouvrir fermer la connection");
            Console.WriteLine();

            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            Console.WriteLine($"Connection State : {connection.State}");

            // await permet au cuisinier de partir faire autre chose et de revenir quand c'est terminé
            // await permet de faire autre chose 
            await connection.OpenAsync();
            Console.WriteLine($"Après ouverture de la connexion : {connection.State}");
            Console.WriteLine($"Version du serveur : {connection.ServerVersion}");
            Console.WriteLine($"base de donné où on est connecté ? : {connection.Database}");
            Console.WriteLine();

            await connection.CloseAsync();
            Console.WriteLine($"après fermeture state connection :  {connection.State}");

            // GARBAGE COLLECTOR
            // sa permet la gestion de votre mémoire de manière automatisé
            // sans que vous devez faire quoi que ce soit

            // problème certaine chose surtout les choses qui demande une connection externe
            // ne partent pas automatiquement
            // il faut donc nous même le faire partir

            //DisposeAsync libère l'objet dans la mémoire
            await connection.DisposeAsync();

            Console.WriteLine("voulez vous continuer ?");
            Console.ReadKey();
        }
    }
}

