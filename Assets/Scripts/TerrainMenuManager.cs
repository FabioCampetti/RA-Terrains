using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TerrainMenuManager : MonoBehaviour {

    public TMP_InputField latitudInputField;
    public TMP_InputField longitudInputField;

    public TMP_InputField terrainSizeInputField;

    public TMP_InputField fileNameInputField;
    public TMP_Dropdown dropdown;

    public Button generateTerrainButton;

    public TerrainElevationGeneration terrainElevationGeneration;

    // Start is called before the first frame update
    void Start() {


        latitudInputField = transform.Find("Latitude").GetComponent<TMP_InputField>();
        longitudInputField = transform.Find("Longitude").GetComponent<TMP_InputField>();
        terrainSizeInputField = transform.Find("TerrainSize").GetComponent<TMP_InputField>();
        fileNameInputField = transform.Find("File").GetComponent<TMP_InputField>();
        dropdown = transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        generateTerrainButton = transform.Find("ADD").GetComponent<Button>();
        generateTerrainButton.onClick.AddListener(OnGenerateTerrainButtonClick);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGenerateTerrainButtonClick() {

    double latitud = double.Parse(latitudInputField.text);
    double longitud = double.Parse(longitudInputField.text);

    int terrainSize = int.Parse(terrainSizeInputField.text);

    Debug.Log(dropdown.options[dropdown.value].text);
    int heightmapResolution = int.Parse(dropdown.options[dropdown.value].text);

    string fileName = fileNameInputField.text;


    TerrainElevationGeneration terrainManager = new TerrainElevationGeneration(new Location(latitud, longitud), terrainSize, heightmapResolution, fileName);

    terrainManager.generateElevations();
    terrainManager.generateTerrain();
    terrainManager.ExportTerrain();

    TerrainController terrainController = new TerrainController();
    terrainController.LoadAndAddObject(terrainManager.fileName);
}
}
