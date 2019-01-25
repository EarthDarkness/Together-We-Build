using System.Collections;
using UnityEngine;

public class House : ScriptableObject
{

    public float[] floorHeights;
    public GameObject[] floorPrefabs;
    public GameObject[] floorRoofs;

    float timer = 0;

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

    //IEnumerator GenerateRoof(int roofIndex)
    //{
    //    timer += Time.deltaTime;
    //    //Animation time
    //    if (timer >= 1f)
    //    {
    //        Instantiate(floorRoofs[])
    //    }
    //}
}
