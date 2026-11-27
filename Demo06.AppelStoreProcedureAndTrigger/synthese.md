# Synthèse Demo06 – Procédures stockées et Triggers

## Procédure stockée = une fonction dans la DB
- Du SQL enregistré dans la base, qu'on appelle par son **nom** avec des **paramètres**.
- Avantages : logique centralisée, réutilisable, paramètres typés (pas d'injection SQL).

## Appeler une procédure en 4 étapes
```csharp
// 1. Dire que c'est une procédure (sinon le texte est vu comme une requête SQL)
command.CommandType = CommandType.StoredProcedure;

// 2. Juste le NOM : pas de EXEC, pas de parenthèses, pas de paramètres
command.CommandText = "AddProprietaire";

// 3. Les paramètres : MÊME NOM que dans la procédure, l'ordre ne compte pas
command.Parameters.Add("@Prenom", SqlDbType.NVarChar, 100).Value = "Jean";

// 4. Choisir le bon Execute selon ce que renvoie la procédure
```
- Paramètre manquant → `SqlException "expects parameter '@Prenom', which was not supplied"`.
- Toujours donner la **taille** (`NVarChar, 100`) / **précision** (`Decimal` : `Precision`, `Scale`)
  comme dans la procédure, sinon SQL Server garde plusieurs plans d'exécution en cache.

## SELECT ou RETURN ? → pas le même Execute

| Dans la procédure | C'est quoi | En C# |
|---|---|---|
| `SELECT CAST(SCOPE_IDENTITY() AS INT)` | un **résultat** (1 ligne) | `ExecuteScalar` |
| `RETURN @@ROWCOUNT` | un **code retour** (toujours INT) | paramètre `ReturnValue` |
| plusieurs lignes `SELECT ...` | un résultat | `ExecuteReader` |

### Récupérer un SELECT (AddProprietaire)
```csharp
object? id = await command.ExecuteScalarAsync(cancellationToken);
if (id is int nouvelId) { ... }
```
- `SCOPE_IDENTITY()` = dernier Id généré par **notre** procédure, renvoyé en `decimal` → d'où le `CAST AS INT`.

### Récupérer un RETURN (UpdateVoiturePrix)
```csharp
SqlParameter retour = command.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
retour.Direction = ParameterDirection.ReturnValue;

await command.ExecuteNonQueryAsync(cancellationToken); // renvoie -1 (SET NOCOUNT ON)
int lignesModifiees = (int)retour.Value;               // rempli APRÈS l'exécution
```
- Un `RETURN` n'est **pas** un résultat → `ExecuteScalar` renverrait `null`.

## `SET NOCOUNT ON`
- Coupe le message "(1 row affected)" envoyé après chaque instruction.
- Conséquence : `ExecuteNonQuery` renvoie **-1** → si on veut le nombre de lignes, on le renvoie
  nous-mêmes (`RETURN @@ROWCOUNT` ou `SELECT @@ROWCOUNT`).

## Trigger = se déclenche tout seul
- Code SQL lié à une **table** et un **événement** (INSERT, UPDATE, DELETE).
- On ne l'appelle **jamais** en C# : pas de `StoredProcedure`, pas de nom de trigger.
- On écrit une requête **normale**, SQL Server déclenche le trigger.

```sql
CREATE TRIGGER Trigger_Proprietaire_OnDelete ON Proprietaire
INSTEAD OF DELETE AS
BEGIN
    SET NOCOUNT ON
    UPDATE Proprietaire SET Actif = 0 WHERE Id IN (SELECT Id FROM DELETED)
END
```
- `INSTEAD OF` = **à la place** de : le DELETE n'a jamais lieu → **suppression logique** (soft delete).
- `DELETED` = table invisible avec les lignes visées par le DELETE.

```csharp
command.CommandText = "DELETE FROM Proprietaire WHERE Id = @Id";
int ligne = await command.ExecuteNonQueryAsync(cancellationToken); // renvoie 1
```
⚠️ **Piège** : renvoie `1` alors que rien n'est supprimé. ExecuteNonQuery compte les lignes
**visées** par le DELETE, pas celles qui ont disparu. Vérifier avec un `SELECT Actif`.
- Sans `SET NOCOUNT ON` dans le trigger, son UPDATE serait ajouté → `2` au lieu de `1`.

## À retenir
1. Procédure → `CommandType.StoredProcedure` + le nom + les paramètres au même nom.
2. `SELECT` → ExecuteScalar / Reader. `RETURN` → paramètre `ReturnValue`.
3. `SET NOCOUNT ON` → ExecuteNonQuery renvoie `-1`.
4. Un trigger ne s'appelle pas : il se déclenche sur une requête normale.
5. `INSTEAD OF DELETE` → la ligne reste, et ExecuteNonQuery ment un peu.
