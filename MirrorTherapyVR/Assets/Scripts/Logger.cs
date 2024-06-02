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
/// Last updated May 2024
/// </summary>

public class Logger : MonoBehaviour
{

    public Text debugText;

    //this nasty system of booleans really should be an enum
    public bool logEssentialFakeHandPoints;
    public bool logEssentialRealHandPoints;

    //we do this odd little workaround because OVRBones can't be exposed in the inspector
    private List<OVRBone> essentialRealHandBones;
    public List<MirroredPoint> essentialRealHandPoints;
    public List<Transform> essentialFakeHandPoints;
    public bool logHeadPose;
    public bool recordOnPlay;
    public bool isRecording;

    List<OVRBone> realHandPoints;
    private List<Transform> fakeHandPoints;
    public Transform headTransform;

    private string recordedDataString = "";
    private string columnHeaders = "";
    public string itemDelimiter = ";";//we don't use a comma because there tend to already be commas in our data
    public string lineBreak = "!";//look, having something very weird makes data processing easier
    public string specialFlag = "***";//for lines where we needed to mention something other than position data

    private string path;//path to save file
    private StreamWriter writer;

    // Start is called before the first frame update
    void Start()
    {
      if(recordOnPlay){
        isRecording = true;
      }
      //if we don't have write permission, ask for it.
      if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

      string timeStamp = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss"); //use date time as unique id for each participant
      path = Application.persistentDataPath + timeStamp + "-log.txt";//test printing at all for now-- add time stamps later
      writer = new StreamWriter(path, true);
      writer.WriteLine("Start of study performed at " + timeStamp);
      writer.Flush();
      writer.Close();
      essentialRealHandBones = new List<OVRBone>();
    }//end start

    // Update is called once per frame
    void FixedUpdate()
    {
      //add any information we're gathering this frame to recordedDataString
      if(isRecording){
        recordedDataString += Time.time + itemDelimiter;//Time.time is the number of seconds since the start of the game
        if(logHeadPose){
          recordedDataString += headTransform.position + itemDelimiter;
          recordedDataString += headTransform.rotation + itemDelimiter;
        }//end if
        if(logEssentialRealHandPoints){
          foreach(OVRBone realHandPoint in essentialRealHandBones){
            recordedDataString += realHandPoint.Transform.position + itemDelimiter;
            recordedDataString += realHandPoint.Transform.rotation + itemDelimiter;
          }
        }//end if
        if(logEssentialFakeHandPoints){
          foreach(Transform fakeHandPoint in essentialFakeHandPoints){
            recordedDataString += fakeHandPoint.position + itemDelimiter;
            recordedDataString += fakeHandPoint.rotation + itemDelimiter;
          }
        }//end if
        recordedDataString += lineBreak;
      }//end if
    }//end class

    public void Log(string textToLog){
      // writer = new StreamWriter(path, true);
      // //write column headers again
      // writer.WriteLine(textToLog);
      //
      // //write data
      // writer.Flush();
      // writer.Close();
      recordedDataString += specialFlag + textToLog + lineBreak;
    }

    //we use pause instead of quit because quit just... doesn't work? Known bug: https://forum.unity.com/threads/onapplicationquit-and-ondestroy-is-not-executed-when-exit-app-on-oculus-quest.795942/
    void OnApplicationPause()
    {
      SaveData();
    }

    public void SetUpDataCollection(){
      //we can't just put any of this in the start function because we need the hand to be assembled /before/ we ask for it

      Log("Set Up Data Collection was called");
      columnHeaders += "TimeStamp" + itemDelimiter;
      //log column headings
      if(logHeadPose){
        columnHeaders += "HeadPos" + itemDelimiter + "HeadRot" + itemDelimiter;
      }

      if(logEssentialRealHandPoints){//do this one after you do the fake hand-- maybe not at all
        Log("Logging real hand points");//runs
        //get all bones from mPs-- OVR Bones can't be exposed directly in the inspector, so this is our workaround
        foreach(MirroredPoint mP in essentialRealHandPoints){
          columnHeaders += "Real" + mP.boneName + "Pos" + itemDelimiter;
          columnHeaders += "Real" + mP.boneName + "Rot" + itemDelimiter;
          // Log("added " + mP.realBone.Id.ToString() + " to column headers");//adds bones, but, like, not the right bones?
          essentialRealHandBones.Add(mP.realBone);
          Log("Added a bone");//doesn't run
        }
        //debugText.text = "real hand points are " + realHandPoints[0].ToString();
        // foreach(OVRBone handPoint in essentialRealHandBones){
        //   //we actually need to understand the order these happen in first
        //   columnHeaders += "Real" + handPoint.Id.ToString() + "Pos" + itemDelimiter;
        //   columnHeaders += "Real" + handPoint.Id.ToString() + "Rot" + itemDelimiter;
        //   Log("added " + handPoint.Id.ToString() + " to column headers");
        // }
      }//end real hand points
      if(logEssentialFakeHandPoints){
        foreach(Transform fakeBone in essentialFakeHandPoints){
          columnHeaders += "Fake " + fakeBone.gameObject.name + "Pos" + itemDelimiter;
          columnHeaders += "Fake " + fakeBone.gameObject.name + "Rot" + itemDelimiter;
          debugText.text = "fake hand points are " + fakeBone.gameObject.name;
        }
      }//end fake hand points

      columnHeaders+=lineBreak;
      InvokeRepeating("SaveData", 30.0f, 30.0f);
    }//end det up data collection

    //saves any data currently in the recorded string
    public void SaveData(){
      writer = new StreamWriter(path, true);
      //write column headers again
      writer.WriteLine(columnHeaders);
      //recording the data string and then writing the whole thing on application quit might be the wrong way to do it.
      //the other way to do it would be to write the data to the file every frame.
      //there's a higher chance for conflict if two things try to log information at the same time that way, though
      writer.WriteLine(recordedDataString);
      recordedDataString = "";

      //write data
      writer.Flush();
      writer.Close();
    }
}//end class
