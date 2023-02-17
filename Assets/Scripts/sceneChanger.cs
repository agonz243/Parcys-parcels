using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{

	Scene currentScene;
	string sceneName;

    public float delay = 0.0f;
    public float timer;
    public bool startTimer = false;

    public AudioClip clip;
    public AudioSource source;
    
    private void Start() {
    	currentScene = SceneManager.GetActiveScene();
    }

    void Update(){
        Debug.Log(timer);
        if (startTimer == true){
            // Debug.Log(timer);
            // Debug.Log("Delay: " + delay);
            // LoadNextScene();
            timer += Time.deltaTime;
            if (timer > 1f){
                LoadNextScene();
                timer = 0;
            }
        }
        
    }

    public void Wait(){
        // source.PlayOneShot(clip);
        startTimer = true;
        // Debug.Log("Real delay: " + delay);
    }

    public void LoadNextScene() {

        Debug.Log(currentScene.name);
        if (currentScene.name == "TitleScreen") {
            SceneManager.LoadScene("DogInstructions");
        } else if (currentScene.name == "DogInstructions") {
            SceneManager.LoadScene("DogChaseDemo");
        } else if (currentScene.name == "LoseDogGame" || currentScene.name == "WinDogGame") {
            SceneManager.LoadScene("SprinklerInstructions");
        } else if (currentScene.name == "SprinklerInstructions") {
            SceneManager.LoadScene("SprinklerV2");
        } else if (currentScene.name == "Win" || currentScene.name == "Lose") {
            SceneManager.LoadScene("MailbagInstructions");
        } else if (currentScene.name == "MailbagInstructions") {
            SceneManager.LoadScene("Mailbag");
        } else if (currentScene.name == "LosePuzzleGame" || currentScene.name == "WinPuzzleGame") {
            SceneManager.LoadScene("TitleScreen");
        } else { 
            Debug.Log("Loading nothing :(");
        }
        
    }
}