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
        input_neurons = new InputNeuron[2];

        ObjectiveNeuron tmp_neuron = new ObjectiveNeuron(0, 0, this.transform);
        input_neurons[0] = tmp_neuron;
    }

    void Update()
    {
        input_neurons[0].updateState();
        // Debug();
    }

    /*
    Method used to print information and perform debugging
    */
    public void Debug(){
        // print("Objective Neuron state:\t" + input_neurons[0].state);
    }
}
