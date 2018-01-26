using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SkillController : MonoBehaviour
{
    #region 技能指示器代码改动部分
    List<Transform> Trigers = new List<Transform>(); 
    List<GameObject> EnemysInSkill = new List<GameObject>();
    public SkillAreaType SkillType;
    private SkillArea SkillAreaParam;
    #endregion
    public float outerCircleRadius = 12f;
    Transform innerCircleTrans;
    //Vector2 outerCircleStartWorldPos = Vector2.zero;

    public Action<Vector2> showSkill;
    public Action hideSkill;
    public Action<Vector2> moveSkill;
    void Awake()
    {
        innerCircleTrans = transform.GetChild(0);
        
        Trigers.Add (transform.GetChild(1));
        Trigers.Add  (transform.GetChild(2));
    }

    void Start()
    {
        Trigers[0].gameObject.SetActive(false);
        Trigers[1].gameObject.SetActive(false);
        SkillAreaParam=GetComponent<SkillArea>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
#region 技能指示器代码改动部分
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit,1000f,layerMask);
            Vector3 TemptouchPos =new Vector3(hit.point.x - transform.position.x,0,hit.point.z-transform.position.z);
            Vector2 touchPos = new Vector2(TemptouchPos.x, TemptouchPos.z);
            if (showSkill != null)
                showSkill(innerCircleTrans.localPosition);
            if (touchPos.magnitude < outerCircleRadius)
               innerCircleTrans.localPosition = touchPos;
            else
                innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

            if (moveSkill != null)
                moveSkill(innerCircleTrans.localPosition);
#endregion
            //技能检测前触开始发器设置
            SkillCheckStart(SkillType);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            innerCircleTrans.localPosition = Vector3.zero;
            if (hideSkill != null)
                hideSkill();
            //技能触发
            SkillCheckTrigger();
        }

    }
    //技能开始检测，根据技能类型设置触发器大小和位置
    void SkillCheckStart(SkillAreaType checkMode)
    {
        switch (checkMode)
        {
            case SkillAreaType.OuterCircle:
                Trigers[0].gameObject.SetActive(true);
                Trigers[0].position = transform.position;
                Trigers[0].localScale=new Vector3(SkillAreaParam.outerRadius,0,SkillAreaParam.outerRadius);
                break;
            case SkillAreaType.OuterCircle_InnerCircle:
                Trigers[0].gameObject.SetActive(true);
                Trigers[0].localScale = new Vector3(SkillAreaParam.innerRadius, 0, SkillAreaParam.innerRadius);
                Trigers[0].position=SkillAreaParam.deltaVec+transform.position;
                break;
            case SkillAreaType.OuterCircle_InnerSector:
                Trigers[0].gameObject.SetActive(true);
                Trigers[0].position = transform.position;
                Trigers[0].localScale=new Vector3(SkillAreaParam.outerRadius,0,SkillAreaParam.outerRadius);
                break;
        }
    }
  
    //在列表的中的敌人触发效果，列表清空，触发器设置为False
    void SkillCheckTrigger()
    {
        foreach (GameObject enemy in EnemysInSkill)
        {
            Debug.Log("技能击中了"+enemy.name);
        }
        Trigers[0].gameObject.SetActive(false);
        Trigers[1].gameObject.SetActive(false);
        EnemysInSkill.Clear();
    }
    void OnTriggerEnter(Collider col)
    {
        //进入碰撞器的为敌人则加入列表
        if (col.tag == "Enemy")
        {
            EnemysInSkill.Add(col.gameObject);
        }

    }    
    void OnTriggerStay(Collider col)
    {
        //判断停留在碰撞器中的敌人是否符合检测条件
        Vector3 enemyPos = col.gameObject.transform.position;
        switch (SkillType)
        {
            case SkillAreaType.OuterCircle:
                if (Vector3.Distance(enemyPos, Trigers[0].position) > outerCircleRadius)
                {
                    EnemysInSkill.Remove(col.gameObject);
                }
                break;
            case SkillAreaType.OuterCircle_InnerCircle:
                if (Vector3.Distance(enemyPos, Trigers[0].position) > outerCircleRadius)
                {
                    EnemysInSkill.Remove(col.gameObject);
                }
                break;
            case SkillAreaType.OuterCircle_InnerSector:
                if (Vector3.Distance(enemyPos, Trigers[0].position) > outerCircleRadius || Vector3.Angle(SkillAreaParam.deltaVec, enemyPos - Trigers[0].position) > SkillAreaParam.angle * 0.5)
                {
                    EnemysInSkill.Remove(col.gameObject);
                }
                else if (!EnemysInSkill.Contains(col.gameObject) && col.tag == "Enemy")
                {
                    EnemysInSkill.Add(col.gameObject);
                }
                break;
        }
    }
    void OnTriggerExit(Collider col)
    {
        //退出碰撞器的移出列表
        EnemysInSkill.Remove(col.gameObject);
    }
 
}
