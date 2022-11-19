using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace tests.utils;

public class TestConfiguration : IConfiguration
{
    private JObject configuration;
    
    public TestConfiguration()
    {
        configuration = new JObject()
        {
            ["DB:host"] = Globals.Host,
            ["DB:database"] = Globals.Database,
            ["DB:username"] = Globals.Username,
            ["DB:password"] = Globals.Password,
        };
    }

    public IEnumerable<IConfigurationSection> GetChildren()
    {
        throw new System.NotImplementedException();
    }

    public IChangeToken GetReloadToken()
    {
        throw new System.NotImplementedException();
    }

    public IConfigurationSection GetSection(string key)
    {
        throw new System.NotImplementedException();
    }

    public string this[string key]
    {
        get => (string)configuration[key];
        set => configuration.Add(key, value);
    }
}