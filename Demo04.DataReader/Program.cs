using Demo04.DataReader.Exercices;
using Demo04.DataReader.Steps;

//await LireAvecIndexeurs.ExecuteAsync();


//await LireAvecGetter.ExecuteAsync();


//await TransformerEnObjet.ExecuteAsync();

//await LireView.ExecuteAsync();

//await GetAllBmw.ExecuteAsync(marque: "BMW");


Console.WriteLine("Choisir la première lettre des propréitaires retourner");
char letter = char.Parse(Console.ReadLine());
await GetProprietaireByFirstLetter.ExecuteAsync(letter);