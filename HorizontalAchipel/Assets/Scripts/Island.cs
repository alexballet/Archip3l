using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Island : MonoBehaviour
{
    public int id;        //0 : ile pricipale ; 1 : haut gauche, 2 : haut droite, 3 : bas gauche, 4 : bas droite
    public List<Building> buildings;
    public List<Ressource> ressources;

    public Island CreateComponent(GameObject where)
    {
        Island island = where.AddComponent<Island>();

        island.buildings = new List<Building>();
        island.ressources = new List<Ressource>();


        return island;
    }

    public Building getBuilding(string name)
    {
        int i = 0;
        while (i < buildings.Count)
        {
            if (buildings[i].buildingName == name)
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
    public IEnumerator createBuilding(string name, float x, float y, string island)
    {
        //TODO : appeler une fonction "défi" --> réussite = création du bâtiment ; échec = rien

        //there can't be several buidlings of the same type ("name")
        if (getBuilding(name) != null)
        {
            Debug.Log("Le batiment " + name + " existe déjà !");
            yield break;
        }
        
        Building building = Building.CreateComponent(island, name, x, y);   //instanciation
        buildings.Add(building);
        yield return building.build(building.constructionTime, x, y, island);  //addition of the building's image to the map (construction time)
        while (building.state == 1)     //consumption/production during the building's life
        {
            building.consume_produce(this);
            if (getRessource(building.ressourceNeeded) != null)
            {
                Debug.Log(getRessource(building.ressourceNeeded).name + " : " + getRessource(building.ressourceNeeded).stock);
                Debug.Log(getRessource(building.ressourceProduced).name + " : " + getRessource(building.ressourceProduced).stock);
            }
            yield return wait(10);
        }
        //if state is 2 --> building removed
        Destroy(building.buldingGameObject);
        buildings.Remove(getBuilding(building.buildingName));
        Debug.Log("Le batiment " + building.buildingName + " a été supprimé !");
        building = null;
    }

    IEnumerator wait(int sec)
    {
        yield return new WaitForSeconds(sec);
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
            Debug.Log("L'ile " + this.id.ToString() + " donne " + quantityWithdrawn.ToString() + " " + name + " à l'ile " + island.id.ToString());
        }
        else
            Debug.Log("La ressource " + name + " n'est plus disponible sur l'ile " + island.id.ToString());
    }


}