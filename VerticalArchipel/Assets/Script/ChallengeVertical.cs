using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.InputSources;
using TouchScript.Gestures;
using TouchScript.Hit;
using System.Collections.Generic;

namespace TouchScript.Examples.Cube
{

    public class ChallengeVertical : InputSource
    {
        static public bool challengeWindowPresent = false;

        public string[] rowSent { get; set; }    //initialized at the reception
        public string question { get; private set; }
        public string answer { get; private set; }
        public string explainations { get; private set; }
        public string[] propositions { get; private set; }
        public int nbPropositions { get; private set; }
        public TypeChallenge typeChallenge { get; set; }
        public SpriteRenderer background { get; private set; }
        public Text resultText { get; private set; }
        public bool goodAnswer { get; private set; }
        public Canvas canvasChallenge { get; private set; }

        public TextAsset csv { get; private set; }

        public void init(TypeChallenge tc, string[] row)
        {

            canvasChallenge = this.transform.parent.GetComponent<Canvas>();

            this.rowSent = row;
            this.typeChallenge = tc;
            if (typeChallenge == TypeChallenge.QCM)
                this.nbPropositions = 3;
            else
                this.nbPropositions = 2;
            

            this.question = row[0];
            this.answer = row[1];
            this.explainations = row[2];
            this.propositions = new string[nbPropositions];
            this.propositions[0] = row[3];
            this.propositions[1] = row[4];
            if (this.nbPropositions == 3)
                this.propositions[2] = row[5];


            foreach (Text text in canvasChallenge.GetComponentsInChildren<Text>())
            {
                switch (text.name)
                {
                    case "Question":
                        text.text = this.question.Replace('*', '\n');        //in CSV: '*' replace a line break ('\n')
                        break;
                    case "Result":
                        resultText = text;
                        break;
                    case "Proposition0":
                        text.text = this.propositions[0];
                        break;
                    case "Proposition1":
                        text.text = this.propositions[1];
                        break;
                    case "Proposition2":
                        text.text = this.propositions[2];
                        break;
                }
            }

            foreach (SpriteRenderer sp in canvasChallenge.GetComponentsInChildren<SpriteRenderer>())
            {
                if (sp.name == "background")
                    this.background = sp;
            }

        }


        public void OnMouseDownSimulation()
        {
            if ((this.name == "Challenge1") || (this.name == "Challenge2") || (this.name == "Challenge3"))
            {
                if (!ChallengeVertical.challengeWindowPresent)
                {
                    ChallengeVertical.challengeWindowPresent = true;
                    Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + this.typeChallenge.ToString());
                    Canvas canvasChallenge = Instantiate(challengePrefab);
                    canvasChallenge.name = "Challenge_" + this.typeChallenge.ToString();
                    foreach (ChallengeVertical cb in canvasChallenge.GetComponentsInChildren<ChallengeVertical>())
                    {
                        cb.init(this.typeChallenge, this.rowSent);
                    }
                    main.removeChallenge(GameObject.Find(this.name));
                }
            }
            else
            {
                string clickedText = this.name.Split('_')[0];

                //modify Result.text     
                if (clickedText == answer)
                {
                    resultText.text = "Réponse correcte !";
                    goodAnswer = true;
                }
                else {
                    resultText.text = "Réponse incorrecte !";
                    goodAnswer = false;
                }

                //modify Propositions background
                if (typeChallenge == TypeChallenge.VraiFaux)
                {
                    foreach (Image background in canvasChallenge.GetComponentsInChildren<Image>())
                    {
                        if (background.name == answer + "_background")
                            background.sprite = Resources.Load<Sprite>("Challenges/VraiFaux/case" + answer + "Clic");
                        else if (background.name.Contains("_background"))
                            background.sprite = Resources.Load<Sprite>("Challenges/VraiFaux/case" + background.name.Split('_')[0] + "Grise");
                    }
                }
                else
                {
                    foreach (Image background in canvasChallenge.GetComponentsInChildren<Image>())
                    {
                        if (background.name == answer + "_background")
                            background.sprite = Resources.Load<Sprite>("Challenges/QCM/case" + answer + "Clic");
                        else if (background.name.Contains("_background"))
                            background.sprite = Resources.Load<Sprite>("Challenges/QCM/case" + background.name.Split('_')[0] + "Grise");
                    }
                }

                StartCoroutine(wait());
            }
            
        }

        IEnumerator wait()
        {
            yield return new WaitForSeconds(0.5f);
            Color color;
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(0.001f);

                color = background.material.color;
                color.a -= 0.01f;
                background.material.color = color;
            }

            //TODO: actions

            Debug.Log("do sth");

            ChallengeVertical.challengeWindowPresent = false;

            Destroy(GameObject.Find("Challenge_" + typeChallenge));

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
            if (Time.time - TouchTime < 0.5)
            {
                TouchTime = 0;
                this.OnMouseDownSimulation();
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
