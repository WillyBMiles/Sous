using EzySlice;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Slicer : MonoBehaviour
{
    [SerializeField] GameObject cuttingPlane;

    [SerializeField] BoxCollider cuttingBox;
    [SerializeField] float jitter;


    private void Awake()
    {
        Sliceable.sliceables.Clear();
    }

    public void SliceBelowKnife()
    {
        Vector3 worldCenter = cuttingBox.transform.TransformPoint(cuttingBox.center);
        Vector3 worldHalfExtents = Vector3.Scale(cuttingBox.size, cuttingBox.transform.lossyScale) * 0.5f;
        Collider[] colliders = Physics.OverlapBox(worldCenter, worldHalfExtents, cuttingBox.transform.rotation);
        if (colliders == null || colliders.Length == 0)
        {
            Debug.Log("Nothing to slice");
        }

        bool hasAnyCuts = false;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Sliceable>(out Sliceable sliceable))
            {
                Slice(sliceable);
                hasAnyCuts = true;
            }
        }
        if (!hasAnyCuts)
        {
            Debug.Log("Still nothing to slice it seems.");
        }
    }

    public GameObject[] Slice(Sliceable sliceable)
    {
        GameObject[] gos = sliceable.gameObject.SliceInstantiate(cuttingPlane.transform.position, cuttingPlane.transform.up, sliceable.InternalMaterial);
        if (gos == null)
        {
            Debug.Log("Slice can't be found");
            return null;
        }
        foreach (GameObject go in gos)
        {
            MeshCollider collider = go.AddComponent<MeshCollider>();
            collider.convex = true;
            Sliceable thisSliceable = go.AddComponent<Sliceable>();
            thisSliceable.InternalMaterial = sliceable.InternalMaterial;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.AddForce((Random.value < 0.5f ? -1 : 1) * jitter * cuttingPlane.transform.up, ForceMode.Impulse);
            rb.mass = 4f;

            foreach (Component component in sliceable.GetComponents(typeof(Component))){
                if (component is not Transform && component is not MeshCollider && component is not Sliceable && component is not Rigidbody
                    && component is not MeshFilter && component is not MeshRenderer)
                {
                    go.AddComponent(component.GetType());
                }
            }
            
        }
        Destroy(sliceable.gameObject);

        return gos;
    }
}
