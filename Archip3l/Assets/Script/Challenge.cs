using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Challenge : ScriptableObject {

    public string question;
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

        csv = Resources.Load<TextAsset>("Challenges/" + typeChallenge.ToString());

        //CSV_reader.DebugOutputGrid(CSV_reader.SplitCsvGrid(csv.text));
        string[] row = CSV_reader.GetRandomLine(csv.text);

        this.question = row[0];
        this.answer = row[1];
        this.propositions = new string[this.nbPropositions];
        this.propositions[0] = row[2];
        this.propositions[1] = row[3];
        if (this.nbPropositions == 3)
            this.propositions[2] = row[4];
       
        
        
        Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + this.typeChallenge);
        canvasChallenge = Instantiate(challengePrefab);
        canvasChallenge.transform.SetParent(GameObject.Find("Virtual_sous_ile_1").transform);
        Text questionText = canvasChallenge.GetComponentInChildren<Text>();
        questionText.text = question;
        Button[] propositionsButtons = canvasChallenge.GetComponentsInChildren<Button>();
        SpriteRenderer background = canvasChallenge.GetComponentInChildren<SpriteRenderer>();

        for (int i = 0; i < this.nbPropositions; i++)
        {
            propositionsButtons[i].GetComponent<Text>().text = this.propositions[i];
        }


        //gérer position background + question/réponses
        
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float backgroundWidth = background.sprite.rect.width;
        float backgroundHeight = background.sprite.rect.height;        

        Vector2 vector2;

        switch (this.minorIsland.nameMinorIsland)
        {
            case "sous_ile_1":
                vector2 = new Vector2(-3.86f, 2.6f);
                break;
            case "sous_ile_2":
                vector2 = new Vector2(screenWidth / 2 + backgroundWidth, screenHeight / 2 + backgroundHeight);
                break;
            case "sous_ile_3":
                vector2 = new Vector2(-screenWidth / 2 - backgroundWidth, -screenHeight / 2 - backgroundHeight);
                break;
            default:
                vector2 = new Vector2(-screenWidth / 2 - backgroundWidth, screenHeight / 2 + backgroundHeight);
                break;
        }


        background.transform.position = vector2; // Camera.main.ScreenToWorldPoint(vector2);


    }
    
}
