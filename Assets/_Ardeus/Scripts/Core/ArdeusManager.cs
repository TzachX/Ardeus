namespace Ardeus.Core
{
    public class ArdeusManager
    {
        public static ArdeusManager Instance;

        public ArdeusManager()
        {
            if (Instance != null ) 
            {
                return;
            }

            Instance = this;
        }
    }
}