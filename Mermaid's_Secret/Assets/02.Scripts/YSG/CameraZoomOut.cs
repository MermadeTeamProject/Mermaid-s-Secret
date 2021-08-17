using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject zoomInCollider;
    bool canZoom = false;

    private void Update()
    {
        if (canZoom)
        {
            zoomOut();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canZoom = true;
        }
    }

    void zoomOut()
    {
        mainCamera.transform.localPosition = new Vector3(mainCamera.transform.localPosition.x,
                                                         mainCamera.transform.localPosition.y,
                                                         Mathf.Lerp(mainCamera.transform.localPosition.z, -4.77f, 0.05f));
        if (mainCamera.transform.localPosition.z <= -4.76f)
        {
            gameObject.SetActive(false);
            zoomInCollider.SetActive(true);
            canZoom = false;
        }
    }

}
