using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dragNdrop2D : MonoBehaviour
{
    
    //taking instance of grid class, in order to access tile positioning for shape "snapping"
    public gridManager gridCoords;

    public List<Transform> snapPoints = new List<Transform>();

    //float for where to snap for grid squares
    public float snapRange = 10.0f;

    //defining for later use
    public GameObject selectedObject;
    public SpriteRenderer spriteRen;
    public Collider2D collidah;
    Vector3 offset;
    public bool isRotating;

    public bool PckgLap;
    public bool Bounding; 

    //here keeping track of values for testing (OLD VALUES)
    public Vector3 targetPos;
    public float gridSize = 10f;
        
    public int gridWidth = 104;
    public int gridHeight = 72;

    // Audio Sources
    public AudioSource clickSource;
    public AudioSource dropSource;

    private Scene currentScene;


    //ROTATE FUNCTION Using game time and Slerp NEEDS TWEAKING FOR FIX
    IEnumerator RotateMe(GameObject pckg, Vector3 byAngles, float inTime)
    {
        var fromAngle = pckg.transform.rotation;
        var toAngle = Quaternion.Euler(pckg.transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            pckg.transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t*2);
            // yield return null;
        }

        yield return null;
    }

    //setting X and Y values 
    public Vector3 setXY(Vector3 pckg, float x, float y)
    {
        pckg.x = x;
        pckg.y = y;
        return pckg;
    }

    //setting Z FNCN for layer priority and visibility
    public Vector3 setZ(Vector3 vector, float z){
        vector.z = z;
        return vector;
    }


    //SNAP FNCN for grid and space snapping on Pckg drop
    public void snapFncn(GameObject pckg, List<Transform> ex)
    {
        //set float for snap distance limit
        float closestDist = -1;
        //on start, there is not a closest tile
        Transform closestTile = null;
        //want to  run through each position of every tile in grid and make them snappable
        //here we define the behavior for snapping, if we are within a range of an object, it becomes the object to snap to
        foreach(Transform snapTile in ex)
        {
            //get the distance from the transform(ie tile) to the package
            float currDist = Vector2.Distance(pckg.transform.localPosition, snapTile.localPosition);

            //if we dont have a current closest tile, or we are closer than prev
            if(closestTile == null || currDist < closestDist)
            {
                //closest tile is now the tileobj closest
                closestTile = snapTile;
                closestDist = currDist;
            }
        }

        if(closestTile != null && closestDist <= snapRange)
        {
            pckg.transform.localPosition = closestTile.localPosition;
        }
    }





    void Start() //initialize the board COME BACK FOR SHADOWING
    {
        gridCoords = GameObject.Find("gridManager").GetComponent<gridManager>();
        currentScene = SceneManager.GetActiveScene();
    }



    //The Meat & Potatoes
    void Update()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //fetching mouse position relative to camera
        if (Input.GetMouseButtonDown(0)) //if LMB pressed
        {
            //mouse position finding bounds

            //collider reference for mouse overlap on specific layer
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition, -10, 10);
            
            //if we are overlapping (aka clicked on) with something
            if (targetObject)
            {

                // Play click sound when piece is clicked
                if (!clickSource.isPlaying){
                    clickSource.Play();
                }
                
                //if we clicked while overlapping w something, that is now our selected object (being held)
                selectedObject = targetObject.transform.gameObject;
                spriteRen = targetObject.GetComponent<SpriteRenderer>();
                collidah = targetObject.GetComponent<Collider2D>();
                offset = selectedObject.transform.position - mousePosition;
            }
        }

        if (selectedObject) //if currently holding an object with mouse click 
        {
            spriteRen.sortingOrder = 90;


            selectedObject.transform.position = setZ(selectedObject.transform.position, 9);
            selectedObject.transform.position = mousePosition + offset; //doing the moving PLUS OFFSET


            if (Input.GetMouseButtonDown(1)) 
            {
                if(currentScene.name == "MailbagDupe") {
                    //cover cases where box is currently rotating, call if not
                    StartCoroutine(RotateMe(selectedObject, Vector3.forward * 90, 1.0f));
                    isRotating = true;
                } else {
                    //cover cases where box is currently rotating, call if not
                    StartCoroutine(RotateMe(selectedObject, Vector3.forward * 45, 1.0f));
                    isRotating = true;
                }
            } else if (Input.GetKeyDown("space")){
                if(currentScene.name == "MailbagDupe") {
                    //cover cases where box is currently rotating, call if not
                    StartCoroutine(RotateMe(selectedObject, Vector3.forward * 90, 1.0f));
                    isRotating = true;
                } else {
                    //cover cases where box is currently rotating, call if not
                    StartCoroutine(RotateMe(selectedObject, Vector3.forward * 45, 1.0f));
                    isRotating = true;
                }
            }

        }
        if (Input.GetMouseButtonUp(0) && selectedObject) //LMB raised while holding movable object, aka dropping it
        {
            //checking cases for return to spawn (aka invalid drop)
            
            //AARON/GRAHAM WILL CHANGE--------------------------------
            PckgLap = selectedObject.GetComponent<Package>().overlap;
            Bounding = selectedObject.GetComponent<Package>().bounds;
            //-------------------------------------------------------

            //for playing audio on drop
            if (!dropSource.isPlaying){
                dropSource.Play();
            }

            //Bounding conditionals
            if (PckgLap == true || Bounding == true)
            {
                selectedObject.transform.position = setXY(selectedObject.transform.position, Random.Range(85, 117), Random.Range(-15, 72));
            }

            var currentPos = selectedObject.transform.position; //fetch the current objects position
            
            

            //taking instance of our gridManager to access the list of our vertices for each tile instantiated, using as snap points
            snapPoints = gridCoords.tilesnapXY;
            snapFncn(selectedObject, snapPoints);
            
            //end state here
            bool winnered;
            var test = FindObjectOfType<gridManager>();
            winnered = test.SolveaDaPuzzle();
            
            if(winnered == true)
            {
                scoreTracker.mailbagWin = true;
                SceneManager.LoadScene("WinPuzzleGame");
            }

            //make sure to set sprite renderer back to base
            spriteRen.sortingOrder = 0;

            //selected object set to null, no longer holding something
            selectedObject.transform.position = setZ(selectedObject.transform.position, 10);
            selectedObject = null;

            //reset 2d collider too
            collidah = null;
        }
    }


    
    //Collision Detect for Package Overlap/Grid Bounding
    
}
