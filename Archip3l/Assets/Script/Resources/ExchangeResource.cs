using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExchangeResource : MonoBehaviour {

    public MinorIsland island;

    SpriteRenderer less = null;
    SpriteRenderer more = null;
    SpriteRenderer resource_sp = null;
    Text quantityValue;
    Text to;

    static public bool otherWindowOpen = false;     //choice of Island or Resource
    static public string resource = string.Empty;
    static public string islandToSend = string.Empty;

    void refresh()
    {
        foreach (SpriteRenderer sp in this.transform.parent.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sp.name == "Less")
            {
                less = sp;
            }
            if (sp.name == "More")
            {
                more = sp;
            }
            if (sp.name == "ResourceValue")
            {
                resource_sp = sp;
            }
        }
        foreach (Text text in this.transform.parent.GetComponentsInChildren<Text>())
        {
            if (text.name == "QuantityValue")
            {
                quantityValue = text;
            }
            if (text.name == "Island")
            {
                to = text;
            }
        }
    }

    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        

        foreach (SpriteRenderer sp in this.transform.parent.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sp.name == "Less")
            {
                less = sp;
            }
            if (sp.name == "More")
            {
                more = sp;
            }
            if (sp.name == "ResourceValue")
            {
                resource_sp = sp;
            }
        }
        foreach (Text text in this.transform.parent.GetComponentsInChildren<Text>())
        {
            if (text.name == "QuantityValue")
            {
                quantityValue = text;
            }
        }

        if (!ExchangeResource.otherWindowOpen)
        {
            string resourceName = Resource.getResourceFromIconName(resource_sp.GetComponent<SpriteRenderer>().sprite.name);
            int resourceQuantity = this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), resourceName)).Stock;

            switch (this.name)
            {
                case "ResourceValue":
                    Canvas listResourcesCanvasPrefab = Resources.Load<Canvas>("Prefab/ListResourcesCanvas");
                    Canvas listResourcesCanvas = Instantiate(listResourcesCanvasPrefab);
                    listResourcesCanvas.name = "listResourcesCanvas_" + island.nameMinorIsland;
                    listResourcesCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                    //TODO : set position depending on island
                    foreach(SpriteRenderer sr in listResourcesCanvas.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sr.GetComponent<BoxCollider2D>().enabled = true;
                        if (this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), sr.name)).Stock < 5)
                        {
                            sr.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + sr.name + "Icon_disabled");
                            sr.GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                    ExchangeResource.otherWindowOpen = true;
                    quantityValue.text = "0";

                    break;
                case "Less":
                    if (quantityValue.text == "0")
                        this.GetComponent<PolygonCollider2D>().enabled = false;
                    else
                    {
                        quantityValue.text = (int.Parse(quantityValue.text) - 5).ToString();
                        more.GetComponent<PolygonCollider2D>().enabled = true;
                    }
                    break;
                case "More":
                    this.GetComponent<PolygonCollider2D>().enabled = true;
                    if ((quantityValue.name == "QuantityValue") && (int.Parse(quantityValue.text) + 5 <= resourceQuantity))
                        {
                            quantityValue.text = (int.Parse(quantityValue.text) + 5).ToString();
                            less.GetComponent<PolygonCollider2D>().enabled = true;
                        }                    
                    break;
                case "Island":
                    Canvas listIslandsCanvasPrefab = Resources.Load<Canvas>("Prefab/ListeIslands");
                    Canvas listIslandsCanvas = Instantiate(listIslandsCanvasPrefab);
                    listIslandsCanvas.name = "listIslandsCanvas_" + island.nameMinorIsland;
                    listIslandsCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                    //TODO : set position depending on island
                    ExchangeResource.otherWindowOpen = true;
                    break;
                case "Close":
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                    island.exchangeWindowPresent = false;
                    break;
            }
        }

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (resource != string.Empty)
        {
            refresh();
            resource_sp.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + resource + "Icon");
            resource = string.Empty;
        }
        if (islandToSend != string.Empty)
        {
            refresh();
            to.text = "Ile " + islandToSend[islandToSend.Length - 1].ToString();
            islandToSend = string.Empty;
        }
    }
}
