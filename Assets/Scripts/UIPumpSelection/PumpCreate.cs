using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PumpCreate : MonoBehaviour {

    public Transform pump;
    public Quaternion rotation;
    public int cost;
    public float pumpTimeBetween;
 

    public static bool anyButtonCreate;
    public bool creating;


    // Use this for initialization
    void Start () {
        rotation = new Quaternion(0, 0, 0, 0);
        creating = false;
    }
	
    void Awake()
    {
        cost = 0;
        pumpTimeBetween = 0.5f;
    }
	// Update is called once per frame
	void Update () {
	/*if (MoneyManager.money < cost ||  creating || PumpCreate.anyButtonCreate) {
		
			transform.GetComponent<Button>().interactable = false;
		
		}

		else {

			transform.GetComponent<Button>().interactable = true;

		}*/
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject.FindGameObjectWithTag("createManager").GetComponent<CreateManager>().createPump(pump, rotation, pumpTimeBetween, cost, transform);
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            //			Debug.Log ("Middle click");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //			Debug.Log ("Right click");
           
        }
    }

    public void createPump()
    {
       GameObject.FindGameObjectWithTag("createManager").GetComponent<CreateManager>().createPump(pump, rotation, pumpTimeBetween, cost, transform);
        if (creating || WindTurbineBtnInfo.anyButtonCreate)
        {
            anyButtonCreate = false;
            GameObject.FindGameObjectWithTag("createManager").GetComponent<CreateManager>().finishCreatePump();
        }
    }
}
