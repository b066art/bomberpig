using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameMng;
    public GameObject gameOverMenu;
    public GameObject arrows;
    public GameObject button;
    public GameObject playerPrefab;
    public GameObject dogPrefab;
    public GameObject stonePrefab;
    private Grid grid;

    void Awake()
    {
        gameMng = this;
    }    

    void Start()
    {
        Time.timeScale = 1f;
        grid = (Grid)FindObjectOfType(typeof(Grid));
        PlayersToStartPos();
        SpawnStones();
        arrows.SetActive(true);
        button.SetActive(true);
    }

    private void PlayersToStartPos()
    {   
        GameObject Player = Instantiate(playerPrefab, grid.GetCellCenterWorld(new Vector3Int(-9, -1, 0)), Quaternion.identity);
        GameObject Dog = Instantiate(dogPrefab, grid.GetCellCenterWorld(new Vector3Int(7, 3, 0)), Quaternion.identity);
    }

    private void SpawnStones()
    {   
        int layerNum = 1;
        for(int r = 2; r > -6; r -= 2) {
            for(int c = -8; c < 8; c += 2) {
                GameObject Stone = Instantiate(stonePrefab, grid.GetCellCenterWorld(new Vector3Int(c, r, 0)), Quaternion.identity);
                Stone.GetComponent<SpriteRenderer>().sortingOrder = layerNum;
            }
            layerNum += 1;
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        Destroy(GameObject.FindWithTag("Player"));
        Destroy(GameObject.FindWithTag("Enemy"));
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Bomb");
        foreach(GameObject go in gos)
            Destroy(go);
        arrows.SetActive(false);
        button.SetActive(false);
        gameOverMenu.SetActive(true);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
