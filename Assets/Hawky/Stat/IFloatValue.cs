namespace Hawky.Stat
{
    public interface IFloatValue
    {
        float Value { get; }
    }

    public class FloatValue : IFloatValue
    {
        public float Value { get; }

        public FloatValue(float value)
        {
            Value = value;
        }
    }
    
}