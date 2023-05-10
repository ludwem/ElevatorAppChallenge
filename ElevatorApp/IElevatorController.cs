public interface IElevatorController
{
    bool AnyPendingRequests();
    public bool AnyRequestsInElevator(int elevatorId);
    int GetClosestDestinationFloor(Elevator elevatorDetail);
    List<Request> GetRequestersThatHaveReachedTheirDestinationFloor(Elevator elevatorDetail);
    int GetTotalNumberOfPendingRequests();
    List<Request> GetAllPendingRequests();
    void HandleRequests();
    bool IsElevatorFull(int elevatorId);
    void LoadElevator();
    void Pickup(int pickupFloor, int destinationFloor);
    void UpdateElevatorPosition(int elevatorId, int floorId, int newFloorId);
    void UpdatePendingRequestsInElevator(List<Request> requests);
    List<Elevator> UnloadElevators();
}
