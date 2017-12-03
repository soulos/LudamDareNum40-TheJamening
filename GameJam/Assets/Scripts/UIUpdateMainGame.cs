using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIUpdateMainGame : MonoBehaviour
{
    private Text score;

    private Image health;
    private Image alcohol;
    // Use this for initialization
    void Start ()
	{
	    score = GameObject.Find("Score").GetComponent<Text>();
	    health = GameObject.Find("Health").GetComponent<Image>();
	    alcohol = GameObject.Find("Alcohol").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    score.text = $"{GameManager.GetCurrentScore():0000000000}";
	}

    public void UpdateHealthAndAlcoholMeters(float healthValue, float alcoholValue)
    {
        Debug.Log($"Health: {healthValue}  Alcohol: {alcoholValue}");
        health.fillAmount = healthValue;
        alcohol.fillAmount = alcoholValue;
    }
}
