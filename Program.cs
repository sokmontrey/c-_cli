/// Author: Sokmontrey Sythat
/// Student ID: 101477705
namespace OrderSystem {

/// <summary>
/// Enum codes for after handle user input.
/// </summary>
public enum ModeCode { NOTHING, CONTINUE, EXIT }

/// <summary>
/// Main entry point of the program
/// </summary>
public class Program {
  private Dictionary<string, float> _coupons = new Dictionary<string, float> {
    { "10OFF", 0.10f },
    { "20OFF", 0.20f },
    { "30OFF", 0.30f },
  };

  private float _tax_rate = 0.13f;

  public static void Main(string[] args) {
    Order order = new Order(new Dictionary<string, float> {
      { "Burger", 5.99f },
      { "Fries", 2.99f },
      { "Drink", 1.99f },
    });

    while (true) {
      Console.Clear();

      displayTitle();
      displayInstructions();

      order.displayMenu();
      order.calculateSubtotal();
      order.displaySummary();

      ModeCode mode = getOrderInput(order);
      switch(mode){
      case ModeCode.EXIT:
        return;
      case ModeCode.CONTINUE:
        break;
      }
    }
  } // Main

  private static void displayTitle() {
    Console.Write("Welcome to the");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(" par la rivière ");
    Console.ResetColor();

    Console.Write("digital ordering system!");
  }

  private static void displayInstructions() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("");
    Console.WriteLine("\n  ↓/j   (down)       ↑/k (up)" +
                      "\n  →/l   (add)        ←/h (remove)" +
                      "\n  enter (continue)     q (quit)\n");
    Console.ResetColor();
  }

  private static ModeCode getOrderInput(Order order) {
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
      return ModeCode.EXIT;

    case ConsoleKey.Enter:
      return ModeCode.CONTINUE;
    }
    return ModeCode.NOTHING;
  }

} // class Program

} // namespace OrderSystem
