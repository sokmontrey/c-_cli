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
/// Enum codes for after handle user input.
/// </summary>
public enum ModeCode {
  NOTHING,
  CONTINUE,
  EDIT,
  CURSOR_UP,
  CURSOR_DOWN,
  INCREMENT_ITEM,
  DECREMENT_ITEM,
  APPLY_COUPON,
}

/// <summary>
/// Represents an order.
/// </summary>
public class Order {
  public Item[] items = new Item[0];
  public float subtotal = 0;
  public float tax_rate = 0;
  public float tax = 0;
  public float discount = 0;
  public string coupon = "";
  public float total = 0;

  public Dictionary<string, float> available_coupons;

  public int cursor_pos;

  public Order(Dictionary<string, float> menu, float tax_rate, Dictionary<string, float> coupons) {
    items = new Item[menu.Count];
    int i = 0;
    foreach (var item in menu) {
      items[i++] =
          new Item { name = item.Key, price = item.Value, quantity = 0 };
    }

    this.tax_rate = tax_rate;
    available_coupons = coupons;
  }

  public void Edit() {
    while (true) {
      Console.Clear();
      DisplayManager.DisplayTitle();
      DisplayManager.DisplayMenu(items, cursor_pos);
      CalculateSubtotal();
      DisplayManager.DisplaySubtotal(subtotal);
      DisplayManager.DisplayEditInstructions();

      ModeCode mode = GetEditInput();
      switch (mode) {
      case ModeCode.NOTHING:
        continue;
      case ModeCode.CONTINUE:
        return;
      case ModeCode.INCREMENT_ITEM:
        AddItem();
        break;
      case ModeCode.DECREMENT_ITEM:
        RemoveItem();
        break;
      case ModeCode.CURSOR_UP:
        MoveUp();
        break;
      case ModeCode.CURSOR_DOWN:
        MoveDown();
        break;
      }
    }
  }

  private ModeCode GetEditInput() {
    ConsoleKeyInfo key = Console.ReadKey(true);
    switch (key.Key) {
    case ConsoleKey.UpArrow:
    case ConsoleKey.K:
      return ModeCode.CURSOR_UP;
    case ConsoleKey.DownArrow:
    case ConsoleKey.J:
      return ModeCode.CURSOR_DOWN;

    case ConsoleKey.LeftArrow:
    case ConsoleKey.H:
      return ModeCode.DECREMENT_ITEM;
    case ConsoleKey.RightArrow:
    case ConsoleKey.L:
      return ModeCode.INCREMENT_ITEM;

    case ConsoleKey.Q:
      Environment.Exit(0);
      break;
    case ConsoleKey.Enter:
      return ModeCode.CONTINUE;
    }
    return ModeCode.NOTHING;
  }

  public bool Summary() {
    while (true) {
      CalculateTax(tax_rate);
      CalculateTotal();
      DisplayManager.DisplayOrderSummary(this);
      DisplayManager.DisplaySummaryInstructions();

      ModeCode mode = GetSummaryInput();
      switch (mode) {
      case ModeCode.EDIT:
        return false;
      case ModeCode.CONTINUE:
        return true;
      case ModeCode.APPLY_COUPON:
        Console.Write("Enter coupon code: ");
        ApplyCoupon(Console.ReadLine(), available_coupons);
        break;
      }
    }
  }

  private ModeCode GetSummaryInput() {
    ConsoleKeyInfo key = Console.ReadKey(true);
    switch (key.Key) {
    case ConsoleKey.Q:
      Environment.Exit(0);
      break;
    case ConsoleKey.E:
      return ModeCode.EDIT;
    case ConsoleKey.Enter:
      return ModeCode.CONTINUE;
    case ConsoleKey.C:
      return ModeCode.APPLY_COUPON;
    }
    return ModeCode.NOTHING;
  }

  public void MoveDown() { cursor_pos = (cursor_pos + 1) % items.Length; }

  public void MoveUp() {
    cursor_pos = (cursor_pos - 1 + items.Length) % items.Length;
  }

  public void AddItem() { items[cursor_pos].quantity++; }

  public void RemoveItem() {
    if (items[cursor_pos].quantity > 0) {
      items[cursor_pos].quantity--;
    }
  }

  public void ApplyCoupon(string code, Dictionary<string, float> coupons) {
    coupon = code;
    discount = 0;
    if (coupons.ContainsKey(code))
      discount = subtotal * coupons[code];
  }

  public float CalculateSubtotal() {
    subtotal = 0;
    for (int i = 0; i < items.Length; i++)
      subtotal += items[i].price * items[i].quantity;
    return subtotal;
  }

  public void CalculateTax(float tax_rate) {
    this.tax_rate = tax_rate;
    tax = subtotal * tax_rate;
  }

  public void CalculateTotal() { total = subtotal + tax - discount; }

  public void HandleSaveOrder() {
    Console.Write("Save to file? y/yes or press any key to exit: ");
    string input = Console.ReadLine().ToLower();
    if (input == "y" || input == "yes") {
      Console.Write("Enter file name: ");
      string file_name = Console.ReadLine();
      while (file_name == "") {
        Console.Write("File name can't be empty. Enter another file name: ");
        file_name = Console.ReadLine();
      }
      while (File.Exists(file_name + ".txt")) {
        Console.Write("File already exists. Enter another file name: ");
        file_name = Console.ReadLine();
      }
      SaveToFile(file_name);
    }
  }

  public void SaveToFile(string filename) {
    using (StreamWriter writer = new StreamWriter(filename + ".txt")) {
      writer.WriteLine("Your Order: \n");
      writer.WriteLine($"{"Item",-20}{"Price",-10}{"Qty",-10}");
      writer.WriteLine(new string('-', 40));
      for (int i = 0; i < items.Length; i++) {
        if (items[i].quantity <= 0)
          continue;

        writer.WriteLine(
            $"{items[i].name,-20}{items[i].price,-10:C2}{items[i].quantity,-10}");
      }
      writer.WriteLine(new string('-', 40));
      writer.WriteLine($"{"Subtotal:",-20}{subtotal:C2}");
      writer.WriteLine($"{"Tax:",-20}{tax:C2} ({tax_rate:P0})");
      writer.WriteLine(
          $"{"Discount:",-20}{discount:C2} (Coupon Code: {coupon})");
      writer.WriteLine(new string('-', 40));
      writer.WriteLine($"{"Total:",-20}{total:C2}");
    }

    Console.WriteLine($"Order saved to {filename}.txt");
  }
}

} // namespace OrderSystem
