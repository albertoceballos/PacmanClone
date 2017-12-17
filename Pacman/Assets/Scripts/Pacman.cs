using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Pacman : MonoBehaviour {
    public float speed = 4f;
    private Rigidbody2D rb;

    public Sprite pausedSprite;

    int lives = 3;

    SoundManager soundManager;

    GameBoard gameBoard;

    Ghost redghost;
    Ghost pinkGhost;
    Ghost lightBlueGhost;
    Ghost orangeGhost;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        gameBoard = FindObjectOfType(typeof(GameBoard)) as GameBoard;

        GameObject redGhostGO = GameObject.Find("redGhost");
        GameObject pinkGhostGO = GameObject.Find("pinkGhost");
        GameObject lightBlueGhostGO = GameObject.Find("lightBlueGhost");
        GameObject orangeGhostGO = GameObject.Find("orangeGhost");

        redghost = (Ghost)redGhostGO.GetComponent(typeof(Ghost));
        pinkGhost = (Ghost)pinkGhostGO.GetComponent(typeof(Ghost));
        lightBlueGhost = (Ghost)lightBlueGhostGO.GetComponent(typeof(Ghost));
        orangeGhost = (Ghost)orangeGhostGO.GetComponent(typeof(Ghost));

    }
    // Use this for initialization
    void Start () {
       rb.velocity = new Vector2(-1, 0) * speed ;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
	}

    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        Vector2 moveVect;
        var localVelocity = transform.InverseTransformDirection(rb.velocity);

        if (Input.GetKeyDown("a"))
        {
            if (localVelocity.x > 0 && gameBoard.isValidSpace(transform.position.x -1,transform.position.y))
            {
                moveVect = new Vector2(horizontalMovement, 0);
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                rb.velocity = moveVect * speed;
                transform.localScale = new Vector2(1, 1);
                transform.localRotation = Quaternion.Euler(0,0,0);
            }
            else {
                moveVect = new Vector2(horizontalMovement, 0);

                if (ValidPosition(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                    rb.velocity = moveVect * speed;
                    transform.localScale = new Vector2(1, 1);
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            

            
        }
        else if (Input.GetKeyDown("d"))
        {
            if (localVelocity.x < 0 && gameBoard.isValidSpace(transform.position.x + 1, transform.position.y))
            {
                moveVect = new Vector2(horizontalMovement, 0);
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                rb.velocity = moveVect * speed;
                transform.localScale = new Vector2(-1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else {
                moveVect = new Vector2(horizontalMovement, 0);
                if (ValidPosition(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                    rb.velocity = moveVect * speed;
                    transform.localScale = new Vector2(-1, 1);
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            
            
        }
        else if (Input.GetKeyDown("w"))
        {
            if (localVelocity.y > 0 && gameBoard.isValidSpace(transform.position.x, transform.position.y +1))
            {
                moveVect = new Vector2(0, verticalMovement);
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                rb.velocity = moveVect * speed;
                transform.localScale = new Vector2(1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else {
                moveVect = new Vector2(0, verticalMovement);
                if (ValidPosition(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                    rb.velocity = moveVect * speed;
                    transform.localScale = new Vector2(1, 1);
                    transform.localRotation = Quaternion.Euler(0, 0, 270);
                }
            }
            
           
        }
        else if (Input.GetKeyDown("s")) {
            if (localVelocity.y < 0 && gameBoard.isValidSpace(transform.position.x, transform.position.y -1))
            {
                moveVect = new Vector2(0, verticalMovement);
                transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                rb.velocity = moveVect * speed;
                transform.localScale = new Vector2(1, 1);
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else {
                moveVect = new Vector2(0, verticalMovement);
                if (ValidPosition(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + .5f, (int)transform.position.y + .5f);
                    rb.velocity = moveVect * speed;
                    transform.localScale = new Vector2(1, 1);
                    transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }
            
            
        }
        UpdateEatingAnimation();
    }

    bool ValidPosition(Vector2 dir) {
        Vector2 pos = transform.position;
        Transform point = GameObject.Find("GridGB").GetComponent<GameBoard>().gBPoints[(int)pos.x, (int)pos.y];
        if (point != null) {
            GameObject pointGO = point.gameObject;

            Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint>().vectToNextPoint;

            foreach (Vector2 vect in vectToNextPoint) {
                if (vect == dir) {
                    return true;
                }
            }
            
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D col) {
        bool hitAWall = false;
        if (col.gameObject.tag == "Point") {
            Vector2[] vectToNextPoint = col.GetComponent<TurningPoint>().vectToNextPoint;

            if (Array.Exists(vectToNextPoint, element => element == rb.velocity.normalized)) {
                hitAWall = false;
            }
            else
            {
                hitAWall = true;
            }
            
            transform.position = new Vector2((int)col.transform.position.x + .4f, (int)col.transform.position.y +.5f);
            if (hitAWall) {
                rb.velocity = Vector2.zero;
            }
        }

        Vector2 pmMoveVect = new Vector2(0, 0);

        if (transform.position.x < 2 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(24.5f, 15.5f);
            pmMoveVect = new Vector2(-1, 0);
            rb.velocity = pmMoveVect * speed;
        }
        else if (transform.position.x > 25 && transform.position.y == 15.5) {
            transform.position = new Vector2(2f, 15.5f);
            pmMoveVect = new Vector2(1, 0);
            rb.velocity = pmMoveVect * speed;
        }

        if (col.gameObject.tag == "Dot") {
            DotEaten(col);
        }

        if(col.gameObject.tag == "Pill")
        {
            SoundManager.instance.PlayOneShot(SoundManager.instance.powerUpEating);
            redghost.TurnGhostBlue();
            pinkGhost.TurnGhostBlue();
            lightBlueGhost.TurnGhostBlue();
            orangeGhost.TurnGhostBlue();

            IncreaseTextUIScore(50);

            Destroy(col.gameObject);
        }

        if(col.gameObject.tag == "Ghost")
        {
            String ghostName = col.GetComponent<Collider2D>().gameObject.name;

            AudioSource audioSource = soundManager.GetComponent<AudioSource>();

            if (ghostName == "redGhost")
            {
                if (redghost.isGhostBlue)
                {
                    redghost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else
                {
                    SoundManager.instance.PlayOneShot(SoundManager.instance.pacmanDies);

                    audioSource.Stop();
                    Destroy(gameObject);
                }
            }else if (ghostName == "pinkGhost")
            {
                if (pinkGhost.isGhostBlue)
                {
                    pinkGhost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else
                {
                    SoundManager.instance.PlayOneShot(SoundManager.instance.pacmanDies);

                    audioSource.Stop();
                    Destroy(gameObject);
                }
            }else if (ghostName == "lightBlueGhost")
            {
                if (lightBlueGhost.isGhostBlue)
                {
                    lightBlueGhost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else
                {
                    SoundManager.instance.PlayOneShot(SoundManager.instance.pacmanDies);

                    audioSource.Stop();
                    Destroy(gameObject);
                }
            }else if(ghostName == "orangeGhost")
            {
                if (orangeGhost.isGhostBlue)
                {
                    orangeGhost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else
                {
                    pacmanDied(audioSource);
                }
            }
        }
    }

    void pacmanDied(AudioSource audioSource)
    {
        if (lives == 0)
        {
           //to be created
        }
        else
        {
            SoundManager.instance.PlayOneShot(SoundManager.instance.pacmanDies);
            audioSource.Stop();
            Destroy(gameObject);
            lives--;
            respawnPacman();
        }
    }

    void respawnPacman()
    {
        Instantiate(gameObject);
    }

    void DotEaten(Collider2D col) {
        IncreaseTextUIScore(10);

        Destroy(col.gameObject);
    }

    void IncreaseTextUIScore(int points) {
        Text textUIComp = GameObject.Find("Score").GetComponent<Text>();
        int score = int.Parse(textUIComp.text);

        score += points;

        textUIComp.text = score.ToString();
    }

    void UpdateEatingAnimation() {
        if (rb.velocity == Vector2.zero)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = pausedSprite;

            soundManager.PausePacman();
        }
        else {
            GetComponent<Animator>().enabled = true;
            soundManager.ResumePacman();
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
