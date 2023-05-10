public class Elevator
{

    public int Id { get; private set; }

    public int CurrentFloor { get; set; }

    public readonly int MaxCapacity = 10; //expressed as total number of people

    public int CurrentCapacity = 0; //expressed as total number of people
    public int DestinationFloor { get; set; }
    public bool IsFull { get; set; }
    public List<Request> Requests { get; set; }

    public const int TopFloor = 10;
    public Elevator(int id)
    {
        Id = id;
        Requests = new List<Request>();
    }

    public Direction Direction
    {
        get
        {
            if (DestinationFloor > CurrentFloor)
            {
                return Direction.UP;
            }
            else if (CurrentFloor == TopFloor || DestinationFloor < CurrentFloor)
            {
                return Direction.DOWN;
            }
            else
            {
                return Direction.NEUTRAL;
            }
        }
    }


}
