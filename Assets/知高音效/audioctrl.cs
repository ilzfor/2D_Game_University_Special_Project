using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioctrl : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public List<AudioClip> specialSounds = new List<AudioClip>(); // 新增一個 List 來存儲特定音效
    private float timer = 0f;
    private float soundInterval = 3f; // 每 3 秒播放一次特定音效

    void Start()
    {
        // 確保背景音樂不為空並且啟用循環播放
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    void Update()
    {
        // 如果背景音樂存在，並且正在播放，則每過 soundInterval 秒播放特定音效
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            timer += Time.deltaTime;
            if (timer >= soundInterval)
            {
                PlayRandomSpecialSound(); // 更改此處來播放隨機的特定音效
                timer = 0f; // 重置計時器
            }
        }
    }

    void PlayRandomSpecialSound()
    {
        // 確保特定音效列表和 AudioSource 都存在
        if (specialSounds.Count > 0 && backgroundMusic != null)
        {
            int randomIndex = Random.Range(0, specialSounds.Count);
            backgroundMusic.PlayOneShot(specialSounds[randomIndex]);
        }
    }
}
