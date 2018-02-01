using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCast : MonoBehaviour
{
    public float Velocity=3.0f;
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
      transform.Translate(transform.forward* Velocity * Time.deltaTime,Space.World);
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
