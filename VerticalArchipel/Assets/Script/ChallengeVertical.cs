﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChallengeVertical : MonoBehaviour {

    public string[] rowSent;    //initialized at the reception
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
    //public MinorIsland minorIsland;
    public bool goodAnswer;

    public TextAsset csv;

    void OnMouseDown()
    {
        Debug.Log("Clic on " + this.name);
    }

    public void init(TypeChallenge tc/*, MinorIsland island*/)
    {

        //this.minorIsland = island;
        this.typeChallenge = tc;
        if (typeChallenge == TypeChallenge.QCM)
            this.nbPropositions = 3;
        else
            this.nbPropositions = 2;

        string[] row = this.rowSent;

        this.question = row[0];
        this.answer = row[1];
        this.explainations = row[2];
        this.propositions = new string[nbPropositions];
        this.propositions[0] = row[3];
        this.propositions[1] = row[4];
        if (this.nbPropositions == 3)
            this.propositions[2] = row[5];


        //graphic part

        Canvas challengePrefab = Resources.Load<Canvas>("Prefab/Challenge_" + this.typeChallenge);
        canvasChallenge = Instantiate(challengePrefab);
        canvasChallenge.name = "Challenge_" + this.typeChallenge;
        canvasChallenge.transform.SetParent(GameObject.Find("Challenges").transform);
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

        
        //canvasChallenge.transform.position = GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform.position;

        

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
        
        Destroy(GameObject.Find("Challenge_" + typeChallenge + "_"));

        //StartCoroutine(minorIsland.destroyPopup(minorIsland.createPopup(explainations), 8));

        
        //TODO


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