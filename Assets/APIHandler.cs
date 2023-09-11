using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;

public class APIHandler {

    private string apiKey = "";
    private List<Location> vertexLocationsList;

    private int terrainSize;

    public APIHandler(List<Location> vertexLocationsList, int terrainSize) {
        this.vertexLocationsList = vertexLocationsList;
        this.terrainSize = terrainSize;
    }

    public ElevationResult[][] getElevations() {
        ElevationResult[][] allElevations = new ElevationResult[terrainSize][];
        ElevationResult[] leftElevations = generateRowOrColumnElevations(vertexLocationsList[0], vertexLocationsList[1], 180.0);
        ElevationResult[] rightElevations = generateRowOrColumnElevations(vertexLocationsList[2], vertexLocationsList[3], 180.0);
        for(int i = 0; i < terrainSize; i++) {
            allElevations[i] = generateRowOrColumnElevations(leftElevations[i].location, rightElevations[i].location, 90.0);
        }
        return allElevations;
    }

    private ElevationResult[] generateRowOrColumnElevations(Location firstLocation, Location finalLocation, double bearing) {
        ElevationResult[] elevations = new ElevationResult[0];
        Location initialLocation = firstLocation;
        for (int i = 0; i < 2; i++) {
            Location middleLocation = Location.calculateNewCoordinates(initialLocation, bearing, terrainSize/3);
            ElevationResult[] intermediateElevations = elevationsBetweenCoordinates(initialLocation, middleLocation, 343);
            elevations = elevations.Concat(intermediateElevations).ToArray();
            elevations = elevations.SkipLast(1).ToArray();
            initialLocation = middleLocation;
        }
        elevations = elevations.Concat(elevationsBetweenCoordinates(initialLocation,finalLocation, 341)).ToArray();
        return elevations;        
    }

    private ElevationResult[] elevationsBetweenCoordinates(Location firstCoordinate, Location secondCoordinate, int distPoints) {
         string url = $"https://maps.googleapis.com/maps/api/elevation/json?path={firstCoordinate.lat},{firstCoordinate.lng}|{secondCoordinate.lat},{secondCoordinate.lng}&samples={distPoints}&key={apiKey}";
         
         using (UnityWebRequest webRequest = UnityWebRequest.Get(url)) {
            webRequest.SendWebRequest();

            while (!webRequest.isDone) {
                // Espera hasta que la solicitud esté completa
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("Error en la solicitud a la API de elevación: " + webRequest.error);
            }

            string jsonResponse = webRequest.downloadHandler.text;
            ElevationResponse elevationResponse =  JsonUtility.FromJson<ElevationResponse>(jsonResponse);

            return elevationResponse.results;
        }
    }
}

[System.Serializable]
public class ElevationResponse
{
    public ElevationResult[] results;
    public string status;
}
