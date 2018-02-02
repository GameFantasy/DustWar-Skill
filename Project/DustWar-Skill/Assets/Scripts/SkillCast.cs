using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCast : MonoBehaviour
{
    public float Velocity=3.0f;
    public float MaxDistance;
    private float curDistance;
	void Start () 
    {
        curDistance = 0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
      transform.Translate(transform.forward* Velocity * Time.deltaTime,Space.World);
      if (curDistance < MaxDistance)
      {
          curDistance += Velocity * Time.deltaTime;
      }
      else
      {
          Destroy(gameObject);
      }
	}
    void OnTriggerEnter(Collider Col)
    {
        if (Col.tag == "Enemy")
        {
            Col.GetComponent<EnemyCtrl>().GetHurt();
            Destroy(gameObject);
        }
    }
}
