using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {

    private GlobalResourceManager resourceManager;

    void Awake()
    {
        this.resourceManager = gameObject.GetComponent<GlobalResourceManager>();
        //this.resourceManager.ChangeResourceStockEvent += ResourceManager_ChangeResourceStockEvent;
    }
    //private void ResourceManager_ChangeResourceStockEvent(object sender, ChangeResourceStockEventArgs e)
    //{
    //    Debug.Log("Try to update interface");
    //    GameObject.Find(e.resourceType.ToString()).GetComponent<RectTransform>().transform.FindChild("Value").gameObject.GetComponent<Text>().text = e.stock.ToString();
    //}
    void Update()
    {
        foreach(Resource res in this.resourceManager.Resources)
        {
            GameObject.Find(res.TypeResource.ToString()).GetComponent<RectTransform>().transform.FindChild("Value").gameObject.GetComponent<Text>().text = res.Stock.ToString();
        }
    }
}
