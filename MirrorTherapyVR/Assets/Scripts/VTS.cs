using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script is part of MirrorTherapy
/// This script handles the synchronous visuotactile stimulation part of the study.
/// It uses a metronome to handle synchronization.
/// The Quest 2 has a headphone jack, so the best way to use this script is to have
/// the experimentor wear headphones which are connected to the participant's HMD.
/// It borrows many ideas from the repository located at https://github.com/a-kalus/virtual_rhi
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated March 2023
/// </summary>

public class VTS : MonoBehaviour
{
    public Text debugText;
    public AudioSource metronomeSound;
    public float tickTime;//the length of each metronome tick in seconds
    public int ticksBeforeStart;//the number of ticks before stroking starts
    public int ticksPerStroke;//how many metronome ticks the stroke should last for
    public int ticksBetweenStrokes;
    public ExperimentManager experimentManager;

    [Tooltip("0 indexed")]
    public int numberOfStrokes;//the number of times we do one stroke, not the number of times we do all possible strokes
    private int numberOfStrokesCompleted = 0;
    private float startTime;//for whatever phase we're in, this is when it started

    public GameObject mobileHand, mobileHandCollider, fixedHand;//during VTS, we turn off the hand and replace it with an identical model that won't move
    public GameObject paintbrush;

    public List<Transform> startPositions;
    public List<Transform> endPositions;
    private float strokeSpeed;
    private int strokeArrayCounter;
    private float interpolationRatio;//for the lerping-- see HandleLerp

    public bool currentlyStroking;

    private Phase phase;
    private enum Phase
    {
        NotStroking = 0,
        BeforeStarting = 1,
        DuringStroke = 2,
        BetweenStrokes = 3
    }

    // Start is called before the first frame update
    void Start()
    {
      strokeSpeed = 1.0f/ticksPerStroke/tickTime/50.0f;//we're using fixedUpdate, which runs at exactly 50fps
      debugText.text = "stroke speed = " + strokeSpeed;
      //if(metronomeSound==null){
      //  metronomeSound = gameObject.GetComponent<AudioSource>();
      //}
      currentlyStroking = false;
      phase = Phase.NotStroking;
    }

    void OnTriggerEnter(Collider other){
      debugText.text = "collision happens";
      if(other.gameObject == mobileHandCollider){
        StartStroking();
      }
    }

    public void StartStroking(){
      debugText.text = "stroking started";
      mobileHand.SetActive(false);
      fixedHand.SetActive(true);
      startTime = Time.time;
      currentlyStroking = true;
      phase = Phase.BeforeStarting;
      //start playing sound
      //disable real hand
      //turn on fake hand
    }

    public void EndStroking(){
      debugText.text = "stroking ended";
      paintbrush.SetActive(false);
      //turn on real hand
      mobileHand.SetActive(true);
      //disable fake hand
      fixedHand.SetActive(false);
      debugText.text = "CHECKPOINT1";//runs
      experimentManager.LoadButtonsTask();//this line seems to be what does it
      debugText.text = "CHECKPOINT2";//this does not get called
    }

    // Update is called once per frame-- FixedUpdate is called exactly 50X per second
    void FixedUpdate()
    {
      if(currentlyStroking){
        if(phase == Phase.BeforeStarting){
          if(Time.time >= startTime + (tickTime * ticksBeforeStart)){
            //start first stroke
            paintbrush.SetActive(true);
            startTime = Time.time;
            phase = Phase.DuringStroke;
          }
        }//end Phase BeforeStarting
        if(phase == Phase.DuringStroke){
          HandleLerp();
        }//end Phase DuringStroke
        if(phase == Phase.BetweenStrokes){
          if(numberOfStrokesCompleted>=numberOfStrokes){
            EndStroking();
          }
          else{
            if(Time.time >= startTime + (tickTime * ticksBetweenStrokes)){
              paintbrush.SetActive(true);
              startTime = Time.time;
              phase = Phase. DuringStroke;
            }
          }//end else
        }//end Phase BetweenStrokes
      }//end if
    }//end fixedupdate

    //we move all of the lerp stuff down here to keep fixedupdate less messy
    void HandleLerp(){
      if(interpolationRatio<1){
        paintbrush.transform.position = Vector3.Lerp(startPositions[strokeArrayCounter].position, endPositions[strokeArrayCounter].position, interpolationRatio);
        interpolationRatio+=strokeSpeed;
      }
      else{
        interpolationRatio = 0;
        numberOfStrokesCompleted++;
        debugText.text = "stroke array count = " + startPositions.Count;
        if(strokeArrayCounter<startPositions.Count-1){
          strokeArrayCounter++;
        }
        else{
          strokeArrayCounter = 0;
        }
        paintbrush.SetActive(false);
        startTime = Time.time;
        phase = Phase.BetweenStrokes;
      }
    }//end handle lerp
}
