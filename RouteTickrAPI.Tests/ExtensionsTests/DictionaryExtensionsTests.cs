using RouteTickrAPI.Extensions;
using ArgumentNullException = System.ArgumentNullException;

namespace RouteTickrAPI.Tests.ExtensionsTests;

[TestFixture]
public class DictionaryExtensionsTests
{
    [Test]
    public void TryIncrementCount_ReturnsFalse_WhenDictionaryIsNull()
    {
        //Arrange
        Dictionary<string, int> dict = null;
        
        //Assert
        Assert.Throws<ArgumentNullException>(() => dict.IncrementCount("test"));
    }

    [Test]
    public void TryIncrementCount_AddsKeyWithValueOne_WhenKeyNotPresent()
    {
        //Arrange
        var dict = new Dictionary<string, int>();
        //Act
        dict.IncrementCount("test");
        //Assert
        Assert.Multiple(() =>
        {
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
        dict.IncrementCount("test");
        //Assert
        Assert.Multiple(() =>
        {
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
        dict.IncrementCount("test");
        dict.IncrementCount("test");
        dict.IncrementCount("test");
        //Assert
        Assert.That(dict["test"], Is.EqualTo(3));
    }

    [Test]
    public void TryIncrementCount_IncrementsOneValue_WhenDictHasMultipleKeys()
    {
        //Arrange
        var dict = new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };
        //Act
        dict.IncrementCount("b");
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(dict["a"], Is.EqualTo(1));
            Assert.That(dict["b"], Is.EqualTo(3));
        });
    }
}