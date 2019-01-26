using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName ="HouseData",menuName ="House Data")]
public class House : ScriptableObject
{
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
        //Change animation
        instantiatedFloors[floorIndex].GetComponent<Animator>().Play("DestroyAnim");
    }

    public GameObject CreateFloor(int floorIndex)
    {
        instantiatedFloors[floorIndex] = Instantiate(floorPrefabs[floorIndex]);
        return instantiatedFloors[floorIndex];
    }
    
}
