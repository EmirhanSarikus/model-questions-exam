
class Program {
     static public void Main(string[] args) {
        using (var context = new ExamContext()) {
            //Seed.ClearDB(context);
            Seed.SeedData(context); 
            var mainCategoryName = "Meat";
            
            Console.WriteLine($"Orders before removing a MainCategory ({mainCategoryName}): {context.Orders.Count()}");
            Console.WriteLine($"Categories before removing a MainCategory ({mainCategoryName}): {context.Categories.Count()}");
            Console.WriteLine($"FoodItems before removing a MainCategory ({mainCategoryName}): {context.FoodItems.Count()}");

            //Removing a MainCategory
            var categoryToBeRemoved = context.MainCategories.Where(_ => _.Name == mainCategoryName).FirstOrDefault();
            if (categoryToBeRemoved != null) {
                context.MainCategories.Remove(categoryToBeRemoved);
                Console.WriteLine($"Records changed: {context.SaveChanges()}");
                Console.WriteLine($"Orders after removing a MainCategory ({mainCategoryName}): {context.Orders.Count()}");
                Console.WriteLine($"Categories after removing a MainCategory ({mainCategoryName}): {context.Categories.Count()}");
                Console.WriteLine($"FoodItems after removing a MainCategory ({mainCategoryName}): {context.FoodItems.Count()}");
            }
            else {
                Console.WriteLine($"MainCategory {mainCategoryName} not found.");
            }  

            var employeeName = "John";
            Console.WriteLine("\n");
            Console.WriteLine($"Orders before removing an Employee ({employeeName}): {context.Orders.Count()}");
            Console.WriteLine($"Categories before removing an Employee ({employeeName}): {context.Categories.Count()}");
            Console.WriteLine($"FoodItems before removing an Employee ({employeeName}): {context.FoodItems.Count()}");

            //Removing an Employee
            var employeeToBeRemoved = context.Employees.Where(_ => _.Name == employeeName).FirstOrDefault();
            if (employeeToBeRemoved != null) {
                context.Employees.Remove(employeeToBeRemoved);
                Console.WriteLine($"Records changed: {context.SaveChanges()}");
                Console.WriteLine($"Orders after removing an Employee ({employeeName}): {context.Orders.Count()}");
                Console.WriteLine($"Categories after removing an Employee ({employeeName}): {context.Categories.Count()}");
                Console.WriteLine($"FoodItems after removing an Employee ({employeeName}): {context.FoodItems.Count()}");
            }
            else {
                Console.WriteLine($"Employee {employeeName} not found.");
            } 

        }
    }
}

class Seed
{   
    public static void ClearDB(ExamContext db) {
           string truncateTables = @"
                    TRUNCATE TABLE ""MainCategories"" CASCADE ;
                    TRUNCATE TABLE ""Categories"" CASCADE ;
                    TRUNCATE TABLE ""Customers"" CASCADE ;
                    TRUNCATE TABLE ""FoodItems"" CASCADE ;
                    TRUNCATE TABLE ""Orders"" CASCADE ;
                    TRUNCATE TABLE ""Employees"" CASCADE ;
                    ";
           db.Database.ExecuteSqlRaw(truncateTables);         
    }

    public static void SeedData(ExamContext db)
    {
        if (db is null)
        {
            Console.WriteLine("ExamContext is null"); return;
        }
        if (db.Orders.Count() != 0)
        {
            return;
            /*
            Console.WriteLine("Clearing DB...");
            ClearDB(db);*/
            
        }
        db.MainCategories.AddRange(GetMainCategories());
        db.Categories.AddRange(GetCategories());
        db.FoodItems.AddRange(GetFoodItems());
        db.Customers.AddRange(GetCustomers());
        db.Employees.AddRange(GetEmployees());
        db.Orders.AddRange(GetOrders());
        Console.WriteLine($"{db.SaveChanges()} rows effected");
    }

    private static List<Category> GetCategories()
    {
        List<Category> categories = new List<Category>(){
            new Category(){ID = 7,Name = "Milk Shake", CategoryID= 1 },
            new Category(){ID= 8, Name ="Hot Drinks",CategoryID = 1 },
            new Category(){ID = 9,Name = "Soft Drinks", CategoryID = 1 },
            new Category(){ID = 10, Name = "Sea Food Starter", CategoryID = 4 },
            new Category(){ID = 11, Name = "Vegetarian Starter",CategoryID =  2}, 
            new Category() {ID = 12, Name = "Meat Starter", CategoryID= 3 },
            new Category(){ID = 13, Name = "Vegetarian Main Course",CategoryID =  2}, 
            new Category() {ID = 14, Name = "Meat Main Course", CategoryID= 3 }
        };
        return categories;
    }
    
