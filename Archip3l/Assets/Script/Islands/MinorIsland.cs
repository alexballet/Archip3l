using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TouchScript.Gestures;
using TouchScript.Hit;

namespace TouchScript.InputSources
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
        public Vector2 placeOfBuildingConstruction;
        public bool wheelPresent = false;                   //wheel present on the island
        public bool buildingInfoPresent = false;            //buildingInfo present on the island
        public bool challengePresent = false;               //challenge present on the island
        public bool moveBuilding = false;                   //moving a building
        public bool exchangeWindowPresent = false;          //exchangeWindow present on the island
        public string nameBuildingTouchCanvas;
        public string buildingClickedWheel;

        //for exchange resources window
        public bool otherWindowOpen = false;     //choice of Island or Resource
        public string resource = string.Empty;
        public string islandToSend = string.Empty;

        //impossible to perform 2 resource exchanges at the same time
        static public bool exchangePerforming = false;

        public Canvas startCanvas;
        public int numPopup = 0;

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
            displayPopup("C'est parti !", 3);
            //StartCoroutine(destroyPopup(createPopup("C'est parti !"), 3));

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

            GameObject.Find(nameMinorIsland).GetComponent<MeshCollider>().enabled = false;

            //random type of ChallengeBuild
            TypeChallenge type;
            System.Random ran = new System.Random();
            int aleat = ran.Next(0, 2);
            if (aleat == 0)
                type = TypeChallenge.VraiFaux;
            else
                type = TypeChallenge.QCM;
            

            Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenges/Build_Challenge_" + type.ToString());
            Canvas canvasChallenge = Instantiate(challengePrefab);

            canvasChallenge.name = "Challenge_" + type.ToString() + "_" + this.nameMinorIsland;
            canvasChallenge.transform.SetParent(GameObject.Find(this.nameMinorIsland).transform);
            Vector3 vec = GameObject.Find("Virtual_" + this.nameMinorIsland).transform.position;
            vec.z = -2;
            canvasChallenge.transform.position = vec;

            //rotation if other side of the table
            char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                canvasChallenge.transform.Rotate(Vector3.forward * 180);

            foreach (ChallengeBuild cb in canvasChallenge.GetComponentsInChildren<ChallengeBuild>())
            {
                cb.init(type, this, (TypeBuilding)System.Enum.Parse(typeof(TypeBuilding), buildingClicked));
            }
            

            GameObject.Find(nameMinorIsland).GetComponent<MeshCollider>().enabled = true;
        }

        public void createChallengeUpgrade(Building building)
        {

            GameObject.Find(nameMinorIsland).GetComponent<MeshCollider>().enabled = false;

            //random type of ChallengeUpgrade
            TypeChallenge type;
            System.Random ran = new System.Random();
            int aleat = ran.Next(0, 2);
            if (aleat == 0)
                type = TypeChallenge.VraiFaux;
            else
                type = TypeChallenge.QCM;


            Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenges/Upgrade_Challenge_" + type.ToString());
            Canvas challengeUpgrade = Instantiate(challengePrefab);

            challengeUpgrade.name = "Challenge_" + type.ToString() + "_" + this.nameMinorIsland;
            challengeUpgrade.transform.SetParent(GameObject.Find(this.nameMinorIsland).transform);
            Vector3 vec = GameObject.Find("Virtual_" + this.nameMinorIsland).transform.position;
            vec.z = -2;
            challengeUpgrade.transform.position = vec;

            //rotation if other side of the table
            char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                challengeUpgrade.transform.Rotate(Vector3.forward * 180);

            foreach (ChallengeUpgrade cu in challengeUpgrade.GetComponentsInChildren<ChallengeUpgrade>())
            {
                cu.init(type, this, building);             //TODO : adapt challenge to TypeBuilding
            }

            GameObject.Find(nameMinorIsland).GetComponent<MeshCollider>().enabled = true;
        }

        public void displayPopup(string popupText, int time)
        {
            if (popupText != string.Empty)
                StartCoroutine(destroyPopup(createPopup(popupText), time));
        }

        //surcharge: for building (explaination displayed at the end of previous popup)
        public void displayPopup(string popupText, int time, string explaination)
        {
            if (popupText != string.Empty)
                StartCoroutine(destroyPopup(createPopup(popupText), time, explaination));
        }

        //returns the name of the Popup (GameObject) created
        public string createPopup(string popupText)
        {
            if (GameObject.Find("PopupCanvas" + "_" + this.nameMinorIsland) != null)
            {
                GameObject.Find("PopupCanvas" + "_" + this.nameMinorIsland).GetComponentInChildren<Popup>().touched = true;
                Destroy(GameObject.Find("PopupCanvas" + "_" + this.nameMinorIsland));
            }

            Canvas popupCanvasPrefab = Resources.Load<Canvas>("Prefab/PopupCanvas");
            Canvas popupCanvas = Instantiate(popupCanvasPrefab);
            popupCanvas.name = "PopupCanvas" + "_" + this.nameMinorIsland;
            this.numPopup++;
            popupCanvas.transform.SetParent(GameObject.Find(this.nameMinorIsland).transform);
            Vector3 vector3 = GameObject.Find("sprite-" + this.nameMinorIsland).transform.position;
            vector3.z = -2;
            popupCanvas.transform.position = vector3;

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
        

        //destroy popup after a certain time
        public IEnumerator destroyPopup(string namePopup, int timer)
        {
            Popup popup = GameObject.Find(namePopup).GetComponentInChildren<Popup>();
            SpriteRenderer popupImage = GameObject.Find(namePopup).GetComponentInChildren<SpriteRenderer>();

            yield return new WaitForSeconds(timer);
            Color color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.01f);
                if (!popup.touched)
                {
                    color = popupImage.color;
                    color.a -= 0.01f;
                    popupImage.color = color;
                }
                else
                    break;

            }
            if (!popup.touched)
                Destroy(GameObject.Find(namePopup));
        }

        //surcharge: for buildings (display explaination after previous popup)
        //destroy popup after a certain time
        public IEnumerator destroyPopup(string namePopup, int timer, string explaination)
        {
            Popup popup = GameObject.Find(namePopup).GetComponentInChildren<Popup>();
            SpriteRenderer popupImage = GameObject.Find(namePopup).GetComponentInChildren<SpriteRenderer>();

            yield return new WaitForSeconds(timer);
            Color color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.01f);
                if (!popup.touched)
                {
                    color = popupImage.color;
                    color.a -= 0.01f;
                    popupImage.color = color;
                }
                else
                    break;

            }
            if (!popup.touched)
                Destroy(GameObject.Find(namePopup));

            yield return new WaitForSeconds(0.5f);
            displayPopup(explaination, timer);
        }
        

        public void removeAllPopups()
        {
            for (int i = 0; i < this.numPopup; i++)
            {

                if (GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameMinorIsland) != null)
                {
                    GameObject.Find("PopupCanvas" + i.ToString() + "_" + nameMinorIsland).GetComponentInChildren<Popup>().touched = true;
                    //StartCoroutine(forceDestroyPopup("PopupCanvas" + i.ToString() + "_" + nameMinorIsland, 0));
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
            Vector3 pos = GameObject.Find(this.nameBuildingTouchCanvas).transform.position;
            pos.z = -2;
            touchBuildingCanvas.transform.position = pos;

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
        
        void createExchangeWindow()
        {
            if (!exchangeWindowPresent && !wheelPresent)
            {
                Canvas exchangeWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/exchangeWindowCanvas");
                Canvas exchangeWindowCanvas = Instantiate(exchangeWindowCanvasPrefab);
                exchangeWindowCanvas.transform.parent = GameObject.Find(this.nameMinorIsland).transform;
                exchangeWindowCanvas.name = "ExchangeWindowCanvas_" + this.nameMinorIsland;
                Vector3 vector3 = GameObject.Find("sprite-" + this.nameMinorIsland).transform.position;
                vector3.z = -2;
                exchangeWindowCanvas.transform.position = vector3;

                this.exchangeWindowPresent = true;
            }
        }

        void OnMouseDownSimulation()
        {

            //moving a building
            if (moveBuilding)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(positionTouched.x, positionTouched.y, 0));
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
                    if (!challengePresent && !exchangeWindowPresent)
                    {
                        if (!wheelPresent)  //if the wheel is not on the island
                        {
                            //Wheel appearance
                            this.placeOfBuildingConstruction = this.positionTouched;
                            Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/WheelCanvas");
                            Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
                            wheelCanvas.name = "WheelCanvas_" + nameMinorIsland;
                            //parent : island
                            wheelCanvas.transform.SetParent(GameObject.Find(nameMinorIsland).transform);
                            SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
                            //position of wheel where it was clicked on
                            Vector3 pos = Camera.main.ScreenToWorldPoint(this.positionTouched);
                            pos.z = -1;
                            wheelImage.transform.position = pos;
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

        //disabled building: default disabled (because of Islands specializations) + building already built on the Island
        private List<string> getDisabledBuildings(string nameMinorIsland)
        {
            List<string> list = new List<string>();
            switch (this.nameMinorIsland)
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
            foreach (Building building in this.buildingManager.buildingList)
            {
                list.Add("wheelIcon_" + building.TypeBuilding.ToString());
            }
            return list;
        }

        void OnTriggerEnter(Collider collider)
        {
            //CHECK IF COLLISION COMES FROM PIRATES OR RESOURCES BOAT !!!!

            //Debug.Log(collider.name);
            /*int resourceCount = this.resourceManager.Resources.Count;
            if (resourceCount > 0)
            {
                System.Random rnd = new System.Random();
                int index = rnd.Next(0, resourceCount);
                int quantity = rnd.Next(10, 50);
                if (this.resourceManager.changeResourceStock(this.resourceManager.Resources[index].TypeResource, -quantity))
                {
                    //Notice someone? network?
                    //Debug.Log("Les pirates vous ont volé : " + quantity + " de " + this.resourceManager.Resources[index].TypeResource.ToString());
                }
            }
            Destroy(collider.gameObject);*/
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
            if (TouchTime == 0 && !MinorIsland.exchangePerforming)
            {
                TouchTime = Time.time;
                this.positionTouched = touch.Position;
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
                this.OnMouseDownSimulation();
            }
            else if(Time.time - TouchTime < 1.5)
            {
                TouchTime = 0;
                this.createExchangeWindow();
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