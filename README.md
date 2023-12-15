# 애니메이션 기반 모바일 게임
<img src="https://github.com/21jae/3D-Tensura/assets/90013449/991671ba-a805-4bbd-b343-e11c2533ca7d" width="40%" height="auto">

## 개요
제목 | 전생했더니 슬라임이었던 건
------------ | ------------- 
장르 | 모바일 RPG
개발 기간 | 23.11.20 ~ 23.12.06
사용 툴 | Unity
개발자 | 21jae

## 게임 소개
이 게임은 "전생했더니 슬라임이었던 건에 대하여"라는 일본 라이트 노벨을 원작으로 했으며, 주인공이 슬라임으로 전생하여 다양한 모험을 겪는 내용입니다. 
이를 게임 속에서 원작의 느낌을 즐길 수 있도록 개발하였습니다.
</br>

## * **게임 플레이 특징**
  * 원작 애니메이션의 스킬을 동일하게 구현하였습니다.
    ### 주인공의 변신 기능 구현 </br>
    <img src="https://github.com/21jae/3D-Tensura/assets/90013449/7c23cab8-3c68-41ff-9a02-e264d09e9310" width="85%" height="auto">
    
    ### 주인공의 흡수 스킬 구현 </br>
    <img src="https://github.com/21jae/3D-Tensura/assets/90013449/1f734e31-faa0-444f-b324-ca9f6cdcddcb" width="42%" height="auto"><img src="https://github.com/21jae/3D-Tensura/assets/90013449/a7bdb128-26bb-4154-810d-b3921cb2d42f" width="50%" height="auto">
    </br>
 
    ### 주인공의 필살기 구현
       <img src="https://github.com/21jae/3D-Tensura/assets/90013449/5823f482-965e-4188-96ba-e3c8aab75717" width="40%" height="auto"> <img src="https://github.com/21jae/3D-Tensura/assets/90013449/a33ba703-30be-4c49-8a52-9737a96ce553" width="58%" height="auto">
</br>

       
## 주요 기능
### * Managers
   * [SkillManager](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Manager/SkillManager.cs) : 모든 스킬들을 관리합니다.
   * [SpawnManager](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Manager/SpawnManager.cs) : 생성된 몬스터를 적절한 위치에 배치 및 초기화를 작업 합니다.
### * Data
  * [PlayerSO](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Character/Data/PlayerSO.cs) : 손쉽게 커스터마이징 가능한 Player 데이터
    * [GroundData](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Character/Data/GroundData.cs)
  * [AnimationData](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Character/Data/AnimationData.cs) : String문자열 Hash 변환
  * [SkillData](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Character/Data/PlayerSkillData.cs): 스킬관련 데이터 관리
    *  [PredationData ](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Character/Data/PlayerPredationData.cs) : 흡수 데이터
    *  [MegidoData](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Character/Data/PlayerMegidoData.cs) : 필살기 데이터

### 유틸 
  * [CoroutineHelper](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/ETC/CoroutineHelper.cs) : 코루틴 최적화를 진행합니다.
  * [Object Pool](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Manager/ObjectPooling.cs) : 오브젝트 풀
</br>


## 배운점
* 모바일게임 성능 최적화 하는 법에 대해 알게 되었습니다. 이 포트폴리오를 끝내기 전까지는 "빌드만 되자"라는 심정이었는데, 성능에 대한 부분도 신경 쓰게 되었습니다. 이러한 부분도 포트폴리오 진행중에 깨닫게 되어서, 다음 프로젝트에서는 프로파일링을 하며 직접 성능을 비교하며 체크해야겠다 느꼈습니다.</br>

* 무언가 구현하는 데에, 수학적인 지식이 중요하다는 것을 느꼈습니다. 스킬을 애니메이션과 똑같이 구현하는데에 수학적 지식이 부족해 어려움이 많았고, 이 부분에서 부족함을 많이 느껴 게임 수학을 더욱더 배우고자 합니다.


## 기술서
[PDF자료](https://file.notion.so/f/f/60d85208-d2f5-4b65-bcee-71940fa52b65/46bfdf9c-3806-40e9-8acf-da2fdd0640b9/%EB%AA%A8%EB%B0%94%EC%9D%BC_%EA%B2%8C%EC%9E%84.pdf?id=3120e8fd-d837-4304-b1ae-7b13b0470646&table=block&spaceId=60d85208-d2f5-4b65-bcee-71940fa52b65&expirationTimestamp=1702598400000&signature=NVtfTyzeOxgpXR0Qfuq1N_h_cw5nTa7F5n_FmSoTk6o&downloadName=%EB%AA%A8%EB%B0%94%EC%9D%BC+%EA%B2%8C%EC%9E%84.pdf)
</br>



## 플레이 영상
[플레이](https://youtu.be/jJ66HjsKkuk)
