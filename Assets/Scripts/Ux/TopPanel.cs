using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CgpEditor.Ux
{
    public class TopPanel : MonoBehaviour
    {
        private float _targetWidth;
        private float _targetHeight;
        private RectTransform _rect;
        private List<Coroutine> _currentCoroutines = new List<Coroutine>();
        
        private void Start()
        {
            _rect = transform as RectTransform;
            _targetWidth = 1;
            _targetHeight = 1;
        }

        private IEnumerator DoHeight()
        {
            while (!Mathf.Approximately(_rect.localScale.y, _targetHeight))
            {
                _rect.localScale = Vector3.MoveTowards(_rect.localScale, new Vector3(_rect.localScale.x, _targetHeight), Time.deltaTime * 25);
                yield return null;
            }
        }
        
        private IEnumerator DoWidth()
        {
            while (!Mathf.Approximately(_rect.localScale.x, _targetWidth))
            {
                _rect.localScale = Vector3.MoveTowards(_rect.localScale, new Vector3(_targetWidth, _rect.localScale.y), Time.deltaTime * 25);
                yield return null;
            }
        }

        private void CancelCoroutines()
        {
            foreach (Coroutine coroutine in _currentCoroutines)
            {
                if (coroutine == null)
                {
                    continue;
                }
                StopCoroutine(coroutine);
            }
            _currentCoroutines.Clear();
        }
        
        public void Enable()
        {
            _targetHeight = 1;
            _targetWidth = 1;
            CancelCoroutines();
            _currentCoroutines.Add(StartCoroutine(DoCoroutinesConsecutively(DoWidth(), DoHeight())));
        }

        public void Disable()
        {
            _targetHeight = 0.1f;
            _targetWidth = 0f;
            CancelCoroutines();
            _currentCoroutines.Add(StartCoroutine(DoCoroutinesConsecutively(DoHeight(), DoWidth())));
        }
        
        private IEnumerator DoCoroutinesConsecutively(params IEnumerator[] enumerators)
        {
            foreach (IEnumerator enumerator in enumerators)
            {
                Coroutine routine = StartCoroutine(enumerator);
                _currentCoroutines.Add(routine);
                yield return routine;
            }
        }
    }
}