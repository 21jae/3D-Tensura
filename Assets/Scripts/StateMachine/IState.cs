public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();           //물리관련x
    public void PhysicsUpdate();    //물리관련
}
