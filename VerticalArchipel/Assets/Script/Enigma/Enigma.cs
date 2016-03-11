using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;
using System.Collections;
using TouchScript;


public class Enigma : InputSource
{
    static public bool enigmaWindowOpen = false;

    static public Text questionObject { get; private set; }
    static public Text resultObject { get; private set; }
    static public string question { get; private set; }
    static public string answer { get; private set; }
    static public string explainations { get; private set; }

    public TextAsset csv { get; private set; }


    void OnMouseDownSimulation()
    {
        for (int i = 0; i < 10; i++)
            GameObject.Find(i.ToString()).GetComponent<BoxCollider>().enabled = false;
        Enigma.questionObject.text = Enigma.explainations;
        StartCoroutine(wait( this.name == Enigma.answer));

    }

    void Awake()
    {
        if (this.name == "Result")
            Enigma.resultObject = this.GetComponent<Text>();
        if (this.name == "Question")
        {
            Enigma.questionObject = this.GetComponent<Text>();
            //CSV part
            //row[0] : question ; row[1] : answer ; row[2] : explainations

            //ENCODAGE : UTF8-16-LE
            csv = Resources.Load<TextAsset>("Enigmes/Enigma");

            string[] row = CSV_reader.GetRandomLine(csv.text);

            Enigma.question = row[0].Replace('*', '\n');
            //addition of '*' for line breaks
            //addLineBreaks();
            Enigma.answer = row[1];
            Enigma.explainations = row[2].Replace('*', '\n');

            this.GetComponent<Text>().text = Enigma.question;
        }

    }

    IEnumerator wait(bool goodAnswer)
    {
        yield return new WaitForSeconds(3);

        for (int i = 0; i < 10; i++)
            GameObject.Find(i.ToString()).SetActive(false);
        if (goodAnswer)
            Enigma.resultObject.text = "Bravo, votre réponse permet ...";
        else
            Enigma.resultObject.text = "Votre mauvaise réponse ...";

        //TODO: action of Enigma




        Enigma.enigmaWindowOpen = false;
        Destroy(GameObject.Find("Enigma"), 5);
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
