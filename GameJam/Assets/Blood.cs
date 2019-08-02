using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    private bool fading = false;

    public float fadeSpeed = 1f;

    public SpriteRenderer sprite;
	// Use this for initialization
	void Start ()
	{
	    sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (isActiveAndEnabled)
	    {
            fading = true;
	    }
	    if (sprite != null)
	    {
	        
            sprite.color = new Color(sprite.color.r, sprite.color.r, sprite.color.r, sprite.color.a - (fadeSpeed * Time.deltaTime));
	        if (sprite.color.a <= 0)
	        {
	            SetFadeFalse();
                ObjectPoolingManager.DestroyPooledObject("Blood1", transform);
	            sprite.color = new Color(sprite.color.r, sprite.color.r, sprite.color.r, 1);
            }

        }
		
	}

    public void SetFadeFalse()
    {
        fading = false;

    }
   
}
