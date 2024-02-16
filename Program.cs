/**
 * @author Sokmontrey Sythat / 101477705
 */
namespace OrderSystem {

public class Program {
  public static string[] menu =
      new string[10] { "Chicken braised with wine",
                       "Beef stew cooked in red wine",
                       "Traditional Provencal fish stew",
                       "Savory tart filled with custard",
                       "French salad",
                       "Slow-cooked meat casserole",
                       "Grilled steak with fries",
                       "White wine Mussels",
                       "Creamy custard with caramel",
                       "Caramelized apple upside-down pastry" };

  public static float[] price =
      new float[10] { 20.99f, 22.99f, 27.50f, 14.75f, 9.99f,
                      24.50f, 23.99f, 18.99f, 9.99f,  11.99f };

  public static float tax_rate = 0.13f;

  public static Dictionary<string, float> coupons =
      new Dictionary<string, float> {
        { "10OFF", 0.10f },
        { "20OFF", 0.20f },
        { "30OFF", 0.30f },
      };

  // aliases for mode code
  public static int NOTHING = 0;
  public static int CONTINUE = 1;
  public static int EDIT = 2;
  public static int CURSOR_UP = 3;
  public static int CURSOR_DOWN = 4;
  public static int INCREMENT_ITEM = 5;
  public static int DECREMENT_ITEM = 6;
  public static int APPLY_COUPON = 7;
  public static int EXIT = 8;

  public static void Main(string[] args) {

    // Initialize application states
    int[] quantity = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int cursor_position = 0;

    // order bill information
    float subtotal = 0;
    float discount = 0;
    string coupon_code = "";
    float tax = 0;
    float total = 0;

    while (true) {
      // Enter Edit mode
      subtotal = Edit(ref quantity, ref cursor_position);

      // user can't continue with an empty order
      if (subtotal == 0) {
        DisplayOrderIsEmpty();
        continue;
      }

      // Enter Summary mode
      bool is_continue = Summary(quantity, subtotal, ref discount,
                                 ref coupon_code, out tax, out total);
      if (is_continue)
        break;

      // Go back and enter Edit mode
    }

    // submit order (to nowhere at the moment)
    Console.Clear();
    DisplayOrderSummary(quantity, subtotal, tax, discount, coupon_code, total);
    Console.WriteLine("\nOrder submitted!");
    Console.WriteLine("Thank you for your purchases!\n");
    // save order based on user's choice
    HandleSaveOrder(quantity, subtotal, tax, discount, coupon_code, total);
  }

  /**
   * Main loop for Editing the order.
   *  - clear the console
   *  - display the title, instructions, menu, and subtotal
   *  - get user input and handle the input (add/remove items, move up/down the
   * cursor)
   *  - repeat until user wants to continue
   */
  public static float Edit(ref int[] quantity, ref int cursor_position) {
    float subtotal = 0;
    while (true) {
      Console.Clear();
      DisplayTitle();
      DisplayEditInstructions();
      DisplayMenu(ref quantity, ref cursor_position);
      PrintSeparateLine();
      subtotal = CalculateSubtotal(quantity);
      DisplaySubtotal(subtotal);

      int mode = WaitForInput();

      // switch statement does not work with a non-constant expression
      // so I use if-else instead
      if (mode == EXIT) {
        Environment.Exit(0);
      } else if (mode == CONTINUE) {
        // break out of the loop by returning from the directly
        return subtotal;
      } else if (mode == INCREMENT_ITEM) {
        quantity[cursor_position]++;
      } else if (mode == DECREMENT_ITEM && quantity[cursor_position] > 0) {
        quantity[cursor_position]--;
      } else if (mode == CURSOR_UP) {
        cursor_position = (cursor_position - 1 + menu.Length) % menu.Length;
      } else if (mode == CURSOR_DOWN) {
        cursor_position = (cursor_position + 1) % menu.Length;
      }
    }
  }

