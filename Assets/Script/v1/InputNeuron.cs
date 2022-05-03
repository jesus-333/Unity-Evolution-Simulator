using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Parent class for every input neurons

public abstract class InputNeuron {

    public float state = 0.5f;
    public bool random_init = false;

    // public Neuron[] front_connected_neuron;

    public InputNeuron() {
        // Initializes the state of the neuron
        state = random_init == true ? UnityEngine.Random.Range(-1f, 1f) : 0.5f;
    }

    public abstract void updateState();

    public static float Sigmoid(float value) {
        float k = Mathf.Exp(value);
        return k / (1.0f + k);
    }

}

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Input neurons based on coordinate

/*
Neuron used to measure the distance from an objective
*/
public class ObjectiveNeuron: InputNeuron {
    public float objective_x, objective_z;
    public GameObject objective = null;
    public Transform my_position;

    public bool normalize_state = false;

    public ObjectiveNeuron(float objective_x, float objecive_z, Transform my_position) {
        // Set objective
        this.objective_x = objective_x;
        this.objective_z = objecive_z;
        this.objective = null;

        this.my_position = my_position;
    }

    public ObjectiveNeuron(GameObject objective, Transform my_position) {
        this.objective = objective;

        this.my_position = my_position;
    }

    public void setNormalizeState(bool normalize_state){ this.normalize_state = normalize_state; }

    public override void updateState() {
        if(objective == null){ // Static objective. Given in input during inizialization and remain fixed (e.g. food).
            state = Mathf.Sqrt(Mathf.Pow((my_position.position.x - objective_x), 2) + Mathf.Pow((my_position.position.z - objective_z), 2));
        } else { // Dynamic objcetive. Linked with a GameObject that can move (e.g. a prey).
            state = Mathf.Sqrt(Mathf.Pow((my_position.position.x - objective.transform.position.x), 2) + Mathf.Pow((my_position.position.z - objective.transform.position.z), 2));
        }

        // scale state between 0 and 1
        if(normalize_state){ state = Sigmoid(state); }
    }

}

/*
Neuron where the state is equal to the X coordinate
*/
public class XNeuron: InputNeuron{
    public Transform my_position;

    public XNeuron(Transform my_position) {
        this.my_position = my_position;
    }

    public override void updateState(){
        state = my_position.position.x;
    }
}

/*
Neuron where the state is equal to the Z coordinate
*/
public class ZNeuron: InputNeuron{
    public Transform my_position;

    public ZNeuron(Transform my_position) {
        this.my_position = my_position;
    }

    public override void updateState(){
        state = my_position.position.z;
    }
}
