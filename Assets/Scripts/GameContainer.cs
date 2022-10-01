using System;
using System.Collections.Generic;
using Managers;
using Stacks;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Container", menuName = "Game/Container", order = 0)]
public class GameContainer : ScriptableObject
{
    [SerializeField] private StackContainer stackContainer;
    [SerializeField] private ManagersContainer managersContainer;



    public StackContainer StackContainer => stackContainer;
    public ManagersContainer ManagersContainer => managersContainer;
}