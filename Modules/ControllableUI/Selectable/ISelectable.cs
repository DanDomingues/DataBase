

public interface ISelectable
{
    void Select(bool value);
    void RunButtonAction();

    UnityEngine.GameObject gameObject { get; }
}
