using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteQDroiteController : MonoBehaviour {

    #region Attributs
    //Porte
    private Rigidbody porteRigidbody;
    private Transform porteTransform;

    //Gestion collision
    private bool collisionPlayer = false;
    private bool alreadyCollision = false;
    private bool porteOuverte = false;

    //Deplacement
    private float initPos = 1.57f;
    private float finalPos = -12f;
    private int waitCount = 1;
    #endregion

    #region Start / Update / OnCollisionEnter
    void Start()
    {
        porteRigidbody = GetComponent<Rigidbody>();
        porteTransform = porteRigidbody.transform;
    }

    
    void Update()
    {
        //Ouverture de la porte
        if (collisionPlayer)
        {
            if(finalPos < porteTransform.localPosition.x)
            {
                porteTransform.localPosition = new Vector3(porteTransform.localPosition.x - 1, porteTransform.localPosition.y, porteTransform.localPosition.z);
            }
            else
            {
                collisionPlayer = false;
                porteOuverte = true;
            }

        }

        //Fermeture de la porte
        else if (porteOuverte)
        {
            //on referme au bout de 3 secondes
            if (waitCount == 90)
            {
                if (initPos >= porteTransform.localPosition.x + 1)
                {
                    porteTransform.localPosition = new Vector3(porteTransform.localPosition.x + 1, porteTransform.localPosition.y, porteTransform.localPosition.z);
                }
                else
                {
                    porteOuverte = false;
                    alreadyCollision = false;
                    waitCount = 1;
                }
            }
            else
            {
                waitCount++;
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (alreadyCollision == false)
            {
                collisionPlayer = true;
                alreadyCollision = true;
            }

        }
    }
    #endregion
}
