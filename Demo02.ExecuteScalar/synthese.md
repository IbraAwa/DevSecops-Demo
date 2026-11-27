# Synthèse Demo02 – ExecuteScalar

## C'est quoi ?
`ExecuteScalar` exécute la requête et renvoie **une seule valeur** :
la **1re colonne de la 1re ligne**. Tout le reste est ignoré.

Parfait pour : `COUNT(*)`, `SUM(...)`, `AVG(...)`, `MAX(...)`, une seule info d'une ligne (`SELECT Marque ... WHERE Id = ...`).

## Le modèle de base
```csharp
await using SqlConnection connection = new SqlConnection(ConnectionString.Value);
await connection.OpenAsync();

await using SqlCommand command = connection.CreateCommand();
command.CommandText = "SELECT COUNT(*) FROM Voiture";

object? resultat = await command.ExecuteScalarAsync();
```

## Pourquoi `object?` ?
- ExecuteScalar ne sait pas à l'avance ce qu'il renvoie (int ? decimal ? string ?).
- En C# tout est `object` → il renvoie un `object`.
- `int` → `object` : automatique. `object` → `int` : il faut **caster** (un object n'est pas forcément un int).

## Les 3 cas possibles

| Ce que renvoie SQL | Ce qu'on reçoit en C# |
|---|---|
| une valeur | l'objet (int, decimal, string...) |
| aucune ligne (WHERE qui ne trouve rien) | `null` |
| une ligne avec la valeur NULL (ex: `SUM` sur table vide) | `DBNull.Value` |

⚠️ `DBNull` n'est **pas** `null` → il faut tester les deux.

## Récupérer la valeur proprement
```csharp
// 1. Cast direct (quand on est sûr du type et que ce n'est jamais null)
int nombre = (int)resultat!;           // ! = "je gère le null, pas d'avertissement"

// 2. TryParse (convertit sans planter)
bool ok = decimal.TryParse(resultat.ToString(), out decimal somme);

// 3. Switch (gère tous les cas d'un coup) – RECOMMANDÉ
string message = resultat switch
{
    null or DBNull => "Aucun résultat",
    string marque  => $"La marque est {marque}",
    _              => "Type inattendu"
};
```

## Correspondance des types SQL → C#

| SQL | C# |
|---|---|
| `INT`, `COUNT(*)` | `int` |
| `DECIMAL`, `SUM(DECIMAL)` | `decimal` |
| `NVARCHAR`, `VARCHAR` | `string` |
| `BIT` | `bool` |
| `SCOPE_IDENTITY()` | `decimal` (→ faire un `CAST(... AS INT)` en SQL) |

## À retenir
1. ExecuteScalar = **une seule valeur** (1re colonne, 1re ligne).
2. Renvoie un `object?` → toujours caster / convertir.
3. Toujours gérer `null` **et** `DBNull`.
4. Plusieurs lignes ou colonnes à lire ? → pas ExecuteScalar mais `ExecuteReader` (Demo04).
