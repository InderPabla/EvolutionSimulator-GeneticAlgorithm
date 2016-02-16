using UnityEngine;
using System.Collections;

public class Bloop : MonoBehaviour {
    public GameObject joint;
    public GameObject line;
    // Use this for initialization
    int numberOfJoint = 4;
    GameObject[] joints;
    GameObject[] lines;
    LineRenderer[] lineRens;
    bool ossilate = false;
    LineRenderer lineRen;
    void Start () {

        lines = new GameObject[numberOfJoint*3];
        lineRens = new LineRenderer[numberOfJoint * 3];

        joints = new GameObject[numberOfJoint];

        for (int i = 0; i < numberOfJoint; i++)
        {
            joints[i] = Instantiate(joint) as GameObject;
            joints[i].transform.parent = transform;
        }

        for (int i = 0; i < numberOfJoint*3; i++)
        {
            lines[i] = Instantiate(line) as GameObject;
            lineRens[i] = lines[i].GetComponent<LineRenderer>();
            lineRens[i].SetWidth(0.1f,0.1f);
        }

        joints[0].transform.position = new Vector3(-0.5f, 0.5f,0);
        joints[1].transform.position = new Vector3(0.5f, 0.5f, 0);
        joints[2].transform.position = new Vector3(-0.5f, -0.5f, 0);
        joints[3].transform.position = new Vector3(0.5f, -0.5f, 0);

        for (int i =0;i < numberOfJoint; i++){
            
            for (int j = 0; j < numberOfJoint; j++){
                if (j != i){
                    //SpringJoint2D spring = new SpringJoint2D();
                    joints[i].AddComponent<SpringJoint2D>();
                    SpringJoint2D[] sp = joints[i].GetComponents<SpringJoint2D>();
                    SpringJoint2D sp1 = sp[sp.Length-1];
                    sp1.connectedBody = joints[j].GetComponent<Rigidbody2D>();
                }
            }
        }

        every();
    }
	
	// Update is called once per frame
	void Update () {
        
        int index = 0;
        for (int i = 0; i < numberOfJoint; i++)
        {
            for (int j = 0; j < numberOfJoint; j++)
            {
                if (j != i)
                {
                    lineRens[index].SetPosition(0, joints[i].transform.position);
                    lineRens[index].SetPosition(1, joints[j].transform.position);
                    index++;
                }
            }
        }
    }

    void every()
    {
        float distance = 1f;

        if (ossilate == true)
        {
            ossilate = false;
            distance = 1.25f;
        }
        else
        {
            ossilate = true;
            distance = 1f;
        }

        for (int i = 0; i < numberOfJoint; i++)
        {
            SpringJoint2D[] sp = joints[i].GetComponents<SpringJoint2D>();
            for (int j = 0; j < sp.Length; j++)
            {
                sp[j].distance = distance;
                
            }
        }

        Invoke("every", 0.25f);
    }
}
