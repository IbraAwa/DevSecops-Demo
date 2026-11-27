// See https://aka.ms/new-console-template for more information
using Demo03.RequeteParametrer.Steps;

Console.WriteLine("Hello, World!");


// Utilisation normale et non mal veillante
await SansParametre.ExecuteAsync("5");


// Je test si une injection SQL est possible
// j'insère ' et si cela crée une erreur SQL je sais que je peux faire une injection SQL
// Sert à trouver les injection sql possible sur votre application
//await SansParametre.ExecuteAsync("'"); // Throw une exception de syntaxe


// Utilisation malveillante j'injecte une requête SQL non voulu
// Grace au UNION je prend une autre table qui pourrait contenir des password
await SansParametre.ExecuteAsync("' OR '1' = '1'  UNION SELECT Nom FROM Proprietaire --");