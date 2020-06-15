using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllGuard : MonoBehaviour {

    #region Attributs
    [SerializeField]
    private GameObject[] guards;

    private System.Random random = new System.Random();
    #endregion

    #region Start
    void Start () {
        //Selection du guard qui va posseder la cle 
        int index = random.Next(guards.Length);

        for(int i = 0; i < guards.Length; i++)
        {
            GuardController guard = guards[i].GetComponent<GuardController>();
            if (i == index)
            {
                guard.SetGuardHasKey(true);
            }
            else
            {
                guard.SetGuardHasKey(false);
            }
        }
    }
    #endregion
}
