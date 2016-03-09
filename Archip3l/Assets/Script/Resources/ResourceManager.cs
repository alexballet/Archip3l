using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TouchScript.InputSources
{

    public class ResourceManager : MonoBehaviour
    {

        public MinorIsland minorIsland { get; private set; }
        public List<Resource> Resources;

        public void init(MinorIsland island)
        {
            this.minorIsland = island;
            this.Resources = new List<Resource>();

            //Add all resources
            foreach (TypeResource resourceType in Enum.GetValues(typeof(TypeResource)))
            {
                //this.addResource(resourceType, 0, 0);

                //TESTS
                this.addResource(resourceType, 0, 0);
            }


            switch (this.minorIsland.nameMinorIsland)
            {
                case "sous_ile_1":
                    this.changeResourceStock(TypeResource.Gold, 10);
                    this.changeResourceStock(TypeResource.Stone, 5);
                    break;
                case "sous_ile_2":
                    this.changeResourceStock(TypeResource.Gold, 5);
                    this.changeResourceStock(TypeResource.Stone, 10);
                    this.changeResourceStock(TypeResource.Oil, 20);
                    this.changeResourceStock(TypeResource.Wood, 10);
                    this.changeResourceStock(TypeResource.Food, 5);
                    break;
                case "sous_ile_3":
                    this.changeResourceStock(TypeResource.Gold, 5);
                    this.changeResourceStock(TypeResource.Stone, 10);
                    this.changeResourceStock(TypeResource.Food, 5);
                    break;
                case "sous_ile_4":
                    this.changeResourceStock(TypeResource.Stone, 5);
                    this.changeResourceStock(TypeResource.Wood, 10);
                    break;
            }


        }
        void Start()
        {
            StartCoroutine("updateStocks");
        }

        public bool addResource(TypeResource resourceType, int quantity, int production)
        {
            bool flag = false;
            foreach (Resource item in this.Resources)
            {
                if (item.TypeResource == resourceType)
                {
                    flag = true;
                }
            }
            if (flag == true)
            {
                return false;
            }
            else
            {
                //To be deleted
                //Resource object is unneeded

                //GameObject myGameObject = new GameObject();
                //myGameObject.name = name + "_" + minorIsland.nameMinorIsland;
                //myGameObject.transform.SetParent(GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform);
                Resource res = ScriptableObject.CreateInstance<Resource>();
                res.init(resourceType, quantity, production);
                this.Resources.Add(res);
                return true;
            }
        }
        public bool changeResourceProduction(TypeResource resourceType, int value)
        {
            Resource resource = this.getResource(resourceType);
            bool result = resource.changeProduction(value);
            return result;
        }
        public bool changeResourceStock(TypeResource resourceType, int value)
        {
            Resource resource = this.getResource(resourceType);
            bool result = false;
            if (resource != null)
            {
                result = resource.changeStock(value);
            }
            else
            {
                if (value >= 0)
                {
                    this.addResource(resourceType, value, 0);
                    result = true;
                }
                else
                {
                    return false;
                }

            }
            return result;
        }
        public bool checkWithdrawPossibility(TypeResource resourceType, int value)
        {
            return this.getResource(resourceType).checkChangeStockPossibility(value);
        }
        public bool donateResource(MinorIsland remoteIsland, TypeResource typeResource, int quantity)
        {
            if (checkWithdrawPossibility(typeResource, -quantity))
            {
                changeResourceStock(typeResource, -quantity);
                remoteIsland.resourceManager.changeResourceStock(typeResource, quantity);
                return true;
            }
            else
            {
                return false;
            }

        }
        public Resource getResource(TypeResource resourceType)
        {
            foreach (Resource item in this.Resources)
            {
                if (item.TypeResource == resourceType)
                {
                    return item;
                }
            }
            return null;
        }

        IEnumerator updateStocks()
        {
            for (;;)
            {
                foreach (Resource res in this.Resources)
                {
                    res.changeStock(res.Production);
                    if (res.Production != 0)
                    {
                        //Debug.Log("Island : " + this.minorIsland + "\tProduction : " + res.Production + "\tStock  : " + res.Name + " : " + res.Stock);
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }

}