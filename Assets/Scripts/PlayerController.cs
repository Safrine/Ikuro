using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Attributs

    //Player
    private Rigidbody m_Rigidbody;
    private Transform m_Transform;
    private Animator animator;

    //Deplacement
    [SerializeField]
    private float m_TranslationSpeed;
    [SerializeField]
    private float m_RotationSpeed;

    //Rotation
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
    }

    [SerializeField]
    private MouseInput MouseControl;
    private Vector2 mouseInput;

    private float MouseInputX;

    //Shoot projectile
    [SerializeField]
    private GameObject m_MissilePrefab;
    [SerializeField]
    private float m_MissileSpeed;
    [SerializeField]
    private float m_CoolDownDuration;
    private float m_NextShootTime;

    //Gestion recuperation de la cle
    private bool hasKey = false;

    //Chrono
    private float time = 0.0f;
    public float interpolationPeriod = 0.7f;

    #endregion

    #region Awake / Start / Update / FixedUpdate / OnCollisionEnter
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); 
        m_Transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();

        MouseInputX = 0;
    }
    
    void Start()
    {
        m_NextShootTime = Time.time;
    }

    
    void Update()
    {
        if (GameManager.Instance.IsPlaying)
        {
            // Increment chrono
            time += Time.deltaTime;
            if (time >= interpolationPeriod)
            {
                time = time - interpolationPeriod;
                GameManager.IncrementChrono();
            }
        }
    }

    private void FixedUpdate()
    {
        
        //Pause
        if (Input.GetKey("escape"))
        {
            EventManager.Instance.Raise(new EscapeButtonClickedEvent());
        }

        //Déplacement du joueur
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        //Rotation
        MouseInputX += Input.GetAxis("Mouse X");
        m_Rigidbody.MoveRotation(Quaternion.AngleAxis(MouseInputX * MouseControl.Sensitivity.x * 1f / MouseControl.Damping.x, Vector3.up));

        //Gestion animation
        animator.SetFloat("Vertical", vInput);
        animator.SetFloat("Horizontal", hInput);
        animator.SetBool("Sprint", Input.GetKey("left shift"));
        animator.SetBool("Punch", Input.GetKey(KeyCode.F));

        //Le joueur court via touche Shift
        if (Input.GetKey("left shift"))
        {
            m_TranslationSpeed = 15;
        }
        else
        {
            m_TranslationSpeed = 5;
        }

        //Tire projectile
        bool fire = Input.GetKey(KeyCode.F);// clic gauche
        if (fire && Time.time > m_NextShootTime)
        {
            ShootMissile();
            m_NextShootTime = Time.time + m_CoolDownDuration; //4 tirs par seconde
        }
        
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Transform.forward * m_TranslationSpeed * vInput * Time.fixedDeltaTime);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Transform.right * m_TranslationSpeed * hInput * Time.fixedDeltaTime);
        //m_Rigidbody.MoveRotation(Quaternion.AngleAxis(m_RotationSpeed * Time.fixedDeltaTime * hInput, Vector3.up) * m_Rigidbody.rotation);

    }

    private void OnCollisionEnter(Collision collision)
    {
        //Player repéré par un champ de vision (camera ou guard)
        if (collision.gameObject.CompareTag("FieldOfView"))
        {
            EventManager.Instance.Raise(new PlayerHasBeenDetectedEvent());
        }

        //Player a récupéré la clé
        if (collision.gameObject.CompareTag("Cle"))
        {
            Destroy(collision.gameObject);
            EventManager.Instance.Raise(new PlayerFindKeyEvent());
            hasKey = true;
        }
    }
    #endregion

    #region Mes fonctions
    //Check si le joueur a recupéré la clé
    public bool getHasKey()
    {
        return hasKey;
    }

    //Permet de lancer un projectile
    private void ShootMissile()
    {
        GameObject missileGo = Instantiate(m_MissilePrefab);
        Destroy(missileGo, 2);
        missileGo.transform.position = m_Transform.position + m_Transform.forward + m_Transform.up;
        missileGo.GetComponent<Rigidbody>().velocity = m_Transform.forward * m_MissileSpeed; 
    }
    #endregion
}
