
using Demo06.AppelStoreProcedureAndTrigger.Steps;

// 1. Procédure stockée qui renvoie une valeur avec un SELECT
//    => on la récupère avec ExecuteScalar
//    On garde l'id du nouveau propriétaire pour l'étape 3
//    Email UNIQUE en DB => on en génère un différent à chaque lancement
//    grâce aux Ticks (l'heure en très précis), sinon SqlException au 2ème lancement
int nouveauProprietaireId = await AppelProcedureStockerAdd.ExecuteAsync(
    prenom: "Jean",
    nom: "Bernard",
    email: $"jean.bernard{DateTime.Now.Ticks}@hotmail.be");

Console.WriteLine();

// 2. Procédure stockée qui renvoie une valeur avec un RETURN
//    => on la récupère avec un paramètre de retour (ReturnValue)
await AppelProcedureStockerUpdate.ExecuteAsync(voitureId: 1, nouveauPrix: 36000m);

Console.WriteLine();

// 3. Un DELETE "normal" sur Proprietaire
//    => le trigger INSTEAD OF DELETE se déclenche tout seul
//    On supprime le propriétaire qu'on vient de créer à l'étape 1
//    comme ça on ne touche pas aux données de base
await AppelTriggerDeleteProprietaire.ExecuteAsync(nouveauProprietaireId);
