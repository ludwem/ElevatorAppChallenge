namespace UnitTests
{
    public class IsElevatorFull_False
    {
        [Fact]
        public void Is_Elevator_Full_False()
        {
            // Arrange  
            bool expectedValue = false;

            // Act 
            IElevatorController elevatorController = new ElevatorController(1);
            bool isElevatorFull = elevatorController.IsElevatorFull(1);

            //Assert  
            Assert.Equal(expectedValue, isElevatorFull);
        }
    }
}