using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;

namespace TouchScript.Examples.Cube
{

    //this class concerns the thophies + the medals + AirportMedal
    public class Trophy : InputSource
    {

        static public bool infoWindowPresent = false;

        public bool active = false;     //true if trophy unlocked
        public bool toBeActivated = false;
        public string trophyName;
        public string description;

        public Sprite wonSprite;

        void Awake()
        {
            this.trophyName = name;
        }

        void OnMouseDownSimulation()
        {
            if (!Island.infoIslandPresent)
            {

                if (!Trophy.infoWindowPresent)
                {
                    Trophy.infoWindowPresent = true;
                    Debug.Log("Clic on " + this.name);
                    switch (this.name)
                    {
                        case "AirportMedal":
                            Canvas infoAirportCanvasPrefab = Resources.Load<Canvas>("Prefab/infoAirportCanvas");
                            Canvas infoAirportCanvas = Instantiate(infoAirportCanvasPrefab);
                            infoAirportCanvas.name = "infoAirportCanvas";
                            break;
                        case "MedalsCollider":
                            Canvas infoMedalsCanvasPrefab = Resources.Load<Canvas>("Prefab/infoMedalsCanvas");
                            Canvas infoMedalsCanvas = Instantiate(infoMedalsCanvasPrefab);
                            infoMedalsCanvas.name = "infoMedalsCanvas";
                            break;
                        case "TrophiesCollider":
                            Canvas infoTrophiesCanvasPrefab = Resources.Load<Canvas>("Prefab/infoTrophiesCanvas");
                            Canvas infoTrophiesCanvas = Instantiate(infoTrophiesCanvasPrefab);
                            infoTrophiesCanvas.name = "infoTrophiesCanvas";
                            break;
                    }
                }
                else if (this.name == "close")
                {
                    Trophy.infoWindowPresent = false;
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                }
            }
        }

        void Update()
        {
            if(this.toBeActivated)
            {
                
                gameObject.GetComponent<SpriteRenderer>().sprite = wonSprite;
            }
        }

        public bool changeToObtained()
        {
            this.toBeActivated = true;
            return true;
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