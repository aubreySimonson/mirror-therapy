using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all MirroredPoints. 
/// The newer, better version of HandPositionMirror,
/// because we're now doing two *different* matrix transformations to the fake hand.
/// Should go on the real hand wrist.
/// 
/// Last edited September 2023
/// ???-->simonson.au@northeastern.edu
/// </summary>

public class MirroredPointsManager : MonoBehaviour
{
    public MirroredPoint [] mirroredPoints;
    public GameObject realWrist, fakeWrist;

    OVRSkeleton skeleton;
    public GameObject realHand;//should be a gameobject with OVRSkeleton on it



    public float adjust;//how far to translate the other hand, if we aren't doing a true mirror

    // Start is called before the first frame update
    void Start()
    {
        //if there's no real wrist specified, assume that this script was put on the real wrist and someone was just lazy
        if(realWrist==null){
            realWrist = gameObject;
        }

        //get all mirroredPoints, so that we never have to deal with that in the inspector.
        mirroredPoints = GameObject.FindObjectsOfType<MirroredPoint>();
        //you're going to then have to trim the above to only have the correct hand of points

        //assign real and fake wrists here, so that we never have to deal with that in the inspector
        foreach(MirroredPoint mP in mirroredPoints){
            mP.SetRealWrist(realWrist.transform);
            mP.SetFakeWrist(fakeWrist.transform);
        }

        skeleton = realHand.GetComponent<OVRSkeleton>();

        AssignBones();
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
        fakeWrist.transform.rotation = realWrist.transform.rotation;

        //rotate all of the fingers of the fake hand to the right spot
        //(second matrix transformation...)
        foreach(MirroredPoint mP in mirroredPoints){
            mP.RotateFromWrist();
        }
    }

    //OVRBones can't be assigned in the inspector. This is the best solution I've found thus far.
    private void AssignBones(){
        //iterate over the skeleton
        //if bone id = [id], get the bone with the corresponding bone name and add it to the thing
        foreach (OVRBone bone in skeleton.Bones)
        {
            if(bone.Id == OVRSkeleton.BoneId.Hand_WristRoot){
                GetBoneByName("wrist").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_ForearmStub){
                GetBoneByName("forearm").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Index1){
                GetBoneByName("index-1").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Index2){
                GetBoneByName("index-2").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Index3){
                GetBoneByName("index-3").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Middle1){
                GetBoneByName("middle-1").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Middle2){
                GetBoneByName("middle-2").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Middle3){
                GetBoneByName("middle-3").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky0){
                GetBoneByName("pinky-0").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky1){
                GetBoneByName("pinky-1").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky2){
                GetBoneByName("pinky-2").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Pinky3){
                GetBoneByName("pinky-3").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Ring1){
                GetBoneByName("ring-1").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Ring2){
                GetBoneByName("ring-2").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Ring3){
                GetBoneByName("ring-3").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb0){
                GetBoneByName("thumb-0").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb1){
                GetBoneByName("thumb-1").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb2){
                GetBoneByName("thumb-2").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_Thumb3){
                GetBoneByName("thumb-3").SetRealBone(bone);
            }
            if(bone.Id == OVRSkeleton.BoneId.Hand_ThumbTip){
                GetBoneByName("thumb-tip").SetRealBone(bone);
            }
        }
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
