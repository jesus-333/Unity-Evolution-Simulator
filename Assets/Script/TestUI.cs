using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;


public class TestUI : MonoBehaviour
{
    public float UI_width, UI_height, line_thickness = 2f;
    public int n_neurons;
    public bool show_net = false;

    public GameObject creature = null, UI_neuron_prefab, line_renderer_prefab;
    public GameObject UI_object, UI_neurons_container, creature_container, line_renderer_container;
    public Color line_color;

    private bool update_neurons_status = false;
    private Brain creature_brain;

    void Update(){
        if(creature == null){
            creature = creature_container.transform.GetChild(0).gameObject;
            creature_brain = creature.GetComponent<Brain>();
        }

        if(show_net && creature != null){
            drawUINeurons();
            drawConnections();
        }

        if(update_neurons_status){
            updateNeuronsStatus();
        }
    }

    public void getUIMeasure(){
        UI_width = UI_object.GetComponent<RectTransform>().rect.width;
        UI_height = UI_object.GetComponent<RectTransform>().rect.height;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Draw related functions

    public void drawUINeurons(){
        show_net = false;
        update_neurons_status = true;

        GameObject tmp_UI_neuron;

        // Retrive UI measures
        getUIMeasure();

        // Create list of UI neurons
        //GameObject[] neuron_list = new GameObject[creature_brain.n_input_neurons + creature_brain.n_output_neurons + creature_brain.n_hidden_neurons];
        n_neurons = creature_brain.n_input_neurons + creature_brain.n_output_neurons + creature_brain.n_hidden_neurons;


        // Variable used during UI Neurons creation
        float tmp_x = (UI_width - 20f)/2f * 0.6f, border = 80f;
        float[] tmp_vector_height_input = SupportMethods.linspace(-(UI_height - border)/2f, (UI_height - border)/2f, creature_brain.n_input_neurons);
        float[] tmp_vector_height_output = SupportMethods.linspace(0f, (UI_height - border)/2f, creature_brain.n_output_neurons);
        float[] tmp_vector_height_hidden = SupportMethods.linspace(-(UI_height - border)/2f, 0f, creature_brain.n_hidden_neurons);
        Vector3 tmp_position;

        // Create UI Neurons
        for(int i = 0; i < n_neurons; i++){
            // Create neurons
            tmp_UI_neuron = Instantiate(UI_neuron_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity, UI_neurons_container.transform);

            // Moved based on type
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

    public void drawConnections(){
        string brain_wiring = creature_brain.brain_wiring;
        print(brain_wiring);

        for(int i = 0; i < brain_wiring.Length; i = i + 2){
            drawSingleConnection(brain_wiring[i] + "" + brain_wiring[i + 1] + "");
        }
    }

    /*
    Use the information inside the connection code (a pair of letter from the brain_wiring string) to draw a connection between two neurons.
    */
    public void drawSingleConnection(string connection_code){
        // print(connection_code);

        // Convert char to int index
        int tmp_index_1, tmp_index_2;
        tmp_index_1 = SupportMethods.CharToIntLowerCase(connection_code[0]);
        tmp_index_2 = SupportMethods.CharToIntLowerCase(connection_code[1]);

        // Get UI Neurons
        Transform neuron_1, neuron_2;
        neuron_1 = UI_neurons_container.transform.GetChild(tmp_index_1);
        neuron_2 = UI_neurons_container.transform.GetChild(tmp_index_2);

        // Position of the two UI neurons
        Vector2 pos_1, pos_2;
        pos_1 = new Vector2(neuron_1.GetComponent<RectTransform>().position.x, neuron_1.GetComponent<RectTransform>().position.y);
        pos_2 = new Vector2(neuron_2.GetComponent<RectTransform>().position.x, neuron_2.GetComponent<RectTransform>().position.y);

        // Creation of object that contain the line line renderer
        GameObject tmp_lr_obj = Instantiate(line_renderer_prefab, new Vector3(0f, 0f, 0f), Quaternion.identity, line_renderer_container.transform);
        tmp_lr_obj.transform.name = "Line " + connection_code;

        // Retrieve line renderer from object and add points
        UILineRenderer lr = tmp_lr_obj.GetComponent<UILineRenderer>();
        List<Vector2> points_list = new List<Vector2>();
        points_list.Add(pos_1);
        points_list.Add(pos_2);

        // Set line thickness and color
        lr.Points = points_list.ToArray();
        lr.LineThickness = line_thickness;
        lr.color = line_color;

        //
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Update related functions

    public void updateNeuronsStatus(){
        Text tmp_text;
        float tmp_state;
        for(int i = 0; i < n_neurons; i++){
            tmp_text = UI_neurons_container.transform.GetChild(i).GetChild(1).GetComponent<Text>();
            if(i <  creature_brain.n_input_neurons){ // Input neurons
                tmp_state = creature_brain.input_neurons[i].state;
            } else if(i >=  creature_brain.n_input_neurons && i < creature_brain.n_input_neurons + creature_brain.n_output_neurons){ //Output neurons
                tmp_state = creature_brain.output_neurons[i - creature_brain.n_input_neurons].state;
            } else { // Hidden neurons
                tmp_state = creature_brain.hidden_neurons[i - creature_brain.n_input_neurons - creature_brain.n_output_neurons].state;
                // print(i + "\t" + tmp_state);
            }

            tmp_text.text = tmp_state.ToString("F2");
        }
    }

}
