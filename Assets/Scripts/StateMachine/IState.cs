public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();           //拱府包访x
    public void PhysicsUpdate();    //拱府包访
}
