using Domain.Shared;
using Microsoft.Data.SqlClient;

namespace Demo01.Connexion.Steps;

public static class LireChaineConnexion
{
    // Data Source=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;

    // Une chaîne de connexion c'est une suite de "clé=valeur;"
    // chaque morceau dit à SqlClient OÙ et COMMENT se connecter

    // L'adresse du serveur
    // le chemin vers le serveur dans votre mssql
    // Server=(localdb)\MSSQLLocalDB;   (Data Source= c'est la même chose que Server=)


    // Ici c'est la base de données que je dois cibler
    // dans le serveur donné ci-dessus
    //Database=DevSecops-Demo;


    // Comment je m'authentifie
    // True => j'utilise mon compte Windows, pas besoin de login / mot de passe
    // Integrated Security=True;


    // La connexion est chiffrée et le serveur montre un certificat
    // True => on fait confiance au certificat sans le vérifier
    // en local avec localdb on met True sinon ça plante, en prod on laisse False
    // Trust Server Certificate=True;

    public static void Executer()
    {
        Console.WriteLine("Lire la chaîne de connexion");
        Console.WriteLine();

        string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;";

        // SqlConnectionStringBuilder découpe la chaîne de connexion
        // et transforme chaque "clé=valeur" en propriété C#
        // => plus besoin de lire la string à la main
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString.Value);

        Console.WriteLine($"Serveur : {builder.DataSource}");
        Console.WriteLine($"Base de données : {builder.InitialCatalog}"); // Database= devient InitialCatalog
        Console.WriteLine($"Compte Windows (Integrated Security) : {builder.IntegratedSecurity}");
        Console.WriteLine($"Confiance certificat serveur : {builder.TrustServerCertificate}");
        Console.WriteLine();

        // Les clés qu'on n'a pas écrites prennent leur valeur par défaut
        Console.WriteLine($"Chiffrement (Encrypt) : {builder.Encrypt}");
        Console.WriteLine($"Pooling : {builder.Pooling}");
        Console.WriteLine($"Timeout de connexion (secondes) : {builder.ConnectTimeout}");
        Console.WriteLine();

        // ConnectionString renvoie la chaîne reconstruite par le builder
        // avec les noms "officiels" des clés (Data Source, Initial Catalog...)
        Console.WriteLine("Chaîne reconstruite par le builder :");
        Console.WriteLine(builder.ConnectionString);

        Console.WriteLine();
        Console.WriteLine("voulez-vous continuer ?");
        Console.ReadKey();
    }
}
