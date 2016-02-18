using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloopDNA
{
    int minNodes = 3;
    int maxNodes = 6; //10
    float[] xBoundary = {-2f,2f};
    float[] yBoundary = {0f,2f};


    public int numberOfNodes;
    public List<List<int>> multiNodeMuscularConnectionIndices = new List<List<int>>();
    public List<List<float[]>> multiNodeMuscularData = new List<List<float[]>>();
    public float[,] nodeData;
    public int numberOfMuscles = 0;
    public float fitness = 0f;

    public BloopDNA()
    {
        
    }

    public BloopDNA(int numOfNodes, List<List<int>> mNodeMuscularConnectionIndices, List<List<float[]>> mNodeMuscularData, float[,] data, int numOfMuscles, float fit)
    {
        this.numberOfNodes = numOfNodes;
        this.numberOfMuscles = numOfMuscles;
        this.nodeData = new float[numberOfNodes,4];

        for (int i = 0; i < numberOfNodes; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                this.nodeData[i, j] = data[i, j];
            }
        }

        this.fitness = fit;
        //Debug.Log("pau");
        for(int i = 0; i < numberOfNodes; i++)
        {
            List<int> sNodeMuscularConnectionIndex = mNodeMuscularConnectionIndices[i];
            List<int> singleNodeMuscularConnectionIndex = new List<int>();
            for (int j = 0; j< sNodeMuscularConnectionIndex.Count; j++)
            {
                int index = sNodeMuscularConnectionIndex[j];
                singleNodeMuscularConnectionIndex.Add(index);
            }
            multiNodeMuscularConnectionIndices.Add(singleNodeMuscularConnectionIndex);
        }

        //Debug.Log("paddddu");
        for (int i = 0; i < numberOfNodes; i++)
        {
            List<float[]> sNodeMuscularData = mNodeMuscularData[i];
            List<float[]> singleNodeMuscularData = new List<float[]>();
            for (int j = 0; j< sNodeMuscularData.Count; j++)
            {
                float[] muscleData = sNodeMuscularData[j];
                float[] newMuscleData = new float[muscleData.Length];
                for (int k = 0; k < 4; k++)
                {

                    newMuscleData[k] = muscleData[k];
                }
                singleNodeMuscularData.Add(newMuscleData);
            }
            multiNodeMuscularData.Add(singleNodeMuscularData);
        }
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
                        float temp = maxDistance;
                        if (maxDistance < minDistance)
                        {
                            maxDistance = minDistance;
                            minDistance = temp;
                        }
                        float[] muscleData = { minDistance, maxDistance, Random.Range(0f, 1f), Random.Range(0f, 3f)};//minDistance, maxDistance, damping, freq
                        singleNodeMuscularData.Add(muscleData);
                        numberOfMuscles++;
                    }
                }
            }
            multiNodeMuscularConnectionIndices.Add(singleNodeMuscularConnectionIndex);
            multiNodeMuscularData.Add(singleNodeMuscularData);

            float randomX = Random.Range(xBoundary[0], xBoundary[1]);
            float randomY = Random.Range(yBoundary[0], yBoundary[1]);
            nodeData[i, 0] = randomX;
            nodeData[i, 1] = randomY;

            nodeData[i, 2] = Random.Range(0f,1f); //friction
            nodeData[i, 3] = Random.Range(0f,0.1f); //bouncyness

        }

    }

    //Not doing gene crossover, but rather asexual reproduction
    public BloopDNA[] Crossover(BloopDNA bloopDNA){
        BloopDNA[] bloopDNACrossover = new BloopDNA[2];

        /*
         The general idea of gene crossover for these creatures
         Gene = node_1, node_1_Data, node_1_#_of_Muscles, node_1_Muscle_Conn_Incidies_X, node_1_Muscle_X_minDist, node_1_Muscle_X_maxDist, node_1_Muscle_X_damp, node_1_Muscle_X_freq
         Example:
         Node A is connected to B, Node B is not connected to A.
         Node A Data:  [X: 1f, Y: 2f, Friction: 0.5f, Bouncyness: 0.1f]
            Node A - B Muscle Data: [minDist: 0.5f, maxDist: 1f, Damp: 0.25f, Freq: 2.5f]
         Node B Data:  [X: 0f, Y: 1f, Friction: 1f, Bouncyness: 0f]  
                 __________________   _____________________   ______________
         Gene = [1f, 2f, 0.5f, 0.1f,  0.5f, 1f, 0.25f, 2.5f,  0f, 1f, 1f, 0f]
                 ------------------   ---------------------   --------------                   
                    Muscle A data      A-B connection data     Muscle B data                                                                   

                      #ofNodes
         #OfMuscles = Σ(#ofMusclesOfNode[i])
                      i=1
                                           
         Total gene size = (#ofNodes + #OfMuscles)*4                                                                    
        */

        int thisBloopGeneSize = (numberOfNodes + numberOfMuscles)*4;
        int nodeCrossoverPoint = Random.Range(0,numberOfNodes);
        return bloopDNACrossover;
    }

    public BloopDNA[] AsexualReproduce()
    {
        BloopDNA[] bloopDNACrossover = new BloopDNA[2];

        bloopDNACrossover[0] = this.CopyDNA();
        bloopDNACrossover[1] = this.CopyDNA();

        if(Random.Range(0,100)<=5)
            bloopDNACrossover[0].Mutate();
        bloopDNACrossover[1].Mutate();
        return bloopDNACrossover;
    }

    public BloopDNA CopyDNA() {
        BloopDNA newDNA = new BloopDNA(numberOfNodes, 
                                       multiNodeMuscularConnectionIndices, 
                                       multiNodeMuscularData,
                                       nodeData,
                                       numberOfMuscles,
                                       fitness);

        return newDNA;
    }

    public void Mutate() {
        int randomNode = Random.Range(0, numberOfNodes);
        int thingToMutate = Random.Range(0,2);

        if(thingToMutate == 1 || multiNodeMuscularData[randomNode].Count == 0)
        {
            int index = Random.Range(0, 4);
            if(index == 0)
            {
                nodeData[randomNode,index] = Random.Range(xBoundary[0], xBoundary[1]);
            }
            else if (index == 1)
            {
                nodeData[randomNode, index] = Random.Range(yBoundary[0], yBoundary[1]);
            }
            else if (index == 2)
            {
                nodeData[randomNode, index] = Random.Range(0f, 1f);
            }
            else if (index == 3)
            {
                nodeData[randomNode, index] = Random.Range(0f, 0.1f);
            }
        }
        else
        {
            
            if (multiNodeMuscularData[randomNode].Count > 0)
            {
                int index1 = Random.Range(0, multiNodeMuscularData[randomNode].Count);
                int index2 = Random.Range(0, 4);
                if (index2 == 0)
                {
                    multiNodeMuscularData[randomNode][index1][index2] = Random.Range(0f, 2f);
                }
                else if (index2 == 1)
                {
                    multiNodeMuscularData[randomNode][index1][index2] = Random.Range(0f, 2f);
                }
                else if (index2 == 2)
                {
                    multiNodeMuscularData[randomNode][index1][index2] = Random.Range(0f, 1f);
                }
                else if (index2 == 3)
                {
                    multiNodeMuscularData[randomNode][index1][index2] = Random.Range(0f, 3f);
                }
            }
        }
    }

    void printMuscularConnectionIndicies(){
        for (int i = 0; i < numberOfNodes; i++)
        {
            List<int> singleNodeMuscularConnections = multiNodeMuscularConnectionIndices[i];
            string conData = "Index: " + i + ", Location X: " + nodeData[i, 0] + ", Location Y: " + nodeData[i, 1] + ", ";
            for (int j = 0; j < singleNodeMuscularConnections.Count; j++)
            {

                conData += singleNodeMuscularConnections[j] + " ";
            }
            Debug.Log(conData);
        }
    }

    

}
