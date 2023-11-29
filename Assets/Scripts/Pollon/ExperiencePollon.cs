using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePollon : Pollon
{
    private int experience_amount;
    private Flower destination_flower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(int experience_amount, Flower destination_flower){
        this.experience_amount = experience_amount;
        this.destination_flower = destination_flower;
    }

    public Flower DestinationFlower(){
        return destination_flower;
    }

    public int ExperienceAmount(){
        return experience_amount;
    }
}
