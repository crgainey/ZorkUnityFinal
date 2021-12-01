namespace Zork
{
    public interface IOutputService
    {
        void Write(object value);

        void WriteLine(object value);

        void Write(string value);

        void WriteLine(string value);

        void Clear();

    }
}
