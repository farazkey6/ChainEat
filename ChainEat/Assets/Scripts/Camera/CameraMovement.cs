using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject character;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y, character.transform.position.z - 20);
    }
}
