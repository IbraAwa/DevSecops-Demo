namespace Domain.Shared;

public static class ConnectionString
{
    // Non sécurisé 
    // mettre dans un json secret (clique droit sur le nom du projet)
    // ou dans un fichier qui est dasn le .gitignore
    // Pourquoi ?
    //pour empêcher votre connectionString d'être dans un repo github
   public const string Value = @"Server=(localdb)\MSSQLLocalDB;Database=DevSecops-Demo;Integrated Security=True;Trust Server Certificate=True;";
}
