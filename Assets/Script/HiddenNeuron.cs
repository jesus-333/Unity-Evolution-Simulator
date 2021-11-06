using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenNeuron {

    public float state = 0.5f;
    public float[] back_connection_input_weights, back_connection_hidden_weights;
    public bool random_init = false;

    public InputNeuron[] back_connected_input_neurons;
    public HiddenNeuron[] back_connected_hidden_neurons;

    // Initializes the state of the neuron with random value
    public HiddenNeuron() { state = random_init == true ? UnityEngine.Random.Range(-1f, 1f) : 0.5f; }

    // Initializes the state of the neuron with given value
    public HiddenNeuron(float state) { this.state = state; }

    /*
    Randomly assing different weight to each connection
    */
    public void randomInitWeights(){
        for (int i = 0; i < back_connected_input_neurons.Length; i++){

        }

        for (int i = 0; i < back_connected_input_neurons.Length; i++){

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
