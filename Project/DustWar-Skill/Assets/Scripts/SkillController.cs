using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SkillController : MonoBehaviour {

    public float outerCircleRadius = 100;

    Transform innerCircleTrans;

    Vector2 outerCircleStartWorldPos = Vector2.zero;

    public Action<Vector2> showSkill;
    public Action hideSkill;
    public Action<Vector2> moveSkill;

    void Awake()
    {
        innerCircleTrans = transform.GetChild(0);
    }

    void Start()
    {
        outerCircleStartWorldPos=new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            
            if (showSkill != null)
                showSkill(innerCircleTrans.localPosition / outerCircleRadius);

            Vector2 touchPos =(Vector2)Input.mousePosition - outerCircleStartWorldPos;
            if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
                innerCircleTrans.localPosition = touchPos;
            else
                innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

            if (moveSkill != null)
                moveSkill(innerCircleTrans.localPosition / outerCircleRadius);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            innerCircleTrans.localPosition = Vector3.zero;
            if (hideSkill != null)
                hideSkill();
        }

    }
 
}
