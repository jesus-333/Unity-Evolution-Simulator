using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainV2 : MonoBehaviour{
    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Variables

    public Vector3 objective;

    public int n_genes = 10, genes_mode_init = 2;
    public int n_links = 0, n_input_neurons = 0, n_hidden_neurons = 0, n_output_neurons = 0;
    public float speed = 4f;

    public InputNeuron[] input_neurons;
    public float[] input_neurons_state, hidden_neurons_state, output_neurons_state;

    public string genes;

    private float x_limit, z_limit;

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

}
