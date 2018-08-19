using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    public float sensitivityX = .5f;
    public float sensitivityY = .5f;
    public float zoomLimitClose = -1;
    public float zoomLimitFar = -15;
    public float zoomSensitivity = 1.0f;
    public float maxY, maxX;
    private Vector2 startingWorldPos;
    private Vector3 newCameraPosition = Vector3.zero;
    private bool clickedState = false;

    private Inputs bonzaiInputManager;
	// Use this for initialization
	void Start () {
        // set up custom input management so you can handle having more than one input assigned to an axis.
        bonzaiInputManager = new Inputs();
        bonzaiInputManager.addMapping("Pan X", "Mouse X");
        bonzaiInputManager.addMapping("Pan Y", "Mouse Y");
        bonzaiInputManager.addMapping("Zoom", "Mouse ScrollWheel");
        bonzaiInputManager.addMapping("Drag", "Fire2");
        maxY = 11;
        maxX = 22;
	}

    // Update is called once per frame
    void Update()
    {
        float zoom = bonzaiInputManager.getAxis("Zoom") * zoomSensitivity;
        //bool drag = bonzaiInputManager.getButton("Drag") && (bonzaiInputManager.getAxis("Pan X") != 0 || bonzaiInputManager.getAxis("Pan Y") != 0);
        bool dragButtonDown = bonzaiInputManager.getButton("Drag");
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z * -1;
        Vector3 position = transform.position;
        float finalZ = position.z + zoom;
        finalZ = Mathf.Clamp(finalZ, zoomLimitFar, zoomLimitClose);
        if (dragButtonDown)
        {
            if (clickedState == false)
            {
                clickedState = true;
                startingWorldPos = getWorldPoint(mousePosition);
            }
        }
        else
        {
            clickedState = false;
        }
        if (clickedState)
        {
            Vector2 mouseScreenPos = getWorldPoint(mousePosition);
            Vector2 mouseDelta = mouseScreenPos - startingWorldPos;
            newCameraPosition = mouseDelta * -1;
            newCameraPosition = getClampedPanTranslation(newCameraPosition.x, newCameraPosition.y, position);
        }
        newCameraPosition.z = finalZ;
        Camera.main.transform.position = newCameraPosition;
    }

    private Vector2 getWorldPoint(Vector3 screenPoint)
    {
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public Vector3 getClampedPanTranslation(float panX, float panY, Vector3 position)
    {
        float posX = panX + position.x;
        float posY = panY + position.y;
        posX = Mathf.Clamp(posX, -1, maxX);
        posY = Mathf.Clamp(posY, maxY * -1, 1);
        Vector3 translation = new Vector3(posX, posY, 0);
        return translation;
    }
}
