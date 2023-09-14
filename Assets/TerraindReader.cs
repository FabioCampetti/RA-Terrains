using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class TerraindReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        TextWriter tw = new StreamWriter(Application.dataPath + "/terrainData.txt", true);

          Terrain terrain = GetComponent<Terrain>();
          float [,] terrainHeigths = terrain.terrainData.GetHeights(0,0,513,513);
          for (int i = 0; i < 513; i++){
            for (int j = 0; j<513; j++) {
                tw.WriteLine("" + terrainHeigths[i,j]);
            }
          }
        tw.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
