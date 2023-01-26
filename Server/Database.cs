namespace dPwManager.Server;

using KvPair = KeyValuePair<string, string>;

//Interface to database backing store, like a file. Implements IDisposable so
//that file handles can be closed by destructor in Database class.
public interface IBackingStore : IDisposable {
    //Return up to "blocks" number of data, or as many as are available.
    public KvPair[] Read(int blocks);
    public KvPair[] ReadAll();
    //Write to end of backing store (creation AND update)
    public void Append(KvPair data);
    public void Delete(string key);
    //Cleans up old data that has been deleted or updated. Needed since the only
    //mutation done on backing store is appending. Deletions or updates don't
    //actually change data until this is called.
    public void Clean();
}

public partial class Database {
    private IBackingStore _backing;
    //Dictionary of key/value pairs where key is string, value is a collection
    //of (int, string) tuples holding multiple versions of data associated with
    //a key.
    private IDictionary<string, ICollection<(int, string)>> _data;

    //Constructor intended to be called by derived class that implements a
    //factory
    protected Database(IBackingStore backingStore,
        IDictionary<string, ICollection<(int, string)>> dataStructure) {
        
        _backing = backingStore;
        _data = dataStructure;
    }

    private string retrieve(string key) {
        return "";
    }

    public string this[string key] {
        get => retrieve(key);
        set {

        }
    }
}

public partial class DummyBackingStore : IBackingStore {
    public KvPair[] Read(int blocks) => ReadAll();
    public KvPair[] ReadAll() => new KvPair[] {new KvPair("dummy", "store")};
    public void Append(KvPair data) { }
    public void Delete(string key) { }
    public void Clean() { }
    public void Dispose() { }
}
