using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brain : MonoBehaviour
{
    public DNA dna;
    
    public int DNALength = 1; // Initializing length of DNA/
  
    public float fuel = 100;
    public bool coll = false;
   
    public float timeAlive; // To log how much time character is alive.
    bool alive = true;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    NavMeshAgent agent;
    Transform target;

   

//  Initialise the agent Navmesh
    public void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
    }

    
   
    // Manage the collision and delete the fuel.instance when collided
   //  Breeding the agents when they collide
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Fuel")
        {
           
            coll = true;
            fuel += 10;
           
            target = collision.gameObject.transform;

            if (coll==true)
            {
                agent.SetDestination(target.position);
                Destroy(collision.gameObject);
             
            }
          
        }      
        else
        {
            coll = false;
        }
        
    }
   

        public void Init()
    {

        /* initialise DNA
        0 forward
        1 back
        2 left
        3 right
        4 
        5 
        */

        dna = new DNA(DNALength, 4);     
        timeAlive = 0;
        
    }
  
    
    private void FixedUpdate()
    {
        // read DNA
        float h = 1;
        float v = 1;   
        if (dna.GetGene(0) == 0)     v += 10;
        else if (dna.GetGene(0) == 1) v += 20;
        else if (dna.GetGene(0) == 2) h += 20;
        else if (dna.GetGene(0) == 3) h += 10;

        if (alive)
        {
            timeAlive += Time.deltaTime;
        }
        fuel -= 1 * Time.deltaTime;

        if (coll == false)
        {


            if (isWandering == false)
            {
                StartCoroutine(Wander());
            }
            if (isRotatingRight == true)
            {
                transform.Rotate((transform.up *h)* Time.deltaTime );

            }
            if (isRotatingLeft == true)
            {
                transform.Rotate((transform.up *-h) * Time.deltaTime );

            }
            if (isWalking == true)
            {
                transform.position += (transform.forward * v) * Time.deltaTime;
            }
        }
    }

    // The agent wander function
    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotateInter = Random.Range(1, 2);
        int rotateLorR = Random.Range(1, 2);
        int WalkTime = Random.Range(0,2);
        int walkWait = Random.Range(1, 2);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(WalkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateInter);

        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }

        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }

        isWandering = false;

    }
}
    