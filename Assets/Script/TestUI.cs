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
        if(creature == null){ creature = creature_container.transform.GetChild(0).gameObject; }

        if(show_net && creature != null){
            show_net = false;

            GameObject tmp_UI_neuron;

            // Retrive UI measures
            getUIMeasure();

            // Get creature brain
            Brain creature_brain = creature.GetComponent<Brain>();

            // Create list of UI neurons
            //GameObject[] neuron_list = new GameObject[creature_brain.n_input_neurons + creature_brain.n_output_neurons + creature_brain.n_hidden_neurons];
            n_neurons = creature_brain.n_input_neurons + creature_brain.n_output_neurons + creature_brain.n_hidden_neurons;


            // Variable used during UI Neurons creation
            float tmp_x = (UI_width - 20f)/2f * 0.6f, tmp_y, border = 80f;
            float[] tmp_vector_height_input = SupportMethods.linspace(-(UI_height - border)/2f, (UI_height - border)/2f, creature_brain.n_input_neurons);
            float[] tmp_vector_height_output = SupportMethods.linspace(0f, (UI_height - border)/2f, creature_brain.n_output_neurons);
            float[] tmp_vector_height_hidden = SupportMethods.linspace(-(UI_height - border)/2f, 0f, creature_brain.n_hidden_neurons);
            Vector3 tmp_position;

            // Create UI Neurons
            for(int i = 0; i < n_neurons; i++){
                print("Show neuron:" + i);
                // Create neurons
                tmp_UI_neuron = Instantiate(UI_neuron_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity, UI_object.transform);

                // Moved and choose color based on type
                if(i <  creature_brain.n_input_neurons){ // Input neurons
                    tmp_position = new Vector3(-tmp_x, tmp_vector_height_input[i], 1);
                } else if(i >=  creature_brain.n_input_neurons && i < creature_brain.n_input_neurons + creature_brain.n_output_neurons){ //Output neurons
                    tmp_position = new Vector3(tmp_x, tmp_vector_height_output[i - creature_brain.n_input_neurons], 1);
                } else { // Hidden neurons
                    tmp_position = new Vector3(0, tmp_vector_height_hidden[i - creature_brain.n_input_neurons - creature_brain.n_output_neurons], 1);
                }

                tmp_UI_neuron.GetComponent<RectTransform>().localPosition = tmp_position;
            }
        }
    }

    public void getUIMeasure(){
        UI_width = UI_object.GetComponent<RectTransform>().rect.width;
        UI_height = UI_object.GetComponent<RectTransform>().rect.height;
    }
}
