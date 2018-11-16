/// <information>
/// Created By: Austin Takechi
/// Purpose: To use Prim's Algorithm to generate
///  a randomly structured maze of dynamic proportions.
/// Contact: MinoruTono@Gmail.com
/// </information>

/// <summary>
/// This script creates and attaches the grid,
/// and also faciliates most of Prim's Algorithm.
/// </summary>


//Included by Default
using UnityEngine; 
//Included by Default
using System.Collections;
//Allows us to use List< >
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//This is the main Class
// All of our scripting in GridScript goes in here.
public class GridScript : MonoBehaviour {
	//The CellPrefab is a Transform Prefab
	// we use as a template to create each
	// cell. This is set in Unity's Inspector.
	public Transform CellPrefab;
	public Transform WallPrefab;
	public Transform BallPrefab;
	public Transform FinishPrefab;
	//Size is a Vector3, denotes the 
	// size of the Grid we're going to create.
	// Only X and Z are used, Y is set to 1 at default.
	// This is set in Unity's Inspector.
	Vector3 Size = new Vector3(10,0,10);
	//Grid[ , ] is a Multidimensional Array that 
	// stores each newly created Cell in an easy
	// X,Z notation. 0-Based.
	public Transform[,] Grid;

	void Awake(){
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//		Input.gyro.enabled = false;
	}
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("Width") >= 10 && PlayerPrefs.GetInt ("Height") >= 10) {
			Size  = new Vector3(PlayerPrefs.GetInt ("Width"),0,PlayerPrefs.GetInt ("Height"));
		}

		CreateWall();

		//CreateGrid will create a new grid of 
		// size X,Z; name and parent those new
		// cells, and add the cell to our Grid List< >
		CreateGrid();

		//SetRandomNumbers goes through each
		// of our blank cells and assigns it a
		// random weight.
		SetRandomNumbers();

		//Set Adjacents goes through each
		// of the cells and assigns the adjacents,
		// and then ranks the adjacents by
		// using our custom sort:
		// SortByLowestWeight( )
		SetAdjacents();

		//We set the entrance cell of the grid,
		// by default in the lower left corner,
		// or Grid[0,0].
		SetStart();

		//FindNext looks for the lowest weight adjacent
		// to all the cells in the Set, and adds that
		// cell to the Set. A cell is disqualified if
		// it has two open neighbors. This makes the 
		// maze full of deadends.
		//FindNext also will invoke itself as soon as it
		// finishes, allowing it to loop indefinitely until
		// the invoke is canceled when we detect our maze is done.
		FindNext();
	}

	void CreateWall(){
		float wallXMin = 0 - ((CellPrefab.transform.localScale.x / 2) + WallPrefab.transform.localScale.x / 2);
		float wallXMax = ((int)Size.x - 1) + ((CellPrefab.transform.localScale.x / 2) + WallPrefab.transform.localScale.x / 2);
		// use wall's x scale because we want to rotate the wall.
		float wallZMin = 0 - ((CellPrefab.transform.localScale.z / 2) + WallPrefab.transform.localScale.x / 2);
		float wallZMax = ((int)Size.z - 1) + ((CellPrefab.transform.localScale.z / 2) + WallPrefab.transform.localScale.x / 2);

		WallPrefab.transform.localScale = new Vector3 (WallPrefab.transform.transform.localScale.x, WallPrefab.transform.transform.localScale.y, Size.z);
		Instantiate (WallPrefab, new Vector3 (wallXMin, 1F, (float)((int)Size.z - 1) / 2), Quaternion.identity);
		Instantiate (WallPrefab, new Vector3 (wallXMax, 1F, (float)((int)Size.z - 1) / 2), Quaternion.identity);
		WallPrefab.transform.localScale = new Vector3 (WallPrefab.transform.transform.localScale.x, WallPrefab.transform.transform.localScale.y, Size.x);
		Instantiate (WallPrefab, new Vector3 ((float)((int)Size.x - 1) / 2, 1F, wallZMin), Quaternion.Euler (new Vector3 (0, 90F, 0)));
		Instantiate (WallPrefab, new Vector3 ((float)((int)Size.x - 1) / 2, 1F, wallZMax), Quaternion.Euler (new Vector3 (0, 90F, 0)));
	}

	void CreateGrid(){
		//Resize Grid to the proper size specified in Inspector.
		// Because we use a Vector3, we need to convert the X, Y, and Z
		// from Float to Int.
		// We do that easily by adding "(int)" in front of the float.
		// This is called explicit downcasting.
		Grid = new Transform[(int)Size.x, (int)Size.z];

		//We now enter a Double For loop.
		// This will create each cell by looping from x = 0 while x < Size.x,
		// and then the same for z.
		for(int x = 0; x < Size.x; x++){
			for(int z = 0; z < Size.z; z++){
				//We create a new Transform to manipulate later.
				Transform newCell;
				//A new CellPrefab is Instantiated at the correct location
				// using "new Vector3(x,0,z)".
				// Quaternion.Identity is a rotation that we don't need to
				// worry about.
				newCell = (Transform)Instantiate(CellPrefab, new Vector3(x, 0, z), Quaternion.identity);
				//The newCell is now renamed "(x,0,z)" using String.Format
				// "(+x+",0,"+z+")" would also work, but is less efficient.
				newCell.name = string.Format("({0},0,{1})",x,z);
				//For clean-ness, we parent all the newCells to the Grid gameObject.
				newCell.parent = transform;
				//We already set the position of the newCell, but the cell's attached
				// script needs to know where it is also.
				// We assign it here.
				newCell.GetComponent<CellScript>().Position = new Vector3(x, 0, z);
				//Grid[,] keeps track of all of the cells.
				// We add the newCell to the appropriate location in the Grid array.
				Grid[x,z] = newCell;
			}
		}
	}

	void SetRandomNumbers(){
		//ForEach cell in the Grid gameObject:
		foreach(Transform child in transform){
			//Get a new random number between 0 and 10.
			int weight = Random.Range(0,10);
			//Assign that number to Weight variable in the CellScript.
			child.GetComponent<CellScript>().Weight = weight;
		}
	}

	void SetAdjacents(){
		//Double For loop acts as a ForEach
		for(int x = 0; x < Size.x; x++){
			for(int z = 0; z < Size.z; z++){
				//Create a new variable to be the
				// cell at position x, z.
				Transform cell;
				cell = Grid[x,z];
				//Create a new CellScript variable to
				// hold the cell's script component.
				CellScript cScript = cell.GetComponent<CellScript>();
				//As long as it is valid,
				// Add the left, right, down and up cells adjacent
				// to the list inside this cell's cellScript.
				if(x - 1 >= 0){
					cScript.Adjacents.Add(Grid[x-1,z]);
				}
				if(x + 1 < Size.x){
					cScript.Adjacents.Add(Grid[x+1,z]);
				}
				if(z - 1 >= 0){
					cScript.Adjacents.Add(Grid[x,z-1]);
				}
				if(z + 1 < Size.z){
					cScript.Adjacents.Add(Grid[x,z+1]);
				}
				//After each cell has been validated and entered,
				// sort all the adjacents in the list
				// by the lowest weight.
				cScript.Adjacents.Sort(SortByLowestWeight);
			}
		}
	}

	//Check the link for more info on custom comparers and sorts.
	//http://msdn.microsoft.com/en-us/library/0e743hdt.aspx
	int SortByLowestWeight(Transform inputA, Transform inputB){
		//Given two transforms, find which cellScript's Weight
		// is the highest. Sort by the Weights.
		int a = inputA.GetComponent<CellScript>().Weight; //a's weight
		int b = inputB.GetComponent<CellScript>().Weight; //b's weight
		return a.CompareTo(b);
	}

	//This list is used for Prim's Algorithm.
	// We start with an empty list,
	// but as the maze is created, cells will be
	// continuously added to this Set list.
	// Look at the Wikipedia page for more info
	// on Prim's.
	// http://en.wikipedia.org/wiki/Prim%27s_algorithm
	private List<Transform> Set;

	//Here we have a List of Lists.
	// Here is the structure:
	//  AdjSet{
	//       [ 0 ] is a list of all the cells
	//         that have a weight of 0, and are
	//         adjacent to the cells in Set.
	//       [ 1 ] is a list of all the cells
	//         that have a weight of 1, and are
	//         adjacent to the cells in Set.
	//       [ 2 ] is a list of all the cells
	//         that have a weight of 2, and are
	//         adjacent to the cells in Set.
	//     etc...
	//       [ 9 ] is a list of all the cells
	//         that have a weight of 9, and are
	//         adjacent to the cells in Set.
	//  }
	//
	// Note: Multiple entries of the same cell
	//  will not appear as duplicates.
	//  (Some adjacent cells will be next to
	//  two or three or four other Set cells).
	//  They are only recorded in the AdjSet once.
	public List<List<Transform>> AdjSet;

	void SetStart(){
		//Create a new List<Transform> for Set.
		Set = new List<Transform>();
		//Also, we create a new List<List<Transform>>
		// and in the For loop, List<Transform>'s.
		AdjSet = new List<List<Transform>>();
		for(int i = 0; i < 10; i++){
			AdjSet.Add(new List<Transform>());	
		}
		//The Start of our Maze/Set will be color
		// coded Green, so we apply that to the renderer's
		// material's color here.
		Grid[0,0].GetComponent<Renderer>().material.color = Color.green;
		//Now, we add the first cell to the Set.
		AddToSet(Grid[0,0]);
	}

	void AddToSet(Transform toAdd){
		//Adds the toAdd object to the Set.
		// The toAdd transform is sent as a parameter.
		Set.Add(toAdd);
		//For every adjacent next to the toAdd object:
		foreach(Transform adj in toAdd.GetComponent<CellScript>().Adjacents){
			//Add one to the adjacent's cellScript's AdjacentsOpened
			adj.GetComponent<CellScript>().AdjacentsOpened++;
			//If
			// a) The Set does not contain the adjacent
			//   (cells in the Set are not valid to be entered as
			//   adjacentCells as well).
			//  and
			// b) The AdjSet does not already contain the adjacent cell.
			// then..
			if(!Set.Contains(adj) && !(AdjSet[adj.GetComponent<CellScript>().Weight].Contains(adj))){
				//.. Add this new cell into the proper AdjSet sub-list.
				AdjSet[adj.GetComponent<CellScript>().Weight].Add(adj);
			}
		}
	}

	void FindNext(){
		//We create an empty Transform variable
		// to store the next cell in.
		Transform next;
		//Perform this loop 
		// While:
		//  The proposed next gameObject's AdjacentsOpened
		//   is less than or equal to 2.
		//   This is to ensure the maze-like structure.
		do{
			//We'll initially assume that each sub-list of AdjSet is empty
			// and try to prove that assumption false in the for loop.
			// This boolean value will keep track.
			bool empty = true;
			//We'll also take a note of which list is the Lowest,
			// and store it in this variable.
			int lowestList = 0;
			for(int i = 0; i < 10; i++){
				//We loop through each sub-list in the AdjSet list of
				// lists, until we find one with a count of more than 0.
				// If there are more than 0 items in the sub-list,
				// it is not empty.
				//We then stop the loop by using the break keyword;
				// We've found the lowest sub-list, so there is no need
				// to continue searching.
				lowestList = i;
				if(AdjSet[i].Count > 0){
					empty = false;
					break;
				}
			}
			//There is a chance that none of the sub-lists of AdjSet will
			// have any items in them.
			//If this happens, then we have no more cells to open, and
			// are done with the maze production.
			if(empty){ 
				//If we finish, as stated and determined above,
				// display a message to the DebugConsole
				// that includes how many seconds it took to finish.
				//Debug.Log("We're Done, "+Time.timeSinceLevelLoad+" seconds taken"); 
				//Then, cancel our recursive invokes of the FindNext function,
				// as we're done with the maze.
				//If we allowed the invokes to keep going, we will receive an error.
				CancelInvoke("FindNext");
				//Set.Count-1 is the index of the last element in Set,
				// or the last cell we opened.
				//This will be marked as the end of our maze, and so
				// we mark it red.
				Set[Set.Count-1].GetComponent<Renderer>().material.color = Color.red;
				//Here's an extra something I put in myself.
				//Every cell in the grid that is not in the set
				// will be moved one unit up and turned black.
				// (I changed the default color from black to clear earlier).
				// If you instantiate a FirstPersonController in the maze now,
				// you can actually try walking through it.
				// It's really hard.
				foreach(Transform cell in Grid){
					if(!Set.Contains(cell)){
						cell.Translate(Vector3.up);	
						cell.GetComponent<Renderer>().material.color = new Color(0.2F,0.2F,0.2F,0.2F);
					}
				}
				Vector3 finishpos = (Set[Set.Count-1].GetComponent<CellScript>().Position);
				finishpos.y = 1.0F;
				Instantiate(FinishPrefab, finishpos, FinishPrefab.transform.rotation);
				Instantiate(BallPrefab);
				GameObject.FindWithTag("Loading").SetActive(false);
				GameObject.Find("Canvas").GetComponent<StartScript>().start = true;
				return;
			}
			//If we did not finish, then:
			// 1. Use the smallest sub-list in AdjSet
			//     as found earlier with the lowestList
			//     variable.
			// 2. With that smallest sub-list, take the first
			//     element in that list, and use it as the 'next'.
			next = AdjSet[lowestList][0];
			//Since we do not want the same cell in both AdjSet and Set,
			// remove this 'next' variable from AdjSet.
			AdjSet[lowestList].Remove(next);
		}while(next.GetComponent<CellScript>().AdjacentsOpened >= 2);
		//The 'next' transform's material color becomes white.
		next.GetComponent<Renderer>().material.color = Color.white;
		//We add this 'next' transform to the Set our function.
		AddToSet(next);
		//Recursively call this function as soon as this function
		// finishes.
		Invoke("FindNext", 0);
	}


	void Update(){
		//When we press the F1 key,
		// we're gonna restart the level.
		// This is for testing purposes.
		if(Input.GetKeyDown(KeyCode.F1)){
			SceneManager.LoadScene("maze");
		}
	}
}
