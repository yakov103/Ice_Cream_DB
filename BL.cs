using System.Collections;
using System.Globalization;
using BusinessEntities;

namespace BusinessLogic
{
    public class RandomDateTime
    {
        DateTime start;
        Random gen;
        int range;

        public RandomDateTime()
        {
            start = new DateTime(1995, 1, 1);
            gen = new Random();
            range = (DateTime.Today - start).Days;
        }

        public DateTime Next()
        {
            return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
        }
    }


    public class Logic
    {

        static string[] colors = { "Yellow", "White", "Black", "Green", "Transparent" };
        static string[] tasks = { "Service 10K", "Wheels", "BodyWork" };
        static string[] desc = { "Replace filets of oil,gasoline,air contidioner", "Change 4 tires", "fix scratchs" };
        static string[] cities = { "Jerusalem", "Tel Aviv", "Beeh Sheva", "Ariel" };

        // tastes = names
        static string[] tastes = { "Vanil", "Chocolate", "Mekupelet", "Pistachio", "Cherry", "Halva", "Mango", "Mint", "Rum", "Strawberry", "Teaberry", "Tutti-Frutti", "Twist", "Watermelon", "Banana" };
        // receptacles = cars
        static string[] receptacles_name = { "Regular_cone", "Special_cone", "Box" };
        static int[] receptacles_price = { 0, 2, 5 };
        // toppings 
        static string[] toppings = { "maple_syrup", "chocolate_syrup", "color_sparkles", "black_sparkls", "tchina", "cherry_syrup" };
        static string[] toppings_for_vanil = { "chocolate_syrup", "color_sparkles", "black_sparkls", "tchina", "cherry_syrup" };
        static string[] toppings_for_chocolate_mekupelet = { "maple_syrup", "color_sparkles", "black_sparkls", "tchina", "cherry_syrup" };
        static string[] toppings_for_chocolate_mekupelet_vanila = { "color_sparkles", "black_sparkls", "tchina", "cherry_syrup" };



