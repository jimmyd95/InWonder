using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveForward : MonoBehaviour
{

    private void Update() {
        // move the object forward slowly
        transform.position += transform.forward * Time.deltaTime * 1f;
    }
}
