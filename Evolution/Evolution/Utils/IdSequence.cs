namespace Ivankarez.Evolution.Utils
{
    public class IdSequence
    {
        public long NextId { get; private set; }

        public IdSequence(long startValue = 0L)
        {
            NextId = startValue;
        }

        public long Next()
        {
            return NextId++;
        }
    }
}
