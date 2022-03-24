using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool isRed = false;

    void Update()
    {
        if(!isRed)
            StartCoroutine(ChangeColor());
        StartCoroutine(TimerToDestroy());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy") {
            DogMovement.dMove.GetDirty();
            BombsBar.bBar.AddBomb();
            Destroy(gameObject); }
    }

    private IEnumerator ChangeColor()
    {
        isRed = true;
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.5f);
        isRed = false;
    }

    private IEnumerator TimerToDestroy()
    {
        yield return new WaitForSeconds(10f);
        BombsBar.bBar.AddBomb();
        Destroy(gameObject);
    }
}
