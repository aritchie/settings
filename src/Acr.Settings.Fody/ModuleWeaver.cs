using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;


namespace Acr.Settings.Fody
{
    public class ModuleWeaver
    {
        // Will log an informational message to MSBuild
        public Action<string> LogInfo { get; set; }

        // An instance of Mono.Cecil.ModuleDefinition for processing
        public ModuleDefinition ModuleDefinition { get; set; }

        TypeSystem typeSystem;

        // Init logging delegates to make testing easier
        public ModuleWeaver()
        {
            LogInfo = m => { };
        }

        public void Execute()
        {
            //typeSystem = ModuleDefinition.TypeSystem;
            //var newType = new TypeDefinition(null, "Hello", TypeAttributes.Public, typeSystem.Object);

            //AddConstructor(newType);

            //AddHelloWorld(newType);

            //ModuleDefinition.Types.Add(newType);
            //LogInfo("Added type 'Hello' with method 'World'.");
        }

        //void AddConstructor(TypeDefinition newType)
        //{
        //    var method = new MethodDefinition(".ctor", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, typeSystem.Void);
        //    var objectConstructor = ModuleDefinition.Import(typeSystem.Object.Resolve().GetConstructors().First());
        //    var processor = method.Body.GetILProcessor();
        //    processor.Emit(OpCodes.Ldarg_0);
        //    processor.Emit(OpCodes.Call, objectConstructor);
        //    processor.Emit(OpCodes.Ret);
        //    newType.Methods.Add(method);
        //}

        //void AddHelloWorld(TypeDefinition newType)
        //{
        //    var method = new MethodDefinition("World", MethodAttributes.Public, typeSystem.String);
        //    var processor = method.Body.GetILProcessor();
        //    processor.Emit(OpCodes.Ldstr, "Hello World");
        //    processor.Emit(OpCodes.Ret);
        //    newType.Methods.Add(method);
        //}
    }
}
