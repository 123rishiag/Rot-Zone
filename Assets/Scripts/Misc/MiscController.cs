using System.Collections;
using UnityEngine;

namespace Game.Misc
{
    public class MiscController
    {
        // Private Variables
        private MiscView miscView;

        public MiscController(MiscView _miscView)
        {
            miscView = Object.Instantiate(_miscView).GetComponent<MiscView>();
        }

        public Coroutine StartManualCoroutine(IEnumerator _coroutine) => miscView.StartCoroutine(_coroutine);
        public void StopManualCoroutine(Coroutine _coroutineHandle) => miscView.StopCoroutine(_coroutineHandle);
    }
}