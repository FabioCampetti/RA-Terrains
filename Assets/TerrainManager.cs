using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using System.Linq;

public class TerrainManager : MonoBehaviour {
    
    // La referencia al objeto de tipo Terrain.
    private Terrain terrain;

    private int terrainSize = 10000;

     private int heightmapResolution = 513;
     private List<Location> vertexCoordinatesList;
     public Location location;
     public float highestElevation;
     public float lowestElevation;

    // Propiedad para acceder a la instancia desde otras clases.

    // En TerrainManager
    void Awake() {
         Debug.Log("TerrainManager Start method is called.");
        terrain = Terrain.CreateTerrainGameObject(new TerrainData()).GetComponent<Terrain>();
        terrain.terrainData.heightmapResolution = heightmapResolution;
        gameObject.SetActive(true);
        terrain.gameObject.SetActive(true);
        location = new Location(0, 0); 
    }


    // Método para obtener el objeto Terrain.
    public Terrain getTerrain()
    {
        return terrain;
    }
    
    // Otras funciones y métodos relacionados con el TerrainManager.
    

    public List<Location> getVertexCoordinates() {

        if (vertexCoordinatesList == null) {

            var distance = Math.Sqrt(Math.Pow((terrainSize/2), 2) + Math.Pow((terrainSize/2), 2));
            vertexCoordinatesList = new List<Location>();
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 315.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 270.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 225.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 0.0, distance));
            vertexCoordinatesList.Add(location);
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 180.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 45.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 90.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 135.0, distance));

        }

        return vertexCoordinatesList;
    }

    public void exportTerrain() {
        ExportTerrain export = new ExportTerrain(terrain);
        export.Export();
    }
}
