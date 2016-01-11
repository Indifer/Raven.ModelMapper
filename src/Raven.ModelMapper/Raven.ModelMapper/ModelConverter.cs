using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Util
{
    public static class ModelConverter
    {
        private static Dictionary<string, ObjectsMapperBaseImpl> _dictMapping = new Dictionary<string, ObjectsMapperBaseImpl>();

        //private ModelConverter()
        //{
        //}

        /// <summary>
        /// object转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static TOut ConvertObject<TIn, TOut>(TIn inData)
        {
            Type tin = typeof(TIn);
            Type tout = typeof(TOut);

            string typeKey = string.Concat(tin, "|", tout);
            ObjectsMapperBaseImpl mapperBase = null;
            ObjectsMapper<TIn, TOut> mapper = null;
            if (_dictMapping.TryGetValue(typeKey, out mapperBase))
            {
                mapper = new ObjectsMapper<TIn, TOut>(mapperBase);
            }
            else
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TIn, TOut>();
                _dictMapping[typeKey] = mapper.MapperImpl;
            }

            return mapper.Map(inData);
        }

        /// <summary>
        /// object转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static void Register<TIn, TOut>(DefaultMapConfig config = null)
        {
            Register<TIn, TOut>(config);
        }

        /// <summary>
        /// object转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static void Register<TIn, TOut>(IMappingConfigurator config = null)
        {
            Type tin = typeof(TIn);
            Type tout = typeof(TOut);
            string typeKey = string.Concat(tin, "|", tout);
            ObjectsMapper<TIn, TOut> mapper = null;
            if (config == null)
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TIn, TOut>();
            }
            else
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TIn, TOut>(config);
            }
            _dictMapping[typeKey] = mapper.MapperImpl;
        }
        
    }
}
