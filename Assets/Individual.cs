using UnityEngine;
using System.Collections;

public class Individual : MonoBehaviour {
    float maxLife = 1f;
    float maxSpeed = 10f;
    float maxGravityScale = 10f;

    
    public float life = 0;

    public float[] genes = new float[4]; //x,y,gs,l
    public Vector3 targetPosition;
    public float fitness;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void Stop() {
        GetComponent<Rigidbody2D>().isKinematic = true;
        fitness = Vector3.Distance(transform.position, targetPosition);
        transform.parent.SendMessage("UpdateCounter");
        
    }

    public void RandomGenes() { 
        genes[0] = Random.Range(-maxSpeed, maxSpeed + 0.001f);
        genes[1] = Random.Range(-maxSpeed, maxSpeed + 0.001f);
        genes[2] = Random.Range(0f, maxGravityScale+0.001f);
        genes[3] = Random.Range(0f, maxLife+0.001f);
        ApplyGenes();

    }

    public void ApplyGenes() {
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.velocity = new Vector2(genes[0], genes[1]);
        r.gravityScale = genes[2];

        Invoke("Stop", genes[3]);
    }

    public void ApplyMutation(){
        /*int randomIndex = Random.Range(0,4);
        Debug.Log(randomIndex);
        int bit = Random.Range(0, 2);
        float ran = Random.Range(0f, 2f);
        int random = Random.Range(0, 21);
        if (randomIndex < 2){
            
            if (bit == 0)
            {
                genes[randomIndex] += ran;
            }
            else
            {
                genes[randomIndex] -= ran;
            }

            if (genes[randomIndex] < -3f)
                genes[randomIndex] = -3f;
            else if (genes[randomIndex] >3f)
                genes[randomIndex] = 3f;

            
            if (random == 3)
            {
                genes[randomIndex] = Random.Range(-3f, 4f); 
            }

        }
        else if(randomIndex == 2)
        {
            if (bit == 0)
            {
                genes[randomIndex] += ran;
            }
            else
            {
                genes[randomIndex] -= ran;
            }
            if (genes[randomIndex] < 0f)
                genes[randomIndex] = 0f;
            else if (genes[randomIndex] > 1f)
                genes[randomIndex] = 1f;

            if (random == 3)
            {
                genes[randomIndex] = Random.Range(0f, 2f);
            }
        }
        else
        {
            if (bit == 0)
            {
                genes[randomIndex] += ran;
            }
            else
            {
                genes[randomIndex] -= ran;
            }
            if (genes[randomIndex] < 0f)
                genes[randomIndex] = 0f;
            else if (genes[randomIndex] > 3f)
                genes[randomIndex] = 3f;

            if (random == 3)
            {
                genes[randomIndex] = Random.Range(0f, 4f);
            }
        }

        if (random == 20)
        {
            RandomGenes();
        }*/

        int randomIndex = Random.Range(0, 4);
        if (randomIndex < 2)
        {
            genes[randomIndex] = Random.Range(-maxSpeed, maxSpeed + 0.001f);
        }
        else if (randomIndex == 2)
        {
            genes[randomIndex] = Random.Range(0f, maxGravityScale + 0.001f);
        }
        else
        {
            genes[randomIndex] = Random.Range(0f, maxLife + 0.001f);
        }
        
    }
}
