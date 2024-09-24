#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Markerplace : MonoBehaviour
{
    public GameObject markerPrefab;       // Prefab pour le repère visuel
    public GameObject obstaclePrefab;     // Prefab pour l'obstacle à placer
    private List<Vector3> markerPositions = new List<Vector3>(); // Liste des positions des repères

    void Update()
    {
        // Placer un repère visuel et enregistrer sa position avec la touche espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceMarker();
        }

        // Générer les obstacles aux positions enregistrées avec la touche O
        if (Input.GetKeyDown(KeyCode.O))
        {
            GenerateObstacles();
        }
    }

    // Fonction pour placer un repère visuel
    void PlaceMarker()
    {
        // Créer un repère visuel à la position actuelle du joueur
        GameObject marker = Instantiate(markerPrefab, transform.position, Quaternion.identity);
        markerPositions.Add(transform.position); // Enregistrer la position du repère
        Debug.Log("Repère placé à : " + transform.position);
    }

    // Fonction pour générer les obstacles à la position des repères
    void GenerateObstacles()
    {
        foreach (Vector3 pos in markerPositions)
        {
            // Créer un obstacle à chaque position de repère
            GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);
            Debug.Log("Obstacle généré à : " + pos);

            // Sauvegarder l'obstacle dans la scène (pas comme prefab, mais dans la hiérarchie)
            SaveToScene(obstacle);
        }

        // Optionnel : Vider la liste après avoir placé les obstacles
        markerPositions.Clear();
    }

    // Fonction pour rendre les obstacles permanents dans la scène
    void SaveToScene(GameObject obstacle)
    {
        // Si on est en mode Play, convertir l'obstacle en objet permanent dans la scène
        if (Application.isPlaying)
        {
            // Marquer l'objet comme "ne pas détruire" lors du rechargement de la scène
            DontDestroyOnLoad(obstacle);

            // Sauvegarder directement l'objet dans la scène en tant qu'objet permanent
            PrefabUtility.RecordPrefabInstancePropertyModifications(obstacle);
            Debug.Log("Obstacle sauvegardé dans la scène.");
        }
        else
        {
            Debug.LogError("Impossible de sauvegarder en dehors du mode Play");
        }
    }
}
#endif
