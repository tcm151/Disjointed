namespace Disjointed.Environment
{
    public interface IUnlockable
    {
        public void Lock();
        public void Unlock();
        public void ToggleLock();

        public void Open();
        public void Close();
    }
}