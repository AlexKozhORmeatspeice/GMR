using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Pistolet : MonoBehaviour
{
    [SerializeField] private AudioSource audioS;
    [SerializeField] private float maxDist = 100.0f;
    [SerializeField] private GameObject bulletPref;
    
    [SerializeField] private float shootRate = 0.3f;
    private float lastShootTime;

    private LineRenderer _lineRenderer;

    private List<Vector3> points;

    private Hero player;
    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        lastShootTime = Time.time;
        points = new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero};
        
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 4;

    }

    // Update is called once per frame
    void Update()
    {
        if(player.menuIsOpen)
            return;
        ShootRaycast();
        
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - lastShootTime > shootRate)
            {
                audioS.Play();
                Bullet bullet = Instantiate(bulletPref).GetComponent<Bullet>();
                bullet.transform.position = transform.position;
                bullet.SetMovePoints(new List<Vector3>(){points[0], points[1], points[2]});

                lastShootTime = Time.time;
            }
        }
    }

    private void ShootRaycast()
    {
        RaycastHit hit;
        _lineRenderer.SetPosition(0, transform.position);
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.transform.GetComponent<Enemy>())
            {
                hit.transform.GetComponent<Enemy>().ActivateMarker();
            }

            if (hit.transform.GetComponent<ElecrticActiveObject>())
            {
                hit.transform.GetComponent<ElecrticActiveObject>().ActivateMarker();
            }
            float nowDist = maxDist;
            
            if (hit.transform.gameObject.tag ==  "Metal")
            {
                nowDist -= Vector3.Distance(transform.position, hit.point);

                Vector3 bounceDir = Vector3.Reflect(transform.forward, hit.normal).normalized;
                
                points[0] = hit.point;
                points[1] = hit.point + bounceDir*nowDist;
                
                _lineRenderer.SetPosition(1, points[0]);
                _lineRenderer.SetPosition(2, points[1]);
                
                RaycastHit hit2;
                if (Physics.Raycast(hit.point, bounceDir, out hit2, Mathf.Infinity) && nowDist > 0.0f)
                {
                    if (hit2.transform.GetComponent<Enemy>())
                    {
                        hit2.transform.GetComponent<Enemy>().ActivateMarker();
                    }

                    if (hit2.transform.GetComponent<ElecrticActiveObject>())
                    {
                        hit2.transform.GetComponent<ElecrticActiveObject>().ActivateMarker();
                    }
                    if (hit2.transform.gameObject.tag == "Metal" )
                    {
                        nowDist = Mathf.Clamp(nowDist - Vector3.Distance(hit.point, hit2.point), 0.0f, maxDist);

                        Vector3 bounceDir2 = Vector3.Reflect(bounceDir, hit2.normal).normalized;
                        
                        points[1] = hit.point + bounceDir*Vector3.Distance(hit.point, hit2.point);
                        points[2] = hit2.point + bounceDir2 * nowDist;
                        
                        _lineRenderer.SetPosition(2, points[1]);
                        _lineRenderer.SetPosition(3, points[2]);
                        
                        RaycastHit hit3;
                        if (Physics.Raycast(hit2.point, bounceDir2, out hit3, Mathf.Infinity) )
                        {
                            if (hit3.transform.GetComponent<Enemy>())
                            {
                                hit3.transform.GetComponent<Enemy>().ActivateMarker();
                            }

                            if (hit3.transform.GetComponent<ElecrticActiveObject>())
                            {
                                hit3.transform.GetComponent<ElecrticActiveObject>().ActivateMarker();
                            }
                        }
                    }
                    else
                    {
                        _lineRenderer.SetPosition(3, points[1]);
                        points[2] = points[1];
                    }

                    
                }
                else
                {
                    _lineRenderer.SetPosition(3, points[1]);
                    points[2] = points[1];
                }

                
                
                
            }
            else
            {
                _lineRenderer.SetPosition(1, transform.position);
                _lineRenderer.SetPosition(2, transform.position);
                _lineRenderer.SetPosition(3, transform.position);
                
                points[0] = transform.position + transform.forward * maxDist;
                points[1] = points[0];
                points[2] = points[1];
            }
        }
        else
        {
            _lineRenderer.SetPosition(1, transform.position);
            _lineRenderer.SetPosition(2, transform.position);
            _lineRenderer.SetPosition(3, transform.position);
                
            points[0] = transform.position + transform.forward * maxDist;
            points[1] = points[0];
            points[2] = points[1];
        }
        
    }
}
