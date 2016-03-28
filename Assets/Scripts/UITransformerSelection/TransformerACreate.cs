using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TransformerACreate : MonoBehaviour {

    public Transform transformerA;
    public Quaternion rotation;
    public int cost;

    public static bool anyButtonCreate;
    public bool creating;

    // Use this for initialization
    void Start () {
        rotation = new Quaternion(0, 0, 0, 0);
        creating = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void createTransformerA()
    {
        GameObject.FindGameObjectWithTag("createManager").GetComponent<CreateManager>().createTransformerA(transformerA, rotation, cost, transform);
        Debug.Log("start");
    }
}
