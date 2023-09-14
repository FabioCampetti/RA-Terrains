using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using System.Linq;

public class TerrainElevationGeneration : MonoBehaviour {


     public int terrainSize = 10000;

     public int heightmapResolution = 513;
     private List<Location> vertexCoordinatesList;

     private Location location;
     public float highestElevation;
     public float lowestElevation;

     private Terrain terrain;

    private CSVHandler csvHandler;

    // Start is called before the first frame update
    void Start() {

        double lat = 27.98865069515548;
        double lon = 86.92544716876068;
        location = new Location(lat, lon);
        csvHandler = new CSVHandler("everest512Elevations", heightmapResolution-1);
        //generateElevations();
        generateTerrain();
        ExportTerrain exporter= new ExportTerrain(terrain);
        exporter.Export();
 
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
        
        terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;


        ElevationResult[,] elevations = csvHandler.ReadCSV();
        terrainData.heightmapResolution = heightmapResolution;

        highestElevation = (float) csvHandler.highestElevation;
        lowestElevation = (float) csvHandler.lowestElevation;
        terrainData.size = new Vector3(terrainSize, highestElevation, terrainSize);

        terrainData.SetHeights(0, 0, calculateHeigths(elevations));
    }

    private float[,] getHeigthsFromImport() {
        StreamReader reader = new StreamReader(Application.dataPath + "/terrainData.txt");

        float[,]    heigths = new float[heightmapResolution, heightmapResolution];
        for (int i = 0; i < heightmapResolution && !reader.EndOfStream ; i++) {
            for (int j = 0; j < heightmapResolution && !reader.EndOfStream; j++) {
                    string line = reader.ReadLine();
                    heigths[i,j] = float.Parse(line);
            }
        }
        return heigths;
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
