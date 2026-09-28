using System.Text.Json;

public class TodoStore
{
    private readonly string _path;
    private readonly List<TodoItem> _items;

    public TodoStore(string path)
    {
        _path = path;
        _items = File.Exists(path)
            ? JsonSerializer.Deserialize<List<TodoItem>>(File.ReadAllText(path)) ?? new()
            : new();
    }

    public IReadOnlyList<TodoItem> GetAll() => _items;

    public void Add(string title)
    {
        int id = _items.Count == 0 ? 1 : _items.Max(i => i.Id) + 1;
        _items.Add(new TodoItem { Id = id, Title = title });
        Save();
    }

    public bool Toggle(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item is null) return false;
        item.IsDone = !item.IsDone;
        Save();
        return true;
    }

    public bool Delete(int id)
    {
        var item = _items.FirstOrDefault(i => i.Id == id);
        if (item is null) return false;
        _items.Remove(item);
        Save();
        return true;
    }

    private void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(_path, JsonSerializer.Serialize(_items, options));
    }
}