using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// This script is part of MirrorTherapy
/// This script checks if the participant has touched each of their fingers to their thumb,
/// and calls ExperimentManager.LoadVTS() when they have.
///
/// Put this script on the thumb.
///
/// It works exactly like touch all to trigger from Coldspray.
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated May 2023
/// </summary>


public class FingerPinchDetector : MonoBehaviour
{
    public List<GameObject> touchThese;
    private List<bool> haveBeenTouched;

    public ExperimentManager experimentManager;

    public Text debugText;
    private int pinchesNumber=0;
    private bool alreadyTriggered = false;



    // Start is called before the first frame update
    void Start()
    {
        debugText.text = "finger pinch detector start function";
        //make a list of falses equal to the number of gameobjects in touch touchThese
        haveBeenTouched = new List<bool>();
        foreach (GameObject touchThis in touchThese)
        {
            haveBeenTouched.Add(false);
        }
    }//end start

    // Update is called once per frame
    void Update()
    {
        //second half of this conditional prevents it from triggering on empty lists
        if (!haveBeenTouched.Contains(false) && haveBeenTouched.Contains(true) && !alreadyTriggered)
        {
            alreadyTriggered = true;
            //if all objects in have been touched, do the thing!
            Destroy(GetComponent<BoxCollider>());//also for trying to make this not trigger over and over
            debugText.text = "calling loadVTS";
            experimentManager.FinishTask();
            Destroy(this);//and then make sure we don't do this again
        }
    }//end update

    void OnTriggerEnter(Collider other)
    {
        debugText.text = "finger pinch detector OnTriggerEnter function: " + pinchesNumber;

        if (touchThese.Contains(other.gameObject))
        {
            pinchesNumber++;
            int index = touchThese.IndexOf(other.gameObject);
            haveBeenTouched[index] = true;
        }
    }//end on trigger enter
}
