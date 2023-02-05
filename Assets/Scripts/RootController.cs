using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RootController : MonoBehaviour
{
    // TODO: A property like "maxSpeed" would be much more useful, but at least this looks right.
    public float forceCoef;
    public float snapshotDistance = 1.0f;
    public float rootRadius = 2.0f;
    public int rootCount = 4;
    public Color[] possibleColors;
    public float expandQuickness = 3.0f;

    public Material trailMaterial;
    public float endWidth = 0.18f;
    public float startWidth = 0.07f;
    public float minVertexDistance = 0.3f;

    // Add color gradient so that end is more dark. Definitely set min, max values, so that it's not too dark altogether.
//    public float startColor = Random.ColorHSV(0.043f, 0.09f, 0.9f, 1.0f);
//    public float endColor = Random.ColorHSV(0.043f, 0.09f, 0.9f, 1.0f);

    private List<Vector3> previousPositions = new List<Vector3>();
    private List<GameObject> trails = new List<GameObject>();

    private Vector3 movementDirection;
    private Vector3 previousSnapshot;
    private float timeSnap;
    private Vector3 startPosition;

    public Color color = Color.yellow;
    private Vector3 tempHSV;

    public Color color2 = Color.yellow;
    private Vector3 tempHSV2;

    void Start()
    {
        startPosition = transform.position;
        previousSnapshot = startPosition;

        // TODO: Change to start pos of controller.
        previousPositions.Add(startPosition);

        // Create trail objects
        for (int i = 0; i < rootCount; i++)
        {
            GameObject trail = new GameObject($"Trail {i} {Random.Range(0, 10000)}");

            trail.transform.position = transform.position + Random.insideUnitSphere * rootRadius;

            TrailRenderer trailComp = trail.AddComponent<TrailRenderer>();
            VariableStoreTrail trailVar = trail.AddComponent<VariableStoreTrail>();

            trailVar.startPos = trail.transform.position;
            trailVar.endPos = trail.transform.position;

            trailComp.time = float.PositiveInfinity;
            trailComp.endWidth = endWidth;
            trailComp.startWidth = startWidth;
            trailComp.material = trailMaterial;
            // Add color gradient so that end is more dark. Definitely set min, max values, so that it's not too dark altogether.
            // TODO ADD THE POSSIBLE COLORS


            Color.RGBToHSV(color, out tempHSV.x, out tempHSV.y, out tempHSV.z);
            tempHSV.x += Random.Range(-0.04f, 0.04f);
            tempHSV.y += Random.Range(-0.04f, 0.04f);
            tempHSV.z += Random.Range(-0.04f, 0.04f);


            Color.RGBToHSV(color2, out tempHSV2.x, out tempHSV2.y, out tempHSV2.z);
            tempHSV2.x += Random.Range(-0.04f, 0.04f);
            tempHSV2.y += Random.Range(-0.04f, 0.04f);
            tempHSV2.z += Random.Range(-0.04f, 0.04f);

            color = Color.HSVToRGB(tempHSV.x, tempHSV.y, tempHSV.z);
            color2 = Color.HSVToRGB(tempHSV2.x, tempHSV2.y, tempHSV2.z);
            trailComp.startColor = color;//Random.ColorHSV(0.043f, 0.09f, 0.9f, 1.0f);
            trailComp.endColor = color2;//Random.ColorHSV(0.043f, 0.09f, 0.9f, 1.0f);
            trailComp.minVertexDistance = minVertexDistance;

            trails.Add(trail);
        }
    }

    void OnDrawGizmos()
    {
        // DEBUG
        // Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(transform.position, rootRadius);

        foreach (var pos in previousPositions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pos, 0.4f);
        }
    }

    void Update()
    {
        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        movementDirection = movementDirection.normalized;
    }

    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        if ((currentPos - previousSnapshot).sqrMagnitude > snapshotDistance * snapshotDistance)
        {
            timeSnap = Time.fixedTime;

            foreach (var trail in trails)
            {
                VariableStoreTrail trailVar = trail.GetComponent<VariableStoreTrail>();
                trailVar.startPos = trailVar.endPos;
                trailVar.endPos = transform.position + Random.insideUnitSphere * rootRadius;
            }

            previousPositions.Add(currentPos);
            previousSnapshot = currentPos;
        }

        foreach (var trail in trails)
        {
            VariableStoreTrail trailVar = trail.GetComponent<VariableStoreTrail>();
            float timeSinceSnap = Time.fixedTime - timeSnap;
            trail.transform.position = Vector3.Lerp(trailVar.startPos, trailVar.endPos, timeSinceSnap * expandQuickness);
        }
    }
}
