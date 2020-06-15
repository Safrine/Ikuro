using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    #region Attributs
    //Box
    private GameObject objet;
    private Animator animator;
    protected Transform m_Transform;

    //Animation ouverture
    private bool isOpen = false;
    private float decalage = 1;

    //Check du joeur
    private bool hasKey = false;
    #endregion

    #region Awake / Update / OnCollisionEnter
    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_Transform = GetComponent<Transform>();
    }

    private void Update()
    {
        //OUverture du coffre
        if (isOpen)
        {
            if (decalage == 20)
            {
                isOpen = false;
                //Si lingot d'or Alors gagné
                if (objet.CompareTag("Gold"))
                {
                    Destroy(objet, 1);
                    Invoke("EventCall", 2);
                }
            }
            else
            {
                objet.transform.position = new Vector3(objet.transform.position.x, objet.transform.position.y + 0.10f, objet.transform.position.z);
                decalage++;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            hasKey = player.getHasKey();

            //Check s'il a la clé
            if (hasKey)
            {
                animator.SetBool("Open", true);
                isOpen = true;
            }
        }
    }
    #endregion

    #region Mes fonctions
    //Appel a l'evenement FindGold
    private void EventCall()
    {
        EventManager.Instance.Raise(new PlayerFindGoldEvent());
    }
    
    //Set l'objet que le coffre contient
    public void SetObjet(GameObject go)
    {
        Destroy(objet);
        objet = Instantiate(go);
        objet.transform.parent = m_Transform;
        objet.transform.position = new Vector3(m_Transform.position.x, m_Transform.position.y, m_Transform.position.z);
    }
    #endregion
}


