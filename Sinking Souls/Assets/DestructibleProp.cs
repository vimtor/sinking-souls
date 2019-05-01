using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleProp : MonoBehaviour {

	public GameObject destroyedVersion;	

	void OnMouseDown ()
	{
		Instantiate(destroyedVersion, transform.position, transform.rotation);
		Destroy(gameObject);	
	}
}
