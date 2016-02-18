using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloopCreature : MonoBehaviour {
 
    public GameObject nodePrefab;
    public GameObject linePrefab;

    GameObject[] bloopNodes;
    BloopDNA bloopDNA;
    bool ossilate = false;

    GameObject[] lines;
    LineRenderer[] lineRens;
    bool activated = false;
    float timeToLive = 0f;

    void Start () {

        
    }

    public void ActivateWithDNA(BloopDNA dna)
    {
        bloopDNA = dna;
        timeToLive = 10f; //15
        CreateCreatureFromDNA();
    }

    public void Activate(){
        bloopDNA = new BloopDNA();
        bloopDNA.GenerateRandomBloopDNA();
        timeToLive = 10f; //15
        CreateCreatureFromDNA(); 
    }

    void CreateCreatureFromDNA(){
        bloopNodes = new GameObject[bloopDNA.numberOfNodes];
        for (int i = 0; i < bloopDNA.numberOfNodes; i++)
        {
            Vector3 position = new Vector3(bloopDNA.nodeData[i, 0], bloopDNA.nodeData[i, 1], 0);
            bloopNodes[i] = (GameObject)Instantiate(nodePrefab, position, nodePrefab.transform.rotation);
            bloopNodes[i].transform.parent = transform;
            bloopNodes[i].name = "Node " + i;

            PhysicsMaterial2D material = new PhysicsMaterial2D();
            material.friction = bloopDNA.nodeData[i, 2];
            material.bounciness = bloopDNA.nodeData[i, 3];
            bloopNodes[i].GetComponent<BoxCollider2D>().sharedMaterial = material;

            if (bloopNodes[i].GetComponent<Renderer>() != null)
            {
                Color bloopNodeColor = new Color(bloopDNA.nodeData[i, 2], bloopDNA.nodeData[i, 2], bloopDNA.nodeData[i, 2]);
                bloopNodes[i].GetComponent<Renderer>().material.color = bloopNodeColor;
            }
        }

        Color[] colors = new Color[bloopDNA.numberOfMuscles];
        int index = 0;
        for (int i = 0; i < bloopDNA.numberOfNodes; i++)
        {
            List<int> singleNodeMuscularConnectionIndex = bloopDNA.multiNodeMuscularConnectionIndices[i];
            List<float[]> singleNodeMuscularData = bloopDNA.multiNodeMuscularData[i];
            for (int j = 0; j < singleNodeMuscularConnectionIndex.Count; j++)
            {
                bloopNodes[i].AddComponent<SpringJoint2D>();
                SpringJoint2D[] bloopNodeSpringJoints = bloopNodes[i].GetComponents<SpringJoint2D>();
                SpringJoint2D springJoint = bloopNodeSpringJoints[bloopNodeSpringJoints.Length - 1];
                springJoint.frequency = singleNodeMuscularData[j][3];
                springJoint.dampingRatio = singleNodeMuscularData[j][2];
                springJoint.connectedBody = bloopNodes[singleNodeMuscularConnectionIndex[j]].GetComponent<Rigidbody2D>();
                float value = singleNodeMuscularData[j][3] / 3f;
                colors[index] = new Color(1f, value, value);
                index++;
            }
        }

        lines = new GameObject[bloopDNA.numberOfMuscles];
        lineRens = new LineRenderer[bloopDNA.numberOfMuscles];
        


        for (int i = 0; i < bloopDNA.numberOfMuscles; i++)
        {
            lines[i] = Instantiate(linePrefab) as GameObject;
            lines[i].transform.parent = transform;
            lineRens[i] = lines[i].GetComponent<LineRenderer>();
            lineRens[i].SetWidth(0.1f, 0.1f);
            //lineRens[i].material.color = Color.blue;
            lineRens[i].material = new Material(Shader.Find("Particles/VertexLit Blended"));
            lineRens[i].SetColors(colors[i], colors[i]);
        }


        Animate();
        Invoke("Finish", timeToLive);
        activated = true;
    }

	void Update () {
        if (activated)
        {
            int muscleCounter = 0;
            for (int i = 0; i < bloopDNA.numberOfNodes; i++)
            {
                List<int> singleNodeMuscularConnectionIndex = bloopDNA.multiNodeMuscularConnectionIndices[i];
                for (int j = 0; j < singleNodeMuscularConnectionIndex.Count; j++)
                {
                    Vector3 position1 = bloopNodes[i].transform.position;
                    Vector3 position2 = bloopNodes[singleNodeMuscularConnectionIndex[j]].transform.position;
                    lineRens[muscleCounter].SetPosition(0, position1);
                    lineRens[muscleCounter].SetPosition(1, position2);

                    muscleCounter++;
                }
            }
        }
    }

    void Finish() {
        bloopDNA.fitness = 0f;
        for(int i = 0; i < bloopDNA.numberOfNodes; i++)
        {
            bloopDNA.fitness += bloopNodes[i].transform.position.x;
        }
        bloopDNA.fitness /= (float)bloopDNA.numberOfNodes;
        transform.parent.SendMessage("UpdateCounter",bloopDNA);
        Destroy(gameObject);
    }

    void Animate(){
        for (int i = 0; i < bloopDNA.numberOfNodes; i++)
        {
            SpringJoint2D[] sp = bloopNodes[i].GetComponents<SpringJoint2D>();
            List<float[]> singleNodeMuscularData = bloopDNA.multiNodeMuscularData[i];
            for (int j = 0; j < sp.Length; j++)
            {

                if(ossilate)
                    sp[j].distance = singleNodeMuscularData[j][0]; //min
                else
                    sp[j].distance = singleNodeMuscularData[j][1]; //max

            }
        }
        
        ossilate = !ossilate;

        //internal clock
         /*if(!ossilate)
             Invoke("Animate", 0.1f); //min
         else
             Invoke("Animate", 0.33f); //max*/
        Invoke("Animate", 0.25f); 
    }
}
