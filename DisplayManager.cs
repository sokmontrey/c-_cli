namespace OrderSystem {

public class DisplayManager {
  public static void DisplayMenu(Item[] items, int cursor_pos) {
    Console.WriteLine($"{"Item",-22}{"Price",-10}{"Qty",-10}");
    PrintSeparateLine();

    for (int i = 0; i < items.Length; i++) {
      if (i == cursor_pos) {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("> ");
      } else {
        Console.Write("  ");
      }

      Console.WriteLine(
          $"{items[i].name,-20}{items[i].price,-10:C2}{items[i].quantity,-10}");
      Console.ResetColor();
    }
    Console.WriteLine("");
  }

  public static void DisplayOrder(Item[] items) {
    Console.WriteLine($"{"Item",-20}{"Price",-10}{"Qty",-10}");
    PrintSeparateLine();
    for (int i = 0; i < items.Length; i++) {
      if (items[i].quantity <= 0)
        continue;

      Console.WriteLine(
          $"{items[i].name,-20}{items[i].price,-10:C2}{items[i].quantity,-10}");
    }
  }

  public static void DisplaySubtotal(float subtotal) {
    Console.WriteLine($"{"Subtotal:",-20}{subtotal:C2}");
  }

  public static void DisplayTax(float tax, float tax_rate) {
    Console.WriteLine($"{"Tax:",-20}{tax:C2} ({tax_rate:P0})");
  }

  public static void DisplayDiscount(float discount, string coupon) {
    Console.WriteLine(
        $"{"Discount:",-20}{discount:C2} (Coupon Code: {coupon})");
  }

  public static void DisplayTotal(float total) {
    Console.WriteLine($"{"Total:",-20}{total:C2}");
  }

  public static void PrintSeparateLine() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(new string('-', 40));
    Console.ResetColor();
  }

  public static void DisplayOrderSummary(Order order) {
    Console.Clear();
    Console.WriteLine("Your Order: \n");
    DisplayOrder(order.items);

    PrintSeparateLine();

    DisplaySubtotal(order.subtotal);
    DisplayTax(order.tax, order.tax_rate);
    DisplayDiscount(order.discount, order.coupon);

    PrintSeparateLine();

    Console.ForegroundColor = ConsoleColor.Blue;
    DisplayTotal(order.total);
    Console.ResetColor();
  }

  public static void DisplayTitle() {
    Console.Write("\nWelcome to the");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(" par la rivière ");
    Console.ResetColor();

    Console.WriteLine("digital ordering system!\n");
  }

  public static void DisplayEditInstructions() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  ↓/j   (down)       ↑/k (up)" +
                      "\n  →/l   (add)        ←/h (remove)" +
                      "\n  enter (continue)     q (quit)\n");
    Console.ResetColor();
  }

  public static void DisplaySummaryInstructions() {
    Console.WriteLine("");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("e (edit)             c (apply coupon)" +
                      "\nenter (continue)     q (quit)\n");
    Console.ResetColor();
  }
}
}
