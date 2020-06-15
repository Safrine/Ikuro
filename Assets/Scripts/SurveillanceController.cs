using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceController : MonoBehaviour
{
    #region Attributs
    //Objet de surveillance
    protected Rigidbody m_RigidBody;
    protected Transform m_Transform;
    
    //Champ de vision
    [SerializeField]
    protected GameObject m_FieldOfView;
    protected GameObject fieldOfView;
    #endregion

    #region Awake / Start
    protected virtual void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Transform = GetComponent<Transform>();
    }
    
    protected virtual void Start()
    {
        initFOV();
    }
    #endregion

    #region Mes fonctions
    //Initialisation du champ de vision
    private void initFOV()
    {
        fieldOfView = Instantiate(m_FieldOfView);
        fieldOfView.transform.parent = m_Transform;
        fieldOfView.transform.position = new Vector3(m_Transform.position.x, (float)-3.4, m_Transform.position.z);

        fieldOfView.transform.rotation = m_RigidBody.rotation;
        fieldOfView.transform.localEulerAngles = new Vector3(0f, -180f, 0f);
    }
    #endregion
    
}
