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
        Debug.Log("Clic on " + this.name);
    }

    void Update()
    {
        if(this.toBeActivated)
        {
            if (!this.active)
            {
                this.active = true;
                switch (this.trophyName)
                {
                    case "Medal1":
                    case "Medal2":
                    case "Medal3":
                        transform.localScale = new Vector3(5.6f, 5f, 1);
                        break;
                    case "Trophy1":
                    case "Trophy2":
                    case "Trophy3":
                        transform.localScale = new Vector3(3.2f, 3.8f, 1);
                        break;
                    case "AirportMedal":
                        transform.localScale = new Vector3(7f, 8f, 1);
                        break;
                }
            }
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