  /**
   * Main loop for displaying the order summary.
   *  - clear the console
   *  - display the order, subtotal, tax, discount, total, and instructions
   *  - get user input and handle the input (continue, edit, apply coupon)
   *  - repeat until user wants to edit or continue
   * return bool
   *  true if user wants to continue with the order
   *  false if user wants to edit the order
   */
  public static bool Summary(int[] quantity, float subtotal, ref float discount,
                             ref string coupon_code, out float tax,
                             out float total) {
    while (true) {
      tax = CalculateTax(subtotal);
      total = CalculateTotal(subtotal, tax, discount);
      Console.Clear();
      DisplayOrderSummary(quantity, subtotal, tax, discount, coupon_code,
                          total);
      DisplaySummaryInstructions();

      int mode = WaitForInput();

      if (mode == EXIT) {
        Environment.Exit(0);
      } else if (mode == CONTINUE) {
        return true;
      } else if (mode == EDIT) {
        return false;
      } else if (mode == APPLY_COUPON) {
        Console.Write("Enter coupon code: ");
        coupon_code = Console.ReadLine();
        discount = GetCouponDiscount(coupon_code, subtotal);
      }
    }
  }

  /**
   * Apply a coupon to the order by setting the discount based on the
   * coupon code.
   *    - Update current_coupon state
   *    - Check if the coupon code is available
   *    - Update the discount based on the coupon code
   *    - Set discount to 0 otherwise.
   */
  public static float GetCouponDiscount(string coupon_code, float subtotal) {
    if (coupons.ContainsKey(coupon_code))
      return subtotal * coupons[coupon_code];
    return 0;
  }

  /**
   * Calculate total tax based on tax rate and subtotal.
   *  Update tax state then return it (for immediate use).
   */
  public static float CalculateTax(float subtotal) {
    return subtotal * tax_rate;
  }

  /**
   * Calculate total by adding tax and subtracting discount from
   * subtotal. Update total state then return it (for immediate use).
   */
  public static float CalculateTotal(float subtotal, float tax,
                                     float discount) {
    return subtotal + tax - discount;
  }

  /**
   * Wait for user input and return a corresponding mode code
   */
  public static int WaitForInput() {
    ConsoleKeyInfo key = Console.ReadKey(true);
    switch (key.Key) {
    case ConsoleKey.UpArrow:
    case ConsoleKey.K:
      return CURSOR_UP;
    case ConsoleKey.DownArrow:
    case ConsoleKey.J:
      return CURSOR_DOWN;
    case ConsoleKey.LeftArrow:
    case ConsoleKey.H:
      return DECREMENT_ITEM;
    case ConsoleKey.RightArrow:
    case ConsoleKey.L:
      return INCREMENT_ITEM;
    case ConsoleKey.Q:
      return EXIT;
    case ConsoleKey.Enter:
      return CONTINUE;
    case ConsoleKey.E:
      return EDIT;
    case ConsoleKey.C:
      return APPLY_COUPON;
    default:
      return NOTHING;
    }
  }

  /**
   * Calculate subtotal by sum up all the items' price multiplied by
   * their quantity. Update subtotal state then return it (for immediate use).
   */
  public static float CalculateSubtotal(int[] quantity) {
    float subtotal = 0;
    for (int i = 0; i < price.Length; i++) {
      subtotal += price[i] * quantity[i];
    }
    return subtotal;
  }

  /**
   * Display the menu with a cursor to indicate the current selection.
   */
  public static void DisplayMenu(ref int[] quantity, ref int cursor_pos) {
    Console.WriteLine($"{"Item",-42}{"Price",-10}{"Qty",-10}");
    PrintSeparateLine();

    for (int i = 0; i < menu.Length; i++) {
      if (i == cursor_pos) {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("> ");
      } else {
        Console.Write("  ");
      }

      Console.WriteLine($"{menu[i],-40}{price[i],-10:C2}{quantity[i],-10}");
      Console.ResetColor();
    }
  }

  /**
   * A method to print a separate line to separate sections.
   */
  public static void PrintSeparateLine() {
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine(new string('-', 60));
    Console.ResetColor();
  }

  /**
   * Display the title of the program.
   */
  public static void DisplayTitle() {
    Console.Write("\nWelcome to the");

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(" par la rivière ");
    Console.ResetColor();

    Console.WriteLine("digital ordering system!\n");
  }

