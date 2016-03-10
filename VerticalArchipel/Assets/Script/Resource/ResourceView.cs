using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResourceView : MonoBehaviour {

    private GlobalResourceManager resourceManager;

    void Awake()
    {
        this.resourceManager = gameObject.GetComponent<GlobalResourceManager>();
    }
    void FixedUpdate()
    {
        foreach(Resource res in this.resourceManager.Resources)
        {
            GameObject.Find(res.TypeResource.ToString()).GetComponent<RectTransform>().transform.FindChild("Value").gameObject.GetComponent<Text>().text = res.Stock.ToString();
        }
    }
}
