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
    public GameObject linePrefab;
    //List<GameObject> line = new List<GameObject>();
    GameObject[] lineArray;
    int lineArrayIndex = 0;
    List<Vector3[]> verticesList = new List<Vector3[]>();
    Vector3 previousEnd;
    List<object[]> speciesList = new List<object[]>();
    float scaleX = 0.1f;
    float scaleY = 0.1f;
    float scaleAtX = 50;
    float scaleAtY = 50;
    float constantX = 3f;
    float constantY = 4f;
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

        Vector3[] linePointsX = new Vector3[2];
        Vector3[] linePointsY = new Vector3[2];
        GameObject lineX = (GameObject)Instantiate(linePrefab);
        GameObject lineY = (GameObject)Instantiate(linePrefab);
        LineRenderer lineRenX = lineX.GetComponent<LineRenderer>();
        LineRenderer lineRenY = lineY.GetComponent<LineRenderer>();
        lineRenX.SetWidth(0.075f,0.075f);
        lineRenY.SetWidth(0.075f, 0.075f);
        linePointsX[0] = new Vector3(0*scaleX + constantX, 0 *scaleY + constantY, 0);
        linePointsX[1] = new Vector3(scaleAtX*scaleX + constantX, 0*scaleY + constantY, 0);
        linePointsY[0] = new Vector3(0 * scaleX + constantX, 0 * scaleY + constantY, 0);
        linePointsY[1] = new Vector3(0 * scaleX + constantX, scaleAtY * scaleY + constantY, 0);
        lineRenX.SetPosition(0, linePointsX[0]);
        lineRenX.SetPosition(1, linePointsX[1]);
        lineRenY.SetPosition(0, linePointsY[0]);
        lineRenY.SetPosition(1, linePointsY[1]);

        lineArray = new GameObject[27];
        lineArrayIndex = 2;
        lineArray[0] = lineX;
        lineArray[1] = lineX;
        /*line.Add(lineX);
        line.Add(lineY);*/

        verticesList.Add(linePointsX);
        verticesList.Add(linePointsY);

        previousEnd = new Vector3(constantX, constantY, 0);

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
            if (numberOfSpecies[node].Length <= muscles || muscles == -1)
            {
                Debug.Log("Issued about to happen: " + muscles + " " + numberOfSpecies[node].Length);
            }
            else
            {
                numberOfSpecies[node][muscles]++;
            }
        }
        
        speciesData.text = "[Generation:" + generationNumber + "]\n Best specie: " + bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration - 1, 0]].speciesName + ", Dist: " + rankedBloopDNAFitness[creaturesPerGeneration - 1, 1] +" Parent Specie: "+ bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration - 1, 0]].parentSpecieName+ "\n";

        Vector3 newEnd = new Vector3((float)(lineArrayIndex-1) *scaleX + constantX, rankedBloopDNAFitness[creaturesPerGeneration - 1, 1] * scaleY + constantY, 0);
        Vector3[] linePoints = {previousEnd,newEnd};
        GameObject newLine = (GameObject)Instantiate(linePrefab);


        //line.Add(newLine);
        Destroy(lineArray[lineArrayIndex]);
        lineArray[lineArrayIndex] = newLine;

        verticesList.Add(linePoints);
        LineRenderer newLineRen = newLine.GetComponent<LineRenderer>();
        newLineRen.SetPosition(0, previousEnd);
        newLineRen.SetPosition(1, newEnd);
        newLineRen.SetWidth(0.075f, 0.075f);
        previousEnd = newEnd;

        lineArrayIndex++;
        if (lineArrayIndex == 27)
        {
            lineArrayIndex = 2;
            previousEnd.x = (float)(lineArrayIndex - 1) * scaleX + constantX;
        }
       

        /*if (generationNumber == scaleAtX)
        {
            float tempScaleX = scaleX;
            float tempScaleY = scaleY;
            scaleX *= (scaleAtX / (scaleAtX + 5));
            scaleAtX += 5;
            
            
            for (int i = 2; i < line.Count; i++)
            {
                LineRenderer lineRen = line[i].GetComponent<LineRenderer>();
                linePoints = verticesList[i];
                linePoints[0] -= new Vector3(constantX,constantY,0);
                linePoints[1] -= new Vector3(constantX, constantY, 0);

                linePoints[0].x /= tempScaleX;
                linePoints[0].y /= tempScaleY;
                linePoints[1].x /= tempScaleX;
                linePoints[1].y /= tempScaleY;
                linePoints[0].x *= scaleX;
                linePoints[0].y *= scaleY;
                linePoints[1].x *= scaleX;
                linePoints[1].y *= scaleY;

                linePoints[0] += new Vector3(constantX, constantY, 0);
                linePoints[1] += new Vector3(constantX, constantY, 0);
                verticesList[i][0] = linePoints[0];
                verticesList[i][1] = linePoints[1];
                lineRen.SetPosition(0,linePoints[0]);
                lineRen.SetPosition(1, linePoints[1]);
            }
            previousEnd -= new Vector3(constantX, constantY, 0);
            previousEnd.x /= tempScaleX;
            previousEnd.y /= tempScaleY;
            previousEnd.x *= scaleX;
            previousEnd.y *= scaleY;
            previousEnd += new Vector3(constantX, constantY, 0);
        }*/

        speciesList.Clear();
        for (int i = 0; i < numberOfSpecies.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < numberOfSpecies[i].Length; j++)
            {
                int count = numberOfSpecies[i][j];
                if (count > 0)
                {
                    int node = i ;
                    int muscle = j;
                    speciesData.text += " | s" + node + "-" + muscle + ": " + count;
                    
                    object[] speciesInfo = {count, "s" + node + "-" + muscle };
                    speciesList.Add(speciesInfo);

                    found = true;
                }
            }
            if(found)
                speciesData.text += "\n";
        }

        //Debug.Log("Best Distance: " + rankedBloopDNAFitness[creaturesPerGeneration - 1, 1] + ", Species: " + bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration - 1, 0]].speciesName + ", Generation: " + generationNumber);


        if (bestBloopDNASize == bestBloopDNA.Length)
        {
            bestBloopDNASize = 0;
        }
        bestBloopDNA[bestBloopDNASize] = bloopDNAList[(int)rankedBloopDNAFitness[creaturesPerGeneration-1, 0]];
        bestBloopDNA[bestBloopDNASize].visible = true;
        bestBloopDNASize++;
        
        if (/*generationNumber < 50*/rankedBloopDNAFitness[creaturesPerGeneration - 1, 1]<50f) 
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
            }
        }
        
    }

    public void BubbleSortSpeciesList()
    {
        object[] speciesInfo = new object[2];
        bool swapped = true;
        int j = 0;
        while (swapped)
        {
            swapped = false;
            j++;
            for (int i = 0; i < creaturesPerGeneration - j; i++)
            {
                if ((int)speciesList[i][0] > (int)speciesList[i+1][0])
                {
                    speciesInfo[0] = speciesList[i][0];
                    speciesInfo[1] = speciesList[i][1];

                    speciesList[i][0] = speciesList[i + 1][0];
                    speciesList[i][1] = speciesList[i + 1][1];

                    speciesList[i + 1][0] = speciesInfo[0];
                    speciesList[i + 1][1] = speciesInfo[1];
                    swapped = true;
                }
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