using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All the visual stuff for a bonzai element
/// </summary>
public class BonzaiGameObject : MonoBehaviour {

    Vector3 targetPosition;
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public void setTargetPosition(Vector3 target)
    {
        this.targetPosition = target;
    }

	// Use this for initialization
	void Start () {
        targetPosition = transform.position;
	}

    private void Awake()
    {
        Start();
    }

    // Update is called once per frame
    void Update () {
        if (targetPosition != null && transform.position != targetPosition) 
        {
            float movementSpeed = Time.deltaTime / GameLoader.playbackSpeedMs;
            transform.position = Vector3.Lerp(transform.position, targetPosition, movementSpeed);
        }
	}


}
