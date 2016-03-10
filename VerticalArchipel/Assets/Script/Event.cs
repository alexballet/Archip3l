using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using UnityEngine.UI;
using System.Collections;

namespace TouchScript.Examples.Cube
{


    public class Event : InputSource
    {
        static public bool eventWindowOpen = false;
        public Text counter;
        public Text eventText;
        static public string islandChosen = string.Empty;
        static public bool actionMade = false;  //not to repeat the final action as many times as there are scripts attached to objects
        public string eventType;


        void OnMouseDownSimulation()
        {
            Event.islandChosen = this.name;
        }


        void Awake()
        {
            this.counter = GameObject.Find("EventCounter").GetComponent<Text>();
            this.eventText = GameObject.Find("EventText").GetComponent<Text>();
            StartCoroutine(counterDecrement());
        }

        IEnumerator counterDecrement()
        {
            for (int i = 10; i >= 0; i--)
            {
                if (Event.islandChosen == string.Empty)
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

            if (!Event.actionMade)
            {
                Event.actionMade = true;
                finalAction();
            }
        }

        void finalAction()
        {
            for (int i = 1; i <= 4; i++)
                GameObject.Find("Event-sous_ile_" + i.ToString()).GetComponent<BoxCollider>().enabled = false;

            if (Event.islandChosen == string.Empty)
            {
                this.eventText.text = "Vous n'avez choisi aucune île !\nEn conséquence, la " + this.eventType + "\ns'abattra sur toutes les îles !";
            }
            else
            {
                Debug.Log(Event.islandChosen);
                string island = Event.islandChosen.Split('-')[1];
                for (int i = 1; i <= 4; i++)
                {
                    if (("Event-sous_ile_" + i.ToString()) != Event.islandChosen)
                    {
                        GameObject.Find("Event-sous_ile_" + i.ToString()).SetActive(false);
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
}
