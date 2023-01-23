using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReplay : MonoBehaviour
{
    AudioSource recordedClip;
    string deviceName;
    public int numberOfSecondsToRecordFor;

    // Start is called before the first frame update
    void Start()
    {
      recordedClip = gameObject.GetComponent<AudioSource>();
      foreach(string mic in Microphone.devices){
        Debug.Log("First of all, the type of this variable is: " + mic.GetType().ToString());
        Debug.Log("Microphone device:" + mic);
      }
      deviceName = Microphone.devices[0];
      Debug.Log("using: " + deviceName);
    }

    public void PlayRecordedAudio(){
      recordedClip.Play();
    }

    public void StartRecording(){
      recordedClip.clip = Microphone.Start(deviceName, false, numberOfSecondsToRecordFor, 44100);
    }

    public void StopRecording(){

    }
}
