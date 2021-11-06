using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public int n_genes = 8;

    public InputNeuron[] input_neurons;
    public HiddenNeuron[] hidden_neurons, output_neurons;

    public bool debug_var;


    void Start()
    {
        // Inizialization of input neurons
        InitInputNeurons();

        // Inizialization of output neurons
        output_neurons = InitHiddenNeurons(2, 0f);
        /*
        Output Neurons list
        0 ---> up/down
        1 ---> left/right
        */
    }

    void Update()
    {
        input_neurons[0].updateState();
        // Debug();
    }

    /*
    Initializes the input neurons one by one. This is necessary due to the fact that input neurons can have particular inizialization.
    */
    public void InitInputNeurons(){
        input_neurons = new InputNeuron[2];

        ObjectiveNeuron tmp_neuron = new ObjectiveNeuron(0, 0, this.transform);
        tmp_neuron.setNormalizeState(true);
        input_neurons[0] = tmp_neuron;
    }

    /*
    Method used to generate an array of HiddenNeuron. Used for both hidden neurons and output neurons since the their update logic is the same.
    */
    public HiddenNeuron[] InitHiddenNeurons(int n_neurons, float state){
        HiddenNeuron[] tmp_neurons_array = new HiddenNeuron[n_neurons];

        for (int i = 0; i < n_neurons; i++){
            if(state > 1f || state < - 1f){ // Init with random state
                tmp_neurons_array[0] = new HiddenNeuron();
            } else { // Init with a specific state
                tmp_neurons_array[0] = new HiddenNeuron(state);
            }
        }

        return tmp_neurons_array;
    }


    /*
    Method used to print information and perform debugging
    */
    public void Debug(){
        // print("Objective Neuron state:\t" + input_neurons[0].state);
    }
}
