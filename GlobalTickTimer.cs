using Godot;

namespace NewSuperTD;

public partial class GlobalTickTimer : Timer
{
	[Signal]
	public delegate void GlobalTickEventHandler(int tickCount, GlobalTickTimer globalTickTimer);

	public GlobalTickTimer()
	{
		TickCount = 0;
	}

	public int TickCount { get; private set; }

	public override void _Ready()
	{
		base._Ready();
		Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		EmitSignal(SignalName.GlobalTick, TickCount, this);
		TickCount++;
	}

}