	using UnityEngine; 
	public class WristScript : MonoBehaviour { 
	
	//THIS DOES IT! YOU DON'T KNOW HOW BUT IT DOES IT!
	//This script goes on the finger, and retains a link to the hand.
    public Transform handTransform;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

	void Start(){
		positionOffset = handTransform.position - gameObject.transform.position;
		rotationOffset = handTransform.localEulerAngles - gameObject.transform.localEulerAngles;
	}

    private void Update()
    {
        // Calculate the combined transformation matrix
        Matrix4x4 transformationMatrix = Matrix4x4.TRS(
            handTransform.position + handTransform.TransformVector(positionOffset),
            handTransform.rotation * Quaternion.Euler(rotationOffset),
            Vector3.one
        );

        // Apply the transformation to the finger
        transform.position = transformationMatrix.GetColumn(3);
        transform.rotation = transformationMatrix.rotation;
    }
}
