using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is part of MirrorTherapy
///
/// This script is honestly a bit of over-engineering,
/// but you didn't want to rely on the hierarchy order of hazards.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated May 2024
/// </summary>

public class HazardHandler : MonoBehaviour
{
    public List<GameObject> hazards;
    private IEnumerator coroutine;

    public void ActivateHazardAfterDelay(int hazardIndex){
      DeactivateAllHazards();
      if(hazardIndex>1){
        hazards[hazardIndex-2].SetActive(true);//-2 because the other list is 1 indexed, and 1 doesn't do anything
      }
    }

    public void DeactivateAllHazards(){
      foreach(GameObject hazard in hazards){
        hazard.SetActive(false);
      }
    }

    public void DeactivateHazard(int hazardIndex){
      if(hazardIndex>1){
        hazards[hazardIndex-2].SetActive(false);//-2 because the other list is 1 indexed, and 1 doesn't do anything
      }
    }
}
