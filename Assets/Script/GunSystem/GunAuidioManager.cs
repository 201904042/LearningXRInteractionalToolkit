using UnityEngine;

public class GunAuidioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource fireSource;   //»ç°Ý(ÀåÀüO) , »ç°Ý(ÀåÀüX)
    [SerializeField] private AudioSource MagSource;  //ÅºÃ¢³¢¿ò, ÅºÃ¢»­
    [SerializeField] private AudioSource slideSource; //½½¶óÀÌ´õ ´ç±è

    [Header("Audio Clips")]
    public AudioClip fireClip;
    public AudioClip emptyClip;
    public AudioClip insertMagClip;
    public AudioClip ejectMagClip;
    public AudioClip slideClip;

    public void PlayFireSound() => Play(fireSource, fireClip);
    public void PlayEmptySound() => Play(fireSource, emptyClip);
    public void PlayInsertMagSound() => Play(MagSource, insertMagClip);
    public void PlayEjectMagSound() => Play(MagSource, ejectMagClip);
    public void PlaySlideSound() => Play(slideSource, slideClip);

    private void Play(AudioSource source, AudioClip clip)
    {
        if (source != null && clip != null)
        {
            source.clip = clip;
            source.Play();
        }
    }
}
