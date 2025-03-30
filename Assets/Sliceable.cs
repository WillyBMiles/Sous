using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField]
    Material internalMaterial;
    public Material InternalMaterial { get { return internalMaterial; } set { internalMaterial = value; } }

    public readonly static List<Sliceable> sliceables = new();

    public MeshFilter meshFilter;
    public float volume;
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sliceables.Add(this);
        volume = MeshSizeCalc.VolumeOfMeshFilter(meshFilter);
        if (volume < OutputAssesser.MinVolume * .5f && sliceables.Count > 100)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        sliceables.Remove(this);
    }

}
