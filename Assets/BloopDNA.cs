using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloopDNA
{
    int minNodes = 3;
    int maxNodes = 10;
    float[] xBoundary = {-2f,2f};
    float[] yBoundary = {0f,2f};

    /* 
    
    */
    public int numberOfNodes;
    public List<List<int>> multiNodeMuscularConnectionIndices = new List<List<int>>();
    public List<List<float[]>> multiNodeMuscularData = new List<List<float[]>>();
    public float[,] nodeData;
    public int numberOfMuscels = 0;
    public float fitness = 0f;
    public BloopDNA()
    {
        
    }


    public void GenerateRandomBloopDNA()
    {
        
        numberOfNodes = Random.Range(minNodes,maxNodes+1);
        nodeData = new float[numberOfNodes, 4];

        for (int i = 0; i < numberOfNodes; i++)
        {
            List<int> singleNodeMuscularConnectionIndex = new List<int>();
            List<float[]> singleNodeMuscularData = new List<float[]>();

            for (int j = 0; j < numberOfNodes; j++)
            {
                if (i != j)
                {
                    int chance = Random.Range(1,maxNodes);
                    if(chance < (maxNodes / 2))
                    {
                        singleNodeMuscularConnectionIndex.Add(j);
                        float minDistance = Random.Range(0f, 2f);
                        float maxDistance = Random.Range(0f, 2f);
                        float[] muscleData = { minDistance, maxDistance, Random.Range(0f, 1f), Random.Range(0f, 3f)};//minDistance, maxDistance, damping, freq
                        singleNodeMuscularData.Add(muscleData);
                        numberOfMuscels++;
                    }
                }
            }
            multiNodeMuscularConnectionIndices.Add(singleNodeMuscularConnectionIndex);
            multiNodeMuscularData.Add(singleNodeMuscularData);

            float randomX = Random.Range(xBoundary[0], xBoundary[1]);
            float randomY = Random.Range(yBoundary[0], yBoundary[1]);
            nodeData[i, 0] = randomX;
            nodeData[i, 1] = randomY;

            nodeData[i, 2] = Random.Range(0f,1f);
            nodeData[i, 3] = Random.Range(0f,0.1f);

        }

        

        /*for (int i = 0; i < numberOfNodes; i++)
        {
            List<int> singleNodeMuscularConnections = multiNodeMuscularConnectionIndices[i];
            string conData = "Index: " + i + ", Location X: " + nodeLocations[i, 0]+", Location Y: " + nodeLocations[i, 1]+", ";
            for (int j = 0; j < singleNodeMuscularConnections.Count; j++)
            {

                conData += singleNodeMuscularConnections[j] + " ";
            }
            Debug.Log(conData);
        }*/
    }
	
}
