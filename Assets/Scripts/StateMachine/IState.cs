using UnityEngine;

public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();           //물리관련x
    public void PhysicsUpdate();    //물리관련
    public void OnAnimationEnterEvent();
    public void OnAnimationExitEvent();
    public void OnAnimationTransitionEvent();    //애니메이션이 특정 프레임에 들어갈때 다른상태로 전환
}
