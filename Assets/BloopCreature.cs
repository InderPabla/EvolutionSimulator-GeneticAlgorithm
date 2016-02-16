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
    void Start () {

        
    }

    public void ActivateWithDNA(BloopDNA dna)
    {
        bloopDNA = dna;
        CreateCreatureFromDNA();
    }

    public void Activate(){
        bloopDNA = new BloopDNA();
        bloopDNA.GenerateRandomBloopDNA();
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
            material.bounciness = bloopDNA.nodeData[i, 2];
            material.bounciness = bloopDNA.nodeData[i, 3];
            bloopNodes[i].GetComponent<BoxCollider2D>().sharedMaterial = material;


        }

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

            }
        }

        lines = new GameObject[bloopDNA.numberOfMuscels];
        lineRens = new LineRenderer[bloopDNA.numberOfMuscels];
        for (int i = 0; i < bloopDNA.numberOfMuscels; i++)
        {
            lines[i] = Instantiate(linePrefab) as GameObject;
            lines[i].transform.parent = transform;
            lineRens[i] = lines[i].GetComponent<LineRenderer>();
            lineRens[i].SetWidth(0.1f, 0.1f);
        }


        Animate();
        Invoke("Finish", 15f);
        activated = true;
    }

	void Update () {
        if (activated)
        {
            int muscelCounter = 0;
            for (int i = 0; i < bloopDNA.numberOfNodes; i++)
            {
                List<int> singleNodeMuscularConnectionIndex = bloopDNA.multiNodeMuscularConnectionIndices[i];
                for (int j = 0; j < singleNodeMuscularConnectionIndex.Count; j++)
                {
                    Vector3 position1 = bloopNodes[i].transform.position;
                    Vector3 position2 = bloopNodes[singleNodeMuscularConnectionIndex[j]].transform.position;
                    lineRens[muscelCounter].SetPosition(0, position1);
                    lineRens[muscelCounter].SetPosition(1, position2);
                    muscelCounter++;
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
                    sp[j].distance = singleNodeMuscularData[j][0];
                else
                    sp[j].distance = singleNodeMuscularData[j][1];

            }
        }
        
        ossilate = !ossilate;
        if(ossilate)
            Invoke("Animate", 0.1f);
        else
            Invoke("Animate", 0.1f);
    }
}
