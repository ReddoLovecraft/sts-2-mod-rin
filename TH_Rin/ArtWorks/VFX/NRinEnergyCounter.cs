using Godot;
using MegaCrit.Sts2.Core.Nodes.Combat;

public partial class NRinEnergyCounter : NEnergyCounter
{
	private Control? _rotationLayers;
	private Node2D? _energyVfxBack;
	private Node2D? _energyVfxFront;

	public override void _Ready()
	{
		base._Ready();
		_rotationLayers = GetNodeOrNull<Control>("%RotationLayers");
		_energyVfxBack = GetNodeOrNull<Node2D>("%EnergyVfxBack");
		_energyVfxFront = GetNodeOrNull<Node2D>("%EnergyVfxFront");
		CallDeferred(MethodName.ApplyCounterClockwiseTransform);
	}

	private void ApplyCounterClockwiseTransform()
	{
		if (_rotationLayers != null)
		{
			_rotationLayers.PivotOffset = _rotationLayers.Size / 2f;
			_rotationLayers.Scale = new Vector2(-Mathf.Abs(_rotationLayers.Scale.X), _rotationLayers.Scale.Y);
		}

		if (_energyVfxBack != null)
		{
			_energyVfxBack.Scale = new Vector2(-Mathf.Abs(_energyVfxBack.Scale.X), _energyVfxBack.Scale.Y);
		}

		if (_energyVfxFront != null)
		{
			_energyVfxFront.Scale = new Vector2(-Mathf.Abs(_energyVfxFront.Scale.X), _energyVfxFront.Scale.Y);
		}
	}
}
