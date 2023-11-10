using UnityEngine;

[System.Serializable]
public class PlayerMeshData
{
    [field:Header("메쉬")]
    [field: SerializeField] public float meshRefreshRate { get; private set; } = 0.1f;
    [field: SerializeField] [field: Range(0f, 5f)] public float meshDestroyDelay { get; private set; } = 3f;

    [field: Header("셰이더")]
    [field: SerializeField] public string shaderVarRef { get; private set; }
    [field: SerializeField][field: Range(0f, 1f)] public float shaderVarRate { get; private set; } = 0.1f;
    [field: SerializeField] [field: Range(0f, 0.3f)] public float shaderVarRefreshRate { get; private set; } = 0.05f;

    [field: Header("변수")]
    [field: SerializeField] public float activeTime { get; private set; } = 2f;
    [field: SerializeField] public bool isTrailActive { get; private set; }

    public void SetIsTrail(bool isTrail)
    {
        isTrailActive = isTrail;
    }
}
