using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float xOffset;
    [SerializeField]
    private float yOffset;
    [SerializeField]
    private float distanceFromTarget;
    [SerializeField]
    private float yRotationOffset;

    private bool cameraIsRightSide = true;

    private void Awake()
    {
        transform.localPosition += new Vector3(xOffset, yOffset, distanceFromTarget);
        transform.Rotate(new Vector3(0, -yRotationOffset, 0));
    }

    public void MoveCameraRight()
    {
        if (!cameraIsRightSide)
        {
            xOffset = Mathf.Abs(xOffset);
            transform.localPosition = new Vector3(xOffset, transform.localPosition.y, transform.localPosition.z);
            transform.Rotate(new Vector3(0, -yRotationOffset * 2, 0));
            cameraIsRightSide = true;
        }
    }

    public void MoveCameraLeft()
    {
        if (cameraIsRightSide)
        {
            xOffset = -Mathf.Abs(xOffset);
            transform.localPosition = new Vector3(xOffset, transform.localPosition.y, transform.localPosition.z);
            transform.Rotate(new Vector3(0, yRotationOffset * 2, 0));
            cameraIsRightSide = false;
        }
    }
}
