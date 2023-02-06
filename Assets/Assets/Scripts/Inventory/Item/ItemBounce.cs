using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
   public class ItemBounce : MonoBehaviour
   {
       private Transform sprteTransform;
       private BoxCollider2D _collider2D;
       public float gravity = 2;
       private bool isGrond;
       private float distance;
       private Vector2 direction;
       private Vector3 targetpos;
       private Transform shadow => transform.GetChild(1);
       private void Awake()
       {
           sprteTransform = transform.GetChild(0);
           _collider2D = GetComponent<BoxCollider2D>();
           _collider2D.enabled = false;
       }

      

       public void InitBounceItem(Vector3 taget, Vector2 dir)
       {
           _collider2D.enabled = false;
           direction = dir;
           targetpos = taget;

           distance = Vector3.Distance(taget, transform.position);
           sprteTransform.position+=Vector3.up*1.95f;
           
       }

       private void Update()
       {
           Bounced();
       }

       private void Bounced()
       {
           isGrond = sprteTransform.position.y <= transform.position.y;

           if (Vector3.Distance(transform.position, targetpos) > 0.1f)
           {
               transform.position += (Vector3) direction * distance * gravity * Time.deltaTime;
           }

           if (!isGrond)
           {
               sprteTransform.position += Vector3.up * -gravity *Time.deltaTime;
           }
           else
           {
               sprteTransform.position = transform.position;
               shadow.gameObject.SetActive(false);
               _collider2D.enabled = true;
           }
       }
   }
 
}
