using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 center;
    public Vector3 size;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube(center, size);

        Gizmos.color = new Color(0, 1, 0, 0.8f); 
        Gizmos.DrawSphere(new Vector3(center.x, size.y, center.z), 1); //For testing size of sphere used for checking if respawn point is empty
    }

    public void spawnPlayer() //Randomizes respawn point for player
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), size.y , Random.Range(-size.z / 2, size.z / 2));
        var hitColliders = Physics.OverlapSphere(new Vector3(pos.x, 2.1f, pos.z), 1);
        if (hitColliders.Length > 0) //If sphere collides this is true
        {
            print("getting new spawnpoint");
            newSpawnPoint(); //Calls for new respawn point, because current one is not empty
        }
        else if (hitColliders.Length <= 0) //Make sure sphere doesnt collide
        {
            
            transform.position = pos;
        }
    }

    private void newSpawnPoint()
    {
        spawnPlayer();
    }


}
