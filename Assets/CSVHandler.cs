using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;
using System.Linq;

public class CSVHandler {

    public static void WriteCSV(string fileName, ElevationResult[][] elevationData) {

        TextWriter tw = new StreamWriter(Application.dataPath + $"/TerrainElevations/{fileName}", false);
        tw.WriteLine("Latitude, Longitude, Elevation");
        tw.Close();

        tw = new StreamWriter(fileName, true);

        for(int i = 0; i < elevationData.Length; i++) {
            for(int j = 0; j < elevationData[i].Length; j++) {
                tw.WriteLine(elevationData[i][j].toString());
            }
        }
        tw.Close();
    }

    public static TerrainElevationData ReadCSV(string fileName, int terrainSize) {

        ElevationResult[,] elevationResults = new ElevationResult[terrainSize, terrainSize];
        double highestElevation = Double.MaxValue * -1;
        double lowestElevation = Double.MaxValue;
        
        StreamReader reader = new StreamReader(Application.dataPath + $"/TerrainElevations/{fileName}");
        string line = reader.ReadLine();
        for (int i = 0; i < terrainSize && !reader.EndOfStream ; i++) {
            for (int j = 0; j < terrainSize && !reader.EndOfStream; j++) {
                
                    line = reader.ReadLine();
                    string[] fields = line.Split(',');

                    double elevation = double.Parse(fields[2]);

                    if ( elevation > highestElevation) {
                        highestElevation = elevation;
                    }
                    if (elevation < lowestElevation) {
                        lowestElevation = elevation;
                    }

                    elevationResults[i,j] = new ElevationResult (elevation,new Location (double.Parse(fields[0]), double.Parse(fields[1]))); // Define la ubicación según tus necesidades.
            }
        }

        return new TerrainElevationData(highestElevation, lowestElevation, elevationResults);
    }

    public static string getSavedElevationsFileName(Location location, int heightmapResolution, int terrainSize) {
        string elevationsFile = "";
        string[] archivosCSV = Directory.GetFiles(Application.dataPath + "/TerrainElevations")
                    .Where(file => file.ToLower().EndsWith(".csv"))
                    .Select(file => Path.GetFileName(file))
                    .ToArray();

        foreach (string archivo in archivosCSV) {
            if (areElevationsInFile(archivo, location, heightmapResolution, terrainSize)) {
                elevationsFile = archivo;
                break;
            }
        }
        return elevationsFile;
    }

    private static bool areElevationsInFile(string archivo, Location location, int heightmapResolution, int terrainSize) {

        bool isInRange = false;
        string path = Path.Combine(Application.dataPath + "/TerrainElevations", archivo);

        if (File.Exists(path)) {
            // Lee las líneas específicas y obtiene las coordenadas
            string[] lineas = File.ReadAllLines(path);

            if((lineas.Length-1)/(heightmapResolution-1) == heightmapResolution-1) {
                Debug.Log(archivo);
                 List<Location> vertexList = new List<Location>();
                 vertexList.Add(getLocationFromline(lineas[1]));
                 vertexList.Add(getLocationFromline(lineas[heightmapResolution]));
                 vertexList.Add(getLocationFromline(lineas[(heightmapResolution-1) * (heightmapResolution-1) - (heightmapResolution-1)]));
                 vertexList.Add(getLocationFromline(lineas[(heightmapResolution-1) * (heightmapResolution-1)]));

                 Vector2 positonInUnity = CoordianteConverter.ConvertToUnityCoordinates(location, vertexList, terrainSize);
                
                 isInRange = positonInUnity.x > 0 && positonInUnity.x < terrainSize && positonInUnity.y > 0 && positonInUnity.y < terrainSize;
            }
        }
        return isInRange;
    }

    private static Location getLocationFromline(string line) {
        string [] coordinate = line.Split(",");
        return new Location (double.Parse(coordinate[0]), double.Parse(coordinate[1]));
    }
}
