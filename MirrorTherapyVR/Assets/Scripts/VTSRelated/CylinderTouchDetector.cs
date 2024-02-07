using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is part of MirrorTherapy
/// It goes on each of the two cylinders involved in VTS, 
/// to detect when the correct real hand is touching it.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated February 2024
/// </summary>
public class CylinderTouchDetector : MonoBehaviour
{
    public bool isLeft;//is on the left side of the table. If false, this is on the right side of the table.
    public GameObject correctHand;//a collider on the associated real hand. Should not be mirrored. 
    public VTS vts;

    void OnTriggerEnter(Collider other){
        if(other.gameObject == correctHand){
            if(isLeft){
                vts.leftCylinderContacted = true;
            }
            else{
                vts.rightCylinderContacted = true;
            }
        }
    }

        void OnTriggerExit(Collider other){
        if(other.gameObject == correctHand){
            if(isLeft){
                vts.leftCylinderContacted = false;
            }
            else{
                vts.rightCylinderContacted = false;
            }
        }
    }


}
