using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        const int numberOfFloors = 20;
        const int numberOfElevators = 10;
        const int numberOfRequests = 20; 
                                        
        var pickupCount = 0;
        var requestsCount = 1;
        var random = new Random();
        IElevatorController elevatorController = new ElevatorController(numberOfElevators);
        while (pickupCount < numberOfRequests)
        {
            //Calling the elevators to a specific floor
            var originatingFloor = random.Next(1, numberOfFloors + 1);
            var destinationFloor = random.Next(1, numberOfFloors + 1);

            if (originatingFloor != destinationFloor)
            {
                DisplayRequesterCurrentAndDestinationFloorStatus(requestsCount, originatingFloor, destinationFloor);

                elevatorController.Pickup(originatingFloor, destinationFloor);
                pickupCount++;
                requestsCount++;
            }
        }
        Console.WriteLine("");

        while(elevatorController.AnyPendingRequests())
        {
            DisplayPendingPickups(elevatorController);

            elevatorController.HandleRequests();
        }
        DisplayPendingPickups(elevatorController);
        Console.ReadLine();
    }

    public static void DisplayRequesterCurrentAndDestinationFloorStatus(int requesterCount, int originatingFloor, int destinationFloor)
    {
        Console.WriteLine("REQUESTER [{0}] IS WAITING ON FLOOR [{1}] AND THE DESTINATIOIN FLOOR [{2}].", requesterCount, originatingFloor, destinationFloor);
    }

    public static void DisplayPendingPickups(IElevatorController elevatorCollection)
    {
        Console.WriteLine("THERE IS CURRENTLY [{0}] REQUESTERS WAITING.", elevatorCollection.GetTotalNumberOfPendingRequests());
        Console.WriteLine("");
    }
}
