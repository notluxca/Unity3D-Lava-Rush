using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundRepeater : MonoBehaviour
{
    // public Transform player;
    public float threshold = 10f;
    public GameObject middleTile;
    public Transform player;

    public List<GameObject> children = new List<GameObject>();
    int currentIndex = 0;
    int currentTilePosition = 3;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        foreach (Transform child in transform)
        {
            // children.Add(child.gameObject); // adiciona as 3 childs
        }
        middleTile = GetMiddleTilePosition(children[0], children[1], children[2]);
        
    }

    private void Update()
    {
        PlayerPositionDetection();
    }

    

    public GameObject GetMiddleTilePosition(GameObject first, GameObject second, GameObject third)
    {
        var list = new List<GameObject> { first, second, third };
        list.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        return list[1];
    }

    public void PlayerPositionDetection(){
        Debug.Log($"{transform.position.x}");
        if(player.position.z > middleTile.transform.position.x - middleTile.transform.parent.position.x){
            TeleportChild(); 
        }
    }
    
    
    public void TeleportChild(){
        children[currentIndex].transform.localPosition = new Vector3(currentTilePosition, 0, 0);
            currentIndex++;
            currentTilePosition++;
            if(currentIndex > 2){
                currentIndex = 0;
            }
            middleTile = GetMiddleTilePosition(children[0], children[1], children[2]);
    }

    // detect who is the midle plataform
    // once player is geting close to the midle plataform, teleport the first plataform to the last plataform

    //observar posição x de cada um
    // através da posição x determinar ordem retornar tile do meio 
    // 
    
}
