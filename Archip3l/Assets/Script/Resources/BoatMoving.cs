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
        public Vector3 startPosition;

        private GameObject harbor;

        private float x1, y1;

        void OnTriggerEnter(Collider col)
        {
            Debug.Log(col.name);
            if (col.name == islandToSend + "_Harbor")
            {
                MinorIsland islandReceiver = GameObject.Find(islandToSend).GetComponent<MinorIsland>();
                this.collided = true;
                Debug.Log("collided with harbor");
                islandReceiver.displayPopup("Vous venez de recevoir une cargaison de " + this.quantityCarried.ToString() + " " + Resource.translateResourceName(resourceSent) + " !", 3);
                StartCoroutine(startBoatDisappearance());
                MinorIsland.exchangePerforming = false;
                //add resources to islandToSend
                TypeResource res = (TypeResource)System.Enum.Parse(typeof(TypeResource), resourceSent);
                islandReceiver.resourceManager.getResource(res).changeStock(this.quantityCarried);
            }
            else
            {
                if (this.quantityCarried / 2 == 0)
                {
                    island.displayPopup("Suite aux dommages subis, vous bâteau coule, ainsi que toutes les ressources transportées ...", 3);
                    StartCoroutine(startBoatDisappearance());
                }
                else {
                    StartCoroutine(resetPosition());
                    island.displayPopup("Attention ! Vous venez de subir une collision, vous perdez donc la moitié des ressources à transmettre", 3);
                    this.quantityCarried /= 2;
                }
            }
        }

        public IEnumerator resetPosition()
        {
            this.GetComponent<BoxCollider>().enabled = false;
            Color color;
            color = this.GetComponent<SpriteRenderer>().color;
            this.GetComponent<SpriteRenderer>().color = color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.01f);
                color = this.GetComponent<SpriteRenderer>().color;
                color.a -= 0.01f;
                this.GetComponent<SpriteRenderer>().color = color;
            }
            this.transform.position = startPosition;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.01f);
                color = this.GetComponent<SpriteRenderer>().color;
                color.a += 0.01f;
                this.GetComponent<SpriteRenderer>().color = color;
            }
            this.GetComponent<BoxCollider>().enabled = true;
        }


        void Start()
        {
            this.island = this.transform.parent.GetComponent<MinorIsland>();
            this.startPosition = this.transform.position;
            this.harbor = GameObject.Find(this.islandToSend + "_Harbor");
            StartCoroutine(startBoatAppearance());
            SpriteRenderer cyclonePrefab = Resources.Load<SpriteRenderer>("Prefab/cyclone");
            SpriteRenderer cyclone = Instantiate(cyclonePrefab);
            cyclone.name = "cyclone";
            StartCoroutine(spine(cyclone));
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

        public IEnumerator spine(SpriteRenderer cyclone)
        {
            while(!this.collided)
            {
                yield return new WaitForSeconds(0.001f);
                cyclone.transform.Rotate(Vector3.back, 2);
            }
            Destroy(GameObject.Find("cyclone"));
        }


        void FixedUpdate()
        {
            x1 = harbor.GetComponent<BoxCollider>().bounds.center.x;
            y1 = harbor.GetComponent<BoxCollider>().bounds.center.y;

            //Debug.Log (point.transform.position);
            float alpha = 90 - (Mathf.Rad2Deg * Mathf.Atan2(y1 - transform.position.y, x1 - transform.position.x));
            transform.rotation = Quaternion.Euler(0f, 0f, -alpha);

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
                positionTouched.z = 0;
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
