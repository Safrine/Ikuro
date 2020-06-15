using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAnimation : MonoBehaviour {

    #region Attribut
    private Animator animator;
    #endregion

    #region Awake / Update
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        //marche devant lui
        animator.SetFloat("Vertical", 1);
    }
    #endregion
}
