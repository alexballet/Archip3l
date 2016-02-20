using System;
using System.Collections.Generic;
using UnityEngine;

public class TrophyManager : MonoBehaviour
{
    public List<Trophy> Trophies { get; private set; }
    public GameObject TrophyGameObject;
    public event EventHandler<TrophyObtainedEventArgs> TrophyObtained;

    void start()
    {
        var trophy1 = Instantiate(this.TrophyGameObject) as GameObject;
        Trophy t = trophy1.gameObject.GetComponent<Trophy>();
        if (t != null)
        {
            this.Trophies.Add(t);
            t.Id = 1;
            t.Name = "Trophée ressources";
            t.Description = "Gain de ressources";
        }
        var trophy2 = Instantiate(this.TrophyGameObject) as GameObject;
        t = trophy2.gameObject.GetComponent<Trophy>();
        if (t != null)
        {
            this.Trophies.Add(t);
            t.Id = 2;
            t.Name = "Trophée innovation";
            t.Description = "Gain de productivité";
        }
        var trophy3 = Instantiate(this.TrophyGameObject) as GameObject;
        t = trophy3.gameObject.GetComponent<Trophy>();
        if (t != null)
        {
            this.Trophies.Add(t);
            t.Id = 3;
            t.Name = "Trophée rapidité";
            t.Description = "Gain de productivité";
        }
        var trophy4 = Instantiate(this.TrophyGameObject) as GameObject;
        t = trophy4.gameObject.GetComponent<Trophy>();
        if (t != null)
        {
            this.Trophies.Add(t);
            t.Id = 5;
            t.Name = "Trophée stratégie";
            t.Description = "Gain de ..";
        }
        var trophy5 = Instantiate(this.TrophyGameObject) as GameObject;
        t = trophy5.gameObject.GetComponent<Trophy>();
        if (t != null)
        {
            this.Trophies.Add(t);
            t.Id = 5;
            t.Name = "Trophée des légendes";
            t.Description = "Gain de ...";
        }

        foreach(Trophy tr in this.Trophies)
        {
            Debug.Log(tr.Name);
            Console.WriteLine(tr.Name);
        }
    }
    //public TrophyManager()
    //{
    //    this.Trophies = new List<Trophy>();

    //    Trophy trophy1 = new Trophy(1, "Trophée ressources", "Gain de ressources"); //, 50, 50, null);
    //    Trophy trophy2 = new Trophy(2, "Trophée innovation", "Multiplication de la productivité par 1.5"); //, 160, 50, null);
    //    Trophy trophy3 = new Trophy(3, "Trophée rapidité", "Multiplication de la productivité par 1.5"); //, 270, 50, null);
    //    Trophy trophy4 = new Trophy(4, "Trophée stratégie", "Une description"); //, 380, 50, null);
    //    Trophy trophy5 = new Trophy(5, "Trophée légende", "Une description"); //, 490, 50, null);

    //    this.Trophies.Add(trophy1);
    //    this.Trophies.Add(trophy2);
    //    this.Trophies.Add(trophy3);
    //    this.Trophies.Add(trophy4);
    //    this.Trophies.Add(trophy5);

    //    changeTrophyToObtained(trophy2);
    //    changeTrophyToObtained(trophy3);
    //    changeTrophyToObtained(trophy5);
    //}
    public List<Trophy> getTrophies(bool status)
    {
        List<Trophy> value = new List<Trophy>();
        if (this.Trophies.Count < 1)
        {
            return value;
        }
        for (int i = 0; i < this.Trophies.Count - 1; i++)
        {
            if (this.Trophies[i].Status == status)
            {
                value.Add(this.Trophies[i]);
            }
        }
        return value;
    }
    public void changeTrophyToObtained(Trophy trophy)
    {
        trophy.changeToObtained();
        if (this.TrophyObtained != null)
        {
            this.TrophyObtained(this, new TrophyObtainedEventArgs { Trophy = trophy });
        }
    }
}
public class TrophyObtainedEventArgs : EventArgs
{
    public Trophy Trophy;
}