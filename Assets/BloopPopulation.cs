using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloopPopulation : MonoBehaviour
{
    double frameCount = 0;
    double dt = 0.0;
    double fps = 0.0;
    double updateRate = 4.0;  // 4 updates per sec.
    public GameObject bloopCreaturePrefab;
    // Use this for initialization
    GameObject[] bloopCreatures;
    int creaturesPerGeneration = 100;
    int creatureFinishedCounter = 0;
    bool printFrames = false;
    List<BloopDNA> bloopDNAList = new List<BloopDNA>();
    void Start()
    {
        bloopCreatures = new GameObject[creaturesPerGeneration];
        for (int i = 0; i < creaturesPerGeneration; i++)
        {

            bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
            bloopCreatures[i].transform.parent = transform;
            bloopCreatures[i].SendMessage("Activate");
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (printFrames)
        {
            frameCount++;
            dt += Time.deltaTime;
            if (dt > 1.0 / updateRate)
            {
                fps = frameCount / dt;
                frameCount = 0;
                dt -= 1.0 / updateRate;
            }
            Debug.Log(fps);
        }


    }

    public void UpdateCounter(BloopDNA bloopDNA)
    {
        creatureFinishedCounter++;
        bloopDNAList.Add(bloopDNA);
        if(creatureFinishedCounter == creaturesPerGeneration){
            Breed();
        }
    }

    public void Breed(){
        int count = bloopDNAList.Count;
        int lowIndex = 0;
        float lowFitness = 0f;
        int highIndex = 0;
        float highFitness = 0f;
        for (int i = 0; i < count; i++)
        {
            if (bloopDNAList[i].fitness > highFitness)
            {
                highIndex = i;
                highFitness = bloopDNAList[i].fitness;
            }
            if (bloopDNAList[i].fitness < lowFitness)
            {
                lowIndex = i;
                lowFitness = bloopDNAList[i].fitness;
            }
            //Debug.Log(bloopDNAList[i].fitness);
        }

        GameObject g = new GameObject();

        g = (GameObject)Instantiate(bloopCreaturePrefab);
        g.SendMessage("ActivateWithDNA", bloopDNAList[highIndex]);
        g.transform.parent = transform;

        g = (GameObject)Instantiate(bloopCreaturePrefab);  
        g.SendMessage("ActivateWithDNA", bloopDNAList[lowIndex]);
        g.transform.parent = transform;

        Debug.Log("Highest: " + highFitness + ", Lowest: " +lowFitness);
    }
}
