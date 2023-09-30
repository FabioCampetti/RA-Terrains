using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using System.Linq;

public class TerrainElevationGeneration : MonoBehaviour {


     public int terrainSize;

     public int heightmapResolution;
     private List<Location> vertexCoordinatesList;

     public Location location;
     private float highestElevation;
     private float lowestElevation;
     public String fileName;
     private Terrain terrain;

    private CSVHandler csvHandler;

    // Start is called before the first frame update
    void Start() {

        csvHandler = new CSVHandler(fileName, heightmapResolution-1);
        generateElevations();
        generateTerrain();
        ExportTerrain exporter= new ExportTerrain(terrain);
        exporter.Export();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void generateElevations() {
        getVertexCoordinates();
        APIHandler aPIHandler = new APIHandler(vertexCoordinatesList, heightmapResolution-1);
        ElevationResult[][] allElevations = aPIHandler.getElevations();
        csvHandler.WriteCSV(allElevations);
    }

    private void generateTerrain() {
        
        terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;


        ElevationResult[,] elevations = csvHandler.ReadCSV();
        terrainData.heightmapResolution = heightmapResolution;

        highestElevation = (float) csvHandler.highestElevation;
        lowestElevation = (float) csvHandler.lowestElevation;
        terrainData.size = new Vector3(terrainSize, highestElevation, terrainSize);

        terrainData.SetHeights(0, 0, calculateHeigths(elevations));
    }

    private float[,] calculateHeigths(ElevationResult[,] elevations) {

        float[,] newElevations = new float[heightmapResolution, heightmapResolution];
        for (int i = 0; i < heightmapResolution; i++) {
            for (int j = 0; j < heightmapResolution; j++) {
                    if (i == heightmapResolution - 1 || j == heightmapResolution - 1) {
                        if (i == heightmapResolution - 1) {
                            newElevations[i, j] = newElevations[i-1, j];
                        } else {
                            newElevations[i, j] = newElevations[i, j-1];
                        }
                    } else {
                        newElevations[i, j] = getScaleHeigth(elevations[i,j]);
                    }
            }
        }
        return newElevations;
    }

    private float getScaleHeigth(ElevationResult elevationResult) {
        float elevation = (float) elevationResult.elevation;
        float result = (elevation - lowestElevation)/(highestElevation - lowestElevation);
        return result;
    }

    private void getVertexCoordinates() {
            vertexCoordinatesList = new List<Location>();
            var distance = Math.Sqrt(Math.Pow((terrainSize/2), 2) + Math.Pow((terrainSize/2), 2));

            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 315.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 225.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 45.0, distance));
            vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 135.0, distance));

            if (heightmapResolution > 513) {
                vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 270.0, distance));
                vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 0.0, distance));
                vertexCoordinatesList.Add(location);
                vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 180.0, distance));
                vertexCoordinatesList.Add(Location.calculateNewCoordinates(location, 90.0, distance));
            }
        }
}