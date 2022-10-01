using System;
using System.Collections.Generic;
using Managers;
using Managers.Base;
using Stacks;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameContainer container;

    private readonly Dictionary<int, BaseManager> _managerInstanceDict = new Dictionary<int, BaseManager>();
    private readonly List<int> _shoudRemoveManagerInstanceList = new List<int>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        var managerList = container.ManagersContainer.ManagerList;
        foreach (var manager in managerList)
        {
            var managerInstance = Instantiate(manager, transform);
            managerInstance.Init();
            
            var type = managerInstance.GetEvents(out BaseManagerEvents events);
            ManagerEventsHelper.AddManagerEvents(events, type);
            events.OnDisable += ManagerOnDisable;

            var managerID = managerInstance.GetInstanceID();
            _managerInstanceDict.Add(managerID, managerInstance);
        }
    }

    private void Update()
    {
        SyncManagerInstanceList();
        foreach (var baseManager in _managerInstanceDict.Values)
            baseManager.OnUpdate();
    }

    public StackContainer GetStackContainer() => Instantiate(container.StackContainer);
    private void ManagerOnDisable(int managerID) => _shoudRemoveManagerInstanceList.Add(managerID);

    private void SyncManagerInstanceList()
    {
        foreach (var baseManager in _shoudRemoveManagerInstanceList)
            _managerInstanceDict.Remove(baseManager);
        _shoudRemoveManagerInstanceList.Clear();
    }
}