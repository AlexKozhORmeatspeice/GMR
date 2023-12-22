using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private GameObject ActivateWall;
    [SerializeField] public GameObject savePoint;
    [SerializeField] private ControlPoint nextControlPoint;
    public int nowEnemy;
    private Hero player;
    private bool isDone;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        isDone = false;
        _enemies = GetComponentsInChildren<Enemy>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDone && _enemies.All(x => !x.gameObject.activeSelf))
        {
            ActivateWall.SetActive(false);
            if(nextControlPoint != null)
                player.nowControlPoint = nextControlPoint;
            isDone = true;
        }

        int i = 0;
        foreach (var enemy in _enemies)
        {
            if (enemy.gameObject.activeSelf)
                i++;
        }

        nowEnemy = i;
    }

    public void LoadPoint()
    {
        foreach (var enemie in _enemies)
        {
            enemie.gameObject.SetActive(true);
        }
        
        ActivateWall.SetActive(true);
    }
}
