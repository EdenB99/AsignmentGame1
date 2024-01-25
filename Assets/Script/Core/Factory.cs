using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum PoolObjectType
{
    FlyingEye,
}

public class Factory : Singleton<Factory>
{
    FlyingEyePool flyingEye;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        flyingEye = GetComponentInChildren<FlyingEyePool>();
        if (flyingEye != null)
        {
            flyingEye.Initialize();
        }

    }
    
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.FlyingEye:
                result = flyingEye.GetObject(position, euler).gameObject;
                break;
        }
        return result;
    }
    public FlyingEye GetFlyingEye()
    {
        return flyingEye.GetObject();
    }
    public FlyingEye GetFlyingEye(Vector3 position, float angle = 0.0f)
    {
        return flyingEye.GetObject(position, angle * Vector3.forward);
    }
}
