using EmitMapper;
using Raven.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.ModelMapper.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Sourse src = new Sourse
            {
                A = 1,
                B = 10M,
                C = DateTime.Parse("2011/9/21 0:00:00"),
                //D = new Inner
                //{
                //    D2 = Guid.NewGuid()
                //},
                E = "test"
            };
            Dest dst = null;
            //dst = mapper.Map(src);

            //Console.WriteLine(dst.A);
            //Console.WriteLine(dst.B);
            //Console.WriteLine(dst.C);
            ////Console.WriteLine(dst.D.D1);
            ////Console.WriteLine(dst.D.D2);
            //Console.WriteLine(dst.F);

            Stopwatch sw = new Stopwatch();
            int seed = 500000;

            ObjectsMapper<Sourse, Dest> mapper = ObjectMapperManager.DefaultInstance.GetMapper<Sourse, Dest>();
            var mapperImpl = mapper.MapperImpl;
            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<Sourse, Dest>();
                //mapper = new ObjectsMapper<Sourse, Dest>(mapperImpl);
                dst = mapper.Map(src);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            dst = ModelConverter.ConvertObject<Sourse, Dest>(src);

            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                dst = ModelConverter.ConvertObject<Sourse, Dest>(src);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            sw.Restart();
            for (var i = 0; i < seed; i++)
            {
                dst = ConvertModel<Sourse, Dest>(src);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }


        #region 数据对象转换（待优化）
        /// <summary>
        /// 创建数据对象（待优化）
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        [Obsolete]
        public static TOut ConvertModel<TIn, TOut>(TIn data)
            where TIn : class, new()
            where TOut : class, new()
        {
            TOut result = new TOut();

            if (data == null)
                return null;
            Type tin = typeof(TIn);
            Type tout = typeof(TOut);

            var propertiesIn = tin.GetProperties();
            var propertiesOutDict = tout.GetProperties().ToDictionary(x => x.Name);

            foreach (var p in propertiesIn)
            {
                string name = p.Name;
                if (propertiesOutDict.ContainsKey(name))
                {
                    object obj = p.GetValue(data);
                    //propertiesOutDict[name].SetValue(result, obj);
                    if (p.PropertyType.IsClass && p.PropertyType != typeof(string))
                    {

                    }
                    else
                    {
                        propertiesOutDict[name].SetValue(result, obj);
                    }
                }
            }

            return result;
        }
        #endregion
    }

    public class Sourse
    {
        public int A { get; set; }
        public decimal? B { get; set; }
        public DateTime C { get; set; }
        //public Inner D;
        public string E { get; set; }
    }

    public class Dest
    {
        public int? A { get; set; }
        public decimal B { get; set; }
        public DateTime C { get; set; }
        //public Inner D;
        public string F { get; set; }
    }

    public class Inner
    {
        public long D1;
        public Guid D2;
    }
}
