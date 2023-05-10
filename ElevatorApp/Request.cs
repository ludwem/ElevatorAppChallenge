public class Request
{
    public Guid Id { get; set; }
    public int DestinationFloor { get; private set; }
    public Direction Direction => WaitingFloor < DestinationFloor ? Direction.UP : Direction.DOWN;
    public int WaitingFloor { get; private set; }
    public Request(int waitingFloor, int destinationFloor)
    {
        WaitingFloor = waitingFloor;
        DestinationFloor = destinationFloor;
        Id = Guid.NewGuid();
    }
}
