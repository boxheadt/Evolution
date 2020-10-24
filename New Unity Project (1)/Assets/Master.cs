using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Master : MonoBehaviour
{
    public GameObject agents;
    public GameObject fuel;
    private Vector3 screenBounds;
    public float respawnTime = 0f;
    public int enemyCount;
    public int fuelCount = 10;
    public int fuelNumber = 0;

   
  

    void Start()
    {
        screenBounds = new Vector3(9, 0, -9); 
        StartCoroutine(Wave());
        StartCoroutine(Fuel());
    }

    private void spawnAgents()
    {
        GameObject a = Instantiate(agents) as GameObject;
        a.transform.position = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y, Random.Range(-screenBounds.z, screenBounds.z));
        enemyCount++;
    }
    public void spawnFuel()
    {       
            GameObject f = Instantiate(fuel) as GameObject;
            f.transform.position = new Vector3(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y, Random.Range(-screenBounds.z, screenBounds.z));
             fuelNumber++;
        } 

        
        
        
   
    
    IEnumerator Wave()
    {
        while (enemyCount < 10)
        {
            yield return new WaitForSeconds(respawnTime);
            spawnAgents();
        }
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