using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public int n_genes = 8;
    public float speed = 1f;

    public InputNeuron[] input_neurons;
    public HiddenNeuron[] hidden_neurons, output_neurons;

    private Vector3 move_vec;
    private CharacterController controller;

    public bool debug_var;


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Unity related methods

    void Start()
    {
        // Inizialization of input neurons
        InitInputNeurons();

        // Inizialization of output neurons
        output_neurons = InitHiddenNeurons(2, 0.5f);
        /*
        Output Neurons list
        0 ---> up/down
        1 ---> left/right
        */

        // Retrive CharacterController
        controller = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        // Update input neurons state
        input_neurons[0].updateState();

        // Move the creature
        Move();

        // Debug();
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Init methods

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
    Method used to generate an array of HiddenNeuron with a given state. Used for both hidden neurons and output neurons since the their update logic is the same.
    */
    public HiddenNeuron[] InitHiddenNeurons(int n_neurons, float state){
        HiddenNeuron[] tmp_neurons_array = new HiddenNeuron[n_neurons];

        for (int i = 0; i < n_neurons; i++){
            if(state > 1f || state < - 1f){ // Init with random state
                tmp_neurons_array[i] = new HiddenNeuron();
            } else { // Init with a specific state
                tmp_neurons_array[i] = new HiddenNeuron(state);
            }
        }

        return tmp_neurons_array;
    }

    /*
    Method used to generate an array of HiddenNeuron with random state. Used for both hidden neurons and output neurons since the their update logic is the same.
    */
    public HiddenNeuron[] InitHiddenNeurons(int n_neurons){
        return InitHiddenNeurons(n_neurons, -2);
    }


    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Action methods

    public void Move(){
        int tmp_threeshold = 20;
        if(this.transform.position.x > tmp_threeshold || this.transform.position.x < - tmp_threeshold){ output_neurons[0].state *= - 1; }
        if(this.transform.position.z > tmp_threeshold || this.transform.position.z < - tmp_threeshold){ output_neurons[1].state *= - 1; }

        move_vec = new Vector3(output_neurons[0].state, 0, output_neurons[1].state);
        controller.Move(move_vec * Time.deltaTime * speed);
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Other methods

    /*
    Method used to print information and perform debugging
    */
    public void Debug(){
        for(int i = 0; i < 2; i++){print(i + " " + output_neurons[i].state);}
    }
}
