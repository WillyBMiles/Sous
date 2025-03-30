using DG.Tweening;
using UnityEngine;

public class OutputAssesser : MonoBehaviour
{
    [SerializeField]
    float minimumVolume;
    public static float MinVolume;
    [SerializeField]
    public AudioSource splash;

    int numberOfSlices;
    public int NumberOfSlices => numberOfSlices;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MinVolume = minimumVolume;
    }

    // Update is called once per frame
    void Update()
    {
        didOnethisFrame = false;
    }

    bool didOnethisFrame = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Sliceable>(out Sliceable sliceable))
        {
            float volume = sliceable.volume;
            Debug.Log(volume);

            if (volume < minimumVolume)
            {
                Debug.Log("Slice too small...");
            }
            else
            {
                numberOfSlices++;
                if (!didOnethisFrame)
                {
                    AudioSource s = Instantiate(splash);
                    s.volume = .1f * sliceable.volume / minimumVolume;
                    s.pitch = Random.Range(.9f, 1.1f);
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(.75f);
                    sequence.AppendCallback(() => Destroy(s.gameObject));
                    s.Play();
                    didOnethisFrame = true;
                }
                

            }


            Destroy(other.gameObject);
        }   
    }
}
