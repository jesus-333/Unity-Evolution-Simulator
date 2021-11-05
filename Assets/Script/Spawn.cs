using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int n_food = 10, n_creature = 10, x_limit = 10, z_limit = 10;

    public GameObject creature_prefab, food_prefab, creature_container, food_container;

    void Start()
    {
        spawnFood();
        spawnCreature();
    }

    void Update()
    {

    }

    public void spawnFood(){
        Vector3 random_position;
        for(int i = 0; i < n_food; i++){
            random_position = new Vector3(Random.Range(-x_limit, x_limit), 0, Random.Range(-z_limit, z_limit));
            Instantiate(food_prefab, random_position, Quaternion.identity, food_container.transform);
        }
    }

    public void spawnCreature(){
        Vector3 random_position;
        for(int i = 0; i < n_food; i++){
            random_position = new Vector3(Random.Range(-x_limit, x_limit), 2, Random.Range(-z_limit, z_limit));
            Instantiate(creature_prefab, random_position, Quaternion.identity, creature_container.transform);
        }
    }
}
