using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    private List<Vector3> movePoints;

    private int nowMovePoint;
    // Start is called before the first frame update
    void Start()
    {
        nowMovePoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, movePoints[nowMovePoint]) < 0.01f)
        {
            if (movePoints.Count - 1 == nowMovePoint)
            {
                StartCoroutine(Deactivate());
            }
            else
            {
                nowMovePoint++;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoints[nowMovePoint], speed * Time.deltaTime);
        
    }

    public void SetMovePoints(List<Vector3> points)
    {
        movePoints = points;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().GetDamage();
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
}
