using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

//Enum for pick-ups (alows for addition at later date)
public enum pickupTypes {
    Fire,
}

public class _GameManager : MonoBehaviour {

    public  Slider glideBar;
    public GameObject scoreMenu;
    public static GameObject staticScoreMenu;

    [SerializeField]
    private GameObject _player;
    public  Text pointText;

    private static float _glideValue;
    private static bool _endTimer;
    private static float _endTurnTimer;
    private static int _noFire;
    private static int _points;
    [SerializeField]
    Camera mainCam;

    Vector3[] cameraPathBounds = {
        Vector3.negativeInfinity,
        Vector3.negativeInfinity,
        Vector3.positiveInfinity,
        Vector3.positiveInfinity
    };
    Vector3 startingPosition = Vector3.positiveInfinity;
    int nextIndex = 0;


    //Create dictoinary to store all gameobjects in scene with collider and DestructableObject script
    public static Dictionary<Collider, IDestructable> desObjDictionary = new Dictionary<Collider, IDestructable>();
	
	void Start () {
        staticScoreMenu = scoreMenu;

        startingPosition = mainCam.transform.position;

        _points = 0;

        Time.timeScale = 1.0f;

        scoreMenu.SetActive(false);
        _endTurnTimer = 7;
        _glideValue = 40;
        

        //Loop through all gameobjects in scene and add to desObjDictionary of it has a collider and 
        // Destructable GameObject Script 
        foreach (GameObject gameObj in Object.FindObjectsOfType<GameObject>())     
        {
            if (gameObj.tag == "IgnorePos")
                continue;

            Collider col = gameObj.GetComponent<Collider>();
            DestructableObject destScript = gameObj.GetComponent<DestructableObject>();

            if (col != null && destScript != null)
                desObjDictionary.Add(col, destScript);

            CheckFurthest(gameObj);
        }

        nextIndex = 0;
        for (int i = 1; i < cameraPathBounds.Length; i++)
        {
            float CurrentShortestToPlayer = Vector3.Distance(cameraPathBounds[nextIndex], _player.transform.position);
            float LoopPositionToPlayer = Vector3.Distance(cameraPathBounds[i], _player.transform.position);
            if (LoopPositionToPlayer < CurrentShortestToPlayer)
            {
                nextIndex = i;
            }
        }


        foreach (Vector3 position in cameraPathBounds)
        {
            Debug.Log(position);
        }



        StartCoroutine(cameraSlerp());

    }
    
    IEnumerator cameraSlerp()
    {        
        Vector3 previousPos = startingPosition;
        for (int dontUseMe = 0; dontUseMe < cameraPathBounds.Length; dontUseMe++)
        {            
            for (float i = 0.0f; i < 1.0f; i += Time.deltaTime)
            {
                mainCam.transform.LookAt(new Vector3(cameraPathBounds[0].x - cameraPathBounds[2].x, 0, cameraPathBounds[1].z - cameraPathBounds[3].z));
                
                mainCam.transform.position = Vector3.Slerp(previousPos, cameraPathBounds[nextIndex], i);
                yield return new WaitForSeconds(0.05f);
            }
            previousPos = cameraPathBounds[nextIndex];

            nextIndex = (nextIndex == 3) ? 0 : nextIndex + 1;
        }
    }

    void CheckFurthest(GameObject gameObj)
    {
        if (gameObj.transform.position.x > cameraPathBounds[0].x)
            cameraPathBounds[0] = gameObj.transform.position + new Vector3(2f, 5f, 0f);
        if (gameObj.transform.position.x < cameraPathBounds[2].x)
            cameraPathBounds[2] = gameObj.transform.position + new Vector3(-2f, 5f, 0f);
        if (gameObj.transform.position.z > cameraPathBounds[1].z)
            cameraPathBounds[1] = gameObj.transform.position + new Vector3(0f, 5f, 2f);
        if (gameObj.transform.position.z < cameraPathBounds[3].z)
            cameraPathBounds[3] = gameObj.transform.position + new Vector3(0f, 5f, -2f);
    }

    public void EndInvoker()
    {
        InvokeRepeating("CheckForEnd", 3.0f, 1.0f);
    }

	void Update () {
        glideBar.value = _glideValue;
        pointText.text = _points.ToString();
    }
   
    public void CheckForEnd()
    {
        foreach (KeyValuePair<Collider,IDestructable> pair in desObjDictionary)
                if (pair.Value.isSettling())
                    return;
        EndTurn();
    }

    //Add points
    public static void AddPoints(int pointsAdded)
    {
        _points += pointsAdded;
    }

    //Update glide value to that used
    public static void UpdateGlide(float glide)
    {
        _glideValue = glide;
    }

    //Add/Remove pick-ups based on enuma and bool input
    public static void QuantPickUp(pickupTypes type, bool isAdding)
    {
        if (isAdding)
            switch (type)
            {
                case pickupTypes.Fire:
                    _noFire += 1;
                    break;
            }
        else
            switch (type)
            {
                case pickupTypes.Fire:
                    if (_noFire > 0)
                        _noFire -= 1;
                    break;
            }
    }

    //Return number of pick-ups
    public static int GetPickUp(pickupTypes type)
    {
        if (type == pickupTypes.Fire)
            return _noFire;
        else
            return -1;
    }

    //Get points 
    public static int GetPoints()
    {
        return _points;
    }

    //End turn
    public static void EndTurn()
    {
        Debug.Log("End turn called");
        Time.timeScale = 0.0f;
        staticScoreMenu.SetActive(true);
    }

    //Reset Level
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
