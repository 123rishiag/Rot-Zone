namespace Game.Utility
{
    public interface IState<T> where T : class
    {
        public T Owner { get; set; }
        public void OnStateEnter();
        public void Update();
        public void FixedUpdate();
        public void LateUpdate();
        public void OnStateExit();
    }
}