        public static void createTables()
        {
            MySqlAccess.MySqlAccess.createTables();
        }
        public static void fillTables(int num)
        {
            Random r = new Random();

            //generate values for Tastes
            for (int i = 0; i < tastes.Length; i++)
            {
                Taste o = new Taste(tastes[i]);
                MySqlAccess.MySqlAccess.insertObject(o);
            }

            //generate values for receptacles
            for (int i = 0; i < receptacles_name.Length; i++)
            {
                Receptacle o = new Receptacle(receptacles_name[i], receptacles_price[i]);
                MySqlAccess.MySqlAccess.insertObject(o);
            }

            //generate values for Toppings
            for (int i = 0; i < toppings.Length; i++)
            {
                Topping o = new Topping(toppings[i]);
                MySqlAccess.MySqlAccess.insertObject(o);
            }


            List<string> chosen_tastes = new List<string>();
            HashSet<string> chosen_toppings = new HashSet<string>();
            RandomDateTime date = new RandomDateTime();
            date.Next();
            Random gen = new Random();
            Random gen2 = new Random();
            //generate random values for Tastes_Sales Topping_Sales Sales
            for (int i = 0; i < num; i++)
            { // 86 85

                int rReceptacle = r.Next(0, receptacles_name.Length);
                date.Next();
                bool completed = true;
                bool paid = true;
                int prob = gen.Next(100);
                int prob2 = gen2.Next(100);
                if (prob <= 20)
                {
                    completed = false;
                }
                if (prob2 <= 20)
                {
                    paid = false;
                }

                int ball_price = 0;
                int balls_amount = 0;

                if (rReceptacle == 0)
                {
                    // if its a regular cone then max balls will be 3
                    balls_amount = r.Next(0, 3);
                }
                else if (rReceptacle == 1)
                {
                    // if its a special cone then max balls will be 4 
                    balls_amount = r.Next(0, 4);
                }
                else if (rReceptacle == 2)
                {
                    // if its a Box then max balls will be 7 
                    balls_amount = r.Next(0, 7);
                }

                // now choose tastes
                for (int x = 0; x < balls_amount; x++)
                {
                    int rTaste = r.Next(0, tastes.Length);
                    chosen_tastes.Add(tastes[rTaste]);
                }

                // now choose toppings if its Box or special cone or regular cone with 2 or more balls
                if (rReceptacle == 1 || rReceptacle == 2 || rReceptacle == 0 && balls_amount > 1)
                {
                    // only 1 topping on a cone or it will break :)
                    if (rReceptacle == 1 || rReceptacle == 0)
                    {
                        if ((chosen_tastes.Contains("Chocolate") || chosen_tastes.Contains("Mekupelet")) && chosen_tastes.Contains("Vanil"))
                        {
                            int rTopping = r.Next(0, toppings_for_chocolate_mekupelet_vanila.Length);
                            chosen_toppings.Add(toppings[rTopping]);
                        }
                        else if (chosen_tastes.Contains("Chocolate") || chosen_tastes.Contains("Mekupelet"))
                        {
                            int rTopping = r.Next(0, toppings_for_chocolate_mekupelet.Length);
                            chosen_toppings.Add(toppings[rTopping]);
                        }
                        else if (chosen_tastes.Contains("Vanil"))
                        {
                            int rTopping = r.Next(0, toppings_for_vanil.Length);
                            chosen_toppings.Add(toppings[rTopping]);
                        }
                        else
                        {
                            int rTopping = r.Next(0, toppings.Length);
                            chosen_toppings.Add(toppings[rTopping]);
                        }
                    }
                    else
                    {
                        // choose max 3 toppings for Box

                        if ((chosen_tastes.Contains("Chocolate") || chosen_tastes.Contains("Mekupelet")) && chosen_tastes.Contains("Vanil"))
                        {
                            for (int three_tastes = 0; three_tastes < 3; three_tastes++)
                            {
                                int rTopping = r.Next(0, toppings_for_chocolate_mekupelet_vanila.Length);
                                chosen_toppings.Add(toppings[rTopping]);
                            }
                        }
                        else if (chosen_tastes.Contains("Chocolate") || chosen_tastes.Contains("Mekupelet"))
                        {
                            for (int three_tastes = 0; three_tastes < 3; three_tastes++)
                            {
                                int rTopping = r.Next(0, toppings_for_chocolate_mekupelet.Length);
                                chosen_toppings.Add(toppings[rTopping]);
                            }
                        }
                        else if (chosen_tastes.Contains("Vanil"))
                        {
                            for (int three_tastes = 0; three_tastes < 3; three_tastes++)
                            {
                                int rTopping = r.Next(0, toppings_for_vanil.Length);
                                chosen_toppings.Add(toppings[rTopping]);
                            }
                        }
                        else
                        {
                            for (int three_tastes = 0; three_tastes < 3; three_tastes++)
                            {
                                int rTopping = r.Next(0, toppings.Length);
                                chosen_toppings.Add(toppings[rTopping]);
                            }
                        }
                    }
                }

                // now handle the ball priceing 
                if (balls_amount == 1)
                {
                    ball_price = 7;
                }
                else if (balls_amount == 2)
                {
                    ball_price = 12;
                }
                else if (balls_amount > 3)
                {
                    ball_price = balls_amount * 6;
                }
                int total_price = ball_price + receptacles_price[rReceptacle];
                Sale s = new Sale(rReceptacle + 1, date.Next(), completed, paid, total_price);
                MySqlAccess.MySqlAccess.insertObject(s);
                // https://stackoverflow.com/questions/15862191/counting-the-number-of-times-a-value-appears-in-an-array
                // insert taste_sales
                // make hashmap for tastes quantity
                Dictionary<string, int> taste_quantity = new Dictionary<string, int>();
                foreach (string taste in chosen_tastes)
                {
                    if (taste_quantity.ContainsKey(taste))
                    {
                        taste_quantity[taste]++;
                    }
                    else
                    {
                        taste_quantity.Add(taste, 1);
                    }
                }
                // now insert to taste_sales
                foreach (KeyValuePair<string, int> entry in taste_quantity)
                {
                    // get the index of the taste
                    int taste_index = Array.IndexOf(tastes, entry.Key);
                    Taste_Sale ts = new Taste_Sale(i + 1, taste_index + 1, entry.Value);
                    MySqlAccess.MySqlAccess.insertObject(ts);
                }
                // insert topping_sales
                // now insert to topping_sales
                foreach (string topping in chosen_toppings)
                {
                    // get the index of the topping
                    int topping_index = Array.IndexOf(toppings, topping);
                    Topping_Sale ts = new Topping_Sale(i + 1, topping_index + 1);
                    MySqlAccess.MySqlAccess.insertObject(ts);
                }

            } // 244
        }

        public static ArrayList getTableData(string tableName)
        {
            ArrayList all = MySqlAccess.MySqlAccess.readAll(tableName);
            ArrayList results = new ArrayList();

            if (tableName == "Tastes")
            {
                foreach (Object[] row in all)
                {
                    Taste o = new Taste((string)row[1]);
                    o.setID((int)row[0]);
                    results.Add(o);
                }
            }

            if (tableName == "Toppings")
            {
                foreach (Object[] row in all)
                {
                    Topping o = new Topping((string)row[1]);
                    o.setID((int)row[0]);
                    results.Add(o);
                }
            }

            if (tableName == "Receptacles")
            {
                foreach (Object[] row in all)
                {
                    Receptacle o = new Receptacle((string)row[1], (int)row[2]);
                    o.setID((int)row[0]);
                    results.Add(o);
                }
            }

            if (tableName == "Sales")
            {
                foreach (Object[] row in all)
                {
                    // format the DateTime to be in the right format
                    // convert the row[2][0] to string
                    // string date = row[2][0].ToString();
                    // string sDate = (string)row[2]);
                    //DateTime date = DateTime.ParseExact("sDate", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    Sale o = new Sale((int)row[1], (DateTime)row[2], (bool)row[3], (bool)row[4], (int)row[5]);
                    o.setID((int)row[0]);
                    results.Add(o);
                }
            }

            if (tableName == "Tastes_Sales")
            {
                foreach (Object[] row in all)
                {
                    Taste_Sale o = new Taste_Sale((int)row[0], (int)row[1], (int)row[2]);
                    results.Add(o);
                }
            }

            if (tableName == "Toppings_Sales")
            {
                foreach (Object[] row in all)
                {
                    Topping_Sale o = new Topping_Sale((int)row[0], (int)row[1]);
                    results.Add(o);
                }
            }

            return results;
        }

    }
}