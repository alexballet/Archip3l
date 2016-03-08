using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;

namespace TouchScript.InputSources
{

    public class ExchangeResource : InputSource
    {

        public MinorIsland island;

        SpriteRenderer less = null;
        SpriteRenderer more = null;
        SpriteRenderer resource_sp = null;
        SpriteRenderer send = null;
        Text quantityValue;
        Text to;

        SpriteRenderer exchangeResourceAnimation;



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
                if (sp.name == "Send")
                {
                    send = sp;
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
                string resourceName;
                int resourceQuantity;
                if (resource_sp.GetComponent<SpriteRenderer>().sprite.name == "Knob"){  //default sprite
                    resourceName = string.Empty;
                    resourceQuantity = 0;
                }
                else
                {
                    resourceName = Resource.getResourceFromIconName(resource_sp.GetComponent<SpriteRenderer>().sprite.name);
                    resourceQuantity = this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), resourceName)).Stock;
                }

                Vector3 vector3;

                switch (this.name)
                {
                    case "ResourceValue":
                        Canvas listResourcesCanvasPrefab = Resources.Load<Canvas>("Prefab/ListResourcesCanvas");
                        Canvas listResourcesCanvas = Instantiate(listResourcesCanvasPrefab);
                        listResourcesCanvas.name = "listResourcesCanvas_" + island.nameMinorIsland;
                        listResourcesCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                        vector3 = GameObject.Find("sprite-" + island.nameMinorIsland).transform.position;
                        vector3.z = -3;
                        listResourcesCanvas.transform.position = vector3;
                        foreach (SpriteRenderer sr in listResourcesCanvas.GetComponentsInChildren<SpriteRenderer>())
                        {
                            sr.GetComponent<BoxCollider>().enabled = true;
                            if ((sr.name != "Close") && (this.island.resourceManager.getResource((TypeResource)System.Enum.Parse(typeof(TypeResource), sr.name)).Stock < 5))
                            {
                                sr.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + sr.name + "Icon_disabled");
                                sr.GetComponent<BoxCollider>().enabled = false;
                            }
                        }
                        island.otherWindowOpen = true;
                        quantityValue.text = "0";
                        break;
                    case "Less":
                        if (quantityValue.text == "0")
                            this.GetComponent<BoxCollider>().enabled = false;
                        else
                        {
                            quantityValue.text = (int.Parse(quantityValue.text) - 5).ToString();
                            more.GetComponent<BoxCollider>().enabled = true;
                        }
                        break;
                    case "More":
                        this.GetComponent<BoxCollider>().enabled = true;
                        if ((quantityValue.name == "QuantityValue") && (int.Parse(quantityValue.text) + 5 <= resourceQuantity))
                        {
                            quantityValue.text = (int.Parse(quantityValue.text) + 5).ToString();
                            less.GetComponent<BoxCollider>().enabled = true;
                        }
                        break;
                    case "Island":
                        Canvas listIslandsCanvasPrefab = Resources.Load<Canvas>("Prefab/ListeIslandsCanvas");
                        Canvas listIslandsCanvas = Instantiate(listIslandsCanvasPrefab);
                        listIslandsCanvas.name = "listIslandsCanvas_" + island.nameMinorIsland;
                        listIslandsCanvas.transform.SetParent(GameObject.Find(island.nameMinorIsland).transform);
                        vector3 = GameObject.Find("sprite-" + island.nameMinorIsland).transform.position;
                        vector3.z = -3;
                        listIslandsCanvas.transform.position = vector3;
                        //disable clic on self island
                        foreach(SpriteRenderer sp in listIslandsCanvas.GetComponentsInChildren<SpriteRenderer>())
                        {
                            if (sp.name == island.nameMinorIsland)
                                sp.GetComponent<BoxCollider>().enabled = false;
                        }
                        island.otherWindowOpen = true;
                        break;
                    case "Close":
                        Destroy(GameObject.Find(this.transform.parent.parent.name));
                        island.exchangeWindowPresent = false;
                        break;
                    case "Send":
                        if (to.text == "Ile X")
                        {
                            island.displayPopup("Veuillez sélectionner une île, en appuyant sur \"Ile X\".", 3);
                            //StartCoroutine(island.destroyPopup(island.createPopup("Veuillez sélectionner une île, en appuyant sur \"Ile X\"."), 3));
                        }
                        else
                        {
                            if (quantityValue.text == "0")
                            {
                                island.displayPopup("Veuillez sélectionner une quantité à envoyer", 3);
                                //StartCoroutine(island.destroyPopup(island.createPopup("Veuillez sélectionner une quantité à envoyer"), 3));
                            }
                            else
                            {
                                if (resource_sp.sprite.name == "Knob") //defaults sprite when not already clicked on
                                { 
                                    island.displayPopup("Veuillez sélectionner une ressource à envoyer", 3);
                                    //StartCoroutine(island.destroyPopup(island.createPopup("Veuillez sélectionner une ressource à envoyer"), 3));
                                }
                                else
                                {
                                    if (MinorIsland.exchangePerforming)
                                    {
                                        island.displayPopup("Veuillez attendre la fin de l'échange en cours", 3);
                                        //StartCoroutine(island.destroyPopup(island.createPopup("Veuillez attendre la fin de l'échange en cours"), 3));
                                    }
                                    else
                                    {
                                        //withdrawal of resources
                                        TypeResource tr = (TypeResource)System.Enum.Parse(typeof(TypeResource), Resource.getResourceFromIconName(this.resource_sp.sprite.name));
                                        int quantitySent = int.Parse(quantityValue.text);
                                        island.resourceManager.getResource(tr).changeStock(-quantitySent);

                                        MinorIsland.exchangePerforming = true;
                                        SpriteRenderer exchangeResourceAnimationPrefab = Resources.Load<SpriteRenderer>("Prefab/exchangeResourceAnimation/exchangeResourceAnimation_" + island.nameMinorIsland[island.nameMinorIsland.Length - 1].ToString()); // + "-" + to.text[to.text.Length - 1].ToString());
                                        exchangeResourceAnimation = Instantiate(exchangeResourceAnimationPrefab);
                                        exchangeResourceAnimation.transform.parent = GameObject.Find(island.nameMinorIsland).transform;
                                        exchangeResourceAnimation.name = "ExchangeResourceAnimation_" + island.nameMinorIsland;
                                        exchangeResourceAnimation.GetComponent<BoatMoving>().islandToSend = "sous_ile_" + to.text[to.text.Length -1].ToString();
                                        exchangeResourceAnimation.GetComponent<BoatMoving>().quantityCarried = quantitySent;
                                        exchangeResourceAnimation.GetComponent<BoatMoving>().resourceSent = tr.ToString();
                                        island.exchangeWindowPresent = false;
                                        island.displayPopup("Emmenez le bateau jusqu'au port de l'île sélectionnée pour lui apporter les ressources", 5);
                                        Destroy(GameObject.Find(this.transform.parent.parent.name));
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            else    //closure of other windows (listResources & listIslands)
            {
                Destroy(GameObject.Find("listIslandsCanvas_" + island.nameMinorIsland));
                Destroy(GameObject.Find("listResourcesCanvas_" + island.nameMinorIsland));

                island.otherWindowOpen = false;
            }

        }
        
        


        // Use this for initialization
        void Start()
        {
            island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
            refresh();
        }

        // Update is called once per frame
        void Update()
        {
            if (island.resource != string.Empty)
            {
                refresh();
                resource_sp.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + island.resource + "Icon");
                resource_sp.transform.localScale = new Vector2(0.1f, 0.1f);
                resource_sp.GetComponent<BoxCollider>().size = new Vector2(5, 5);
                //island.resource = string.Empty;
            }
            if (island.islandToSend != string.Empty)
            {
                refresh();
                to.text = "Ile " + island.islandToSend[island.islandToSend.Length - 1].ToString();
                //island.islandToSend = string.Empty;
            }
            if ((island.resource != string.Empty) && (island.islandToSend != string.Empty) && (this.quantityValue.text != "0"))
            {
                if (send.sprite.name != "boutonEnvoyer")
                    send.sprite = Resources.Load<Sprite>("fenetreEchange/boutonEnvoyer");
            }
            else
            {
                if (send.sprite.name != "boutonEnvoyerGrise")
                    send.sprite = Resources.Load<Sprite>("fenetreEchange/boutonEnvoyerGrise");
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
