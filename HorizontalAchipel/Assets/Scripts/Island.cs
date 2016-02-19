using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island
{
    public int id;        //0 : ile pricipale ; 1 : haut gauche, 2 : haut droite, 3 : bas gauche, 4 : bas droite
    public List<Building> buildings;
    public List<Ressource> ressources;


    public Island()
    {
        buildings = new List<Building>();
        ressources = new List<Ressource>();
    }

    public Building getBuilding(string name)
    {
        int i = 0;
        while (i < buildings.Count)
        {
            if (buildings[i].name == name)
                return buildings[i];
            i++;
        }
        return null;
    }

    public Ressource getRessource(string name)
    {
        int i = 0;
        while (i < ressources.Count)
        {
            if (ressources[i].name == name)
                return ressources[i];
            i++;
        }
        return null;
    }


    //creates a building, adds it to the list and starts the consumption/production of ressources (every 10 seconds)
    //the corresponding image is added in "canvas"
    public async void createBuilding(string name, int x, int y, Canvas canvas)
    {

        //TODO : appeler une fonction "défi" --> réussite = création du bâtiment ; échec = rien

        //there can't be several buidlings of the same type ("name")
        if (getBuilding(name) != null)
        {
            System.Diagnostics.Debug.WriteLine("Le batiment " + name + " existe déjà !");
            return;
        }

        Building building = new Building(name, x, y);                   //instanciation
        buildings.Add(building);
        await building.build(building.constructionTime, x, y, canvas);  //addition of the building's image to the map (construction time)
        while (building.state == 1)     //consumption/production during the building's life
        {
            building.consume_produce(this);
            if (getRessource(building.ressourceNeeded) != null)
            {
                System.Diagnostics.Debug.WriteLine(getRessource(building.ressourceNeeded).name + " : " + getRessource(building.ressourceNeeded).stock);
                System.Diagnostics.Debug.WriteLine(getRessource(building.ressourceProduced).name + " : " + getRessource(building.ressourceProduced).stock);
            }
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
        //if state is 2 --> building removed
        canvas.Children.RemoveAt(building.indexCanvas);
        System.Diagnostics.Debug.WriteLine("Le batiment " + building.name + " a été supprimé !");
        building = null;
        return;
    }

    //give a stock of "quantity" of the ressource named "name" to "island"
    public void giveRessourceToIsland(string name, int quantity, Island island)
    {
        RessourceManager rm = new RessourceManager();
        int quantityWithdrawn = rm.withdrawRessource(name, this, quantity);
        //we give to "island" the quantity withdrawn from the current island
        if (quantityWithdrawn != 0)
        {
            rm.giveRessource(name, island, quantityWithdrawn);
            System.Diagnostics.Debug.WriteLine("L'ile " + this.id.ToString() + " donne " + quantityWithdrawn.ToString() + " " + name + " à l'ile " + island.id.ToString());
        }
        else
            System.Diagnostics.Debug.WriteLine("La ressource " + name + " n'est plus disponible sur l'ile " + island.id.ToString());
    }


}