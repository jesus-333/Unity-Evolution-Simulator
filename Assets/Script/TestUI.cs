using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public float UI_width, UI_height;
    public int n_neurons;

    public GameObject creature = null, UI_neuron_prefab, UI_object, creature_container;
    public bool show_net = false;

    void Update(){
        if(creature != null){ creature = creature_container.transform.GetChild(0).gameObject; }

        if(show_net && creature != null){
            GameObject tmp_UI_neuron;

            // Retrive UI measures
            getUIMeasure();

            // Get creature brain
            Brain creature_brain = creature.GetComponent<Brain>();

            // Create list of UI neurons
            //GameObject[] neuron_list = new GameObject[creature_brain.n_input_neurons + creature_brain.n_output_neurons + creature_brain.n_hidden_neurons];
            n_neurons = creature_brain.n_input_neurons + creature_brain.n_output_neurons + creature_brain.n_hidden_neurons;

            // Create UI Neurons
            for(int i = 0; i < n_neurons; i++){
                Vector3 random_position = new Vector3(Random.Range(-UI_width/2, UI_width/2), Random.Range(-UI_height/2, UI_height/2), 1);
                tmp_UI_neuron = Instantiate(UI_neuron_prefab, random_position, Quaternion.identity, UI_object.transform);

                // tmp_UI_neuron = Instantiate(UI_neuron_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity, UI_object.transform);
                // tmp_UI_neuron.GetComponent<RectTransform>().localPosition = ;
            }

            show_net = false;
        }
    }

    public void getUIMeasure(){
        UI_width = UI_object.GetComponent<RectTransform>().rect.width;
        UI_height = UI_object.GetComponent<RectTransform>().rect.height;
    }
}
