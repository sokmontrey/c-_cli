/// Author: Sokmontrey Sythat
/// Student ID: 101477705
namespace OrderSystem {

/// <summary>
/// Enum codes for after handle user input.
/// </summary>
public enum ModeCode { NOTHING, CONTINUE, EDIT }

/// <summary>
/// Main entry point of the program
/// </summary>
public class Program {
  public static Dictionary<string, float> _menu =
      new Dictionary<string, float> {
        { "Burger", 5.99f },
        { "Fries", 2.99f },
        { "Drink", 1.99f },
      };

  public static float _tax_rate = 0.13f;

  public static Dictionary<string, float> _coupons =
      new Dictionary<string, float> {
        { "10OFF", 0.10f },
        { "20OFF", 0.20f },
        { "30OFF", 0.30f },
      };

  public static void Main(string[] args) {
    Order order = new Order(_menu);
    ModeCode mode;

    while (true) {
      (mode, order) = edit(order);

      // user can't continue with an empty order
      if (order.calculateSubtotal() == 0) {
        Console.WriteLine("Order is empty. Please add items to the order.");
        Console.Write("Press any key to continue...");
        Console.ReadKey(true);
        continue;
      }

      // user have the decision to edit the order
      (mode, order) = summary(order);
      if (mode == ModeCode.EDIT)
        continue;
      if (mode == ModeCode.CONTINUE)
        break;
    }

    Console.Clear();
    order.displaySummary();
    Console.WriteLine("\nOrder submitted!");
    Console.WriteLine("Thank you for your order!\n");
    handleSaveOrder(order);
  }

  private static void handleSaveOrder(Order order) {
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
      order.saveToFile(file_name);
    }
  }

  private static (ModeCode, Order) edit(Order order) {
    while (true) {
      Console.Clear();
      displayTitle();
      order.displayMenu();

      Console.WriteLine("");
      order.calculateSubtotal();
      order.displaySubtotal();

      displayEditInstructions();
      ModeCode mode = getEditInput(order);
      if (mode != ModeCode.NOTHING)
        return (mode, order);
    }
  } 

  private static (ModeCode, Order) summary(Order order) {
    while (true) {
      order.calculateTax(_tax_rate);
      order.displaySummary();

      Console.WriteLine("");
      displaySummaryInstructions();

      ConsoleKeyInfo key = Console.ReadKey(true);
      switch (key.Key) {
      case ConsoleKey.Q:
        Environment.Exit(0);
        break;

      case ConsoleKey.E:
        return (ModeCode.EDIT, order);

      case ConsoleKey.Enter:
        return (ModeCode.CONTINUE, order);

      case ConsoleKey.C:
        Console.Write("Enter coupon code: ");
        order.applyCoupon(Console.ReadLine(), _coupons);
        break;
      }
    }
  }

  private static void displayTitle() {
    Console.Write("\nWelcome to the");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(" par la rivière ");
    Console.ResetColor();

    Console.WriteLine("digital ordering system!\n");
  } 

  private static void displayEditInstructions() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("\n  ↓/j   (down)       ↑/k (up)" +
                      "\n  →/l   (add)        ←/h (remove)" +
                      "\n  enter (continue)     q (quit)\n");
    Console.ResetColor();
  }

  private static void displaySummaryInstructions() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("e (edit)             c (apply coupon)" +
                      "\nenter (continue)     q (quit)\n");
    Console.ResetColor();
  }

  private static ModeCode getEditInput(Order order) {
    ConsoleKeyInfo key = Console.ReadKey(true);
    switch (key.Key) {
    case ConsoleKey.DownArrow:
    case ConsoleKey.J:
      order.moveDown();
      break;

    case ConsoleKey.UpArrow:
    case ConsoleKey.K:
      order.moveUp();
      break;

    case ConsoleKey.RightArrow:
    case ConsoleKey.L:
      order.addItem();
      break;

    case ConsoleKey.LeftArrow:
    case ConsoleKey.H:
      order.removeItem();
      break;

    case ConsoleKey.Q:
      Environment.Exit(0);
      break;
    case ConsoleKey.Enter:
      return ModeCode.CONTINUE;
    }
    return ModeCode.NOTHING;
  }

} // class Program

} // namespace OrderSystem
