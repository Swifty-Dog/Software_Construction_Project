using Moq;

public static class MyContextMockHelper
{
    public static Mock<MyContext> CreateMockContext<T>(List<T> data) where T : class
    {
        var mockSet = DbSetMockHelper.CreateMockDbSet(data);
        var mockContext = new Mock<MyContext>();
        mockContext.Setup(c => c.Set<T>()).Returns(mockSet.Object);
        return mockContext;
    }
}
