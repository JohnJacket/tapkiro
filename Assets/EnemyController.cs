using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    MID = 0,
    CUSTOM
}


public class EnemyController : MonoBehaviour {

    public int life = 1;
    public float speed = 1.0f;

    private bool isMoving = false;
    private bool isCollided = false;
    private int xDirection = 1;
    private int yDirection = 1;

    // Use this for initialization
    void Start () {
        if (transform.position.x >= 0)
            xDirection = -1;

        if (transform.position.y >= 0)
            yDirection = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x >= 0)
            xDirection = -1;
        else
            xDirection = 1;

        if (transform.position.y >= 0)
            yDirection = -1;
        else
            yDirection = 1;

        if (!isMoving)
        {
            isMoving = true;
            StartCoroutine(Moving(MoveType.MID));
        }

    }

    private void OnDestroy()
    {
        isMoving = false;
    }

    IEnumerator Moving(MoveType moveType)
    {
        while (true)
        {
            if (!isMoving)
                break;

            if (moveType == MoveType.MID)
            {
                transform.Translate(xDirection * speed * Time.deltaTime, yDirection * speed * Time.deltaTime, 0);
            }
            else
            {
                // calculate speed of move
                transform.Translate(xDirection * speed * Time.deltaTime, yDirection * speed * Time.deltaTime, 0);
            }


            yield return new WaitForEndOfFrame();
        }
    }

    public void OnHit()
    {
        life--;

        if (life <= 0)
        {
            if (!isCollided)
            {
                isCollided = true;
                isMoving = false;
                EventManager.TriggerEvent("enemyKilled");
                Destroy(gameObject);
            }
        }
        else
        {
            EventManager.TriggerEvent("enemyLifeDestroyed");
        }

    }
}
