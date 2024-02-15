/**
 * @author Sokmontrey Sythat / 101477705
 */
namespace OrderSystem {

/**
 * @brief Represents an item in the order.
 */
public struct Item {
  public string name;
  public float price;
  public int quantity;
}

/**
 * @brief Enum codes representing user commands.
 */
public enum ModeCode {
  NOTHING,
  CONTINUE,
  EDIT,
  CURSOR_UP,
  CURSOR_DOWN,
  INCREMENT_ITEM,
  DECREMENT_ITEM,
  APPLY_COUPON,
  EXIT,
}

/**
 * @brief Holds the state of the order.
 *    - Number of items in the menu
 *    - Keeping track of the number of items the user added to the order
 *    - Calculate subtotal, tax, discount, and total
 *    - Handle user input
 *    - Keeping track of the current cursor position in the menu
 *    - Handle saving the order to a file
 */
public class Order {
  // Array of Items in the menu with quantity and price data.
  public Item[] items = new Item[0];
  public float subtotal = 0;
  public float tax_rate = 0;
  public float tax = 0;
  public float discount = 0;
  public string current_coupon = "";
  public float total = 0;

  private Dictionary<string, float> available_coupons;
  // Current cursor position in the menu CLI.
  private int cursor_pos;

  /**
   * @brief map ConsoleKey to ModeCode for user input handling. More scalable than using a switch statement.
   */
  public Dictionary<ConsoleKey, ModeCode> key_mode_map =
      new Dictionary<ConsoleKey, ModeCode> {
        { ConsoleKey.UpArrow, ModeCode.CURSOR_UP },
        { ConsoleKey.K, ModeCode.CURSOR_UP },

        { ConsoleKey.DownArrow, ModeCode.CURSOR_DOWN },
        { ConsoleKey.J, ModeCode.CURSOR_DOWN },

        { ConsoleKey.LeftArrow, ModeCode.DECREMENT_ITEM },
        { ConsoleKey.H, ModeCode.DECREMENT_ITEM },

        { ConsoleKey.RightArrow, ModeCode.INCREMENT_ITEM },
        { ConsoleKey.L, ModeCode.INCREMENT_ITEM },

        { ConsoleKey.Q, ModeCode.EXIT },
        { ConsoleKey.Enter, ModeCode.CONTINUE },
        { ConsoleKey.E, ModeCode.EDIT },
        { ConsoleKey.C, ModeCode.APPLY_COUPON },
      };

  /**
   * @constructor
   * @brief Initializes a new array of Items with name, price, and quantity
   * data.
   * @param {Dictionary<string, float>} menu name and price of the item
   * @param {float} tax_rate Tax rate in decimal form (0.13 for 13%)
   * @param {Dictionary<string, float>} coupons Dictionary of coupon code and
   * its discount rate
   */
  public Order(Dictionary<string, float> menu, float tax_rate,
               Dictionary<string, float> coupons) {

    items = new Item[menu.Count];
    int i = 0;
    foreach (var item in menu) {
      items[i++] =
          new Item { name = item.Key, price = item.Value, quantity = 0 };
    }

    this.tax_rate = tax_rate;
    available_coupons = coupons;
  }

