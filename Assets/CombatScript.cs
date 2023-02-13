using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    PlayerController _controller;
    private void Awake() {
        _controller = GetComponent<PlayerController>();
    }
}
