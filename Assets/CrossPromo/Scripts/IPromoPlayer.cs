namespace CrossPromo.Scripts
{
    public interface IPromoPlayer
    {
        /// <summary>
        /// Base API implementor
        /// </summary>
        public void Next();

        public void Previous();

        public void Pause();

        public void Resume();
    }
}