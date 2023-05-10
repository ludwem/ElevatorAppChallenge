using ElevatorApp;

namespace UnitTests
{
    public class GetRequestersThatHaveReachedTheirDestinationFloor_False
    {
        [Fact]
        public void Get_Requesters_That_Have_Reached_Their_Destination_Floor_False()
        {
            // Arrange  
            Request request = new(1, 2);
            List<Request> expectedValue = new()
            {
                request
            };

            // Act 
            Elevator elevator = new(1)
            {
                CurrentFloor = 2,
                DestinationFloor = 2
            };
            elevator.Requests.Add(request);
            IElevatorController elevatorController = new ElevatorController(1);
            List<Request> requesters = elevatorController.GetRequestersThatHaveReachedTheirDestinationFloor(elevator);

            //Assert  
            Assert.Equal(expectedValue, requesters);
        }
    }
}