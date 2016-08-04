using UnityEngine;
using System.Collections;

public class CameraRigSpawner : MonoBehaviour {
    public NewtonVR.NVRPlayer cameraRigPrefab;

	// Use this for initialization
	void Awake () {
        var player = GameObject.FindObjectOfType<NewtonVR.NVRPlayer>();
        if (player == null)
        {
            player = Instantiate(cameraRigPrefab);
        }
        GameObject playerSpawnPoint = GameObject.Find("playerSpawnPoint");
        Vector3 playerSpawnLocation = Vector3.zero;
        if (playerSpawnPoint != null) playerSpawnLocation = playerSpawnPoint.transform.position;
        player.transform.position = playerSpawnLocation;
        DontDestroyOnLoad(player.gameObject);
        Destroy(this);
	}
	
}
