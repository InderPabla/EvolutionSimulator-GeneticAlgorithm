using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloopDNA
{
    int minNodes = 3;//3
    int maxNodes = 10; //10
    float[] xBoundary = {-2f,2f};
    float[] yBoundary = {0f,4f};
    float maxBouncy = 0.1f;
    float percentChanceToGainMuscle = 33;

    public int numberOfNodes;
    public List<List<int>> multiNodeMuscularConnectionIndices = new List<List<int>>();
    public List<List<float[]>> multiNodeMuscularData = new List<List<float[]>>();
    public float[,] nodeData;
    public int numberOfMuscles = 0;
    public float fitness = 0f;
    public bool visible = true;
    public string speciesName = "";
    public string parentSpecieName = "";
    public BloopDNA()
    {
        
    }

    public BloopDNA(int numOfNodes, List<List<int>> mNodeMuscularConnectionIndices, List<List<float[]>> mNodeMuscularData, float[,] data, int numOfMuscles, float fit)
    {
        this.numberOfNodes = numOfNodes;
        this.numberOfMuscles = numOfMuscles;
        this.nodeData = new float[numberOfNodes, 4];

        for (int i = 0; i < numberOfNodes; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                this.nodeData[i, j] = data[i, j];
            }
        }

        this.fitness = fit;

        for (int i = 0; i < numberOfNodes; i++)
        {
            List<int> sNodeMuscularConnectionIndex = mNodeMuscularConnectionIndices[i];
            List<int> singleNodeMuscularConnectionIndex = new List<int>();
            for (int j = 0; j < sNodeMuscularConnectionIndex.Count; j++)
            {
                int index = sNodeMuscularConnectionIndex[j];
                singleNodeMuscularConnectionIndex.Add(index);
            }
            multiNodeMuscularConnectionIndices.Add(singleNodeMuscularConnectionIndex);
        }

        for (int i = 0; i < numberOfNodes; i++)
        {
            List<float[]> sNodeMuscularData = mNodeMuscularData[i];
            List<float[]> singleNodeMuscularData = new List<float[]>();
            for (int j = 0; j < sNodeMuscularData.Count; j++)
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
        speciesName = "s" + numberOfNodes + "-" + numberOfMuscles;
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
                    int chance = Random.Range(0, 100);/* Random.Range(1,maxNodes);*/
                    if(chance <= percentChanceToGainMuscle /*chance < (maxNodes / 2)*/)
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


            nodeData[i, 0] = Random.Range(xBoundary[0], xBoundary[1]); 
            nodeData[i, 1] = Random.Range(yBoundary[0], yBoundary[1]); 

            nodeData[i, 2] = Random.Range(0f,1f); //friction
            nodeData[i, 3] = Random.Range(0f, maxBouncy); //bouncyness
        }
        speciesName = "s" + numberOfNodes + "-" + numberOfMuscles;
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

        bloopDNACrossover[0].parentSpecieName = speciesName;
        bloopDNACrossover[1].parentSpecieName = speciesName;

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
        int randomNodeIndex = Random.Range(0, numberOfNodes);
        
        if (Random.Range(0, 4) == 0)
        {
            
            int randomExtremeMutation = Random.Range(0,2);
            bool extremeMutation = false;
            if (randomExtremeMutation == 0 && numberOfNodes < maxNodes)
            {
                MutationAddNode();
                extremeMutation = true;
            }

            if (randomExtremeMutation == 1 && numberOfNodes > minNodes)
            {
                MutationDisconnectNode(randomNodeIndex);
                extremeMutation = true;
            }

            if (extremeMutation == false)
                MutateNodeProperty(randomNodeIndex);
        
            speciesName = "s" + numberOfNodes + "-" + numberOfMuscles;
        }
        else
        {
            MutateNodeProperty(randomNodeIndex);
        }      
    }

    public void MutationRemoveMuscle(int index)
    {
        if (multiNodeMuscularConnectionIndices[index].Count > 0)
        {
            int index1 = Random.Range(0, multiNodeMuscularConnectionIndices[index].Count);
            multiNodeMuscularData[index].RemoveAt(index1);
            multiNodeMuscularConnectionIndices[index].RemoveAt(index1);
            numberOfMuscles--;
        }
    }

    public void MutationDisconnectNode(int nodeIndex)
    {
        
        for (int i = 0; i< multiNodeMuscularConnectionIndices[nodeIndex].Count; i++)
        {
            multiNodeMuscularConnectionIndices[nodeIndex].RemoveAt(i);
            multiNodeMuscularData[nodeIndex].RemoveAt(i);
            numberOfMuscles--;
        }
        for (int i = 0; i < multiNodeMuscularConnectionIndices.Count; i++)
        {
            if (i != nodeIndex)
            {
                for (int j = 0; j < multiNodeMuscularConnectionIndices[i].Count; j++)
                {
                    if (multiNodeMuscularConnectionIndices[i][j] == nodeIndex)
                    {
                        multiNodeMuscularConnectionIndices[i].RemoveAt(j);
                        multiNodeMuscularData[i].RemoveAt(j);
                        numberOfMuscles--;
                    }
                }
            }         
        }

        for (int i = 0; i < multiNodeMuscularConnectionIndices.Count; i++)
        {
            for (int j = 0; j < multiNodeMuscularConnectionIndices[i].Count; j++)
            {
                if (multiNodeMuscularConnectionIndices[i][j] > nodeIndex)
                {
                    multiNodeMuscularConnectionIndices[i][j]--;
                }
            } 
        }
        multiNodeMuscularConnectionIndices.RemoveAt(nodeIndex);
        multiNodeMuscularData.RemoveAt(nodeIndex);

        List<float[]> newNodeDataList = new List<float[]>();
        
        for (int i = 0; i < numberOfNodes; i++)
        {
            float[] data = new float[4];
            for(int j = 0; j < 4; j++)
            {
                data[j] = nodeData[i,j];
            }
            newNodeDataList.Add(data);
        }
        newNodeDataList.RemoveAt(nodeIndex);
        numberOfNodes--;
        nodeData = new float[numberOfNodes,4];
        for (int i = 0; i < numberOfNodes; i++)
        {
            float[] data = newNodeDataList[i];
            for (int j = 0; j < 4; j++)
            {
                nodeData[i,j] = data[j];
            }
        }
    }

    public void MutationAddNode()
    {
        numberOfNodes++;
        float[,] newNodeData = new float[numberOfNodes,4];

        newNodeData[numberOfNodes - 1, 0] = Random.Range(xBoundary[0], xBoundary[1]); 
        newNodeData[numberOfNodes - 1, 1] = Random.Range(yBoundary[0], yBoundary[1]); 
        newNodeData[numberOfNodes - 1, 2] = Random.Range(0f, 1f); //friction
        newNodeData[numberOfNodes - 1, 3] = Random.Range(0f, maxBouncy); //bouncyness

        for (int i = 0; i < numberOfNodes - 1; i++)
        {
            for (int j = 0; j <4; j++)
            {
                newNodeData[i,j] = nodeData[i,j];
            }
        }

        nodeData = new float[numberOfNodes, 4];

        for (int i = 0; i < numberOfNodes; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                nodeData[i, j] = newNodeData[i, j];
            }
        }

        List<int> singleNodeMuscularConnectionIndex = new List<int>();
        List<float[]> singleNodeMuscularData = new List<float[]>();
        for(int i = 0; i<numberOfNodes - 1; i++)
        {
            int chance = Random.Range(0, 100);/* Random.Range(1,maxNodes);*/
            if (chance <= percentChanceToGainMuscle /*chance < (maxNodes / 2)*/)
            {
                singleNodeMuscularConnectionIndex.Add(i);
                float minDistance = Random.Range(0f, 2f);
                float maxDistance = Random.Range(0f, 2f);
                float temp = maxDistance;
                if (maxDistance < minDistance)
                {
                    maxDistance = minDistance;
                    minDistance = temp;
                }
                float[] muscleData = { minDistance, maxDistance, Random.Range(0f, 1f), Random.Range(0f, 3f) };//minDistance, maxDistance, damping, freq
                singleNodeMuscularData.Add(muscleData);
                numberOfMuscles++;
            }
        }
        
        multiNodeMuscularConnectionIndices.Add(singleNodeMuscularConnectionIndex);
        multiNodeMuscularData.Add(singleNodeMuscularData);

        
        for (int i = 0; i<numberOfNodes - 1; i++)
        {
            int j = numberOfNodes - 1;
            int chance = Random.Range(0, 100);/* Random.Range(1,maxNodes);*/
            if (chance <= percentChanceToGainMuscle /*chance < (maxNodes / 2)*/)
            {
                multiNodeMuscularConnectionIndices[i].Add(j);
                float minDistance = Random.Range(0f, 2f);
                float maxDistance = Random.Range(0f, 2f);
                float temp = maxDistance;
                if (maxDistance < minDistance)
                {
                    maxDistance = minDistance;
                    minDistance = temp;
                }
                float[] muscleData = { minDistance, maxDistance, Random.Range(0f, 1f), Random.Range(0f, 3f) };//minDistance, maxDistance, damping, freq
                multiNodeMuscularData[i].Add(muscleData);
                numberOfMuscles++;
            }
        }
    }

    public void MutateNodeProperty(int nodeIndex)
    {
        int thingToMutate = Random.Range(0, 2);

        if (thingToMutate == 1 || multiNodeMuscularData[nodeIndex].Count == 0)
        {
            int index = Random.Range(0, 4);
            if (index == 0)
            {
                nodeData[nodeIndex, index] = Random.Range(xBoundary[0], xBoundary[1]);
            }
            else if (index == 1)
            {
                nodeData[nodeIndex, index] = Random.Range(yBoundary[0], yBoundary[1]);
            }
            else if (index == 2)
            {
                nodeData[nodeIndex, index] = Random.Range(0f, 1f);
            }
            else if (index == 3)
            {
                nodeData[nodeIndex, index] = Random.Range(0f, maxBouncy);
            }
        }
        else
        {

            if (multiNodeMuscularData[nodeIndex].Count > 0)
            {
                int index1 = Random.Range(0, multiNodeMuscularData[nodeIndex].Count);
                int index2 = Random.Range(0, 4);
                if (index2 == 0)
                {
                    multiNodeMuscularData[nodeIndex][index1][index2] = Random.Range(0f, 2f);
                }
                else if (index2 == 1)
                {
                    multiNodeMuscularData[nodeIndex][index1][index2] = Random.Range(0f, 2f);
                }
                else if (index2 == 2)
                {
                    multiNodeMuscularData[nodeIndex][index1][index2] = Random.Range(0f, 1f);
                }
                else if (index2 == 3)
                {
                    multiNodeMuscularData[nodeIndex][index1][index2] = Random.Range(0f, 3f);
                }
            }
        }
    }

    void printMuscularConnectionIndicies(){
        for (int i = 0; i < numberOfNodes; i++)
        {
            List<int> singleNodeMuscularConnections = multiNodeMuscularConnectionIndices[i];
            string conData = "Index: " + i + "- ";
            for (int j = 0; j < singleNodeMuscularConnections.Count; j++)
            {

                conData += singleNodeMuscularConnections[j] + " ";
            }
            Debug.Log(conData);
        }
    }

}
