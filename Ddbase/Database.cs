namespace Ddbase;

public partial class Database
{
    string Filename { get; }

    FileStream _file;

    public Database(string filename) {
        this.Filename = filename;
        //try {
            //we expect file to already exist, to avoid situations where an
            //existing file is overwritten. Use DatabaseMaker
            _file = new FileStream(
                Filename,
                FileMode.Open, 
                FileAccess.ReadWrite,
                FileShare.None);
        //}
        //catch (Exception e){
        //    throw e;
        //}
    }

    ~Database() {
        _file.Dispose();
    }
}

public partial class DatabaseMaker
{
    string Path { get; set; }

    DatabaseMaker(string path) {
        this.Path = path;
    }

    public Database build() {
        File.Create(Path);
        return new Database(Path);
    }
}
