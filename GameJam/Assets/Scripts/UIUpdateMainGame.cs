using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIUpdateMainGame : MonoBehaviour
{
    private Text score;
	// Use this for initialization
	void Start ()
	{
	    score = GameObject.Find("Score").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    score.text = $"{GameManager.GetCurrentScore():0000000000}";
	}
}
