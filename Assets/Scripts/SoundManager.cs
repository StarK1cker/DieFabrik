using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource audioSource;
    public AudioClip placeMinerSound;
    public AudioClip sellSound;
    public AudioClip saveSound;
    public AudioClip placeFliessbandSound;
    public AudioClip UI;

    void Awake()
    {
        Instance = this;
    }

    public void PlayMinerPlace()
    {
        audioSource.PlayOneShot(placeMinerSound);
    }
    public void PlaySell()
    {
        audioSource.PlayOneShot(sellSound);
    }

    public void PlaySave()
    {
        audioSource.PlayOneShot(saveSound);
    }

    public void PlayFliessbandPlace()
    {
        audioSource.PlayOneShot(placeFliessbandSound);
    }

    public void PlayUI()
    {
        audioSource.PlayOneShot(UI);
    }
}
