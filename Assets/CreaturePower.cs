using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreaturePower : MonoBehaviour {

    Renderer ren;
    public GameObject creature;
    int counter = 0;
    // Use this for initialization
    public Transform target;
    GameObject[] game;
    int size = 2000;
    void Start()
    {
        ren = gameObject.GetComponent<Renderer>();
        ren.material.color = Color.red;

        game = new GameObject[size];
        for (int i = 0; i < size; i++)
        {
            game[i] = Instantiate(creature) as GameObject;
            game[i].SendMessage("RandomGenes") ;
            game[i].GetComponent<Individual>().targetPosition = target.position;
            game[i].transform.parent = transform;

        }

    }

    // Update is called once per frame
    void Update () {
	    
	}

    public void UpdateCounter() {
        counter++;
        if (counter == size)
        {
            Debug.Log("Done");
            Breed();
            /*int index1 = 0,index2 = 0 , index3 =0;
            float best1 = 10000000000000000000;
            float best2 = 10000000000000000000;
            float best3 = 10000000000000000000;

            for (int i = 0; i < size; i++)
            {
                float dis = game[i].GetComponent<Individual>().fitness ;


                if (dis <= best1)
                {
                    best3 = best2;
                    index3 = index2;

                    best2 = best1;
                    index2 = index1;

                    best1 = dis;
                    index1 = i;
                }
                else if (dis <= best2)
                {
                    best3 = best2;
                    index3 = index2;

                    best2 = dis;
                    index2 = i;
                }
                else if (dis <= best3)
                {
                    best3 = dis;
                    index3 = i;
                }
            }

            float[] genes1 = new float[4];
            float[] genes2 = new float[4];
            float[] genes3 = new float[4];

            genes1 = game[index1].GetComponent<Individual>().genes;
            genes2 = game[index2].GetComponent<Individual>().genes;
            genes3 = game[index3].GetComponent<Individual>().genes;

            for (int i = 0; i < size; i++){
                Destroy(game[i]);
            }

            int crossoverPoint1 = Random.Range(0,4);
            float[] newGenes = new float[4];

            for(int i = 0; i < crossoverPoint1; i++)
            {
                newGenes[i] = genes1[i];
            }

            for (int i = crossoverPoint1; i < 4; i++)
            {
                newGenes[i] = genes2[i];
            }
            counter = 0;

            for (int i = 0; i < size; i++)
            {
                game[i] = Instantiate(creature) as GameObject;
                Individual ind = game[i].GetComponent<Individual>();
                game[i].transform.parent = transform;

                for (int j = crossoverPoint1; j < 4; j++)
                {
                    ind.genes[j] = newGenes[j];   
                }
                ind.ApplyMutation();
                ind.ApplyGenes();
               
                 
            }*/

           
           
        }
    }

    void Breed() {
        float[,] oldGenes = new float[size, 4];
        float[,] newGenes = new float[size, 4];
        float[,] fit = new float[size, 2];

        for (int i = 0;i< size; i++){
            Individual individual = game[i].GetComponent<Individual>();
            fit[i,0] = i; 
            fit[i,1] = individual.fitness;

            for (int k = 0; k < 4; k++)
            {
                oldGenes[i, k] = individual.genes[k];
                newGenes[i, k] = individual.genes[k];
            }
        }
        float[] tmp = new float[2];
        bool swapped = true;
        int j = 0;
        int n = size;

        while (swapped)
        {
            swapped = false;
            j++;
            for (int i = 0; i < n - j; i++)
            {
                if (fit[i,1] > fit[i + 1,1])
                {
                    tmp[0] = fit[i,0];
                    tmp[1] = fit[i, 1];

                    fit[i,0] = fit[i + 1,0];
                    fit[i, 1] = fit[i + 1, 1];

                    fit[i + 1,0] = tmp[0];
                    fit[i + 1, 1] = tmp[1];
                    swapped = true;
                }
            }
        }

        int[,] pairedIndcies = new int[(size/2),2];
        List<int> list = new List<int>();
       
        for (int i = 0; i < size; i++)
            list.Add(i);
        
        for (int i = 0; i < (size / 2); i++)
        {
            /*int ran1 = Random.Range(0, list.Count);
            int ran2 = 0;
            int index1 = list.ToArray()[ran1];
            int index2 = index1;
            while (index1 == index2)
            {
                ran2 = Random.Range(0, list.Count);
                index2 = list.ToArray()[ran2];
            }

            list.RemoveAt(ran1);
            list.RemoveAt(ran2);

            pairedIndcies[i, 0] = index1;
            pairedIndcies[i, 1] = index2;*/

            pairedIndcies[i, 0] = Random.Range(0, size/2);
            pairedIndcies[i, 1] = Random.Range(0, size/2);

        }
        int indexCounter = 0;
        for (int i = 0; i < (size)/2; i++)
        {
            /*float randomX = Random.Range(0f,5f);
            float randomIndex = Mathf.Pow(randomX, 4.2920296742201791520103197062919f);
            //Debug.Log(randomIndex);
            int crossoverPoint = Random.Range(0, 4);

            for (int k = crossoverPoint; k < 4; k++)
            {
                newGenes[i,k] = newGenes[(int)randomIndex,k];
            }*/
            //int randomIndex = Random.Range(0, 501);

            int crossoverPoint = Random.Range(0, 4);
            int fitIndex1 = (int)fit[pairedIndcies[i, 0], 0];
            int fitIndex2 = (int)fit[pairedIndcies[i, 1], 0];
            for (int k = 0; k < crossoverPoint - 1; k++)
            {
                
                newGenes[indexCounter, k] = oldGenes[fitIndex1, k];
            }
            for (int k = crossoverPoint; k < 4; k++)
            {
                newGenes[indexCounter, k] = oldGenes[fitIndex2, k];
            }

            for (int k = 0; k < crossoverPoint - 1; k++)
            {
                newGenes[indexCounter+1, k] = oldGenes[fitIndex2, k];
            }
            for (int k = crossoverPoint; k < 4; k++)
            {
                newGenes[indexCounter+1, k] = oldGenes[fitIndex1, k];
            }

            indexCounter +=2;

        }

        for (int i = 0; i < size; i++)
        {
            Destroy(game[i]);
        }

        for (int i = 0; i < size; i++)
        {
            game[i] = Instantiate(creature) as GameObject;
            Individual ind = game[i].GetComponent<Individual>();
            ind.targetPosition = target.position;
            game[i].transform.parent = transform;

            for (int k = 0; k < 4; k++)
            {
                ind.genes[k] = newGenes[i,k];
            }

            int random = Random.Range(0, size);
            if(random< (float)size*(0.05f))
                ind.ApplyMutation();

            ind.ApplyGenes();
            

        }
        counter = 0;
    }



}
