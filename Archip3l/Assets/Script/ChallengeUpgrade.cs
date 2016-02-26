﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChallengeUpgrade : MonoBehaviour {

    public string buildingConcerned;
    public string question;
    public string answer;
    public string explainations;
    public string[] propositions;
    public int nbPropositions;
    public TypeChallenge typeChallenge;
    public Canvas canvasChallenge;
    public SpriteRenderer background;
    public Button[] propositionsButtons;
    public Text resultText;
    public MinorIsland minorIsland;
    public Building buidling;
    public bool goodAnswer;


    public TextAsset csv;

    public void init(TypeChallenge tc, MinorIsland island, Building myBuilding)         //pb with TypeBuildingStat
    {
        this.buidling = myBuilding;
        this.minorIsland = island;
        this.typeChallenge = tc;
        if (typeChallenge == TypeChallenge.QCM)
            this.nbPropositions = 3;
        else
            this.nbPropositions = 2;


        //CSV part
        //row[0] : building ; row[1] : question ; row[2] : answer ; row[3] : explainations ; after : propositions
        //VraiFaux : answer = VRAI ou answer = FAUX
        //QCM : answer = Proposition0 ou answer = Proposition1 ou answer = Proposition2

        csv = Resources.Load<TextAsset>("Challenges/" + typeChallenge.ToString() + "/" + typeChallenge.ToString());

        //CSV_reader.DebugOutputGrid(CSV_reader.SplitCsvGrid(csv.text));
        string[] row = CSV_reader.GetRandomLine(csv.text);

        //theme of question is linked to building to construct  --> order ??
        this.buildingConcerned = row[0];
        this.question = row[1];
        this.answer = row[2];
        this.explainations = row[3];
        this.propositions = new string[nbPropositions];
        this.propositions[0] = row[4];
        this.propositions[1] = row[5];
        if (this.nbPropositions == 3)
            this.propositions[2] = row[6];


        //graphic part

        Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + this.typeChallenge);
        canvasChallenge = Instantiate(challengePrefab);
        canvasChallenge.name = "Challenge_" + this.typeChallenge + "_" + minorIsland.name;
        canvasChallenge.transform.SetParent(GameObject.Find(minorIsland.nameMinorIsland).transform);
        Text questionText = null;
        foreach (Text text in canvasChallenge.GetComponentsInChildren<Text>())
        {
            if (text.name == "Question")
                questionText = text;
            else if (text.name == "Result")
                resultText = text;
        }

        propositionsButtons = canvasChallenge.GetComponentsInChildren<Button>();
        background = canvasChallenge.GetComponentInChildren<SpriteRenderer>();
        
        questionText.text = question.Replace('*', '\n');        //in CSV: '*' replace a line break ('\n')
        for (int i = 0; i < this.nbPropositions; i++)
        {
            propositionsButtons[i].GetComponent<Text>().text = this.propositions[i];
            propositionsButtons[i].onClick.AddListener(() => { propositionClick();});
        }

        
        canvasChallenge.transform.position = GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform.position;

        //rotation if other side of the table
        char id = minorIsland.nameMinorIsland[minorIsland.nameMinorIsland.Length - 1];
        if (id == '1' || id == '2')
            canvasChallenge.transform.Rotate(Vector3.forward * 180);

        background.transform.localPosition = new Vector3(0, 0, -1);

        
    }


    public void propositionClick()
    {
        string clickedText = EventSystem.current.currentSelectedGameObject.GetComponent<Text>().name;
        
        //modify Result.text     
        if (clickedText == answer)
        {
            resultText.text = "Réponse correcte !";
            goodAnswer = true;
        }           
        else{
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
        
        Destroy(GameObject.Find("Challenge_" + typeChallenge + "_" + minorIsland.nameMinorIsland));

        StartCoroutine(minorIsland.destroyPopup(minorIsland.createPopup(explainations), 8));

        minorIsland.challengePresent = false;
        
        //minorIsland.buildingClicked is a string --> conversion necessary
        if(Enum.IsDefined(typeof(TypeBuilding), minorIsland.buildingClicked))
        {
            TypeBuilding typeBuilding = (TypeBuilding)Enum.Parse(typeof(TypeBuilding), minorIsland.buildingClicked, true);

            //TODO : upgrade of the building
            TypeResource typeResourceProduced = (TypeResource)Enum.Parse(typeof(TypeResource), minorIsland.getNameResourceOrStatProduced(buidling.TypeBuilding.ToString()), true);
            Debug.Log(typeResourceProduced);
            if (buidling.resourceManager.getResource(typeResourceProduced) == null)
                Debug.Log("resource not found");
            Debug.Log(buidling.resourceManager.getResource(typeResourceProduced).Production.ToString());
            buidling.resourceManager.changeResourceProduction(typeResourceProduced, buidling.resourceManager.getResource(typeResourceProduced).Production * 2);
            Debug.Log(buidling.resourceManager.getResource(typeResourceProduced).Production.ToString());


        }
        else if(Enum.IsDefined(typeof(TypeBuildingStat), minorIsland.buildingClicked))
        {
            TypeBuildingStat typeBuildingStat = (TypeBuildingStat)Enum.Parse(typeof(TypeBuildingStat), minorIsland.buildingClicked, true);

            //TODO : gérer le cas de la construction d'un batiment qui ne produit pas de ressources, mais des stats (ex : Hotel, Airport...)
            
        }


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