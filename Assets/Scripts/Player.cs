using System.Collections;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10;
    GameObject nextTile, oldCurTile, currentTile;
    int dice = 0, counter = -1;
    public LayerMask tileLayer;
    public float rayMaxDist;
    bool walking = false;
    public bool isTurn = false;

    void Update()
    {
        /*
         * Roll a dice
         */
        if(Input.GetKeyDown(KeyCode.K))
        {
            isTurn = true;
            dice = UnityEngine.Random.Range(1, 7);
            Debug.Log("Dice thrown: " + dice);
            StartCoroutine(Walk());
        }
    }

    bool shownPointers;
    private void FixedUpdate()
    {
        /* 
         * Check for current tile id
         * with a raycast casting down
         */

        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up,
            out hit, rayMaxDist, tileLayer.value))
        {
            if(hit.collider.tag == "Tile")
            {
                currentTile = hit.collider.gameObject;
                nextTile = hit.collider.gameObject.GetComponent<TileG>().nextTile;
                
            }
            if(hit.collider.tag == "BranchTile")
            {
                hit.collider.gameObject
                        .GetComponent<BranchTileManager>().playerIsHere = true;
                shownPointers = true;
                GameObject managerNT = hit.collider.gameObject
                        .GetComponent<BranchTileManager>().nextTile;
                currentTile = hit.collider.gameObject;

                if (managerNT == currentTile)
                {
                    StartCoroutine(Center());
                }
                else
                {
                    nextTile = managerNT;
                }
            }
            else if (shownPointers)
            {
                GameObject[] branchTiles = GameObject.FindGameObjectsWithTag("BranchTile");
                foreach (GameObject branchTile in branchTiles)
                {
                    branchTile.GetComponent<BranchTileManager>().playerIsHere = false;
                }
                shownPointers = false;
            }
            if (currentTile != oldCurTile)
            {
                counter++;
                oldCurTile = currentTile;
            }
        }
        if (!walking)
        StartCoroutine(Center());
    }
    private void OnGUI()
    {
        /*
         * Draw the raycast for 
         * checking the current id
         */

        Debug.DrawRay(transform.position, -transform.up * rayMaxDist, Color.red);
    }
    IEnumerator Center()
    {
        if (walking) yield return new WaitForEndOfFrame();
        Vector3 target = new Vector3(currentTile.transform.position.x, transform.position.y,
            currentTile.transform.position.z);
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position,target,
                speed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator Walk()
    {
        walking = true;
        while(counter < dice)
        {
            if (nextTile != null)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(nextTile.transform.position.x, transform.position.y,
                    nextTile.transform.position.z),
                    speed * Time.deltaTime);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
        }
        isTurn = false;
        counter = 0;
        walking = false;
    }
}
