using UnityEngine;
using UnityEngine.UI;
using SFB;

public class FileLoader : MonoBehaviour {

    public Button LoadTerrainButton;

    void Start() {

        LoadTerrainButton = transform.Find("LoadTerrain").GetComponent<Button>();
        LoadTerrainButton.onClick.AddListener(LoadFile);
    }

    public void LoadFile() {
    
        string initialDirectory = Application.dataPath + $"/TerrainObjects";
        var paths = StandaloneFileBrowser.OpenFilePanel("Seleccionar archivo", initialDirectory, "obj", false);

        // Verifica si el usuario seleccionó un archivo
        if (paths.Length > 0)
        {
            // Obtiene la ruta del archivo seleccionado
            string filePath = paths[0];

            // Muestra la ruta del archivo en un Text (o haz lo que desees con la ruta)

            // Implementa la lógica de carga del archivo aquí
            // Puedes utilizar la ruta del archivo para cargar su contenido en tu escena
            new TerrainController().LoadAndAddObject(filePath);
        }
    }
}
