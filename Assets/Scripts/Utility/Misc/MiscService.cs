using System.Collections;
using UnityEngine;

namespace Game.Utility
{
    public class MiscService : MonoBehaviour
    {
        public Coroutine StartManualCoroutine(IEnumerator _coroutine) => StartCoroutine(_coroutine);

        public void StopManualCoroutine(Coroutine _coroutineHandle) => StopCoroutine(_coroutineHandle);
    }
}