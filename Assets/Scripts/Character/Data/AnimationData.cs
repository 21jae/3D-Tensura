using UnityEngine;

[System.Serializable]
public class AnimationData
{
    [Header("Move Parameter Names")]
    [SerializeField] private string moveParameterName =      "MoveSpeed";

    [Header("Attack Parameter Names")]
    [SerializeField] private string attack01ParameterName =   "Attack01";
    [SerializeField] private string attack02ParameterName =   "Attack02";
    [SerializeField] private string attack03ParameterName =   "Attack03";
    [SerializeField] private string attack04ParameterName =   "Attack04";

    [Header("State Parameter Names")]
    [SerializeField] private string damageParameterName =       "Damage";
    [SerializeField] private string bigDamageParameterName = "BigDamage";
    [SerializeField] private string standUpParameterName =     "StandUp";
    [SerializeField] private string DeathParameterName =         "Death";
    [SerializeField] private string changeParameterName =   "ChangeFrom";
    [SerializeField] private string winParameterName =             "Win";

    [Header("NPC Parameter Names")]
    [SerializeField] private string talkParameterName = "isTalking";


    public int MoveParmeterName         { get; private set; }
    public int Attack01ParmeterName     { get; private set; }
    public int Attack02ParmeterName     { get; private set; }
    public int Attack03ParmeterName     { get; private set; }
    public int Attack04ParmeterName     { get; private set; }
    public int DamageParmeterName       { get; private set; }
    public int BigDamageParmeterName    { get; private set; }
    public int StandUpParmeterName      { get; private set; }
    public int DeathParmeterName        { get; private set; }
    public int ChangeParmeterName       { get; private set; }
    public int WinParameterName         { get; private set; }
    public int TalkParmeterName         { get; private set; }

    public void Initialize()
    {
        MoveParmeterName = Animator.StringToHash(moveParameterName);

        Attack01ParmeterName = Animator.StringToHash(attack01ParameterName);
        Attack02ParmeterName = Animator.StringToHash(attack02ParameterName);
        Attack03ParmeterName = Animator.StringToHash(attack03ParameterName);    
        Attack04ParmeterName = Animator.StringToHash(attack04ParameterName);

        DamageParmeterName = Animator.StringToHash(damageParameterName);
        BigDamageParmeterName = Animator.StringToHash(bigDamageParameterName);
        StandUpParmeterName = Animator.StringToHash(standUpParameterName);
        DeathParmeterName = Animator.StringToHash(DeathParameterName);
        ChangeParmeterName = Animator.StringToHash(changeParameterName);

        WinParameterName = Animator.StringToHash(winParameterName);
        TalkParmeterName = Animator.StringToHash(talkParameterName);
    }
}
