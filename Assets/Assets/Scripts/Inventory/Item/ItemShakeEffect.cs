using System;
using System.Collections;
using UnityEngine;

namespace Inventory
{
    public class ItemShakeEffect : MonoBehaviour
    {
        private bool isAnimation;
        private WaitForSeconds pause = new WaitForSeconds(0.04f);

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isAnimation)
            {
                if (col.transform.position.x < transform.position.x)
                {
                    //向右摇晃
                    StartCoroutine(RotateRight());
                }
                else
                {
                    //向左摇晃
                    StartCoroutine(RotateLeft());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!isAnimation)
            {
                if (other.transform.position.x > transform.position.x)
                {
                    //向右摇晃
                    StartCoroutine(RotateRight());
                }
                else
                {
                    //向左摇晃
                    StartCoroutine(RotateLeft());
                }
            }
            
        }

        private IEnumerator RotateLeft()
        {
            isAnimation = true;
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).Rotate(0,0,2);
                yield return pause;
            }

            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(0).Rotate(0,0,-2);
                yield return pause;
            }
            transform.GetChild(0).Rotate(0,0,2);
            yield return pause;
            isAnimation = false;
            
        }
        private IEnumerator RotateRight()
        {
            isAnimation = true;
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(0).Rotate(0,0,-2);
                yield return pause;
            }

            for (int i = 0; i < 5; i++)
            {
                transform.GetChild(0).Rotate(0,0,2);
                yield return pause;
            }
            transform.GetChild(0).Rotate(0,0,-2);
            yield return pause;
            isAnimation = false;
            
        }
    }
}