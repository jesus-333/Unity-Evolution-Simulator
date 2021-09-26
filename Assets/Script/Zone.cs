using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [Range(-100, 100)]
    public int field_side = 50;

    public GameObject border_prefab, grass_field_prefab;
    public Vector3 start_position, end_position;

    void Start()
    {
        createGrassField(field_side);

        drawSafeZone(start_position, end_position);
    }

    void createGrassField(int field_side){
        // Create empty GameObject to contain the grass tiles
        GameObject field_container = new GameObject("Grass Field Container");
        field_container.transform.position = new Vector3(0,0,0);

        // Temporary variable for the tiles creation
        GameObject tmp_tiles;

        // Tiles creation cycle
        for(int i = -field_side/2; i < field_side/2; i = i + 10){
            for(int j = -field_side/2; j < field_side/2; j = j + 10){
                tmp_tiles = Instantiate(grass_field_prefab, new Vector3(i, 0, j) , Quaternion.identity);
                tmp_tiles.transform.parent = field_container.transform;
            }
        }
    }

    void drawSafeZone(Vector3 start_position, Vector3 end_position){
        float x_length, z_length;
        GameObject side_1, side_2, side_3, side_4;

        // Evaluate border length based on start and end position
        z_length = Mathf.Abs(start_position.z - end_position.z);
        x_length = Mathf.Abs(start_position.x - end_position.x);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Creation side 1 and 3 (along z axis)
        side_1 = Instantiate(border_prefab, start_position - new Vector3(0, 0, z_length/2), Quaternion.identity);
        side_1.transform.Rotate(90, 0, 0);
        side_1.transform.localScale = new Vector3(1f, z_length/2, 1f);
        side_1.transform.name = "Side 1";

        side_3 = Instantiate(border_prefab, end_position + new Vector3(0, 0, z_length/2), Quaternion.identity);
        side_3.transform.Rotate(90, 0, 0);
        side_3.transform.localScale = new Vector3(1f, z_length/2, 1f);
        side_3.transform.name = "Side 3";

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Creation side 2 and 4 (along x axis)
        side_2 = Instantiate(border_prefab, start_position + new Vector3(x_length/2, 0, 0), Quaternion.identity);
        side_2.transform.Rotate(90, 0, 90);
        side_2.transform.localScale = new Vector3(1f, x_length/2, 1f);
        side_2.transform.name = "Side 2";

        side_4 = Instantiate(border_prefab, end_position - new Vector3(x_length/2, 0, 0), Quaternion.identity);
        side_4.transform.Rotate(90, 0, 90);
        side_4.transform.localScale = new Vector3(1f, x_length/2, 1f);
        side_4.transform.name = "Side 4";

    }
}
