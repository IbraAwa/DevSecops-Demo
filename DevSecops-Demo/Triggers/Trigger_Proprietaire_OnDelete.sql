CREATE TRIGGER [dbo].[Trigger_Proprietaire_OnDelete]
    ON [dbo].[Proprietaire]
    INSTEAD OF DELETE
    AS
    BEGIN
        SET NoCount ON -- Le nombre de row renvoyé par trigger est pas compté
        UPDATE [Proprietaire] 
            SET Actif = 0 
            WHERE [Id] IN (SELECT [Id] FROM DELETED)
        -- DELETED est une table invisibile crée quand vous deleter une ligne
        -- il recupère tout les Id delete et les mets dans la table deleted
        -- qu'on récupère pour faire notre condition
    END