#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Markerplace : MonoBehaviour
{
    public GameObject markerPrefab;       // Prefab pour le rep�re visuel
    public GameObject obstaclePrefab;     // Prefab pour l'obstacle � placer
    private List<Vector3> markerPositions = new List<Vector3>(); // Liste des positions des rep�res

    void Update()
    {
        // Placer un rep�re visuel et enregistrer sa position avec la touche espace
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceMarker();
        }

        // G�n�rer les obstacles aux positions enregistr�es avec la touche O
        if (Input.GetKeyDown(KeyCode.O))
        {
            GenerateObstacles();
        }
    }

    // Fonction pour placer un rep�re visuel
    void PlaceMarker()
    {
        // Cr�er un rep�re visuel � la position actuelle du joueur
        GameObject marker = Instantiate(markerPrefab, transform.position, Quaternion.identity);
        markerPositions.Add(transform.position); // Enregistrer la position du rep�re
        Debug.Log("Rep�re plac� � : " + transform.position);
    }

    // Fonction pour g�n�rer les obstacles � la position des rep�res
    void GenerateObstacles()
    {
        foreach (Vector3 pos in markerPositions)
        {
            // Cr�er un obstacle � chaque position de rep�re
            GameObject obstacle = Instantiate(obstaclePrefab, pos, Quaternion.identity);
            Debug.Log("Obstacle g�n�r� � : " + pos);

            // Sauvegarder l'obstacle dans la sc�ne (pas comme prefab, mais dans la hi�rarchie)
            SaveToScene(obstacle);
        }

        // Optionnel : Vider la liste apr�s avoir plac� les obstacles
        markerPositions.Clear();
    }

    // Fonction pour rendre les obstacles permanents dans la sc�ne
    void SaveToScene(GameObject obstacle)
    {
        // Si on est en mode Play, convertir l'obstacle en objet permanent dans la sc�ne
        if (Application.isPlaying)
        {
            // Marquer l'objet comme "ne pas d�truire" lors du rechargement de la sc�ne
            DontDestroyOnLoad(obstacle);

            // Sauvegarder directement l'objet dans la sc�ne en tant qu'objet permanent
            PrefabUtility.RecordPrefabInstancePropertyModifications(obstacle);
            Debug.Log("Obstacle sauvegard� dans la sc�ne.");
        }
        else
        {
            Debug.LogError("Impossible de sauvegarder en dehors du mode Play");
        }
    }
}
#endif
