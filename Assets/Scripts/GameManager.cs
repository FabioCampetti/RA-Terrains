using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    public event Action OnMainMenu;
    public event Action OnTerrainMenu;
    public event Action OnTerrain;

    public static GameManager instance;

    private void Awake() {
        if (instance != null && instance != this){

            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start() {
        MainMenu();
    }

    public void MainMenu(){
        OnMainMenu?.Invoke();
        Debug.Log("Main Menu Start");
    }

    public void TerrainMenu(){
        OnTerrainMenu?.Invoke();
        Debug.Log("Terrain Menu Start");
    }

    public void Terrain() {
        OnTerrain?.Invoke();
         Debug.Log("Terrain Start");
    }

    public void CloseApp() {
        Application.Quit();
    }
}
