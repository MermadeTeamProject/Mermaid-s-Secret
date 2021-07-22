using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceSpotLightScr : MonoBehaviour
{
    

    public void Start()
    {
       StartCoroutine(flashNow());
    }

    public void Update()
    {
       
    }

    public IEnumerator flashNow()
    {
       

      

        while (true)
        {
            for ( int i=0; i<=10;i++)
            {
                if (i % 2 == 0)
                {
                    this.GetComponent<Light>().enabled = false;
                }
                else
                {
                    this.GetComponent<Light>().enabled = true;
                }
                yield return new WaitForSeconds(0.1f);
            }
            

        }

    }
}