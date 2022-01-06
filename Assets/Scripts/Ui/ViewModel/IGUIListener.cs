namespace GUI.ViewModel
{
    public interface IGUIListener
    {
        bool IsEnabled { get; }

        void UpdateGUI();
    }
}
