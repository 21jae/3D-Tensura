# 애니메이션 기반 모바일 게임
<img src="https://github.com/21jae/3D-Tensura/assets/90013449/991671ba-a805-4bbd-b343-e11c2533ca7d" width="40%" height="auto">

## 개요
제목 | 전생했더니 슬라임이었던 건
------------ | ------------- 
장르 | 모바일 RPG
개발 기간 | 1달
사용 툴 | Unity
개발자 | 21jae

## 게임 소개
이 게임은 "전생했더니 슬라임이었던 건에 대하여" 라는 일본 라이트 노벨을 원작으로 했으며, 주인공이 슬라임으로 전생하여 다양한 모험을 겪는 내용입니다. 
이를 게임속에서 원작의 느낌을 즐길수있도록 개발하였습니다.
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
  * [Object Pool](https://github.com/21jae/3D-Tensura/blob/main/Assets/Scripts/Manager/ObjectPooling.cs) : 
</br>


## 기술서

</br>


## 배운점


</br>
