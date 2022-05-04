using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenNeuronV2{

    public float state = 0.5f, min_weight = -1f, max_weight = 1f; //P.S. I know... I should have used private variable with getters and setters... but for now it is faster this way
    public float[] back_connection_input_weights, back_connection_hidden_weights;

    public string id = "";

    // public InputNeuron[] back_connected_input_neurons;
    // public HiddenNeuron[] back_connected_hidden_neurons;
    public List<InputNeuron> back_connected_input_neurons;
    public List<HiddenNeuron> back_connected_hidden_neurons;

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Constructor methods

    // Initializes the state of the neuron with given value
    public HiddenNeuronV2(float state) {
        this.state = state;

        this.back_connected_input_neurons = new List<InputNeuron>();
        this.back_connected_hidden_neurons = new List<HiddenNeuron>();

        this.id = SupportMethods.generateRandomString(10);
    }

    // Initializes the state of the neuron with random value
    public HiddenNeuronV2() : this(UnityEngine.Random.Range(-1f, 1f)) {}

    // Initializes the new neuron taking the value of the old neuron
    public HiddenNeuronV2(HiddenNeuron old_neuron){
        // Copy old state
        this.state = old_neuron.state;

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Copy input neuron connected to this hidden neuron
        this.back_connected_input_neurons = new List<InputNeuron>(old_neuron.back_connected_input_neurons);

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Copy other hidden neuron connected to the this hidden neuron
        this.back_connected_hidden_neurons = new List<HiddenNeuron>(old_neuron.back_connected_hidden_neurons);

        // Bugged
        // back_connected_hidden_neurons = new List<HiddenNeuron>(old_neuron.back_connected_hidden_neurons.Count);
        // foreach(HiddenNeuron connected_neuron in old_neuron.back_connected_hidden_neurons){
        //     back_connected_hidden_neurons.Add(new HiddenNeuron(connected_neuron));
        // }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Copy weights
        if(old_neuron.back_connection_input_weights != null){
            this.back_connection_input_weights = new float[back_connected_input_neurons.Count];
            old_neuron.back_connection_input_weights.CopyTo(this.back_connection_input_weights, 0);
        }
        if(old_neuron.back_connection_hidden_weights != null){
            this.back_connection_hidden_weights = new float[back_connected_hidden_neurons.Count];
            old_neuron.back_connection_hidden_weights.CopyTo(this.back_connection_hidden_weights, 0);
         }
    }

    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
    // Setter methods (TODO in future)


    // * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *

    /*
    Randomly assing different weight to each connection
    */
    public void randomInitWeights(){
        // Input/hidden connections
        back_connection_input_weights = new float[back_connected_input_neurons.Count];
        for (int i = 0; i < back_connected_input_neurons.Count; i++){
            back_connection_input_weights[i] = UnityEngine.Random.Range(min_weight, max_weight);
        }
        // Debug.Log(back_connection_input_weights.Length);

        // Hidden/Hidden (or Hidden/output) connections
        back_connection_hidden_weights = new float[back_connected_hidden_neurons.Count];
        for (int i = 0; i < back_connected_hidden_neurons.Count; i++){
            back_connection_hidden_weights[i] = UnityEngine.Random.Range(min_weight, max_weight);
        }
    }

    /*
    Basic update state as the tanh of the weighted sum of the output of the previous neurons
    */
    public void updateState() {
        // Temporary variable to save the new state
        float tmp_new_state = 0f;

        // Evaluate (temporary) new state as the weighted sum of the previous input neurons
        for (int i = 0; i < back_connection_input_weights.Length; i++){
            tmp_new_state += back_connection_input_weights[i] * back_connected_input_neurons[i].state;
        }

        // Evaluate (temporary) new state as the weighted sum of the previous hidden neurons
        for (int i = 0; i < back_connection_hidden_weights.Length; i++){
            tmp_new_state += back_connection_hidden_weights[i] * back_connected_hidden_neurons[i].state;
        }

        // Evaluate final new state as tanh of the weighted sum and update state
        state = (float) Math.Tanh(tmp_new_state);
    }

}
