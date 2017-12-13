using UnityEngine;

//ObjectPoolObject
public class SkillEff: MonoBehaviour
{
    AudioSource source;
    AudioSource Source
    {
        get
        {
            if (source == null)
            {
                source = GetComponent<AudioSource>();
                if (source == null)
                    source = gameObject.AddComponent<AudioSource>();
            }
            return source;
        }
    }



    public void SetAudio(string key)
    {
        if (key.IsNull()) return;
        Source.clip= Manage.Instance.AB.GetGame<AudioClip>(AssetbundleEnum.SkillAudio, key);
        Source.Play();
    }

    public void Play()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

}