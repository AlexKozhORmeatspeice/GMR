using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audios;
    private LineRenderer _lineRenderer;
    private AudioSource audioS;
    
    [SerializeField] private float shootRate = 0.5f;
    [SerializeField] private GameObject marker;
    private int HP = 3;
    private float lastShootTime;
    
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        
        audioS = GetComponent<AudioSource>();
        
        lastShootTime = Time.time;
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(DeactivateEvery1SMarker());
        
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (HP == 0)
        {
            StartCoroutine(DeathWithDelay());
        }
        Vector3 lookDir = -Vector3.Normalize(player.transform.position - transform.position);

        Vector3 lookPos = player.transform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        
        CheckCanKill();
        
        
    }

    void CheckCanKill()
    {
        RaycastHit hit;
        Vector3 dir = (player.transform.position - transform.position);
        
        if (Physics.Raycast(transform.position, dir, out hit, Mathf.Infinity))
        {
            if (hit.transform.GetComponent<Hero>())
            {
                if (Time.time - lastShootTime > shootRate)
                {
                    audioS.Stop();
                    audioS.clip = audios[0];
                    audioS.Play();
                    
                    _lineRenderer.SetPosition(1, hit.point);
                    
                    hit.transform.GetComponent<Hero>().GetDamage(dir.normalized);
                    lastShootTime = Time.time;

                    StartCoroutine(DeleteDir());
                }
            }
        }
        
        
    }

    private IEnumerator DeleteDir()
    {
        yield return new WaitForSeconds(1.0f);
        
        _lineRenderer.SetPosition(1, transform.position);
    }
    public void GetDamage()
    {
        HP -= 1;
        Debug.Log(1);
    }

    public void ActivateMarker()
    {
        marker.SetActive(true);   
    }
    public void DeactivateMarker()
    {
        marker.SetActive(false); 
    }

    IEnumerator DeactivateEvery1SMarker() //это пиздец
    {
        marker.SetActive(false); 
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DeactivateEvery1SMarker());
    }

    private void OnEnable()
    {
        HP = 3;
        StopCoroutine(DeactivateEvery1SMarker());
        StartCoroutine(DeactivateEvery1SMarker());
    }

    IEnumerator DeathWithDelay()
    {
        audioS.Stop();
        audioS.clip = audios[1];
        audioS.Play();
        
        yield return new WaitForSeconds(2.0f);
        gameObject.SetActive(false);
    }
    
}
