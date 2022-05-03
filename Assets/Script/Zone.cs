// Default when a new unity script is created
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to save the zone
using System.IO;
using System.Text.RegularExpressions;

public class Zone : MonoBehaviour
{
    [Range(-100, 100)]
    public int field_side = 50;

    public GameObject border_prefab, angle_prefab, grass_field_prefab;

    // N.b. start_position = Upper left angle and end_position = lower right angle
    // The plane is the x-z plane
    public Vector3 start_position, end_position;

    void Start(){
        // Eventualy correct the coordinate
        checkZoneCoordinates();

        // Create terrain
        createGrassField(field_side);

        // Create boundaries
        drawSafeZone(start_position, end_position);
    }

    /*
    This function check the start and end position.
    It change the two vectors in order to have start position in the upper left angle and end position in the lower right angle
    */
    void checkZoneCoordinates(){
        // Check x coordinate
        float lower_x, higher_x;

        if(start_position.x > end_position.x){
            lower_x = end_position.x;
            higher_x = start_position.x;
        } else {
            lower_x = start_position.x;
            higher_x = end_position.x;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Check z coordinate
        float lower_z, higher_z;

        if(start_position.z > end_position.z){
            lower_z = end_position.z;
            higher_z = start_position.z;
        } else {
            lower_z = start_position.z;
            higher_z = end_position.z;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Create start and end position in order to have upper left angle and lower right angle

        start_position = new Vector3(lower_x, 0f, higher_z);
        end_position = new Vector3(higher_x, 0f, lower_z);
    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Methods related to draw stuff

    void createGrassField(int field_side){
        // Create empty GameObject to contain the grass tiles
        GameObject field_container = new GameObject("Grass Field Container");
        field_container.transform.position = new Vector3(0,0,0);

        // Temporary variable for the tiles creation
        GameObject tmp_tiles;

        // Tiles creation cycle
        for(int i = -field_side/2; i <= field_side/2; i = i + 10){
            for(int j = -field_side/2; j <= field_side/2; j = j + 10){
                tmp_tiles = Instantiate(grass_field_prefab, new Vector3(i, 0, j) , Quaternion.identity, field_container.transform);
            }
        }
    }

    void drawSafeZone(Vector3 start_position, Vector3 end_position){
        // Create empty GameObject to contain the border
        GameObject border_container = new GameObject("Border Container");
        border_container.transform.position = new Vector3(0,0,0);

        float x_length, z_length;
        GameObject side_1, side_2, side_3, side_4;
        GameObject angle_1, angle_2, angle_3, angle_4;

        // Evaluate border length based on start and end position
        z_length = Mathf.Abs(start_position.z - end_position.z);
        x_length = Mathf.Abs(start_position.x - end_position.x);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Creation side 1 and 3 (along z axis)
        side_1 = Instantiate(border_prefab, start_position - new Vector3(0, 0, z_length/2), Quaternion.identity, border_container.transform);
        side_1.transform.Rotate(90, 0, 0);
        side_1.transform.localScale = new Vector3(1f, z_length/2, 1f);
        side_1.transform.name = "Side 1";

        side_3 = Instantiate(border_prefab, end_position + new Vector3(0, 0, z_length/2), Quaternion.identity, border_container.transform);
        side_3.transform.Rotate(90, 0, 0);
        side_3.transform.localScale = new Vector3(1f, z_length/2, 1f);
        side_3.transform.name = "Side 3";

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Creation side 2 and 4 (along x axis)
        side_2 = Instantiate(border_prefab, start_position + new Vector3(x_length/2, 0, 0), Quaternion.identity, border_container.transform);
        side_2.transform.Rotate(90, 0, 90);
        side_2.transform.localScale = new Vector3(1f, x_length/2, 1f);
        side_2.transform.name = "Side 2";

        side_4 = Instantiate(border_prefab, end_position - new Vector3(x_length/2, 0, 0), Quaternion.identity, border_container.transform);
        side_4.transform.Rotate(90, 0, 90);
        side_4.transform.localScale = new Vector3(1f, x_length/2, 1f);
        side_4.transform.name = "Side 4";

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Create angles
        angle_1 = Instantiate(angle_prefab, start_position, Quaternion.identity, border_container.transform);
        angle_2 = Instantiate(angle_prefab, end_position, Quaternion.identity, border_container.transform);
        angle_3 = Instantiate(angle_prefab, new Vector3(start_position.x, 0, end_position.z), Quaternion.identity, border_container.transform);
        angle_4 = Instantiate(angle_prefab, new Vector3(end_position.x, 0, start_position.z), Quaternion.identity, border_container.transform);

        angle_1.transform.name = "Angle 1";
        angle_2.transform.name = "Angle 2";
        angle_3.transform.name = "Angle 3";
        angle_4.transform.name = "Angle 4";

    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Methods related to save/load zones position

    /*
    Save the current zone in a text files.
    The method read all the previous saved position and if the current location is not present it is saved.
    Each position is saved in 3 lines: the first for the start position, the second for the end position and the last empty to allow easy reading of the file
    */
    public void saveZone(){
        // Eventualy correct the coordinate
        checkZoneCoordinates();

        // Read previous saved zone
        string previous_saved_zone, start_position_string = "", end_position_string = "";
        using(StreamReader readtext = new StreamReader("Data/zone.txt")){
            previous_saved_zone = readtext.ReadToEnd();
        }

        using(StreamWriter writetext = new StreamWriter("Data/zone.txt")){
            start_position_string = start_position.x + " " + start_position.y + " " + start_position.z;
            end_position_string = end_position.x + " " + end_position.y + " " + end_position.z;

            if(checkIfAlreadySaved(start_position_string, end_position_string, previous_saved_zone)){
                // If present only notify that the current zone is already saved
                print("Position already saved");
            } else {
                // If not present add and save the current zone
                previous_saved_zone = previous_saved_zone + start_position_string + "\n" + end_position_string + "\n";
                print("New zone saved");
            }

            // Clean the string and saved it
            previous_saved_zone = Regex.Replace(previous_saved_zone, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            writetext.WriteLine(previous_saved_zone);
        }
    }

    /*
    Check if the positon defined by the start_position_string and end_position_string is 'present in previous_saved_zone
    */
    private bool checkIfAlreadySaved(string start_position_string, string end_position_string, string previous_saved_zone){
        string[] previous_zone_split_by_line = previous_saved_zone.Split("\n");

        for(int i = 0; i < previous_zone_split_by_line.Length; i = i + 3){ //Each position is saved in 3 line
            if(previous_zone_split_by_line[i].Equals(start_position_string)){ // Check the start position
                if(previous_zone_split_by_line[i + 1].Equals(end_position_string)){ // Check the end position
                    //If both start and end position are presents sequentially this means that I already have saved this position
                    return true;
                }
            }
        }

        // If arrive here it means that has not found the current position
        return false;
    }
}
