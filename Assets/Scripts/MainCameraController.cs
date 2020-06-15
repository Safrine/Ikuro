using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    #region Attributs
    public GameObject player;

    [SerializeField]
    public int offsetX;
    [SerializeField]
    public int offsetY;
    [SerializeField]
    public int offsetZ;

    private Vector3 offset;
    #endregion

    #region Start
    void Start()
    {
        offset = transform.position - player.transform.position;

        this.transform.parent = player.transform;
        this.transform.position = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);

        this.transform.rotation = player.transform.rotation;
        this.transform.localRotation = Quaternion.Euler((float)22.0, (float)0.0, (float)0.0);

    }
    #endregion


}
