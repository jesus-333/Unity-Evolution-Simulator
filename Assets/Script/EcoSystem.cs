using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcoSystem : MonoBehaviour
{
    public float time_in_seconds = 120f;
    public Text timer, count_safe, count_not_safe;
    public GameObject creature_container;

    private int tot_creature;
    private float time_copy;
    private Vector3 start_safe_zone, end_safe_zone;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public void Start(){
        retrieveInformation();

        time_copy = time_in_seconds;
    }

    void Update()
    {

        if(time_in_seconds > 0){ // If there is time continue the countdown
            time_in_seconds -= Time.deltaTime;
        } else {
            time_in_seconds = 0;

            killOutsideSafeZone();

            time_in_seconds = time_copy;
        }

        displayInfo();

        // if (Input.GetKeyDown(KeyCode.R)){ copySurvivedBrain(); }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    public void retrieveInformation(){
        // Safe zone limits
        start_safe_zone = GameObject.Find("Script Container").GetComponent<Zone>().start_position;
        end_safe_zone = GameObject.Find("Script Container").GetComponent<Zone>().end_position;

        // Creatures counts
        tot_creature = GameObject.Find("Script Container").GetComponent<Spawn>().n_creature;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Methods related to display information

    // Convert time in minutes:seconds format.
    public string convertTimeInString(){
        float minutes = Mathf.FloorToInt(time_in_seconds / 60);
        float seconds = Mathf.FloorToInt(time_in_seconds % 60);

        return minutes + ":" + seconds + "\t";
    }

    public int countSafe(){
        int tot_safe = 0;
        for(int i = 0; i < creature_container.transform.childCount; i++){
            if(checkIfInsideSafeZone(creature_container.transform.GetChild(i).position)){
                tot_safe++;
            }

        }

        return tot_safe;
    }

    public bool checkIfInsideSafeZone(Vector3 position){
        float min_x, max_x, min_z, max_z;

        // Find min and max of x in coordinate of the safe zone
        if(start_safe_zone.x < end_safe_zone.x){
            min_x = start_safe_zone.x;
            max_x = end_safe_zone.x;
        } else {
            min_x = end_safe_zone.x;
            max_x = start_safe_zone.x;
        }

        // Find min and max of z in coordinate of the safe zone
        if(start_safe_zone.z < end_safe_zone.z){
            min_z = start_safe_zone.z;
            max_z = end_safe_zone.z;
        } else {
            min_z = end_safe_zone.z;
            max_z = start_safe_zone.z;
        }

        // Check position
        if(position.x >= min_x && position.x <= max_x){
            if(position.z >= min_z && position.z <= max_z){
                return true;
            } else return false;
        }
        return false;
    }

    public void displayInfo(){
        // Display remaining time
        timer.text = convertTimeInString();

        // Display count of creature inside/outside safe zone
        count_safe.text = "IN Safe: " + countSafe().ToString();
        count_not_safe.text = "OUT Safe: " + (tot_creature - countSafe()).ToString();

    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Methods related to natural selection

    /*
    Destroy all the creatures outside the safe zone
    */
    public void killOutsideSafeZone(){
        for(int i = 0; i < creature_container.transform.childCount; i++){
            if(!checkIfInsideSafeZone(creature_container.transform.GetChild(i).position)){
                Destroy(creature_container.transform.GetChild(i).gameObject);
            }

        }
    }

    public void copySurvivedBrain(){
        GameObject tmp_creatures_container = new GameObject();
        tmp_creatures_container.transform.name = "Temporary Survived Creatures Container";
        Brain tmp_brain;

        GameObject.Find("Script Container").GetComponent<Spawn>().spawnCreature(countSafe(), tmp_creatures_container);

        for(int i = 0; i < creature_container.transform.childCount; i++){
            // Retrieve brain of the survived creature
            tmp_brain = creature_container.transform.GetChild(i).gameObject.GetComponent<Brain>();

            // Removed brain with random init and add brain of the survived creature
            Destroy(tmp_creatures_container.transform.GetChild(i).gameObject.GetComponent<Brain>());

            UnityEditorInternal.ComponentUtility.CopyComponent(tmp_brain);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(tmp_creatures_container.transform.GetChild(i).gameObject);
        }

        // Kill old survived creatures
        for(int i = 0; i < creature_container.transform.childCount; i++){ Destroy( creature_container.transform.GetChild(i).gameObject);}

        // Move new creature inside the normal container
        foreach(Transform creature in creature_container.transform){creature.transform.parent = GameObject.Find("Creature Container").transform;}

        // Destroy temporary container
        Destroy(tmp_creatures_container);

    }

    /*
    Spawn new creatures. The number of new creatures is the number of creatures killed.
    */
    public void repopulate(){
        Spawn spawn_script = GameObject.Find("Script Container").GetComponent<Spawn>();
        int n_new_creature = spawn_script.n_creature - countSafe();
        spawn_script.spawnCreature(n_new_creature);
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
}
