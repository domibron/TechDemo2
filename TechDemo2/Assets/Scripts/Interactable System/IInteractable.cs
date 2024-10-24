public interface IInteractable
{
	public string InteractionToolTipDescriptive { get; }

	public void InteractWithObject();

	public void OnSelected();
	public void OnDeselected();
}
