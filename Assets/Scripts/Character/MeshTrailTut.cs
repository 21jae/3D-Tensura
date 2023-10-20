using System;
using System.Collections;
using UnityEngine;

public class MeshTrailTut : MonoBehaviour
{
    public float activeTime = 2f;
    private bool isTrailActive;

    [Header("메쉬")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f;
    public Transform positionToSpawn;

    [Header("셰이더")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;

    //캐릭터가 얼마나 많은 스킨과 메쉬 렌더링을 갖고있는가
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

            //각 스킨과 메쉬 반복
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation); 

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                //각 정점이 어디에있는지 기록한다.
                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = new Material(mat);  //새로운 재질 인스턴스를 사용해 복제 매쉬 생성

                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }

        //0 이되면 더이상 트레일이 활성화되지않음
        isTrailActive = false;
    }

    //페이드아웃을 위해 머터리얼의 목표, 속도, 새로고침 빈도 매개변수 설정
    private IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        //애니메이션 부동 소수점에 액세스
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate); //애니메이션 값 전달
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
