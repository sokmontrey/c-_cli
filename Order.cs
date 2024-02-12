/// Author: Sokmontrey Sythat
/// Student ID: 101477705
namespace OrderSystem {

/// <summary>
/// Represents an item in the order.
/// </summary>
public struct Item {
  public string name;
  public float price;
  public int quantity;
}

/// <summary>
/// Represents an order.
/// </summary>
public class Order {
  private Item[] _items;
  private float _subtotal;
  private float _tax;
  private float _discount;
  private float _total;

  private int _cursor_pos;

  public Order(Item[] items) { _items = items; }

  public Order(Dictionary<string, float> menu) {
    _items = new Item[menu.Count];
    int i = 0;
    foreach (var item in menu) {
      _items[i++] =
          new Item { name = item.Key, price = item.Value, quantity = 0 };
    }
  }

  public void clear() {
    for (int i = 0; i < _items.Length; i++) {
      _items[i].quantity = 0;
    }
  }

  public void displayMenu() {
    Console.WriteLine($"{"Item",-22}{"Price",-10}{"Qty",-10}");
    Console.WriteLine(new string('-', 40));
    for (int i = 0; i < _items.Length; i++) {
      if (i == _cursor_pos) {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("> ");
      } else {
        Console.Write("  ");
      }

      Console.WriteLine(
          $"{_items[i].name,-20}{_items[i].price,-10:C2}{_items[i].quantity,-10}");
      Console.ResetColor();
    }
  }

  public void calculateSubtotal() {
    _subtotal = 0;
    for (int i = 0; i < _items.Length; i++) {
      _subtotal += _items[i].price * _items[i].quantity;
    }
  }

  public void displaySummary() {
    Console.WriteLine("");
    Console.WriteLine($"{"Subtotal:",-20}{_subtotal:C2}");
  }

  public void moveDown() { _cursor_pos = (_cursor_pos + 1) % _items.Length; }

  public void moveUp() {
    _cursor_pos = (_cursor_pos - 1 + _items.Length) % _items.Length;
  }

  public void addItem() { _items[_cursor_pos].quantity++; }

  public void removeItem() {
    if (_items[_cursor_pos].quantity > 0) {
      _items[_cursor_pos].quantity--;
    }
  }
}

} // namespace OrderSystem
