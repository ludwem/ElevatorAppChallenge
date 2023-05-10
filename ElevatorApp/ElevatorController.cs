

public class ElevatorController : IElevatorController
{
    public List<Elevator> Elevators { get; set; }

    public List<Request> PendingRequests { get; set; }
    public List<int>? BusyElevatorsIds { get; set; }

    public ElevatorController(int numberOfElevators)
    {
        PendingRequests = new List<Request>();
        Elevators = Enumerable.Range(1, numberOfElevators)
            .Select(elevatorId => new Elevator(elevatorId))
            .ToList();
    }

    public List<Request> GetAllPendingRequests()
    {
        return PendingRequests;
    }

    public bool AnyPendingRequests()
    {
        return PendingRequests.Any();
    }

    public static void DisplayElevatorStatus(Elevator elevator)
    {
        Console.WriteLine("ELEVATOR: [{0}] CURRENT DIRECTION IS: [{1}]. CURRENT FLOOR IS: [{2}]. TOTAL PASSENGERS IN THE ELEVATOR ARE: [{3}].", elevator.Id, elevator.Direction, elevator.CurrentFloor, elevator.Requests.Count);
    }

    public bool AnyRequestsInElevator(int elevatorId)
    {
        return Elevators.Any(elevator => elevator.Requests.Count > 0);
    }

    public int GetTotalNumberOfPendingRequests()
    {
        return GetAllPendingRequests().Count;
    }
    
    public bool IsElevatorFull(int elevatorId)
    {
        return Elevators.Any(elevator => elevator.Id == elevatorId && elevator.Requests.Count == elevator.MaxCapacity);
    }

    public void UpdatePendingRequestsInElevator(List<Request> requesters)
    {
       PendingRequests = PendingRequests.Where(requests => requesters.All(r => r.Id != requests.Id)).ToList();
    }

    public List<Request> GetRequestersThatHaveReachedTheirDestinationFloor(Elevator elevatorDetail)
    {
        return elevatorDetail.Requests.Where(requester => requester.DestinationFloor == elevatorDetail.CurrentFloor).ToList();
    }

    public List<Elevator> UnloadElevators()
    {
        return Elevators.Select(elevatorDetail =>
        {
            //If they have reached their destination, they should dropoff
            var requestersToDropOff = GetRequestersThatHaveReachedTheirDestinationFloor(elevatorDetail);

            //If there is anyone who needs to dropoff
            if (requestersToDropOff.Any())
            {
                Console.WriteLine("ELEVATOR [{0}] IS NOW DROPPING OFF [{1}] REQUESTER(S) AT FLOOR [{2}]...", elevatorDetail.Id, requestersToDropOff.Count, elevatorDetail.CurrentFloor);
                elevatorDetail.Requests.RemoveAll(requester => requester.DestinationFloor == elevatorDetail.CurrentFloor);
            }
            return elevatorDetail;
        }).ToList();
    }

    public void LoadElevator()
    {
        // Take off requesters to available elevators
        //Determine the requester original floor and direction
        PendingRequests.GroupBy(request => new
        {
            request.WaitingFloor,
            request.Direction
        }).ToList().ForEach(waitingFloor =>
        {
            //Deem an elevator available only if it meets all the below conditions.
            var availableElevator = Elevators.FirstOrDefault(elevatorDetail => (elevatorDetail.CurrentFloor == waitingFloor.Key.WaitingFloor) && 
            (elevatorDetail.Direction == waitingFloor.Key.Direction) ||
            (!elevatorDetail.Requests.Any()));

            if (availableElevator != null)
            {
                BusyElevatorsIds?.Add(availableElevator.Id);
                var requestsToBeAdded = waitingFloor.ToList();

                UpdateElevator(availableElevator.Id, elevatorDetail =>
                {
                    foreach (var request in requestsToBeAdded)
                    {
                        //The elevator should not exceed its maximum capacity
                        if (!IsElevatorFull(availableElevator.Id))
                        {
                            elevatorDetail.Requests.Add(request);
                            Console.WriteLine("ELEVATOR [{0}] CURRENT CAPACITY IS [{1}].", availableElevator.Id, availableElevator.Requests.Count);
                        }
                        else
                        {
                            Console.WriteLine("ELEVATOR [{0}] IS NOW FULL WITH [{1}] REQUESTS.", availableElevator.Id, availableElevator.Requests.Count);
                        }
                    }
                });
                UpdatePendingRequestsInElevator(requestsToBeAdded);
            }
        });
    }

    public int GetClosestDestinationFloor(Elevator elevatorDetail)
    {
        //Given a pool of elevators, the program should send the nearest available elevator to that person.
       return elevatorDetail.Requests.OrderBy(request => Math.Abs(request.DestinationFloor - elevatorDetail.CurrentFloor)).First().DestinationFloor;
    }

    public void HandleRequests()
    {
        BusyElevatorsIds = new List<int>();

        LoadElevator();

        foreach (var elevatorDetail in Elevators)
        {
            var isBusy = BusyElevatorsIds.Contains(elevatorDetail.Id);
            int destinationFloor;
            if (elevatorDetail.Requests.Any())
            {
                destinationFloor = GetClosestDestinationFloor(elevatorDetail);
            }
            else if (elevatorDetail.CurrentFloor == elevatorDetail.DestinationFloor && PendingRequests.Any())
            {
                destinationFloor = PendingRequests.GroupBy(requester => new
                {
                    requester.WaitingFloor
                }).OrderBy(group => group.Count()).First().Key.WaitingFloor;
            }
            else
            {
                destinationFloor = elevatorDetail.DestinationFloor;
            }

            var floorId = isBusy ? elevatorDetail.CurrentFloor : elevatorDetail.CurrentFloor + (destinationFloor > elevatorDetail.CurrentFloor ? 1 : -1);
            UpdateElevatorPosition(elevatorDetail.Id, floorId, destinationFloor);
            DisplayElevatorStatus(elevatorDetail);
        };

        Elevators = UnloadElevators();
    }

    public void UpdateElevatorPosition(int elevatorId, int floorId, int newFloorId)
    {
        UpdateElevator(elevatorId, elevatorDetail =>
        {
            elevatorDetail.CurrentFloor = floorId;
            elevatorDetail.DestinationFloor = newFloorId;
        });
    }

    public void Pickup(int pickupFloor, int destinationFloor)
    {
        PendingRequests.Add(new Request(pickupFloor, destinationFloor));
    }

    private void UpdateElevator(int elevatorId, Action<Elevator> updateData)
    {
        Elevators = Elevators.Select(elevatorDetail =>
        {
            if (elevatorDetail.Id == elevatorId)
            {
                updateData(elevatorDetail);
            }
            return elevatorDetail;
        }).ToList();
    }
}
