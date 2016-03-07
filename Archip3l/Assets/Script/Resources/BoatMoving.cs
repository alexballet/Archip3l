using UnityEngine;
using System.Collections.Generic;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections;

namespace TouchScript.Examples.Cube
{
    public class BoatMoving : InputSource
    {

        public MinorIsland island;
        public string islandToSend;
        public int quantityCarried;
        public string resourceSent;
        public bool appeared = false;
        public bool collided = false;



        void collision()
        {
            this.collided = true;
            StartCoroutine(startBoatDisappearance());
            MinorIsland.exchangePerforming = false;
        }


        void Start()
        {
            StartCoroutine(startBoatAppearance());
        }

        public IEnumerator startBoatAppearance()
        {
            Color color;
            color = this.GetComponent<SpriteRenderer>().color;
            color.a = 0;
            this.GetComponent<SpriteRenderer>().color = color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.03f);
                color = this.GetComponent<SpriteRenderer>().color;
                color.a += 0.01f;
                this.GetComponent<SpriteRenderer>().color = color;
            }

            this.appeared = true;
        }

        public IEnumerator startBoatDisappearance()
        {
            Color color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.03f);
                color = this.GetComponent<SpriteRenderer>().color;
                color.a -= 0.01f;
                this.GetComponent<SpriteRenderer>().color = color;
            }
            Destroy(this.gameObject);
        }


        void Update()
        {
            if (this.GetComponent<BoxCollider>().bounds.Intersects(GameObject.Find("sous_ile_2").GetComponent<MeshCollider>().bounds))
            {
                Debug.Log("collided island");
            }
            if (!this.collided && this.GetComponent<BoxCollider>().bounds.Intersects(GameObject.Find(islandToSend + "_Harbor").GetComponent<BoxCollider>().bounds))
                collision();
            for (int i = 1; i <= 4; i++) 
                if (this.GetComponent<BoxCollider>().bounds.Intersects(GameObject.Find("sous_ile_" + i.ToString()).GetComponent<MeshCollider>().bounds))
                    {
                        Debug.Log("collided island");
                    }
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
            if (TouchTime == 0)
            {
                TouchTime = Time.time;
            }

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
            if (this.appeared && !this.collided)
            {
                Vector3 positionTouched = Camera.main.ScreenToWorldPoint(touch.Position);
                positionTouched.z = -1;
                this.transform.position = positionTouched;
            }
        }

        private void touchEndedHandler(object sender, MetaGestureEventArgs metaGestureEventArgs)
        {
            int id;
            var touch = metaGestureEventArgs.Touch;
            if (touch.InputSource == this) return;
            if (!map.TryGetValue(touch.Id, out id)) return;
            endTouch(id);
            if (Time.time - TouchTime < 0.5)
            {
                TouchTime = 0;
                //this.OnMouseDownSimulation();
            }
            else if (Time.time - TouchTime < 1.5)
            {
                TouchTime = 0;
            }
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
