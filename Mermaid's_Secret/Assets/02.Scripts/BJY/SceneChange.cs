using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Island_20210721"); //¼¶ ¸Ê ¾À ºÒ·¯¿À±â(»ó°æ¾¾ ¸Ê)
        }
    }
}
