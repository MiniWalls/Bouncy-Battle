using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class placeWall : MonoBehaviour
{
    public string Team; // Team
    public GameObject holoPrefabDog; // Prefab
    public GameObject holoPrefabAlien; // Prefab
    public float Fade; // Duration of disappear
    public float RotationSpeed; // Speed of rotation
    public float MaxY; // Depth position
    public LayerMask mask; // Layermask
    public float Cooldown; // Cooldown between walls
    private float CD;
    public Camera MyCam; // Player's own camera

    private GameObject holoPrefab; // Prefab
    private bool Press; // Boolean for holograph
    public GameObject holo; // Holograph object
    private Vector3 position; // Holograph's position
    private Quaternion rotation; // Holograph's rotation
    private Vector3 oldPostition; // Old mouse position

    void Start()
    {
        MyCam = GetComponentInChildren<Camera>();
        MyCam.enabled = true;
        Press = false;
        if (Team == "Dog")
        {
            holoPrefab = holoPrefabDog;
        }
        else
        {
            holoPrefab = holoPrefabAlien;
        }
        /*foreach (var item in Camera.allCameras)
        {
            if (MyCam != item)
            {
                item.gameObject.SetActive(false);
            }
        }*/
        
    }

    void Update()
    {
        if(Cooldown > CD)
        {
            CD += Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                if (Press == false) // Holo and position
                {
                
                    RaycastHit hit;
                    if (Physics.Raycast(MyCam.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask))
                    {
                        Press = true;
                        position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        oldPostition = position;
                        rotation = Quaternion.identity;
                        holo = Instantiate(holoPrefab, position, rotation);
                    }
                }
                else // Rotation
                {
                    RaycastHit hit;
                    Physics.Raycast(MyCam.ScreenPointToRay(Input.mousePosition), out hit, 1000, mask);
                    rotation = Quaternion.Euler(0, rotation.eulerAngles.y + (hit.point.z - oldPostition.z) * RotationSpeed * Time.deltaTime, 0);
                    holo.transform.rotation = rotation;
                    oldPostition = hit.point;
                }
            }
            if (Input.GetMouseButtonUp(1) && Press == true) // Placement
            {
                CD = 0;
                Press = false;
                Destroy(holo);
                //StartCoroutine(fade(Instantiate(Prefab, position, rotation)));
                print("Wall " + Team);
                StartCoroutine(fade(PhotonNetwork.Instantiate("Wall " + Team, position, rotation))); // RESOURCE folder string = gameobject
            }
        }
        
    }

    IEnumerator fade(GameObject obj) // Make objects disappear
    {
        yield return new WaitForSeconds(Fade);
        PhotonNetwork.Destroy(obj);
        Destroy(obj);
    }
}
