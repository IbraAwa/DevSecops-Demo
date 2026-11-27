# Synthèse Demo05 – ExecuteNonQuery

## C'est quoi ?
"NonQuery" = **pas une requête de lecture**.
- Pour `INSERT`, `UPDATE`, `DELETE` (et `CREATE`, `DROP`...).
- Ne renvoie **aucune donnée**, juste un `int` = **le nombre de lignes affectées**.

## Le modèle de base
```csharp
await using SqlCommand command = connection.CreateCommand();
command.CommandText = "UPDATE Voiture SET Prix = @Prix WHERE Id = @Id";
command.Parameters.Add("@Prix", SqlDbType.Decimal).Value = 18000m;
command.Parameters.Add("@Id", SqlDbType.Int).Value = 1;

int ligne = await command.ExecuteNonQueryAsync(cancellationToken);
```

## Que veut dire le `int` renvoyé ?

| Valeur | Signification |
|---|---|
| `> 0` | nombre de lignes insérées / modifiées / supprimées |
| `0` | la requête a marché mais **aucune** ligne ne correspondait (WHERE qui ne trouve rien) |
| `-1` | SQL Server n'a **pas compté** : SELECT, CREATE/DROP, ou `SET NOCOUNT ON` |

⚠️ `-1` **n'est pas une erreur**. Une erreur SQL (contrainte, colonne inconnue...) lance une `SqlException`.

## Les pièges
- **UPDATE / DELETE sans WHERE** → modifie / supprime **TOUTES** les lignes.
- **IDENTITY** : SQL Server garde en cache des blocs d'Id. S'il crash, il saute au bloc suivant
  → des "trous" dans les Id (ex: 11 puis 1002). C'est normal, ne jamais compter sur des Id qui se suivent.
- **Clé étrangère** : supprimer un propriétaire qui a encore des voitures → `SqlException`.

## Procédure stockée (aperçu, détails en Demo06)
```csharp
command.CommandType = CommandType.StoredProcedure; // OBLIGATOIRE
command.CommandText = "AddProprietaire";           // juste le nom, pas de EXEC
command.Parameters.Add("@Prenom", SqlDbType.NVarChar, 100).Value = "Jean";
object? id = await command.ExecuteScalarAsync(cancellationToken);
```
- La procédure renvoie l'id avec un `SELECT` → `ExecuteScalar`, pas `ExecuteNonQuery`.
- Avec `SET NOCOUNT ON` dans la procédure, `ExecuteNonQuery` renverrait `-1`.

## Trigger (aperçu, détails en Demo06)
- `DELETE FROM Proprietaire WHERE Id = 6` déclenche le trigger `INSTEAD OF DELETE`.
- Le propriétaire n'est **pas supprimé**, il passe à `Actif = 0`.
- Mais `ExecuteNonQuery` renvoie quand même **1**.

## Quelle méthode Execute choisir ?

| Méthode | Quand | Renvoie |
|---|---|---|
| `ExecuteScalar` | 1 seule valeur | `object?` |
| `ExecuteReader` | plusieurs lignes / colonnes | `SqlDataReader` |
| `ExecuteNonQuery` | INSERT / UPDATE / DELETE | `int` (lignes affectées) |

## À retenir
1. ExecuteNonQuery = modifier des données, pas en lire.
2. Le `int` = lignes affectées (`0` = rien trouvé, `-1` = pas compté).
3. Les erreurs = `SqlException`, pas `-1`.
4. **Toujours** un WHERE sur UPDATE / DELETE.
