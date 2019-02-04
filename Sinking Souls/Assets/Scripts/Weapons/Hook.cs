using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public float range;
    public float distanceOffset;

	public void Throw()
    {

            Debug.Log("PASO1");
            if (InputManager.LeftJoystick.magnitude != 0)
            {
                Vector3 direction = Camera.main.transform.forward.normalized * InputManager.RightJoystick.y * -1 + (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) * InputManager.RightJoystick.x;

                float closests = 180;
                Vector2 direction2 = new Vector2(direction.x, direction.z);

                foreach (GameObject target in GameController.instance.roomEnemies)
                {

                    float distance = Vector3.Distance(target.transform.position, transform.position);
                    float angle = Vector2.Angle(direction2, new Vector2(target.transform.position.x, target.transform.position.z) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.z));

                    if (distance <= range && angle < closests)
                    {
                        transform.position = target.transform.position + (transform.position - target.transform.position).normalized * distanceOffset;
                        GetComponent<Player>().lockedEnemy = target;
                        closests = angle;
                    }


                }
            }
            else if(GetComponent<Player>().lockedEnemy != null)        
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
