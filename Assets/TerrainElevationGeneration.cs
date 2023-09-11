using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class TerrainElevationGeneration : MonoBehaviour {


     public int terrainSize = 10000;

     public int heightmapResolution = 1025;
     private List<Location> vertexCoordinatesList;

     private Location location;
     public float highestElevation;


    private CSVHandler csvHandler;

    // Start is called before the first frame update
    void Start() {

        double lat = 27.98865069515548;
        double lon = 86.92544716876068;
        location = new Location(lat, lon);
        csvHandler = new CSVHandler("everestElevations", heightmapResolution);
        //generateElevations();
        generateTerrain(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateElevations() {
        vertexCoordinatesList = getVertexCoordinates();
        APIHandler aPIHandler = new APIHandler(vertexCoordinatesList, heightmapResolution);
        ElevationResult[][] allElevations = aPIHandler.getElevations();
        csvHandler.WriteCSV(allElevations);
    }

    private void generateTerrain() {
        
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;


        ElevationResult[,] elevations = csvHandler.ReadCSV();
        terrainData.heightmapResolution = heightmapResolution;

        highestElevation = (float) csvHandler.highestElevation;
        terrainData.size = new Vector3(10000, highestElevation, 10000);
        
        terrainData.SetHeights(0, 0, calculateHeigths(elevations));
        Debug.Log(terrainData.heightmapResolution);
    }

    private float[,] calculateHeigths(ElevationResult[,] elevations) {
        float[,] newElevations = new float[heightmapResolution, heightmapResolution];
        for (int i = 0; i < heightmapResolution; i++) {
            for (int j = 0; j < heightmapResolution; j++) {
                if (elevations[i,j] != null) {
                    newElevations[j, i] = getScaleHeigth(elevations[j,i]);
                }
            }
        }
        return newElevations;
    }

    private float getScaleHeigth(ElevationResult elevationResult) {
        float result = (float) (elevationResult.elevation /highestElevation);
        return result;
    }

    private List<Location> getVertexCoordinates() {
        var distance = Math.Sqrt(Math.Pow((terrainSize/2), 2) + Math.Pow((terrainSize/2), 2));
        List<Location> vertex = new List<Location>();
        vertex.Add(Location.calculateNewCoordinates(location, 315.0, distance));
        vertex.Add(Location.calculateNewCoordinates(location, 225.0, distance));
        vertex.Add(Location.calculateNewCoordinates(location, 45.0, distance));
        vertex.Add(Location.calculateNewCoordinates(location, 135.0, distance));
        return vertex;
    }
}
