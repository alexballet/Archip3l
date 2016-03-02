using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;

namespace TouchScript.Examples.Cube
{
    public class MinorIsland : InputSource
    {

        public BuildingManager buildingManager { get; private set; }
        public ResourceManager resourceManager { get; private set; }

        public Transform buildingManagerPrefab;
        public Transform resourceManagerPrefab;

        public string nameMinorIsland;

        //communication with WheelIcon, BuildingInfo & ChallengeBuild scripts + Popups & TouchBuilding
        public Vector2 positionTouched;
        public bool wheelPresent = false;                   //wheel present on the island
        public bool buildingInfoPresent = false;            //buildingInfo present on the island
        public bool challengePresent = false;               //challenge present on the island
        public bool moveBuilding = false;                   //moving a building
        public bool exchangeWindowPresent = false;          //exchangeWindow present on the island
        public string nameBuildingTouchCanvas;
        public string buildingClickedWheel;
        public int numPopup = 0;

        //for exchange resources window
        public bool otherWindowOpen = false;     //choice of Island or Resource
        public string resource = string.Empty;
        public string islandToSend = string.Empty;

        public Canvas startCanvas;

        void Awake()
        {

            var buildingManagerTransform = Instantiate(buildingManagerPrefab) as Transform;
            BuildingManager buildingManager = buildingManagerTransform.GetComponent<BuildingManager>();
            if (buildingManager != null)
            {
                buildingManager.init(this);
                buildingManager.transform.SetParent(this.transform);
                this.buildingManager = buildingManager;
            }

            var resourceManagerTransform = Instantiate(resourceManagerPrefab) as Transform;
            ResourceManager resourceManager = resourceManagerTransform.GetComponent<ResourceManager>();
            if (resourceManager != null)
            {
                resourceManager.init(this);
                resourceManager.transform.SetParent(this.transform);
                this.resourceManager = resourceManager;
            }

            Vector3 harborPosition;
            switch (this.nameMinorIsland)
            {
                case "sous_ile_1":
                    harborPosition = new Vector3(-84, 88, -1);
                    break;
                case "sous_ile_2":
                    harborPosition = new Vector3(132, 111, -1);
                    break;
                case "sous_ile_3":
                    harborPosition = new Vector3(-107, -71, -1);
                    break;
                default:
                    harborPosition = new Vector3(111, -77, -1);
                    break;
            }
            buildingManager.createBuilding(TypeBuilding.Harbor, harborPosition);

            StartCoroutine(destroyPopup(createPopup("C'est parti !"), 3));

        }

        public void Start()
        {
            if (nameMinorIsland == "sous_ile_1")
            {
                Canvas startCanvasPrefab = Resources.Load<Canvas>("Prefab/StartCanvas");
                startCanvas = Instantiate(startCanvasPrefab);
                startCanvas.name = "StartCanvas";
                Color color = startCanvas.GetComponentInChildren<SpriteRenderer>().color;
                color.a = 1;
                startCanvas.GetComponentInChildren<SpriteRenderer>().color = color;
                StartCoroutine(this.startFade());
            }
        }

        public IEnumerator startFade()
        {
            SpriteRenderer sp = startCanvas.GetComponentInChildren<SpriteRenderer>();
            Color color;
            for (int i = 0; i < 200; i++)
            {
                yield return new WaitForSeconds(0.01f);
                color = sp.color;
                color.a -= 0.005f;
                sp.color = color;
            }
            Destroy(GameObject.Find("StartCanvas"));
        }

        public void createChallengeBuild(string buildingClicked)
        {

            GameObject.Find(nameMinorIsland).GetComponent<BoxCollider>().enabled = false;
            ChallengeBuild challengeBuild = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<ChallengeBuild>();

            //random type of ChallengeBuild
            TypeChallenge type;
            System.Random ran = new System.Random();
            int aleat = ran.Next(0, 2);
            if (aleat == 0)
                type = TypeChallenge.VraiFaux;
            else
                type = TypeChallenge.QCM;

            challengeBuild.init(type, this, (TypeBuilding)System.Enum.Parse(typeof(TypeBuilding), buildingClicked));

            GameObject.Find(nameMinorIsland).GetComponent<BoxCollider>().enabled = true;
        }

