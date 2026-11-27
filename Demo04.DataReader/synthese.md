# Synthèse Demo04 – SqlDataReader

## C'est quoi ?
`ExecuteReader` renvoie un `SqlDataReader` qui lit le résultat **ligne par ligne**.
- Pour les requêtes qui renvoient **plusieurs lignes et/ou colonnes**.
- **Lecture seule** et **vers l'avant uniquement** (impossible de revenir en arrière).
- La connexion doit rester **ouverte** pendant toute la lecture.

## Le modèle de base
```csharp
command.CommandText = "SELECT Id, Nom, Prenom FROM Proprietaire";
await using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

while (await reader.ReadAsync(cancellationToken)) // avance d'une ligne, false quand c'est fini
{
    // lire la ligne en cours
}
```
- Avant le 1er `ReadAsync()`, le reader n'est sur **aucune** ligne.
- **Toujours** récupérer l'`Id` : c'est avec lui qu'on fera les autres requêtes (détail, update, delete).

## 3 façons de lire une colonne

| Façon | Exemple | Renvoie | Remarque |
|---|---|---|---|
| Par position | `reader[0]` | `object` | casse si on change l'ordre du SELECT |
| Par nom | `reader["Nom"]` | `object` | **recommandé**, il faut caster |
| Getter typé | `reader.GetString(pos)` | le bon type | rapide, mais plante sur NULL |

```csharp
int positionPrix = reader.GetOrdinal("Prix"); // position via le nom, UNE fois avant la boucle
decimal prix = reader.GetDecimal(positionPrix);
```
- Getter ≠ type SQL → `InvalidCastException` (`INT` → `GetInt32`, `DECIMAL` → `GetDecimal`...).
- Colonne non sélectionnée → `IndexOutOfRangeException`.
- Colonne nullable → vérifier `reader.IsDBNull(position)` avant le getter.

## Mapper : ligne SQL → objet C#
Un mapper transforme une ligne du reader en objet (`Voiture`, `Proprietaire`).
```csharp
List<Voiture> voitures = new();
while (await reader.ReadAsync(cancellationToken))
{
    voitures.Add(VoitureMapper.VersVoiture(reader));
}
```
- Le mapper doit **correspondre au SELECT** (pas de colonne absente).
- Gérer le NULL même sur une colonne `NOT NULL` : si la DB change un jour, rien ne casse.
```csharp
string modele = reader["Modele"] is DBNull ? "" : (string)reader["Modele"];
```

## Mapper amélioré : méthode d'extension (Tools.Shared)
```csharp
int id = reader.Get<int>("Id");                  // NULL ou colonne absente => 0
string nom = reader.Get<string>("Nom") ?? "";    // ?? = si null à gauche, prend la droite
```
- `this SqlDataReader reader` → on ajoute une méthode **directement** au reader.
- `ColumnExist` vérifie que la colonne est dans le SELECT (sans tenir compte des majuscules).
- `Get<T>` gère colonne absente + `DBNull` + cast en un seul appel.

## Les vues (VIEW)
```sql
CREATE VIEW V_Proprietaire AS SELECT Id, Prenom, Nom, Email FROM Proprietaire WHERE Actif = 1;
```
```csharp
command.CommandText = "SELECT * FROM V_Proprietaire";
```
- Une vue = une **requête SQL enregistrée** dans la DB, utilisable comme une table.
- La longue requête est écrite **une seule fois** → code uniforme partout.

## À retenir
1. Plusieurs lignes → `ExecuteReader` + `while (await reader.ReadAsync())`.
2. Lire par **nom** de colonne plutôt que par position.
3. Toujours gérer `DBNull`.
4. Un mapper par objet, qui correspond au SELECT.
5. Requête répétée partout → une **vue**.
