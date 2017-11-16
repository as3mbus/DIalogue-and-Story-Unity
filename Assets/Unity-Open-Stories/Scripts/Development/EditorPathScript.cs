using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

//draw path in editor using gizmos 
public class EditorPathScript : MonoBehaviour
{
    public Color rayColor = Color.white;
    public List<Transform> pathObjects = new List<Transform>();
    Transform[] theArray;

    // JsonData pathJson;
    // Pathing paths;
    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform>();
        pathObjects.Clear();
        foreach (Transform pathObject in theArray)
        {
            if (pathObject != this.transform)
                pathObjects.Add(pathObject);
        }
        // paths.x = new float[pathObjects.Count];
        // paths.y = new float[pathObjects.Count];
        // paths.z = new float[pathObjects.Count];
		for (int i = 0; i < pathObjects.Count; i++)
		{
            // paths.x[i] =pathObjects[i].position.x; 
            // paths.y[i] =pathObjects[i].position.y; 
            // paths.z[i] =pathObjects[i].position.z; 
			Vector3 position = pathObjects[i].position;
			if (i> 0){
				Vector3 previous = pathObjects[i-1].position;
				Gizmos.DrawLine(previous,position);
				Gizmos.DrawWireSphere(position,0.3f);
			}

		}
    }
}