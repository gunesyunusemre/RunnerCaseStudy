using System;
using DefaultNamespace;
using Stacks;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameInstaller : MonoBehaviour
{
    public static GameInstaller instance;
    [SerializeField] private GameContainer container;

    private void Awake()
    {
        instance = this;
    }

    public StackContainer GetStackContainer() => Instantiate(container.StackContainer);
}