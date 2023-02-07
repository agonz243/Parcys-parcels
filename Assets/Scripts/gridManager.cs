using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridManager : MonoBehaviour
{
    //private variables for dimensions of grid
    [SerializeField] private int _width, _height;

    //making an individual tile
    [SerializeField] private Tile tilePrefab;

    //want to center camera on our grid, solve problem of "origin"
    [SerializeField] private Transform camcorder;

    //float for where to snap for grid squares
    public float snapRange = 0.5f;

    //list of coordinates for each tile on grid to snap to
    //i think a vector3 here
    public List<Tile> tilesnapPoints = new List<Tile>();

    //list of the tilepoints vector3s for xy values
    public List<Vector3> tilesnapXY = new List<Vector3>();

    //start method here
    void Start()
    {
        MakeaDaGrid();
    }



    //function for gridd(y)ing

    //note: tiles populate from left to right, bottom to top
    void MakeaDaGrid()
    {
        //loop over width
        for(int x = 0; x < _width; x = x+15) //added 15 to x and y for increased size of tiles
        {
            for(int y = 0; y < _height; y = y+15)
            {
                //these numbers are tweaked for tile and board size, ask graham if changes needed :)
                var spawnTile = Instantiate(tilePrefab, new Vector3(x-40, y-2), Quaternion.identity); //need to watch the Quarternion HERE for rotation
               
                
                //snapPoints.Add(spawnTile);
                
                spawnTile.name = $"Tile {x} {y}";
                
                //have the tile pbjects in a list, do not think needed
                tilesnapPoints.Add(spawnTile);

                //here we have the (x, y,z) vector 3 of each tile's position, want to use for snapping
                tilesnapXY.Add(spawnTile.transform.position);
                
                //math here using mods to determine checkerboard coloring for tiles
                var isOffset = (x + y) % 2 == 1;
                spawnTile.Init(isOffset);
            }
        }


        //put the coordinates in the bag and nobody gets hurt
        foreach (Vector3 pair in tilesnapXY)
        {
            Debug.Log(pair);
        }


        //actually moving the cam here using reference from above
        camcorder.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }
}
