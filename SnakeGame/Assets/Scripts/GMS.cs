using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class GMS : MonoBehaviour
{
    public Sprite grass,darkgrass;

    private void Start()
    {
        for (int i = -5; i <= 4; i++)
        {
            for (int j = -14; j <= 13; j++)
            {
                GameObject Ground = new GameObject("Grass", typeof(SpriteRenderer));
                Ground.GetComponent<SpriteRenderer>().sprite = grass;
                Ground.transform.position = new Vector3((float)(j + 0.5), (float)(i + 0.5), 1);
            }
        }
        for (int j = -14; j <= 13; j++)
        {
            GameObject Walld = new GameObject("DarkGrass", typeof(SpriteRenderer));
            GameObject Wallu = new GameObject("DarkGrass", typeof(SpriteRenderer));
            Walld.GetComponent<SpriteRenderer>().sprite = darkgrass;
            Wallu.GetComponent<SpriteRenderer>().sprite = darkgrass;
            Walld.transform.position = new Vector3((float)(j + 0.5), (float)(-6 + 0.5), 1);
            Wallu.transform.position = new Vector3((float)(j + 0.5), (float)(5 + 0.5), 1);
        }
        for (int i = -6; i <= 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject Walll = new GameObject("DarkGrass", typeof(SpriteRenderer));
                GameObject Wallr = new GameObject("DarkGrass", typeof(SpriteRenderer));
                Walll.GetComponent<SpriteRenderer>().sprite = darkgrass;
                Wallr.GetComponent<SpriteRenderer>().sprite = darkgrass;
                Walll.transform.position = new Vector3((float)(-15 - j + 0.5), (float)(i + 0.5), 1);
                Wallr.transform.position = new Vector3((float)(14 + j + 0.5), (float)(i + 0.5), 1);
            }
        }
    }
}
