
namespace Ddbase;

public interface IBackingStore : IDisposable
{
    public bool Append(string key, string data);
    public string[] Load();
    public bool RecenterAt(string key);
}

public partial class Database
{
    IBackingStore _backing;
    IDictionary<string, string> _memStore;

    public Database(IBackingStore backend, 
                    IDictionary<string, string> memStore) 
    {
        this._backing = backend;
        this._memStore = memStore;
    }

    ~Database() {
        _backing.Dispose();
    }

    public bool read(string key, out string value) {
        value = "";
        return true;
    }

    public bool update(string key, string value) {
        return true;
    }

    public bool delete(string key) {
        return true;
    }

}

    /* public static Database make(string filename) {
        return new Database(new Backend(new FileStream(filename,
                                           FileMode.OpenOrCreate,
                                           FileAccess.ReadWrite,
                                           FileShare.None)));
    } */