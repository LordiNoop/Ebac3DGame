using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Ebac.Core.Singleton;

public class EffectsManager : Singleton<EffectsManager>
{
    public PostProcessVolume processVolume;
    [SerializeField] private Vignette _vignette;

    public float duration = 1.0f;

    public ColorParameter standardColor;

    [NaughtyAttributes.Button]
    public void ChangeVignette()
    {
        StartCoroutine(FlashColorVignette());
    }

    IEnumerator FlashColorVignette()
    {
        Vignette tmp;

        if (processVolume.profile.TryGetSettings<Vignette>(out tmp))
        {
            _vignette = tmp;
        }

        ColorParameter c = new ColorParameter();

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;

            c.value = Color.Lerp(standardColor, Color.red, time / duration);

            _vignette.color.Override(c);

            yield return new WaitForEndOfFrame();
        }

        time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            c.value = Color.Lerp(Color.red, standardColor, time / duration);

            _vignette.color.Override(c);

            yield return new WaitForEndOfFrame();
        }

    }
}
