using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject zoomOutCollider;
    bool canZoom = false;

    private void Update()
    {
        if (canZoom)
        {
            zoomIn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canZoom = true;
        }
    }

    void zoomIn()
    {
        mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x,
                                                         mainCamera.transform.localPosition.y,
                                                         Mathf.Lerp(mainCamera.transform.localPosition.z, -1.77f, 0.05f));
        if (mainCamera.transform.localPosition.z >= -1.78f)
        {
            gameObject.SetActive(false);
            zoomOutCollider.SetActive(true);
            canZoom = false;
        }
    }

}
