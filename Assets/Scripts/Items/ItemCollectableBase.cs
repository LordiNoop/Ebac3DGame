using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Itens
{
    public class ItemCollectableBase : MonoBehaviour
    {
        public ItemType itemType;

        public string compareTag = "Player";
        public ParticleSystem particleSyst;
        public float timeToHide = 1f;
        public GameObject graphicItem;

        protected bool _collected = false;

        public Collider coll;

        [Header("Sounds")]
        public AudioSource audioSource;

        private void Awake()
        {
            //if (particleSystem != null) particleSystem.transform.SetParent(null);
        }

        private void Update()
        {
            if (GetComponent<Magnetic>() != null)
            {
                coll.enabled = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag(compareTag))
            {
                Collect();
            }
        }

        protected virtual void Collect()
        {
            if (graphicItem != null) graphicItem.SetActive(false);
            Invoke(nameof(HideObject), timeToHide);
            if (!_collected) OnCollect();
        }

        private void HideObject()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnCollect()
        {
            if (particleSyst != null)
            {
                particleSyst.Play();
            }
            if (audioSource != null)
            {
                audioSource.Play();
            }
            _collected = true;

            ItemManager.Instance.AddByType(itemType);
        }
    }
}
