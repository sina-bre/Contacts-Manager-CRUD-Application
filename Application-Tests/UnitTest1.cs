namespace Application_Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            MyMath myMath = new MyMath();
            int input1 = 10, input2 = 5;
            int exeptedValue = 15;

            //Act
            int actualValue = myMath.Add(input1, input2);

            //Assert
            Assert.Equal(exeptedValue, actualValue);
        }
    }
}