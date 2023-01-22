using Godot;
using System;

public partial class GlobalTickTimer : Timer
{
	[Signal]
	public delegate void GlobalTickEventHandler(int tickCount, GlobalTickTimer globalTickTimer);

	public int TickCount { get; private set; }
	
	public GlobalTickTimer()
	{
		TickCount = 0;
	}

	public override void _Ready()
	{
		base._Ready();
		Timeout += OnTimerTimeout;
	}

	void OnTimerTimeout()
	{
		EmitSignal(SignalName.GlobalTick, TickCount, this);
		TickCount++;
	}
}
