using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcoSystem : MonoBehaviour
{
    public float time_in_seconds = 120f;
    public Text timer, percentage_safe;
    public GameObject creature_container;

    private int tot_creatures, creature_survived;
    private float time_copy;
    private Vector3 start_safe_zone, end_safe_zone;

    public bool debug_var = false;
    [Range(0.0f, 10.0f)]
    public float time_speed = 1.0f;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

    public void Start(){
        retrieveInformation();
        if(debug_var){ print("START - tot_creatures = " + tot_creatures); }

        time_copy = time_in_seconds;
    }

    void Update()
    {

        if(time_in_seconds > 0){ // If there is time continue the countdown
            time_in_seconds -= Time.deltaTime;
        } else {
            naturalCycle();
        }

        displayInfo();

        //  - - - - - - - - - - - - -
        // Stuff for debug

        // Force the natural cycle when R is pressed
        if (Input.GetKeyDown(KeyCode.R)){
            naturalCycle();
        }

        // Change game speed
        Time.timeScale = time_speed;
    }

    public void naturalCycle(){
        // Save the number of creatures survived
        creature_survived = countSafe();
        if(debug_var){
            print("SAFE Creatures:\t" + creature_survived);
            print("TOTAL Creatures:\t" + creature_container.transform.childCount + " (Before killing)");
        }

        // Kill all the creatures outside the safe zone
        killOutsideSafeZone();
        if(debug_var){ print("TOTAL Creatures:\t" + creature_container.transform.childCount + " (After killing)");}

        // Spawn new creatures with the brains of the survived creatures
        if(debug_var){ print("START REPOPULATION"); }
        repopulate();
        if(debug_var){ print("END REPOPULATION"); }
        // if(debug_var){ print("TOTAL Creatures:\t" + creature_container.transform.childCount + " (After repopulation)");}

        // Reset the timer
        time_in_seconds = time_copy;
    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

    public void retrieveInformation(){
        // Safe zone limits
        start_safe_zone = GameObject.Find("Script Container").GetComponent<Zone>().start_position;
        end_safe_zone = GameObject.Find("Script Container").GetComponent<Zone>().end_position;

        // Creatures counts
        tot_creatures = GameObject.Find("Script Container").GetComponent<Spawn>().n_creature;
    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
        percentage_safe.text = "IN Safe: " + ((float)countSafe()/(float)(tot_creatures) * 100).ToString() + "%";
        // percentage_safe.text = "IN Safe: " + (float)countSafe();

        // Display total number of creatures
        GameObject.Find("Tot Creature Text").GetComponent<Text>().text = "Total: " + creature_container.transform.childCount;

        // Display the time speed
        time_speed = GameObject.Find("Time Speed Slider").GetComponent<Slider>().value;
        GameObject.Find("Time Speed Text").GetComponent<Text>().text = time_speed + "";
    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Methods related to natural selection

    /*
    Destroy all the creatures outside the safe zone.
    */
    public void killOutsideSafeZone(){
        for(int i = 0; i < creature_container.transform.childCount; i++){
            if(!checkIfInsideSafeZone(creature_container.transform.GetChild(i).position)){
                Destroy(creature_container.transform.GetChild(i).gameObject);
                // if(debug_var){ print("\t Child " + i + " killed"); }
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
    Copy the brain of the surived creatures in all the new spawned creatures as evenly as possible.
    E.g. if 13 creatures survived and 100 new creatures spawn the first 13 new creatures will have the brain of the first creatures survived etc.
    */
    public void repopulate(){
        // Backup array with the brains of the survived creatures
        Brain[] backup_brains = backupSurvived_V1();
        if(debug_var){ print("\tREPOPULATION: N. of backup brain: " + backup_brains.Length);}

        // Spawn new creatures
        GameObject tmp_creatures_container = new GameObject();
        tmp_creatures_container.transform.name = "Temporary Survived Creatures Container";
        GameObject.Find("Script Container").GetComponent<Spawn>().spawnCreature(tot_creatures, tmp_creatures_container);

        // Upload the brain of the survived creatures
        int brain_index = 0, tmp_i = 0;
        for(int i = 0; i < tmp_creatures_container.transform.childCount; i++){
            tmp_creatures_container.transform.GetChild(i).gameObject.GetComponent<Brain>().InitCopyBrain(backup_brains[brain_index]);
            if(tmp_i >= (tot_creatures / backup_brains.Length)){
                tmp_i = 0;
                brain_index++;
            } else { tmp_i++; }
        }

        // Kill the original survived creatures
        killAllCreatures();
        if(debug_var){ print("\tREPOPULATION: TOTAL Creatures:\t" + creature_container.transform.childCount + " (creature_container)");}
        if(debug_var){ print("\tREPOPULATION: TOTAL Creatures:\t" + creature_container.transform.childCount + " (tmp_creatures_container)");}

        // Move new creatures inside the normal container
        // foreach(Transform creature in tmp_creatures_container.transform){creature.transform.parent = GameObject.Find("Creature Container").transform; }
        int tmp_creature_to_move = tmp_creatures_container.transform.childCount;
        for(int i = tmp_creature_to_move - 1; i >= 0 ; i--){
            tmp_creatures_container.transform.GetChild(i).transform.parent = GameObject.Find("Creature Container").transform;
        }


        // // Destroy temporary container
        Destroy(tmp_creatures_container);
    }


    public Brain[] backupSurvived_V1(){
        Brain[] backup_brains = new Brain[creature_survived];
        for(int i = 0; i < creature_survived; i++){
            backup_brains[i] = creature_container.transform.GetChild(i).gameObject.GetComponent<Brain>();
        }

        return backup_brains;
    }

    public Brain[] backupSurvived_V2(){
        Brain tmp_brain;
        List<Brain> backup_brains_list = new List<Brain>();
        List<string> backup_wired_string = new List<string>();

        // Cycled through survived creatures
        for(int i = 0; i < creature_survived; i++){
            // Extract brain of the current creatures
            tmp_brain = creature_container.transform.GetChild(i).gameObject.GetComponent<Brain>();

            // Check if I have already done a backup of an identical brain in another creatures
            print(i + " " + !backup_wired_string.Contains(tmp_brain.brain_wiring) + " " + tmp_brain.brain_wiring);
            if(!backup_wired_string.Contains(tmp_brain.brain_wiring)){
                // If it is unique add the brain to the list
                backup_brains_list.Add(tmp_brain);
                backup_wired_string.Add(tmp_brain.brain_wiring);
            }

        }

        print("creature_survived  =\t" + creature_survived);
        print("backup_brains_list =\t" + backup_brains_list.Count);
        print("backup_wired_string =\t" + backup_wired_string.Count);
        return backup_brains_list.ToArray();
    }


    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
}
