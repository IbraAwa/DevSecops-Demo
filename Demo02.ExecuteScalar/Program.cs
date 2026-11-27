using Demo02.ExecuteScalar.Exercices;
using Demo02.ExecuteScalar.Steps;

await CompterVoitures.ExecuteAsync();

Console.Clear();

await PrixTotalVoiture.ExecuteAsync();

Console.Clear();

await PrixMoyenVoiture.ExecuteAsync();

Console.Clear();

//Console.WriteLine("Quel voiture voulez-vous chercher ? taper son identifiant... ");
//int id = int.Parse(Console.ReadLine());

//string? message = await GetMarqueVoitureById.ExecuteAsync(id);

//Console.WriteLine(message);


//Console.WriteLine("Quel propriétaire voulez-vous chercher ? taper son identifiant... ");
//string id = Console.ReadLine();

//string? nomProprietaire = await GetProprietaireById.ExecuteAsync(id);

//Console.WriteLine($"le proprietaire avec l'id {id} est {nomProprietaire}");

Console.WriteLine("Quel propriétaire voulez-vous chercher via sa voiture Id ? taper identifiant de la voiture... ");
int id = int.Parse(Console.ReadLine());


// SUPER OPTIMISER
string? nomProprietaire = await GetProprietaireByVoitureId.ExecuteAsync(id);

Console.WriteLine($"le proprietaire de la voiture avec l'id {id} est {nomProprietaire}");

Console.WriteLine();
Console.WriteLine("-----------------");


Console.WriteLine("Quel propriétaire voulez-vous chercher via sa voiture Id ? taper identifiant de la voiture... ");
int id1 = int.Parse(Console.ReadLine());

// Double requête
// double latence
// ET si entre les deux requête un autre collègue à changer le propriétaire de la voiture
// vous pouvez perdre l'information
// montrer car SqlAlchemy fait ce genre de requête
// Parfois EFCore si mal utilisé
string? nomProprietaire1 = await GetProprietaireByVoitureId.ExecuteDoubleRequestAsync(id1);

Console.WriteLine($"le proprietaire de la voiture avec l'id {id1} est {nomProprietaire1}");



