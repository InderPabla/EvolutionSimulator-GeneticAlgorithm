using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BloopPopulation : MonoBehaviour
{
    double frameCount = 0;
    double dt = 0.0;
    double fps = 0.0;
    double updateRate = 4.0;  // 4 updates per sec.
    public GameObject bloopCreaturePrefab;
    public TextMesh speciesData;
    GameObject[] bloopCreatures;
    int creaturesPerGeneration = 100;
    int creatureFinishedCounter = 0;
    bool printFrames = false;
    List<BloopDNA> bloopDNAList = new List<BloopDNA>();
    float[,] rankedBloopDNAFitness;
    int generationNumber = 1;
    private object mutex = new object();
    int numberOfCreaturesPerRun = 10;

    BloopDNA[] bestBloopDNA = new BloopDNA[5];
    int bestBloopDNASize = 0;

    //List<int[]> numberOfSpecies = new List<int[]>();

    void Start()
    {
        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        System.Type type = assembly.GetType("UnityEditorInternal.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);

        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();
        Time.timeScale = 100f;
        bloopCreatures = new GameObject[creaturesPerGeneration];
        rankedBloopDNAFitness = new float[creaturesPerGeneration,2];

        Application.runInBackground = true;

        /*for (int i = 1; i <= 10; i++)
        {
            int[] muscles = new int[(i * (i - 1))+1];
            numberOfSpecies.Add(muscles);
        }*/

        for (int i = creatureFinishedCounter; i < (creatureFinishedCounter + numberOfCreaturesPerRun); i++)
        {
            bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
            bloopCreatures[i].transform.parent = transform;
            bloopCreatures[i].SendMessage("Activate");
        }

        
    }

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
        lock (mutex)
        {
            creatureFinishedCounter++;
            //Debug.Log(creatureFinishedCounter);
            if(generationNumber == 1)
                bloopDNAList.Add(bloopDNA);

            if (creatureFinishedCounter == creaturesPerGeneration)
            {
                Breed();
            }
            else if (creatureFinishedCounter % numberOfCreaturesPerRun == 0)
            {
                if (generationNumber == 1)
                {
                    for (int i = creatureFinishedCounter; i < (creatureFinishedCounter + numberOfCreaturesPerRun); i++)
                    {
                        bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
                        bloopCreatures[i].transform.parent = transform;
                        bloopCreatures[i].SendMessage("Activate");
                    }
                }
                else
                {
                    for (int i = creatureFinishedCounter; i < (creatureFinishedCounter + numberOfCreaturesPerRun); i++)
                    {
                        bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
                        bloopCreatures[i].transform.parent = transform;
                        bloopCreatures[i].SendMessage("ActivateWithDNA", bloopDNAList[i]);
                    }
                }

            }
        }
        
    }

    
    public void Breed(){
        List<BloopDNA> newGenerationBloopDNAList = new List<BloopDNA>();

        int indexCounter = 0;

        for (int i = 0; i < creaturesPerGeneration; i++) {
            rankedBloopDNAFitness[i, 0] = i;
            rankedBloopDNAFitness[i, 1] = bloopDNAList[i].fitness;
            Destroy(bloopCreatures[i]);
        }
        
        BubbleSortBloopDNA();

        List<int[]> numberOfSpecies = new List<int[]>();
        for (int i = 0; i <= 10; i++)
        {
            int[] muscles = new int[(i * (i - 1)) + 1];
            numberOfSpecies.Add(muscles);
        }

        for (int i = 0; i < bloopDNAList.Count; i++)
        {
            int node = bloopDNAList[i].numberOfNodes;
            int muscles = bloopDNAList[i].numberOfMuscles;
            numberOfSpecies[node][muscles]++;
        }
        speciesData.text = "Species Count\n";
        //Debug.Log(bloopDNAList.Count);
        for (int i = 0; i < numberOfSpecies.Count; i++)
        {
            for (int j = 0; j < numberOfSpecies[i].Length; j++)
            {
                int count = numberOfSpecies[i][j];
                if (count > 0)
                {
                    int node = i ;
                    int muscle = j;
                    speciesData.text += "s" + node + "-" + muscle + ": " + count + "\n";
                }
            }
        }
        Debug.Log("Best Distance: " + rankedBloopDNAFitness[creaturesPerGeneration - 1, 1] + ", Species: " + bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration - 1, 0]].speciesName + ", Generation: " + generationNumber);


        if (bestBloopDNASize == bestBloopDNA.Length)
        {
            bestBloopDNASize = 0;
        }
        bestBloopDNA[bestBloopDNASize] = bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration-1, 0]];
        bestBloopDNA[bestBloopDNASize].visible = true;
        bestBloopDNASize++;
        
        if (/*generationNumber < 50*/rankedBloopDNAFitness[creaturesPerGeneration - 1, 1]<75f) 
        { 
            int index = creaturesPerGeneration - 1;

            for (int i = 0; i < creaturesPerGeneration / 2; i++)
            {
                BloopDNA[] bloopDNAReproduce = bloopDNAList[(int)rankedBloopDNAFitness[index, 0]].AsexualReproduce();
                newGenerationBloopDNAList.Add(bloopDNAReproduce[0]);
                newGenerationBloopDNAList.Add(bloopDNAReproduce[1]);
                indexCounter += 2;
                index--;
            }

            bloopDNAList.Clear();
            for (int i = 0; i < creaturesPerGeneration; i++)
            {
                newGenerationBloopDNAList[i].visible = false;
                bloopDNAList.Add(newGenerationBloopDNAList[i]);
            }
            bloopDNAList[0].visible = true;
            bloopDNAList[1].visible = true;
            
            generationNumber++;
            creatureFinishedCounter = 0;

            for (int i = creatureFinishedCounter; i < (creatureFinishedCounter + numberOfCreaturesPerRun); i++)
            {
                bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
                bloopCreatures[i].transform.parent = transform;
                bloopCreatures[i].SendMessage("ActivateWithDNA", bloopDNAList[i]);
            }
        }
        else
        {
            Time.timeScale = 0.25f;
            for (int i = 0; i < 5; i++)
            {
                bloopCreatures[i] = (GameObject)Instantiate(bloopCreaturePrefab);
                bloopCreatures[i].transform.parent = transform;
                bloopCreatures[i].SendMessage("ActivateWithDNAForever", bestBloopDNA[i]);

                //bloopCreatures[i].transform.position = new Vector3(-10f+(i*2), 0, 0);
            }
        }
        
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