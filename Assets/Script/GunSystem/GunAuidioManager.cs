using UnityEngine;

public class GunAuidioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource fireSource;   //���(����O) , ���(����X)
    [SerializeField] private AudioSource MagSource;  //źâ����, źâ��
    [SerializeField] private AudioSource slideSource; //�����̴� ���

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
