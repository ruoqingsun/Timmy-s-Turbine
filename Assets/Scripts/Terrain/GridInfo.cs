using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridInfo : InfoItem {

    /*
    
        Attribites: 
            Elevation: height of the grid
            ExtraCost: extra construction of the turbine on the grid
            GridType: 0 is normal, 1 is river route
            [x, z]: Grid Position from [0, 0] to [19, 19]
            click: Sound of the clicking the grid
            showInfo: show floating window of the elevation information or not (set in the LevelEditor)
            
        Functions:
            GetInfo(): Return a string showing the information of the grid.
            OnMouseDown(): Place a turbine on the map
            setAttribute(): set extra cost (extra cost = elevation)
            OnMouseEnter(): Showing information box when Hovering over the grid
            OnMouseExit(): Information box disappearing when the mouse exit the grid.
               
        */


    public int Elevation;
	public int ExtraCost;
	
	public int GridType;
	
	public int x;
	public int z;
	public AudioClip click;

	public static bool showInfo;
	
	// Use this for initialization
	void Start () {
        GridInfo.showInfo = GameObject.Find("LevelEditor").GetComponent<LevelEditor>().GridInfo_showInfo;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public override string GetInfo()
	{
		string print = "";
		
		if(GridType == 0){
			print = "Terrain\n\n\n\n" + "Elevation: " + Elevation + "\nConstruction Cost: " + ExtraCost;
		}
		if(GridType == 1){
			print = "River\n\n\n\n";
		}
		
		return print;
	}
	
	void OnMouseDown()
	{
		if (LockUI.OverGui) return;

		GameObject sellBtn = GameObject.FindGameObjectWithTag("sellButton");
		GameObject repairBtn = GameObject.FindGameObjectWithTag("repairButton");
		
		if (sellBtn != null)
			sellBtn.GetComponent<SellManager> ().disableButton ();
		
		if (repairBtn != null)
			repairBtn.GetComponent<RepairManager> ().disableButton ();

		AudioSource.PlayClipAtPoint (click, Camera.main.transform.position, 0.8f);
		CreateManager createManager = GameObject.FindGameObjectWithTag ("createManager").transform.GetComponent<CreateManager> ();
		bool creating = createManager.creating;
		CreateManager.createType createType = createManager.currentType;
		
		if (creating) {
			
			if(this.GridType == 1 || TerrainInfo.placeItemInfo[x, z] == 1){
				
				createManager.creating = false;
				return;
				
			}
			
			if(createType == CreateManager.createType.turbine){
				
				Transform newTransform = createManager.newTransform;
				Quaternion rotation = createManager.rotation;
				int maxOutput = createManager.maxOutput; 
				int directionIndex = createManager.directionIndex;
				int cost = createManager.cost;
				float timeForWork = createManager.timeForWork;
				Vector3 pos = transform.position;
				pos.y += 1;
				Transform newObject = (Transform)Instantiate(newTransform, pos, rotation);
            
				TurbineInfo newTurbineInfo = newObject.GetComponent<TurbineInfo> ();
				
				newTurbineInfo.maxOutput = maxOutput;
				newTurbineInfo.CalculateOutput(Elevation);
				newTurbineInfo.directionIndex = directionIndex % 8;
				newTurbineInfo.cost = cost;
				newTurbineInfo.timeForWork = timeForWork;
				newTurbineInfo.turbineColor = createManager.turbineColor;
				Debug.Log(timeForWork);
				newTurbineInfo.x = x;
				newTurbineInfo.z = z;
				
				if(MoneyManager.money < cost + ExtraCost){
					createManager.finishCreateTurbine();
					Destroy(newObject.gameObject);
				}
				else{
					MoneyManager.money -= cost + ExtraCost;
					CreateManager.turbineNum++;
					createManager.finishCreateTurbine();
					TerrainInfo.placeItemInfo[x, z] = 1;
				}
			}
			
			//Create Pump here
			else if(createType == CreateManager.createType.pump)
			{
                Transform newTransform = createManager.newTransform;
                Quaternion rotation = createManager.rotation;
                int cost = createManager.cost;
                float pumpTimeBetween = createManager.pumpTimeBetween;
                Vector3 pos = transform.position;
                pos.y += 1;

                Transform newObject = (Transform)Instantiate(newTransform, pos, rotation);
             

                PumpInfo newPumpInfo = newObject.GetComponent<PumpInfo>();
                

                newPumpInfo.pumpTimeBetween = pumpTimeBetween;
                newPumpInfo.cost = cost;
                newPumpInfo.x = x;
                newPumpInfo.z = z;

                if (MoneyManager.money < cost)
                {
                    createManager.finishCreatePump();
                    Destroy(newObject.gameObject);
                }
                else {
                    MoneyManager.money -= cost;
                    
                    createManager.finishCreatePump();
                    TerrainInfo.placeItemInfo[x, z] = 1;
                }

            }

            //Create Transformer here
            else if(createType == CreateManager.createType.transformerA)
			{
                Transform newTransform = createManager.newTransform;
                Quaternion rotation = createManager.rotation;
                int cost = createManager.cost;
                Vector3 pos = transform.position;
                pos.y += 1;
                
                Transform newObject = (Transform)Instantiate(newTransform, pos, rotation);

                if (MoneyManager.money < cost)
                {
                    createManager.finishCreateTransformer();
                    Destroy(newObject.gameObject);
                }
                else {
                    MoneyManager.money -= cost;

                    createManager.finishCreateTransformer();
                    TerrainInfo.placeItemInfo[x, z] = 1;
                }
            }
            else if (createType == CreateManager.createType.transformerB)
            {
                Transform newTransform = createManager.newTransform;
                Quaternion rotation = createManager.rotation;
                int cost = createManager.cost;
                Vector3 pos = transform.position;
                pos.y += 1;

                Transform newObject = (Transform)Instantiate(newTransform, pos, rotation);

                if (MoneyManager.money < cost)
                {
                    createManager.finishCreateTransformer();
                    Destroy(newObject.gameObject);
                }
                else {
                    MoneyManager.money -= cost;

                    createManager.finishCreateTransformer();
                    TerrainInfo.placeItemInfo[x, z] = 1;
                }
            }

        }
		
		if (!creating) {
			GameObject.FindGameObjectWithTag ("screens").GetComponent<CustomizationSwitch> ().toSelectionP ();
			GameObject.FindGameObjectWithTag ("selectionPanel").GetComponent<InfoPanel> ().UpdateInfo (gameObject.transform.GetComponent<GridInfo>());
			Debug.Log(GetInfo ());
			
			GameObject.FindGameObjectWithTag ("createManager").transform.position = gameObject.transform.position;
		}
	}
	
	public void setAttribute(){
		
		ExtraCost = Elevation;
		
	}

	public void OnMouseEnter (){

		if (!GridInfo.showInfo)
			return;

		if (GridType == 0) {

			CreateManager createManager = GameObject.FindGameObjectWithTag ("createManager").transform.GetComponent<CreateManager> ();

			transform.GetChild (0).GetComponent<CanvasGroup> ().alpha = 0.7f;

			int totalCost = createManager.cost + ExtraCost;

			if (!createManager.creating)
				transform.GetChild (0).GetChild (1).GetComponent<Text> ().text = "Elevation: " + Elevation + "\n" + "Extra Cost: " + ExtraCost + " TC";
			else
				transform.GetChild (0).GetChild (1).GetComponent<Text> ().text = "Elevation: " + Elevation + "\n" + "Extra Cost: " + ExtraCost + " TC" + "\nTotal Cost: " + totalCost + " TC";
		}
	}

	public void OnMouseExit(){

		if (GridType == 0) {

			transform.GetChild (0).GetComponent<CanvasGroup> ().alpha = 0f;
			transform.GetChild (0).GetChild (1).GetComponent<Text> ().text = "";

		}
	}
}
