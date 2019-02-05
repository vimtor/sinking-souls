using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public float range;
    public float distanceOffset;
    public float detectionOffset;

	public void Throw()
    {
        if(InputManager.LeftJoystick.magnitude == 0) {
            if (GetComponent<Player>().lockedEnemy != null) {
                Debug.Log("PASO1");
                float furthest = 0;
                GameObject aux = null;
                foreach (GameObject target in GameController.instance.roomEnemies) {
                    if (furthest < Vector3.Distance(target.transform.position, transform.position)) {
                        furthest = Vector3.Distance(target.transform.position, transform.position);
                        aux = target;

                    }
                }
                if (aux != null) {
                    transform.position = aux.transform.position + (transform.position - aux.transform.position).normalized * distanceOffset;
                    GetComponent<Player>().lockedEnemy = aux;
                }
            }
            else {
                Debug.Log("PASO2");
                float closest = range;
                GameObject aux = null;
                foreach (GameObject target in GameController.instance.roomEnemies) {
                    if (closest > Vector3.Distance(target.transform.position, transform.position)) {
                        closest = Vector3.Distance(target.transform.position, transform.position);
                        aux = target;

                    }
                }
                if (aux != null) {
                    transform.position = aux.transform.position + (transform.position - aux.transform.position).normalized * distanceOffset;
                    GetComponent<Player>().lockedEnemy = aux;
                }
            }
        }
        else {
            Debug.Log("Directed");
            Vector3 direction = Camera.main.transform.forward.normalized * InputManager.LeftJoystick.y * -1 + (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) * InputManager.LeftJoystick.x;
            float minDist = range;
            Vector2 direction2 = new Vector2(direction.x, direction.z);
            GameObject tpTo = null;

            foreach (GameObject target in GameController.instance.roomEnemies) {

                Vector3 enemyProjection = Vector3.Project(new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), new Vector3(direction.x, 0, direction.z).normalized);
                Vector3 porjectionDirection = (transform.position + new Vector3(enemyProjection.x, 0, enemyProjection.z)) - new Vector3(target.transform.position.x, 0, target.transform.position.z);

                if (porjectionDirection.magnitude <= distanceOffset){
                    if(Vector3.Distance(target.transform.position, transform.position)< minDist){
                        minDist = Vector3.Distance(target.transform.position, transform.position);
                        tpTo = target;
                    }
                }
            }
            if(tpTo != null) {
                transform.position = tpTo.transform.position + (transform.position - tpTo.transform.position).normalized * distanceOffset;
                GetComponent<Player>().lockedEnemy = tpTo;
            }

        }
    }
}
