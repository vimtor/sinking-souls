using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public float range;
    public float distanceOffset;

	public void Throw()
    {
        if (InputManager.ButtonY)
        {
            Debug.Log("PASO1");
            if (InputManager.LeftJoystick.magnitude != 0)
            {

            }
            else         
            {
                Debug.Log("PASO2");
                float furthest = 0;
                GameObject aux = null;
                foreach (GameObject target in GameController.instance.roomEnemies)
                {               
                    if(furthest < Vector3.Distance(target.transform.position, transform.position))
                    {
                        furthest = Vector3.Distance(target.transform.position, transform.position);
                        aux = target; 
                        
                    }                   
                }
                if (aux != null)
                {
                    transform.position = aux.transform.position + (transform.position - aux.transform.position).normalized * distanceOffset;
                    GetComponent<Player>().lockedEnemy = aux;
                }
              
            }
        }
    }

}
