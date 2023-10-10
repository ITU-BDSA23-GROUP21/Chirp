namespace Chirp.Infrastructure_test;

public class EnvironmentVariableFixture
{
    public EnvironmentVariableFixture()
    {
        Environment.SetEnvironmentVariable("CHIRPDBPATH", "./../../../../../data/chirptest.db");
    }
}

[CollectionDefinition("Environment Variable")]
public class EnvironmentVariableCollection : ICollectionFixture<EnvironmentVariableFixture>
{
    
}