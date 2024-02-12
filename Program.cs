/// Author: Sokmontrey Sythat
/// Student ID: 101477705
namespace OrderSystem {

/// <summary>
/// Main entry point of the program
/// </summary>
public class Program {

  public static void Main(string[] args) {
    Order order = new Order(
        new Dictionary<string, float> {
          // menu
          { "Burger", 5.99f },
          { "Fries", 2.99f },
          { "Drink", 1.99f },
        },
        0.13f, // 13% tax
        new Dictionary<string, float> {
          // available coupons
          { "10OFF", 0.10f },
          { "20OFF", 0.20f },
          { "30OFF", 0.30f },
        });

    while (true) {
      order.Edit();

      // user can't continue with an empty order
      if (order.subtotal == 0) {
        Console.WriteLine("Order is empty. Please add items to the order.");
        Console.Write("Press any key to continue...");
        Console.ReadKey(true);
        continue;
      }

      // user can decision to edit the order
      if (order.Summary())
        break;
    }

    Console.Clear();
    order.CalculateTotal();
    DisplayManager.DisplayOrderSummary(order);
    Console.WriteLine("\nOrder submitted!");
    Console.WriteLine("Thank you for your purchases!\n");
    order.HandleSaveOrder();
  }

} // class Program

} // namespace OrderSystem
