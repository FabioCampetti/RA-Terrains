using Dummiesman;
using UnityEngine;
using UnityEngine.UI;

public class TerrainController : MonoBehaviour {

    public GameObject tObject;
    // Start is called before the first frame update
    public GameObject imported;

    public void LoadAndAddObject(string fileName) {


        tObject = GameObject.Find("TerrainObject");
       
        imported = new OBJLoader().Load(fileName,null);
        if (imported != null) {

        imported.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f);
        imported.transform.position = new Vector3(728, 80, -115);;

        // Ajusta la posición del objeto padre a la posición fija
        tObject.transform.position = new Vector3(516, 266.5f, 67);

        // Establece el objeto hijo como hijo del objeto padre
        imported.transform.parent = tObject.transform;
    }
    else
    {
        Debug.LogError("Error loading the .obj file");
    }
    }

    Vector3 CalculateCenterOfObject(GameObject obj) {
        // Calcula el centro del objeto tomando el promedio de sus extremos
        Bounds bounds = obj.GetComponent<Renderer>().bounds;
        return bounds.center;
    }

    public void OnDeleteTerrainOnClick() {
        if (tObject.transform.childCount > 0) {

            Transform importedTransform = tObject.transform.GetChild(0);

            if (importedTransform!= null && importedTransform.gameObject != null) {
                //Debug.Log("Deleting object: " + imported.name); // Debug statement
                DestroyImmediate(importedTransform.gameObject);
                Debug.Log("Object deleted");
            }
            else {
                Debug.LogWarning("Imported object is null or already destroyed.");
            }
        }
    }
}
