using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;
using System.Collections;
using TouchScript;



public class Disturbance : InputSource
{
    static public bool disturbanceWindowOpen = false;
    public Text counter;
    public Text disturbanceText;
    static public string islandChosen = string.Empty;
    static public bool actionMade = false;  //not to repeat the final action as many times as there are scripts attached to objects
    public TypeDisturbance disturbanceType;


    void OnMouseDownSimulation()
    {
        Disturbance.islandChosen = this.name;
        finalAction();
    }


    void Awake()
    {
        this.counter = GameObject.Find("DisturbanceCounter").GetComponent<Text>();
        this.disturbanceText = GameObject.Find("DisturbanceText").GetComponent<Text>();
        StartCoroutine(counterDecrement());
    }

    IEnumerator counterDecrement()
    {
        for (int i = 10; i >= 0; i--)
        {
            if (Disturbance.islandChosen == string.Empty)
            {
                this.counter.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            else
            {
                this.counter.text = string.Empty;
                break;
            }
        }

        if (!Disturbance.actionMade)
        {
            finalAction();
        }
    }

    void finalAction()
    {
        Disturbance.actionMade = true;
        for (int i = 1; i <= 4; i++)
            GameObject.Find("Disturbance-sous_ile_" + i.ToString()).GetComponent<BoxCollider>().enabled = false;

        if (Disturbance.islandChosen == string.Empty)
        {
            this.disturbanceText.text = "Vous n'avez choisi aucune île !\nEn conséquence, la perturbation\ns'abattra sur toutes les îles !";
        }
        else
        {
            Debug.Log(Disturbance.islandChosen);
            string island = Disturbance.islandChosen.Split('-')[1];
            for (int i = 1; i <= 4; i++)
            {
                if (("Disturbance-sous_ile_" + i.ToString()) != Disturbance.islandChosen)
                {
                    GameObject.Find("Disturbance-sous_ile_" + i.ToString()).SetActive(false);
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //-------------- TUIO -----------------------------------------------------------------------

    public int Width = 512;
    public int Height = 512;
    float TouchTime;

    private MetaGesture gesture;
    private Dictionary<int, int> map = new Dictionary<int, int>();

    public override void CancelTouch(TouchPoint touch, bool @return)
    {
        base.CancelTouch(touch, @return);

        map.Remove(touch.Id);
        if (@return)
        {
            TouchHit hit;
            if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
            map.Add(touch.Id, beginTouch(processCoords(hit.RaycastHit.textureCoord), touch.Tags).Id);
        }
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        gesture = GetComponent<MetaGesture>();
        if (gesture)
        {
            gesture.TouchBegan += touchBeganHandler;
            gesture.TouchMoved += touchMovedhandler;
            gesture.TouchCancelled += touchCancelledhandler;
            gesture.TouchEnded += touchEndedHandler;
        }
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (gesture)
        {
            gesture.TouchBegan -= touchBeganHandler;
            gesture.TouchMoved -= touchMovedhandler;
            gesture.TouchCancelled -= touchCancelledhandler;
            gesture.TouchEnded -= touchEndedHandler;
        }
    }

    private Vector2 processCoords(Vector2 value)
    {
        return new Vector2(value.x * Width, value.y * Height);
    }

    private void touchBeganHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        map.Add(touch.Id, beginTouch(processCoords(touch.Hit.RaycastHit.textureCoord), touch.Tags).Id);
        //this.OnMouseDownSimulation();
        TouchTime = Time.time;
    }

    private void touchMovedhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        TouchHit hit;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        if (!gesture.GetTargetHitResult(touch.Position, out hit)) return;
        moveTouch(id, processCoords(hit.RaycastHit.textureCoord));
    }

    private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        endTouch(id);
        if (Time.time - TouchTime < 1)
            this.OnMouseDownSimulation();
    }

    private void touchCancelledhandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
    {
        int id;
        var touch = metaGestureEventArgs.Touch;
        if (touch.InputSource == this) return;
        if (!map.TryGetValue(touch.Id, out id)) return;
        cancelTouch(id);
    }

}

