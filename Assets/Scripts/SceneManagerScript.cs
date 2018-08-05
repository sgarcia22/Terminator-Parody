using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public void RestartGame ()
    {
        PlayerMovement.health = 3;
        Spawner.totalEnemies = 0;
        SceneManager.LoadScene(1);
    }
}
