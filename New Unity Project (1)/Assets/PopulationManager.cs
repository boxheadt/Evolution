using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
public class PopulationManager : MonoBehaviour
{
   
    public int populationSize;
    public float trialTime = 50f;
    public float elapsedTime;
    public float fuelTimer = 0;
    public int fuelCount = 10;
    public int fuelNumber = 0;
    public float respawnTime = 0f;

    public bool canMate = true;
    public float mateCooldown = 0f;
    private Vector3 screenBounds;
    private bool coll;
     
 public GameObject agentPrefab;
 public GameObject fuel;
        NavMeshAgent agent;
        Transform target;
        GUIStyle guiStyle = new GUIStyle();
        List<GameObject> population = new List<GameObject>();


    void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
       // GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
       // GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
       //  GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
        GUI.EndGroup();
    }


    void Start()
    {
        StartCoroutine(Fuel());
        agent = GetComponent<NavMeshAgent>();
        screenBounds = new Vector3(50, 0, -50);
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 startingPos = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y, Random.Range(-screenBounds.z, screenBounds.z));
            GameObject a = Instantiate(agentPrefab, startingPos, this.transform.rotation);
            a.GetComponent<Brain>().Init();
            population.Add(a);
            
        }

    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        fuelTimer += Time.deltaTime;


           

        if (fuelTimer >= 10)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject f = Instantiate(fuel) as GameObject;
                f.transform.position = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), 0.5f, Random.Range(-screenBounds.z, screenBounds.z));
                fuelTimer = 0;
            }
            
        }

        if (elapsedTime < mateCooldown)
        {
            canMate = false;
        }
        else
        {
            canMate = true;
        }
        

        for (int a = 0; a < population.Count; a++)
        {
            if (population[a].GetComponent<Brain>().fuel <= 0)
            {

                Debug.Log("Dead");
                Destroy(population[a].gameObject);
                population.RemoveAt(a);
                populationSize--; 
            }

            for (int b = 0; b < population.Count; b++)
            {                         
                float dist = Vector3.Distance(population[a].transform.position, population[b].transform.position);

                      if (dist <= 5 && dist != 0 && canMate == true)
                      {
                    
                    population.Add(Breed(population[a], population[b]));
                    elapsedTime = 0;
                         }


               }
        }
       
        

    }
   
    public GameObject Breed(GameObject parent1, GameObject parent2)
    {
        canMate = false;
        populationSize++;
        GameObject offspring = Instantiate(agentPrefab, (parent1.transform.localPosition - parent2.transform.position /2), this.transform.rotation);
        Brain b = offspring.GetComponent<Brain>();
        if (Random.Range(0, 10) == 1) //mutate 
        {
            b.Init();
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return offspring;
    }
   
    public void spawnFuel()
    {
       
            GameObject f = Instantiate(fuel) as GameObject;
            f.transform.position = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), 0.5f, Random.Range(-screenBounds.z, screenBounds.z));
            fuelNumber++;
            




    }


    IEnumerator Fuel()
    {
        while (fuelNumber < fuelCount)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnFuel();
        }
    }
}


