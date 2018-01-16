using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pacman : MonoBehaviour {
    public float speed = 4f;
    private Rigidbody2D rb;

    public Sprite pausedSprite;

    public static bool PausePacman = false;

    int highScore;

    int lives = 3;

    SoundManager soundManager;
    GameBoard gameBoard;

    Ghost redghost;
    Ghost pinkGhost;
    Ghost lightBlueGhost;
    Ghost orangeGhost;
    int dotCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameBoard = FindObjectOfType(typeof(GameBoard)) as GameBoard;
        
        redghost = (Ghost)GameObject.Find("redGhost").GetComponent(typeof(Ghost));
        pinkGhost = (Ghost)GameObject.Find("pinkGhost").GetComponent(typeof(Ghost));
        lightBlueGhost = (Ghost)GameObject.Find("lightBlueGhost").GetComponent(typeof(Ghost));
        orangeGhost = (Ghost)GameObject.Find("orangeGhost").GetComponent(typeof(Ghost));

    }
    // Use this for initialization
    void Start () {
        rb.velocity = new Vector2(-1, 0) * speed ;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        getHighScore();
        dotCount = GameObject.FindGameObjectsWithTag("Dot").Length + 4;
	}

    void MovePacman(float horM, float VertM, int localScale1, int localScale2, float eulerValue3)
    {
        Vector2 moveVect = new Vector2(horM, VertM);
        transform.position = new Vector2((int) transform.position.x + .5f, (int)transform.position.y + .5f);
        rb.velocity = moveVect * speed;
        transform.localScale = new Vector2(localScale1, localScale2);
        transform.localRotation = Quaternion.Euler(0, 0, eulerValue3);
    }

    void MoveAfterEaten()
    {
        rb.velocity = new Vector2(-1, 0) * speed;
    }

    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        Vector2 moveVect;
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        if (PausePacman)
        {
            rb.velocity = Vector2.zero;

        }
            if (Input.GetKeyDown("a") && !PausePacman)
            {
                if (localVelocity.x > 0 && gameBoard.isValidSpace(transform.position.x - 1, transform.position.y))
                {
                    MovePacman(horizontalMovement, 0, 1, 1, 0);
                }
                else
                {
                    moveVect = new Vector2(horizontalMovement, 0);

                    if (ValidPosition(moveVect))
                    {
                        MovePacman(horizontalMovement, 0, 1, 1, 0);
                    }
                }



            }
            else if (Input.GetKeyDown("d") && !PausePacman)
            {
                if (localVelocity.x < 0 && gameBoard.isValidSpace(transform.position.x + 1, transform.position.y))
                {
                    MovePacman(horizontalMovement, 0, -1, 1, 0);
                }
                else
                {
                    moveVect = new Vector2(horizontalMovement, 0);
                    if (ValidPosition(moveVect))
                    {
                        MovePacman(horizontalMovement, 0, -1, 1, 0);
                    }
                }


            }
            else if (Input.GetKeyDown("w") && !PausePacman)
            {
                if (localVelocity.y > 0 && gameBoard.isValidSpace(transform.position.x, transform.position.y + 1))
                {
                    MovePacman(0, verticalMovement, 1, 1, 270);
                }
                else
                {
                    moveVect = new Vector2(0, verticalMovement);
                    if (ValidPosition(moveVect))
                    {
                        MovePacman(0, verticalMovement, 1, 1, 270);
                    }
                }


            }
            else if (Input.GetKeyDown("s")&& !PausePacman)
            {
                if (localVelocity.y < 0 && gameBoard.isValidSpace(transform.position.x, transform.position.y - 1))
                {
                    MovePacman(0, verticalMovement, 1, 1, 90);
                }
                else
                {
                    moveVect = new Vector2(0, verticalMovement);
                    if (ValidPosition(moveVect))
                    {
                        MovePacman(0, verticalMovement, 1, 1, 90);
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
            PillEaten(col);
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
                else if(!redghost.isGhostBlue)
                {
                    pacmanDied(audioSource);
                }
            }else if (ghostName == "pinkGhost")
            {
                if (pinkGhost.isGhostBlue)
                {
                    pinkGhost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else if(!pinkGhost.isGhostBlue)
                {
                    pacmanDied(audioSource);
                }
            }else if (ghostName == "lightBlueGhost")
            {
                if (lightBlueGhost.isGhostBlue)
                {
                    lightBlueGhost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else if(!lightBlueGhost.isGhostBlue)
                {
                    pacmanDied(audioSource);
                }
            }else if(ghostName == "orangeGhost")
            {
                if (orangeGhost.isGhostBlue)
                {
                    orangeGhost.ResetGhostAfterEaten(gameObject);
                    SoundManager.instance.PlayOneShot(SoundManager.instance.eatingGhost);
                    IncreaseTextUIScore(400);
                }
                else if (!orangeGhost.isGhostBlue)
                {
                    pacmanDied(audioSource);
                }
            }
        }
    }

    void PillEaten(Collider2D col)
    {
        SoundManager.instance.PlayOneShot(SoundManager.instance.powerUpEating);
        redghost.TurnGhostBlue();
        pinkGhost.TurnGhostBlue();
        lightBlueGhost.TurnGhostBlue();
        orangeGhost.TurnGhostBlue();

        IncreaseTextUIScore(50);

        Destroy(col.gameObject);
        dotCount--;
    }

    void pacmanDied(AudioSource audioSource)
    {
        if (lives == 0)
        {
            audioSource.Stop();
            Destroy(GameObject.Find("Life1"));
            gameOver();
        }
        else
        {
            Debug.Log("Lives:" + lives);
            SoundManager.instance.PlayOneShot(SoundManager.instance.pacmanDies);
            audioSource.Stop();
            lives--;
            if (lives == 2)
            {
                Destroy(GameObject.Find("Life3"));
            }else if (lives == 1)
            {
                Destroy(GameObject.Find("Life2"));
            }
            
            transform.position = new Vector2(13.5f, 6.5f);
            rb.velocity = Vector2.zero;
            redghost.ResetGhost();
            pinkGhost.ResetGhost();
            lightBlueGhost.ResetGhost();
            orangeGhost.ResetGhost();
            Invoke("MoveAfterEaten", 2f);
        }
    }
   
    void getHighScore()
    {
        String textUIComp = PlayerPrefs.GetInt("HighScore").ToString();
        highScore = int.Parse(textUIComp);
    }

    void gameOver()
    {
        Debug.Log("GAME OVER");
        var textUIComp = GameObject.Find("Score").GetComponent<Text>();
        int score = int.Parse(textUIComp.text);
        if(score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        Destroy(gameObject);
        Destroy(redghost);
        Destroy(orangeGhost);
        Destroy(pinkGhost);
        Destroy(lightBlueGhost);
        SceneManager.LoadScene("GameOver");
    }
    

    void DotEaten(Collider2D col) {
        IncreaseTextUIScore(10);
        dotCount--;
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
        }
        else {
            GetComponent<Animator>().enabled = true;
        }
    }
    
}
