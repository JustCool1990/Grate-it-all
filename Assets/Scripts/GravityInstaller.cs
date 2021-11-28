using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInstaller : MonoBehaviour
{
    [SerializeField] private float _gravityForce;

    private void Awake()
    {
        Physics.gravity = new Vector3(0, _gravityForce, 0);
    }
}
