using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Required]
    public BlockManager blockManager;


    //[Required]
    //public GameObject pressInterface, alternateInterface, combinationInterface;

    [Required]
    public GameObject playerPanel;

    public List<Block> puzzleBlocks = new List<Block>();
    public List<Player> players = new List<Player>();
    public House house;

    public int puzzleNumber = 3;
    public float failedPuzzleTime = 5.0f;

    public Vector3 houseStartPos;

    private int houseFloor = 0;
    private float failedTimer = 0.0f;

    List<InputPuzzle> playerPuzzles = new List<InputPuzzle>();


    private void Start()
    {
        for (int i = 0; i < puzzleNumber; i++)
        {
            puzzleBlocks.Add(blockManager.CreateBlock(transform.position + Vector3.up * 8f + Vector3.left * 3f + (Vector3.right * 3.0f) * i,
                    blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
			puzzleBlocks[i].stoppd = true;
        }

        playerPuzzles.AddRange(playerPanel.GetComponentsInChildren<InputPuzzle>(true));

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block"))
        {
            Block b = other.GetComponent<Block>();

            for (int i = 0; i < puzzleBlocks.Count; i++)
            {
                if (puzzleBlocks[i].blockData.ID == b.blockData.ID)
                {
                    Destroy(puzzleBlocks[i].gameObject, .1f);
                    b.timer = 0.0f;
                    puzzleBlocks.RemoveAt(i);
                    break;
                }
            }

            if (puzzleBlocks.Count <= 0)
            {
                Debug.Log("Test");
                for (int i = 0; i < players.Count; i++)
                {
                    int randType = Random.Range(0, 3);
                    playerPuzzles[i].CreatePuzzle((InputPuzzle.PuzzleType)2, 4);
                    playerPuzzles[i].transform.GetChild(randType).gameObject.SetActive(true);
                    playerPuzzles[i].transform.GetChild(randType).GetComponentInChildren<ButtonCodeWriter>().SetText();


                    playerPanel.transform.GetChild(i).gameObject.SetActive(true);
					players[i].Build(true);
                    StartCoroutine(CheckForPuzzles(playerPuzzles.Take(players.Count).ToList()));
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + " Saiu na casa!222");
            //Player p = other.GetComponentInParent<Player>();
        }
    }

    IEnumerator CheckForPuzzles(List<InputPuzzle> inputPuzzles)
    {
        while (true)
        {
            bool won = true;
            foreach (InputPuzzle puzzle in inputPuzzles)
            {
                if (!puzzle.isComplete)
                {
                    won = false;
                }
            }

            if (won)
            {
                //houseFloor++;
                //if (houseFloor >= house.MaxHouseFloor)
                //{
                //    //Finished the game
                //    break;
                //}
                Debug.Log("Venceu!");
                //house.CreateFloor(houseStartPos, houseFloor);
                break;
            }

            if (failedTimer >= failedPuzzleTime)
            {
                //Players losed the puzzle - return a floor
                //house.RemoveFloor(houseFloor);
                Debug.Log("Perdeu!");
                break;
            }

            failedTimer += Time.deltaTime;

            yield return null;
        }

		for (int i = 0; i < players.Count; i++){
			players[i].Build(false);
		}

        yield return null;
    }

}
