public interface IFocusable
{
    float ZoomAmountOnFocus { get; set; }
    void OnFocus();
    void OnUnfocus();
}