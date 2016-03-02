using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TouchScript.Examples.Cube;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;

namespace TouchScript.Examples.Cube
{

    public class ExchangeResource : InputSource
    {

        public MinorIsland island;

        SpriteRenderer less = null;
        SpriteRenderer more = null;
        SpriteRenderer resource_sp = null;
        Text quantityValue;
        Text to;



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

        void OnMouseDownSimulation()
        {

            refresh();

            if (!island.otherWindowOpen)
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
                        Vector3 vector3 = GameObject.Find(island.nameMinorIsland).transform.position;
                        vector3.z = -3;
                        listResourcesCanvas.transform.position = vector3;
                        foreach (SpriteRenderer sr in listResourcesCanvas.GetComponentsInChildren<SpriteRenderer>())
                        {
                            sr.GetComponent<BoxCollider2D>().enabled = true;
                            if ((sr.name != "Close") && (this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), sr.name)).Stock < 5))
                            {
                                sr.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + sr.name + "Icon_disabled");
                                sr.GetComponent<BoxCollider2D>().enabled = false;
                            }
                        }
                        island.otherWindowOpen = true;
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
                        Canvas listIslandsCanvasPrefab = Resources.Load<Canvas>("Prefab/ListeIslandsCanvas");
                        Canvas listIslandsCanvas = Instantiate(listIslandsCanvasPrefab);
                        listIslandsCanvas.name = "listIslandsCanvas_" + island.nameMinorIsland;
                        listIslandsCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                        Vector3 vect3 = GameObject.Find(island.nameMinorIsland).transform.position;
                        vect3.z = -3;
                        listIslandsCanvas.transform.position = vect3;
                        island.otherWindowOpen = true;
                        break;
                    case "Close":
                        Destroy(GameObject.Find(this.transform.parent.parent.name));
                        island.exchangeWindowPresent = false;
                        break;
                    case "Send":
                        if (to.text == "Ile X")
                        {
                            StartCoroutine(island.destroyPopup(island.createPopup("Veuillez sélectionner une île, en appuyant sur \"Ile X\"."), 3));
                        }
                        else
                        {
                            if (quantityValue.text == "0")
                            {
                                StartCoroutine(island.destroyPopup(island.createPopup("Veuillez sélectionner une quantité à envoyer"), 3));
                            }
                            else
                            {
                                //TODO 
                                Debug.Log("Start of sending resource");
                            }
                        }
                        break;
                }
            }
            else    //closure of other windows (listResources & listIslands)
            {
                Debug.Log("toto");
                Destroy(GameObject.Find("listIslandsCanvas_" + island.nameMinorIsland));
                Destroy(GameObject.Find("listResourcesCanvas_" + island.nameMinorIsland));

                island.otherWindowOpen = false;
            }

        }

        // Use this for initialization
        void Start()
        {
            island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        }

        // Update is called once per frame
        void Update()
        {
            if (island.resource != string.Empty)
            {
                refresh();
                resource_sp.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + island.resource + "Icon");
                island.resource = string.Empty;
            }
            if (island.islandToSend != string.Empty)
            {
                refresh();
                to.text = "Ile " + island.islandToSend[island.islandToSend.Length - 1].ToString();
                island.islandToSend = string.Empty;
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
