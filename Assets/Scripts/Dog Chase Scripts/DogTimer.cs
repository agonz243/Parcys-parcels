using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DogTimer : MonoBehaviour
{

    public float totalTime = 40;
    private float gameTimer; // timer for entire minigame

    [SerializeField]private TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        gameTimer = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        // TIMER STUFF (USED IN CHARACTER HIDE)
        if(gameTimer > 1){
            gameTimer -= Time.deltaTime;
            float seconds = Mathf.FloorToInt(gameTimer % 60);
            timerText.text = string.Format("{0}", seconds);

            scoreTracker.dogTime = (totalTime % 60) - seconds;
        } else {
            if(SceneManager.GetActiveScene().name == "DogChase") {
                SceneManager.LoadScene("LoseDogGame");
            } else {
                SceneManager.LoadScene("LosePuzzleGame");
            }
        }
    }
}