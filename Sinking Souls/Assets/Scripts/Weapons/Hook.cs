using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public float range;
    public float distanceOffset;
    public float detectionOffset;
    public float pullingSpeed;
    public GameObject hookObject;
    private GameObject tpTo = null;
    private GameObject currentHook;

    public bool move = false;

    public void LaunchHook() {
        GetComponent<Animator>().SetTrigger("Hook");
        currentHook = Instantiate(hookObject);
        currentHook.GetComponent<hookBehaviour>().target = tpTo;
        currentHook.GetComponent<hookBehaviour>().player = gameObject;
        currentHook.GetComponent<hookBehaviour>().move = true;
        GameController.instance.player.gameObject.layer = 10;//layer dash
    }

	public void Throw()
    {
        if(InputManager.LeftJoystick.magnitude == 0) {
            if (GetComponent<Player>().lockedEnemy != null) {
                float furthest = 0;
                GameObject aux = null;
                foreach (GameObject target in GameController.instance.roomEnemies) {
                    if (furthest < Vector3.Distance(target.transform.position, transform.position)) {
                        furthest = Vector3.Distance(target.transform.position, transform.position);
                        aux = target;

                    }
                }
                if (aux != null) {
                    tpTo = aux;
                    LaunchHook();
                    GetComponent<Player>().lockedEnemy = aux;
                }
            }
            else {
                float closest = range;
                GameObject aux = null;
                foreach (GameObject target in GameController.instance.roomEnemies) {
                    if (closest > Vector3.Distance(target.transform.position, transform.position)) {
                        closest = Vector3.Distance(target.transform.position, transform.position);
                        aux = target;

                    }
                }
                if (aux != null) {
                    tpTo = aux;
                    LaunchHook();
                    GetComponent<Player>().lockedEnemy = aux;
                }
            }
        }
        else {
            Vector3 direction = Camera.main.transform.forward.normalized * InputManager.LeftJoystick.y * -1 + (Quaternion.Euler(new Vector3(0, 90, 0)) * Camera.main.transform.forward.normalized) * InputManager.LeftJoystick.x;
            float minDist = range;
            Vector2 direction2 = new Vector2(direction.x, direction.z);
            
            foreach (GameObject target in GameController.instance.roomEnemies) {

                Vector3 enemyProjection = Vector3.Project(new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z), new Vector3(direction.x, 0, direction.z).normalized);
                Vector3 porjectionDirection = (transform.position + new Vector3(enemyProjection.x, 0, enemyProjection.z)) - new Vector3(target.transform.position.x, 0, target.transform.position.z);

                if (porjectionDirection.magnitude <= detectionOffset && Vector3.Angle(new Vector3(direction.x,0,direction.z), new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)) < 90){
                    if(Vector3.Distance(target.transform.position, transform.position) < minDist && target != GetComponent<Player>().lockedEnemy){
                        minDist = Vector3.Distance(target.transform.position, transform.position);
                        tpTo = target;
                    }
                }
            }
            if(tpTo != null) {
                LaunchHook();
                GetComponent<Player>().lockedEnemy = tpTo;
            }
        }
    }

    public void Update() {
        if (move) {
            GetComponent<Player>().m_PlayerState = Player.PlayerState.PULLING;
            if (tpTo != null && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(tpTo.transform.position.x, tpTo.transform.position.z)) > distanceOffset){
                float step = pullingSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, tpTo.transform.position, step);
            }
            else {
                GameController.instance.player.gameObject.layer = 9;//layer player

                GetComponent<Player>().m_PlayerState = Player.PlayerState.MOVING;
                GetComponent<Player>().lockedEnemy = tpTo;
                tpTo = null;
                move = false;
                GetComponent<Animator>().SetTrigger("EndHook");
                if (currentHook != null) Destroy(currentHook);
            }
        }
    }
}
