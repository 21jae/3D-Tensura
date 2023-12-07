using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject soundManagerObj = new GameObject("SoundManager");
                    instance = soundManagerObj.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeAuidoSource();
    }

    private void InitializeAuidoSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayBackgroundMusic()
    {
        if (audioClips.Length > 0)
        {
            audioSource.clip = audioClips[0];
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void PlayAttackSound01()
    {
        if (audioClips.Length > 1)
            audioSource.PlayOneShot(audioClips[1]);
    }

    public void PlayAttackSound02()
    {
        if (audioClips.Length > 2)
            audioSource.PlayOneShot(audioClips[2]);
    }

    public void PlayAttackSound03()
    {
        if (audioClips.Length > 3)
            audioSource.PlayOneShot(audioClips[3]);
    }

    public void PlayAttackSound04()
    {
        if (audioClips.Length > 4)
            audioSource.PlayOneShot(audioClips[4]);
    }

    public void PlaySpecialSound01()
    {
        if (audioClips.Length > 5)
            audioSource.PlayOneShot(audioClips[5]);
    }
    public void PlaySpecialSound02()
    {
        if (audioClips.Length > 6)
            audioSource.PlayOneShot(audioClips[6]);
    }
    public void PlaySpecialSound03()
    {
        if (audioClips.Length > 7)
            audioSource.PlayOneShot(audioClips[7]);
    }

    public void PlayFightSound()
    {
        if (audioClips.Length > 8)
            audioSource.PlayOneShot(audioClips[8]);
    }

    public void PlayBlessingSound()
    {
        if (audioClips.Length > 9)
            audioSource.PlayOneShot(audioClips[9]);
    }

    public void PlayChangeSound()
    {
        if (audioClips.Length > 10)
            audioSource.PlayOneShot(audioClips[10]);
    }
    public void PlayBuffSound()
    {
        if (audioClips.Length > 11)
            audioSource.PlayOneShot(audioClips[11]);
    }

    public void PlayDamageSound()
    {
        if (audioClips.Length > 12)
            audioSource.PlayOneShot(audioClips[12]);
    }
    public void PlayDashSound()
    {
        if (audioClips.Length > 13)
            audioSource.PlayOneShot(audioClips[13]);
    }

    public void PlayPredationSound()
    {
        if (audioClips.Length > 14)
            audioSource.PlayOneShot(audioClips[14]);
    }

    public void PlayNPCSound01()
    {
        if (audioClips.Length > 15)
            audioSource.PlayOneShot(audioClips[15]);
    }
    public void PlayNPCSound02()
    {
        if (audioClips.Length > 16)
            audioSource.PlayOneShot(audioClips[16]);
    }
    public void PlayBGMBattleSound()
    {
        if (audioClips.Length >30)
        {
            audioSource.clip = audioClips[17];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayDialogSound01()
    {
        if (audioClips.Length > 18)
            audioSource.PlayOneShot(audioClips[18]);
    }
    public void PlayDialogSound02()
    {
        if (audioClips.Length > 19)
            audioSource.PlayOneShot(audioClips[19]);
    }

    public void EquipmentSound()
    {
        if (audioClips.Length > 20)
            audioSource.PlayOneShot(audioClips[20]);
    }

    public void OpenSound()
    {
        if (audioClips.Length > 21)
            audioSource.PlayOneShot(audioClips[21]);
    }

    public void PlaySlimePredationSound()
    {
        if (audioClips.Length > 22)
            audioSource.PlayOneShot(audioClips[22]);
    }

    public void EquipmentGrowSound()
    {
        if (audioClips.Length > 23)
            audioSource.PlayOneShot(audioClips[23]);
    }
    public void PlayHumanPredationSound()
    {
        if (audioClips.Length > 24)
            audioSource.PlayOneShot(audioClips[24]);
    }

    public void PlayMegidoSound()
    {
        if (audioClips.Length > 25)
            audioSource.PlayOneShot(audioClips[25]);
    }

    public void SlimeShootingSound()
    {
        if (audioClips.Length > 26)
            audioSource.PlayOneShot(audioClips[26]);
    }
    public void PlayBleesingStartSound()
    {
        if (audioClips.Length > 27)
            audioSource.PlayOneShot(audioClips[27]);
    }
    public void PlaySpecialSound04()
    {
        if (audioClips.Length > 28)
            audioSource.PlayOneShot(audioClips[28]);
    }
    public void PlaySpecialSound05()
    {
        if (audioClips.Length > 29)
            audioSource.PlayOneShot(audioClips[29]);
    }
    public void PlayVictoryBGM()
    {
        if (audioClips.Length > 30)
        {
            audioSource.clip = audioClips[30];
            audioSource.loop = true;
            audioSource.Play();
        }
    }
    public void PlayResultPlayerSound()
    {
        if (audioClips.Length > 31)
            audioSource.PlayOneShot(audioClips[31]);
    }

    public void StopMusic() => audioSource.Stop();
    public void StopBackgroundMusic()
    {
        if (audioSource.isPlaying && audioSource.clip == audioClips[0])
            audioSource.Stop();
    }
}
