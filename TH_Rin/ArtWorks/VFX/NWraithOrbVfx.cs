using System.Threading;
using System.Threading.Tasks;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;

namespace MegaCrit.Sts2.Core.Nodes.Vfx;

public partial class NWraithOrbVfx : Node2D
{
	private Line2D _orb;
	private Line2D _innerOrb;
	private Line2D _glow;
	private float _orbScale = 1.0f;
	private float _orbRotation = 0.0f;
	private float _pulsePhase = 0.0f;
	
	private CancellationTokenSource? _cts;
	private Vector2 _startPosition;
	private Vector2 _targetPosition;
	private float _duration = 0.5f;
	
	public static async Task Play(Creature from, Creature to)
	{
		if (TestMode.IsOn || NCombatRoom.Instance == null)
		{
			return;
		}
		
		NCreature? fromNode = NCombatRoom.Instance.GetCreatureNode(from);
		NCreature? toNode = NCombatRoom.Instance.GetCreatureNode(to);
		
		if (fromNode == null || toNode == null)
		{
			return;
		}
		
		NWraithOrbVfx vfx = new NWraithOrbVfx();
		vfx._startPosition = fromNode.VfxSpawnPosition;
		vfx._targetPosition = toNode.VfxSpawnPosition;
		vfx.Initialize();
		
		NCombatRoom.Instance.CombatVfxContainer.AddChildSafely(vfx);
		
		await vfx.PlaySequence();
	}
	
	private void Initialize()
	{
		_glow = new Line2D();
		_glow.Width = 3.0f;
		_glow.DefaultColor = new Color(0.9f, 0.5f, 1.0f, 0.4f);
		_glow.Closed = true;
		AddChild(_glow);
		_orb = new Line2D();
		_orb.Width = 4.0f;
		_orb.DefaultColor = new Color(0.8f, 0.4f, 1.0f, 0.8f);
		_orb.Closed = true;
		AddChild(_orb);
		_innerOrb = new Line2D();
		_innerOrb.Width = 2.0f;
		_innerOrb.DefaultColor = new Color(0.6f, 0.2f, 0.8f, 0.9f);
		_innerOrb.Closed = true;
		AddChild(_innerOrb);
		GlobalPosition = _startPosition;
		UpdateOrbShape();
	}
	
	private void UpdateOrbShape()
	{
		Vector2[] glowPoints = new Vector2[32];
		Vector2[] orbPoints = new Vector2[32];
		Vector2[] innerPoints = new Vector2[16];
		
		float glowRadius = 22.0f * _orbScale;
		float radius = 15.0f * _orbScale;
		float innerRadius = 8.0f * _orbScale;
		
		for (int i = 0; i < 32; i++)
		{
			float angle = (float)i / 32.0f * Mathf.Tau + _orbRotation;
			float r = glowRadius + Mathf.Sin(angle * 3.0f) * 3.0f;
			glowPoints[i] = new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
			
			angle = (float)i / 32.0f * Mathf.Tau + _orbRotation;
			r = radius + Mathf.Sin(angle * 3.0f) * 2.0f;
			orbPoints[i] = new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
		}
		
		for (int i = 0; i < 16; i++)
		{
			float angle = (float)i / 16.0f * Mathf.Tau - _orbRotation;
			float r = innerRadius + Mathf.Cos(angle * 4.0f) * 1.5f;
			innerPoints[i] = new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
		}
		
		_glow.ClearPoints();
		_orb.ClearPoints();
		_innerOrb.ClearPoints();
		
		foreach (Vector2 p in glowPoints)
		{
			_glow.AddPoint(p);
		}
		
		foreach (Vector2 p in orbPoints)
		{
			_orb.AddPoint(p);
		}
		
		foreach (Vector2 p in innerPoints)
		{
			_innerOrb.AddPoint(p);
		}
	}
	
	private async Task PlaySequence()
	{
		_cts = new CancellationTokenSource();
		double timer = 0.0;
		while (timer < (double)_duration && !_cts.IsCancellationRequested)
		{
			float weight = (float)timer / _duration;
			float curveHeight = 60.0f * Mathf.Sin(weight * Mathf.Pi);
			Vector2 curvedPosition = _startPosition.Lerp(_targetPosition, weight);
			curvedPosition.Y -= curveHeight;
			GlobalPosition = curvedPosition;
			_pulsePhase += (float)GetProcessDeltaTime() * 8.0f;
			_orbScale = 1.0f + Mathf.Sin(_pulsePhase) * 0.2f;
			_orbRotation += (float)GetProcessDeltaTime() * 3.0f;
			
			UpdateOrbShape();
			
			timer += GetProcessDeltaTime();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
		
		if (!_cts.IsCancellationRequested)
		{
			await PlayExplosion();
			await Cmd.Wait(0.3f, _cts.Token);
			
			this.QueueFreeSafely();
		}
	}
	
	private async Task PlayExplosion()
	{
		Line2D[] waves = new Line2D[3];
		float[] waveSizes = { 10.0f, 15.0f, 20.0f };
		Color[] waveColors = { 
			new Color(1.0f, 0.6f, 1.0f, 0.8f), 
			new Color(0.8f, 0.4f, 0.9f, 0.6f), 
			new Color(0.6f, 0.2f, 0.8f, 0.4f) 
		};
		
		for (int w = 0; w < 3; w++)
		{
			waves[w] = new Line2D();
			waves[w].Width = 3.0f;
			waves[w].DefaultColor = waveColors[w];
			waves[w].Closed = true;
			AddChild(waves[w]);
		}
		_orb.Visible = false;
		_innerOrb.Visible = false;
		_glow.Visible = false;

		double explodeTimer = 0.0;
		float explodeDuration = 0.4f;
		
		while (explodeTimer < explodeDuration && !_cts.IsCancellationRequested)
		{
			float t = (float)explodeTimer / explodeDuration;
			
			for (int w = 0; w < 3; w++)
			{
				float delay = w * 0.08f;
				float localT = Mathf.Clamp((t - delay) / (1.0f - delay), 0.0f, 1.0f);
				
				float radius = waveSizes[w] + localT * 40.0f;
				float alpha = 1.0f - localT;
				
				waves[w].DefaultColor = waveColors[w] * new Color(1, 1, 1, alpha);
				
				Vector2[] points = new Vector2[24];
				for (int i = 0; i < 24; i++)
				{
					float angle = (float)i / 24.0f * Mathf.Tau;
					float r = radius + Mathf.Sin(angle * 5.0f + t * 10.0f) * 3.0f;
					points[i] = new Vector2(Mathf.Cos(angle) * r, Mathf.Sin(angle) * r);
				}
				
				waves[w].ClearPoints();
				foreach (Vector2 p in points)
				{
					waves[w].AddPoint(p);
				}
			}
			
			explodeTimer += GetProcessDeltaTime();
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}
	
	public override void _ExitTree()
	{
		_cts?.Cancel();
		_cts?.Dispose();
	}
}
