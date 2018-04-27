using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour {

    public Transform player;

    private void Update()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.rotation = Quaternion.Euler(90, player.eulerAngles.y, 0);

    }
}
