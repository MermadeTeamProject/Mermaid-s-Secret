using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCtrl : MonoBehaviour
{
    public static bool isWater = false;

    [SerializeField] private float waterDrag;//���� �߷�
    private float originDrag;//���ۿ� ���ý� �ٽ� ���� �߷´�� 
    [SerializeField] private Color waterColor;//���� ����
    [SerializeField] private float waterFogDensity;//�� Ź�� ����

    private Color originColor;
    private float originFogDensity;

    private void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }
    void GetWater(Collider _player)
    {
        isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        RenderSettings.fogColor = waterColor;
        RenderSettings.fogDensity = waterFogDensity;
    }
    void GetOutWater(Collider _player)
    {
        if(isWater)
        {
            isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;
            RenderSettings.fogColor = originColor;
            RenderSettings.fogDensity = originFogDensity;
        }
    }
}
