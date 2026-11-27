using Demo02.ExecuteScalar.Exercices;
using Demo02.ExecuteScalar.Steps;

await CompterVoitures.ExecuteAsync();

Console.Clear();

await PrixTotalVoiture.ExecuteAsync();

Console.Clear();

await PrixMoyenVoiture.ExecuteAsync();

Console.Clear();

//Console.Write("Ecrire l'identifiant de la voiture pour voir sa marque : ");
//string idChoisi = Console.ReadLine();

//string? voitureMarque = await GetVoitureById.ExecuteAsync(idChoisi);

//if (voitureMarque is not null)
//{
//    Console.WriteLine($"marque de la voiture {idChoisi} est {voitureMarque}");
//}
//else
//{
//    Console.WriteLine($"voiture avec l'id {idChoisi} introuvable");
//}

//Console.Clear();

//Console.WriteLine("Trouver le nom d'un propriétaire via son identifiant ?");
//int id = int.Parse(Console.ReadLine());

//string? resultat = await GetProprietaireById.ExecuteAsync(id);

//if(resultat is not null)
//{
//    Console.WriteLine(resultat);
//}
//else
//{
//    Console.WriteLine($"Proprietaire avec l'id {id} introuvable");
//}


string nom = await GetProprietaireByVoitureId.ExecuteAsync1("3");
Console.WriteLine(nom);