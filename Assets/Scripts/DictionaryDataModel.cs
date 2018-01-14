using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryDataModel {

    // Destructable dictionary data model, where by detructabke objects can be added and removed usiing therir collider as a key. 
    private Dictionary<Collider, IDestructable> desObjDictionary;


    public void DesObjDiction()
    {
         desObjDictionary = new Dictionary<Collider, IDestructable>();
    }


    public void AddElement(Collider _key, IDestructable _value)
    {
        desObjDictionary.Add(_key, _value);
    }

    public void RemoveEllement(Collider _key)
    {
        desObjDictionary.Remove(_key);
    }

    public IDestructable ReturnElementByKey(Collider _key)
    {
        IDestructable returnVal = null;

       if(desObjDictionary.ContainsKey(_key))
            returnVal =  desObjDictionary[_key];

        return returnVal;
    }

}
