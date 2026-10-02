using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
namespace Database_Technical_Soloution
{
    internal class Program
    {
        const int GraphWidth = 41;
        const int graphHight = 20;
        const int graphDistFromTop = 4;
        const int graphDistFromSide = 8;
        const int graphmidpointY = 20 + graphDistFromTop;
        static string[] tableID = new string[6];
        static void Main(string[] args)
        {
            Console.WindowWidth = Console.LargestWindowWidth - 80;
            tableID[0] = "customer";
            tableID[1] = "Orders";
            tableID[2] = "OrderDetails";
            tableID[3] = "Products";
            tableID[4] = "footballClub";
            tableID[5] = "stats";
            int choice = 0;
            int orderTableChoice = 0;
            while (choice != 9)
            {
                choice = GetMainMenuChoices();
                if (choice == 1)
                {
                    switch (NonOrderTables())
                    {
                        case 1:
                            CustomerTable(0);
                            break;
                        case 2:
                            ProductTable(3);
                            break;
                        case 3:
                            FootballClubTable(4);
                            break;
                        case 4:
                            StatsTable(5);
                            break;
                        case 9:
                            break;
                        default:
                            Console.WriteLine("error no table chosen");
                            break;
                    }
                }
                else if (choice == 2)
                {
                    MakeAnOrder();
                }
                else if (choice == 3)
                {
                    Console.WriteLine("WARNING: editing either of these tables may cause bugs in the database if not done properly");
                    Console.WriteLine();
                    bool error = true;
                    while (error == true)
                    {
                        error = false;
                        try
                        {
                            Console.WriteLine("which table would you like to view :");
                            Console.WriteLine("1)database order table");
                            Console.WriteLine("2)database order details table");
                            Console.WriteLine("9) exit");
                            orderTableChoice = int.Parse(Console.ReadLine());
                            error = false;
                            while (orderTableChoice != 1 && orderTableChoice != 2 && orderTableChoice != 9)
                            {
                                Console.WriteLine("please enter a valid choice (1,2,9) ");
                                orderTableChoice = int.Parse(Console.ReadLine());
                                error = false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("error must be a number (1,2,9)");
                            Console.WriteLine();
                            error = true;
                        }
                    }
                    if (orderTableChoice == 1)
                    {
                        OrderTable(1);
                    }
                    else if (orderTableChoice == 2)
                    {
                        OrderDetailsTable(2);
                    }
                }
                else if (choice == 4)
                {
                    ViewAllOrders();
                }
                else if (choice == 5)
                {
                    // graph and staticstics
                    int ID = 0;
                    bool error = true;
                    while (error == true)
                    {
                        error = false;
                        try
                        {
                            Console.WriteLine("what action would you like to complete ");
                            Console.WriteLine("1)view a graph ");
                            Console.WriteLine("2)view statistics");
                            Console.WriteLine("9)exit to main menu");
                            ID = int.Parse(Console.ReadLine());
                            error = false;
                            while (ID != 1 && ID != 2 && ID != 9)
                            {
                                Console.WriteLine("please enter a valid choice (1,2,9) ");
                                ID = int.Parse(Console.ReadLine());
                                error = false;
                            }
                        }
                        catch
                        {
                            Console.WriteLine("error must be a number (1,2,9)");
                            Console.WriteLine();
                            error = true;
                        }
                    }
                    //graph choice
                    if (ID == 1)
                    {
                        GraphChoice(ID);
                    }
                    // statistic choice
                    else if (ID == 2)
                    {
                        ShowStatistics(ID);
                    }
                }
            }
        }
        static int NonOrderTables()
        {
            int ID = 0;
            Console.WriteLine("which table would you like to access?");
            Console.WriteLine("1)Customer table");
            Console.WriteLine("2)Product table");
            Console.WriteLine("3)Football club table");
            Console.WriteLine("4)Stats table");
            Console.WriteLine("9)Exit");
            return ID = int.Parse(Console.ReadLine());
        }
        static void ShowStatistics(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            ID = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("what stat would you like to view");
                    Console.WriteLine("1)Gets the total amount of orders each customer has made");
                    Console.WriteLine("2)Gets the full names of the customers who have orders with prices larger than the average prices of all the shirts");
                    Console.WriteLine("3)Gets the product with the most sales");
                    Console.WriteLine("4)Gets the name and address of all customers on the database who have not made an order");
                    Console.WriteLine("9)exit");
                    ID = int.Parse(Console.ReadLine());
                    error = false;
                    while (ID != 1 && ID != 2 && ID != 3 && ID != 4 && ID != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4 or 9 to exit)");
                        ID = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            if (ID != 9)
            {
                if (ID == 1)
                {
                    //Gets the total amount of orders each customer has made
                    GetTotalAmountOfOrdersEachCustomerHasMade();
                }
                else if (ID == 2)
                {
                    //Gets the full names of the customers who have orders with prices larger than the average prices of all the shirts
                    GetAllCustomersWhoMadeAnOrderAboveAverage();
                }
                else if (ID == 3)
                {
                    //most popular product
                    GetMostPopularProduct();
                }
                else if (ID == 4)
                {
                    //customers who never made an order
                    GetCustomersWhoNeverMadeAnOrder();
                }
            }
        }
        static void GetTotalAmountOfOrdersEachCustomerHasMade()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT (FirstName ||' '||LastName) AS 'fullname',count(Orders.CustomerID) AS 'total' FROM customer , Orders WHERE customer.CustomerID = Orders.CustomerID GROUP BY Orders.CustomerID", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + "| ");
            }
            while (result.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + "| ");
                }
            }
            conn.Close();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("press enter to return to main menu");
            Console.ReadKey();
        }
        static void GetAllCustomersWhoMadeAnOrderAboveAverage()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT (FirstName ||' '|| LastName) AS 'full name', price AS 'prices larger than average', (SELECT round(avg(price)) from Products)as 'average' from OrderDetails,Orders,customer ,Products WHERE OrderDetails.OrderID = Orders.OrderID and Products.ProductID = OrderDetails.ProductID AND Orders.CustomerID = customer.CustomerID AND price > (SELECT avg(price) from Products)", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + "| ");
            }
            while (result.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + "| ");
                }
            }
            conn.Close();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("press enter to return to main menu");
            Console.ReadKey();
        }
        static void GetMostPopularProduct()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT Name, count(Name) AS 'number of sales' from Orders, Products ,OrderDetails WHERE OrderDetails.ProductID = Products.ProductID AND OrderDetails.OrderID = Orders.OrderID GROUP BY Name ORDER BY count(Name) DESC", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + "| ");
            }
            while (result.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + "| ");
                }
            }
            conn.Close();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("press enter to return to main menu");
            Console.ReadKey();
        }
        static void GetCustomersWhoNeverMadeAnOrder()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT (FirstName ||' '||LastName) AS 'fullname', Address FROM customer,Orders WHERE customer.CustomerID NOT IN (SELECT Orders.CustomerID FROM customer,Orders GROUP BY Orders.CustomerID) GROUP BY customer.CustomerID", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + "| ");
            }
            while (result.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + "| ");
                }
            }
            conn.Close();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("press enter to return to main menu");
            Console.ReadKey();
        }
        static void MakeAnOrder()
        {
            Console.Clear();
            SQLiteConnection conn = CreateConnection();
            int orderID = GetMaxIncrementNum(1);
            int count = GetFieldCountForProducts();
            int productChoice = 0;
            int customerChoice = 0;
            int quanityChoice = 0;
            int addExtraToOrder = 0;
            productChoice = GetProductChoice(count);
            if (productChoice != 9)
            {
                quanityChoice = GetQunatityChoice();
                customerChoice = GetCustomerChoice(count);
                SQLiteCommand cmd1 = new SQLiteCommand("INSERT INTO orders VALUES('" + orderID + "','" + customerChoice + "','" + DateTime.Now.ToString().Substring(0, 10) + "')", conn);
                cmd1.ExecuteNonQuery();
                SQLiteCommand cmd2 = new SQLiteCommand("INSERT INTO orderDetails VALUES('" + GetMaxIncrementNum(2) + "','" + orderID + "','" + productChoice + "','" + quanityChoice + "')", conn);
                cmd2.ExecuteNonQuery();
                addExtraToOrder = GetAddMoreToOrderChoice();
                while (addExtraToOrder == 1)
                {
                    AddMoreToOrder(count, orderID);
                    addExtraToOrder = GetAddMoreToOrderChoice();
                }
                Console.WriteLine("press enter to continue");
                Console.ReadKey();
            }
        }
        static int GetProductChoice(int count)
        {
            SQLiteConnection conn = CreateConnection();
            int productChoice = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which product would you like to order ");
                    Console.WriteLine();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT ProductID,Name,size,Price FROM Products", conn);
                    SQLiteDataReader result = cmd.ExecuteReader();
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        Console.Write(result.GetName(i) + "| ");
                    }
                    while (result.Read())
                    {
                        Console.WriteLine();
                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            Console.Write(result[i] + "| ");
                        }
                    }
                    result.Close();
                    Console.WriteLine();
                    Console.WriteLine("9) exit");
                    Console.WriteLine("Please enter your choice: ");
                    productChoice = int.Parse(Console.ReadLine());
                    error = true;
                    for (int i = 1; i <= count; i++)
                    {
                        if (productChoice == i || productChoice == 9 || error == false)
                        {
                            error = false;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            return productChoice;
        }
        static int GetFieldCountForProducts()
        {
            SQLiteConnection conn = CreateConnection();
            int count = 0;
            SQLiteCommand cmd = new SQLiteCommand("SELECT ProductID,Name,size,Price FROM Products", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                count++;
            }
            return count;
        }
        static int GetQunatityChoice()
        {
            bool error = true;
            int quanityChoice = 0;
            error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("what quanity of this product do you want to buy");
                    quanityChoice = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            return quanityChoice;
        }
        static int GetCustomerChoice(int count)
        {
            int customerChoice = 0;
            SQLiteConnection conn = CreateConnection();
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which customer is making the order");
                    Console.WriteLine();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT CustomerID, FirstName,LastName FROM customer", conn);
                    SQLiteDataReader result = cmd.ExecuteReader();
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        Console.Write(result.GetName(i) + "| ");
                    }
                    while (result.Read())
                    {
                        Console.WriteLine();
                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            Console.Write(result[i] + "| ");
                        }
                        count++;
                    }
                    result.Close();
                    Console.WriteLine();
                    Console.WriteLine("Please enter your choice: ");
                    customerChoice = int.Parse(Console.ReadLine());
                    error = true;
                    for (int i = 1; i <= count; i++)
                    {
                        if (customerChoice == i || customerChoice == 9)
                        {
                            error = false;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            return customerChoice;
        }
        static int GetAddMoreToOrderChoice()
        {
            int addExtraToOrder = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("would you like to add more to this order?");
                    Console.WriteLine("1)yes");
                    Console.WriteLine("2)no");
                    addExtraToOrder = int.Parse(Console.ReadLine());
                    error = false;
                    while (addExtraToOrder != 1 && addExtraToOrder != 2)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4 or 9 to exit) ");
                        addExtraToOrder = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            return addExtraToOrder;
        }
        static void AddMoreToOrder(int count, int orderID)
        {
            SQLiteConnection conn = CreateConnection();
            int productChoice = 0;
            int quanityChoice = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which product would you like to order ");
                    Console.WriteLine();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT ProductID,Name,size,Price FROM Products", conn);
                    SQLiteDataReader result = cmd.ExecuteReader();
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        Console.Write(result.GetName(i) + "| ");
                    }
                    while (result.Read())
                    {
                        Console.WriteLine();
                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            Console.Write(result[i] + "| ");
                        }
                        count++;
                    }
                    result.Close();
                    Console.WriteLine();
                    Console.WriteLine("9) exit");
                    Console.WriteLine("Please enter your choice: ");
                    productChoice = int.Parse(Console.ReadLine());
                    error = true;
                    for (int i = 1; i <= count; i++)
                    {
                        if (productChoice == i || productChoice == 9)
                        {
                            error = false;
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("what quanity of this product do you want to buy");
                    quanityChoice = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            //only order details needed
            SQLiteCommand cmd3 = new SQLiteCommand("INSERT INTO orderDetails VALUES('" + GetMaxIncrementNum(2) + "','" + orderID + "','" + productChoice + "','" + quanityChoice + "')", conn);
            cmd3.ExecuteNonQuery();
        }
        static void ViewAllOrders()
        {
            Console.Clear();
            SQLiteConnection conn = CreateConnection();
            int certainOrdersChoice = 0;
            int viewAllOrders = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("would you like to view all orders? ");
                    Console.WriteLine("1)Yes");
                    Console.WriteLine("2)No");
                    viewAllOrders = int.Parse(Console.ReadLine());
                    error = false;
                    while (viewAllOrders != 1 && viewAllOrders != 2)
                    {
                        Console.WriteLine("please enter a valid choice (1,2) ");
                        viewAllOrders = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2)");
                    Console.WriteLine();
                    error = true;
                }
            }
            if (viewAllOrders == 1)
            {
                SelectAllOrders();
            }
            error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("would you like to view certain orders:");
                    Console.WriteLine("1)search orders from a certain customer");
                    Console.WriteLine("2)All orders that include a certain team");
                    Console.WriteLine("3)All orders with a certain price");
                    Console.WriteLine("4)All orders from a certain city");
                    Console.WriteLine("9) exit");
                    Console.WriteLine("Please enter your choice: ");
                    certainOrdersChoice = int.Parse(Console.ReadLine());
                    error = false;
                    while (certainOrdersChoice != 1 && certainOrdersChoice != 2 && certainOrdersChoice != 3 && certainOrdersChoice != 4 && certainOrdersChoice != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4 or 9 to exit) ");
                        certainOrdersChoice = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            if (certainOrdersChoice == 1)
            {
                GetCertainOrderByCustomer();
            }
            else if (certainOrdersChoice == 2)
            {
                GetCertainOrderByClub();
            }
            else if (certainOrdersChoice == 3)
            {
                GetCertainOrderByPriceOfShirt();
            }
            else if (certainOrdersChoice == 4)
            {
                GetCertainOrderByCity();
            }
            Console.WriteLine();
            Console.WriteLine("press enter to continue");
            Console.ReadKey();
            conn.Close();
        }
        static void SelectAllOrders()
        {
            SQLiteConnection conn = CreateConnection();
            Console.WriteLine("all orders:");
            Console.WriteLine();
            SQLiteCommand cmd = new SQLiteCommand("SELECT Orders.OrderID, FirstName || ' ' || LastName as 'Full Name',Name as 'Product Name',Products.price,city,OrderDate,ClubName,quantity,size FROM customer , Products,Orders ,FootballClub,OrderDetails WHERE Products.ProductID = OrderDetails.ProductID AND Orders.OrderID = OrderDetails.OrderID AND FootballClub.clubID = Products.clubID AND customer.CustomerID = Orders.CustomerID GROUP BY OrderDetails.OrderDetailsID", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + "| ");
            }
            while (result.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i].ToString().PadRight(1) + "| ");
                }
            }
            result.Close();
            Console.WriteLine();
            Console.WriteLine();
        }
        static void GetCertainOrderByCustomer()
        {
            SQLiteConnection conn = CreateConnection();
            string space = "      ";
            string search = "";
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            int rowCount = 0;
            Console.WriteLine("search for a certain customers order by name(not case sensitive):");
            search = Console.ReadLine();
            SQLiteCommand cmd1 = new SQLiteCommand("SELECT Orders.OrderID, FirstName || ' ' || LastName as 'Full Name',Name as 'Product Name',Products.price,city,OrderDate,ClubName,quantity,size FROM customer , Products,Orders ,FootballClub,OrderDetails WHERE Products.ProductID = OrderDetails.ProductID AND Orders.OrderID = OrderDetails.OrderID AND FootballClub.clubID = Products.clubID AND customer.CustomerID = Orders.CustomerID AND FirstName || ' ' || LastName like'" + search + "%' GROUP BY OrderDetails.OrderDetailsID", conn);
            SQLiteDataReader result1 = cmd1.ExecuteReader();
            while (result1.Read())
            {
                for (int i = 0; i < result1.FieldCount; i++)
                {
                    //adds the title of the collumn from the table and the row as the key and then stores the data from that row and collumn as the value(from the dictionary)
                    // this allows me to store the entire table in a dictionary
                    attributes.Add(result1.GetName(i) + rowCount, result1[i]);
                }
                rowCount++;
            }
            for (int i = 0; i < rowCount; i++)
            {
                Console.Write("Order:" + attributes["OrderID" + i] + space + attributes["OrderDate" + i]);
                Console.WriteLine();
                for (int j = 0; j < 100; j++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                Console.WriteLine(attributes["Full Name" + i] + space + attributes["city" + i]);
                Console.WriteLine();
                Console.WriteLine(attributes["Product Name" + i] + "(" + attributes["ClubName" + i] + ")" + space + "£" + (attributes["Price" + i]) + space + "size:" + attributes["size" + i] + space + "quantity:" + attributes["quantity" + i]);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        static void GetCertainOrderByClub()
        {
            SQLiteConnection conn = CreateConnection();
            string search = "";
            Console.WriteLine("search for a certain football club's orders by name(not case sensitive):");
            search = Console.ReadLine();
            SQLiteCommand cmd1 = new SQLiteCommand("SELECT Orders.OrderID, FirstName || ' ' || LastName as 'Full Name',Name as 'Product Name',Products.price,city,OrderDate,ClubName,quantity,size FROM customer , Products,Orders ,FootballClub,OrderDetails WHERE Products.ProductID = OrderDetails.ProductID AND Orders.OrderID = OrderDetails.OrderID AND FootballClub.clubID = Products.clubID AND customer.CustomerID = Orders.CustomerID AND ClubName like'" + search + "%' GROUP BY OrderDetails.OrderDetailsID", conn);
            SQLiteDataReader result1 = cmd1.ExecuteReader();
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            int rowCount = 0;
            string space = "      ";
            while (result1.Read())
            {
                for (int i = 0; i < result1.FieldCount; i++)
                {
                    attributes.Add(result1.GetName(i) + rowCount, result1[i]);
                }
                rowCount++;
            }
            Console.WriteLine();
            for (int i = 0; i < rowCount; i++)
            {
                Console.Write("Order:" + attributes["OrderID" + i] + space + attributes["OrderDate" + i]);
                Console.WriteLine();
                for (int j = 0; j < 100; j++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                Console.WriteLine(attributes["Full Name" + i] + space + attributes["city" + i]);
                Console.WriteLine();
                Console.WriteLine(attributes["Product Name" + i] + "(" + attributes["ClubName" + i] + ")" + space + "£" + (attributes["Price" + i]) + space + "size:" + attributes["size" + i] + space + "quantity:" + attributes["quantity" + i]);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        static void GetCertainOrderByPriceOfShirt()
        {
            SQLiteConnection conn = CreateConnection();
            string search = "";
            Console.WriteLine("search for a certain order by price:");
            search = Console.ReadLine();
            SQLiteCommand cmd1 = new SQLiteCommand("SELECT Orders.OrderID, FirstName || ' ' || LastName as 'Full Name',Name as 'Product Name',Products.price,city,OrderDate,ClubName,quantity,size FROM customer , Products,Orders ,FootballClub,OrderDetails WHERE Products.ProductID = OrderDetails.ProductID AND Orders.OrderID = OrderDetails.OrderID AND FootballClub.clubID = Products.clubID AND customer.CustomerID = Orders.CustomerID AND Price like'" + search + "%' GROUP BY OrderDetails.OrderDetailsID", conn);
            SQLiteDataReader result1 = cmd1.ExecuteReader();
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            int rowCount = 0;
            string space = "      ";
            while (result1.Read())
            {
                for (int i = 0; i < result1.FieldCount; i++)
                {
                    attributes.Add(result1.GetName(i) + rowCount, result1[i]);
                }
                rowCount++;
            }
            Console.WriteLine();
            for (int i = 0; i < rowCount; i++)
            {
                Console.Write("Order:" + attributes["OrderID" + i] + space + attributes["OrderDate" + i]);
                Console.WriteLine();
                for (int j = 0; j < 100; j++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                Console.WriteLine(attributes["Full Name" + i] + space + attributes["city" + i]);
                Console.WriteLine();
                Console.WriteLine(attributes["Product Name" + i] + "(" + attributes["ClubName" + i] + ")" + space + "£" + (attributes["Price" + i]) + space + "size:" + attributes["size" + i] + space + "quantity:" + attributes["quantity" + i]);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        static void GetCertainOrderByCity()
        {
            SQLiteConnection conn = CreateConnection();
            string search = "";
            Console.WriteLine("search for a certain order by city(not case sensitive):");
            search = Console.ReadLine();
            SQLiteCommand cmd1 = new SQLiteCommand("SELECT Orders.OrderID, FirstName || ' ' || LastName as 'Full Name',Name as 'Product Name',Products.price,city,OrderDate,ClubName,quantity,size FROM customer , Products,Orders ,FootballClub,OrderDetails WHERE Products.ProductID = OrderDetails.ProductID AND Orders.OrderID = OrderDetails.OrderID AND FootballClub.clubID = Products.clubID AND customer.CustomerID = Orders.CustomerID AND city like'" + search + "%' GROUP BY OrderDetails.OrderDetailsID", conn);
            SQLiteDataReader result1 = cmd1.ExecuteReader();
            Dictionary<string, object> attributes = new Dictionary<string, object>();
            int rowCount = 0;
            string space = "      ";
            while (result1.Read())
            {
                for (int i = 0; i < result1.FieldCount; i++)
                {
                    attributes.Add(result1.GetName(i) + rowCount, result1[i]);
                }
                rowCount++;
            }
            Console.WriteLine();
            for (int i = 0; i < rowCount; i++)
            {
                Console.Write("Order:" + attributes["OrderID" + i] + space + attributes["OrderDate" + i]);
                Console.WriteLine();
                for (int j = 0; j < 100; j++)
                {
                    Console.Write("-");
                }
                Console.WriteLine();
                Console.WriteLine(attributes["Full Name" + i] + space + attributes["city" + i]);
                Console.WriteLine();
                Console.WriteLine(attributes["Product Name" + i] + "(" + attributes["ClubName" + i] + ")" + space + "£" + (attributes["Price" + i]) + space + "size:" + attributes["size" + i] + space + "quantity:" + attributes["quantity" + i]);
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
            }
        }
        static int GetMainMenuChoices()
        {
            Console.Clear();
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("which action do you want to complete?");
            Console.WriteLine("------------");
            Console.WriteLine("1)Non-order tables");
            Console.WriteLine("2)Make an order");
            Console.WriteLine("3)order tables");
            Console.WriteLine("4)View all orders");
            Console.WriteLine("5)view graphs and statistics");
            Console.WriteLine("9)exit");
            int ID = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.Write("Please enter your choice: ");
                    ID = int.Parse(Console.ReadLine());
                    error = false;
                    while (ID != 1 && ID != 2 && ID != 3 && ID != 4 && ID != 5 && ID != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4,5 or 9 to exit) ");
                        ID = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4,5 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            Console.WriteLine();
            return ID;
        }
        static int GetSecondMenuChoices()
        {
            Console.Clear();
            Console.WriteLine("what action would you like to complete");
            Console.WriteLine("1)view the table");
            Console.WriteLine("2)insert values into the table");
            Console.WriteLine("3)edit the records in the table");
            Console.WriteLine("4)alter the table's columnns");
            Console.WriteLine("9)main menu");
            int ID = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine();
                    Console.Write("Please enter your choice: ");
                    ID = int.Parse(Console.ReadLine());
                    error = false;
                    while (ID != 1 && ID != 2 && ID != 3 && ID != 4 && ID != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4 or 9 to exit) ");
                        ID = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            Console.WriteLine();
            Console.Clear();
            return ID;
        }
        static void ViewTable(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * from " + tableID[ID], conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + "| ");
            }
            while (result.Read())
            {
                Console.WriteLine();
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + "| ");
                }
            }
            conn.Close();
            Console.WriteLine();
            Console.WriteLine("press enter to return to main menu");
            Console.ReadKey();
        }
        static void EditTable(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            List<string> tableIDString = new List<string>();
            int columnnToChangeInt = 0;
            string newEntry = "";
            int IdToChange = 0;
            SQLiteCommand cmd = new SQLiteCommand("SELECT * from " + tableID[ID], conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            for (int i = 0; i < result.FieldCount; i++)
            {
                Console.Write(result.GetName(i) + " ");
                tableIDString.Add(result.GetName(i));
            }
            Console.WriteLine();
            Console.WriteLine();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("enter which column would you like to edit (listed below)");
                    Console.WriteLine();
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        Console.Write((i + 1) + ")" + result.GetName(i) + " ");
                    }
                    Console.Write("9)exit");
                    columnnToChangeInt = int.Parse(Console.ReadLine());
                    error = false;
                    while (columnnToChangeInt != 1 && columnnToChangeInt != 2 && columnnToChangeInt != 3 && columnnToChangeInt != 4 && columnnToChangeInt != 5 && columnnToChangeInt != 6 && columnnToChangeInt != 7 && columnnToChangeInt != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4,5,6,7 or 9 to exit) ");
                        columnnToChangeInt = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4,5,6,7 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            if (columnnToChangeInt != 9)
            {
                error = true;
                while (error == true)
                {
                    error = false;
                    try
                    {
                        Console.WriteLine("which ID does the data entry have that you want to change have");
                        IdToChange = int.Parse(Console.ReadLine());
                        error = false;
                    }
                    catch
                    {
                        Console.WriteLine("error must be a number ");
                        Console.WriteLine();
                        error = true;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("what would you like to change this to");
                newEntry = Console.ReadLine();
                SQLiteCommand cmd1 = new SQLiteCommand("UPDATE " + tableID[ID] + " SET " + tableIDString[columnnToChangeInt - 1] + " = '" + newEntry + "' WHERE " + result.GetName(0) + " = " + IdToChange, conn);
                cmd1.ExecuteReader();
            }
            Console.WriteLine("press enter to continue");
            Console.ReadKey();
        }
        static void AlterTable(int ID)
        {
            int choice = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("would you like to do to the " + tableID[ID] + " table:");
                    Console.WriteLine("1)add a column");
                    Console.WriteLine("2)remove a column");
                    Console.WriteLine("3)rename a column");
                    Console.WriteLine("9)exit");
                    choice = int.Parse(Console.ReadLine());
                    error = false;
                    while (choice != 1 && choice != 2 && choice != 3 && choice != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2 or 3) ");
                        choice = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3)");
                    Console.WriteLine();
                    error = true;
                }
            }
            switch (choice)
            {
                case 1:
                    AddColumn(ID);
                    break;
                case 2:
                    RemoveColumn(ID);
                    break;
                case 3:
                    RenameTable(ID);
                    break;
                case 9:
                    break;
                default:
                    throw new Exception();
            }
        }
        static void AddColumn(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            string columnName = "";
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("what would you like to call the new column?");
                    columnName = Console.ReadLine();
                    error = false;
                }
                catch
                {
                    Console.WriteLine("error must be a string ");
                    Console.WriteLine();
                    error = true;
                }
            }
            Console.WriteLine("the data type will be string (VARCHAR)");
            Console.WriteLine("press enter to continue ");
            Console.ReadKey();
            SQLiteCommand cmd = new SQLiteCommand("ALTER TABLE " + tableID[ID] + " ADD " + columnName + " VARCHAR", conn);
            cmd.ExecuteNonQuery();
        }
        static void RemoveColumn(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            int columnIndex = 0;
            List<string> columnIDString = new List<string>();
            bool error = true;
            bool errorPrint = false;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which column would you like to remove");
                    SQLiteCommand cmd1 = new SQLiteCommand("SELECT * from " + tableID[ID], conn);
                    SQLiteDataReader result = cmd1.ExecuteReader();
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        Console.WriteLine((i + 1) + ")" + result.GetName(i));
                        columnIDString.Add(result.GetName(i));
                    }
                    columnIndex = int.Parse(Console.ReadLine());
                    error = false;
                    errorPrint = true;
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        if (columnIndex == i + 1)
                        {
                            errorPrint = false;
                        }
                    }
                    while (errorPrint == true)
                    {
                        Console.WriteLine("please enter a valid number");
                        for (int i = 0; i < columnIDString.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ")" + columnIDString[i]);
                        }
                        columnIndex = int.Parse(Console.ReadLine());
                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            if (columnIndex == i + 1)
                            {
                                errorPrint = false;
                            }
                        }
                    }
                    result.Close();
                }
                catch
                {
                    Console.WriteLine("error must be a number");
                    Console.WriteLine();
                    error = true;
                }
            }
            SQLiteCommand cmd6 = new SQLiteCommand("ALTER TABLE " + tableID[ID] + " DROP COLUMN " + columnIDString[columnIndex - 1], conn);
            cmd6.ExecuteNonQuery();
            Console.WriteLine("press enter to continue");
            Console.ReadKey();
        }
        static void RenameTable(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            int columnIndex = 0;
            string newColumnName = "";
            List<string> columnIDString = new List<string>();
            bool error = true;
            bool errorPrint = false;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which column would you like to rename?");
                    SQLiteCommand cmd3 = new SQLiteCommand("SELECT * from " + tableID[ID], conn);
                    SQLiteDataReader result1 = cmd3.ExecuteReader();
                    for (int i = 0; i < result1.FieldCount; i++)
                    {
                        Console.WriteLine((i + 1) + ")" + result1.GetName(i));
                        columnIDString.Add(result1.GetName(i));
                    }
                    columnIndex = int.Parse(Console.ReadLine());
                    error = false;
                    errorPrint = true;
                    for (int i = 0; i < result1.FieldCount; i++)
                    {
                        if (columnIndex == i + 1)
                        {
                            errorPrint = false;
                        }
                    }
                    while (errorPrint == true)
                    {
                        Console.WriteLine("please enter a valid number");
                        for (int i = 0; i < columnIDString.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ")" + columnIDString[i]);
                        }
                        columnIndex = int.Parse(Console.ReadLine());
                        for (int i = 0; i < result1.FieldCount; i++)
                        {
                            if (columnIndex == i + 1)
                            {
                                errorPrint = false;
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3)");
                    Console.WriteLine();
                    error = true;
                }
            }
            Console.WriteLine("what would you like to rename it to?");
            newColumnName = Console.ReadLine();
            SQLiteCommand cmd4 = new SQLiteCommand("ALTER TABLE " + tableID[ID] + " RENAME COLUMN " + columnIDString[columnIndex - 1] + " to " + newColumnName, conn);
            cmd4.ExecuteNonQuery();
        }
        static void LastRowOfDataEntered(int ID)
        {
            if (ID == 0)
            {
                GetLastRowOfDataEnteredCustomer(ID);
            }
            else if (ID == 1)
            {
                GetLastRowOfDataEnteredOrder(ID);
            }
            else if (ID == 2)
            {
                GetLastRowOfDataEnteredOrderDetails(ID);
            }
            else if (ID == 3)
            {
                GetLastRowOfDataEnteredProduct(ID);
            }
            else if (ID == 4)
            {
                GetLastRowOfDataEnteredFootballClub(ID);
            }
            Console.WriteLine();
        }
        static void GetLastRowOfDataEnteredCustomer(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + tableID[ID] + " WHERE CustomerID =(SELECT max(customerID) FROM customer)", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + " ");
                }
            }
            result.Close();
            conn.Close();
        }//get last row of data entered unique to each table (below)
        static void GetLastRowOfDataEnteredOrder(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + tableID[ID] + " WHERE OrderID =(SELECT max(OrderID) FROM " + tableID[ID] + ")", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + " ");
                }
            }
            result.Close();
            conn.Close();
        }
        static void GetLastRowOfDataEnteredOrderDetails(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + tableID[ID] + " WHERE OrderDetailsID =(SELECT max(OrderDetailsID) FROM " + tableID[ID] + ")", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + " ");
                }
            }
            result.Close();
            conn.Close();
        }
        static void GetLastRowOfDataEnteredProduct(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + tableID[ID] + " WHERE ProductID =(SELECT max(ProductID) FROM " + tableID[ID] + ")", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + " ");
                }
            }
            result.Close();
            conn.Close();
        }
        static void GetLastRowOfDataEnteredFootballClub(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + tableID[ID] + " WHERE ClubID =(SELECT max(ClubID) FROM " + tableID[ID] + ")", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    Console.Write(result[i] + " ");
                }
            }
            result.Close();
            conn.Close();
        }
        static void InsertIntoTable(int ID)
        {
            string[] dataEntries = new string[8];
            switch (ID)
            {
                case 0:
                    //customers
                    InsertIntoCustomer(ID);
                    break;
                case 1:
                    //orders
                    InsertIntoOrders(ID);
                    break;
                case 2:
                    //order details
                    InsertIntoOrderDetails(ID);
                    break;
                case 3:
                    //products
                    InsertIntoProducts(ID);
                    break;
                case 4:
                    //football club
                    InsertIntoFootballClub(ID);
                    break;
                case 5:
                    //stats table
                    InsertIntoStatsTable();
                    break;
                default:
                    throw new Exception("error incorrect table ID");
            }
            Console.WriteLine("press enter to return to main menu");
            Console.ReadKey();
        }
        static void InsertIntoCustomer(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            string[] dataEntries = new string[8];
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(ID);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column : firstname,lastname,address,postal code, country,city (Customer ID will be automatically incremented)");
            Console.WriteLine("enter first name:");
            dataEntries[0] = Console.ReadLine();
            Console.WriteLine("enter last name:");
            dataEntries[1] = Console.ReadLine();
            Console.WriteLine("enter address:");
            dataEntries[2] = Console.ReadLine();
            Console.WriteLine("enter postage code:");
            dataEntries[3] = Console.ReadLine();
            Console.WriteLine("enter country:");
            dataEntries[4] = Console.ReadLine();
            Console.WriteLine("enter city:");
            dataEntries[5] = Console.ReadLine();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + tableID[ID] + " VALUES('" + GetMaxIncrementNum(ID) + "','" + dataEntries[0] + "','" + dataEntries[1] + "','" + dataEntries[2] + "',' " + dataEntries[3] + "','" + dataEntries[4] + "','" + dataEntries[5] + "')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        static void InsertIntoOrders(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            string[] dataEntries = new string[8];
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(ID);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column : customer ID (order ID will be automatically incremented and order Date will be today's date)");
            Console.WriteLine("enter customer ID:");
            dataEntries[0] = Console.ReadLine();
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + tableID[ID] + " VALUES('" + GetMaxIncrementNum(ID) + "','" + dataEntries[0] + "','" + DateTime.Now.ToString().Substring(0, 10) + "')", conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine("you must now enter data into order details to make sure the database logic still works");
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(2);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column : Product ID,quantity (order details ID will be automatically incremented)");
            Console.WriteLine("enter product ID:");
            dataEntries[0] = Console.ReadLine();
            Console.WriteLine("enter quantity:");
            dataEntries[1] = Console.ReadLine();
            SQLiteCommand cmd2 = new SQLiteCommand("INSERT INTO " + tableID[2] + " VALUES('" + GetMaxIncrementNum(2) + "','" + GetMaxIncrementNum(2) + "','" + dataEntries[0] + "','" + dataEntries[1] + "')", conn);
            cmd2.ExecuteNonQuery();
            conn.Close();
        }
        static void InsertIntoOrderDetails(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            string[] dataEntries = new string[8];
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(ID);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column : customer ID (order ID will be automatically incremented and order Date will be today's date)");
            Console.WriteLine("enter customer ID:");
            dataEntries[0] = Console.ReadLine();
            SQLiteCommand cmd3 = new SQLiteCommand("INSERT INTO " + tableID[ID] + " VALUES('" + GetMaxIncrementNum(ID) + "','" + dataEntries[0] + "','" + DateTime.Now.ToString().Substring(0, 10) + "')", conn);
            cmd3.ExecuteNonQuery();
            Console.WriteLine("now you can enter data into order details");
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(ID);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column : Product ID,quantity (order details ID will be automatically incremented)");
            Console.WriteLine("enter product ID:");
            dataEntries[0] = Console.ReadLine();
            Console.WriteLine("enter quantity:");
            dataEntries[1] = Console.ReadLine();
            SQLiteCommand cmd4 = new SQLiteCommand("INSERT INTO " + tableID[2] + " VALUES('" + GetMaxIncrementNum(2) + "','" + GetMaxIncrementNum(2) + "','" + dataEntries[0] + "','" + dataEntries[1] + "')", conn);
            cmd4.ExecuteNonQuery();
            conn.Close();
        }
        static void InsertIntoProducts(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            string[] dataEntries = new string[8];
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(ID);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column : Name,stock,description,size,price,clubID (product ID will be automatically incremented)");
            Console.WriteLine("enter name:");
            dataEntries[0] = Console.ReadLine();
            Console.WriteLine("enter stock:");
            dataEntries[1] = Console.ReadLine();
            Console.WriteLine("enter description:");
            dataEntries[2] = Console.ReadLine();
            Console.WriteLine("enter size:");
            dataEntries[3] = Console.ReadLine();
            Console.WriteLine("enter price:");
            dataEntries[4] = Console.ReadLine();
            Console.WriteLine("enter clubID:");
            dataEntries[5] = Console.ReadLine();
            SQLiteCommand cmd5 = new SQLiteCommand("INSERT INTO " + tableID[ID] + " VALUES('" + GetMaxIncrementNum(ID) + "','" + dataEntries[0] + "','" + dataEntries[1] + "','" + dataEntries[2] + "',' " + dataEntries[3] + "',' " + dataEntries[4] + "',' " + dataEntries[5] + "')", conn);
            cmd5.ExecuteNonQuery();
            conn.Close();
        }
        static void InsertIntoFootballClub(int ID)
        {
            SQLiteConnection conn = CreateConnection();
            string[] dataEntries = new string[8];
            Console.WriteLine("the last row of data entered:");
            LastRowOfDataEntered(ID);
            Console.WriteLine();
            Console.WriteLine("please enter data for each column :Club Name (Club ID will be automatically incremented)");
            Console.WriteLine("enter club name:");
            dataEntries[0] = Console.ReadLine();
            SQLiteCommand cmd6 = new SQLiteCommand("INSERT INTO " + tableID[ID] + " VALUES('" + GetMaxIncrementNum(ID) + "','" + dataEntries[0] + "')", conn);
            cmd6.ExecuteNonQuery();
            conn.Close();
        }
        static void InsertIntoStatsTable()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd7 = new SQLiteCommand("INSERT INTO stats(totalProduct, totalCustomer, totalOrder,DateOfEntry) VALUES((SELECT COUNT(*) FROM Products), (SELECT COUNT(*) FROM customer), (SELECT COUNT(*) FROM Orders),'" + DateTime.Now.ToString().Substring(0, 10) + "')", conn);
            cmd7.ExecuteNonQuery();
            conn.Close();
        }
        static int GetMaxIncrementNum(int ID)
        {
            if (ID == 0)
            {
                return GetMaxIncrementNumCustomerID();
            }
            else if (ID == 1)
            {
                return GetMaxIncrementNumOrderID();
            }
            else if (ID == 2)
            {
                return GetMaxIncrementNumOrderDetailsID();
            }
            else if (ID == 3)
            {
                return GetMaxIncrementNumProductID();
            }
            else if (ID == 4)
            {
                return GetMaxIncrementNumclubID();
            }
            else
            {
                throw new Exception("error incorrect table ID");
            }
        }
        static int GetMaxIncrementNumCustomerID()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT max(customerID) FROM customer", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            cmd = conn.CreateCommand();
            int myReader = 0;
            while (result.Read())
            {
                myReader = result.GetInt16(0);
            }
            result.Close();
            conn.Close();
            myReader++;
            return myReader;
        }
        static int GetMaxIncrementNumOrderID()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT max(OrderID) FROM Orders", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            cmd = conn.CreateCommand();
            int myReader = 0;
            while (result.Read())
            {
                myReader = result.GetInt16(0);
            }
            result.Close();
            conn.Close();
            myReader++;
            return myReader;
        }
        static int GetMaxIncrementNumOrderDetailsID()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT max(OrderDetailsID) FROM OrderDetails", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            cmd = conn.CreateCommand();
            int myReader = 0;
            while (result.Read())
            {
                myReader = result.GetInt16(0);
            }
            result.Close();
            conn.Close();
            myReader++;
            return myReader;
        }
        static int GetMaxIncrementNumProductID()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT max(ProductID) FROM Products", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            cmd = conn.CreateCommand();
            int myReader = 0;
            while (result.Read())
            {
                myReader = result.GetInt16(0);
            }
            result.Close();
            conn.Close();
            myReader++;
            return myReader;
        }
        static int GetMaxIncrementNumclubID()
        {
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT max(clubID) FROM FootballClub", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            int myReader = 0;
            while (result.Read())
            {
                myReader = result.GetInt16(0);
            }
            result.Close();
            conn.Close();
            myReader++;
            return myReader;
        }
        static void GraphChoice(int ID)
        {
            ID = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which graph would you like to view ");
                    Console.WriteLine("1) The sum of profit from each day");
                    Console.WriteLine("2) The price of the shirt that was sold each day");
                    Console.WriteLine("3) The total products each day");
                    Console.WriteLine("4) The total amount of customers each day");
                    Console.WriteLine("5) The total amount of orders each day");
                    Console.WriteLine("9) exit to main menu");
                    ID = int.Parse(Console.ReadLine());
                    error = false;
                    while (ID != 1 && ID != 2 && ID != 3 && ID != 4 && ID != 5 && ID != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4,5,9) ");
                        ID = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4,5)");
                    Console.WriteLine();
                    error = true;
                }
            }
            if (ID != 9)
            {
                double[,] BothAxis = GetAxis(ID);
                double[] x = new double[BothAxis.GetUpperBound(0) + 1];
                double[] y = new double[BothAxis.GetUpperBound(0) + 1];
                for (int i = 0; i < BothAxis.GetUpperBound(0) + 1; i++)
                {
                    y[i] = BothAxis[i, 0];
                    x[i] = BothAxis[i, 1];
                }
                Console.Clear();
                PrintGraph(x, y, y.Length, ID);
                //// test data 1
                //double[] testX = new double[24] { 16, 17, 18, 19, 20, 21, 13, 14, 5, 7, 20, 9, 1, 24, 1, 2, 3, 4, 5, 6, 7, 8, 9, 15 };
                //double[] testY = new double[24] { 16, 17, 18, 19, 20, 21, 5, 4, 6, 20, 7, 1, 1, 24, 1, 2, 3, 4, 5, 6, 7, 8, 9, 15 };
                //// test data 2
                //double[] testX = new double[12] { 16, 17, 20, 30, 40, 50, 60, 70, 100, 200, 500, 1000 };
                //double[] testY = new double[12] { 16, 17, 20, 30, 40, 50, 60, 70, 100, 200, 500, 1000 };
                //PrintGraph(testX, testY, testY.Length, ID);
            }
        }
        static string GetMonthOfData()
        {
            int month = 0;
            string monthString = "0";
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("which month would you like to view?");
                    Console.WriteLine("1)January");
                    Console.WriteLine("2)February");
                    Console.WriteLine("3)March");
                    Console.WriteLine("4)April");
                    Console.WriteLine("5)May");
                    Console.WriteLine("6)June");
                    Console.WriteLine("7)July");
                    Console.WriteLine("8)August");
                    Console.WriteLine("9)September");
                    Console.WriteLine("10)October");
                    Console.WriteLine("11)November");
                    Console.WriteLine("12)December");
                    month = int.Parse(Console.ReadLine());
                    error = false;
                    while (month != 1 && month != 2 && month != 3 && month != 4 && month != 5 && month != 6 && month != 7 && month != 8 && month != 9 && month != 10 && month != 11 && month != 12)
                    {
                        Console.WriteLine("please enter a valid choice (1,2,3,4,5,6,7,8,9,10,11 or 12 ");
                        month = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2,3,4,5,6,7,8,9,10,11 or 12");
                    Console.WriteLine();
                    error = true;
                }
            }
            if (month.ToString().Length > 1)
            {
                monthString = month.ToString();
            }
            else
            {
                monthString += month.ToString();
            }
            return monthString;
        }
        static double[,] GetAxis(int ID)
        {
            List<string> getData = new List<string>();
            List<double> Xaxis = new List<double>();
            List<double> Yaxis = new List<double>();
            string monthString = GetMonthOfData();
            if (ID == 1)
            {
                return GetSumOfProfitForGraph(getData, Xaxis, Yaxis, monthString);
            }
            else if (ID == 2)
            {
                return GetPriceOfShirtForGraph(getData, Xaxis, Yaxis, monthString);
            }
            else if (ID == 3)
            {
                return GetTotalProductsForGraph(getData, Xaxis, Yaxis, monthString);
            }
            else if (ID == 4)
            {
                return GetTotalAmountOfCustomersForGraph(getData, Xaxis, Yaxis, monthString);
            }
            else if (ID == 5)
            {
                return GetTotalAmountOfOrdersForGraph(getData, Xaxis, Yaxis, monthString);
            }
            else
            {
                throw new Exception("graph does not exist");
            }
        }
        static double[,] GetSumOfProfitForGraph(List<string> getData, List<double> Xaxis, List<double> Yaxis, string monthString)
        {
            SQLiteConnection conn = CreateConnection();
            //1) The sum of profit from each day
            SQLiteCommand cmd = new SQLiteCommand("SELECT sum(price),OrderDate FROM customer,Orders,OrderDetails ,Products WHERE customer.CustomerID = Orders.CustomerID AND Orders.OrderID = OrderDetails.OrderID AND OrderDetails.ProductID = Products.ProductID AND substr(OrderDate,4,2)='" + monthString + "' GROUP BY OrderDate", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    getData.Add(result[i].ToString());
                }
            }
            conn.Close();
            for (int i = 0; i < getData.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Yaxis.Add(double.Parse(getData[i]));
                }
                else
                {
                    Xaxis.Add(double.Parse(getData[i].Substring(0, 2)));
                }
            }
            double[,] dataOut = new double[Yaxis.Count, 2];
            for (int i = 0; i < Yaxis.Count; i++)
            {
                dataOut[i, 0] = Yaxis[i];
                dataOut[i, 1] = Xaxis[i];
            }
            return dataOut;
        }
        static double[,] GetPriceOfShirtForGraph(List<string> getData, List<double> Xaxis, List<double> Yaxis, string monthString)
        {
            SQLiteConnection conn = CreateConnection();
            //2) The price of each shirt sold each day
            SQLiteCommand cmd = new SQLiteCommand("SELECT price, OrderDate from Orders ,OrderDetails, Products WHERE substr(OrderDate,4,2)='" + monthString + "' AND Products.ProductID = OrderDetails.ProductID AND Orders.OrderID = OrderDetails.OrderID", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    getData.Add(result[i].ToString());
                }
            }
            conn.Close();
            for (int i = 0; i < getData.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Yaxis.Add(double.Parse(getData[i]));
                }
                else
                {
                    Xaxis.Add(double.Parse(getData[i].Substring(0, 2)));
                }
            }
            double[,] dataOut = new double[Yaxis.Count, 2];
            for (int i = 0; i < Yaxis.Count; i++)
            {
                dataOut[i, 0] = Yaxis[i];
                dataOut[i, 1] = Xaxis[i];
            }
            return dataOut;
        }
        static double[,] GetTotalProductsForGraph(List<string> getData, List<double> Xaxis, List<double> Yaxis, string monthString)
        {
            //3) The total products each day
            SQLiteConnection conn = CreateConnection();
            SQLiteCommand cmd = new SQLiteCommand("SELECT totalProduct, DateOfEntry from stats WHERE substr(DateOfEntry,4,2)='" + monthString + "'", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    getData.Add(result[i].ToString());
                }
            }
            conn.Close();
            for (int i = 0; i < getData.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Yaxis.Add(double.Parse(getData[i]));
                }
                else
                {
                    Xaxis.Add(double.Parse(getData[i].Substring(0, 2)));
                }
            }
            double[,] dataOut = new double[Yaxis.Count, 2];
            for (int i = 0; i < Yaxis.Count; i++)
            {
                dataOut[i, 0] = Yaxis[i];
                dataOut[i, 1] = Xaxis[i];
            }
            return dataOut;
        }
        static double[,] GetTotalAmountOfCustomersForGraph(List<string> getData, List<double> Xaxis, List<double> Yaxis, string monthString)
        {
            SQLiteConnection conn = CreateConnection();
            //4) The total amount of customers each day
            SQLiteCommand cmd = new SQLiteCommand("SELECT totalCustomer, DateOfEntry from stats WHERE substr(DateOfEntry,4,2)='" + monthString + "'", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    getData.Add(result[i].ToString());
                }
            }
            conn.Close();
            for (int i = 0; i < getData.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Yaxis.Add(double.Parse(getData[i]));
                }
                else
                {
                    Xaxis.Add(double.Parse(getData[i].Substring(0, 2)));
                }
            }
            double[,] dataOut = new double[Yaxis.Count, 2];
            for (int i = 0; i < Yaxis.Count; i++)
            {
                dataOut[i, 0] = Yaxis[i];
                dataOut[i, 1] = Xaxis[i];
            }
            return dataOut;
        }
        static double[,] GetTotalAmountOfOrdersForGraph(List<string> getData, List<double> Xaxis, List<double> Yaxis, string monthString)
        {
            SQLiteConnection conn = CreateConnection();
            //5) The total amount of orders each day
            SQLiteCommand cmd = new SQLiteCommand("SELECT totalOrder, DateOfEntry from stats WHERE substr(DateOfEntry,4,2)='" + monthString + "'", conn);
            SQLiteDataReader result = cmd.ExecuteReader();
            while (result.Read())
            {
                for (int i = 0; i < result.FieldCount; i++)
                {
                    getData.Add(result[i].ToString());
                }
            }
            conn.Close();
            for (int i = 0; i < getData.Count; i++)
            {
                if (i % 2 == 0)
                {
                    Yaxis.Add(double.Parse(getData[i]));
                }
                else
                {
                    Xaxis.Add(double.Parse(getData[i].Substring(0, 2)));
                }
            }
            double[,] dataOut = new double[Yaxis.Count, 2];
            for (int i = 0; i < Yaxis.Count; i++)
            {
                dataOut[i, 0] = Yaxis[i];
                dataOut[i, 1] = Xaxis[i];
            }
            return dataOut;
        }
        static void PrintGraph(double[] x, double[] y, int n, int ID)
        {
            // prints graph to console
            Console.WindowHeight = 40;
            //printing out the axis
            Console.CursorTop = graphDistFromTop;
            for (int i = 0; i < (graphHight); i++)
            {
                if (i == 0)
                {
                    Console.CursorLeft = graphDistFromSide - 2;
                    Console.Write("20");
                }
                else if (i == 5)
                {
                    Console.CursorLeft = graphDistFromSide - 2;
                    Console.Write("15");
                }
                else if (i == 10)
                {
                    Console.CursorLeft = graphDistFromSide - 2;
                    Console.Write("10");
                }
                else if (i == graphHight - 5)
                {
                    Console.CursorLeft = graphDistFromSide - 2;
                    Console.Write("50");
                }
                else if (i == graphHight - 1)
                {
                    Console.CursorLeft = graphDistFromSide - 1;
                    Console.Write("0");
                }
                Console.CursorLeft = graphDistFromSide;
                Console.WriteLine("|");
            }
            Console.CursorLeft = graphDistFromSide;
            // orginally 40 (left point)
            for (int i = 0; i < GraphWidth; i++)
            {
                Console.Write("-");
            }
            Console.SetCursorPosition(0, 0);
            PrintDataset(n, x, y);
            //printing out the data x and y at the top of the screen (for refernce with graph)
            PrintAxis(ID);
            //checks to see if co-ordinate is a duplicate and changes it's colour based on how many times it is repeated
            CheckIfDataIsAduplicateAndPrint(ID, n, x, y);
            // prints a key of colours to show how many duplicates of a data point there is
            DataDupliacteKeyPrint();
            //prints the type of error that happened to the graph
            PrintErrorWithGraph(x, y);
        }
        static void PrintDataset(int n, double[] x, double[] y)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write("x=" + x[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < n; i++)
            {
                Console.Write("y=" + y[i]);
            }
        }
        static void CheckIfDataIsAduplicateAndPrint(int ID, int n, double[] x, double[] y)
        {
            List<string> DupeString = new List<string>();
            int dupeCount = 0;
            for (int i = 0; i < n; i++)
            {
                dupeCount = 0;
                DupeString.Add((x[i]).ToString() + (y[i] / 10).ToString());
                for (int j = 0; j < DupeString.Count; j++)
                {
                    if ((x[i]).ToString() + (y[i] / 10).ToString() == DupeString[j] && i != j)
                    {
                        dupeCount++;
                    }
                }
                // if the chosen graph type is 1 or 2 then the y value will be divided by 10
                if (ID == 1 || ID == 2)
                {
                    if (dupeCount == 0)
                    {
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i] / 10));
                        Console.Write("x");
                    }
                    if (dupeCount == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i] / 10));
                        Console.Write("x");
                    }
                    if (dupeCount == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i] / 10));
                        Console.Write("x");
                    }
                    if (dupeCount == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i] / 10));
                        Console.Write("x");
                    }
                    if (dupeCount == 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i] / 10));
                        Console.Write("x");
                    }
                    if (dupeCount > 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i] / 10));
                        Console.Write("x");
                    }
                }
                // if the chosen graph type is 3 4 or 5 then the y value will not be divied by 10
                else if (ID == 3 || ID == 4 || ID == 5)
                {
                    if (dupeCount == 0)
                    {
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i]));
                        Console.Write("x");
                    }
                    if (dupeCount == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i]));
                        Console.Write("x");
                    }
                    if (dupeCount == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i]));
                        Console.Write("x");
                    }
                    if (dupeCount == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i]));
                        Console.Write("x");
                    }
                    if (dupeCount == 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i]));
                        Console.Write("x");
                    }
                    if (dupeCount > 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(graphDistFromSide + (int)(x[i]), graphmidpointY - (int)(y[i]));
                        Console.Write("x");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        static void PrintErrorWithGraph(double[] x, double[] y)
        {
            double pmmcVal = Pmcc(x, y);
            double LeastSquareRegressionVal = LeastSquareRegression(x, y);
            if (x.Length == 0 || y.Length == 0)
            {
                Console.SetCursorPosition(0, 30);
                Console.WriteLine("the graph is empty because there is no data for this month yet");
                Console.WriteLine("press enter to continue");
                Console.ReadKey();
            }
            else
            {
                if (pmmcVal.ToString() == "NaN" || LeastSquareRegressionVal == -1)
                {
                    Console.SetCursorPosition(0, 30);
                    Console.WriteLine("error there is not enough data to show correlation between points or predict the future or the data is invalid");
                    Console.WriteLine("press enter to continue");
                    Console.ReadKey();
                }
                else
                {
                    Console.SetCursorPosition(70, 27);
                    Console.WriteLine("this graph has The Product Moment Correlation Coefficient");
                    Console.SetCursorPosition(70, 28);
                    Console.WriteLine("of " + Math.Round(pmmcVal, 2));
                    Console.SetCursorPosition(0, 27);
                    if (LeastSquareRegressionVal == -1)
                    {
                        Console.WriteLine("error Least Square Regression could not be calculated");
                    }
                }
            }
        }
        static void DataDupliacteKeyPrint()
        {
            Console.SetCursorPosition(100, 0);
            Console.WriteLine("x = no same data (white)");
            Console.SetCursorPosition(100, 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("x");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("= 1 duplicate of data (red)");
            Console.SetCursorPosition(100, 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("x");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("= 2 duplicate data (Yellow)");
            Console.SetCursorPosition(100, 3);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("x");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("= 3 duplicate data (Blue)");
            Console.SetCursorPosition(100, 4);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("x");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("= 4 duplicate data (Purple)");
            Console.SetCursorPosition(100, 5);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("x");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("= more than 4 duplicates of data (green)");
        }
        static void PrintAxis(int ID)
        {
            // prints the graphs axis differntly depedning on which data is being used
            Console.SetCursorPosition(0, 10);
            if (ID == 1 || ID == 2)
            {
                Console.WriteLine("Y axis");
                Console.WriteLine();
                Console.WriteLine("P|");
                Console.WriteLine(" |");
                Console.WriteLine("R|");
                Console.WriteLine(" |");
                Console.WriteLine("I|");
                Console.WriteLine(" |");
                Console.WriteLine("C|");
                Console.WriteLine(" |");
                Console.WriteLine("E|");
                Console.SetCursorPosition(20, 26);
                Console.WriteLine("X axis Date(day)");
                Console.SetCursorPosition(19, 27);
                Console.WriteLine("------------------");
            }
            else if (ID == 3 || ID == 4 || ID == 5)
            {
                Console.WriteLine("Y axis");
                Console.WriteLine();
                Console.WriteLine("T|");
                Console.WriteLine(" |");
                Console.WriteLine("O|");
                Console.WriteLine(" |");
                Console.WriteLine("T|");
                Console.WriteLine(" |");
                Console.WriteLine("A|");
                Console.WriteLine(" |");
                Console.WriteLine("L|");
                Console.SetCursorPosition(20, 26);
                Console.WriteLine("X axis Date(day)");
                Console.SetCursorPosition(19, 27);
                Console.WriteLine("------------------");
            }
        }
        static double Pmcc(double[] x, double[] y)
        {
            double n = x.Length;
            double xMean = 0;
            double yMean = 0;
            double xMinusXMean = 0;
            double xMinusXMeanTotal = 0;
            double xMinusXMeanTotalSquared = 0;
            double yMinusYMean = 0;
            double yMinusYMeanTotal = 0;
            double yMinusYMeanTotalSquared = 0;
            double numerator = 0;
            double denominator = 0;
            double pmccID = 0;
            for (int i = 0; i < n; i++)
            {
                xMean += x[i];
            }
            xMean /= n;
            for (int i = 0; i < n; i++)
            {
                yMean += y[i];
            }
            yMean = yMean / n;
            for (int i = 0; i < n; i++)
            {
                xMinusXMean = x[i] - xMean;
                xMinusXMeanTotal += xMinusXMean;
                xMinusXMeanTotalSquared += Math.Pow(xMinusXMean, 2);
                yMinusYMean = y[i] - yMean;
                yMinusYMeanTotal += yMinusYMean;
                yMinusYMeanTotalSquared += Math.Pow(yMinusYMean, 2);
                numerator += xMinusXMean * yMinusYMean;
            }
            denominator = xMinusXMeanTotalSquared * yMinusYMeanTotalSquared;
            pmccID = numerator / Math.Sqrt(denominator);
            return pmccID;
        }
        static double LeastSquareRegression(double[] x, double[] y)
        {
            double n = y.Length;
            double sumx = 0;
            double sumy = 0;
            double sumxy = 0;
            double c = 0;
            double yTarget = 0;
            double xPredict = 0;
            double sumXSquared = 0;
            double m = 0;
            for (int i = 0; i < n; i++)
            {
                sumxy += x[i] * y[i];
                sumx += x[i];
                sumy += y[i];
                sumXSquared += x[i] * x[i];
            }
            m = (n * sumxy - (sumx * sumy))
            / (n * sumXSquared - (sumx * sumx));
            c = (sumy - m * sumx) / n;
            if (!(m.ToString() == "NaN" || c.ToString() == "NaN"))
            {
                Console.SetCursorPosition(0, 30);
                Console.WriteLine("y = mx + c:");
                Console.WriteLine("y = " + Math.Round(m, 3) + " x + " + Math.Round(c, 3));
                bool error = true;
                while (error == true)
                {
                    error = false;
                    try
                    {
                        Console.WriteLine("whats your target y");
                        yTarget = double.Parse(Console.ReadLine());
                        error = false;
                    }
                    catch
                    {
                        Console.WriteLine("error must be a number exit");
                        Console.WriteLine();
                        error = true;
                    }
                }
                xPredict = (yTarget - c) / m;
                Console.WriteLine("you will reach your target when x =" + Math.Round(xPredict, 3) + " --> thats " + Math.Round((xPredict - x[x.Length - 1]), 3) + " days more than the latest x value (which is the date: " + (x[x.Length - 1] + "th)"));
                Console.WriteLine("press enter to exit to main menu");
                Console.ReadKey();
                return xPredict;
            }
            return -1;
        }
        static void CustomerTable(int ID)
        {
            //sends the user to differnt actions for the customer table
            switch (GetSecondMenuChoices())
            {
                case 1:
                    ViewTable(ID);
                    break;
                case 2:
                    InsertIntoTable(ID);
                    break;
                case 3:
                    EditTable(ID);
                    break;
                case 4:
                    AlterTable(ID);
                    break;
                case 9:
                    break;
            }
        }
        static void OrderTable(int ID)
        {
            //sends the user to differnt actions for the Order Table
            switch (GetSecondMenuChoices())
            {
                case 1:
                    ViewTable(ID);
                    break;
                case 2:
                    InsertIntoTable(ID);
                    break;
                case 3:
                    EditTable(ID);
                    break;
                case 4:
                    AlterTable(ID);
                    break;
                case 9:
                    break;
            }
        }
        static void OrderDetailsTable(int ID)
        {
            //sends the user to differnt actions for the OrderDetailsTable
            switch (GetSecondMenuChoices())
            {
                case 1:
                    ViewTable(ID);
                    break;
                case 2:
                    InsertIntoTable(ID);
                    break;
                case 3:
                    EditTable(ID);
                    break;
                case 4:
                    AlterTable(ID);
                    break;
                case 9:
                    break;
            }
        }
        static void ProductTable(int ID)
        {
            //sends the user to differnt actions for the ProductTable
            switch (GetSecondMenuChoices())
            {
                case 1:
                    ViewTable(ID);
                    break;
                case 2:
                    InsertIntoTable(ID);
                    break;
                case 3:
                    EditTable(ID);
                    break;
                case 4:
                    AlterTable(ID);
                    break;
                case 9:
                    break;
            }
        }
        static void FootballClubTable(int ID)
        {
            //sends the user to differnt actions for the FootballClubTable
            switch (GetSecondMenuChoices())
            {
                case 1:
                    ViewTable(ID);
                    break;
                case 2:
                    InsertIntoTable(ID);
                    break;
                case 3:
                    EditTable(ID);
                    break;
                case 4:
                    AlterTable(ID);
                    break;
                case 9:
                    break;
            }
        }
        static void StatsTable(int ID)
        {
            //sends the user to differnt actions for the StatsTable
            int choice = 0;
            bool error = true;
            while (error == true)
            {
                error = false;
                try
                {
                    Console.WriteLine("what action would you like to complete");
                    Console.WriteLine("1)view the stats table");
                    Console.WriteLine("2)add to stats table(will enter the stats from today's date)");
                    Console.WriteLine("9) exit");
                    choice = int.Parse(Console.ReadLine());
                    error = false;
                    while (choice != 1 && choice != 2 && choice != 9)
                    {
                        Console.WriteLine("please enter a valid choice (1,2 or 9 to exit) ");
                        choice = int.Parse(Console.ReadLine());
                        error = false;
                    }
                }
                catch
                {
                    Console.WriteLine("error must be a number (1,2 or 9 to exit)");
                    Console.WriteLine();
                    error = true;
                }
            }
            switch (choice)
            {
                case 1:
                    ViewTable(ID);
                    break;
                case 2:
                    InsertIntoTable(ID);
                    break;
                case 9:
                    break;
            }
        }
        static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=Football shirt database.db;Version=3;New=True;Compress=True;");
            // Open the connection:
            sqlite_conn.Open();
            return sqlite_conn;
        }
    }
}