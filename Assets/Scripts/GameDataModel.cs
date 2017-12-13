using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataModel {

    //Enum for pick-ups (alows for addition at later date)
    public enum pickupTypes
    {
        Fire,
    }


    private static int _numberFirePU;
    private static int _points;
    private static float _glideValue;
    private static bool _playMode;

    //Create dictoinary to store all gameobjects in scene with collider and DestructableObject script
    private static Dictionary<Collider, IDestructable> desObjDictionary = new Dictionary<Collider, IDestructable>();


    public static int Points
    {
        get{ return _points; }
        set{_points = value;}
    }

    public static float GlideValue
    {
        get { return _glideValue; }
        set { _glideValue = value; }
    }

   public static bool PlayMode
    {
        get {return _playMode;}
        set { _playMode = value;}
    }

    //Add/Remove pick-ups based on enuma and bool input
    public static void QuantPickUp(pickupTypes type, bool isAdding)
    {
        if (isAdding)
            switch (type)
            {
                case pickupTypes.Fire:
                    _numberFirePU += 1;
                    break;
            }
        else
            switch (type)
            {
                case pickupTypes.Fire:
                    if (_numberFirePU > 0)
                        _numberFirePU -= 1;
                    break;
            }
    }

    //Return number of pick-ups
    public static int GetPickUp(pickupTypes type)
    {
        if (type == pickupTypes.Fire)
            return _numberFirePU;

        else
            return -1;
    }

    public static void SetPickUpVal(pickupTypes type, int val)
    {
        if (type == pickupTypes.Fire)
            _numberFirePU = val;
        else
            return;
    }

    public static Dictionary<Collider, IDestructable> DesObjDictionary
    {
        get
        {
            return desObjDictionary;
        }
    }
}
