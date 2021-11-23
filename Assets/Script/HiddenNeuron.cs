using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenNeuron {

    public float state = 0.5f, min_weight = -1f, max_weight = 1f; //P.S. I know... I should have used private variable with getters and setters... but for now it is faster this way
    public float[] back_connection_input_weights, back_connection_hidden_weights;

    // public InputNeuron[] back_connected_input_neurons;
    // public HiddenNeuron[] back_connected_hidden_neurons;
    public List<InputNeuron> back_connected_input_neurons;
    public List<HiddenNeuron> back_connected_hidden_neurons;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Constructor methods

    // Initializes the state of the neuron with given value
    public HiddenNeuron(float state) {
        this.state = state;

        back_connected_input_neurons = new List<InputNeuron>();
        back_connected_hidden_neurons = new List<HiddenNeuron>();
    }

    // Initializes the state of the neuron with random value
    public HiddenNeuron() : this(UnityEngine.Random.Range(-1f, 1f)) {}

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Setter methods (TODO in future)


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

    /*
    Randomly assing different weight to each connection
    */
    public void randomInitWeights(){
        // Input/hidden connections
        back_connection_input_weights = new float[back_connected_input_neurons.Count];
        for (int i = 0; i < back_connected_input_neurons.Count; i++){
            back_connection_input_weights[i] = UnityEngine.Random.Range(min_weight, max_weight);
        }

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
