using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_CustomerSpawn : MonoBehaviour
{
    public List<GameObject> CustomerPrefab;
    public float timer;
    public bool hasSpawned;
    public Transform MAINSHOP;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(60f, 120f);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0 && hasSpawned == false)
        {
            hasSpawned = true;
            SpawnCustomer();
            timer = Random.Range(60f, 120f);
        }
    }

    //Spawn Random Customer with random Prefab
    private void SpawnCustomer()
    {
        int ran = Random.Range(0, CustomerPrefab.Count);
        GameObject Customer = Instantiate(CustomerPrefab[ran], MAINSHOP.transform);
        Customer.GetComponent<Shop_Customer>().prefabNum = ran;
    }
    
    //Spawn Quest NPC after certain days in QuestManager
    public void SpawnQuestNPC(int customerType, Script_Quest quest)
    {
        GameObject Customer = Instantiate(CustomerPrefab[customerType], MAINSHOP.transform);
        Customer.GetComponent<Shop_Customer>().isAccepted = true;
        Customer.GetComponent<Shop_Customer>().quest = quest;
    }
}
