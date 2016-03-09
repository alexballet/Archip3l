using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

    public static SoundPlayer Instance;

    //Sound sources
    public AudioClip airportSound;
    public AudioClip amusementParkSound;
    public AudioClip churchSound;
    public AudioClip cinemaSound;
    public AudioClip constructionSound;
    public AudioClip factorySound;
    public AudioClip farmSound;
    public AudioClip harborSound;
    public AudioClip hotelsound;
    public AudioClip labSound;
    public AudioClip mineSound;
    public AudioClip oilPlantSound;
    public AudioClip powerPlantSound;
    public AudioClip sawMillSound;
    public AudioClip schoolSound;
    public AudioClip upgradeSound;
    public AudioClip windTurbineSound;

    public AudioClip explosionOneSound;
    public AudioClip boatSinkSound;

    public AudioClip startGameSound;
    public AudioClip endGameSound;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of SoundManager!");
        }
        Instance = this;
    }
    private void MakeSound(AudioClip originalClip)
    {
        AudioSource.PlayClipAtPoint(originalClip, transform.position, 10f);
    }

    public void playAirportSound()
    {
        MakeSound(airportSound);
    }
    public void playAmusementParkSound()
    {
        MakeSound(amusementParkSound);
    }
    public void playChurchSound()
    {
        MakeSound(churchSound);
    }
    public void playCinemaSound()
    {
        MakeSound(cinemaSound);
    }
    public void playConstructionSound()
    {
        MakeSound(constructionSound);
    }
    public void playFactorySound()
    {
        MakeSound(factorySound);
    }
    public void playFarmSound()
    {
        MakeSound(farmSound);
    }
    public void playHarborSound()
    {
        MakeSound(harborSound);
    }
    public void playLabSound()
    {
        MakeSound(labSound);
    }
    public void playMineSound()
    {
        MakeSound(mineSound);
    }
    public void playOilPlantSound()
    {
        MakeSound(oilPlantSound);
    }
    public void playPowerPlantSound()
    {
        MakeSound(powerPlantSound);
    }
    public void playSawMilSound()
    {
        MakeSound(sawMillSound);
    }
    public void playSchoolSound()
    {
        MakeSound(schoolSound);
    }
    public void playUpgradeSound()
    {
        MakeSound(upgradeSound);
    }
    public void playWindTurbineSound()
    {
        MakeSound(windTurbineSound);
    }
    public void playExplosionOneSound()
    {
        MakeSound(explosionOneSound);
    }
    public void playBoatSinkSound()
    {
        MakeSound(boatSinkSound);
    }

}
