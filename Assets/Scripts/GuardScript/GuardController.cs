using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : SurveillanceController
{
    #region Attributs

    //Rotation
    [SerializeField]
    private int rotationAngle;
    private int r = 1;
    private bool hasToRotate = false;
    
    //pour le faire parcourir d'un point a un autre
    public Transform[] waypoints;
    public float duration = 1.0f;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private int targetWaypoint;
    private float startTime;
    
    //animation marcher
    private Animator animator;

    //gestion clé
    private GameObject key = null;
    private bool isPunch = false;
    private bool guardHasKey = false;
    private bool keyAlreadyGet = false;
    private bool canGetKey = false;
    private int decalage = 1;
    #endregion

    #region Awake / Start / Update / OnCollisionEnter
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }


    protected override void Start()
    {
        base.Start();
        

        startPoint = transform.position;
        startTime = Time.time;

        if (waypoints.Length <= 0)
        {
            enabled = false;
        }
        targetWaypoint = 0;
        endPoint = waypoints[targetWaypoint].position;
        
    }
    
    void Update()
    {
        if (isPunch)
        {
            Invoke("LetsWalk", 8);
        }
        else
        {
            animator.SetFloat("Vertical", 1);
            Mouvement();
        }
        

        //pop la clé
        if (canGetKey)
        {
            if (decalage == 20)
            {
                canGetKey = false;
            }
            else
            {
                if(key != null)
                {
                    key.transform.position = new Vector3(key.transform.position.x + 0.20f, key.transform.position.y, key.transform.position.z);
                }
                decalage++;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Missile"))
        {
            animator.SetBool("isPunch", true);
            isPunch = true;
            fieldOfView.SetActive(false);

            if (!keyAlreadyGet)
            {
                if (guardHasKey)
                {
                    key = Instantiate(Resources.Load("Prefab/Key") as GameObject);
                    key.transform.parent = GameObject.Find("Level " + GameManager.levelIndex).transform;
                    key.transform.position = new Vector3(m_Transform.position.x, m_Transform.position.y, m_Transform.position.z);
                    keyAlreadyGet = true;
                    canGetKey = true;
                }
            }
        }
    }
    #endregion

    #region Mes fonctions
    //Marcher
    public void Mouvement()
    {
        var i = (Time.time - startTime) / duration;
        transform.position = Vector3.Lerp(startPoint, endPoint, i);
        
        if (i >= 1)
        {
            hasToRotate = true;
            GetNewPoint();
        }
    }

    //Changement de waypoint
    void GetNewPoint()
    {
        startTime = Time.time;
        targetWaypoint++;
        targetWaypoint = targetWaypoint % waypoints.Length;

        startPoint = endPoint;
        endPoint = waypoints[targetWaypoint].position;
        
        Quaternion q = Quaternion.AngleAxis(rotationAngle, Vector3.up) * m_RigidBody.rotation;
        m_RigidBody.MoveRotation(q);

    }

    //Marcher a nouveau (apres collision)
    private void LetsWalk()
    {
        isPunch = false;
        animator.SetBool("isPunch", false);
        fieldOfView.SetActive(true);
    }

    public void SetGuardHasKey(bool check)
    {
        guardHasKey = check;
    }
    #endregion



}
