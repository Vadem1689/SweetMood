using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<BaseService> _services;

    private void Start()
    {
        foreach (var service in _services)
            service.Initialize();
    }
}
