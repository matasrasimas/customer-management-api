namespace server.Converters;

public abstract class Converter<T, U>
{
    public abstract U Convert(T input);

    public List<U> Convert(List<T> input) => input.Select(Convert).ToList();
}