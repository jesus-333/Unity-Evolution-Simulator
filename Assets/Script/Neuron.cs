using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {

    public float state = 0.5f;
    public float[] back_connection_weights;
    public bool random_init = false;

    public Neuron[] back_connected_neuron, front_connected_neuron;

    public Neuron() {
        // Initializes the state of the neuron
        state = random_init == true ? UnityEngine.Random.Range(-1f, 1f) : 0.5f;
    }

    public void updateState() {
        // Temporary variable to save the new state
        float tmp_new_state = 0f;

        // Evaluate (temporary) new state as the weighted sum of the previous neurons
        for (int i = 0; i < back_connection_weights.Length; i++){
            tmp_new_state += back_connection_weights[i] * back_connected_neuron[i].state;
        }

        // Evaluate final new state as tanh of the weighted sum and update state
        state = (float) Math.Tanh(tmp_new_state);
    }

}

/*
Neurons used to measur the distance from an objective
*/
public class ObjectiveNeuron: Neuron {
    public float my_x, my_z, objective_x, objective_z;
    public GameObject obj = null;

    public ObjectiveNeuron(float objective_x, float objecive_z) {
        this.objective_x = objective_x;
        this.objective_z = objecive_z;
        this.obj = null;
    }

    public ObjectiveNeuron(GameObject obj) {
        this.obj = obj;
    }

    public void updateState(){
        if(obj == null){ // Static objective. Given in input during inizialization and remain fixed (e.g. food).
            state = Mathf.Sqrt(Mathf.Pow((my_x - objective_x), 2) + Mathf.Pow((my_z - objective_z), 2));
        } else { // Dynamic objcetive. Linked with a GameObject that can move (e.g. a prey).
            state = Mathf.Sqrt(Mathf.Pow((my_x - obj.transform.position.x), 2) + Mathf.Pow((my_z - obj.transform.position.z), 2));
        }

    }
}
