using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDataModel {

    private static int _numberFirePU;
    private static int _points;
    private static float _glideValue;
    private static bool _playMode;

    //Create dictoinary to store all gameobjects in scene with collider and DestructableObject script
    private static Dictionary<Collider, IDestructable> desObjDictionary = new Dictionary<Collider, IDestructable>();


    public static int NumberFirePU
    {
        get {return _numberFirePU;}
        set {_numberFirePU = value;}
    }

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

    public static Dictionary<Collider, IDestructable> DesObjDictionary
    {
        get
        {
            return desObjDictionary;
        }
    }
}
