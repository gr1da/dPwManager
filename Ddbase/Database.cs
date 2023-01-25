namespace Ddbase;
public class Database {
    public struct VersionedValue {
        public readonly int Version;
        public readonly string Value;
    }
    public interface IBackingStore : IDisposable {
        public int LoadSize { get => 0; set => value = 0; }
        public bool Append(string key, VersionedValue data);
        public KeyValuePair<string, List<VersionedValue>>[] Load();
        public bool ReloadAt(string key) => true;
        public void Cleanup(int versToKeep) { }
    }

    IBackingStore _backing;
    IDictionary<string, List<VersionedValue>> _memStore;

    public int CachedSize { get => _memStore.Count; }

    public Database(IBackingStore backend, 
                    IDictionary<string, List<VersionedValue>> memStore) {

        _backing = backend;
        _memStore = memStore;
    }
    ~Database() {
        _backing.Dispose();
    }
    private void loadFromBacking() {
        foreach (var kv in _backing.Load()) {
            if (! _memStore.ContainsKey(kv.Key)) {
                _memStore[kv.Key] = kv.Value;
            }
        }
    }
    private List<VersionedValue>? getAllVals(string key) {
        if (!_memStore.ContainsKey(key)) {
            //then try recaching
            _backing.ReloadAt(key); 
            loadFromBacking();
            if (!_memStore.ContainsKey(key)) {
                return null;
            }
        }
        return _memStore[key];
    }
    public string? getOfVersion(string key, int version) {
        List<VersionedValue>? allVals = getAllVals(key);
        if (allVals is null) {
            return null;
        }
        return allVals.Find(v => v.Version == version).Value;
    }
    public bool getNewest(string key) {

    }
    public string this[string key] {
        get => "";
    } 
}

    /* public static Database make(string filename) {
        return new Database(new Backend(new FileStream(filename,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None)));
    } */