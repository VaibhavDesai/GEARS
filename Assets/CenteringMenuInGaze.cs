using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenteringMenuInGaze : MonoBehaviour
{

    public GameObject itemObject;
    public float distance = 3.0f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (itemObject != null)
        {
            float y_offset=0f;

            if (gameObject.name == "SimulationSceneText")
                y_offset = -0.100f;
            else if (gameObject.name == "SimulationSceneMenuButtons")
                y_offset = -0.200f;
            else if (gameObject.name == "StaticSceneText")
                y_offset = 0.100f;
            else if (gameObject.name == "SharingSceneText")
                y_offset = -0.300f;
            else if (gameObject.name == "SharingSceneMenuButtons")
                y_offset = -0.400f;

            itemObject.transform.position = new Vector3(0, y_offset, 0)+ Camera.main.transform.position + Camera.main.transform.forward * distance;
            itemObject.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
        }

    }
}

