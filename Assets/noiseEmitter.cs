using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noiseEmitter : MonoBehaviour {

    //Scrapped for now
    //Noise only wanted when shooting

    private float noiseValue; //Total distance the object can be heard from
    private float terrainNoise; //Leaves, grass, rocks
    private float actionNoise; //Shooting, running, crafting
    private float weatherCover; //Noise muffling depending on rain, sunshine, wind

    private void updateNoiseValue()
    {
        noiseValue = terrainNoise + actionNoise + weatherCover;
    }

    public float getNoiseValue()
    {
        return noiseValue;
    }

    public void setTerrainNoise(float distance)
    {
        terrainNoise = distance;
        updateNoiseValue();
    }

    public void setActionNoise(float distance)
    {
        terrainNoise = distance;
        updateNoiseValue();
    }

    public void setWeatherCover(float distance)
    {
        terrainNoise = distance;
        updateNoiseValue();
    }
}
