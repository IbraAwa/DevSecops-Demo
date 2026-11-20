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

        // Un restaurant engage un cuisinier : en C# (thread)
        // (Thread) : C#

        // Quand on est en synchrone
        // quand on va demander au cuisinier de préparer des pâtes
        // il va préparer la casserole, bouillir l'eau
        // il va attendre devant la casserole que l'eau bout
        // et pour chaque étape il va attendre que l'étape finisse

        // CONSÉQUENCE : ça va prendre du temps !

        // Engager plusieurs cuisiniers (plusieurs threads)
        // un cuisinier = un thread

        // le problème d'engager plusieurs cuisiniers c'est que c'est cher
        // un thread c'est cher car il demande une lecture écriture etc...
        // (chaque thread réserve sa propre mémoire, environ 1 Mo)

        // donc si vous avez 10 000 utilisateurs il y aura 10 000 threads donc votre
        // application va ralentir

        // on va former notre cuisinier
        // pour qu'il fasse de l'asynchrone

        //il va mettre l'eau à bouillir et pendant ce temps il va préparer la sauce tomate
        // quand l'eau commence à bouillir il revient à la casserole et met les pâtes

        // il optimise donc il va plus vite


        // l'asynchrone ce n'est pas attendre qu'une étape finisse pour continuer à faire des TASK

        // souvent l'asynchrone est utilisé quand on appelle des ressources externes (DB, fichier, disque etc.)
        // pourquoi ? Car c'est ce qui prend le + de temps
        // et donc on doit surtout pas attendre
    }
}
