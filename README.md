# KataAccountCreation
## Description
Exercice technique de création de compte utilisateur.

### Ennoncé
* Un endpoint d’API REST en JSON pour s'inscrire
* L'utilisateur envoie un pseudo de 3 lettres exactement (A à Z, tout en majuscule. Pas de chiffre), ni plus, ni moins pour l’instant.
* Si le pseudo est libre, on l'enregistre, et on répond à l'utilisateur avec le pseudo qu'il vient d’enregistrer.
* S'il n'est pas libre
    * on en trouve un libre, avec les mêmes règles que précédemment (3 lettres exactement, A à Z, tout en majuscule, pas de chiffre)
    * on l'enregistre,
    * on renvoie à l’utilisateur ce pseudo en question.
RQ: Le pseudo libre que l'app choisit pour l'utilisateur dans ce cas n'a pas besoin de ressembler de près ou de loin au pseudo
choisi par l'utilisateur.


## How to run it
* Avoir le .net 6 SDK installé sur la machine
* Ouvrir une console sur le répertoire AccountManagementApi
* Lancer la commande dotnet build
* Lancer la commande dotnet run
* Ouvrir l'uri : https://localhost:7191/swagger/index.html

## Choix techniques
L'exercice a été réalisé en utilisant .NET 6.

J'ai mis en place la librairie MediatR pour envoyer les demandes reçues du controller en Command/Request et avoir une approche CQS

La solution est dans une optique clean-architecture, et découpée en Application/Domain/Infrastructure

La résolution a été faite avec une approche TDD, qui se concentre sur le CommandHandler présent dans la couche Application

### Algo mis en place pour la génération d'un UserName unique
La solution mise en place va à la première utilisation générer une liste (de façon lazy) de toutes les combinaisons possibles. Les utilisations suivantes utiliseront cette liste déjà générée.
L'algo nécessite d'avoir en paramètre d'entrée tout les pseudos déjà utilisés.
Création d'une nouvelle liste à partir de tout les choix possible moins ceux déjà utilisé.
Retour aléatoire d'un des éléments de la liste des UserName libre.
Etant donné le nombre limité de combinaisons (26 * 26 * 26 = 17576), récupérer au max ce nombre d'UserName d'une couche persistance n'est pas trop couteux.
Cette solution serait à remettre en question si le nombre de combinaisons augmente.

#### Complexité BigO

* Algo : 
    * Création d'une liste de UserName libre: O(N)
    * Choix d'un random : O(1)

* Space:  O(N)


## DDD Trilemma
Domain purity vs Domain completness vs Performance.
Dans ce cas j'ai choisis de respecter Domain purity et performance.
L'exercice aurait pu vu sa résolution être en Domain purity et Domain completness, mais je n'ai pas choisi cette approche, afin que ceux ayant l'habitude du DDD trilemma reconnaissent plus vite le code et puissent plus facilement l'utiliser.


## Persistance
Afin de faciliter la réalisation de l'exercice, une persistance InMemory a été choisie.
On peut très facilement changer la persistance étant donné que toutes la logique est dans la couche Domain. Il serait donc trivial d'avoir une base relationnelle qui stocke ces Account, et de l'accéder directement soit avec du Raw SQL dans les repository, soit à l'aide d'un ORM qui aurait un mapping très simple.

## Pour allez plus loin
L'accès concurrent n'a pas été pris en compte, au cas où 2 UserLogin identique seraient demandé simultanément. Une résolution simple de ce problème pourrait être une gestion d'exception qui ferait un Retry (et récupérerait donc un UserName aléatoire différent).
Pourrait être fait avec un autre Behavior MediatR

Le monitoring mis en place est minimal. Il faudrait donc prévoir de configurer ILogger afin qu'il déverse dans divers sink, et avoir une gestion de log un peu plus poussée