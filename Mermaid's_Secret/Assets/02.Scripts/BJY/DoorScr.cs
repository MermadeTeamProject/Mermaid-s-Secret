using UnityEngine;

public class DoorScr : MonoBehaviour
{
    //20210723 Door 문 여닫이 처리 X

    Animator A_animator;


    void Start()
    {
        A_animator = GetComponent<Animator>(); //문 애니메이터 불러옴
    }

    private void OnCollisionEnter(Collision collision) //플레이어가 문에 닿이면 문이 열린다
    {
        if (collision.gameObject.tag == "Player")
        {
            A_animator.SetBool("IsOpen", true);
        }
    }
    private void OnTriggerEnter(Collider other) //플레이어가 문에 닿이면 문이 열린다
    {
        if (other.gameObject.tag == "Player")
        {
            A_animator.SetBool("IsOpen", true);
        }
    }

    void OnCollisionExit(Collision collision)//플레이어가 문에서 멀어지면 문이 닫힌다
    {
        if (collision.gameObject.tag == "Player")
        {
            A_animator.SetBool("IsOpen", false);
            //this.GetComponent<Collision>().enabled = true;
        }
    }

    void OnTriggerExit(Collider collision)//플레이어가 문에서 멀어지면 문이 닫힌다
    {
        if (collision.gameObject.tag == "Player")
        {
            A_animator.SetBool("IsOpen", false);
            //this.GetComponent<Collision>().enabled = true;
        }
    }



}









