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
    float[,] rankedBloopDNAFitness;
    
    void Start()
    {
        bloopCreatures = new GameObject[creaturesPerGeneration];
        rankedBloopDNAFitness = new float[creaturesPerGeneration,2];
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

    public void UpdateCounter(BloopDNA bloopDNA){
        creatureFinishedCounter++;
        bloopDNAList.Add(bloopDNA);
        if(creatureFinishedCounter == creaturesPerGeneration){
            Breed();
        }
    }

    public void Breed(){
        Debug.Log(bloopDNAList.Count);
        List<BloopDNA> newGenerationBloopDNAList = new List<BloopDNA>();
        int[,] randomPairedIndcies = new int[(creaturesPerGeneration / 2), 2];
        int indexCounter = 0;

        for (int i = 0; i < creaturesPerGeneration; i++) {
            rankedBloopDNAFitness[i, 0] = i;
            rankedBloopDNAFitness[i, 1] = bloopDNAList[i].fitness;
        }
        
        BubbleSortBloopDNA();

        for (int i = 0;i< creaturesPerGeneration / 2; i++)
        {
            randomPairedIndcies[i, 0] = Random.Range((creaturesPerGeneration/2), creaturesPerGeneration);
            randomPairedIndcies[i, 1] = Random.Range((creaturesPerGeneration/2), creaturesPerGeneration);
        }

        for (int i = 0; i < creaturesPerGeneration / 2; i++)
        {
            BloopDNA[] bloopDNAReproduce = bloopDNAList[(int)randomPairedIndcies[i, 0]].AsexualReproduce();
            newGenerationBloopDNAList.Add(bloopDNAReproduce[0]);
            newGenerationBloopDNAList.Add(bloopDNAReproduce[1]);
            indexCounter += 2;
        }

        //bloopDNAList = newGenerationBloopDNAList;
        /*Debug.Log("here1");
        Debug.Log(bloopDNAList.Count);
        for (int i = 0; i < creaturesPerGeneration; i++)
        {
            Debug.Log("here12");
            bloopDNAList.RemoveAt(i);

        }
        Debug.Log("here2");*/
        bloopDNAList = new List<BloopDNA>();
        for (int i = 0; i < creaturesPerGeneration; i++)
        {
            bloopDNAList.Add(newGenerationBloopDNAList[i]);

        }
        creatureFinishedCounter = 0;

        for (int i = 0; i < creaturesPerGeneration; i++)
        {

            bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
            bloopCreatures[i].transform.parent = transform;
            bloopCreatures[i].SendMessage("ActivateWithDNA", bloopDNAList[i]);
        }

        /*for (int i = 0; i < creaturesPerGeneration/2; i++)
       {
           BloopDNA[] bloopDNACorssover = bloopDNAList[(int)randomPairedIndcies[i, 0]].Crossover(bloopDNAList[(int)randomPairedIndcies[i, 1]]);
           newGenerationBloopDNAList.Add(bloopDNACorssover[0]);
           newGenerationBloopDNAList.Add(bloopDNACorssover[1]);
           indexCounter += 2;
       }*/
        //CreateTheBest();

    }

    public void BubbleSortBloopDNA() {
        float[] tempFitness = new float[2];
        bool swapped = true;
        int j = 0;
        while (swapped)
        {
            swapped = false;
            j++;
            for (int i = 0; i < creaturesPerGeneration - j; i++)
            {
                if (rankedBloopDNAFitness[i, 1] > rankedBloopDNAFitness[i + 1, 1])
                {
                    tempFitness[0] = rankedBloopDNAFitness[i, 0];
                    tempFitness[1] = rankedBloopDNAFitness[i, 1];

                    rankedBloopDNAFitness[i, 0] = rankedBloopDNAFitness[i + 1, 0];
                    rankedBloopDNAFitness[i, 1] = rankedBloopDNAFitness[i + 1, 1];

                    rankedBloopDNAFitness[i + 1, 0] = tempFitness[0];
                    rankedBloopDNAFitness[i + 1, 1] = tempFitness[1];
                    swapped = true;
                }
            }
        }
    }

    public void CreateTheBest() { 
        GameObject g = new GameObject();

        g = (GameObject)Instantiate(bloopCreaturePrefab);
        g.SendMessage("ActivateWithDNA", bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration-1, 0]]);
        g.transform.parent = transform;

        g = (GameObject)Instantiate(bloopCreaturePrefab);
        g.SendMessage("ActivateWithDNA", bloopDNAList[(int)rankedBloopDNAFitness[0, 0]]);
        g.transform.parent = transform;
    }
    
}
