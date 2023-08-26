using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// wrist script 2
/// this one goes on the real wrist, and makes the real wrist movement correctly drive a fake wrist and fake finger joint
/// </summary>
public class WristScript2 : MonoBehaviour
{
    public Transform fakeWrist;
    public Transform fakeFinger;//and then eventually this becomes a list??

    public Vector3 positionOffset;
    public Vector3 rotationOffset;

	void Start(){
        positionOffset = GetPositionOffset();
		rotationOffset = fakeWrist.localEulerAngles - fakeFinger.localEulerAngles;
	}

    private void Update()
    {
        //first, real wrist drives fake wrist
        fakeWrist.transform.rotation = gameObject.transform.rotation;

        //then, fake wrist drives fake fingers
        // Calculate the combined transformation matrix
        Matrix4x4 transformationMatrix = Matrix4x4.TRS(
            fakeWrist.position + fakeWrist.TransformVector(positionOffset),
            fakeWrist.rotation * Quaternion.Euler(rotationOffset),
            Vector3.one
        );

        // Apply the transformation to the finger
        fakeFinger.position = transformationMatrix.GetColumn(3);
        fakeFinger.rotation = transformationMatrix.rotation;
    }

    #region 
    //helper methods
    private Vector3 GetPositionOffset(){
        //There's no reasonable explanation for the following mess of code.
        //You basically tried things until it worked. 
        Vector3 tempPositionOffset = fakeWrist.position - fakeFinger.position;
        float distanceInX;
        if(fakeFinger.position.x > fakeWrist.position.x){
            distanceInX = Mathf.Abs(tempPositionOffset.x);
        }
        else{
            distanceInX = distanceInX = Mathf.Abs(tempPositionOffset.x) * -1.0f;
        }
        float distanceInY;
        if(fakeFinger.position.y > fakeWrist.position.y){
            distanceInY = Mathf.Abs(tempPositionOffset.y);
        }
        else{
            distanceInY = distanceInY = Mathf.Abs(tempPositionOffset.y) * -1.0f;
        }
        float distanceInZ;
        if(fakeFinger.position.z > fakeWrist.position.z){
            distanceInZ = Mathf.Abs(tempPositionOffset.z);
        }
        else{
            distanceInZ = distanceInZ = Mathf.Abs(tempPositionOffset.z) * -1.0f;
        }
        return new Vector3(distanceInX, distanceInY, distanceInZ);
    }

    #endregion
}
