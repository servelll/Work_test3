using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Work_test3
{
    class Program
    {
        public class Country_row
        {
            public int user_id;
            public int count;
            public String country;
        };
        public class Country_stat
        {
            public String country;
            public int sum_count;

            public List<Country_row> uniq_user_id_rows;

            public Country_stat(Country_row inp)
            {
                country = inp.country;
                sum_count = 0;
                uniq_user_id_rows = new List<Country_row>();
                Add(inp);
            }

            public void Add(Country_row what)
            {
                if (what.country == country)
                {
                    if (!uniq_user_id_rows.Any(c => c.user_id == what.user_id))
                    {
                        uniq_user_id_rows.Add(what);
                    }
                    sum_count += what.count;
                }
            }
        }
        static void Main(string[] args)
        {
            //сразу будем собирать статистику
            List<Country_stat> stat = new List<Country_stat>();

            using (TextReader reader = new StreamReader("input.txt", Encoding.Default))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    //помучаемся, используя только базовые типы (изобретаем велосипед)

                    String[] delims = str.Split(';');
                    if (delims.Count() != 3)
                    {
                        Console.WriteLine("Строка \"" + str + "\" не содержит нужное количество данных");
                        continue;
                    }
                    else
                    {
                        //дробим дальше
                        Country_row temp_row = new Country_row();
                        var props = typeof(Country_row).GetFields();

                        //проверяем все поля,
                        bool fields_ok = true;
                        for (int i = 0; i < 3; i++)
                        {
                            if (i == 0 || i == 1)
                            {
                                if (Int32.TryParse(delims[i], out int outt))
                                {
                                    typeof(Country_row).GetField(props[i].Name).SetValue(temp_row, outt);
                                }
                                else
                                {
                                    fields_ok = false;
                                    Console.WriteLine("Поле \"" + delims[i] + "\" в строке \"" + str + "" + "\" нельзя привести к числу");
                                    break;
                                }
                            }
                            else
                            {
                                //Здесь, по хорошему, нужно подключить библиотеку для распознавания стран
                                temp_row.country = delims[i];
                            }
                        }
                        //и, если эти поля нормальные, учитываем их
                        if (fields_ok)
                        {
                            //stringBuilder, конечно, не базовый, но здесь правильно будет его использовать, чтобы собрать строку, а не перезаписывать ее
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in props)
                            {
                                sb.Append(String.Join(" ", new String[] { item.Name, typeof(Country_row).GetField(item.Name).GetValue(temp_row).ToString() }) + "; ");
                            }
                            Console.WriteLine(sb.ToString());

                            //добавляем к статистике
                            var finded = stat.Find(c => (c.country == temp_row.country));
                            if (finded != null)
                            {
                                finded.Add(temp_row);
                            }
                            else //если нет страны в статистике, то добавляем
                            {
                                stat.Add(new Country_stat(temp_row));
                            }
                        }
                    }
                }
            }

            //печать статистики
            StringBuilder sb2 = new StringBuilder();
            foreach (var item in stat)
            {
                sb2.Append(String.Join(" ", new String[] { item.country, item.sum_count.ToString(), item.uniq_user_id_rows.Count().ToString() }) + "\n");
            }
            Console.WriteLine(sb2.ToString());

            Console.ReadKey();
        }
    }
}