  /**
   * @brief Main loop for Editing the order.
   *  - clear the console
   *  - display the title, instructions, menu, and subtotal
   *  - get user input and handle the input (add/remove items, move up/down the
   * cursor)
   *  - repeat until user wants to continue
   * @return void
   */
  public void Edit() {
    while (true) {
      Console.Clear();
      DisplayManager.DisplayTitle();
      DisplayManager.DisplayEditInstructions();
      DisplayManager.DisplayMenu(items, cursor_pos);
      CalculateSubtotal();
      DisplayManager.PrintSeparateLine();
      DisplayManager.DisplaySubtotal(subtotal);

      ModeCode mode = WaitForInput();
      switch (mode) {
      case ModeCode.EXIT:
        Environment.Exit(0);
        break;
      case ModeCode.CONTINUE:
        // break out of the loop by returning from the directly
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

  /**
   * @brief Main loop for displaying the order summary.
   *  - clear the console
   *  - display the order, subtotal, tax, discount, total, and instructions
   *  - get user input and handle the input (continue, edit, apply coupon)
   *  - repeat until user wants to edit or continue
   * @return bool
   *  true if user wants to continue with the order
   *  false if user wants to edit the order
   */
  public bool Summary() {
    while (true) {
      CalculateTax();
      CalculateTotal();
      Console.Clear();
      DisplayManager.DisplayOrderSummary(this);
      DisplayManager.DisplaySummaryInstructions();

      ModeCode mode = WaitForInput();
      switch (mode) {
      case ModeCode.EDIT:
        return false;
      case ModeCode.CONTINUE:
        return true;
      case ModeCode.APPLY_COUPON:
        Console.Write("Enter coupon code: ");
        ApplyCoupon(Console.ReadLine());
        break;
      }
    }
  }

  /**
   * @brief Wait for user input and return a corresponding ModeCode
   * @return {ModeCode} representing user command.
   */
  private ModeCode WaitForInput() {
    ConsoleKeyInfo key = Console.ReadKey(true);
    return key_mode_map.ContainsKey(key.Key) ? key_mode_map[key.Key]
                                             : ModeCode.NOTHING;
  }

  /**
   * @brief Move the cursor down in the menu CLI by incrementing the cursor
   * position (0 is at the top). Reset the cursor position to the end of the
   * menu if it reaches the top.
   * @return void
   */
  public void MoveDown() { cursor_pos = (cursor_pos + 1) % items.Length; }

  /**
   * @brief Move the cursor up in the menu CLI by decrementing the cursor
   * position (0 is at the top). Reset the cursor position to the end of the
   * menu if it reaches the top.
   * @return void
   */
  public void MoveUp() {
    cursor_pos = (cursor_pos - 1 + items.Length) % items.Length;
  }

  /**
   * @brief Add an item to the order by incrementing the quantity of the item at
   * the cursor position.
   * @return void
   */
  public void AddItem() { items[cursor_pos].quantity++; }

  /**
   * @brief Remove an item from the order by decrementing the quantity of the
   * item at the cursor position. Can't remove an item if the quantity is
   * already 0.
   * @return void
   */
  public void RemoveItem() {
    if (items[cursor_pos].quantity > 0)
      items[cursor_pos].quantity--;
    // items[cursor_pos].quantity -= items[cursor_pos].quantity > 0 ? 1 : 0;
  }

  /**
   * @brief Apply a coupon to the order by setting the discount based on the
   * coupon code.
   *    - Update current_coupon state
   *    - Check if the coupon code is available
   *    - Update the discount based on the coupon code
   *    - Set discount to 0 otherwise.
   * @param {string} code Coupon code in String
   * @return void
   */
  public void ApplyCoupon(string code) {
    current_coupon = code;
    discount = 0;
    if (available_coupons.ContainsKey(code))
      discount = subtotal * available_coupons[code];
  }

  /**
   * @brief Calculate subtotal by sum up all the items' price multiplied by
   * their quantity. Update subtotal state then return it (for immediate use).
   * @return {float} subtotal
   */
  public float CalculateSubtotal() {
    subtotal = 0;
    for (int i = 0; i < items.Length; i++)
      subtotal += items[i].price * items[i].quantity;
    return subtotal;
  }

  /**
   * @brief Calculate total tax based on tax rate and subtotal.
   *  Update tax state then return it (for immediate use).
   * @return {float} total tax
   */
  public float CalculateTax() { return tax = subtotal * tax_rate; }

  /**
   * @brief Calculate total by adding tax and subtracting discount from
   * subtotal. Update total state then return it (for immediate use).
   * @return {float} total
   */
  public float CalculateTotal() { return total = subtotal + tax - discount; }

  /**
   * @brief Handle user input to save the order to a file.
   *    - Ask user if they want to save the order to a file
   *    - If yes, ask for file name, check if file already exist, and save the
   *    order to the file
   *    - If no, do nothing
   * @return void
   */
  public void HandleSaveOrder() {
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
      SaveToFile(file_name);
    }
  }

  /**
   * @brief Save the order to a file with the given file name.
   *    - Write the order summary to the file
   *    - Print a message to the console to confirm the file is saved
   * @param {string} filename File name in String
   * @return void
   */
  public void SaveToFile(string filename) {
    using (StreamWriter writer = new StreamWriter(filename + ".txt")) {
      writer.WriteLine("Your Order: \n");
      writer.WriteLine($"{"Item",-40}{"Price",-10}{"Qty",-10}");
      writer.WriteLine(new string('-', 60));

      for (int i = 0; i < items.Length; i++) {
        if (items[i].quantity <= 0)
          continue;
        writer.WriteLine(
            $"{items[i].name,-40}{items[i].price,-10:C2}{items[i].quantity,-10}");
      }

      writer.WriteLine(new string('-', 60));
      writer.WriteLine($"{"Subtotal:",-40}{subtotal:C2}");
      writer.WriteLine($"{"Tax:",-40}{tax:C2} ({tax_rate:P0})");
      writer.WriteLine(
          $"{"Discount:",-40}{discount:C2} (Coupon Code: {current_coupon})");
      writer.WriteLine(new string('-', 60));
      writer.WriteLine($"{"Total:",-40}{total:C2}");
    }

    Console.WriteLine($"Order saved to {filename}.txt");
  }
} // class Order

} // namespace OrderSystem
