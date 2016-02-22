using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Challenge : ScriptableObject {

    public string question;
    public string answer;
    public string[] propositions;
    public TypeChallenge typeChallenge;
    public Canvas canvasChallenge;
    public MinorIsland minorIsland;

    public void init(TypeChallenge tc, MinorIsland island)
    {
        //init questions-answer-propositions

        this.minorIsland = island;
        this.typeChallenge = tc;
        Canvas canvas = Resources.Load<Canvas>("Prefab/Challenge_VraiFaux");
        canvasChallenge = Instantiate(canvas);
        Text question = canvasChallenge.GetComponentInChildren<Text>();
        question.text = "toto";
        Button[] propositions = canvasChallenge.GetComponentsInChildren<Button>();
        int nbPropositions;
        if (TypeChallenge.QCM == typeChallenge)
            nbPropositions = 3;
        else
            nbPropositions = 2;
        for (int i = 0; i < nbPropositions; i++)
        {
            propositions[i].GetComponent<Text>().text = "prop" + i.ToString();
        }

        //gérer position background + question/réponses

    }


/*
    void Awake()
    {
        canvasChallenge = (Canvas)Instantiate(Resources.Load("Challenge_" + typeChallenge.ToString()));
    }

    void Start()
    {

    }

    void Update()
    {

    }*/

}
