using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class CombineMeshes : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private void Combine()
    {
        int childCount = transform.childCount;

        CombineInstance[] combine = new CombineInstance[childCount];
        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(i);
            var meshFilter = child.GetComponent<MeshFilter>();
            combine[i].mesh = meshFilter.sharedMesh;
            combine[i].transform = meshFilter.transform.localToWorldMatrix;
            //child.gameObject.SetActive(false);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.name = "Combined Mesh";
        combinedMesh.CombineMeshes(combine);

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
        }

        meshFilter.sharedMesh = combinedMesh;
        meshCollider.sharedMesh = combinedMesh;

        Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
        Material urpLitMaterial = new Material(urpLitShader);
        meshRenderer.material = urpLitMaterial;
    }

    [CustomEditor(typeof(CombineMeshes))]
    private class CombineMeshesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Combine"))
            {
                ((CombineMeshes)target).Combine();
            }
        }
    }
}
