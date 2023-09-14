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
        ElevationResult[] leftElevations = elevationsBetweenCoordinates(vertexLocationsList[0], vertexLocationsList[1]);
        ElevationResult[] rightElevations = elevationsBetweenCoordinates(vertexLocationsList[2], vertexLocationsList[3]);
        for(int i = 0; i < terrainSize; i++) {
            allElevations[i] = elevationsBetweenCoordinates(leftElevations[i].location, rightElevations[i].location);
        }
        return allElevations;
    }

    private ElevationResult[] elevationsBetweenCoordinates(Location firstCoordinate, Location secondCoordinate) {
         string url = $"https://maps.googleapis.com/maps/api/elevation/json?path={firstCoordinate.lat},{firstCoordinate.lng}|{secondCoordinate.lat},{secondCoordinate.lng}&samples=512&key={apiKey}";
         
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
