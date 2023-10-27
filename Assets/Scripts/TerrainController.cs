using Dummiesman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainController : MonoBehaviour {

    [SerializeField] public GameObject tObject;
    // Start is called before the first frame update
    void Start() {
    }

    public void LoadAndAddObject(string fileName) {


        tObject = GameObject.Find("TerrainObject");
        // Load the mesh from the .obj file
        string objFilePath = Application.dataPath + $"/TerrainObjects/{fileName}.obj";
        Debug.Log(objFilePath);
        GameObject imported = new OBJLoader().Load(objFilePath,null);

        if (imported != null)
        {
            //Instantiate(imported);

            // Puedes ajustar la posición, rotación y escala según tus necesidades
            //newInstance.transform.position = Vector3.zero;
            //newInstance.transform.rotation = Quaternion.identity;
            //newInstance.transform.localScale = Vector3.one;

            // Puedes establecer cualquier otro ajuste o lógica adicional aquí

            // Opcional: Configurar el nuevo objeto como hijo de algún otro objeto en la escena
             // Establecer como hijo del objeto actual (este script)
            imported.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
            // Opcional: Cambiar el nombre del nuevo objeto
            //newInstance.name = "NuevoObjetoEnEscena";
            imported.transform.position = new Vector3(201, -172, -105);
            
            tObject.transform.position = new Vector3(0, -24, 81);
            imported.transform.parent = tObject.transform;

            tObject.transform.position = new Vector3(263, 148, 0);
        }
        else
        {
            Debug.LogError("Error loading the .obj file");
        }
    }
}
