// See https://aka.ms/new-console-template for more information
using Demo03.RequeteParametrer.Steps;

Console.WriteLine("Hello, World!");


await SansParametre.ExecuteAsync("5");

Console.WriteLine();

// Je vais regarder si une injection sql est possible
// en mettant juste un ' a lieu d'un nombre ou d'un texte
// Si SQL renvoie une erreur c'est que une injection SQL est possible
// Pourquoi ? Car il va considérer le paramètre comme du texte et donc 
// fermer la chaine de caractère et il y'aura un ' orphelin donc erreur de syntaxe
//await SansParametre.ExecuteAsync("'");

// Effectivement erreur throw par SGBD
// Donc Sql Injection (injecter des requête SQL en profitant d'une autre requête SQL)


// Utilisation de l'injection
// Utilisation de "OR" ainsi le where est toujours vrai et me renvoie tout
// utilisaiton de -- pour commenter les autres condition pssible dans la requête
// au lieu de peugeot on aura la premiere marque de la table
// Comme executeScalar = une valeur ça va mais si cétait un dataReader 
// on aurait toute la table
await SansParametre.ExecuteAsync("0 OR '1' = '1'  -- ");

Console.WriteLine();


// ============================================================
// Même requête, même injection, mais avec des paramètres
// ============================================================
string injection = "0 OR '1' = '1'  -- ";

// 2. AddWithValue : type deviné (string -> NVARCHAR)
Console.WriteLine("=== 2. AddWithValue ===");
await AddWithValue.ExecuteAsync("5");
try
{
    await AddWithValue.ExecuteAsync(injection);
}
catch (Exception ex)
{
    // SqlException : SQL Server n'arrive pas à convertir le texte en INT
    // => l'injection est traitée comme une VALEUR, pas comme du code
    Console.WriteLine($"Injection bloquée ({ex.GetType().Name}) : {ex.Message}");
}

Console.WriteLine();

// 3. Moderne : new SqlParameter { ... } avec type explicite
Console.WriteLine("=== 3. Moderne ===");
await Moderne.ExecuteAsync("5");
try
{
    await Moderne.ExecuteAsync(injection);
}
catch (Exception ex)
{
    // Exception côté C# : la requête n'est même pas partie vers SQL Server
    Console.WriteLine($"Injection bloquée ({ex.GetType().Name}) : {ex.Message}");
}

Console.WriteLine();

// 4. Recommandé : Parameters.Add(nom, type).Value = valeur
Console.WriteLine("=== 4. Recommandé ===");
await Recommande.ExecuteAsync("5");
try
{
    await Recommande.ExecuteAsync(injection);
}
catch (Exception ex)
{
    Console.WriteLine($"Injection bloquée ({ex.GetType().Name}) : {ex.Message}");
}