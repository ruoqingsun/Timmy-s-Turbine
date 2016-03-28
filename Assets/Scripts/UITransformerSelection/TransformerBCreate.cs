using UnityEngine;
using System.Collections;

public class TransformerBCreate : MonoBehaviour {

    public Transform transformerB;
    public Quaternion rotation;
    public int cost;

    public bool creating;

    // Use this for initialization
    void Start () {
        rotation = new Quaternion(0, 0, 0, 0);
        creating = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void createTransformerB()
    {
        GameObject.FindGameObjectWithTag("createManager").GetComponent<CreateManager>().createTransformerB(transformerB, rotation, cost, transform);
        Debug.Log("start");
    }
}
