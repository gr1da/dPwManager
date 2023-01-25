using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ddbase;

namespace Ddbase.Tests;

[TestClass]
public class DdbaseTests {
    [TestMethod]
    public void WhenNotExists_NewDbWillBeEmpty() {
        var db = newBlankDummy();
        Assert.AreEqual(0, db.CachedSize);
    }

    private Database newBlankDummy() {
        return new Database(
            new InMemBackingStore(), 
            new Dictionary<string, List<Database.VersionedValue>> { });
    }
}

class InMemBackingStore : Database.IBackingStore {
    public int LoadSize { get => 0; set => value = 0; }

    private Dictionary<string, List<Database.VersionedValue>> _data;

    public InMemBackingStore() {
        _data = new Dictionary<string, List<Database.VersionedValue>> { };
    }
    public bool Append(string key, Database.VersionedValue value) {
        _data[key].Append(value);
        return true;
    } 
    public KeyValuePair<string, List<Database.VersionedValue>>[] Load() {
        KeyValuePair<string, List<Database.VersionedValue>>[] arr = 
            new KeyValuePair<string,List<Database.VersionedValue>>[_data.Count];

        foreach (var kv in _data) {
            arr.Append(kv);
        }
        return arr;
    }
    public bool ReloadAt(string key) {
        return true;
    }
    public void Dispose() {

    }
    public void Cleanup(int versToKeep) {
        
    }
}