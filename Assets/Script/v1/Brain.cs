using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public int n_genes = 8, genes_mode_init = 2;
    public int n_links = 0, n_hidden_neurons = 0, n_input_neurons = 0, n_output_neurons = 0;
    public float speed = 4f;

    public string genes, brain_wiring;

    public InputNeuron[] input_neurons;
    public HiddenNeuron[] hidden_neurons, output_neurons;


    private float x_limit, z_limit;
    private bool init_executed = false;

    private Vector3 move_vec, objective;
    private CharacterController controller;

    public bool debug_var;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Unity related methods

    void Update()
    {

        if(init_executed){
            // Update input neurons state
            foreach(InputNeuron tmp_neuron in input_neurons){ tmp_neuron.updateState(); }

            // Update hidden neurons state
            foreach(HiddenNeuron tmp_neuron in hidden_neurons){ tmp_neuron.updateState(); }

            // Update output neurons state
            foreach(HiddenNeuron tmp_neuron in output_neurons){ tmp_neuron.updateState(); }

            // Move the creature
            Move();
        }


        if(debug_var){ Debug(); }

    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Init methods during first generation

    public void firstGenerationInit(Vector3 objective){
        // Inizialization of the genes
        InitGenes(genes_mode_init);

        // Inizialization of input neurons
        /*
        Input Neurons list
        0 ---> objcetive
        1 ---> x position
        2 ---> z position
        */
        InitInputNeurons(objective.x, objective.z);
        n_input_neurons = input_neurons.Length;
        this.objective = objective;

        // Inizialization of output neurons
        /*
        Output Neurons list
        0 ---> x-axis movement
        1 ---> z-axis movements
        */
        output_neurons = InitHiddenNeurons(2, 0f);
        n_output_neurons = output_neurons.Length;

        // Inizialization of hidden neurons
        hidden_neurons = InitHiddenNeurons(n_hidden_neurons);

        // Create initial random wiring
        InitWiring();

        // Retrive CharacterController
        controller = this.GetComponent<CharacterController>();

        // Find coordinate limits
        x_limit = GameObject.Find("Script Container").GetComponent<Spawn>().x_limit;
        z_limit = GameObject.Find("Script Container").GetComponent<Spawn>().z_limit;

        // Colorate the creature
        setCreatureColor();

        init_executed = true;
    }

    /*
    Divide the number of genes between links and hidden neurons with the constraint that n_links + n_hidden_neurons = n_genes.
    */
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

        ObjectiveNeuron tmp_neuron = new ObjectiveNeuron(objective_x, objective_z, this.transform);
        // tmp_neuron.setNormalizeState(true);
        input_neurons[0] = tmp_neuron;

        input_neurons[1] = new XNeuron(this.transform);

        input_neurons[2] = new ZNeuron(this.transform);
    }

    public void InitInputNeurons(){
        InitInputNeurons(0f, 0f);
    }

    /*
    Method used to generate an array of HiddenNeuron with a given state. Used for both hidden neurons and output neurons since the their update logic is the same.
    */
    public HiddenNeuron[] InitHiddenNeurons(int n_neurons, float state){
        HiddenNeuron[] tmp_neurons_array = new HiddenNeuron[n_neurons];

        for (int i = 0; i < n_neurons; i++){
            if(state > 1f || state < - 1f){ // Init with random state
                tmp_neurons_array[i] = new HiddenNeuron();
            } else { // Init with a specific state
                tmp_neurons_array[i] = new HiddenNeuron(state);
            }
        }

        return tmp_neurons_array;
    }

    /*
    Method used to generate an array of HiddenNeuron with random state. Used for both hidden neurons and output neurons since the their update logic is the same.
    */
    public HiddenNeuron[] InitHiddenNeurons(int n_neurons){
        return InitHiddenNeurons(n_neurons, -2);
    }

    /*
    Method used to create the connections between the neurons randomly.
    The wiring is then codify into a string called brain_wiring (pubblic attribute of the class).
    The string is composed by pair of letters. For each pair the first letter is the starting neuron of the connections and the second the end neuron of the connection.
    */
    public void InitWiring(){
        brain_wiring = "";

        // Temporary variable to select neurons and link type.
        float select_link_type = 0f;
        int tmp_index_1, tmp_index_2; // Index 1 is for the start neuron and index 2 for the end neuron
        string tmp_wiring_string = "";

        for(int i = 0; i < n_links; i++){
            // Randomly select the type of the link
            select_link_type = Random.Range(0f, 1f);

            if(select_link_type >= 0f && select_link_type <= 0.25f){ // Link between input and hidden
                tmp_index_1 = Random.Range(0, n_hidden_neurons);
                tmp_index_2 = Random.Range(0, n_input_neurons);
                hidden_neurons[tmp_index_1].back_connected_input_neurons.Add(input_neurons[tmp_index_2]);
                tmp_wiring_string = SupportMethods.IntToCharLowerCase(tmp_index_2) + SupportMethods.IntToCharLowerCase(tmp_index_1 + n_input_neurons + n_output_neurons);

            } else if (select_link_type >= 0.25f && select_link_type <= 0.5f){ // Link between hidden and output
                tmp_index_1 = Random.Range(0, n_output_neurons);
                tmp_index_2 = Random.Range(0, n_hidden_neurons);
                output_neurons[tmp_index_1].back_connected_hidden_neurons.Add(hidden_neurons[tmp_index_2]);
                tmp_wiring_string = SupportMethods.IntToCharLowerCase(tmp_index_2 + n_input_neurons + n_output_neurons) + SupportMethods.IntToCharLowerCase(tmp_index_1 + n_input_neurons);

            } else if (select_link_type >= 0.5f && select_link_type <= 0.75f){ // Link between hidden and hidden
                tmp_index_1 = Random.Range(0, n_hidden_neurons);
                tmp_index_2 = Random.Range(0, n_hidden_neurons);
                hidden_neurons[tmp_index_1].back_connected_hidden_neurons.Add(hidden_neurons[tmp_index_2]);
                tmp_wiring_string = SupportMethods.IntToCharLowerCase(tmp_index_2 + n_input_neurons + n_output_neurons) + SupportMethods.IntToCharLowerCase(tmp_index_1 + n_input_neurons + n_output_neurons);

            } else { // Link between input and output
                tmp_index_1 = Random.Range(0, n_output_neurons);
                tmp_index_2 = Random.Range(0, n_input_neurons);
                output_neurons[tmp_index_1].back_connected_input_neurons.Add(input_neurons[tmp_index_2]);
                tmp_wiring_string = SupportMethods.IntToCharLowerCase(tmp_index_2) + SupportMethods.IntToCharLowerCase(tmp_index_1 + n_input_neurons);
            }

            // Update wiring string
            brain_wiring = brain_wiring + tmp_wiring_string;
        }

        // Assign random weight to each connection
        foreach(HiddenNeuron tmp_neuron in hidden_neurons){ tmp_neuron.randomInitWeights(); }
        foreach(HiddenNeuron tmp_neuron in output_neurons){ tmp_neuron.randomInitWeights(); }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // "Init" methods (and Init related methods) for future generations

    /*
    Recreate che connections in a brain based on a given input.
    */
    public void CopyWiring(string base_brain_wiring){
        brain_wiring = base_brain_wiring;
        int tmp_index_1, tmp_index_2, link_type; // Index 1 is for the start neuron and index 2 for the end neuron

        for(int i = 0; i < base_brain_wiring.Length; i = i + 2){
            // Convert letter to index number (0 to 25)
            tmp_index_1 = SupportMethods.CharToIntLowerCase(base_brain_wiring[i]);
            tmp_index_2 = SupportMethods.CharToIntLowerCase(base_brain_wiring[i + 1]);

            // Find the type of connections
            link_type = FindLinkTypeForCopyWiring(tmp_index_1, tmp_index_2);

            switch(link_type){
                case 1: // Input to hidden
                    // Shift index to the corresponding value for tha various array
                    tmp_index_1 = ReshiftIndexForCopyWiring(tmp_index_1);
                    tmp_index_2 = ReshiftIndexForCopyWiring(tmp_index_2);
                    hidden_neurons[tmp_index_1].back_connected_input_neurons.Add(input_neurons[tmp_index_2]);
                break;

                case 2: // Hidden to output
                    // Shift index to the corresponding value for tha various array
                    tmp_index_1 = ReshiftIndexForCopyWiring(tmp_index_1);
                    tmp_index_2 = ReshiftIndexForCopyWiring(tmp_index_2);
                    output_neurons[tmp_index_1].back_connected_hidden_neurons.Add(hidden_neurons[tmp_index_2]);
                break;

                case 3: // Hidden to hidden
                    // Shift index to the corresponding value for tha various array
                    tmp_index_1 = ReshiftIndexForCopyWiring(tmp_index_1);
                    tmp_index_2 = ReshiftIndexForCopyWiring(tmp_index_2);
                    hidden_neurons[tmp_index_1].back_connected_hidden_neurons.Add(hidden_neurons[tmp_index_2]);
                break;

                case 4: // Input to output
                    // Shift index to the corresponding value for tha various array
                    tmp_index_1 = ReshiftIndexForCopyWiring(tmp_index_1);
                    tmp_index_2 = ReshiftIndexForCopyWiring(tmp_index_2);
                    output_neurons[tmp_index_1].back_connected_input_neurons.Add(input_neurons[tmp_index_2]);
                break;
            }
        }
    }

    /*
    Used during the InitWiring that recreate the connections based on a string.
    Find the type of link based on the value of the two index.
    1 ---> Input to hidden
    2 ---> Hidden to output
    3 ---> Hidden to hidden
    4 ---> Input to output
    */
    private int FindLinkTypeForCopyWiring(int tmp_index_1, int tmp_index_2){
        if(tmp_index_1 < n_input_neurons){ // The link start from an input neurons
            if(tmp_index_2 > n_input_neurons && tmp_index_2 < n_input_neurons + n_output_neurons){ return 1; } // Input to hidden
            else if(tmp_index_2 >= n_input_neurons + n_output_neurons){ return 4; } // Input to output
        } else if(tmp_index_1 >= n_input_neurons + n_output_neurons){ // The link start from a hidden neurons
            if(tmp_index_2 > n_input_neurons && tmp_index_2 < n_input_neurons + n_output_neurons){ return 2; } // Hidden to hidden
            else if(tmp_index_2 >= n_input_neurons + n_output_neurons){ return 3; } // Hidden to output
        }

        return -1; // ERROR
    }

    /*
    Used during the InitWiring that recreate the connections based on a string.
    Shift the index from 0 to 25 into the value for the various array of neurons.
    */
    private int ReshiftIndexForCopyWiring(int tmp_index){
        if(tmp_index < n_input_neurons){ //Input neurons
            return tmp_index;
        } else if(tmp_index >= n_input_neurons && tmp_index < n_input_neurons + n_output_neurons){ // Output neurons
            return tmp_index - n_input_neurons;
        } else if(tmp_index >=n_input_neurons + n_output_neurons){ // Hidden neurons
            return tmp_index - n_input_neurons - n_output_neurons;
        }

        return -1; // ERROR
    }

    /*
    Copy the information inside the old_brain
    */
    public void InitCopyBrain(Brain old_brain){
        // Copy primitive attributes
        n_genes = old_brain.n_genes;
        genes_mode_init = old_brain.genes_mode_init;
        n_links = old_brain.n_links;
        n_hidden_neurons = old_brain.n_hidden_neurons;
        n_input_neurons = old_brain.n_input_neurons;
        n_output_neurons = old_brain.n_output_neurons;
        speed = old_brain.speed;
        genes = old_brain.genes;
        brain_wiring = old_brain.brain_wiring;

        // Copy Input Neurons
        InitInputNeurons(old_brain.objective.x, old_brain.objective.z);
        this.objective = old_brain.objective;
        // print(this.objective);

        // Copy hidden neurons
        hidden_neurons = new HiddenNeuron[old_brain.hidden_neurons.Length];
        for(int i = 0; i < hidden_neurons.Length; i++){ hidden_neurons[i] = new HiddenNeuron(old_brain.hidden_neurons[i]);}

        // Copy output neurons
        output_neurons = new HiddenNeuron[old_brain.output_neurons.Length];
        for(int i = 0; i < output_neurons.Length; i++){ output_neurons[i] = new HiddenNeuron(old_brain.output_neurons[i]);}

        // Retrive CharacterController
        controller = this.GetComponent<CharacterController>();

        // Find coordinate limits
        x_limit = GameObject.Find("Script Container").GetComponent<Spawn>().x_limit;
        z_limit = GameObject.Find("Script Container").GetComponent<Spawn>().z_limit;

        // Colorate the creature
        setCreatureColor();

        init_executed = true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Action methods

    public void Move(){
        if(checkPosition()){
            // Evaluate direction based on output neurons and move in that direction
            move_vec = new Vector3(output_neurons[0].state, 0, output_neurons[1].state);
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

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Other methods

    public void setCreatureColor(){
        Color tmp_color;
        tmp_color = generateColor(brain_wiring);

        // Body
        this.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", tmp_color);

        // Head
        this.transform.GetChild(1).GetComponent<Renderer>().material.SetColor("_Color", tmp_color);
    }

    /*
    Convert the input string in a color
    */
    public Color generateColor(string input){
        Color tmp_color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        int step = input.Length/3;
        float r,g,b, total;

        r = SupportMethods.addStringLetter(input.Substring(0, step));
        g = SupportMethods.addStringLetter(input.Substring(step, 2 * step));
        b = SupportMethods.addStringLetter(input.Substring(2 * step));

        total = r + g + b;
        tmp_color = new Color(r/total, g/total, b/total);

        return tmp_color;
    }

    /*
    Method used to print information and perform debugging.
    */
    public void Debug(){
        // Print of movement neurons state
        // for(int i = 0; i < 2; i++){print(i + " - move neuron - " + output_neurons[i].state);}

        // Print of input neurons state
        // print("ObjectiveNeuron:\t\t" + input_neurons[0].state);
        // print("XNeuron:\t\t" + input_neurons[1].state);
        // print("ZNeuron:\t\t" + input_neurons[2].state);
        // print("Position:\t\t" + this.transform.position);
    }
}
