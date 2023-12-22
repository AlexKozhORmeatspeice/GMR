using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecrticActiveObject : MonoBehaviour
{
    [SerializeField] private GameObject activateObject;
    [SerializeField] private GameObject marker;
    [SerializeField] private float timerOfActive = 3.0f;

    private void Start()
    {
        StartCoroutine(DeactivateMarker1s());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
            activateObject.SetActive(true);
            StartCoroutine(DeactivateObect());
        }
    }

    IEnumerator DeactivateObect()
    {
        yield return new WaitForSeconds(timerOfActive);
        activateObject.SetActive(false);
    }

    public void ActivateMarker()
    {
        marker.SetActive(true);
    }

    IEnumerator DeactivateMarker1s()
    {
        yield return new WaitForSeconds(0.5f);
        marker.SetActive(false);
        StartCoroutine(DeactivateMarker1s());
    }

}
