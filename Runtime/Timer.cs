public struct Timer
{
    private float elapsed;

    public bool Tick(float dt, float duration)
    {
        elapsed += dt;
        if (elapsed >= duration)
        {
            elapsed = 0f;
            return true;
        }
        return false;
    }

    public void Reset() => elapsed = 0f;
    public float Elapsed => elapsed;
}