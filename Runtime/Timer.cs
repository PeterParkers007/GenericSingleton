public struct Timer
{
    public float duration;
    private float elapsed;
    private bool completed;

    public Timer(float duration)
    {
        this.duration = duration;
        elapsed = 0f;
        completed = false;
    }

    public bool Tick(float dt)
    {
        if (completed) return true;
        elapsed += dt;
        if (elapsed >= duration)
        {
            elapsed = duration;
            completed = true;
            return true;
        }
        return false;
    }

    public float Progress => (duration > 0f) ? (elapsed / duration) : 1f;
    public bool Completed => completed;

    public void Reset()
    {
        elapsed = 0f;
        completed = false;
    }
}