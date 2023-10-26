using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool IsBoss { get; private set; }
    public bool IsLizard { get; private set; }
    public bool IsOrc { get; private set; }

    public void SetAsBoss()
    {
        IsBoss = true;
    }

    public void SetAsLizard()
    {
        IsLizard = true;
    }
    public void SetAsOrc()
    {
        IsOrc = true;
    }

}
