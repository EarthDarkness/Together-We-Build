using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [Required]
    public BlockManager blockManager;


    //[Required]
    //public GameObject pressInterface, alternateInterface, combinationInterface;

    private string winString = "With everyone cooperation,\nYou did it. \nThis is your home now!";

    private string loseString = "Without cooperation you can't create a sweet home.";


    TextMeshPro endGameText;

    [Required]
	public FloatVariable PuzzleTimer;

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

	public float remainingTime = 60.0f;//0 == lose

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

		StartCoroutine(UpdateTime());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Block") )
        {
            if (other.GetComponent<Rigidbody>().isKinematic || puzzleBlocks.Count <= 0)
            {
                Debug.Log("Finished");
                return;
            }

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
                failedTimer = 0;
				playerPuzzles[0].transform.parent.parent.GetChild(0).gameObject.SetActive(true);
                Debug.Log("Test");
                for (int i = 0; i < players.Count; i++)
                {
                    int randType = Random.Range(0, 3);
                    Debug.Log(i + " : " + players[i].playerData.ID);
                    playerPuzzles[players[i].playerData.playerID].CreatePuzzle((InputPuzzle.PuzzleType)2, players[i].playerData.ID, 4);
                    playerPuzzles[players[i].playerData.playerID].transform.GetChild(2).gameObject.SetActive(true);
                    playerPuzzles[players[i].playerData.playerID].transform.GetChild(2).GetComponentInChildren<ButtonCodeWriter>().SetText(0);


                    playerPanel.transform.GetChild(players[i].playerData.playerID).gameObject.SetActive(true);
					players[i].Build(true);
                }
                StartCoroutine(CheckForPuzzles(playerPuzzles.Take(players.Count).ToList()));
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
                Debug.Log(puzzle.gameObject.name + " : " + puzzle.isComplete);

                if (!puzzle.isComplete)
                {
                    won = false;
                }
            }

            if (won)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        playerPuzzles[0].transform.parent.parent.GetChild(0).gameObject.SetActive(false);
                        playerPuzzles[players[i].playerData.playerID].transform.GetChild(j).gameObject.SetActive(false);
                        playerPanel.transform.GetChild(players[i].playerData.playerID).gameObject.SetActive(false);
                        players[i].Build(false);
                    }
                   
                }

                house.CreateFloor(houseFloor);
                houseFloor++;
                if (houseFloor > house.MaxHouseFloor)
                {
                    endGameText.text = winString;
                    endGameText.gameObject.SetActive(true);

                    Invoke("ChangeScene", 5);
                    break;
                }


                puzzleNumber++;
                for (int i = 0; i < puzzleNumber; i++)
                {
                    puzzleBlocks.Add(blockManager.CreateBlock(transform.position + Vector3.up * 3.5f * houseFloor + Vector3.left * 3f + (Vector3.right * 3.0f) * i,
                            blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
                    puzzleBlocks[i].stoppd = true;
                }
                break;
            }

            if (failedTimer >= failedPuzzleTime)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        playerPuzzles[0].transform.parent.parent.GetChild(0).gameObject.SetActive(false);
                        playerPuzzles[players[i].playerData.playerID].transform.GetChild(j).gameObject.SetActive(false);
                        playerPanel.transform.GetChild(players[i].playerData.playerID).gameObject.SetActive(false);
                        players[i].Build(false);
                    }
                }

                for (int i = 0; i < puzzleNumber; i++)
                {
                    puzzleBlocks.Add(blockManager.CreateBlock(transform.position + Vector3.up * 3.5f * houseFloor + Vector3.left * 3f + (Vector3.right * 3.0f) * i,
                            blockManager.blockData[Random.Range(0, blockManager.blockData.Length)]));
                    puzzleBlocks[i].stoppd = true;
                }
                break;
            }

            failedTimer += Time.deltaTime;
			PuzzleTimer.value = (failedPuzzleTime-failedTimer)/failedPuzzleTime;

            yield return null;
        }

		for (int i = 0; i < players.Count; i++){
			players[i].Build(false);
		}

        yield return null;
    }

	IEnumerator UpdateTime()
	{
		TextMeshProUGUI textCtrl = null;
		GameObject timeText = GameObject.Find("PuzzleCanvas");
		if(timeText){
			textCtrl = timeText.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		}
		while(remainingTime > 0.0f){
			if(textCtrl == null)
				break;

			System.TimeSpan ts = System.TimeSpan.FromSeconds(remainingTime);

			textCtrl.text = ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");

			remainingTime -= Time.deltaTime;

			yield return null;
		}

        endGameText.text = loseString;
        endGameText.gameObject.SetActive(true);

        Invoke("ChangeScene", 5);

		yield return null;
	}

    private void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
