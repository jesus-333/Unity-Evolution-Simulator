using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

/*
Neurons used to measure the distance from an objective
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
