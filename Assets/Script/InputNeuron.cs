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

    }

    /*
    Neurons used to measure the distance from an objective
    */
    public class ObjectiveNeuron: InputNeuron {
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

        public override void updateState(){
            if(obj == null){ // Static objective. Given in input during inizialization and remain fixed (e.g. food).
                state = Mathf.Sqrt(Mathf.Pow((my_x - objective_x), 2) + Mathf.Pow((my_z - objective_z), 2));
            } else { // Dynamic objcetive. Linked with a GameObject that can move (e.g. a prey).
                state = Mathf.Sqrt(Mathf.Pow((my_x - obj.transform.position.x), 2) + Mathf.Pow((my_z - obj.transform.position.z), 2));
            }

        }
    }
