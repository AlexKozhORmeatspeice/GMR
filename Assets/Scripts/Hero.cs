using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Hero : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    [SerializeField] private List<AudioClip> audios;
    private AudioSource audioS;
    [SerializeField] private GameObject menu;
    public bool menuIsOpen;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float soundWalkTimer = 0.5f;
    [SerializeField] private float soundWalkTimerWithShift = 0.3f;
    private float lastStepTime;
    [SerializeField] private float shiftPlusSpeed = 1.0f;
    [SerializeField] private Transform firstCam;
    [SerializeField] public ControlPoint nowControlPoint;


    private float startPitch;
    public Vector3 nowSavePoint; 
    private float startSpeed;
    [SerializeField] private float sensCam = 10.0f;
    private float startSensCam;

    private int hp = 2;
     
    private Rigidbody rb;
    private Vector3 moveDir;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        lastStepTime = Time.time;
        audioS = GetComponent<AudioSource>();
        audioS.clip = audios[0];
        startPitch = audioS.pitch;
        
        startSensCam = sensCam;
        PlayerPrefs.SetInt("Score", 100);
        
        menuIsOpen = false;
        
        startSpeed = speed;
        
        rb = GetComponent<Rigidbody>();
        Cursor.lockState=CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Enemy: {nowControlPoint.nowEnemy.ToString()}";
        sensCam = startSensCam * PlayerPrefs.GetFloat("MouseSense");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menuIsOpen);
            menuIsOpen = !menuIsOpen;
            
        }

        if (menuIsOpen)
        {
            Cursor.lockState=CursorLockMode.None;
        }
        else
        {
            Cursor.lockState=CursorLockMode.Locked;
        }
        Time.timeScale = menuIsOpen ? 0.0f : 1.0f;
        if(menuIsOpen)
            return;
        RotateCam();

        
        
        if (hp == 0)
        {
            transform.position = nowSavePoint;
            PlayerPrefs.SetInt("Score", Mathf.Clamp(PlayerPrefs.GetInt("Score") - 10, 0, 100));   
            nowControlPoint.LoadPoint();
            hp = 2;
        }
        
    }

    private void FixedUpdate()
    {
        if(menuIsOpen)
            return;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = startSpeed + shiftPlusSpeed;
        }
        else
        {
            speed = startSpeed;
        }
        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = firstCam.right * x + firstCam.forward * z;

        // normalize move if has a magnitude > 1 to prevent faster diagonal movement
        if (move.sqrMagnitude > 1)
        {
            move.Normalize();
        }

        if (Time.time - lastStepTime > ((speed == (startSpeed + shiftPlusSpeed)) ? soundWalkTimerWithShift : soundWalkTimer) && (Mathf.Abs(x - 0) > 0.05 || Mathf.Abs(z - 0) > 0.05))
        {
            audioS.pitch = startPitch + Random.Range(-0.5f, 0.5f);
            
            audioS.Play();
            lastStepTime = Time.time;
        }
        else
        {
            audioS.pitch = startPitch;
        }

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
        
    }

    void RotateCam()
    {
        float X = Input.GetAxis("Mouse X") * sensCam * Time.deltaTime;
        float Y = -Input.GetAxis("Mouse Y") * sensCam * Time.deltaTime;

        float eulerX = (transform.rotation.eulerAngles.x + Y) % 180;
        float eulerY = (transform.rotation.eulerAngles.y + X) % 180;
        
        firstCam.rotation = Quaternion.Euler(firstCam.eulerAngles.x + eulerX,firstCam.eulerAngles.y + eulerY, 0);
    }

    public void GetDamage(Vector3 damageDir)
    {
        hp -= 1;
        rb.AddForce(damageDir * 10.0f, ForceMode.Impulse);
    }
}
