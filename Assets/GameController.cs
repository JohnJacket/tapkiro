using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PushType
{
    DOWN = 0,
    UP
}

public class GameController : MonoBehaviour {

    RuntimePlatform platform;

    private static GameController instance;

    GameObject hitGameObject;
    int hitLayerMask;
    int uiLayerMask;

    public static GameController Instance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        platform = Application.platform;
        hitLayerMask = 1 << LayerMask.NameToLayer("Hit");
        uiLayerMask = 1 << LayerMask.NameToLayer("UI");
        hitGameObject = null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: to level view interaction class
        if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    CheckTouch(Input.GetTouch(0).position, PushType.DOWN);
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    CheckTouch(Input.GetTouch(0).position, PushType.UP);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckClick(Input.mousePosition, PushType.DOWN);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CheckClick(Input.mousePosition, PushType.UP);
            }
        }
    }

    // TODO: to level view interaction class
    void CheckTouch(Vector2 position, PushType pushType)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        Vector2 touchPosition = new Vector2(worldPosition.x, worldPosition.y);

        Collider2D hit = null;

        if (pushType == PushType.DOWN)
        {
            hit = Physics2D.OverlapPoint(touchPosition, hitLayerMask | uiLayerMask);

            if (hit)
            {
                try
                {
                    hitGameObject = hit.transform.gameObject;
                    hit.transform.gameObject.SendMessage("DownEvent");
                }
                catch
                {

                }
            }
        }
        else if (pushType == PushType.UP)
        {
            try
            {
                hitGameObject.SendMessage("UpEvent");
            }
            finally
            {
                hitGameObject = null;
            }
        }
    }

    void CheckClick(Vector3 position, PushType pushType)
    {
        RaycastHit2D hit;

        if (pushType == PushType.DOWN)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, hitLayerMask | uiLayerMask);
            if (hit)
            {
                if (hit.collider)
                {
                    try
                    {
                        hit.transform.gameObject.SendMessage("DownEvent");
                    }
                    catch
                    {

                    }

                }
            }
        }
        else if (pushType == PushType.UP)
        {
            try
            {
                if (hitGameObject)
                    hitGameObject.SendMessage("UpEvent");
            }
            finally
            {
                hitGameObject = null;
            }
        }
    }
}
