using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SurveillanceController
{
    #region Attributs
    [SerializeField]
    private float m_CameraSpeed;
    [SerializeField]
    private float m_RotationDuration;

    private float timeCount;
    private int rotationDirection;
    #endregion

    #region Awake / Start / Update
    protected override void Awake()
    {
        base.Awake();
        timeCount = 1;
        rotationDirection = 1;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Rotate left and right
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        
        if (timeCount >= m_RotationDuration)
        {
            rotationDirection = -rotationDirection;
        }
        else if (timeCount <= 0)
        {
            rotationDirection = -rotationDirection;
        }
        m_RigidBody.MoveRotation(m_RigidBody.rotation * Quaternion.AngleAxis(Mathf.Sin(rotationDirection * m_CameraSpeed * Time.fixedDeltaTime), Vector3.up));
        timeCount += rotationDirection;

    }
    #endregion
}
