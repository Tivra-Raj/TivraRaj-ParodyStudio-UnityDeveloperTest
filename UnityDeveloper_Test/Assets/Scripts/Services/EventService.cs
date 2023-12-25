using Event;

namespace Services
{
    public class EventService
    {
        private static EventService instance;
        public static EventService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventService();
                }
                return instance;
            }
        }

        public EventController<int> OnPoinCubeColletedEvent { get; private set; }

        public EventService()
        {
            OnPoinCubeColletedEvent = new EventController<int>();
        }
    }
}