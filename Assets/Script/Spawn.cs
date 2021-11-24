using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int n_food = 10, n_creature = 10, x_limit = 10, z_limit = 10;

    public GameObject creature_prefab, food_prefab, creature_container, food_container;

    void Start()
    {
        spawnFood(n_food);
        spawnCreature(n_creature, creature_container);
    }

    void Update()
    {

    }

    public void spawnFood(int n){
        Vector3 random_position;
        for(int i = 0; i < n; i++){
            random_position = new Vector3(Random.Range(-x_limit, x_limit), 0, Random.Range(-z_limit, z_limit));
            Instantiate(food_prefab, random_position, Quaternion.identity, food_container.transform);
        }
    }

    public void spawnCreature(int n, GameObject creature_container){
        Vector3 random_position, objective;
        GameObject tmp_creature;

        // Find objective as the center of the safe zone
        objective = (GameObject.Find("Script Container").GetComponent<Zone>().start_position + GameObject.Find("Script Container").GetComponent<Zone>().end_position) / 2;

        // Spawn creature
        float y = creature_prefab.transform.Find("Body").localScale.y;
        for(int i = 0; i < n; i++){
            random_position = new Vector3(Random.Range(-x_limit, x_limit), y, Random.Range(-z_limit, z_limit));
            tmp_creature = Instantiate(creature_prefab, random_position, Quaternion.identity, creature_container.transform);

            // Invoke init methods for the creatures (neurons creation, wiring creation etc)
            tmp_creature.GetComponent<Brain>().firstGenerationInit(objective);
        }
    }

    public void spawnCreature(int n){
        spawnCreature(n, creature_container);
    }


}
