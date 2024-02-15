/**
 * @author Sokmontrey Sythat / 101477705
 */
namespace OrderSystem {

/**
 * @brief A static with methods to display menu, order, summary, and etc.
 */
public class DisplayManager {
  /**
   * @brief Display the menu with a cursor to indicate the current selection.
   * @param {Item[]} items the menu items with quantity data
   * @param {int} cursor_pos the current cursor position
   */
  public static void DisplayMenu(Item[] items, int cursor_pos) {
    Console.WriteLine($"{"Item",-42}{"Price",-10}{"Qty",-10}");
    PrintSeparateLine();

    for (int i = 0; i < items.Length; i++) {
      if (i == cursor_pos) {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("> ");
      } else {
        Console.Write("  ");
      }

      Console.WriteLine(
          $"{items[i].name,-40}{items[i].price,-10:C2}{items[i].quantity,-10}");
      Console.ResetColor();
    }
  }

  /**
   * @brief Display only the items user has selected.
   * @param {Item[]} items the menu items with quantity data
   */
  public static void DisplayOrder(Item[] items) {
    Console.WriteLine($"{"Item",-40}{"Price",-10}{"Qty",-10}");
    PrintSeparateLine();
    for (int i = 0; i < items.Length; i++) {
      if (items[i].quantity <= 0)
        continue;

      Console.WriteLine(
          $"{items[i].name,-40}{items[i].price,-10:C2}{items[i].quantity,-10}");
    }
  }

  /**
   * @brief Display the order is empty message.
   */
  public static void DisplayOrderIsEmpty() {
    Console.WriteLine("Order is empty. Please add items to the order.");
    Console.Write("Press any key to continue...");
    Console.ReadKey(true);
  }

  /**
   * @brief Display the subtotal of the order.
   * @param {float} subtotal the subtotal of the order
   */
  public static void DisplaySubtotal(float subtotal) {
    Console.WriteLine($"{"Subtotal:",-40}{subtotal:C2}");
  }

  /**
   * @brief Display the tax and the tax rate.
   * @param {float} tax the tax amount
   * @param {float} tax_rate the tax rate
   */
  public static void DisplayTax(float tax, float tax_rate) {
    Console.WriteLine($"{"Tax:",-40}{tax:C2} ({tax_rate:P0})");
  }

  /**
   * @brief Display the discount and the coupon code.
   * @param {float} discount the discount amount
   * @param {string} coupon the coupon code
   */
  public static void DisplayDiscount(float discount, string coupon) {
    Console.WriteLine(
        $"{"Discount:",-40}{discount:C2} (Coupon Code: {coupon})");
  }

  /**
   * @brief Display the total of the order.
   * @param {float} total the total of the order
   */
  public static void DisplayTotal(float total) {
    Console.WriteLine($"{"Total:",-40}{total:C2}");
  }

  /**
   * @brief A method to print a separate line to separate sections.
   */
  public static void PrintSeparateLine() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(new string('-', 60));
    Console.ResetColor();
  }

  /**
   * @brief Display the user order, subtotal, tax, discount, and total.
   * @param {Order} order the order object
   */
  public static void DisplayOrderSummary(Order order) {
    Console.WriteLine("Your Order: \n");
    DisplayOrder(order.items);

    PrintSeparateLine();

    DisplaySubtotal(order.subtotal);
    DisplayTax(order.tax, order.tax_rate);
    DisplayDiscount(order.discount, order.current_coupon);

    PrintSeparateLine();

    Console.ForegroundColor = ConsoleColor.Blue;
    DisplayTotal(order.total);
    Console.ResetColor();
  }

  /**
   * @brief Display the title of the program.
   */
  public static void DisplayTitle() {
    Console.Write("\nWelcome to the");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(" par la rivière ");
    Console.ResetColor();

    Console.WriteLine("digital ordering system!\n");
  }

  /**
   * @brief Display the instructions for Edit mode.
   */
  public static void DisplayEditInstructions() {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("Instructions:");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  ↓/j   (down)       ↑/k (up)" +
                      "\n  →/l   (add)        ←/h (remove)" +
                      "\n  enter (continue)     q (quit)\n");
    Console.ResetColor();
  }

  /**
   * @brief Display the instructions for Summary mode.
   */
  public static void DisplaySummaryInstructions() {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nInstructions:");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("e (edit)             c (apply coupon)" +
                      "\nenter (continue)     q (quit)\n");
    Console.ResetColor();
  }
}
}
