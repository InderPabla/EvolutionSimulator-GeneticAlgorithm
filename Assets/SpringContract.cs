using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SpringContract : MonoBehaviour {

    List<SpringJoint2D[]> springJointsList = new List<SpringJoint2D[]>();
    SpringJoint2D[,] sprintJointsArray;
    Transform[] joints = new Transform[4];
    // Use this for initialization
    bool ossilate = false;
	void Start () {


        Invoke("after",1);
       

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void after()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            joints[i] = transform.GetChild(i);
            springJointsList.Add(joints[i].GetComponents<SpringJoint2D>());
        }

        Invoke("every",0);

    }

    void every()
    {
        float distance = 1f;

        if (ossilate == true){
            ossilate = false;
            distance = 2f;
        }
        else{
            ossilate = true;
            distance = 1f;
        }

        (springJointsList[0])[0].distance = distance;
        (springJointsList[2])[0].distance = distance;
        (springJointsList[2])[1].distance = distance;

        (springJointsList[3])[0].distance = distance;
        (springJointsList[3])[1].distance = distance;
        (springJointsList[3])[2].distance = distance;

        Invoke("every", 0.25f);
    }


}