        public void createChallengeUpgrade(Building building)
        {

            GameObject.Find(nameMinorIsland).GetComponent<BoxCollider>().enabled = false;
            ChallengeUpgrade challengeUpgrade = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<ChallengeUpgrade>();

            //random type of ChallengeUpgrade
            TypeChallenge type;
            System.Random ran = new System.Random();
            int aleat = ran.Next(0, 2);
            if (aleat == 0)
                type = TypeChallenge.VraiFaux;
            else
                type = TypeChallenge.QCM;

            challengeUpgrade.init(type, this, building);             //TODO : adapt challenge to TypeBuilding

            GameObject.Find(nameMinorIsland).GetComponent<BoxCollider>().enabled = true;
        }


        //returns the name of the Popup (GameObject) created
        public string createPopup(string popupText)
        {
            //this.removeAllPopups();

            Canvas popupCanvasPrefab = Resources.Load<Canvas>("Prefab/PopupCanvas");
            Canvas popupCanvas = Instantiate(popupCanvasPrefab);
            this.numPopup++;
            popupCanvas.name = "PopupCanvas" + numPopup.ToString() + "_" + this.nameMinorIsland;
            popupCanvas.transform.SetParent(GameObject.Find(this.nameMinorIsland).transform);
            Vector3 vector3 = GameObject.Find(this.nameMinorIsland).transform.position;
            vector3.z = (-1) * numPopup;
            popupCanvas.transform.position = vector3;
            //popupCanvas.transform.position = GameObject.Find(this.nameMinorIsland).transform.position;
            //rotation of image according to the place of the island
            char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                popupCanvas.transform.Rotate(Vector3.forward * 180);

            popupCanvas.GetComponentInChildren<Text>().text = popupText;

            //name + island passed to get the Canvas to destroy
            popupCanvas.GetComponentInChildren<Popup>().namePopupCanvas = popupCanvas.name;
            popupCanvas.GetComponentInChildren<Popup>().island = this;

            return popupCanvas.name;
        }

        public IEnumerator destroyPopup(string namePopup, int timer)
        {
            SpriteRenderer popupImage = GameObject.Find(namePopup).GetComponentInChildren<SpriteRenderer>();

            yield return new WaitForSeconds(timer);
            Color color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.01f);

