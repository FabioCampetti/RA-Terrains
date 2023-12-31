using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public static class CSVHandler {

    private static string folderPath = Application.dataPath + $"/TerrainElevations/";
    public static void WriteCSV(string fileName, ElevationResult[][] elevationData) {

        TextWriter writer  = new StreamWriter($"{folderPath}{fileName}", false);
        writer.WriteLine("Latitude, Longitude, Elevation");
        writer.Close();
 
        writer = new StreamWriter($"{folderPath}{fileName}", true);

        for(int i = 0; i < elevationData.Length; i++) {
            for(int j = 0; j < elevationData[i].Length; j++) {
                writer.WriteLine(elevationData[i][j].toString());
            }
        }
        writer.Close();
    }

    public static TerrainElevationData ReadCSV(string fileName, int terrainSize) {

        ElevationResult[,] elevationResults = new ElevationResult[terrainSize, terrainSize];
        double highestElevation = double.MaxValue * -1;
        double lowestElevation = double.MaxValue;
        
        StreamReader reader = new StreamReader($"{folderPath}{fileName}");
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

        reader.Close();

        return new TerrainElevationData(highestElevation, lowestElevation, elevationResults);
    }

    public static string getSavedElevationsFileName(Location location, int heightmapResolution, int terrainSize) {
        string elevationsFile = "";
        string[] archivosCSV = Directory.GetFiles(folderPath)
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
        string path = Path.Combine(folderPath, archivo);

        if (File.Exists(path)) {
            // Lee las líneas específicas y obtiene las coordenadas
            string[] lineas = File.ReadAllLines(path);

            if((lineas.Length-1)/(heightmapResolution-1) == heightmapResolution-1) {
                
                 List<Location> vertexList = new List<Location>
                 {
                     getLocationFromline(lineas[1]),
                     getLocationFromline(lineas[heightmapResolution]),
                     getLocationFromline(lineas[(heightmapResolution - 1) * (heightmapResolution - 1) - (heightmapResolution - 1)]),
                     getLocationFromline(lineas[(heightmapResolution - 1) * (heightmapResolution - 1)])
                 };

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
