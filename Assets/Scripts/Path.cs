using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // added for game timer text
using UnityEngine.SceneManagement;

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] Points; // array to hold points

    [SerializeField]private float moveSpeed; // how fast the character moves

    [SerializeField]private float hideTimer = 2; // how long you can hide for

    [SerializeField]private float gameTimer = 180; // timer for entire minigame

    [SerializeField]private GameObject brella; // temp for umbrella?

    [SerializeField]private Text timerText;

    private float keyAlt = 0; // used to check for alternate key press

    private int pointIndex; // for counting the points in the array

    private bool hide = false; // used to check if character is hidden

    private bool timerisRunning = false; // used to start and stop the timer

    private bool hit = false; // used to check if character has been hit

    private Rigidbody2D rb;


    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = Points[pointIndex].transform.position; // starting the game goes to the first point
        brella.SetActive(false); // initially disable the umbrella
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // WHILE THERE ARE STILL POINTS TO GET TOWARDS
        if(pointIndex <= Points.Length - 1){

            // TIMER STUFF (USED IN CHARACTER HIDE)
            if(gameTimer > 0){
                gameTimer -= Time.deltaTime;
                float seconds = Mathf.FloorToInt(gameTimer % 60);
                timerText.text = string.Format("{0}", seconds);
            } else{
                SceneManager.LoadScene("Win");
                Debug.Log("Testing switchin scenes");
            }

            // CHARACTER IS HIDING

            if(hide && timerisRunning){ // character is hidden
                if(hideTimer > 0){ // there is still time to hide
                    hideTimer -= Time.deltaTime; // subtract from the time
                    brella.transform.position = new Vector3(transform.position[0], transform.position[1], 0); // set umbrella position to player position
                    rb.isKinematic = true; // disables collisions & applied forces for player
                } else{ // otherwise the timer is out
                    hide = false; // set hidden to false
                    timerisRunning = false; // stop the timer
                    hideTimer = 2; // reset the timer
                    rb.isKinematic = false; // enables collisions & applie dforces for player
                    brella.SetActive(false); // disable the umbrella
                }
            }

            // CHARACTER MOVEMENT

            if(!hide && !hit){
                // Character moves when alternating the "A" & "D" keys
                //       - requires the keyAlt variable
                //Position if moved when alternating the keys "A" and "D"
                if(Input.GetKeyDown(KeyCode.A) && keyAlt == 0){
                    transform.position = Vector2.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed);
                    keyAlt = 1;
                } else if(Input.GetKeyDown(KeyCode.D) && keyAlt == 1){
                    transform.position = Vector2.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed);
                    keyAlt = 0;
                }

                // Character moves when presing the "space" key
                //   - requires the keyAlt variable
                // if(Input.GetKeyDown("space")){
                //     transform.position = Vector2.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed);
                // }
            }

            // Character moves continuously - Time.deltaTime (consistent movement regardless of frame rate)
            // transform.position = Vector2.MoveTowards(transform.position, Points[pointIndex].transform.position, moveSpeed * Time.deltaTime); 
            

            // CHARACTER HIDE / SHIELD INPUT

            if(Input.GetKeyDown(KeyCode.DownArrow)){
                // GameObject.Find("brella").transform.position = transform.position(transform.position[0], transform.position[1], 0); // might break the code
                hide = true;
                timerisRunning = true;
                brella.SetActive(true);
            }

            // HITTING A POINT MOVES THE INDEX TO THE NEXT ONE

            if(transform.position == Points[pointIndex].transform.position){
                pointIndex += 1; // change to the next point of travel
                Debug.Log("Point Index: " + pointIndex);
                // Debug.Log("Point Index - 1: " + (pointIndex - 1));
            }
            
            // CHARACTER GOT HIT

            if(hit){
                rb.isKinematic = true;
                moveSpeed = 5F;
                transform.position = Vector2.MoveTowards(transform.position, Points[pointIndex - 1].transform.position, moveSpeed * Time.deltaTime);
                if(transform.position == Points[pointIndex - 1].transform.position){
                    hit = false;
                    rb.isKinematic = false;
                    moveSpeed = 0.15F;
                }
            }
        }else{
            SceneManager.LoadScene("Win Scene");
        }
    }
    // COLLISION W/ SPRINKLERS
    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Enemy" && !hide){
            hit = true;
        }
    }


    
}

