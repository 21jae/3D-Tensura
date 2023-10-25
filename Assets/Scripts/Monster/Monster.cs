using UnityEngine;

public class Monster : MonoBehaviour
{
    public bool IsBoss { get; private set; }

    public void SetAsBoss()
    {
        IsBoss = true;
    }
}
