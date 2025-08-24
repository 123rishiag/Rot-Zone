namespace Game.Misc
{
    public class MiscService
    {
        // Private Variables
        private MiscController miscController;

        public MiscService(MiscView _miscView)
        {
            miscController = new MiscController(_miscView);
        }

        // Getters
        public MiscController GetController() => miscController;
    }
}