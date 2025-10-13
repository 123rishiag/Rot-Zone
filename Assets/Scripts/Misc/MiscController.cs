using System.Collections;
using UnityEngine;

namespace Game.Misc
{
    public class MiscController
    {
        // Private Variables
        private MiscView miscView;

        private float defaultFixedDeltaTime;
        private WaitForSecondsRealtime slowDownWaitForSecondsRealtimeYield;
        private IEnumerator slowDownRoutine;

        public MiscController(MiscView _miscView)
        {
            miscView = Object.Instantiate(_miscView).GetComponent<MiscView>();

            defaultFixedDeltaTime = Time.fixedDeltaTime;
            slowDownWaitForSecondsRealtimeYield = new WaitForSecondsRealtime(1f);
            slowDownRoutine = SlowDownRoutine();
        }

        public Coroutine StartManualCoroutine(IEnumerator _coroutine) => miscView.StartCoroutine(_coroutine);
        public void StopManualCoroutine(Coroutine _coroutineHandle) => miscView.StopCoroutine(_coroutineHandle);

        public void StartSlowDownCoroutine(float _seconds)
        {
            slowDownWaitForSecondsRealtimeYield = new WaitForSecondsRealtime(_seconds);
            slowDownRoutine = SlowDownRoutine();
            miscView.StopCoroutine(slowDownRoutine);
            StartManualCoroutine(slowDownRoutine);
        }

        public IEnumerator SlowDownRoutine()
        {
            Time.timeScale = 0.3f;
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            yield return slowDownWaitForSecondsRealtimeYield;

            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime * 1f;
        }
    }
}