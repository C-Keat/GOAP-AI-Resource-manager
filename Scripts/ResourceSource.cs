using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum ResourceType
{
    Food
}


public class ResourceSource : MonoBehaviour
{

    public ResourceType type;//this is where we will change the different resource types

    public int quantity;

    //events
    public UnityEvent onGuantityChange; //gets called when a resource is gathered, causes the event to take place

    public void GatherResource (int amount, Player gatheringPlayer)
    {
        quantity -= amount;

        int amountToGive = amount;

        // make sure we dont give more than we have
        if (quantity < 0)
        {
            amountToGive = amount + quantity;
        }

        //give the player the resource

        gatheringPlayer.GainResource(type, amountToGive);


        //if we are depleted, delete the resource
        if (quantity <= 0)
        {
            Destroy(gameObject);
        }

        //call the "onqualityChange" event
        if (onGuantityChange != null)
        {
            onGuantityChange.Invoke();
        }
         


    }

}
