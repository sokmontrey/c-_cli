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
  private Item[] _items = new Item[0];
  private float _subtotal = 0;
  private float _tax_rate = 0;
  private float _tax = 0;
  private float _discount = 0;
  private string _coupon = "";
  private float _total = 0;

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

  public void displayMenu() {
    Console.WriteLine($"{"Item",-22}{"Price",-10}{"Qty",-10}");
    printSeparateLine();

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

  public void displayOrder() {
    Console.WriteLine($"{"Item",-20}{"Price",-10}{"Qty",-10}");
    printSeparateLine();
    for (int i = 0; i < _items.Length; i++) {
      if (_items[i].quantity > 0) {
        Console.WriteLine(
            $"{_items[i].name,-20}{_items[i].price,-10:C2}{_items[i].quantity,-10}");
      }
    }
  }

  public void applyCoupon(string code, Dictionary<string, float> coupons) {
    _coupon = code;
    _discount = 0;
    if (coupons.ContainsKey(code))
      _discount = _subtotal * coupons[code];
  }

  public float calculateSubtotal() {
    _subtotal = 0;
    for (int i = 0; i < _items.Length; i++)
      _subtotal += _items[i].price * _items[i].quantity;
    return _subtotal;
  }

  public void displaySubtotal() {
    Console.WriteLine($"{"Subtotal:",-20}{_subtotal:C2}");
  }

  public void calculateTax(float tax_rate) {
    _tax_rate = tax_rate;
    _tax = _subtotal * tax_rate;
  }

  public void displayTax() {
    Console.WriteLine($"{"Tax:",-20}{_tax:C2} ({_tax_rate:P0})");
  }

  public void displayDiscount() {
    Console.WriteLine(
        $"{"Discount:",-20}{_discount:C2} (Coupon Code: {_coupon})");
  }

  public void displayTotal() {
    _total = _subtotal + _tax - _discount;
    Console.WriteLine($"{"Total:",-20}{_total:C2}");
  }

  public void displaySummary() {
    Console.Clear();
    Console.WriteLine("Your Order: \n");
    displayOrder();

    printSeparateLine();

    displaySubtotal();
    displayTax();
    displayDiscount();

    printSeparateLine();

    Console.ForegroundColor = ConsoleColor.Blue;
    displayTotal();
    Console.ResetColor();
  }

  public static void printSeparateLine() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(new string('-', 40));
    Console.ResetColor();
  }

  public void saveToFile(string filename) {
    using (StreamWriter writer = new StreamWriter(filename + ".txt")) {
      writer.WriteLine("Your Order: \n");
      writer.WriteLine($"{"Item",-20}{"Price",-10}{"Qty",-10}");
      writer.WriteLine(new string('-', 40));
      for (int i = 0; i < _items.Length; i++) {
        if (_items[i].quantity <= 0) continue;

        writer.WriteLine(
            $"{_items[i].name,-20}{_items[i].price,-10:C2}{_items[i].quantity,-10}");
      }
      writer.WriteLine(new string('-', 40));
      writer.WriteLine($"{"Subtotal:",-20}{_subtotal:C2}");
      writer.WriteLine($"{"Tax:",-20}{_tax:C2} ({_tax_rate:P0})");
      writer.WriteLine(
          $"{"Discount:",-20}{_discount:C2} (Coupon Code: {_coupon})");
      writer.WriteLine(new string('-', 40));
      writer.WriteLine($"{"Total:",-20}{_total:C2}");
    }

    Console.WriteLine($"Order saved to {filename}.txt");
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
