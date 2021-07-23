using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScr : MonoBehaviour
{
    //20210723 Door 문 여닫이 처리 X
   
    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
       
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {           
            animator.SetBool("IsOpen", true);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetBool("IsOpen", false);
        }
    }

}
