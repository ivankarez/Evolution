namespace Ivankarez.Evolution.Utils
{
    public class IdSequence
    {
        public long NextId { get; private set; }

        public IdSequence(long startValue = 0l)
        {
            NextId = startValue;
        }

        public long Next()
        {
            return NextId++;
        }
    }
}
