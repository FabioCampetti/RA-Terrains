using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class CSVHandler {

    private string fileName;
    public double highestElevation;
    public double lowestElevation;
    private int terrainSize;

    public CSVHandler (string fileName, int terrainSize) {
        this.fileName = Application.dataPath + $"/{fileName}.csv";
        this.terrainSize = terrainSize;
        highestElevation = 0.0;
        lowestElevation = Double.MaxValue;
    }

    public void WriteCSV(ElevationResult[][] elevationData) {
        TextWriter tw = new StreamWriter(fileName, false);
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

    public ElevationResult[,] ReadCSV() {

        ElevationResult[,] elevationResults = new ElevationResult[terrainSize, terrainSize];

        StreamReader reader = new StreamReader(fileName);
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
        return elevationResults;
    }
}
