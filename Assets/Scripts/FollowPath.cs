using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public EditorPathScript pathRoute;
    public int currentWaypoint = 0;
    public float speed = 1f, routeRadius = 1f, rotationSpeed = 5f;
	public string pathName;

	Vector3 lastPosition, currentPosition;

    // Use this for initialization
    void Start()
    {
		pathRoute = GameObject.Find(pathName).GetComponent<EditorPathScript>();
		lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		float distance = Vector3.Distance(pathRoute.pathObjects[currentWaypoint].position,transform.position);
		transform.position = Vector3.MoveTowards(transform.position,pathRoute.pathObjects[currentWaypoint].position,Time.deltaTime * speed );
		if ( Input.GetButtonDown("Fire1")&&distance<= routeRadius && currentWaypoint< pathRoute.pathObjects.Count){

			currentWaypoint++;
		}
    }
}
