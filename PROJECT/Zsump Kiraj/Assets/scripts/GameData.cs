using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float volume;
    public int resolution;
    public int graphics;
    public bool fullscreen;

    public GameData(GameSettings settings)
    {
        volume = settings.volumeSAVE;
        resolution = settings.resolutionSAVE;
        graphics = settings.graphicsSAVE;
        fullscreen = settings.fullscreenSAVE;
}
}
