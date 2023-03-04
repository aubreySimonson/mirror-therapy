using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Android;



/// <summary>
/// This script is part of MirrorTherapy
/// This script handles all logging.
/// It borrows many ideas from the Cornell Repository located at https://github.com/eric-cornellvel/VR-MirrorTherapy
///
///On a Quest 2, this will save data to Android/data/app name
///If you're looking at this script to try to figure out why your logging in a different project isn't working,
///remember to set your Write Permissions to External in the Player settings
///
/// ???--> Aubrey (followspotfour@gmail.com)
/// Last updated February 2023
/// </summary>

public class Logger : MonoBehaviour
{

    public Text debugText;

    public bool logAllRealHandPoints;
    public bool logAllFakeHandPoints;
    public bool logHeadPose;
    public bool recordOnPlay;
    public bool isRecording;

    public HandPositionMirror handPositionMirror;
    List<OVRBone> realHandPoints;
    private List<Transform> fakeHandPoints;

    private string recordedDataString;
    private string columnHeaders = "";
    public string itemDelimiter = ";";//we don't use a comma because there tend to already be commas in our data
    public string lineBreak = "!";//look, having something very weird makes data processing easier

    private string path;//path to save file
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
      //if we don't have write permission, ask for it.
      if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

      string timeStamp = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss"); //use date time as unique id for each participant
      path = Application.persistentDataPath + timeStamp + "log.txt";//test printing at all for now-- add time stamps later
      writer = new StreamWriter(path, true);
      writer.WriteLine("Start of study performed at " + timeStamp);
      writer.Flush();
      writer.Close();
    }//end start

    // Update is called once per frame
    void Update()
    {
      //add any information we're gathering this frame to recordedDataString
    }

    public void SetUpDataCollection(){
      //we can't just put any of this in the start function because we need the hand to be assembled /before/ we ask for it
      if(handPositionMirror == null){
        handPositionMirror = (HandPositionMirror)FindObjectOfType(typeof(HandPositionMirror));
      }

      //log column headings
      if(logHeadPose){
        columnHeaders += "HeadPos" + itemDelimiter + "headRot" + itemDelimiter;
      }

      if(logAllRealHandPoints){
        //get hand points from HandPositionMirror
        realHandPoints = handPositionMirror.GetRealBones();
        debugText.text = "real hand points are " + realHandPoints[0].ToString();
        foreach(OVRBone handPoint in realHandPoints){
          debugText.text = "bone name is " + handPoint.Id.ToString();
          //we actually need to understand the order these happen in first
          columnHeaders += handPoint.Id.ToString();
        }
      }//end real hand points
      if(logAllFakeHandPoints){
        debugText.text = "we haven't implemented that feature yet";
      }
      SaveData();//once we're up and running, we won't be calling this in the start function
    }

    //saves any data currently in the recorded string
    public void SaveData(){
      writer = new StreamWriter(path, true);
      //write column headers again
      writer.WriteLine(columnHeaders);

      //write data
      writer.Flush();
      writer.Close();
    }
}
