using UnityEngine;

public class MiscSoundEffects : MonoBehaviour
{
    public AudioClip drinkClip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string soundName)
    {
        if (audioSource.isPlaying)
            return;
        switch (soundName.ToLower())
        {
            case "drink":
                audioSource.clip = drinkClip;
                break;
            default:
                return;
        }
        audioSource.Play();
    }
}