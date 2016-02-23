using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Challenge : MonoBehaviour {

    public string question;
    public string buildingConcerned;
    public string answer;
    public string[] propositions;
    public int nbPropositions;
    public TypeChallenge typeChallenge;
    public Canvas canvasChallenge;
    public MinorIsland minorIsland;

    public TextAsset csv;

    public void init(TypeChallenge tc, MinorIsland island)
    {

        this.minorIsland = island;
        this.typeChallenge = tc;
        if (typeChallenge == TypeChallenge.QCM)
            this.nbPropositions = 3;
        else
            this.nbPropositions = 2;


        //CSV part
        //row[0] : building ; row[1] : question ; row[2] : answer ; after: propositions

        csv = Resources.Load<TextAsset>("Challenges/" + typeChallenge.ToString());

        //CSV_reader.DebugOutputGrid(CSV_reader.SplitCsvGrid(csv.text));
        string[] row = CSV_reader.GetRandomLine(csv.text);

        this.buildingConcerned = row[0];
        this.question = row[1];
        this.answer = row[2];
        this.propositions = new string[nbPropositions];
        this.propositions[0] = row[3];
        this.propositions[1] = row[4];
        if (this.nbPropositions == 3)
            this.propositions[2] = row[5];


        //graphic part

        Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + this.typeChallenge);
        canvasChallenge = Instantiate(challengePrefab);
        canvasChallenge.transform.SetParent(GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform);
        Text questionText = canvasChallenge.GetComponentInChildren<Text>();
        Button[] propositionsButtons = canvasChallenge.GetComponentsInChildren<Button>();
        SpriteRenderer background = canvasChallenge.GetComponentInChildren<SpriteRenderer>();
        
        questionText.text = question.Replace('*', '\n');        //in CSV: '*' replace a line break ('\n')
        for (int i = 0; i < this.nbPropositions; i++)
        {
            propositionsButtons[i].GetComponent<Text>().text = this.propositions[i];
        }

        
        canvasChallenge.transform.position = GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform.position;

        //rotation if other side of the table
        char id = minorIsland.nameMinorIsland[minorIsland.nameMinorIsland.Length - 1];
        if (id == '1' || id == '2')
            canvasChallenge.transform.Rotate(Vector3.forward * 180);

        background.transform.localPosition = new Vector3(0, 0, -1);

        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
