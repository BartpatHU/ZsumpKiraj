using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using System;
public class Collectables : MonoBehaviour
{
    public GameObject player;

    public LayerMask yellowMask;
    public LayerMask redMask;
    public LayerMask blueMask;
    public LayerMask whiteMask;
    public LayerMask pinkMask;
    public LayerMask cyanMask;
    public LayerMask greenMask;

    public GameObject yellowCoin;
    public GameObject redCoin;
    public GameObject blueCoin;
    public GameObject cyanCoin;
    public GameObject whiteCoin;
    public GameObject pinkCoin;
    public GameObject greenCoin;

    public GameObject yellowSkin;
    public GameObject redSkin;
    public GameObject blueSkin;
    public GameObject cyanSkin;
    public GameObject whiteSkin;
    public GameObject pinkSkin;
    public GameObject greenSkin;

    bool yellowOverlap;
    bool redOverlap;
    bool whiteOverlap;
    bool blueOverlap;
    bool cyanOverlap;
    bool pinkOverlap;
    bool greenOverlap;

    public bool whiteSkinSAVE = false;
    public bool yellowSkinSAVE = false;
    public bool redSkinSAVE = false;
    public bool blueSkinSAVE = false;
    public bool greenSkinSAVE = false;
    public bool pinkSkinSAVE = false;
    public bool cyanSkinSAVE = false;

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        whiteSkinSAVE = data.whiteSkin;
        yellowSkinSAVE = data.yellowSkin;
        redSkinSAVE = data.redSkin;
        blueSkinSAVE = data.blueSkin;
        greenSkinSAVE = data.greenSkin;
        pinkSkinSAVE = data.pinkSkin;
        cyanSkinSAVE = data.cyanSkin;
    }

    // Start is called before the first frame update
    void Start()
    {
        yellowOverlap = false;
        greenOverlap = false;
        pinkOverlap = false;
        cyanOverlap = false;
        blueOverlap = false;
        whiteOverlap = false;
        redOverlap = false;
    }

    // Update is called once per frame
    void Update()
    {

        yellowOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, yellowMask);
        redOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, redMask);
        blueOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, blueMask);
        whiteOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, whiteMask);
        cyanOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, cyanMask);
        pinkOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, pinkMask);
        greenOverlap = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 0.6f), new Vector2(1.2f, 1.7f), 0f, greenMask);


        if (yellowOverlap || yellowSkinSAVE)
        {
            yellowCoin.SetActive(false);
            yellowSkin.SetActive(true);
            yellowSkinSAVE = true;
        }
        if (redOverlap || redSkinSAVE)
        {
            redCoin.SetActive(false);
            redSkin.SetActive(true);
            redSkinSAVE = true;
        }
        if (blueOverlap || blueSkinSAVE)
        {
            blueCoin.SetActive(false);
            blueSkin.SetActive(true);
            blueSkinSAVE = true;
        }
        if (whiteOverlap || whiteSkinSAVE)
        {
            whiteCoin.SetActive(false);
            whiteSkin.SetActive(true);
            whiteSkinSAVE = true;
        }
        if (cyanOverlap || cyanSkinSAVE)
        {
            cyanCoin.SetActive(false);
            cyanSkin.SetActive(true);
            cyanSkinSAVE = true;
        }
        if (pinkOverlap || pinkSkinSAVE)
        {
            pinkCoin.SetActive(false);
            pinkSkin.SetActive(true);
            pinkSkinSAVE = true;
        }
        if (greenOverlap || greenSkinSAVE)
        {
            greenCoin.SetActive(false);
            greenSkin.SetActive(true);
            greenSkinSAVE = true;
        }
    }
}
