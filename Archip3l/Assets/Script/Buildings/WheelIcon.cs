using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TouchScript.Examples.Cube;
using TouchScript.Gestures;
using TouchScript.InputSources;
using TouchScript.Hit;

namespace TouchScript.Examples.Cube
{
    public class WheelIcon : InputSource
    {

        MinorIsland island;


        // Use this for initialization
        void Start()
        {
            island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void OnMouseDownSimulation()
        {
            island.buildingClickedWheel = this.name.Split('_')[1];

            if (island.buildingInfoPresent == false)        //if any buildingInfo is open (not more than one at the same time)
            {
                Canvas buildingInfoPrefab = Resources.Load<Canvas>("Prefab/BuildingInfoWindowCanvas");
                Canvas buildingInfo = Instantiate(buildingInfoPrefab);

                buildingInfo.name = "BuildingInfo_" + this.name;
                buildingInfo.transform.SetParent(this.transform.parent.parent.parent);  //parent : minorIsland
                Vector3 pos = island.transform.position;
                pos.z = -2;
                buildingInfo.transform.position = pos;
                //modification of the content of the different Text Children of the Canvas

                List<Tuple<TypeResource, int>> constructionResourceNeeded = Building.getConstructionResourcesNeeded(island.buildingClickedWheel);


                foreach (Text textInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<Text>())
                {
                    switch (textInCanvas.name)
                    {
                        case "Name":
                            textInCanvas.text = Building.translateBuildingName(island.buildingClickedWheel);
                            break;
                        case "CostValue1":
                            textInCanvas.text = constructionResourceNeeded[0].Second.ToString();
                            break;
                        case "CostValue2":
                            if (constructionResourceNeeded.Count == 2)
                                textInCanvas.text = constructionResourceNeeded[1].Second.ToString();
                            else
                                textInCanvas.text = "-";
                            break;
                        case "ProductionValueGoodAnswer":
                            textInCanvas.text = (Building.getQuantityResourceOrStatProduced(island.buildingClickedWheel) * 2).ToString();
                            break;
                        case "ProductionValueBadAnswer":
                            textInCanvas.text = (Building.getQuantityResourceOrStatProduced(island.buildingClickedWheel)).ToString();
                            break;
                    }
                }

                //modification of the background of the different SpriteRenderer Children of the Canvas
                foreach (SpriteRenderer imageInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<SpriteRenderer>())
                {
                    switch (imageInCanvas.name)
                    {
                        case "CostImage1":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + constructionResourceNeeded[0].First.ToString() + "Icon");
                            break;
                        case "CostImage2":
                            if (constructionResourceNeeded.Count == 2)
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + constructionResourceNeeded[1].First.ToString() + "Icon");
                            else
                                imageInCanvas.sprite = null;
                            break;
                        //mêmes images
                        case "ProductionImage":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(island.buildingClickedWheel) + "Icon");
                            break;
                        case "ProductionImage2":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(island.buildingClickedWheel) + "Icon");
                            break;
                        case "Build":
                            if (island.resourceManager.getResource(constructionResourceNeeded[0].First).Stock < constructionResourceNeeded[0].Second)
                            {
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/boutonConstruireGrise");
                                imageInCanvas.GetComponent<BoxCollider2D>().enabled = false;
                            }
                            else if (constructionResourceNeeded.Count == 2)
                            {
                                if (island.resourceManager.getResource(constructionResourceNeeded[1].First).Stock < constructionResourceNeeded[1].Second)
                                {
                                    imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/boutonConstruireGrise");
                                    imageInCanvas.GetComponent<BoxCollider2D>().enabled = false;
                                }
                            }
                            break;
                    }
                }
                //rotation of image according to the place of the island
                char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];
                if (id == '1' || id == '2')
                    buildingInfo.transform.Rotate(Vector3.forward * 180);

                island.buildingInfoPresent = true;
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
            island.positionTouched = touch.Position;
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