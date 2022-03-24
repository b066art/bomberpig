using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject bombPrefab;
    public Sprite[] spriteArray;
    public LayerMask layerMask;
    private Grid grid;
    private Vector3 startPos, targetPos;
    private float timeToMove = 0.2f;
    private bool isMoving = false;
    private bool canPlant = true;

    void Start()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.Abs((grid.WorldToCell(targetPos).y - 6) / 2);
    }

    void Update()
    {
        if(SimpleInput.GetButtonDown("Jump"))
            PlaceBomb();

        if(SimpleInput.GetAxisRaw("Horizontal") == 1 && !isMoving) {
            GetComponent<SpriteRenderer>().sprite = spriteArray[0];
            if(!CheckCollision(Vector3.right))
                StartCoroutine(MovePlayer(Vector3Int.right)); }

        if(SimpleInput.GetAxisRaw("Horizontal") == -1 && !isMoving) {
            GetComponent<SpriteRenderer>().sprite = spriteArray[1];
            if(!CheckCollision(Vector3.left))
                StartCoroutine(MovePlayer(Vector3Int.left)); }

        if(SimpleInput.GetAxisRaw("Vertical") == 1 && !isMoving) {
            GetComponent<SpriteRenderer>().sprite = spriteArray[2];
            if(!CheckCollision(Vector3.up))
                StartCoroutine(MovePlayer(Vector3Int.up)); }
            
        if(SimpleInput.GetAxisRaw("Vertical") == -1 && !isMoving) {
            GetComponent<SpriteRenderer>().sprite = spriteArray[3];
            if(!CheckCollision(Vector3.down))
                StartCoroutine(MovePlayer(Vector3Int.down)); }
    }

    private IEnumerator MovePlayer(Vector3Int direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        
        startPos = transform.position;
        targetPos = grid.GetCellCenterWorld(grid.WorldToCell(transform.position) + direction);

        while(elapsedTime < timeToMove) {
            transform.position = Vector3.Lerp(startPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null; }

        transform.position = targetPos;
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.Abs((grid.WorldToCell(targetPos).y - 6) / 2);
        isMoving = false;
    }

    private bool CheckCollision(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, layerMask);

        if (hit.collider != null)
            return true;
        else
            return false;
    }

    void PlaceBomb()
    {
        if (canPlant && BombsBar.bBar.bombsAmount > 0) {
            GameObject Bomb = Instantiate(bombPrefab, grid.GetCellCenterWorld(grid.WorldToCell(transform.position)) - new Vector3(0.25f, 0, 0), Quaternion.identity);
            Bomb.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
            BombsBar.bBar.RemoveBomb(); }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
            GameManager.gameMng.GameOver();
        if (collider.tag == "Bomb")
            canPlant = false;
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Bomb")
            canPlant = true;
    }
}
