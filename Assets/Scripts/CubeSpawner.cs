using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [Header("Project References")] public GameObject piece; // reference to the piece prefab stored in the project view

    [Header("Scene References")]
    public GameObject faceRotationAxis; // reference to the FaceRotationAxis in the scene view

    [Header("Game Settings")] public int cubeSize = 3;

    [Header("Layout Settings")] public float spaceBetweenPieces = 1.0f;

    // stores references to pieces in each face [faceIndex(0-5)][pieceIndex]
    private List<GameObject>[] _piecesByFace;

    private void Start()
    {
        // create the cube's 6 faces to store references for the pieces
        _piecesByFace = new List<GameObject>[6];
        for (var i = 0; i < 6; i++) _piecesByFace[i] = new List<GameObject>();

        for (var x = 0; x < cubeSize; x++)
        for (var y = 0; y < cubeSize; y++)
        for (var z = 0; z < cubeSize; z++)
            if (x > 0 && x < cubeSize - 1 && y > 0 && y < cubeSize - 1 && z > 0 && z < cubeSize - 1)
            {
                // skip inner cubes
                // Debug.Log("Skipping " + x + "," + y + "," + z);
            }
            else
            {
                // calculate the position of the piece, centering the cube around (0,0,0)
                var pos = new Vector3(
                    CalcPosition(x),
                    CalcPosition(y),
                    CalcPosition(z)
                );

                // instantiate the cube at the calculated position
                var cube = Instantiate(piece, pos, Quaternion.identity);

                cube.transform.SetParent(transform);
                cube.name = x + "," + y + "," + z;

                // Add reference to this piece in its corresponding face
                // TODO: Add Middle (Between L and R), Equator (between U and D), Standing (between F and B)
                if (x == 0) _piecesByFace[0].Add(cube); // "Left Face";
                if (x == cubeSize - 1) _piecesByFace[1].Add(cube); // "Right Face";
                if (y == 0) _piecesByFace[2].Add(cube); // "Bottom Face";
                if (y == cubeSize - 1) _piecesByFace[3].Add(cube); // "Up Face";
                if (z == 0) _piecesByFace[4].Add(cube); // "Back Face";
                if (z == cubeSize - 1) _piecesByFace[5].Add(cube); // "Front Face";
            }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) RotateFace(0);
        if (Input.GetKeyDown(KeyCode.R)) RotateFace(1);
        if (Input.GetKeyDown(KeyCode.D)) RotateFace(2);
        if (Input.GetKeyDown(KeyCode.U)) RotateFace(3);
        if (Input.GetKeyDown(KeyCode.B)) RotateFace(4);
        if (Input.GetKeyDown(KeyCode.F)) RotateFace(5);
    }

    private float CalcPosition(int coordinate)
    {
        return coordinate - cubeSize * spaceBetweenPieces / 2f + spaceBetweenPieces / 2f;
    }

    // TODO: comment function
    private void RotateFace(int faceIndex, bool clockwise = true)
    {
        foreach (var p in _piecesByFace[faceIndex]) p.transform.SetParent(faceRotationAxis.transform);

        var angle = 90f;
        if (!clockwise) angle *= -1f;

        var rotationAxis = Vector3.zero;
        if (faceIndex is 0 or 1) rotationAxis = Vector3.right;
        if (faceIndex is 2 or 3) rotationAxis = Vector3.up;
        if (faceIndex is 4 or 5) rotationAxis = Vector3.forward;

        faceRotationAxis.transform.Rotate(rotationAxis, angle, Space.Self);
        foreach (var p in _piecesByFace[faceIndex]) p.transform.SetParent(transform);
    }
}