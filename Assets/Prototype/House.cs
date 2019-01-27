using System.Collections;
using UnityEngine;

public class House : ScriptableObject
{

    public float[] floorHeights;
    public GameObject[] floorPrefabs;
    public GameObject[] floorRoofs;

    private GameObject[] instantiatedFloors;

    private void Awake()
    {
        instantiatedFloors = new GameObject[MaxHouseFloor];   
    }

    public int MaxHouseFloor
    {
        private set { }
        get
        {
            return floorPrefabs.Length;
        }
    }

    public void RemoveFloor(int floorIndex)
    {
        instantiatedFloors[floorIndex].GetComponent<Animator>().Play("DestroyAnim");
    }

    public GameObject CreateFloor(Vector3 pos, int floorIndex)
    {
        instantiatedFloors[floorIndex] = Instantiate(floorPrefabs[floorIndex], pos, Quaternion.identity);
        return instantiatedFloors[floorIndex];
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
