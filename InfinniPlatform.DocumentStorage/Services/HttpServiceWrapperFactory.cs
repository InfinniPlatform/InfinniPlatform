using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Loader;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Предоставляет интерфейс для создания типизированных декораторов для экземпляров <see cref="IHttpService"/>.
    /// </summary>
    /// <remarks>
    /// В текущей реализации хостинг <see cref="IHttpService"/> выполнен на базе функционала NancyFx, который требует,
    /// чтобы каждый сервис имел уникальный CLR-тип. Тем не менее, на уровне прикладной разработки часто возникает
    /// необходимость создания однотипных сервисов. Вариант решения с автоматической генерацией кода сервисов был
    /// отклонен, так как это усложняет процесс прикладной разработки, неадекватно увеличивает кодовую базу, требует
    /// поддержки ее в актуальном состоянии. И все эти сложности только из-за текущей реализации механизма хостинга
    /// сервисов. Поэтому был рассмотрен вариант автоматической генерации типизированных декораторов для экземпляров
    /// <see cref="IHttpService"/>. Для каждого экземпляра <see cref="IHttpService"/> создается декоратор с уникальным
    /// CLR-типом, который скрывает обращение к реальному экземпляру. Это решение оставляет все неудобства на уровне
    /// реализации и предоставляет на прикладном уровне простую и удобную в использовании абстракцию. Генерация типов
    /// делается с использованием классов из пространства "System.Reflection.Emit", поскольку это самый быстрый и не
    /// требующий дополнительных установок способ генерации типов (по сравнению с CSharpCodeProvider). При написании
    /// кода был использован плагин "Reflector.ReflectionEmitLanguage".
    /// </remarks>
    internal sealed class HttpServiceWrapperFactory : IHttpServiceWrapperFactory
    {
        public HttpServiceWrapperFactory()
        {
            _httpServiceWrapperModule = DefineHttpServiceWrapperModule();
        }


        private readonly ModuleBuilder _httpServiceWrapperModule;


        /// <summary>
        /// Создать типизированный декоратор для экземпляра <see cref="IHttpService"/>.
        /// </summary>
        public IHttpService CreateServiceWrapper(string httpServiceWrapperTypeName, IHttpService httpService)
        {
            // public class HttpServiceWrapper : IHttpService
            // {
            //     private readonly IHttpService _httpService;
            //
            //     public HttpServiceWrapper(IHttpService httpService)
            //     {
            //         _httpService = httpService;
            //     }
            //
            //     public void Load(IHttpServiceBuilder builder)
            //     {
            //         _httpService.Load(builder);
            //     }
            // }

            var httpServiceWrapperType = DefineHttpServiceWrapperType(httpServiceWrapperTypeName);
            var httpServiceField = DefineHttpServiceField(httpServiceWrapperType);
            DefineHttpServiceWrapperConstructor(httpServiceWrapperType, httpServiceField);
            DefineHttpServiceLoadMethod(httpServiceWrapperType, httpServiceField);

            // return new HttpServiceWrapper(httpService);

            return (IHttpService)Activator.CreateInstance(httpServiceWrapperType.CreateTypeInfo().AsType(), httpService);
        }


        private static ModuleBuilder DefineHttpServiceWrapperModule()
        {
            //TODO Check if it's work.
            var assemblyName = AssemblyLoadContext.GetAssemblyName("HttpServiceWrapperTypes");
            var assembly = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule(assemblyName.Name);

            return module;
        }

        private TypeBuilder DefineHttpServiceWrapperType(string httpServiceWrapperTypeName)
        {
            // public class HttpServiceWrapper : IHttpService { ... }

            var type = _httpServiceWrapperModule.DefineType(
                httpServiceWrapperTypeName,
                TypeAttributes.Public,
                typeof(object),
                new[] { typeof(IHttpService) });

            return type;
        }

        private static FieldBuilder DefineHttpServiceField(TypeBuilder httpServiceWrapperType)
        {
            // private readonly IHttpService _httpService;

            var field = httpServiceWrapperType.DefineField(
                "_httpService",
                typeof(IHttpService),
                FieldAttributes.Private);

            return field;
        }

        private static void DefineHttpServiceWrapperConstructor(TypeBuilder httpServiceWrapperType, FieldBuilder httpServiceField)
        {
            // public HttpServiceWrapper(IHttpService httpService) { _httpService = httpService; }

            var baseConstructor = typeof(object).GetTypeInfo().GetConstructor(Type.EmptyTypes);

            var constructor = httpServiceWrapperType.DefineMethod(".ctor", MethodAttributes.Public | MethodAttributes.HideBySig);
            constructor.SetReturnType(typeof(void));
            constructor.SetParameters(typeof(IHttpService));
            constructor.DefineParameter(1, ParameterAttributes.None, "httpService");

            var constructorBody = constructor.GetILGenerator();
            constructorBody.Emit(OpCodes.Ldarg_0);
            constructorBody.Emit(OpCodes.Call, baseConstructor);
            constructorBody.Emit(OpCodes.Ldarg_0);
            constructorBody.Emit(OpCodes.Ldarg_1);
            constructorBody.Emit(OpCodes.Stfld, httpServiceField);
            constructorBody.Emit(OpCodes.Ret);
        }

        private static void DefineHttpServiceLoadMethod(TypeBuilder httpServiceWrapperType, FieldBuilder httpServiceField)
        {
            // public void Load(IHttpServiceBuilder builder) { _httpService.Load(builder); }

            var invokeMethod = typeof(IHttpService).GetTypeInfo().GetMethod("Load", new[] { typeof(IHttpServiceBuilder) });

            var method = httpServiceWrapperType.DefineMethod("Load", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot);
            method.SetReturnType(typeof(void));
            method.SetParameters(typeof(IHttpServiceBuilder));
            method.DefineParameter(1, ParameterAttributes.None, "builder");

            var methodBody = method.GetILGenerator();
            methodBody.Emit(OpCodes.Ldarg_0);
            methodBody.Emit(OpCodes.Ldfld, httpServiceField);
            methodBody.Emit(OpCodes.Ldarg_1);
            methodBody.Emit(OpCodes.Callvirt, invokeMethod);
            methodBody.Emit(OpCodes.Ret);
        }
    }
}