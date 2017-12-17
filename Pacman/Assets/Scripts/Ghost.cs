using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Ghost : MonoBehaviour {

    public float speed = 4f;
    private Rigidbody2D rb;
    public Sprite blueGhost;
    public Sprite lookLeftSprite;
    public Sprite lookRightSprite;
    public Sprite lookDownSprite;
    public Sprite lookUpSprite;

    bool isGhostBlue = false;

    public SpriteRenderer sr;

    Vector2[] destinations = new Vector2[] {
        new Vector2(1,29), new Vector2(26,29),new Vector2(26,1),
        new Vector2(1,1), new Vector2(6,16)
    };

    public int destinationIndex;
    Vector2 moveVect;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

	// Use this for initialization
	void Start () {
        float xDest = destinations[destinationIndex].x;
        if (transform.position.x > xDest)
        {
            rb.velocity = new Vector2(-1, 0) * speed;

        }
        else {
            rb.velocity = new Vector2(1, 0) * speed;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Point") {
            moveVect = GetNewDirection(collision.transform.position);
            transform.position = new Vector2((int)collision.transform.position.x + .5f, (int)collision.transform.position.y + .5f);

            if (moveVect.x != 2) {
                if(moveVect == Vector2.right && !isGhostBlue) 
                {
                    sr.sprite = lookRightSprite;
                }else if(moveVect == Vector2.left && !isGhostBlue)
                {
                    sr.sprite = lookLeftSprite;
                }else if(moveVect == Vector2.up && !isGhostBlue)
                {
                    sr.sprite = lookUpSprite;
                }else if(moveVect == Vector2.down && !isGhostBlue)
                {
                    sr.sprite = lookDownSprite;
                }
                rb.velocity = moveVect * speed;
            }
        }
        Vector2 ghostMoveVect = new Vector2(0, 0);
        if (transform.position.x < 2 && transform.position.y==15.5)
        {
            transform.position = new Vector2(24.5f, 15.5f);
            ghostMoveVect = new Vector2(-1, 0);
            rb.velocity = ghostMoveVect * speed;
        }else if(transform.position.x > 25 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(2f, 15.5f);
            ghostMoveVect = new Vector2(1, 0);
            rb.velocity = ghostMoveVect * speed;
        }
    }

    public void TurnGhostBlue()
    {
        StartCoroutine(TurnGhostBlueAndBack());
    }

    IEnumerator TurnGhostBlueAndBack()
    {
        isGhostBlue = true;
        sr.sprite = blueGhost;
        yield return new WaitForSeconds(6.0f);
        isGhostBlue = false;
    }

    

    Vector2 GetNewDirection(Vector2 pointVect) {
        float xPos = (float)Math.Floor(Convert.ToDouble(transform.position.x));
        float yPos = (float)Math.Floor(Convert.ToDouble(transform.position.y));

        pointVect.x = (float)Math.Floor(Convert.ToDouble(pointVect.x));

        pointVect.y = (float)Math.Floor(Convert.ToDouble(pointVect.y));

        Vector2 dest = destinations[destinationIndex];

        if(((pointVect.x + 1) == dest.x) && ((pointVect.y +1) == dest.y))
        {
            destinationIndex = (destinationIndex == 4) ? 0 : destinationIndex + 1;
            Debug.Log("New Destionation " + destinations[destinationIndex]);
        }
        dest = destinations[destinationIndex];
        Vector2 newDir = new Vector2(2, 0);
        Vector2 prevDir = rb.velocity.normalized;

        Vector2 opPrevDir = prevDir * -1;

        Vector2 goRight = new Vector2(1, 0);
        Vector2 goLeft = new Vector2(-1, 0);
        Vector2 goUp = new Vector2(0, 1);
        Vector2 goDown = new Vector2(0, -1);

        float destXDist = dest.x - xPos;
        float destYDist = dest.y - yPos;

        Debug.Log("Get new Direction");
        Debug.Log("x pos "+ xPos);
        Debug.Log("y pos " + yPos);
        Debug.Log("dest x pos " + dest.x);
        Debug.Log(" dest y pos " + dest.y);
        Debug.Log("point x pos" + pointVect.x);
        Debug.Log(" point y pos " + pointVect.y);

        // Upper Left

        // Keeps Ghost from going toward the portal
        if (destYDist > 0 && destXDist < 0)
        {

            if (pointVect.x == 5 && pointVect.y == 15)
            {

                if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }

                // Pick Up or Left depending whether I'm closest to
                // the X or Y
            }
            else if (destYDist > destXDist)
            {

                if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }


            }
            else if (destYDist < destXDist)
            {

                if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }

        }

        // Upper Right

        if (destYDist > 0 && destXDist > 0)
        {

            if (destYDist > destXDist)
            {

                if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition (goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }
            else if (destYDist < destXDist)
            {

                if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }

        }

        // Lower Right

        if (destYDist < 0 && destXDist > 0)
        {

            if (destYDist > destXDist)
            {

                if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }
            else if (destYDist < destXDist)
            {

                if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }

        }

        // Lower Left

        if (destYDist < 0 && destXDist < 0)
        {

            if (destYDist > destXDist)
            {

                if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }
            else if (destYDist < destXDist)
            {

                if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
                {
                    newDir = goDown;
                }
                else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
                {
                    newDir = goLeft;
                }
                else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
                {
                    newDir = goRight;
                }
                else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
                {
                    newDir = goUp;
                }
                else if (ValidPosition(opPrevDir, pointVect))
                {
                    newDir = opPrevDir;
                }

            }

        }

        // Ys Equal and Want to go Right
        // Done because the above don't test for if Xs & Ys are equal

        if ((int)(dest.y) == (int)(yPos)
            && destXDist > 0)
        {

            Debug.Log("5");

            if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
            {
                newDir = goRight;
            }
            else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
            {
                newDir = goUp;
            }
            else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
            {
                newDir = goDown;
            }
            else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
            {
                newDir = goLeft;
            }

        }

        // Ys Equal and Want to go Left

        if ((int)(dest.y) == (int)(yPos)
            && destXDist < 0)
        {

            Debug.Log("6");

            if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
            {
                newDir = goLeft;
            }
            else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
            {
                newDir = goUp;
            }
            else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
            {
                newDir = goDown;
            }
            else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
            {
                newDir = goRight;
            }

        }

        // Xs Equal and Want to go Up

        if ((int)(dest.x) == (int)(xPos)
            && destYDist > 0)
        {

            Debug.Log("7");

            if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
            {
                newDir = goUp;
            }
            else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
            {
                newDir = goRight;
            }
            else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
            {
                newDir = goLeft;
            }
            else if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
            {
                newDir = goDown;
            }
        }


        // Xs Equal and Want to go Down

        if ((int)(dest.x) == (int)(xPos)
            && destYDist < 0)
        {

            Debug.Log("8");

            if (ValidPosition(goDown, pointVect) && goDown != opPrevDir)
            {
                newDir = goDown;
            }
            else if (ValidPosition(goRight, pointVect) && goRight != opPrevDir)
            {
                newDir = goRight;
            }
            else if (ValidPosition(goLeft, pointVect) && goLeft != opPrevDir)
            {
                newDir = goLeft;
            }
            else if (ValidPosition(goUp, pointVect) && goUp != opPrevDir)
            {
                newDir = goUp;
            }

        }
        return newDir;
    }

    bool ValidPosition(Vector2 dir, Vector2 pointVect)
    {
        Vector2 pos = transform.position;
        Transform point = GameObject.Find("GridGB").GetComponent<GameBoard>().gBPoints[(int)pointVect.x, (int)pointVect.y];
        if (point != null)
        {
            GameObject pointGO = point.gameObject;

            Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint>().vectToNextPoint;

            Debug.Log("Checking Vects " + dir);

            foreach (Vector2 vect in vectToNextPoint)
            {
                Debug.Log("Check " + vect);
                if (vect == dir)
                {
                    return true;
                }
            }

        }
        return false;
    }
}
