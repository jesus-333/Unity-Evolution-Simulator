using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class BrainV2 : MonoBehaviour{
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Variables

    public int n_genes = 10, genes_mode_init = 2;
    public int n_links = 0, n_input_neurons = 0, n_hidden_neurons = 0, n_output_neurons = 0;
    public float speed = 4f;

    public InputNeuron[] input_neurons;
    public float[] input_neurons_state, hidden_neurons_state, output_neurons_state, connections_weights;

    public string genes, brain_wiring;

    private float x_limit, z_limit;

    private Vector3 move_vec, objective;
    private CharacterController controller;
    private string[] hidden_neurons_connections, output_neurons_connections;
    private float[,] input_to_hidden_weights, hidden_to_hidden_weights, hidden_to_output_weights, input_to_output_weights;

    public bool debug_var;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Unity Methods

    void Start(){

    }

    void Update(){

    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Init methods

    public void FirstGenerationInit(Vector3 objective){
        // Inizialization of the genes
        InitGenes(genes_mode_init);

        // Inizialization of input neurons
        // Input Neurons list: 0 ---> objcetive, 1 ---> x position, 2 ---> z position
        InitInputNeurons(objective.x, objective.z);
        n_input_neurons = input_neurons.Length;
        this.objective = objective;

        // Inizialization of output neurons
        // Output Neurons list: 0 ---> x-axis movement, 1 ---> z-axis movements
        float[] output_neurons_state = {0f, 0f};

        // Inizialization of hidden neurons
        InitHiddenNeurons();

        // Create initial wiring and weights
        InitWiring();
        InitWeights();

        // Retrive CharacterController
        controller = this.GetComponent<CharacterController>();
    }


    // Based on the variable mode the function divided the number of genes in links between neurons and number of hidden neurons
    public void InitGenes(int mode){
        switch(mode){
            // Even split (more or less) between hidden neurons and links.
            case 0:
                n_links = n_genes / 2;
                n_hidden_neurons = n_genes - n_links; // To account odd number of genes.
            break;

            // Randomly split between hidden neurons and links. 0 hidden_neurons or 0 links are possible.
            case 1:
                n_links = Random.Range(0, n_genes + 1);
                n_hidden_neurons = n_genes - n_links;
            break;

            // Randomly split between hidden neurons and links. 0 hidden_neurons or 0 links are NOT possible.
            case 2:
                n_links = Random.Range(1, n_genes);
                n_hidden_neurons = n_genes - n_links;
            break;

            // Defualt case if the wrong mode is used.
            default:
                if(debug_var){ print("Wrong number for mode selection. Use case 0 as defualt"); }
                n_links = n_genes / 2;
                n_hidden_neurons = n_genes - n_links; // To account odd number of genes.
            break;
        }

        genes = SupportMethods.IntToCharLowerCase(n_links) + SupportMethods.IntToCharLowerCase(n_hidden_neurons);
    }

    /*
    Initializes the input neurons one by one. This is necessary due to the fact that input neurons can have particular inizialization.
    */
    public void InitInputNeurons(float objective_x, float objective_z){
        input_neurons = new InputNeuron[3];
        input_neurons_state = new float[3];

        ObjectiveNeuron tmp_neuron = new ObjectiveNeuron(objective_x, objective_z, this.transform);
        // tmp_neuron.setNormalizeState(true);
        input_neurons[0] = tmp_neuron;
        input_neurons_state[0] = input_neurons[0].state;

        input_neurons[1] = new XNeuron(this.transform);
        input_neurons_state[1] = input_neurons[1].state;

        input_neurons[2] = new ZNeuron(this.transform);
        input_neurons_state[2] = input_neurons[2].state;
    }

    public void InitHiddenNeurons(){
        hidden_neurons_state = new float[n_hidden_neurons];
        for(int i = 0; i < n_hidden_neurons; i++){
            hidden_neurons_state[i] = UnityEngine.Random.Range(-1f, 1f);
        }
    }

    /*
    Method used to create the connections between the neurons randomly.
    The wiring is codify into a string called brain_wiring (pubblic attribute of the class).
    The string is composed by n groups of 3 char (with n = n_links). Each gropu is composed in the following way: xyz
        x is the type of connections (input->hidden, input->output, hidden->output, hidden->hidden).
        y and z are the index of the neurons connected. The connection is from y to z. Each index is codify by a lower case letter. So for now I can have at max 26 neurons per type.
    N.b. This string contain the complete connections map but it is not used for computation.
    For computation exist 2 vectors (hidden_neurons_connections, output_neurons_connections). See updateState function for more info.
    */
    public void InitWiring(){
        hidden_neurons_connections = Enumerable.Repeat("", 26).ToArray();
        output_neurons_connections = Enumerable.Repeat("", 3).ToArray();

        // Temporary variable to select neurons and link type.
        float select_link_type = 0f;
        int tmp_index_1, tmp_index_2; // Index 1 is for the start neuron and index 2 for the end neuron
        string tmp_wiring_string = "";

        for(int i = 0; i < n_links; i++){
            // Randomly select the type of the link
            select_link_type = Random.Range(0, 4);

            if(select_link_type == 0){ // Link between input and hidden
                tmp_index_1 = Random.Range(0, n_input_neurons);
                tmp_index_2 = Random.Range(0, n_hidden_neurons);
            } else if (select_link_type == 1){ // Link between hidden and output
                tmp_index_1 = Random.Range(0, n_hidden_neurons);
                tmp_index_2 = Random.Range(0, n_output_neurons);
            } else if (select_link_type == 2){ // Link between hidden and hidden
                tmp_index_1 = Random.Range(0, n_hidden_neurons);
                tmp_index_2 = Random.Range(0, n_hidden_neurons);
            } else if (select_link_type == 3){ // Link between input and output
                tmp_index_1 = Random.Range(0, n_input_neurons);
                tmp_index_2 = Random.Range(0, n_output_neurons);
            } else {
                tmp_index_1 = tmp_index_2 = -1;
                print("ERROR");
            }

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Update the complete brain wiring string
            tmp_wiring_string = select_link_type + SupportMethods.IntToCharLowerCase(tmp_index_1) + SupportMethods.IntToCharLowerCase(tmp_index_2);
            brain_wiring = brain_wiring + tmp_wiring_string;

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Save connections in the connections vector

            // Connection towards hidden neurons
            if(select_link_type == 0 || select_link_type == 2){ output_neurons_connections[tmp_index_2] += tmp_wiring_string; }

            // Connection towards output neurons
            if(select_link_type == 1 || select_link_type == 3){ hidden_neurons_connections[tmp_index_2] += tmp_wiring_string; }
        }
    }

    public void InitWeights(float min_value = -1f, float max_value = 1f){
        // connections_weights = new float[n_links];
        // for(int i = 0; i < n_links; i++){connections_weights[i] = UnityEngine.Random.Range(min_value, max_value); }

        input_to_hidden_weights = SupportMethods.fill2dArray(3, 26);
        hidden_to_hidden_weights = SupportMethods.fill2dArray(26, 26);
        hidden_to_output_weights = SupportMethods.fill2dArray(26, 2);
        input_to_output_weights = SupportMethods.fill2dArray(3, 2);
    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Update/Action methods

    /*
    Update the status of various neurons. First update the hidden neurons and then the output neurons.
    The connections are saved in the vector hidden_neurons_connections and output_neurons_connections. Each connections is saved with a string of length 3 (as in the brain wiring)
    Two cycle are used to iterate through the connections.
    */
    public void updateState(){
        string tmp_link_list, tmp_link = "";
        int link_type, tmp_index_1, tmp_index_2;
        float  tmp_state = 0f;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Connection towards hidden neurons

        for(int i = 0; i < hidden_neurons_connections.Length; i++){
            // Retrieve the connection for the i-th hidden neuron
            tmp_link_list = hidden_neurons_connections[i];

            // Check if there are connections for that particular neuron
            if(tmp_link_list.Length > 0){
                tmp_state = 0f;

                // Iterate through connections. N.b. The connections are saved in groups of 3 char.
                // So if A string codify all the connections of a neuron for that neuron I will have string.Length/3 connection
                for(int j = 0; j < tmp_link_list.Length / 3; j++){
                    tmp_link = tmp_link_list.Substring(j * 3, 3);

                    link_type = (int)tmp_link[0];
                    tmp_index_1 = SupportMethods.CharToIntLowerCase((char)tmp_link[1]);
                    tmp_index_2 = SupportMethods.CharToIntLowerCase((char)tmp_link[2]);

                    if(link_type == 0){ // Input to hidden
                        tmp_state += input_to_hidden_weights[tmp_index_1, tmp_index_2] * input_neurons_state[tmp_index_1];
                    } else if(link_type == 2){ // Hidden to hidden
                        tmp_state += hidden_to_hidden_weights[tmp_index_1, tmp_index_2] * hidden_neurons_state[tmp_index_1];
                    }
                }

                // Add non linearity and update state
                hidden_neurons_state[i] = (float) Math.Tanh(tmp_state);
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Connection towards output neurons

        for(int i = 0; i < output_neurons_connections.Length; i++){
            // Retrieve the connection for the i-th hidden neuron
            tmp_link_list = output_neurons_connections[i];

            // Check if there are connections for that particular neuron
            if(tmp_link_list.Length > 0){
                tmp_state = 0f;

                // Iterate through connections. N.b. The connections are saved in groups of 3 char.
                // So if A string codify all the connections of a neuron for that neuron I will have string.Length/3 connection
                for(int j = 0; j < tmp_link_list.Length / 3; j++){
                    tmp_link = tmp_link_list.Substring(j * 3, 3);

                    link_type = (int)tmp_link[0];
                    tmp_index_1 = SupportMethods.CharToIntLowerCase((char)tmp_link[1]);
                    tmp_index_2 = SupportMethods.CharToIntLowerCase((char)tmp_link[2]);

                    if(link_type == 1){ // Hidden to output
                        tmp_state += hidden_to_output_weights[tmp_index_1, tmp_index_2] * hidden_neurons_state[tmp_index_1];
                    } else if(link_type == 3){ // Input to output
                        tmp_state += input_to_output_weights[tmp_index_1, tmp_index_2] * input_neurons_state[tmp_index_1];
                    }
                }

                // Add non linearity and update state
                output_neurons_state[i] = (float) Math.Tanh(tmp_state);
            }
        }

    }

    public void Move(){
        if(checkPosition()){
            // Evaluate direction based on output neurons and move in that direction
            move_vec = new Vector3(output_neurons_state[0], 0, output_neurons_state[1]);
            controller.Move(move_vec * Time.deltaTime * speed);

            // Turn the creature in the movement direction
            // if (move_vec != Vector3.zero){
            //     this.transform.forward = move_vec;
            //     this.transform.Rotate(0, -90, 0);
            // }
        }
    }

    /*
    Check if the creature is inside the limits. If inside return true otherwise false.
    */
    public bool checkPosition(){
        if(Mathf.Abs(this.transform.position.x) >= x_limit){ return false; }
        if(Mathf.Abs(this.transform.position.z) >= z_limit){ return false; }

        return true;
    }


}
