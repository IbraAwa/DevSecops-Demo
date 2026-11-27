
using Demo05.ExecuteNonQuery.Steps;

// Toutes les valeurs sont choisies ICI
// les méthodes, elles, ne contiennent plus aucune valeur codée en dur

//await InsererUneVoiture.ExecuteAsync(marque: "Toyota", modele: "Yaris", prix: 15000m, proprietaireId: 5);

//await ModifierUneVoiture.ExecuteAsync(marque: "Toyota", nouveauPrix: 18000m);

//await SupprimerUneVoiture.ExecuteAsync(marque: "Toyota");

// Email UNIQUE en DB => on en génère un différent à chaque lancement
//await AppelProcedureStocker.ExecuteAsync(prenom: "Jean", nom: "Bernard", email: $"jean.bernard{DateTime.Now.Ticks}@hotmail.be");

await DeleteProprietaire.ExecuteAsync(proprietaireId: 6);
