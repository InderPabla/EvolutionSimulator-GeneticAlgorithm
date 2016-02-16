using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour {
    Rigidbody2D rig;
    Renderer ren;
    // Use this for initialization
    
	void Start () {
        rig = gameObject.GetComponent<Rigidbody2D>();
        ren = gameObject.GetComponent<Renderer>();
        ren.material.color = Color.red;


	}
	
	// Update is called once per frame
	void Update () {
        Vector2 vel = Vector2.zero;
        Vector3 pos = transform.position;
        /*vel.x = Map(0, 1, 4, -4, Mathf.PerlinNoise(n, 0));
        vel.y = Map(0, 1, 4, -4, Mathf.PerlinNoise(n, 0));
        n += 0.01f;*/


        /*if (Random.value > .5)
        {
            vel.x = 2 * 2 * Random.value;
        }
        else
        {
            vel.x = -2 * 2 * Random.value;
        }
        if (Random.value > .5)
        {
            vel.y = 2 * 2 * Random.value;
        }
        else
        {
            vel.y = -2 * 2 * Random.value;
        }*/
        float per1 = Mathf.PerlinNoise(pos.x,pos.y);
        float per2 = Mathf.PerlinNoise(pos.x, pos.y);
        float rand1 = Random.value;
        float rand2 = Random.value;

        vel.x = (per1 * 2f) - 1f*rand1;
        vel.y = (per1 * 2f) - 1f*rand2;

        rig.velocity = vel;
    }

    public float Map(float from, float to, float from2, float to2, float value)
    {
        if (value <= from2)
        {
            return from;
        }
        else if (value >= to2)
        {
            return to;
        }
        else
        {
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }
    }
}
