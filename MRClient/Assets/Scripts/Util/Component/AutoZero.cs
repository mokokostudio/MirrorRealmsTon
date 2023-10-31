using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoZero : MonoBehaviour {
    private void Start() {
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one / transform.lossyScale.x;
    }
}
