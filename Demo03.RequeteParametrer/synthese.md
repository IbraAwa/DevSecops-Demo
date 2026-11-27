# Synthèse Demo03 – Requêtes paramétrées

## Le problème : l'injection SQL
```csharp
// ❌ DANGEREUX
command.CommandText = $"SELECT Marque FROM Voiture WHERE Id = '{voitureId}'";
```
- La valeur est **collée** dans la requête → la DB la lit comme du SQL.
- Si l'utilisateur tape `' OR '1' = '1' UNION SELECT ...` → il exécute **sa** requête.
- Conséquence : accès à des données confidentielles, suppression de tables...

## La solution : paramétrer
```csharp
command.CommandText = "SELECT Marque FROM Voiture WHERE Id = @voitureId";
```
- `@voitureId` = un emplacement. La valeur est envoyée **à part**, jamais comme du SQL.
- 2 règles :
  1. **Toujours** paramétrer la requête.
  2. Bien **typer** les paramètres de la méthode (`int voitureId` plutôt que `string`).

## 3 façons d'ajouter un paramètre

### 1. `AddWithValue` – rapide mais piégeux
```csharp
command.Parameters.AddWithValue("@voitureId", voitureId);
```
- Devine le type SQL à partir du type C# (`string` → `NVARCHAR`, `3.14m` → `DECIMAL(3,2)`).
- Si le type deviné ≠ type de la colonne (`VARCHAR`, `DECIMAL(15,2)`) → **l'index peut sauter**.
- Aucun bug visible... mais les performances baissent petit à petit → **bug silencieux** en prod.

### 2. `new SqlParameter` – moderne
```csharp
SqlParameter param = new SqlParameter("@voitureId", SqlDbType.Int) { Value = voitureId };
command.Parameters.Add(param);
```

### 3. `Parameters.Add(...).Value` – RECOMMANDÉ
```csharp
command.Parameters.Add("@voitureId", SqlDbType.Int).Value = voitureId;
command.Parameters.Add("@marque", SqlDbType.NVarChar, 50).Value = "BMW"; // + la taille
```
- On **choisit** le type SQL → identique à la colonne → index conservé.
- Court à écrire, une seule ligne.

## CancellationToken
```csharp
public static async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
...
await command.ExecuteScalarAsync(cancellationToken);
```
- Permet d'**annuler** une requête en cours (l'utilisateur ferme la page, timeout...).
- On le reçoit en paramètre et on le **passe à chaque méthode Async**.

## À retenir
1. **Jamais** de `$"... {variable} ..."` dans une requête SQL.
2. Toujours des `@parametres`.
3. Éviter `AddWithValue` → préférer `Parameters.Add(nom, SqlDbType).Value = ...`.
4. Le type SQL du paramètre doit être **le même** que celui de la colonne.
5. Passer le `CancellationToken` partout.
