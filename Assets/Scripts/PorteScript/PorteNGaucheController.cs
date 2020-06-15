using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorteNGaucheController : MonoBehaviour {

    #region Attributs
    //Porte
    private Rigidbody porteRigidbody;
    private Transform porteTransform;

    //Gestion collision
    private bool collisionPlayer = false;
    private bool alreadyCollision = false;

    //Deplacement
    private float finalPos = -13.35f;
    #endregion


    #region Start / Update / OnCollisionEnter
    void Start () {
        porteRigidbody = GetComponent<Rigidbody>();
        porteTransform = porteRigidbody.transform;
    }
	
	
	void Update () {
        //Deplacer la porte
        if (collisionPlayer)
        {
            if (finalPos < porteTransform.localPosition.x)
            {
                porteTransform.localPosition = new Vector3(porteTransform.localPosition.x - 1, porteTransform.localPosition.y, porteTransform.localPosition.z);
            }
            else
            {
                collisionPlayer = false;
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Check si porte deja ouverte
            if (alreadyCollision == false)
            {
                collisionPlayer = true;
                alreadyCollision = true;
            }
            
        }
    }
    #endregion
}
