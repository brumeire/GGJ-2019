using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Asteroid : MonoBehaviour {

    private Interactable interactable;
    private Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;
    private GameObject player;
    private Vector3 startingHandPos;
    private Vector3 previousHandPos;
    private float durationGrab = 0f;

    private Vector3 startingPlayerPos;
    private List<Vector3> positionList = new List<Vector3>();

    private Vector3 previousAsteroidPos;

    private bool isDetached = true;

    protected virtual void Awake()
    {
        interactable = GetComponent<Interactable>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
        {
            isDetached = false;
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);

            Debug.Log("grabing start hand position :  " + hand.transform.position);
            startingHandPos = hand.transform.position;
            previousHandPos = startingHandPos;
            previousAsteroidPos = transform.position;
            
            durationGrab = 0f;
            player.transform.SetParent(this.gameObject.transform);
            //startingPlayerPos = player.transform.position;
            ClearPositionList();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    protected virtual void HandAttachedUpdate(Hand hand)
    {
        if (!isDetached)
        {
            UpdatePositionList(player.transform.position);
            Debug.Log("grabing start hand position :  " + hand.transform.position);
            durationGrab += Time.deltaTime;
            //Debug.Log("grab is on the way : " + hand.transform.position);

            Vector3 asteroidMovement = transform.position - previousAsteroidPos;
            Vector3 playerMovement = previousHandPos - hand.transform.position;

            player.transform.position = player.transform.position + playerMovement + asteroidMovement;
            previousHandPos = hand.transform.position;
            previousAsteroidPos = transform.position;
           


        }
        if (hand.IsGrabEnding(this.gameObject))
        {
            hand.DetachObject(gameObject);
            Debug.Log("grabing ending, duration grab : " + durationGrab);
        }
        
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        isDetached = true;
        //player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.SetParent(null);
        //Vector3 totalMovement = (startingPlayerPos - player.transform.position);
        player.GetComponent<Rigidbody>().AddForce(GetDirectionPositionList() * 500f);
        Debug.Log("grabing finish : " + GetDirectionPositionList());
    }

    private void UpdatePositionList(Vector3 v)
    {
        positionList.Add(v);
        if (positionList.Count >= 10)
        {
            positionList.RemoveAt(0);          
        }
    }

    private Vector3 GetDirectionPositionList()
    {
        Vector3 pos = positionList[positionList.Count - 1] - positionList[0];
        return pos;
    }

    private void ClearPositionList()
    {
        positionList.Clear();
    }


}
