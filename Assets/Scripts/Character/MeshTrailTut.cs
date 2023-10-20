using System;
using System.Collections;
using UnityEngine;

public class MeshTrailTut : MonoBehaviour
{
    public float activeTime = 2f;
    private bool isTrailActive;

    [Header("�޽�")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;

    [Header("���̴�")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    //ĳ���Ͱ� �󸶳� ���� ��Ų�� �޽� �������� �����ִ°�
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Awake()
    {   
        //Getcomponet 's' 
        skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();  

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
        }
    }
          
    private IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {  
            timeActive -= meshRefreshRate;

            //�� ��Ų�� �޽� �ݺ�
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation); 

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                //�� ������ ����ִ��� ����Ѵ�.
                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = new Material(mat);  //���ο� ���� �ν��Ͻ��� ����� ���� �Ž� ����

                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        //0 �̵Ǹ� ���̻� Ʈ������ Ȱ��ȭ��������
        isTrailActive = false;
    }

    //���̵�ƿ��� ���� ���͸����� ��ǥ, �ӵ�, ���ΰ�ħ �� �Ű����� ����
    private IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        //�ִϸ��̼� �ε� �Ҽ����� �׼���
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate); //�ִϸ��̼� �� ����
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