    private static List<MainCategory> GetMainCategories()
    {
        var mainCategories = new List<MainCategory>(){
                new MainCategory(){ID = 1, Name="Beverages" },
                new MainCategory(){ID= 2, Name = "Vegetarian" },
                new MainCategory(){ID = 3, Name = "Meat" },
                new MainCategory(){ID = 4, Name = "Fish" },
                new MainCategory(){ID = 5, Name = "Dessert" },};
        return mainCategories;
    }

    public static List<FoodItem> GetFoodItems()
    {
        List<FoodItem> foodItems = new List<FoodItem>(){
            new FoodItem(1, "Tea", 8, 2.50m, "250ml"),
            new FoodItem(2, "Espresso", 8, 3, "30ml"),
            new FoodItem(3, "Cappuccino", 8, 3.45m, "100ml"),
            new FoodItem(4, "Chinotto", 9, 3.50m, "300ml"),
            new FoodItem(5, "Water", 9, 1.65m ,"1Liter"),
            new FoodItem(6, "Fritto misto", 10, 9.50m, "350g"),
            new FoodItem(7, "Samosa", 11, 5.50m, "100g"),
            new FoodItem(8, "Corn Kebab", 12, 6.50m, "100g"),
            new FoodItem(9, "Chicken Tikka", 14, 10.50m, "300g"),
            new FoodItem(10, "Coniglio in agrodolce", 14, 15.55m,"250g"),
            new FoodItem(11, "Spinach", 13, 12m),
            new FoodItem(12, "Aubergine", 13, 9.50m),
            new FoodItem(13, "Broccoli", 11, 8m),
            new FoodItem(14, "Melanzane alla parmigiana", 13, 12.55m),
            new FoodItem(15, "Meatballs", 14, 12m),
            new FoodItem(16, "Chicken Fried Rice",14, 13.50m),
            new FoodItem(17, "Vitello Tonnato", 14, 20m),
            new FoodItem(18, "Mango Lassi", 7, 2m),
        };

        return foodItems;
    }
    
    public static List<Employee> GetEmployees()
    {  
            return new List<Employee>(){
                    new Employee(1, "John"),
                    new Employee(2, "Laura")
                    };
      
    }

    public static List<Customer> GetCustomers()
    {
        //return File.ReadAllLines(Environment.ProcessPath.Split("bin")[0]+"Customers.csv") //if using VisualStudio use this line
        return File.ReadAllLines("./Customers.csv")  // for VS code, comment it when using VisualStudio      
                   .Skip(1) //Header
                   .Where(_ => _.Length > 0)
                   .Select(_ => Customer(_)).ToList();
    }

    private static Customer Customer(string row)
    {
        string format = "dd/MM/yyyy HH:mm";
        CultureInfo provider = CultureInfo.InvariantCulture;
        
        var columns = row.Split(',');
        return new Customer()
        {
            ID = int.Parse(columns[0]),
            Name = columns[1],
            DateTime = DateTime.SpecifyKind(DateTime.ParseExact(columns[2], format, provider), DateTimeKind.Utc),
            TableNumber = int.Parse(columns[3])
        };
    }

    public static List<Order> GetOrders()
    {
        List<Order> orders = new List<Order>();
        Random rand = new Random();
        var employees = GetEmployees();
        var foodItems = GetFoodItems();
        int totalFoodItems = foodItems == null? 0 : GetFoodItems().Count;
        
        Order newOrder;
        int maxJ = 0;
        for (int i = 1; i <= 100; i++) {
            maxJ = 3; //rand.Next(1,4);
            for (int j = 0; j < maxJ; j++)
            {
                newOrder = new Order()
                {
                    CustomerID = i,
                    FoodItemID = foodItems[i % totalFoodItems].ID,
                    Quantity = rand.Next(1, 3),
                    EmployeeID = employees[i > 49 ? 0 : 1].ID,
                };
                if(orders.Count(_ => _.CustomerID == newOrder.CustomerID && _.FoodItemID == newOrder.FoodItemID)==0)
                    orders.Add(newOrder);
            }
        }
        return orders;
    }  
}

