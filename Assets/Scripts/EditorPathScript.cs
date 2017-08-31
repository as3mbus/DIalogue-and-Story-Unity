using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EditorPathScript : MonoBehaviour
{
    public Color rayColor = Color.white;
    public List<Transform> pathObjects = new List<Transform>();
    Transform[] theArray;

    Pathing paths;
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
        paths.x = new float[pathObjects.Count];
        paths.y = new float[pathObjects.Count];
        paths.z = new float[pathObjects.Count];
		for (int i = 0; i < pathObjects.Count; i++)
		{
            paths.x[i] =pathObjects[i].position.x; 
            paths.y[i] =pathObjects[i].position.y; 
            paths.z[i] =pathObjects[i].position.z; 
			Vector3 position = pathObjects[i].position;
			if (i> 0){
				Vector3 previous = pathObjects[i-1].position;
				Gizmos.DrawLine(previous,position);
				Gizmos.DrawWireSphere(position,0.3f);
			}

		}
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.S)){
            Pathing tesPath=new Pathing();
            //tesPath.paths = pathObjects.ToArray;
        }
    }
}

[System.Serializable]
public class Pathing{
    public float[] x,y,z;
    //public List<float> camSize;
}