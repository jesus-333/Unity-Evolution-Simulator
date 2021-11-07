using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public int n_genes = 8, n_links = 0, n_hidden_neurons = 0, genes_mode_init = 2;
    public float speed = 4f;

    public string genes;

    public InputNeuron[] input_neurons;
    public HiddenNeuron[] hidden_neurons, output_neurons;

    private Vector3 move_vec;
    private CharacterController controller;

    public bool debug_var;


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Unity related methods

    void Start()
    {
        // Inizialization of the genes
        InitGenes(genes_mode_init);

        // Inizialization of input neurons
        InitInputNeurons();

        // Inizialization of output neurons
        output_neurons = InitHiddenNeurons(2, 0.5f);
        /*
        Output Neurons list
        0 ---> up/down
        1 ---> left/right
        */

        // Retrive CharacterController
        controller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Update input neurons state
        foreach(InputNeuron tmp_neuron in input_neurons){ tmp_neuron.updateState(); }
        // input_neurons[0].updateState();

        // Move the creature
        Move();

        if(debug_var){ Debug(); }

    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Init methods

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

        genes = supportMethods.IntToCharLowerCase(n_links) + supportMethods.IntToCharLowerCase(n_hidden_neurons);
    }

    /*
    Initializes the input neurons one by one. This is necessary due to the fact that input neurons can have particular inizialization.
    */
    public void InitInputNeurons(){
        input_neurons = new InputNeuron[3];

        ObjectiveNeuron tmp_neuron = new ObjectiveNeuron(0, 0, this.transform);
        // tmp_neuron.setNormalizeState(true);
        input_neurons[0] = tmp_neuron;

        input_neurons[1] = new XNeuron(this.transform);

        input_neurons[2] = new ZNeuron(this.transform);
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
    Method used to create the connections between the neurons.
    */
    public void InitWiring(){

    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Action methods

    public void Move(){
        // Temporary code to test movement
        // int tmp_threeshold = 20;
        // if(this.transform.position.x > tmp_threeshold || this.transform.position.x < - tmp_threeshold){ output_neurons[0].state *= - 1; }
        // if(this.transform.position.z > tmp_threeshold || this.transform.position.z < - tmp_threeshold){ output_neurons[1].state *= - 1; }

        // Evaluate direction based on output neurons and move in that direction
        move_vec = new Vector3(output_neurons[0].state, 0, output_neurons[1].state);
        controller.Move(move_vec * Time.deltaTime * speed);

        // Turn the creature in the movement direction
        // if (move_vec != Vector3.zero){ this.transform.forward = move_vec; }

    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Other methods

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
