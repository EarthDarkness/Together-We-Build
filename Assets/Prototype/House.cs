using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : ScriptableObject {

    public float[] floorHeights;
    public GameObject[] floorPrefabs;

    public int MaxHouseFloor
    {
        private set { }
        get
        {
            return floorPrefabs.Length;
        }
    }


    public GameObject CreateFloor(Vector3 pos, int floorIndex)
    {
        return Instantiate(floorPrefabs[floorIndex], pos, Quaternion.identity);
    }
}