                color = popupImage.color;
                color.a -= 0.01f;
                popupImage.color = color;

            }
            Destroy(GameObject.Find(namePopup));
        }

        public void removeAllPopups()
        {
            for (int i = this.numPopup; i > 0; i--)
            {
                if (GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameMinorIsland) != null)
                {
                    Destroy(GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameMinorIsland));
                }
            }
        }

        public void createBuildingTouch(Building building)
        {
            this.nameBuildingTouchCanvas = building.name;

            Canvas touchBuildingCanvasPrefab = Resources.Load<Canvas>("Prefab/touchBuildingCanvas");
            Canvas touchBuildingCanvas = Instantiate(touchBuildingCanvasPrefab);
            touchBuildingCanvas.transform.SetParent(this.transform);
            touchBuildingCanvas.name = "touchBuilding_" + this.nameBuildingTouchCanvas;
            touchBuildingCanvas.transform.position = GameObject.Find(this.nameBuildingTouchCanvas).transform.position;

            //Exception: moving and removing are impossible for Harbor
            if (building.TypeBuilding == TypeBuilding.Harbor)
            {
                foreach (SpriteRenderer sr in touchBuildingCanvas.GetComponentsInChildren<SpriteRenderer>())
                {
                    switch (sr.name)
                    {
                        case "Move":
                            sr.sprite = Resources.Load<Sprite>("wheelAppuiBatiment/boutonDeplacer_disabled");
                            break;
                        case "Remove":
                            sr.sprite = Resources.Load<Sprite>("wheelAppuiBatiment/boutonSupprimer_disabled");
                            break;
                    }
                }
            }

            //last level of upgrade : 3
            if (building.level == 3)
            {
                foreach (SpriteRenderer sr in touchBuildingCanvas.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (sr.name == "Upgrade")
                    {
                        sr.sprite = Resources.Load<Sprite>("wheelAppuiBatiment/boutonAmeliorer_disabled");
                        sr.GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }

            foreach (TouchBuilding touchBuilding in touchBuildingCanvas.GetComponentsInChildren<TouchBuilding>())
            {
                touchBuilding.island = this;
                touchBuilding.building = building;
            }
            //touchBuildingCanvas.GetComponent<TouchBuilding>().island = this;

            //rotation of image according to the place of the island
            char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                touchBuildingCanvas.transform.Rotate(Vector3.forward * 180);

        }



        // Update is called once per frame
        void Update()
        {

        }


        //test
        void createExchangeWindow()
        {
            if (!exchangeWindowPresent)
            {
                Canvas exchangeWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/exchangeWindowCanvas");
                Canvas exchangeWindowCanvas = Instantiate(exchangeWindowCanvasPrefab);
                exchangeWindowCanvas.transform.parent = GameObject.Find(this.nameMinorIsland).transform;
                exchangeWindowCanvas.name = "ExchangeWindowCanvas_" + this.nameMinorIsland;
                Vector3 vector3 = GameObject.Find(this.nameMinorIsland).transform.position;
                vector3.z = -2;
                exchangeWindowCanvas.transform.position = vector3;

                this.exchangeWindowPresent = true;
            }
        }


        void OnMouseDownSimulation()
        {

            //TESTS for exchangeResource
            /*this.wheelPresent = true;
            if (!exchangeWindowPresent)
            {
                Canvas exchangeWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/exchangeWindowCanvas");
                Canvas exchangeWindowCanvas = Instantiate(exchangeWindowCanvasPrefab);
                exchangeWindowCanvas.transform.parent = GameObject.Find(this.nameMinorIsland).transform;
                exchangeWindowCanvas.name = "ExchangeWindowCanvas_" + this.nameMinorIsland;
                Vector3 vector3 = GameObject.Find(this.nameMinorIsland).transform.position;
                vector3.z = -2;
                exchangeWindowCanvas.transform.position = vector3;

                this.exchangeWindowPresent = true;
            }*/

            //--------------


            //moving a building
            if (moveBuilding)
            {
                Vector3 pos = new Vector3(positionTouched.x, positionTouched.y, 0);
                pos.z = -1;
                GameObject.Find(this.nameBuildingTouchCanvas).transform.position = pos;
                this.moveBuilding = false;
                this.nameBuildingTouchCanvas = string.Empty;
            }
            else
            {
                if (this.nameBuildingTouchCanvas != String.Empty)
                {
                    Destroy(GameObject.Find("touchBuilding_" + this.nameBuildingTouchCanvas));
                    this.nameBuildingTouchCanvas = String.Empty;
                }
                else
                {
                    if (!challengePresent)      //if any challenge is open on the island
                    {
                        if (!wheelPresent)  //if the wheel is not on the island
                        {
                            //Wheel appearance
                            Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/WheelCanvas");
                            Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
                            wheelCanvas.name = "WheelCanvas_" + nameMinorIsland;
                            //parent : island
                            wheelCanvas.transform.SetParent(GameObject.Find(nameMinorIsland).transform);
                            SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
                            //position of wheel where it was clicked on
                            wheelImage.transform.position = positionTouched;
                            //rotation of image according to the place of the island
                            char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
                            if (id == '1' || id == '2')
                                wheelImage.transform.Rotate(Vector3.forward * 180);

                            //disable specific buildings
                            List<string> list = getDisabledBuildings(this.nameMinorIsland);
                            foreach (SpriteRenderer sr in wheelImage.GetComponentsInChildren<SpriteRenderer>())
                            {
                                if (list.Contains(sr.name))
                                {
                                    sr.sprite = Resources.Load<Sprite>("Building/Icons_Disabled/" + sr.name + "_disabled");
                                    sr.GetComponent<BoxCollider>().enabled = false;
                                }
                            }

                            this.wheelPresent = true;
                        }
                        else
                        {
                            if (!buildingInfoPresent)       //if the wheel is on the island, but not the buildingInfo
                            {
                                //destruction of the wheel if clic somewhere else in the island
                                Destroy(GameObject.Find("WheelCanvas_" + nameMinorIsland));
                                this.wheelPresent = false;
                            }
                        }
                    }
                }
            }

        }


        private List<string> getDisabledBuildings(string nameMinorIsland)
        {
            List<string> list = new List<string>();
            switch (nameMinorIsland)
            {
                case "sous_ile_1":
                    list.Add("wheelIcon_OilPlant");
                    list.Add("wheelIcon_StoneMine");
                    list.Add("wheelIcon_Sawmill");
                    break;
                case "sous_ile_2":
                    list.Add("wheelIcon_GoldMine");
                    list.Add("wheelIcon_StoneMine");
                    list.Add("wheelIcon_Sawmill");
                    break;
                case "sous_ile_3":
                    list.Add("wheelIcon_OilPlant");
                    list.Add("wheelIcon_GoldMine");
                    list.Add("wheelIcon_Sawmill");
                    break;
                case "sous_ile_4":
                    list.Add("wheelIcon_OilPlant");
                    list.Add("wheelIcon_GoldMine");
                    list.Add("wheelIcon_StoneMine");
                    break;
            }
            return list;
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
            positionTouched = Camera.main.ScreenToWorldPoint(touch.Position);
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
            else
                this.createExchangeWindow();
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