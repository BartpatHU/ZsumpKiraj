using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public bool whiteSkin;
    public bool yellowSkin;
    public bool redSkin;
    public bool blueSkin;
    public bool greenSkin;
    public bool pinkSkin;
    public bool cyanSkin;

    public float[] position;


    public PlayerData(Move2D move2D, Collectables collectables)
    {
        whiteSkin = collectables.whiteSkinSAVE;
        yellowSkin = collectables.yellowSkinSAVE;
        redSkin = collectables.redSkinSAVE;
        blueSkin = collectables.blueSkinSAVE;
        greenSkin = collectables.greenSkinSAVE;
        pinkSkin = collectables.pinkSkinSAVE;
        cyanSkin = collectables.cyanSkinSAVE;

        position = new float[3];

        position[0] = move2D.transform.position.x;
        position[1] = move2D.transform.position.y;
        position[2] = move2D.transform.position.z;
    }
}
