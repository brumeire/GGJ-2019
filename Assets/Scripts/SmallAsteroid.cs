using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SmallAsteroid : Asteroid
{

    protected override void OnDetachedFromHand(Hand hand)
    {
        isDetached = true;
        //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.SetParent(null);
        //Vector3 totalMovement = (startingPlayerPos - player.transform.position);
        player.GetComponent<Rigidbody>().AddForce(GetDirectionPositionList() * 350f);
        gameObject.GetComponent<Rigidbody>().AddForce(-GetDirectionPositionList() * 350f);
        Debug.Log("grabing finish : " + GetDirectionPositionList());
    }
}
