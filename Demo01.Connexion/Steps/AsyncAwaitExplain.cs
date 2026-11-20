using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo01.Connexion.Steps;

internal static class AsyncAwaitExplain
{
    public static void Executer()
    {
        // ASYNC 
        // Asynchrone 
        // qu'est-ce que ça veut dire asynchrone ?

        // Un restaurant engage un cuisiner : en C# (thread)
        // (Thread) : C#

        // Quand on est en synchrone
        // quand on va demander au cuisinier de préparer des pates
        // il va préparer la casserole bouillir l'eau
        // il va attendre devant la casserole que l'eau boue 
        // et pour chaque étape il va attendre que l'étape finisse

        // CONSEQUENCE : ça va prendre du temps ! 

        // Engager plusieurs cuisiniers (plusieurs threads)
        // un cuisiner = un thread

        // le problème d'engager plusieurs cuisinier c'est que c'est chère
        // un thread c'est chère car il demande une lecture écriture etc...

        // donc si vous avez 10 000 utilisateur il y'aura 10 000 thread donc votre 
        // application va ralentir

        // on va former notre cuisinier
        // pour qu'il fasse de l'asynchrone

        //il va mettre l'eau à bouillir et pendant ce temps il va préparer la sauce tomate
        // quand l'eau commence à bouillir il revient à la casserole et remet les pates

        // il optimsie donc il va plus vite


        // l'asynchrone ce n'est pas attendre qu'une étape finisse pour continuer à faire des TASK

        // souvent l'asynchrone est utilisé quand on appel des ressource extene (DB fichier disque etc.)
        // pourquoi ? Car c'est ce qui prend le + de temps
        // et donc on doit surtout pas attendre
    }
}
