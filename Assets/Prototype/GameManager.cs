using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Required]
    public BlockManager blockManager;
    public List<Block> puzzle = new List<Block>();

    public House house;

    public int puzzleNumber = 3;
    private int houseFloor = 0;
    int playerNumber = 3;
    public float failedPuzzleTime = 5.0f;
    private float failedTimer = 0.0f;

    public Vector3 houseStartPos;

    private void Start()
    {
        for (int i = 0; i < puzzleNumber; i++)
        {
            puzzle.Add(blockManager.CreateBlock(transform.position + Vector3.up * 8f + Vector3.left * 3f + (Vector3.right * 3.0f) * i,
                    blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponentInParent<Player>();
            if (!p.catchBlock)
            {
                return;
            }
            Block currentBlock;
            for (int i = 0; i < puzzle.Count; i++)
            {
                if (puzzle[i].blockData.ID == p.catchBlock.blockData.ID)
                {
                    Destroy(puzzle[i].gameObject, .1f);
                    Destroy(p.catchBlock.gameObject, .1f);
                    puzzle.RemoveAt(i);
                    p.catchBlock = null;
                    break;
                }
            }

            if (puzzle.Count <= 0)
            {
                List<InputPuzzle> playerPuzzles = new List<InputPuzzle>();
                for (int i = 0; i < playerNumber; i++)
                {
                    InputPuzzle puzzle = new GameObject().AddComponent<InputPuzzle>();
                    puzzle.puzzleType = (InputPuzzle.PuzzleType)Random.Range(0, 3);
                    puzzle.RandomizeAlternate();
                    puzzle.RandomizeCombination(3);
                    playerPuzzles.Add(puzzle);
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
                houseFloor++;
                if (houseFloor >= house.MaxHouseFloor)
                {
                    //Finished the game
                    break;
                }

                house.CreateFloor(houseStartPos, houseFloor);
                break;
            }

            if (failedTimer >= failedPuzzleTime)
            {
                //Players losed the puzzle - return a floor
                break;
            }

            failedTimer += Time.deltaTime;

            yield return null;
        }
    }

}
