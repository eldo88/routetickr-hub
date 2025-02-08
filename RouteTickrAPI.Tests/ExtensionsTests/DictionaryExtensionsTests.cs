using RouteTickrAPI.Extensions;

namespace RouteTickrAPI.Tests.ExtensionsTests;

[TestFixture]
public class DictionaryExtensionsTests
{
    [Test]
    public void TryIncrementCount_ReturnsFalse_WhenDictionaryIsNull()
    {
        //Arrange
        Dictionary<string, int> dict = null;
        //Act
        var result = dict.TryIncrementCount("test");
        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void TryIncrementCount_AddsKeyWithValueOne_WhenKeyNotPresent()
    {
        //Arrange
        var dict = new Dictionary<string, int>();
        //Act
        var result = dict.TryIncrementCount("test");
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(dict.ContainsKey("test"), Is.True);
            Assert.That(dict["test"], Is.EqualTo(1));
        });
    }
    
    [Test]
    public void TryIncrementCount_UpdatesIncrement_WhenKeyExists()
    {
        //Arrange
        var dict = new Dictionary<string, int> { { "test", 1 } };
        //Act
        var result = dict.TryIncrementCount("test");
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(dict.ContainsKey("test"), Is.True);
            Assert.That(dict["test"], Is.EqualTo(2));
        });
    }

    [Test]
    public void TryIncrementCount_IncrementsValue_WhenKeyPassedMultipleTimes()
    {
        //Arrange
        var dict = new Dictionary<string, int>();
        //Act
        dict.TryIncrementCount("test");
        dict.TryIncrementCount("test");
        dict.TryIncrementCount("test");
        //Assert
        Assert.That(dict["test"], Is.EqualTo(3));
    }

    [Test]
    public void TryIncrementCount_IncrementsOneValue_WhenDictHasMultipleKeys()
    {
        //Arrange
        var dict = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };
        //Act
        var result = dict.TryIncrementCount("b");
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(dict["a"], Is.EqualTo(1));
            Assert.That(dict["b"], Is.EqualTo(3));
        });
    }
}