  /**
   * Display the instructions for Edit mode.
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
   * Display the user order, subtotal, tax, discount, and total.
   */
  public static void DisplayOrderSummary(int[] quantity, float subtotal,
                                         float tax, float discount,
                                         string coupon_code, float total) {
    Console.WriteLine("Your Order: \n");
    DisplayOrder(quantity);

    PrintSeparateLine();

    DisplaySubtotal(subtotal);
    Console.WriteLine($"{"Tax:",-40}{tax:C2} ({tax_rate:P0})");
    Console.WriteLine(
        $"{"Discount:",-40}{discount:C2} (Coupon Code: {coupon_code})");

    PrintSeparateLine();

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine($"{"Total:",-40}{total:C2}");
    Console.ResetColor();
  }

  /**
   * Display the instructions for Summary mode.
   */
  public static void DisplaySummaryInstructions() {
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\nInstructions:");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("e (edit)             c (apply coupon)" +
                      "\nenter (continue)     q (quit)\n");
    Console.ResetColor();
  }

  /**
   * Display only the items user has selected.
   */
  public static void DisplayOrder(int[] quantity) {
    Console.WriteLine($"{"Item",-40}{"Price",-10}{"Qty",-10}");
    PrintSeparateLine();
    for (int i = 0; i < quantity.Length; i++) {
      if (quantity[i] <= 0)
        continue;
      Console.WriteLine($"{menu[i],-40}{price[i],-10:C2}{quantity[i],-10}");
    }
  }

  /**
   * Display the subtotal of the order.
   */
  public static void DisplaySubtotal(float subtotal) {
    Console.WriteLine($"{"Subtotal:",-40}{subtotal:C2}");
  }

  /**
   * Display the order is empty message.
   */
  public static void DisplayOrderIsEmpty() {
    Console.WriteLine("Order is empty. Please add items to the order.");
    Console.Write("Press any key to continue...");
    Console.ReadKey(true);
  }

  /**
   * Handle user input to save the order to a file.
   *    - Ask user if they want to save the order to a file
   *    - If yes, ask for file name, check if file already exist, and save the
   *    order to the file
   *    - If no, do nothing
   */
  public static void HandleSaveOrder(int[] quantity, float subtotal, float tax,
                                     float discount, string current_coupon,
                                     float total) {
    Console.Write("Save to file? y/yes or press any key to exit: ");
    string input = Console.ReadLine().ToLower();
    if (input == "y" || input == "yes") {
      Console.Write("Enter file name: ");
      string file_name = Console.ReadLine();
      while (file_name == "") // keep asking for file name if it's empty
      {
        Console.Write("File name can't be empty. Enter another file name: ");
        file_name = Console.ReadLine();
      }
      while (File.Exists(
          file_name + ".txt")) // keep asking for file name if it already exists
      {
        Console.Write("File already exists. Enter another file name: ");
        file_name = Console.ReadLine();
      }
      SaveToFile(file_name, quantity, subtotal, tax, discount, current_coupon,
                 total);
    }
  }

  /**
   * Save the order to a file with the given file name.
   *    - Write the order summary to the file
   *    - Print a message to the console to confirm the file is saved
   */
  public static void SaveToFile(string filename, int[] quantity, float subtotal,
                                float tax, float discount, string coupon_code,
                                float total) {
    using (StreamWriter writer = new StreamWriter(filename + ".txt")) {
      writer.WriteLine("Your Order: \n");
      writer.WriteLine($"{"Item",-40}{"Price",-10}{"Qty",-10}");
      writer.WriteLine(new string('-', 60));

      for (int i = 0; i < menu.Length; i++) {
        if (quantity[i] <= 0)
          continue;
        writer.WriteLine($"{menu[i],-40}{price[i],-10:C2}{quantity[i],-10}");
      }

      writer.WriteLine(new string('-', 60));
      writer.WriteLine($"{"Subtotal:",-40}{subtotal:C2}");
      writer.WriteLine($"{"Tax:",-40}{tax:C2} ({tax_rate:P0})");
      writer.WriteLine(
          $"{"Discount:",-40}{discount:C2} (Coupon Code: {coupon_code})");
      writer.WriteLine(new string('-', 60));
      writer.WriteLine($"{"Total:",-40}{total:C2}");
    }

    Console.WriteLine($"Order saved to {filename}.txt");
  }
}
}
