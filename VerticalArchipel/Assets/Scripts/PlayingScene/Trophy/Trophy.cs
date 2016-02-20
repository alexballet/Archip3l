using UnityEngine;
using System;

public class Trophy : MonoBehaviour
{
    public int Id;
    public string Name;
    public string Description;
    public bool Status;
    //public int PosX { get; private set; }
    //public int PosY { get; private set; }
    //public string EmptyMedalPath { get; private set; }
    //public string ObtainedMedalPath { get; private set; }
    //private Image EmptyMedal;
    //private Image WonMedal;
    //public Image Image { get; private set; }
    //public Bonus Bonus { get; private set; }

    public Trophy(int id, string name, string description) //, int posX, int posY, Bonus bonus)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        //this.PosX = posX;
        //this.PosY = posY;
        //this.EmptyMedalPath = "C:/tempConcours/trophy-emptyMedal.png";
        //this.ObtainedMedalPath = "C:/tempConcours/trophy-wonMedal-" + this.Id + ".png";
        this.Status = false;
        //this.Bonus = bonus;
        //this.EmptyMedal = new Image { Name = "trophy" + this.Id, Source = new BitmapImage(new Uri(this.EmptyMedalPath, UriKind.RelativeOrAbsolute)), };
        //this.WonMedal = new Image { Name = "trophy" + this.Id, Source = new BitmapImage(new Uri(this.ObtainedMedalPath, UriKind.RelativeOrAbsolute)), };
        //this.Image = EmptyMedal;
    }
    public bool changeToObtained()
    {
        if (this.Status)
        {
            return false;
        }
        else
        {
            //if (!this.Bonus.applyBonus())
            //{
            //    return false;
            //}
            this.Status = true;
            //this.Image = this.WonMedal;
            return true;
        }
    }
    public bool changeToEmpty()
    {
        if (!this.Status)
        {
            return false;
        }
        else
        {
            this.Status = false;
            //this.Image = this.EmptyMedal;
            return true;
        }
    }
}
