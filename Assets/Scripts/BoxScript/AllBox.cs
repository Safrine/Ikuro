using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllBox : MonoBehaviour {

    #region Attributs
    [SerializeField]
    private GameObject[] boxes;

    private System.Random random = new System.Random();
    #endregion

    #region Start
    void Start () {
        //Selection du box qui va posseder la lingot d'or 
        int index = random.Next(boxes.Length);
        for(int i = 0; i < boxes.Length; i++)
        {
            Box box = boxes[i].GetComponent<Box>();
            if (i == index)
            {
                box.SetObjet(Resources.Load("Prefab/or") as GameObject);
            }
            else
            {
                box.SetObjet(Resources.Load("Prefab/sac") as GameObject);
            }
        }
	}
    #endregion
}
