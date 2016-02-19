using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building
{
    public string name;
    public string ressourceNeeded;
    public int consumptionCost;     //quantity consumed every 10 seconds
    public string ressourceProduced;
    public int productionCost;      //quantity produced every 10 seconds
    public int state; //0 : being built, 1 : built, 2 : closed
    public string imageBeingBuilt;
    public string imageBuilt;
    public int indexCanvas;
    public int coordX;
    public int coordY;
    public int constructionTime;    //time after which the building'state becomes 1


    public Building(string argName, int x, int y)     //TODO : finir switch
    {
        name = argName;
        switch (argName)
        {
            case "scierie":
                ressourceNeeded = "or";
                consumptionCost = 3;
                ressourceProduced = "bois";
                productionCost = 4;
                constructionTime = 5;
                break;
            case "mine":
                ressourceNeeded = null;
                consumptionCost = 0;
                ressourceProduced = "or";
                productionCost = 0;
                constructionTime = 0;
                break;
            case "usine":
                ressourceNeeded = null;
                consumptionCost = 0;
                ressourceProduced = "metal";
                productionCost = 0;
                constructionTime = 0;
                break;
            case "ferme":
                ressourceNeeded = null;
                consumptionCost = 0;
                ressourceProduced = "metal";
                productionCost = 0;
                constructionTime = 0;
                break;
        }
        coordX = x;
        coordY = y;
        imageBeingBuilt = @"c:\tempConcours\building-beingbuilt-" + argName + ".png";
        imageBuilt = @"c:\tempConcours\building-built-" + argName + ".png";


    }

    public async Task<bool> build(int time, int x, int y, Canvas canvas)
    {
        state = 0;
        System.Diagnostics.Debug.WriteLine("La construction du batiment " + name + " a commencé !");

        //creation of the image of the building in construction
        Image image = new Image
        {
            Width = 50,
            Height = 50,
            Name = this.name,
            Source = new BitmapImage(new Uri(imageBeingBuilt, UriKind.Absolute)),
        };
        Canvas.SetTop(image, y);
        Canvas.SetLeft(image, x);
        indexCanvas = canvas.Children.Add(image);

        await Task.Delay(TimeSpan.FromSeconds(time));   //construction of the building
        state = 1;
        System.Diagnostics.Debug.WriteLine("Le batiment " + name + " est construit !");

        //modification of the image: the building is now built
        image.Source = new BitmapImage(new Uri(imageBuilt, UriKind.Absolute));
        return true;

    }


    //consume the ressourceNeedded and produce the ressourceProduced (only if there was still a stock of ressourceNeedded)
    //method called by an Island, each 10 seconds while the state of the building is 1
    public void consume_produce(Island island)
    {
        RessourceManager rm = new RessourceManager();
        //checks if the ressourceNeeded exists and if there is enough of its stock
        if ((island.getRessource(ressourceNeeded) != null) && (island.getRessource(ressourceNeeded).stock >= consumptionCost))
        {
            rm.withdrawRessource(ressourceNeeded, island, consumptionCost); //consumption
            rm.giveRessource(ressourceProduced, island, productionCost);    //production
            System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " utilise " + this.consumptionCost.ToString() + " " + this.ressourceNeeded + " et produit " + this.productionCost.ToString() + " " + this.ressourceProduced + " sur l'ile " + island.id.ToString());
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " ne peut plus produire de " + this.ressourceProduced + " car il ne reste pas assez de " + ressourceNeeded + " sur l'ile " + island.id.ToString());
        }
    }

    public bool increaseProduction(int quantity)
    {
        productionCost += quantity;
        System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " augmente sa production de " + quantity.ToString());
        return true;
    }

    public bool increaseConsumption(int quantity)
    {
        consumptionCost += quantity;
        System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " augmente sa consommation de " + quantity.ToString());
        return true;

    }

    public bool decreaseProduction(int quantity)
    {
        if (productionCost == 0)
        {
            System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " a deja un taux de production nul");
            return false;
        }
        else
        {
            productionCost -= quantity;
            if (productionCost < 0)
                productionCost = 0;
            System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " a maintenant un taux de production de " + productionCost.ToString());
            return true;
        }
    }

    public bool decreaseConsumption(int quantity)
    {
        if (consumptionCost == 0)
        {
            System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " a deja un taux de production nul");
            return false;
        }
        else
        {
            consumptionCost -= quantity;
            if (consumptionCost < 0)
                consumptionCost = 0;
            System.Diagnostics.Debug.WriteLine("Le batiment " + this.name + " a maintenant un taux de consommation de " + productionCost.ToString());
            return true;
        }
    }

    public void moveBuilding(int x, int y)
    {
        this.coordX = x;
        this.coordY = y;
    }

}
}
