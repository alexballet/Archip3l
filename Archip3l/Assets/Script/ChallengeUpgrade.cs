using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.Gestures;
using TouchScript.Hit;
using TouchScript;
using TouchScript.InputSources;

public class ChallengeUpgrade : InputSource
{


    static public string question { get; private set; }
    static public string answer { get; private set; }
    static public string explainations { get; private set; }
    static public string[] propositions { get; private set; }
    static public int nbPropositions { get; private set; }
    static public bool goodAnswer { get; private set; }
    static public TypeChallenge typeChallenge { get; private set; }
    static public SpriteRenderer background { get; private set; }
    static public Text resultText { get; private set; }
    static public MinorIsland minorIsland { get; private set; }
    static public Building building { get; private set; }
    static public Canvas canvasChallenge { get; private set; }

    static public TextAsset csv { get; private set; }

    public void init(TypeChallenge tc, MinorIsland island, Building myBuilding)
    {

        canvasChallenge = this.transform.parent.GetComponent<Canvas>();

        ChallengeUpgrade.building = myBuilding;
        ChallengeUpgrade.minorIsland = island;
        ChallengeUpgrade.typeChallenge = tc;
        if (typeChallenge == TypeChallenge.QCM)
            ChallengeUpgrade.nbPropositions = 3;
        else
            ChallengeUpgrade.nbPropositions = 2;


        //CSV part
        //row[0] : question ; row[1] : answer ; row[2] : explainations ; after : propositions
        //VraiFaux : answer = Proposition0 ou answer = Proposition1
        //QCM : answer = Proposition0 ou answer = Proposition1 ou answer = Proposition2

        //ENCODAGE : UTF8-16-LE
        //last line of file usually blank --> to be removed!
        csv = Resources.Load<TextAsset>("Challenges/ChallengesFiles/" + typeChallenge.ToString() + "/" + typeChallenge.ToString() + "_" + myBuilding.TypeBuilding.ToString());
        //csv = Resources.Load<TextAsset>("Challenges/" + typeChallenge.ToString() + "/" + typeChallenge.ToString() + "_Tests");


        string[] row = CSV_reader.GetRandomLine(csv.text);

        ChallengeUpgrade.question = row[0];
        addLineBreaks();
        ChallengeUpgrade.answer = row[1];
        ChallengeUpgrade.explainations = row[2];
        ChallengeUpgrade.propositions = new string[nbPropositions];
        ChallengeUpgrade.propositions[0] = row[3];
        ChallengeUpgrade.propositions[1] = row[4];
        if (ChallengeUpgrade.nbPropositions == 3)
            ChallengeUpgrade.propositions[2] = row[5];


        foreach (Text text in canvasChallenge.GetComponentsInChildren<Text>())
        {
            switch (text.name)
            {
                case "Question":
                    text.text = ChallengeUpgrade.question;
                    break;
                case "Result":
                    resultText = text;
                    break;
                case "Proposition0":
                    text.text = ChallengeUpgrade.propositions[0];
                    break;
                case "Proposition1":
                    text.text = ChallengeUpgrade.propositions[1];
                    break;
                case "Proposition2":
                    text.text = ChallengeUpgrade.propositions[2];
                    break;
            }
        }

        foreach (SpriteRenderer sp in canvasChallenge.GetComponentsInChildren<SpriteRenderer>())
        {
            if (sp.name == "background")
                ChallengeUpgrade.background = sp;
        }
    }

    void addLineBreaks()
    {
        const int maxChar = 35;
        List<int> spaces = new List<int>();
        int i = 0;
        foreach (char c in ChallengeBuild.question)
        {
            if (c == ' ')
                spaces.Add(i);
            i++;
        }

        int j = 0;
        i = 1;
        string toto = ChallengeBuild.question;
        while (maxChar * i <= ChallengeBuild.question.Length && j < spaces.Count)
        {
            while (spaces[j] < maxChar * i)
                j++;
            ChallengeUpgrade.question = question.Substring(0, spaces[j - 1]) + "\n" + question.Substring(spaces[j - 1] + 1);
            i++;
        }
    }


    void OnMouseDownSimulation()
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

        minorIsland.challengePresent = false;

        //ChallengeUpgrade.building.name is a string --> conversion necessary + split (name like: "sous_ile_X_nameBuilding")
        if (Enum.IsDefined(typeof(TypeBuilding), ChallengeUpgrade.building.name.Split('_')[3]))
        {
            //TypeBuilding typeBuilding = (TypeBuilding)Enum.Parse(typeof(TypeBuilding), minorIsland.buildingClicked, true);

            TypeResource typeResourceProduced = (TypeResource)Enum.Parse(typeof(TypeResource), Building.getNameResourceOrStatProduced(building.TypeBuilding.ToString()), true);

            if (goodAnswer)
            {
                building.level += 1;
                building.changeProduction(building.quantityProduced);
                building.quantityProduced *= 2;
                minorIsland.displayPopup("Bonne réponse ! Votre bâtiment passe au niveau " + building.level.ToString() + " !", 3, explainations);
            }
            else
            {
                if (building.level > 0)
                {
                    building.level -= 1;
                    building.changeProduction(-building.quantityProduced / 2);
                    building.quantityProduced /= 2;
                    minorIsland.displayPopup("Mauvaise réponse ! Votre bâtiment redescend au niveau " + building.level.ToString() + " ...", 3, explainations);
                }
                else
                    minorIsland.displayPopup("Mauvaise réponse ! L'amélioration n'a donc pas pu se faire ...", 3, explainations);
            }

            //upgrading animation
            if (building.level != 0)
                StartCoroutine(building.launchUpgradeAnimation());
        }

        Destroy(GameObject.Find("Challenge_" + typeChallenge + "_" + minorIsland.nameMinorIsland));

    }

    // Use ChallengeUpgrade for initialization
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
        if (TouchTime == 0 && !MinorIsland.exchangePerforming)
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
