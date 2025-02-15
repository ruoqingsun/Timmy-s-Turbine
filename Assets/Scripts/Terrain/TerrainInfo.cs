﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class TerrainInfo : MonoBehaviour {

    public static float pathHeight;             //place the grid at this height
    public int[,] gridInfo = new int[20, 20];		//0: normal grid; 1: grid
	public int[,] elevationInfo = new int[20, 20];		//Elevation Index: From 0 ~ elevationMax/ElevationInterval		
	public static int[,] placeItemInfo = new int[20, 20];		//0: not occupied; 1: occupied
	
	public int elevationInterval;           //The interval of the elevation. e.g. 10
	public int elevationMax;        //Maxmum elevation. e.g, 100

    private int indexMax;       //Maximum elevation index in this map, which is smaller than or equal to elevationMax/elevationInterval
    private int indexMin;       //Minimum elevation index in this map, which is larger than or equal to 0

    List<Transform> routeTurningPoints = new List<Transform>();
	List<Vector2> gridTurningPoints = new List<Vector2>();
	
	public Transform placeHolderTile;
	public Transform routeTile;

    /*
        LoadElevation(): Fill in elevationInfo[,]. Load elevation index from (0, 0)
        LoadElevation(x, y): Fill in elevationInfo[,]. Load elevation index where elevation index at [x, y] = elevationMax/ElevationInterval
        loadMap (): Fill in gridInfo[,]. Load grid information. 0 is available. 1 is occupied.
        loadRoute(): Load gridInfo according to the turning point. change it to 1 when the place is occupied by a routeTile.
        buildMap (): place corresponding tile Transform according to elevationInfo[,] and gridInfo[,].

        Static functions:
            GridToTranform(x, z): return a Vector3 showing Transform position. y = pathHeight;
            TransformToGrid(x, y, z): return a Vector2 showing corresponding tile index such as [0, 0];
        */

    // Use this for initialization
    void Start () {
        TerrainInfo.pathHeight = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().TerrainInfo_pathHeight;
        elevationInterval = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().TerrainInfo_elevationInterval;
        elevationMax = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().TerrainInfo_elevationMax;

        Transform turningPointsObject = GameObject.FindGameObjectWithTag ("routeTurningPoints").transform;
		int i = 0;

		routeTurningPoints.Add(GameObject.FindGameObjectWithTag ("startPoint").transform);
		gridTurningPoints.Add(TerrainInfo.TransformToGrid(GameObject.FindGameObjectWithTag ("startPoint").transform.position));
		
		while (i < turningPointsObject.childCount) {
			routeTurningPoints.Add (turningPointsObject.GetChild (i).transform);
			gridTurningPoints.Add(TerrainInfo.TransformToGrid(turningPointsObject.GetChild (i).transform.position));
			//Debug.Log ("Turning Points" + gridTurningPoints[i]);
			i++;
		}
			
		loadElevation (12, 6);
		loadMap ();
		buildMap ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static Vector3 GridToTranform(int x, int z){
		
		return new Vector3 ((float)(10 * x - 95), pathHeight,(float)( -10 * z + 95));
		
	}
	
	public static Vector2 TransformToGrid(Vector3 newVector){
		
		return new Vector2 ((int)(newVector.x + 95) / 10, (int)(95 - newVector.z) / 10);
		
	}
	
	public void loadElevation(){
		
		int elevationBound = elevationMax / elevationInterval + 1;
		int currentIndex = 0;
		int prevIndexX = 0;
		int prevIndexZ = 0;
		
		indexMin = elevationBound - 1;
		indexMax = 0;
		
		for (int x = 0; x < 20; x++) {
			
			for(int z = 0; z < 20; z++){
				
				if (x == 0 && z == 0){
					
					currentIndex = UnityEngine.Random.Range(0, elevationBound);
					
				}
				
				else if (x == 0){
					
					prevIndexZ = elevationInfo[x, z - 1];
					currentIndex = UnityEngine.Random.Range(Math.Max(prevIndexZ - 1, 0), Math.Min(prevIndexZ + 2, elevationBound));
					
				}
				
				else if (z == 0){
					
					prevIndexX = elevationInfo[x - 1, z];
					currentIndex = UnityEngine.Random.Range(Math.Max(prevIndexX - 1, 0), Math.Min(prevIndexX + 2, elevationBound));
				}
				
				else {
					
					prevIndexX = elevationInfo[x - 1, z];
					prevIndexZ = elevationInfo[x, z - 1];
					
					int min = Math.Max(Math.Max(prevIndexX - 1, prevIndexZ - 1), 0);
					int max = Math.Min(Math.Min(prevIndexX + 2, prevIndexZ + 2), elevationBound);
					currentIndex = UnityEngine.Random.Range(min, max);
				}
				
				elevationInfo[x, z] = currentIndex;
				indexMax = Math.Max (currentIndex, indexMax);
				indexMin = Math.Min (currentIndex, indexMin);
				
			}
			
		}

	}
	
	public void loadElevation(int m, int n){
		
		int elevationBound = elevationMax / elevationInterval + 1;
		int currentIndex = 0;
		int prevIndexX = 0;
		int prevIndexZ = 0;
		
		indexMin = elevationBound - 1;
		indexMax = 0;
		
		//m = 12; n = 6;
		elevationInfo [m, n] = elevationBound - 1;
		
		for (int z = n-1; z >= 0; z--) {
			
			prevIndexZ = elevationInfo[m, z + 1];
			elevationInfo[m, z] = UnityEngine.Random.Range(Math.Max(prevIndexZ - 1, 0), Math.Min(prevIndexZ + 2, elevationBound));
			currentIndex = elevationInfo[m, z];
			indexMax = Math.Max (currentIndex, indexMax);
			indexMin = Math.Min (currentIndex, indexMin);
			
		}
		
		for (int z = n+1; z < 20; z++) {
			
			prevIndexZ = elevationInfo[m, z - 1];
			elevationInfo[m, z] = UnityEngine.Random.Range(Math.Max(prevIndexZ - 1, 0), Math.Min(prevIndexZ + 2, elevationBound));
			currentIndex = elevationInfo[m, z];
			indexMax = Math.Max (currentIndex, indexMax);
			indexMin = Math.Min (currentIndex, indexMin);
			
		}
		
		for (int x = m - 1; x >= 0; x--) {
			
			for(int z = 0; z < 20; z++)
			{
				if (z == 0){
					
					prevIndexX = elevationInfo[x + 1, z];
					currentIndex = UnityEngine.Random.Range(Math.Max(prevIndexX - 1, 0), Math.Min(prevIndexX + 2, elevationBound));
				}
				
				else{
					
					prevIndexX = elevationInfo[x + 1, z];
					prevIndexZ = elevationInfo[x, z - 1];
					
					int min = Math.Max(Math.Max(prevIndexX - 1, prevIndexZ - 1), 0);
					int max = Math.Min(Math.Min(prevIndexX + 2, prevIndexZ + 2), elevationBound);
					currentIndex = UnityEngine.Random.Range(min, max);
					
				}
				
				elevationInfo[x, z] = currentIndex;
				indexMax = Math.Max (currentIndex, indexMax);
				indexMin = Math.Min (currentIndex, indexMin);
			}
			
		}
		
		for (int x = m + 1; x < 20; x++) {
			
			for(int z = 0; z < 20; z++)
			{
				if (z == 0){
					
					prevIndexX = elevationInfo[x - 1, z];
					currentIndex = UnityEngine.Random.Range(Math.Max(prevIndexX - 1, 0), Math.Min(prevIndexX + 2, elevationBound));
				}
				
				else{
					
					prevIndexX = elevationInfo[x - 1, z];
					prevIndexZ = elevationInfo[x, z - 1];
					
					int min = Math.Max(Math.Max(prevIndexX - 1, prevIndexZ - 1), 0);
					int max = Math.Min(Math.Min(prevIndexX + 2, prevIndexZ + 2), elevationBound);
					currentIndex = UnityEngine.Random.Range(min, max);
					
				}
				
				elevationInfo[x, z] = currentIndex;
				indexMax = Math.Max (currentIndex, indexMax);
				indexMin = Math.Min (currentIndex, indexMin);
			}
			
		}
		
	}
	
	public void loadMap(){
		
		for (int x = 0; x < 20; x++) {
			
			for(int z = 0; z < 20; z++){
				
				gridInfo[x,z] = 0;
				placeItemInfo[x, z] = 0;
				
			}
			
		}
		
		Vector2 prev = gridTurningPoints[0];
		
		for (int i = 1; i < gridTurningPoints.Count; i++) {
			
			loadRoute (prev, gridTurningPoints[i]);
			prev = gridTurningPoints[i];
			
		}
		
		//Detect the palce of all turbines, pumps and transformers that already in the scene
		
		GameObject[] allTurbines = GameObject.FindGameObjectsWithTag ("turbine");
		GameObject[] allPumps = GameObject.FindGameObjectsWithTag ("waterTower");
		GameObject[] allTransformers = GameObject.FindGameObjectsWithTag ("transformer");
		GameObject[] allTransformerForTurbines = GameObject.FindGameObjectsWithTag("transformerForTurbine");
		
		for (int i = 0; i < allTurbines.Length; i++) {
			
			Transform currentTransform = allTurbines[i].transform;
			Vector2 gridPlace = TerrainInfo.TransformToGrid(currentTransform.position);
			TurbineInfo currentTurbine = allTurbines[i].transform.GetComponent<TurbineInfo>();
			currentTurbine.x = (int) gridPlace.x;
			currentTurbine.z = (int) gridPlace.y;
			placeItemInfo[(int)gridPlace.x, (int)gridPlace.y] = 1;
			
		}
		
		for (int i = 0; i < allPumps.Length; i++) {
			
			Transform currentTransform = allPumps[i].transform;
			Vector2 gridPlace = TerrainInfo.TransformToGrid(currentTransform.position);
			PumpInfo currentPump = allPumps[i].transform.GetComponent<PumpInfo>();
			currentPump.x = (int)gridPlace.x;
			currentPump.z = (int)gridPlace.y;
			placeItemInfo[(int)gridPlace.x, (int)gridPlace.y] = 1;
			
		}
		
		for (int i = 0; i < allTransformers.Length; i++) {
			
			Transform currentTransform = allTransformers[i].transform;
			Vector2 gridPlace = TerrainInfo.TransformToGrid(currentTransform.position);
			TransformerInfo currentTransformer = allTransformers[i].transform.GetComponent<TransformerInfo>();
			currentTransformer.x = (int)gridPlace.x;
			currentTransformer.z = (int)gridPlace.y;
			placeItemInfo[(int)gridPlace.x, (int)gridPlace.y] = 1;
			
		}
		
		for (int i = 0; i < allTransformerForTurbines.Length; i++) {
			
			Transform currentTransform = allTransformerForTurbines[i].transform;
			Vector2 gridPlace = TerrainInfo.TransformToGrid(currentTransform.position);
			TransformerForTurbineInfo currentTransformerForTurbine = allTransformerForTurbines[i].transform.GetComponent<TransformerForTurbineInfo>();
			currentTransformerForTurbine.x = (int)gridPlace.x;
			currentTransformerForTurbine.z = (int)gridPlace.y;
			placeItemInfo[(int)gridPlace.x, (int)gridPlace.y] = 1;
			
		}
	}
		
	private void loadRoute(Vector2 prev, Vector2 after)
	{
		if ((int)prev.x == (int)after.x) {
			
			int max = Math.Max((int)prev.y, (int)after.y);
			int min = Math.Min((int)prev.y, (int)after.y);
			
			for(int i = min; i<= max; i++){
				
				gridInfo[(int)prev.x, i] = 1;
				placeItemInfo[(int)prev.x, i] = 1;
				
			}
			
		}
		
		if ((int)prev.y == (int)after.y) {
			
			int max = Math.Max((int)prev.x, (int)after.x);
			int min = Math.Min((int)prev.x, (int)after.x);
			
			for(int i = min; i<=max; i++){
				
				gridInfo[i, (int)prev.y] = 1;
				placeItemInfo[i, (int)prev.y] = 1;
			}
			
		}
	}

	public void buildMap()
	{
		int elevationBound = elevationMax / elevationInterval;
		
		for (int x = 0; x < 20; x++) {
			
			for(int z = 0; z < 20; z++){
				
				if(gridInfo[x, z]==0)
				{
					Transform newGrid = (Transform)Instantiate(placeHolderTile, TerrainInfo.GridToTranform(x, z), Quaternion.identity);
					newGrid.SetParent(gameObject.transform);
					GridInfo newGridInfo = newGrid.GetComponent<GridInfo>();
					newGridInfo.x = x;
					newGridInfo.z = z;
					newGrid.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 1f / (float) (indexMax-indexMin) * (float)(elevationInfo[x, z]-indexMin));
					newGridInfo.Elevation = elevationInfo[x, z] * elevationInterval;
					newGridInfo.setAttribute();
				}
				
				else if(gridInfo[x, z] == 1)
				{
					Transform newGrid = (Transform)Instantiate(routeTile, TerrainInfo.GridToTranform(x, z), Quaternion.identity);
					newGrid.SetParent(gameObject.transform);
					GridInfo newGridInfo = newGrid.GetComponent<GridInfo>();
					newGridInfo.x = x;
					newGridInfo.z = z;
					newGridInfo.Elevation = elevationInfo[x, z] * elevationInterval;
					newGridInfo.setAttribute();
				}
				
			}
			
		}
	}
}
