using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcoSystem : MonoBehaviour
{
    public float time_in_seconds = 120f;
    public Text timer, count_safe, count_not_safe;
    public GameObject creature_container;

    private int tot_creature, creature_survived;
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
            // // Save the number of creatures survived
            // creature_survived = countSafe();
            //
            // // Kill all the creatures outside the safe zone
            // killOutsideSafeZone();
            //
            // // Copy the brains of the survived creatures in new creatures
            // copySurvived();
            //
            // // Spawn new creatures with random wiring
            // repopulate();
            //
            // // Reset the timer
            // time_in_seconds = time_copy;
        }

        displayInfo();

        if (Input.GetKeyDown(KeyCode.R)){

            creature_survived = countSafe();
            print("creature_survived = " + creature_survived);

            killOutsideSafeZone();

            copySurvived();

            repopulate();

            time_in_seconds = time_copy;
        }
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
        string tmp_time = "";
        float minutes = Mathf.FloorToInt(time_in_seconds / 60);
        float seconds = Mathf.FloorToInt(time_in_seconds % 60);

        if(minutes < 10){ tmp_time = tmp_time + "0" + minutes; }
        else {tmp_time = tmp_time + minutes; }

        if(seconds < 10){ tmp_time = tmp_time + ":0" + seconds; }
        else {tmp_time = tmp_time + ":" + seconds; }

        return tmp_time;
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
    Destroy all the creatures outside the safe zone.
    */
    public void killOutsideSafeZone(){
        for(int i = 0; i < creature_container.transform.childCount; i++){
            if(!checkIfInsideSafeZone(creature_container.transform.GetChild(i).position)){
                Destroy(creature_container.transform.GetChild(i).gameObject);
            }
        }
    }

    /*
    Kill all the creatures alive. (Can be combined with the killOutsideSafeZone() methods)
    */
    public void killAllCreatures(){
        for(int i = 0; i < creature_container.transform.childCount; i++){
            Destroy(creature_container.transform.GetChild(i).gameObject);
        }
    }

    /*
    Create a copy of the survived creatures. Method necessary due to the problem with child position and relocation.
    */
    public void copySurvived(){
        // Backup array with the brains of the survived creatures
        Brain[] backup_brains = new Brain[creature_survived];
        for(int i = 0; i < creature_survived; i++){
            backup_brains[i] = creature_container.transform.GetChild(i).gameObject.GetComponent<Brain>();
        }

        // Spawn new creature in numbers equal to the survived inside a temporary container
        GameObject tmp_creatures_container = new GameObject();
        tmp_creatures_container.transform.name = "Temporary Survived Creatures Container";
        GameObject.Find("Script Container").GetComponent<Spawn>().spawnCreature(creature_survived, tmp_creatures_container);

        // Upload the brain of the survived creatures
        for(int i = 0; i < tmp_creatures_container.transform.childCount; i++){
            tmp_creatures_container.transform.GetChild(i).gameObject.GetComponent<Brain>().InitCopyBrain(backup_brains[i]);
        }

        // Kill the original survived creatures
        killAllCreatures();

        // Move new creatures inside the normal container
        foreach(Transform creature in creature_container.transform){creature.transform.parent = GameObject.Find("Creature Container").transform;}

        // Destroy temporary container
        Destroy(tmp_creatures_container);
    }


    /*
    Spawn new creatures with random brains. The number of new creatures is the number of creatures killed.
    */
    public void repopulate(){
        Spawn spawn_script = GameObject.Find("Script Container").GetComponent<Spawn>();
        int n_new_creature = spawn_script.n_creature - creature_survived;
        spawn_script.spawnCreature(n_new_creature);
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
}
