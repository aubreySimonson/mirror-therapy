using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Manages all MirroredPoints for one hand. 
/// The newer, better version of HandPositionMirror,
/// because we're now doing two *different* matrix transformations to the fake hand.
/// Should go on the real hand wrist.
/// 
/// Last edited October 2023
/// ???-->simonson.au@northeastern.edu
/// </summary>

public enum UnderlyingHandedness {Right, Left};//match the handedness of the real hand, not the fake hand-- god this is all confusing


public class MirroredPointsManager : MonoBehaviour
{
    public List<MirroredPoint> mirroredPoints;
    public GameObject realWrist, fakeWrist;

    OVRSkeleton skeleton;
    public GameObject realHand;//should be a gameobject with OVRSkeleton on it


    public float adjust;//how far to translate the other hand, if we aren't doing a true mirror

    public UnderlyingHandedness underlyingHandedness;

    public Text debugText;


    // Start is called before the first frame update
    void Start()
    {
        debugText.text = "hello from the start function";
        //if there's no real wrist specified, assume that this script was put on the real wrist and someone was just lazy
        if(realWrist==null){
            realWrist = gameObject;
        }

        //get all mirroredPoints, so that we never have to deal with that in the inspector.
        MirroredPoint [] points = GameObject.FindObjectsOfType<MirroredPoint>();
        mirroredPoints = new List<MirroredPoint>();
        foreach(MirroredPoint p in points){
            if(p.underlyingHandedness == underlyingHandedness){
                mirroredPoints.Add(p);
            }
        }
        //you're going to then have to trim the above to only have the correct hand of points

        //assign real and fake wrists here, so that we never have to deal with that in the inspector
        foreach(MirroredPoint mP in mirroredPoints){
            mP.SetRealWrist(realWrist.transform);
            mP.SetFakeWrist(fakeWrist.transform);
        }

        skeleton = realHand.GetComponent<OVRSkeleton>();
        debugText.text = "got skeleton";
        AssignBones();
        debugText.text = "bones assigned";
    }

    // Update is called once per frame
    void Update()
    {
        //if we're doing the sensible translation thing, recalculate adjust...

        //make the fake hand an exact mirror of this one
        foreach(MirroredPoint mP in mirroredPoints){
            mP.Reflect();
        }

        //recalculate the offset after that first matrix transformation...
        foreach(MirroredPoint mP in mirroredPoints){
            mP.RecalculateOffset();
        }

        //real wrist rotation drives fake wrist rotation
        //fakeWrist.transform.rotation = realWrist.transform.rotation;//<--this makes every bone be backwards in the worst possible way.
        //try to make it not-backwards
        //...is this supposed to happen before we recalculate the offset
        fakeWrist.transform.rotation = realWrist.transform.rotation * Quaternion.Euler(0, 180, 0);
        

        //rotate all of the fingers of the fake hand to the right spot
        //(second matrix transformation...)
        foreach(MirroredPoint mP in mirroredPoints){
            mP.RotateFromWrist();
        }

        debugText.text = "wrist is at " + mirroredPoints[0].gameObject.transform.position;
    }

    //OVRBones can't be assigned in the inspector. This is the best solution I've found thus far.
    private void AssignBones(){
        //iterate over the skeleton
        //if bone id = [id], get the bone with the corresponding bone name and add it to the thing
        foreach (OVRBone bone in skeleton.Bones)
        {
            if(bone.Id == OVRSkeleton.BoneId.Hand_WristRoot){
                try{
                    GetBoneByName("wrist").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - wrist (that's REAL bad)";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_ForearmStub){
                //GetBoneByName("forearm").SetRealBone(bone);
                try{
                    GetBoneByName("forearm").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - forearm";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Index1){
                //GetBoneByName("index-1").SetRealBone(bone);
                try{
                    GetBoneByName("index-1").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - index-1";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Index2){
                //GetBoneByName("index-2").SetRealBone(bone);
                try{
                    GetBoneByName("index-2").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - index-2";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Index3){
                //GetBoneByName("index-3").SetRealBone(bone);
                try{
                    GetBoneByName("index-3").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - index-3";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Middle1){
                //GetBoneByName("middle-1").SetRealBone(bone);
                try{
                    GetBoneByName("middle-1").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - middle-1";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Middle2){
                //GetBoneByName("middle-2").SetRealBone(bone);
                try{
                    GetBoneByName("middle-2").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - middle-2";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Middle3){
                //GetBoneByName("middle-3").SetRealBone(bone);
                try{
                    GetBoneByName("middle-3").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - middle-3";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky0){
                //GetBoneByName("pinky-0").SetRealBone(bone);
                try{
                    GetBoneByName("pinky-0").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - pinky-0";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky1){
                //GetBoneByName("pinky-1").SetRealBone(bone);
                try{
                    GetBoneByName("pinky-1").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - pinky-1";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky2){
                //GetBoneByName("pinky-2").SetRealBone(bone);
                try{
                    GetBoneByName("pinky-2").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "pinky-2";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky3){
                //GetBoneByName("pinky-3").SetRealBone(bone);
                try{
                    GetBoneByName("pinky-3").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - pinky-3";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Ring1){
                //GetBoneByName("ring-1").SetRealBone(bone);
                try{
                    GetBoneByName("ring-1").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - ring-1";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Ring2){
                //GetBoneByName("ring-2").SetRealBone(bone);
                try{
                    GetBoneByName("ring-2").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - ring-2";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Ring3){
                //GetBoneByName("ring-3").SetRealBone(bone);
                try{
                    GetBoneByName("ring-3").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - ring-3";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb0){
                //GetBoneByName("thumb-0").SetRealBone(bone);
                try{
                    GetBoneByName("thumb-0").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - thumb-0";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb1){
                //GetBoneByName("thumb-1").SetRealBone(bone);
                try{
                    GetBoneByName("thumb-1").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - thumb-1";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb2){
                //GetBoneByName("thumb-2").SetRealBone(bone);
                try{
                    GetBoneByName("thumb-2").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - thumb-2";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb3){
                //GetBoneByName("thumb-3").SetRealBone(bone);
                try{
                    GetBoneByName("thumb-3").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - thumb-3";
                }
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_ThumbTip){
                //GetBoneByName("thumb-tip").SetRealBone(bone);
                try{
                    GetBoneByName("thumb-tip").SetRealBone(bone);
                }
                catch(Exception e){
                    debugText.text = "missing bone - thumb-tip";
                }
            }
        }
        debugText.text = "bones were assigned";
    }

    private MirroredPoint GetBoneByName(string name){
        foreach(MirroredPoint mP in mirroredPoints){
            if(mP.boneName == name){
                return mP;
            }
        }
        //we shouldn't get here
        return null;
    } 
}
