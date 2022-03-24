using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombsBar : MonoBehaviour
{
    public static BombsBar bBar;
    public int bombsAmount = 3;
    List<GameObject> bombs = new List<GameObject>();

    void Awake()
    {
        bBar = this;
    }

    void Start()
    {
        for(int i = 0; i < this.transform.childCount; i++)
            bombs.Add(this.transform.GetChild(i).gameObject);
        for(int i = 0; i < bombsAmount; i++)
            bombs[i].SetActive(true);
    }

    public void AddBomb()
    {
        bombs[bombsAmount].SetActive(true);
        bombsAmount += 1;
    }    

    public void RemoveBomb()
    {
        bombsAmount -= 1;
        bombs[bombsAmount].SetActive(false);
        if(bombsAmount == 0)
            ScreenText.screenTxt.Full();
    }
}
