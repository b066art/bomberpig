using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public static DogMovement dMove;
    public Sprite[] spriteArray;
    public LayerMask layerMask;
    public bool isBlown = false;
    private Grid grid;
    private GameObject player;
    private Vector3 startPos, targetPos, newDir, lastDir;
    private Vector3Int playerDir;
    private float timeToMove = 0.2f;
    private int spriteIndex = 0;
    private bool isMoving = false;
    private bool isAngry = false;

    void Awake()
    {
        dMove = this;
    }

    void Start()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
        player = GameObject.FindWithTag("Player");
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.Abs((grid.WorldToCell(targetPos).y - 6) / 2);
    }

    void Update()
    {
        if(!isBlown) {
            playerDir = CheckPlayer();
            if(playerDir == Vector3Int.zero) {
                isAngry = false;
                newDir = MoveDecision(); }
            else {
                isAngry = true;
                newDir = playerDir;
                lastDir = Vector3.one; }}

        if(newDir == Vector3.right && newDir != lastDir && !isMoving && !isBlown) {
            if(isAngry)
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 4];
            else
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex];
            if(!CheckCollision(Vector3.right))
                StartCoroutine(MovePlayer(Vector3Int.right)); 
            lastDir = -newDir; }

        if(newDir == Vector3.left && newDir != lastDir && !isMoving && !isBlown) {
            if(isAngry)
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 5];
            else
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 1];
            if(!CheckCollision(Vector3.left))
                StartCoroutine(MovePlayer(Vector3Int.left));
            lastDir = -newDir; }

        if(newDir == Vector3.up && newDir != lastDir && !isMoving && !isBlown) {
            if(isAngry)
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 6];
            else
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 2];
            if(!CheckCollision(Vector3.up))
                StartCoroutine(MovePlayer(Vector3Int.up));
            lastDir = -newDir; }
            
        if(newDir == Vector3.down && newDir != lastDir && !isMoving && !isBlown) {
            if(isAngry)
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 7];
            else
                GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 3];
            if(!CheckCollision(Vector3.down))
                StartCoroutine(MovePlayer(Vector3Int.down));
            lastDir = -newDir; }
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
        yield return new WaitForSeconds(0.3f);
        isMoving = false;
    }

    private Vector3 MoveDecision()
    {
        List<Vector3> allDirections = new List<Vector3>() { Vector3.right,Vector3.left,Vector3.up,Vector3.down };
        List<Vector3> rightDirections = new List<Vector3>() {};
        foreach(Vector3 dir in allDirections) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, layerMask);
            if (hit.collider == null)
                rightDirections.Add(dir); }
        return rightDirections[Random.Range(0, rightDirections.Count)];
    }

    private Vector3Int CheckPlayer()
    {
        Vector3Int playerDir = Vector3Int.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0.3f, 0, 0), player.transform.position - transform.position - new Vector3(0.1f, 0, 0), Mathf.Infinity, layerMask);
        if(hit.collider != null) {
            if(hit.collider.tag == "Player")
                playerDir = Vector3Int.RoundToInt(Vector3.Normalize(player.transform.position - transform.position)); }
        return playerDir;
    }

    private bool CheckCollision(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, layerMask);
        if (hit.collider != null)
            return true;
        else
            return false;
    }

    public void GetDirty()
    {
        isBlown = true;
        if(newDir == Vector3.right)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 8];
        if(newDir == Vector3.left)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 9];
        if(newDir == Vector3.up)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 10];
        if(newDir == Vector3.down)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 11];
        StartCoroutine(WaitAndClean());
    }

    private IEnumerator WaitAndClean()
    {
        yield return new WaitForSeconds(3f);
        if(newDir == Vector3.right)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex];
        if(newDir == Vector3.left)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 1];
        if(newDir == Vector3.up)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 2];
        if(newDir == Vector3.down)
            GetComponent<SpriteRenderer>().sprite = spriteArray[spriteIndex + 3];
        isBlown = false;
    }
}
